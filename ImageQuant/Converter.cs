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
using SWF = System.Windows.Forms;

namespace ImageQuant
{
    public class Converter
    {
        //public readonly string[] ImageFormatList = new string[] { "JPEG", "GIF", "PNG", "BMP", "TIFF" };
        public readonly string[] ImageFormatList = new string[] { "JPEG", "GIF", "PNG" };


        public string TempDir;
        public string DestDir;

        public string DestDirChild
        {
            get
            {
                var d = DestDir == "" ? TempDir : DestDir;
                if (Settings.Default.CreateChildDirectory)
                    return Path.Combine(d, Settings.Default.ChildDirectory);
                else
                    return d;
            }
        }

        public string DestDirChildE
        {
            get
            {
                return Directory.Exists(DestDirChild) ? DestDirChild : Path.GetDirectoryName(DestDirChild);
            }
        }

        public ImageCodecInfo JpgImageCodecInfo;
        public ImageCodecInfo PngImageCodecInfo;
        public ImageCodecInfo GifImageCodecInfo;

        public Converter()
        {
            TempDir = $"{Path.GetTempPath()}\\{SWF.Application.ProductName}\\{Guid.NewGuid().ToString("N").Substring(0, 12)}";
            Directory.CreateDirectory(TempDir);
            DestDir = "";

            JpgImageCodecInfo = QImaging.GetEncoderInfo("image/jpeg");
            PngImageCodecInfo = QImaging.GetEncoderInfo("image/png");
            GifImageCodecInfo = QImaging.GetEncoderInfo("image/gif");
        }

        public void Dispose()
        {
            if (Directory.Exists(TempDir))
            {
                Directory.Delete(TempDir, true);
            }
        }

        public ConverterResult Convert(string sourceFilename)
        {
            var sourceFileInfo = new FileInfo(sourceFilename);
            var sourceFileType = QImaging.GetFileType(sourceFilename);
            if(sourceFileType==QFileType.Unknown)
            {
                return new ConverterResult(false, "このファイルは画像ファイルではないようです。", sourceFilename, "", 0, 0, 0, 0, sourceFileInfo, null, QFileType.Unknown, 0, 0);
            }
            else if (sourceFileType == QFileType.Pdf)
            {
                return ConvertFromPDF(sourceFilename, sourceFileInfo);
            }
            else
            {
                return ConvertImage(sourceFilename, sourceFileInfo);
            }
        }

        private ConverterResult ConvertFromPDF(string sourceFilename, FileInfo sourceFileInfo)
        {
            MakeDest(sourceFilename, out QFileType destFormat, out string destFilename);
            if (OverwriteConfirm(destFilename) == false)
                return new ConverterResult(false, "ユーザにより取り消されました。", sourceFilename, destFilename, 0, 0, 0, 0, sourceFileInfo, null, QFileType.Unknown, 0, 0);
            Assembly myAssembly = Assembly.GetEntryAssembly();
            string imageQuantPath = myAssembly.Location;
            string gspath = Path.GetDirectoryName(imageQuantPath) + @"\gswin64c.exe";
            if (File.Exists(gspath))
            {
                using var proc = new Process();
                proc.StartInfo.FileName = gspath;
                proc.StartInfo.Arguments = $"-sDEVICE={Settings.Default.GSProfile} -r{Settings.Default.GSDpi} -dGraphicsAlphaBits=4 -o \"{destFilename}\" \"{sourceFilename}";
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();
                proc.WaitForExit();
                //Debug.WriteLine(proc.StandardOutput.ReadToEnd(),"stdout");
                //Debug.WriteLine(proc.StandardOutput.ReadToEnd(),"stderr");
                if (proc.ExitCode != 0)
                {
                    throw new RuntimeException("gswin64c.exe exited with code non-zero", gspath + " " + proc.StartInfo.Arguments, proc.ExitCode, proc.StandardOutput.ReadToEnd(), proc.StandardError.ReadToEnd());
                }
            }
            else
            {
                throw new RuntimeException("gswin64c.exeが見つかりません。", gspath, -1, "", "");
            }
            var destFileInfo = new FileInfo(destFilename);
            var destImage = Image.FromFile(destFilename);
            var ret= new ConverterResult(true, "正常終了", sourceFilename, destFilename, 0, 0, destImage.Width, destImage.Height, sourceFileInfo, destFileInfo, destFormat, 0, 24);
            destImage.Dispose();
            return ret;
        }

        private ConverterResult ConvertImage(string sourceFilename, FileInfo sourceFileInfo)
        {
            MakeDest(sourceFilename, out QFileType destFormat, out string destFilename);
            if (OverwriteConfirm(destFilename)==false)
                return new ConverterResult(false, "ユーザにより取り消されました。", sourceFilename, destFilename, 0, 0, 0, 0, sourceFileInfo, null, QFileType.Unknown, 0, 0);

            using var sourceImage = Image.FromFile(sourceFilename, true);
            using var destImage = Settings.Default.Resize ? QImaging.Resize(sourceImage, Settings.Default.MaximumSize) : sourceImage;

            long destDepth = Image.GetPixelFormatSize(sourceImage.PixelFormat);
            if (Settings.Default.ChangeColorDepth)
                destDepth = Settings.Default.ColorDepth < destDepth ? Settings.Default.ColorDepth : destDepth;

            if (destFormat == QFileType.Jpeg)
            {
                SaveJpg(destImage, destFilename, destDepth);
            }
            else if (destFormat == QFileType.Png)
            {
                SavePng(destImage, destFilename, destDepth);
            }
            else if (destFormat == QFileType.Gif)
            {
                SaveGif(destImage, destFilename, destDepth);
            }
            else
            {
                throw new NotImplementedException();
            }
            var destFileInfo = new FileInfo(destFilename);
            return new ConverterResult(true, "正常終了", sourceFilename, destFilename, sourceImage.Width, sourceImage.Height, destImage.Width, destImage.Height, sourceFileInfo, destFileInfo, destFormat, 0, destDepth);
        }

