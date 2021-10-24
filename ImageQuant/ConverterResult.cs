using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageQuant
{
    public class ConverterResult
    {
        public bool Success;
        public string ErrorMessage;
        public string SourcePath;
        public string DestPath;
        public int SourceWidth;
        public int SourceHeight;
        public int DestWidth;
        public int DestHeight;
        public FileInfo SourceFileInfo;
        public FileInfo DestFileInfo;
        public ImageFormat ImageFormat;
        public long ColorDepth;
        public long Quality;
        public Image Thumbnail;

        public ConverterResult()
        {

        }

        public ConverterResult(bool success, string errorMessage, string source, string dest, int sourcewidth, int sourceheight, int destwidth, int destheight, FileInfo sourceFileInfo, FileInfo destFileInfo, ImageFormat format, long quality, long depth, Image thumbnail)
        {
            Success = success;
            ErrorMessage = errorMessage;
            SourcePath = source;
            DestPath = dest;
            SourceWidth = sourcewidth;
            SourceHeight = sourceheight;
            DestWidth = destwidth;
            DestHeight = destheight;
            SourceFileInfo = sourceFileInfo;
            DestFileInfo = destFileInfo;
            ImageFormat = format;
            Quality = quality;
            ColorDepth = depth;
            Thumbnail = thumbnail;
        }

    }
}
