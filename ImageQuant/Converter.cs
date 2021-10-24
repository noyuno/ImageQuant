using ImageQuant.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace ImageQuant
{
    public class Converter
    {
        public readonly string[] ImageFormatList = new string[] { "JPEG", "GIF", "PNG", "BMP", "TIFF" };

        public string TempDir;
        public string DestDir;

        public string DestDirChild
        {
            get
            {
                if (Settings.Default.CreateChildDirectory)
                    return Path.Combine(DestDir, Settings.Default.ChildDirectory);
                else
                    return DestDir;
            }
        }

        //public string DestDirName = "ImageQuant";
        //public ImageFormat DestFormat;
        //public bool ChangeJpgQuality = true;
        //public long JpgQuality = 90;
        //public bool PngQuant = true;
        public ImageCodecInfo JpgImageCodecInfo;
        public ImageCodecInfo PngImageCodecInfo;

        public Converter()
        {
            TempDir = Path.GetTempFileName();
            File.Delete(TempDir);
            Directory.CreateDirectory(TempDir);
            DestDir = "";

            JpgImageCodecInfo = QImaging.GetEncoderInfo("image/jpeg");
            PngImageCodecInfo = QImaging.GetEncoderInfo("image/png");
        }

        public void Dispose()
        {
            if (Directory.Exists(TempDir))
            {
                Directory.Delete(TempDir);
            }
        }

        public ConverterResult Convert(string sourceFilename)
        {
            var sourceImage = Image.FromFile(sourceFilename, true);
            var sourceFileInfo = new FileInfo(sourceFilename);
            var destFormat = Settings.Default.ChangeFormat ? QImaging.GetImageFormat(Settings.Default.Format) : QImaging.GetImageFormat(sourceFilename);
            var destFilename = GetDestFilename(sourceFilename);
            if (!Directory.Exists(Path.GetDirectoryName(destFilename)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destFilename));
            }
            if (Settings.Default.OverwriteConfirm && File.Exists(destFilename))
            {
                if (MessageBox.Show("ファイルが存在しますが、上書きしますか？\r\n" + destFilename,
                    "ImageQuant", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return new ConverterResult(false, "ユーザにより取り消されました。", sourceFilename, destFilename, sourceImage.Width, sourceImage.Height, 0, 0, sourceFileInfo, null, sourceImage.RawFormat, 0, 0, null);
                }
            }
            Image destImage;
            if (Settings.Default.Resize)
            {
                destImage = QImaging.Resize(sourceImage, Settings.Default.MaximumSize);
            }
            else
            {
                destImage = sourceImage;
            }

            long destDepth = Image.GetPixelFormatSize(sourceImage.PixelFormat);
            if (Settings.Default.ChangeColorDepth)
                destDepth = Settings.Default.ColorDepth < destDepth ? Settings.Default.ColorDepth : destDepth;

            if (destFormat == ImageFormat.Jpeg)
            {
                SaveJpg(destImage, destFilename, destDepth);
            }
            else if (destFormat == ImageFormat.Png)
            {
                SavePng(destImage, destFilename, destDepth);
            }
            var destThumbnail = QImaging.GetThumbnail(destFilename);
            var destFileInfo = new FileInfo(destFilename);
            return new ConverterResult(true, "正常終了", sourceFilename, destFilename, sourceImage.Width, sourceImage.Height, destImage.Width, destImage.Height, sourceFileInfo, destFileInfo, destFormat, 0, destDepth, destThumbnail);
        }

        private void SavePng(Image image, string destfilename, long depth)
        {
            var pngEncoderParameters = new EncoderParameters(1);
            pngEncoderParameters.Param[0] = new EncoderParameter(Encoder.ColorDepth, depth);
            image.Save(destfilename, PngImageCodecInfo, pngEncoderParameters);
            image.Save(destfilename, ImageFormat.Png);
            if (Settings.Default.ChangePngQuality)
            {
                Assembly myAssembly = Assembly.GetEntryAssembly();
                string imageQuantPath = myAssembly.Location;
                string pngquantPath = Path.GetDirectoryName(imageQuantPath) + @"\pngquant.exe";
                if (File.Exists(pngquantPath))
                {
                    var quantProcess = new Process();
                    quantProcess.StartInfo.FileName = pngquantPath;
                    quantProcess.StartInfo.Arguments =
                        "-f --quality -" + Settings.Default.PngQuality.ToString() + " " + destfilename;
                    quantProcess.StartInfo.CreateNoWindow = true;
                    quantProcess.StartInfo.UseShellExecute = false;
                    quantProcess.Start();
                    quantProcess.WaitForExit();
                    if (quantProcess.ExitCode != 0)
                    {
                        throw new Exception("pngquant returned " + quantProcess.ExitCode.ToString());
                    }
                }
            }
        }

        private void SaveJpg(Image image, string destfilename, long depth)
        {
            var quality = Settings.Default.ChangeJpgQuality ? Settings.Default.JpgQuality : 90L;
            var jpgEncoderParameters = new EncoderParameters(2);
            jpgEncoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
            jpgEncoderParameters.Param[1] = new EncoderParameter(Encoder.ColorDepth, depth);
            image.Save(destfilename, JpgImageCodecInfo, jpgEncoderParameters);
        }

        private string GetDestFilename(string target)
        {
            string dir, fname;
            dir = Settings.Default.SaveManualPath ? Settings.Default.SavePath : Path.GetDirectoryName(target);
            if (dir == string.Empty)
            {
                dir = DestDir == "" ? TempDir : DestDir;
            }
            dir = Settings.Default.CreateChildDirectory ?
                Path.Combine(dir, Settings.Default.ChildDirectory) :
                dir;
            fname = Settings.Default.Prefix ? Settings.Default.PrefixName : "";
            fname += Path.GetFileNameWithoutExtension(target);
            fname = Settings.Default.Suffix ? fname + Settings.Default.SuffixName : fname;
            fname += Path.GetExtension(target);
            fname = Settings.Default.ChangeFormat ?
                Path.ChangeExtension(fname, QImaging.GetDestExt(QImaging.GetImageFormat(Settings.Default.Format))) :
                fname;
            return Path.Combine(dir, fname);
        }

        public string SaveBmp(Bitmap bmp)
        {
            string destfilename = GetDestFilename(@"\bitmap_" + (new DateTime()).ToString("yyyyMMdd-HHmmss") + QImaging.GetDestExt(ImageFormat.Bmp));
            bmp.Save(destfilename);
            return destfilename;
        }


    }
}
