using ImageQuant.Properties;
using System;
using System.Drawing;
using System.IO;
using SWF = System.Windows.Forms;

namespace ImageQuant
{
    public class Converter
    {
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

        public Converter()
        {
            TempDir = $"{Path.GetTempPath()}\\{SWF.Application.ProductName}\\{Guid.NewGuid().ToString("N").Substring(0, 12)}";
            Directory.CreateDirectory(TempDir);
            DestDir = "";
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
            return sourceFileType switch
            {
                QFileType.Pdf => ConvertFromPDF(sourceFilename, sourceFileInfo),
                QFileType.Unknown => new ConverterResult(false, "このファイルは画像ファイルではないようです。",
                    sourceFilename, "", 0, 0, 0, 0, sourceFileInfo, null, QFileType.Unknown, 0, 0,
                    false, false, RotateFlipType.RotateNoneFlipNone, false),
                _ => ConvertImage(sourceFilename, sourceFileInfo)
            };
        }

        private ConverterResult ConvertFromPDF(string sourceFilename, FileInfo sourceFileInfo)
        {
            MakeDest(sourceFilename, out QFileType destFormat, out string destFilename);
            QImaging.Ghostscript(sourceFilename, destFilename, Settings.Default.GSProfile, Settings.Default.GSDpi);
            var destFileInfo = new FileInfo(destFilename);
            var destImage = Image.FromFile(destFilename);
            var ret = new ConverterResult(true, "正常終了", sourceFilename, destFilename, 0, 0, destImage.Width, destImage.Height, sourceFileInfo, destFileInfo, destFormat, 0, 24, false, true, RotateFlipType.RotateNoneFlipNone, false);
            destImage.Dispose();
            return ret;
        }


        private ConverterResult ConvertImage(string sourceFilename, FileInfo sourceFileInfo)
        {
            MakeDest(sourceFilename, out QFileType destFormat, out string destFilename);
            using var sourceImage = Image.FromFile(sourceFilename, true);

            var rotate = RotateFlipType.RotateNoneFlipNone;
            if (Settings.Default.RotateFromExif)
            {
                QImaging.RotateImage(sourceImage);
            }

            using var destImage = Settings.Default.Resize ? QImaging.Resize(sourceImage, Settings.Default.MaximumSize) : sourceImage;

            long destDepth = Image.GetPixelFormatSize(sourceImage.PixelFormat);
            if (Settings.Default.ChangeColorDepth)
                destDepth = Settings.Default.ColorDepth < destDepth ? Settings.Default.ColorDepth : destDepth;

            bool pngquant = false;
            var quality = 0;
            if (destFormat == QFileType.Jpeg)
            {
                quality = Settings.Default.ChangeJpgQuality ? Settings.Default.JpgQuality : 90;
                QImaging.SaveJpg(destImage, destFilename, destDepth, quality);
            }
            else if (destFormat == QFileType.Png)
            {
                QImaging.SavePng(destImage, destFilename, destDepth, TempDir, Settings.Default.ChangePngQuality, Settings.Default.PngQuality, out pngquant);
                quality = pngquant ? Settings.Default.PngQuality : 0;
            }
            else if (destFormat == QFileType.Gif)
            {
                QImaging.SaveGif(destImage, destFilename, destDepth);
            }
            else
            {
                throw new NotImplementedException();
            }

            var metadata = false;
            if (Settings.Default.SaveExif)
                metadata = QImaging.CopyMetadata(sourceFilename, destFilename, Settings.Default.RotateFromExif);

            var destFileInfo = new FileInfo(destFilename);

            return new ConverterResult(true, "正常終了", sourceFilename, destFilename, 
                sourceImage.Width, sourceImage.Height, destImage.Width, destImage.Height,
                sourceFileInfo, destFileInfo, destFormat, quality, destDepth, pngquant,
                false, rotate, metadata);
        }

        public ConverterResult RotatePng(string filename, ConverterResult converterResult, RotateFlipType rotation)
        {

            var image = Image.FromFile(filename);
            image.RotateFlip(rotation);
            var depth = Image.GetPixelFormatSize(image.PixelFormat);
            QImaging.SavePng(image, filename, depth, TempDir, Settings.Default.ChangePngQuality, Settings.Default.PngQuality, out var pngquant);
            var destFileInfo = new FileInfo(filename);
            string source;
            int sourceWidth;
            int sourceHeight;
            FileInfo sourceFileInfo;
            if (converterResult == null)
            {
                source = destFileInfo.FullName;
                sourceWidth = image.Width;
                sourceHeight = image.Height;
                sourceFileInfo = destFileInfo;
            }
            else
            {
                source = converterResult.SourceFileInfo.FullName;
                sourceWidth = converterResult.SourceWidth;
                sourceHeight = converterResult.SourceHeight;
                sourceFileInfo = converterResult.SourceFileInfo;
            }
            var quality = pngquant ? Settings.Default.PngQuality : 0;
            return new ConverterResult(true, "正常終了", source,
                filename, sourceWidth, sourceHeight, image.Width, image.Height, 
                sourceFileInfo, destFileInfo, QFileType.Png, quality, depth, pngquant,
                false, RotateFlipType.RotateNoneFlipNone, false);
        }

        public void MakeDest(string sourceFilename, out QFileType destFormat, out string destFilename)
        {
            destFormat = Settings.Default.ChangeFormat ? QImaging.GetFileType(Settings.Default.Format) : QImaging.GetFileType(sourceFilename);
            destFilename = GetDestFilename(sourceFilename);
            if (!Directory.Exists(Path.GetDirectoryName(destFilename)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destFilename));
            }
        }

        private string GetDestFilename(string target)
        {
            string dir, fname;
            //dir = Settings.Default.SaveManualPath ? Settings.Default.SavePath : DestDir;
            dir = DestDir == "" ? Path.GetDirectoryName(target) : DestDir;
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
