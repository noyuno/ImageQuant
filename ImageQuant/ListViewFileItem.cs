using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ImageQuant
{
    class ListViewFileItem : ListViewItem
    {
        public bool Success;

        private string _FileName;
        public string FileName
        {
            get
            {
                if (FileInfo != null)
                {
                    return FileInfo.FullName;
                }
                else
                {
                    return _FileName;
                }
            }
        }

        public string Message;

        [Category("変換結果")]
        public ConverterResult ConverterResult { get; set; }

        [Category("ファイル情報")]
        public FileInfo FileInfo { get; set; }

        public Image Thumbnail { get; set; }

        public int ThumbnailFallback { get; set; }

        public ListViewFileItem():base()
        {
        }

        public ListViewFileItem(bool success, string filename, string message) : base()
        {
            Success = success;
            _FileName = filename;
            Message = message;
        }

        public ListViewFileItem(ConverterResult converterResult)
    : base(Path.GetFileName(converterResult.DestPath), converterResult.DestFileInfo.FullName)
        {
            InitFromConverterResult(converterResult);
        }

        //public ListViewFileItem(ConverterResult converterResult)
        //    :base(Path.GetFileName(converterResult.DestPath), converterResult.DestFileInfo.FullName)
        //{
        //    Init(converterResult);
        //}

        private void InitFromConverterResult(ConverterResult converterResult)
        {
            ConverterResult = converterResult;
            FileInfo = ConverterResult.DestFileInfo;

            var sourceSize = QImaging.FileSizeToString(ConverterResult.SourceFileInfo.Length);
            var destSize = QImaging.FileSizeToString(FileInfo.Length);
            var ratio = (double)FileInfo.Length / ConverterResult.SourceFileInfo.Length * 100 - 100;
            ToolTipText = $"{FileInfo.Name}\r\n{sourceSize}->{destSize}({ratio:F1}%)\r\n";
            if (ratio < 100)
            {
                base.ForeColor = Color.Blue;
            }
            else
            {
                base.ForeColor = Color.Red;
            }            
        }

        public ListViewFileItem(FileInfo fileInfo)
            : base(fileInfo.Name, fileInfo.FullName)
        {
            FileInfo = fileInfo;
            ToolTipText = $"{FileInfo.Name}\r\n{QImaging.FileSizeToString(FileInfo.Length)}\r\n";
        }

        //public ListViewFileItem(FileInfo fileInfo)
        //    :base(fileInfo.Name, fileInfo.FullName)
        //{
        //    FileInfo = fileInfo;
        //}
    }
}
