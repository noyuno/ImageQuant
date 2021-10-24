using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Category("0_状態")]
        public bool Success { get; }

        [Category("0_状態")]
        public string ErrorMessage { get; }

        [Category("1_入力")]
        public string SourcePath { get; }

        [Category("2_出力")]
        public string DestPath { get; }

        [Category("1_入力")]
        public string SourceSize { get { return new Size(SourceWidth, SourceHeight).ToString(); } }

        public int SourceWidth;
        public int SourceHeight;

        [Category("2_出力")]
        public string DestSize { get { return new Size(DestWidth, DestHeight).ToString(); } }

        public int DestWidth;
        public int DestHeight;

        public FileInfo SourceFileInfo;
        public FileInfo DestFileInfo;

        [Category("1_入力")]
        public string SourceFileSize { get { return QImaging.FileSizeToString(SourceFileInfo.Length); } }

        [Category("2_出力")]
        public string DestFileSize { get { return QImaging.FileSizeToString(DestFileInfo.Length); } }

        [Category("2_出力")]
        public QFileType QFileType { get; }

        [Category("2_出力")]
        public long ColorDepth { get; }

        [Category("2_出力")]
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
            QFileType = format;
            Quality = quality;
            ColorDepth = depth;
            Thumbnail = thumbnail;
        }

    }
}
