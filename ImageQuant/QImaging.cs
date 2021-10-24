using ImageQuant.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack;
using Microsoft.WindowsAPICodePack.Shell;

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

        public static int GetIconIndex(string name)
        {
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
                String type = "";
                SHFILEINFO shinfo = new SHFILEINFO();
                IntPtr hSuccess = SHGetFileInfo(name, 0, out shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_LARGEICON | SHGFI_SMALLICON | SHGFI_SYSICONINDEX | SHGFI_TYPENAME);
                if (hSuccess != IntPtr.Zero)
                {
                    iconIndex = shinfo.iIcon;
                }
            }
            return iconIndex;
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

        //public static string GetExtension(ImageFormat format)
        //{
        //    if (format == ImageFormat.Jpeg) return ".jpg";
        //    else if (format == ImageFormat.Gif) return ".gif";
        //    else if (format == ImageFormat.Png) return ".png";
        //    else if (format == ImageFormat.Bmp) return ".bmp";
        //    else if (format == ImageFormat.Tiff) return ".tif";
        //    else throw new NotImplementedException(format.ToString());
        //}

        //public static ImageFormat GetImageFormat(string name)
        //{
        //    if (name == "JPEG") return ImageFormat.Jpeg;
        //    else if (name == "JPG") return ImageFormat.Jpeg;
        //    else if (name == "GIF") return ImageFormat.Gif;
        //    else if (name == "PNG") return ImageFormat.Png;
        //    else if (name == "BMP") return ImageFormat.Bmp;
        //    else if (name == "TIFF") return ImageFormat.Tiff;
        //    else
        //    {
        //        var ext = Path.GetExtension(name).ToLower();
        //        if (ext == ".jpg") return ImageFormat.Jpeg;
        //        else if (ext == ".jpeg") return ImageFormat.Jpeg;
        //        else if (ext == ".jfif") return ImageFormat.Jpeg;
        //        else if (ext == ".gif") return ImageFormat.Gif;
        //        else if (ext == ".png") return ImageFormat.Png;
        //        else if (ext == ".bmp") return ImageFormat.Bmp;
        //        else if (ext == ".tif") return ImageFormat.Tiff;
        //        else if (ext == ".tiff") return ImageFormat.Tiff;
        //        else return null;
        //    }
        //}

        public static string GetExtension(QFileType format)
        {
            if (format == QFileType.Jpeg) return ".jpg";
            else if (format == QFileType.Gif) return ".gif";
            else if (format == QFileType.Png) return ".png";
            else if (format == QFileType.Bmp) return ".bmp";
            else if (format == QFileType.Tiff) return ".tif";
            else if (format == QFileType.Icon) return ".ico";
            else if (format == QFileType.Pdf) return ".pdf";
            else return "";
        }

        public static QFileType GetFileType(string name)
        {
            if (name == "JPEG") return QFileType.Jpeg;
            else if (name == "JPG") return QFileType.Jpeg;
            else if (name == "GIF") return QFileType.Gif;
            else if (name == "PNG") return QFileType.Png;
            else if (name == "BMP") return QFileType.Bmp;
            else if (name == "TIFF") return QFileType.Tiff;
            else if (name == "PDF") return QFileType.Pdf;
            else if (name == "ICON") return QFileType.Icon;
            else if (name == "ICO") return QFileType.Icon;
            else
            {
                var ext = Path.GetExtension(name).ToLower();
                if (ext == ".jpg") return QFileType.Jpeg;
                else if (ext == ".jpeg") return QFileType.Jpeg;
                else if (ext == ".jfif") return QFileType.Jpeg;
                else if (ext == ".gif") return QFileType.Gif;
                else if (ext == ".png") return QFileType.Png;
                else if (ext == ".bmp") return QFileType.Bmp;
                else if (ext == ".tif") return QFileType.Tiff;
                else if (ext == ".tiff") return QFileType.Tiff;
                else if (ext == ".ico") return QFileType.Icon;
                else return QFileType.Unknown;
            }
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

        public static Bitmap GetThumbnail(string name)
        {
            Bitmap ret;
            //if (GetImageFormat(name) == null)
            //{
            using (var so = ShellObject.FromParsingName(name))
            {
                //file.Thumbnail.FormatOption = ShellThumbnailFormatOption.IconOnly;
                ret = so.Thumbnail.LargeBitmap;
                ret.MakeTransparent(Color.Black);
            }
            //}
            //else
            //{
            //    var original = Bitmap.FromFile(name);
            //    ret = GetThumbnail(original);
            //    original.Dispose();
            //}
            return ret;
        }

        //public static Image GetThumbnail(Image image)
        //{
        //    var size = Settings.Default.ThumbnailSize;
        //    var scale = Math.Min((float)size / image.Width, (float)size / image.Height);
        //    var fw = image.Width * scale;
        //    var fh = image.Height * scale;
        //    Image ret = new Bitmap(size, size);
        //    Graphics g = Graphics.FromImage(ret);
        //    g.FillRectangle(new SolidBrush(Color.White), 0, 0, size, size);
        //    g.DrawImage(image, (size - fw) / 2, (size - fh) / 2, fw, fh);
        //    g.Dispose();
        //    return ret;
        //}
    

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
    }
}
