using ImageQuant.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageQuant
{
    class FileImaging
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

        public static Image GetThumbnail(string name)
        {
            var original = Bitmap.FromFile(name);
            int width = Settings.Default.ThumbnailSize;
            int height = Settings.Default.ThumbnailSize;
            Bitmap thumbnail = new Bitmap(width, height);

            Graphics g = Graphics.FromImage(thumbnail);
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, width, height);
            float fw = (float)width / (float)original.Width;
            float fh = (float)height / (float)original.Height;
            float scale = Math.Min(fw, fh);
            fw = original.Width * scale;
            fh = original.Height * scale;
            g.DrawImage(original, (width - fw) / 2, (height - fh) / 2, fw, fh);
            g.Dispose();
            original.Dispose();
            return thumbnail;
        }
    }
}
