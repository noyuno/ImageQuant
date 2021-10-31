//using ImageQuant.Properties;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace ImageQuant
{
    class QImaging
    {
        /// <summary>
        /// ファイル情報を取得
        /// </summary>
        /// <param name="pszPath"></param>
        /// <param name="dwFileAttributes"></param>
        /// <param name="psfi"></param>
        /// <param name="cbFileInfo"></param>
        /// <param name="uFlags"></param>
        /// <returns></returns>
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, out SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        /// <summary>
        /// イメージリストを登録
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// SHGetFileInfo関数で使用する構造体
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };
#pragma warning disable IDE0051 // 使用されていないプライベート メンバーを削除する

        // ファイル情報用
        private const int SHGFI_LARGEICON = 0x00000000;
        private const int SHGFI_SMALLICON = 0x00000001;


        private const int SHGFI_USEFILEATTRIBUTES = 0x00000010;
        private const int SHGFI_OVERLAYINDEX = 0x00000040;
        private const int SHGFI_ICON = 0x00000100;
        private const int SHGFI_SYSICONINDEX = 0x00004000;
        private const int SHGFI_TYPENAME = 0x000000400;

        // TreeView用
        private const int TVSIL_NORMAL = 0x0000;
        private const int TVSIL_STATE = 0x0002;
        private const int TVM_SETIMAGELIST = 0x1109;

        // ListView用
        private const int LVSIL_NORMAL = 0;
        private const int LVSIL_SMALL = 1;
        private const int LVM_SETIMAGELIST = 0x1003;
#pragma warning restore IDE0051 // 使用されていないプライベート メンバーを削除する

        public static int GetIconIndex(string name)
        {
#pragma warning disable IDE0059 // 値の不必要な代入
            int iconIndex = 0;
            if (Directory.Exists(name))
            {
                SHFILEINFO shFileInfo = new SHFILEINFO();
                IntPtr hSuccess = SHGetFileInfo(name, 0, out shFileInfo, (uint)Marshal.SizeOf(shFileInfo), SHGFI_ICON | SHGFI_LARGEICON | SHGFI_SMALLICON | SHGFI_SYSICONINDEX | SHGFI_TYPENAME);
                if (hSuccess != IntPtr.Zero)
                {
                    iconIndex = shFileInfo.iIcon;
                }
            }
            else
            {
                SHFILEINFO shinfo = new SHFILEINFO();
                IntPtr hSuccess = SHGetFileInfo(name, 0, out shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_LARGEICON | SHGFI_SMALLICON | SHGFI_SYSICONINDEX | SHGFI_TYPENAME);
                if (hSuccess != IntPtr.Zero)
                {
                    iconIndex = shinfo.iIcon;
                }
            }
            return iconIndex;
#pragma warning restore IDE0059 // 値の不必要な代入

        }

        public static string FileSizeToString(long fileSize)
        {
            string ret = fileSize + " B";
            if (fileSize > (1024f * 1024f * 1024f))
            {
                ret = Math.Round((fileSize / 1024f / 1024f / 1024f), 2).ToString() + " GB";
            }
            else if (fileSize > (1024f * 1024f))
            {
                ret = Math.Round((fileSize / 1024f / 1024f), 2).ToString() + " MB";
            }
            else if (fileSize > 1024f)
            {
                ret = Math.Round((fileSize / 1024f)).ToString() + " KB";
            }

            return ret;
        }


        public static ImageCodecInfo GetEncoderInfo(string mineType)
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

        public static ImageCodecInfo GetEncoderInfo(ImageFormat f)
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

        public static string GetExtension(QFileType format)
        {
            return format switch
            {
                QFileType.Jpeg => ".jpg",
                QFileType.Gif => ".gif",
                QFileType.Png => ".png",
                QFileType.Bmp => ".bmp",
                QFileType.Tiff => ".tif",
                QFileType.Icon => ".ico",
                QFileType.Pdf => ".pdf",
                QFileType.Unknown => "",
                _ => ""
            };
        }

        public static QFileType GetFileType(string name)
        {
            return name switch
            {
                "JPEG" => QFileType.Jpeg,
                "JPG" => QFileType.Jpeg,
                "GIF" => QFileType.Gif,
                "PNG" => QFileType.Png,
                "BMP" => QFileType.Bmp,
                "TIFF" => QFileType.Tiff,
                "PDF" => QFileType.Pdf,
                "ICON" => QFileType.Icon,
                "ICO" => QFileType.Icon,
                _ => Path.GetExtension(name).ToLower() switch
                {
                    ".jpg" => QFileType.Jpeg,
                    ".jpeg" => QFileType.Jpeg,
                    ".jfif" => QFileType.Jpeg,
                    ".gif" => QFileType.Gif,
                    ".png" => QFileType.Png,
                    ".bmp" => QFileType.Bmp,
                    ".tif" => QFileType.Tiff,
                    ".tiff" => QFileType.Tiff,
                    ".ico" => QFileType.Icon,
                    ".pdf" => QFileType.Pdf,
                    _ => QFileType.Unknown
                }
            };
            
        }

        public static QFileType GetFileType(ImageFormat format)
        {
            if (format == ImageFormat.Jpeg) return QFileType.Jpeg;
            else if (format == ImageFormat.Gif) return QFileType.Gif;
            else if (format == ImageFormat.Png) return QFileType.Png;
            else if (format == ImageFormat.Bmp) return QFileType.Bmp;
            else if (format == ImageFormat.Tiff) return QFileType.Tiff;
            else if (format == ImageFormat.Icon) return QFileType.Icon;
            else return QFileType.Unknown;
        }

        public static (Bitmap,int) GetThumbnail(string name, int width, int height, RotateFlipType rotation, Bitmap fail)
        {
            const int thumbsize = 100;
            var ret = new Bitmap(thumbsize, thumbsize);
            using var g = Graphics.FromImage(ret);
            g.FillRectangle(new SolidBrush(Color.Black), 0, 0, thumbsize, thumbsize);
            var destpoints = GetThumbnailPoints(ret, width, height, rotation);

            Bitmap thumb;
            int fallback;
            (thumb, fallback) = GetThumbnail(name, fail);
            g.DrawImage(thumb, destpoints) ;
            g.Dispose();
            ret.MakeTransparent(Color.Black);
            return (ret, fallback);
        }

        public static (Bitmap,int) GetThumbnail(string name, Bitmap fail)
        {
            Bitmap thumb;
            var fallback = 0;
            using (var so = ShellObject.FromParsingName(name))
            {
                try
                {
                    so.Thumbnail.FormatOption = ShellThumbnailFormatOption.ThumbnailOnly;
                    thumb = so.Thumbnail.LargeBitmap;
                }
                catch (Exception)
                {
                    try
                    {
                        so.Thumbnail.FormatOption = ShellThumbnailFormatOption.Default;
                        thumb = so.Thumbnail.LargeBitmap;
                        fallback = 1;
                    }
                    catch (Exception)
                    {
                        try
                        {
                            thumb = so.Thumbnail.Bitmap;
                            fallback = 2;
                        }
                        catch (Exception)
                        {
                            thumb = fail;
                            fallback = 3;
                        }
                    }
                }
            }
            thumb.MakeTransparent(Color.Black);
            return (thumb, fallback);
        }

        private static Point[] GetThumbnailPoints(Image ret, int width, int height, RotateFlipType rotation)
        {
            int thumbsize = ret.Width;
            int large, small;
            if (width > height)
            {
                // (0,a) (b,a)
                // (0,c) (b,c)
                large = width;
                small = height;
            }
            else
            {
                // (a,0) (a,b)
                // (c,0) (c,b)
                large = height;
                small = width;
            }
            var scale = (double)thumbsize / large;
            var a = (int)((large - small) / 2 * scale);
            var b = (int)(large * scale);
            var c = (int)((small + (large - small) / 2) * scale);
            //var d = (int)((small - (large - small) / 2) * scale);
            var destpoints = new Point[3];
            // 0 1
            // 2 -
            if (rotation == RotateFlipType.Rotate90FlipNone || rotation == RotateFlipType.Rotate270FlipNone)
            {
                if (height > width)
                {
                    destpoints[0] = new Point(0, a);
                    destpoints[1] = new Point(b, a);
                    destpoints[2] = new Point(0, c);
                    //destpoints[3] = new Point(b, c);
                }
                else
                {
                    destpoints[0] = new Point(a, 0);
                    destpoints[2] = new Point(a, b);
                    destpoints[1] = new Point(c, 0);
                    //destpoints[3] = new Point(c, b);
                }
            }
            else
            {
                if (width > height)
                {
                    destpoints[0] = new Point(0, a);
                    destpoints[1] = new Point(b, a);
                    destpoints[2] = new Point(0, c);
                    //destpoints[3] = new Point(b, c);
                }
                else
                {
                    destpoints[0] = new Point(a, 0);
                    destpoints[2] = new Point(a, b);
                    destpoints[1] = new Point(c, 0);
                    //destpoints[3] = new Point(c, b);
                }
            }

            return destpoints;
        }

        //public object[] SlideArray(object[] obj)
        //{
        //    object[] ret = new object[obj.Length];
        //    object tmp;
        //    for (int i = 0; i < obj.Length; i++)
        //    {

        //    }
        //}

        public static bool SaveJpg(Image image, string destfilename, long depth, int quality)
        {
            using var jpgEncoderParameters = new EncoderParameters(2);
            using var ep0 = new EncoderParameter(Encoder.Quality, quality);
            using var ep1 = new EncoderParameter(Encoder.ColorDepth, depth);
            jpgEncoderParameters.Param[0] = ep0;
            jpgEncoderParameters.Param[1] = ep1;
            image.Save(destfilename, GetEncoderInfo("image/jpeg"), jpgEncoderParameters);
            return true;
        }

        public static bool SavePng(Image image, string destfilename, long depth, string tempdir, bool usepngquant, int quality, out bool pngquant)
        {
            using var pngEncoderParameters = new EncoderParameters(1);
            using var ep0 = new EncoderParameter(Encoder.ColorDepth, depth);
            pngEncoderParameters.Param[0] = ep0;
            if (usepngquant)
            {
                var t = Path.Combine(tempdir, $"pngquant-temp-{Guid.NewGuid().ToString("N").Substring(0, 12)}.png");
                image.Save(t, GetEncoderInfo("image/png"), pngEncoderParameters);
                PngQuant(t, quality);
                if (File.Exists(destfilename))
                {
                    File.Delete(destfilename);
                }
                File.Move(t, destfilename);
                pngquant = true;
            }
            else
            {
                image.Save(destfilename, GetEncoderInfo("image/png"), pngEncoderParameters);
                pngquant = false;
            }
            return true;
        }

        // bug : 日本語を含むファイル名は変換できない
        private static void PngQuant(string destfilename, int quality)
        {
            Assembly myAssembly = Assembly.GetEntryAssembly();
            string imageQuantPath = myAssembly.Location;
            string pngquantPath = Path.GetDirectoryName(imageQuantPath) + @"\pngquant.exe";
            if (File.Exists(pngquantPath))
            {
                using var proc = new Process();
                proc.StartInfo.FileName = pngquantPath;
                proc.StartInfo.Arguments = $"-f --quality -{quality} -o \"{destfilename}\" \"{destfilename}\"";
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();
                proc.WaitForExit();
                if (proc.ExitCode != 0)
                {
                    throw new RuntimeException("pngquant.exeが0以外のコードを出力しました。", imageQuantPath + " " + proc.StartInfo.Arguments, proc.ExitCode, proc.StandardOutput.ReadToEnd(), proc.StandardError.ReadToEnd());
                }
            }
            else
            {
                throw new RuntimeException("pngquant.exeが見つかりません。", pngquantPath, 0, "", "");
            }
        }

        public static bool SaveGif(Image image, string destFilename, long destDepth)
        {
            using var gifep = new EncoderParameters(1);
            using var ep0 = new EncoderParameter(Encoder.ColorDepth, destDepth);
            gifep.Param[0] = ep0;
            image.Save(destFilename, GetEncoderInfo("image/gif"), gifep);
            return true;
        }


        public static void Ghostscript(string sourceFilename, string destFilename, string gsprofile, int gsdpi)
        {
            Assembly myAssembly = Assembly.GetEntryAssembly();
            string imageQuantPath = myAssembly.Location;
            string gspath = Path.GetDirectoryName(imageQuantPath) + @"\gswin64c.exe";
            if (File.Exists(gspath))
            {
                using var proc = new Process();
                proc.StartInfo.FileName = gspath;
                proc.StartInfo.Arguments = $"-sDEVICE={gsprofile} -r{gsdpi} -dGraphicsAlphaBits=4 -o \"{destFilename}\" \"{sourceFilename}";
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
        }

        public static Image Resize(Image image, int maximumSize)
        {
            var scale = Math.Min((float)maximumSize / image.Width, (float)maximumSize / image.Height);
            var fw = image.Width * scale;
            var fh = image.Height * scale;
            Image ret = new Bitmap((int)fw, (int)fh);
            Graphics g = Graphics.FromImage(ret);
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, maximumSize, maximumSize);
            g.DrawImage(image, 0, 0, fw, fh);
            g.Dispose();
            return ret;
        }

        //public static void RotateFromExif(Image image)
        //{
        //    image.RotateFlip(ReadOrientation(image));
        //}

        public static RotateFlipType RotateImage(Image image)
        {
            var rotate = QImaging.ReadOrientation(image);
            image.RotateFlip(rotate);
            return rotate;
        }

        public static RotateFlipType ReadOrientation(Image image, RotateFlipType add = RotateFlipType.RotateNoneFlipNone)
        {
            var rotint = (int)add; // 0
            foreach (var item in image.PropertyItems)
            {
                if (item.Id != 0x0112)
                {
                    continue;
                }
                rotint += (int)(item.Value[0] switch
                {
                    1 => RotateFlipType.RotateNoneFlipNone,
                    2 => RotateFlipType.RotateNoneFlipY,
                    3 => RotateFlipType.Rotate180FlipNone, // 2
                    4 => RotateFlipType.RotateNoneFlipX,
                    5 => RotateFlipType.Rotate270FlipY,
                    6 => RotateFlipType.Rotate90FlipNone, // 1
                    7 => RotateFlipType.Rotate90FlipY,
                    8 => RotateFlipType.Rotate270FlipNone, // 3
                    _ => RotateFlipType.RotateNoneFlipNone
                });
                if (rotint >= 4)
                {
                    rotint -= 4;
                }
            }
            return (RotateFlipType)rotint;
        }

        public static uint GetExifOrientation(RotateFlipType type)
        {
            return (int)type switch
            {
                0 => 1,
                1 => 6,
                2 => 3,
                3 => 8,
                4 => 4,
                5 => 5,
                6 => 2,
                7 => 7,
                _ => 1
            };
        }

        private static readonly List<string> queryPadding = new List<string>()
        {
            "/app1/ifd/PaddingSchema:Padding", // Query path for IFD metadata
            "/app1/ifd/exif/PaddingSchema:Padding", // Query path for EXIF metadata
            "/xmp/PaddingSchema:Padding", // Query path for XMP metadata
        };

        public static bool CopyMetadata(string sourceFilename, string destFilename, bool ignoreOrientation)
        {
            using var source = new MemoryStream(File.ReadAllBytes(sourceFilename));
            using var dest = new MemoryStream(File.ReadAllBytes(destFilename));
            using var destFile = File.Open(destFilename, FileMode.Open);
            var ret = EditDateTaken(source, dest, DateTime.Now, ignoreOrientation);
            if (ret == null)
                return false;
            destFile.Write(ret, 0, ret.Length);
            destFile.Close();
            return true;
        }

        public static Byte[] EditDateTaken(Stream source, Stream dest, DateTime date, bool ignoreOrientation)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (date == null)
                throw new ArgumentNullException("date");

            if (0 < source.Position)
                source.Seek(0, SeekOrigin.Begin);

            // Create BitmapDecoder for a lossless transcode.
            var sourceDecoder = BitmapDecoder.Create(
                source,
                BitmapCreateOptions.PreservePixelFormat | BitmapCreateOptions.IgnoreColorProfile,
                BitmapCacheOption.None);

            var destDecoder = BitmapDecoder.Create(
                dest,
                BitmapCreateOptions.PreservePixelFormat | BitmapCreateOptions.IgnoreColorProfile,
                BitmapCacheOption.None);

            // Check if the source image data is in JPG format.
            if (!sourceDecoder.CodecInfo.FileExtensions.Contains("jpg"))
                return null;

            if ((sourceDecoder.Frames[0] == null) || (sourceDecoder.Frames[0].Metadata == null))
                return null;

            var sourceMetadata = sourceDecoder.Frames[0].Metadata.Clone() as BitmapMetadata;

            // Add padding (4KiB) to metadata.
            queryPadding.ForEach(x => sourceMetadata.SetQuery(x, 4096U));

            using var ms = new MemoryStream();
            // Perform a lossless transcode with metadata which includes added padding.
            var outcomeEncoder = new JpegBitmapEncoder();

            outcomeEncoder.Frames.Add(BitmapFrame.Create(
                destDecoder.Frames[0],
                destDecoder.Frames[0].Thumbnail,
                sourceMetadata,
                destDecoder.Frames[0].ColorContexts));

            outcomeEncoder.Save(ms);

            // Create InPlaceBitmapMetadataWriter.
            ms.Seek(0, SeekOrigin.Begin);

            var outcomeDecoder = BitmapDecoder.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.Default);

            var metadataWriter = outcomeDecoder.Frames[0].CreateInPlaceBitmapMetadataWriter();

            // Edit date taken field by accessing property of InPlaceBitmapMetadataWriter.
            metadataWriter.DateTaken = date.ToString();

            // Edit date taken field by using query with path string.
            metadataWriter.SetQuery("/app1/ifd/exif/{ushort=36867}", date.ToString("yyyy:MM:dd HH:mm:ss"));

            if (ignoreOrientation)
            {
                metadataWriter.SetQuery("/app1/ifd/exif:{uint=274}", 1U);
            }

            // Try to save edited metadata to stream.
            if (metadataWriter.TrySave())
            {
                Debug.WriteLine("InPlaceMetadataWriter succeeded!");
                return ms.ToArray();
            }
            else
            {
                Debug.WriteLine("InPlaceMetadataWriter failed!");
                return null;
            }
        }

        public static bool RotateExif(string path, RotateFlipType rotation)
        {

            using var source = new MemoryStream(File.ReadAllBytes(path));

            

            // Create BitmapDecoder for a lossless transcode.
            var sourceDecoder = BitmapDecoder.Create(
                source,
                BitmapCreateOptions.PreservePixelFormat | BitmapCreateOptions.IgnoreColorProfile,
                BitmapCacheOption.None);

            // Check if the source image data is in JPG format.
            if (!sourceDecoder.CodecInfo.FileExtensions.Contains("jpg"))
                return false;

            if ((sourceDecoder.Frames[0] == null) || (sourceDecoder.Frames[0].Metadata == null))
                return false;

            var sourceMetadata = sourceDecoder.Frames[0].Metadata.Clone() as BitmapMetadata;

            // Add padding (4KiB) to metadata.
            queryPadding.ForEach(x => sourceMetadata.SetQuery(x, 4096U));

            using var ms = new MemoryStream();
            // Perform a lossless transcode with metadata which includes added padding.
            var outcomeEncoder = new JpegBitmapEncoder();

            outcomeEncoder.Frames.Add(BitmapFrame.Create(
                sourceDecoder.Frames[0],
                sourceDecoder.Frames[0].Thumbnail,
                sourceMetadata,
                sourceDecoder.Frames[0].ColorContexts));

            outcomeEncoder.Save(ms);

            // Create InPlaceBitmapMetadataWriter.
            ms.Seek(0, SeekOrigin.Begin);

            var outcomeDecoder = BitmapDecoder.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.Default);

            var metadataWriter = outcomeDecoder.Frames[0].CreateInPlaceBitmapMetadataWriter();

            var rot = GetExifOrientation(rotation);
            metadataWriter.SetQuery("/app1/ifd/exif:{uint=274}", rot);


            // Try to save edited metadata to stream.
            if (metadataWriter.TrySave())
            {
                Debug.WriteLine("InPlaceMetadataWriter succeeded!");
                using var file = File.Open(path, FileMode.Open);
                file.Write(ms.ToArray(), 0, (int)ms.Length);
                file.Close();
                return true;
            }
            else
            {
                Debug.WriteLine("InPlaceMetadataWriter failed!");
                return false;
            }
        }

    }
}
