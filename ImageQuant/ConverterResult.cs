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
        public bool Success { get; }
        public string ErrorMessage { get; }
        public string SourcePath { get; }
        public string DestPath { get; }
        public int SourceWidth { get; }
        public int SourceHeight { get; }
        public int DestWidth { get; }
        public int DestHeight { get; }
        public FileInfo SourceFileInfo;
        public FileInfo DestFileInfo;
        public QFileType ImageFormat { get; }
        public long ColorDepth { get; }
        public long Quality { get; }
        public Image Thumbnail;

        public ConverterResult()
        {

        }

        public ConverterResult(bool success, string errorMessage, string source, string dest, int sourcewidth, int sourceheight, int destwidth, int destheight, FileInfo sourceFileInfo, FileInfo destFileInfo, QFileType format, long quality, long depth, Image thumbnail)
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