        private void MakeDest(string sourceFilename, out QFileType destFormat, out string destFilename)
        {
            destFormat = Settings.Default.ChangeFormat ? QImaging.GetFileType(Settings.Default.Format) : QImaging.GetFileType(sourceFilename);
            destFilename = GetDestFilename(sourceFilename);
            if (!Directory.Exists(Path.GetDirectoryName(destFilename)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destFilename));
            }
        }

        private bool OverwriteConfirm(string destFilename)
        {
            if (Settings.Default.OverwriteConfirm && File.Exists(destFilename))
            {
                if (MessageBox.Show("ファイルが存在しますが、上書きしますか？\r\n" + destFilename,
                    "ImageQuant", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return false;
                }
            }
            return true;
        }

        private void SaveGif(Image destImage, string destFilename, long destDepth)
        {
            using var gifep = new EncoderParameters(1);
            using var ep0 = new EncoderParameter(Encoder.ColorDepth, destDepth);
            gifep.Param[0] = ep0;
            destImage.Save(destFilename, GifImageCodecInfo, gifep);
        }

        private void SavePng(Image image, string destfilename, long depth)
        {
            using var pngEncoderParameters = new EncoderParameters(1);
            using var ep0 = new EncoderParameter(Encoder.ColorDepth, depth);
            pngEncoderParameters.Param[0] = ep0;
            image.Save(destfilename, PngImageCodecInfo, pngEncoderParameters);
            if (Settings.Default.ChangePngQuality)
            {
                Assembly myAssembly = Assembly.GetEntryAssembly();
                string imageQuantPath = myAssembly.Location;
                string pngquantPath = Path.GetDirectoryName(imageQuantPath) + @"\pngquant.exe";
                if (File.Exists(pngquantPath))
                {
                    using var proc = new Process();
                    proc.StartInfo.FileName = pngquantPath;
                    proc.StartInfo.Arguments = $"-f --quality -{Settings.Default.PngQuality} \"{destfilename}";
                    proc.StartInfo.CreateNoWindow = true;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.RedirectStandardError = true;
                    proc.StartInfo.UseShellExecute = false;
                    proc.Start();
                    proc.WaitForExit();
                    if (proc.ExitCode != 0)
                    {
                        throw new RuntimeException("pngquant.exeが0以外のコードを出力しました。", imageQuantPath, proc.ExitCode, proc.StandardOutput.ReadToEnd(), proc.StandardError.ReadToEnd());
                    }
                }
                else
                {
                    Debug.WriteLine("pngquantが見つかりません");
                }

            }
        }

        private void SaveJpg(Image image, string destfilename, long depth)
        {
            var quality = Settings.Default.ChangeJpgQuality ? Settings.Default.JpgQuality : 90L;
            using var jpgEncoderParameters = new EncoderParameters(2);
            using var ep0 = new EncoderParameter(Encoder.Quality, quality);
            using var ep1 = new EncoderParameter(Encoder.ColorDepth, depth);
            jpgEncoderParameters.Param[0] = ep0;
            jpgEncoderParameters.Param[1] = ep1;
            image.Save(destfilename, JpgImageCodecInfo, jpgEncoderParameters);
        }

        private string GetDestFilename(string target)
        {
            string dir, fname;
            dir = Settings.Default.SaveManualPath ? Settings.Default.SavePath : DestDir;
            dir = dir == "" ? Path.GetDirectoryName(target) : dir;
            dir = dir == "" ? TempDir : dir;

            dir = Settings.Default.CreateChildDirectory ?
                Path.Combine(dir, Settings.Default.ChildDirectory) :
                dir;
            fname = Settings.Default.Prefix ? Settings.Default.PrefixName : "";
            fname += Path.GetFileNameWithoutExtension(target);
            fname = Settings.Default.Suffix ? fname + Settings.Default.SuffixName : fname;
            fname += Path.GetExtension(target);
            fname = Settings.Default.ChangeFormat ?
                Path.ChangeExtension(fname, QImaging.GetExtension(QImaging.GetFileType(Settings.Default.Format))) :
                fname;
            return Path.Combine(dir, fname);
        }

        public string SaveBmpTemp(Bitmap bmp)
        {
            string dir, fname;
            dir = Settings.Default.SaveManualPath ? Settings.Default.SavePath : DestDir;
            dir = dir == "" ? TempDir : dir;

            fname = $"bitmap_{DateTime.Now:yyyyMMdd-HHmmss)}{QImaging.GetExtension(QFileType.Bmp)}";
            if(!Directory.Exists(Path.GetDirectoryName(dir)))
                Directory.CreateDirectory(Path.GetDirectoryName(dir));
            bmp.Save(Path.Combine(dir,fname));
            return Path.Combine(dir, fname);
        }


    }
}
