using ImageQuant.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImageQuant
{
    public class Converter
    {
        public readonly string[] ImageFormatList = new string[] { "JPEG", "GIF", "PNG", "BMP", "TIFF" };

        public string TempDir;
        public string DestDir;

        //public string DestDirName = "ImageQuant";
        //public ImageFormat DestFormat;
        //public bool ChangeJpgQuality = true;
        //public long JpgQuality = 90;
        //public bool PngQuant = true;
        public ImageCodecInfo JpgImageCodecInfo;

        public Converter()
        {
            TempDir = Path.GetTempFileName();
            File.Delete(TempDir);
            Directory.CreateDirectory(TempDir);

            JpgImageCodecInfo = GetEncoderInfo("image/jpeg");
        }

        public void Dispose()
        {
            if (Directory.Exists(TempDir))
            {
                Directory.Delete(TempDir);
            }
        }

        private static ImageCodecInfo GetEncoderInfo(string mineType)
        {
            ImageCodecInfo[] encs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo enc in encs)
            {
                if (enc.MimeType == mineType)
                {
                    return enc;
                }
            }
            return null;
        }

        private static ImageCodecInfo GetEncoderInfo(ImageFormat f)
        {
            ImageCodecInfo[] encs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo enc in encs)
            {
                if (enc.FormatID == f.Guid)
                {
                    return enc;
                }
            }
            return null;
        }

        public string GetDestExt(ImageFormat format)
        {
            if (format == ImageFormat.Jpeg) return ".jpg";
            else if (format == ImageFormat.Gif) return ".gif";
            else if (format == ImageFormat.Png) return ".png";
            else if (format == ImageFormat.Bmp) return ".bmp";
            else if (format == ImageFormat.Tiff) return ".tif";
            else throw new NotImplementedException(format.ToString());
        }

        public ImageFormat GetImageFormat(string name)
        {
            if (name == "JPEG") return ImageFormat.Jpeg;
            else if (name == "GIF") return ImageFormat.Gif;
            else if (name == "PNG") return ImageFormat.Png;
            else if (name == "BMP") return ImageFormat.Bmp;
            else if (name == "TIFF") return ImageFormat.Tiff;
            else
            {
                var ext = Path.GetExtension(name).ToLower();
                if (ext == ".jpg") return ImageFormat.Jpeg;
                else if (ext == ".jpeg") return ImageFormat.Jpeg;
                else if (ext == ".jfif") return ImageFormat.Jpeg;
                else if (ext == ".gif") return ImageFormat.Gif;
                else if (ext == ".png") return ImageFormat.Png;
                else if (ext == ".bmp") return ImageFormat.Bmp;
                else if (ext == ".tif") return ImageFormat.Tiff;
                else if (ext == ".tiff") return ImageFormat.Tiff;
                else throw new NotImplementedException(name);
            }
        }

        public string Convert(string target)
        {
            var image = Image.FromFile(target, true);
            var destformat = Settings.Default.ChangeFormat ?
                GetImageFormat(Settings.Default.Format) :
                GetImageFormat(target);
            var destfilename = GetDestFilename(target);
            if (!Directory.Exists(Path.GetDirectoryName(destfilename)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destfilename));
            }
            if (File.Exists(destfilename))
            {
                if (MessageBox.Show("ファイルが存在しますが、上書きしますか？\r\n" + destfilename,
                    "ImageQuant", MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    return "";
                }
            }

            if (destformat == ImageFormat.Jpeg)
            {
                if (Settings.Default.ChangeJpgQuality)
                {
                    var JpgEncoderParameters = new EncoderParameters(1);
                    var ep = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality,
                        (long)Settings.Default.JpgQuality);
                    JpgEncoderParameters.Param[0] = ep;
                    image.Save(destfilename, JpgImageCodecInfo, JpgEncoderParameters);
                }
                else
                {
                    image.Save(destfilename, ImageFormat.Jpeg);
                }
            }
            else if (destformat == ImageFormat.Png)
            {
                image.Save(destfilename, ImageFormat.Png);
                Assembly myAssembly = Assembly.GetEntryAssembly();
                string imageQuantPath = myAssembly.Location;
                string pngquantPath = Path.GetDirectoryName(imageQuantPath) + @"\pngquant.exe";
                if (File.Exists(pngquantPath))
                {
                    var quantProcess = new Process();
                    quantProcess.StartInfo.FileName = pngquantPath;
                    if (Settings.Default.ChangePngQuality)
                    {
                        quantProcess.StartInfo.Arguments =
                            "--quality -" + Settings.Default.PngQuality.ToString();
                    }
                    quantProcess.Start();
                    quantProcess.WaitForExit();

                }
            }
            return destfilename;
        }

        private string GetDestFilename(string target)
        {
            string dir, fname;
            dir = Settings.Default.SaveManualPath ?
                Settings.Default.SavePath :
                Path.GetDirectoryName(target);
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
                Path.ChangeExtension(fname, GetDestExt(GetImageFormat(Settings.Default.Format))) :
                fname;
            return Path.Combine(dir, fname);
        }

        public string SaveBmp(Bitmap bmp)
        {
            string destfilename = GetDestFilename(@"\bitmap_" + (new DateTime()).ToString("yyyyMMdd-HHmmss") + GetDestExt(ImageFormat.Bmp));
            bmp.Save(destfilename);
            return destfilename;
        }


    }
}
