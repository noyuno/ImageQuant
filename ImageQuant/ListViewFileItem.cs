using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageQuant
{
    class ListViewFileItem : ListViewItem
    {
        [Category("変換結果")]
        public ConverterResult ConverterResult { get; set; }

        [Category("ファイル情報")]
        public FileInfo FileInfo { get; set; }

        public ListViewFileItem():base()
        {
        }

        public ListViewFileItem(ConverterResult converterResult, string imageKey)
            :base(Path.GetFileName(converterResult.DestPath), imageKey)
        {
            ConverterResult = converterResult;
            FileInfo = ConverterResult.DestFileInfo;
            if (ConverterResult.Success)
            {
                base.ForeColor = Color.Blue;
            }
            else
            {
                base.ForeColor = Color.Red;
            }
        }

        public ListViewFileItem(FileInfo fileInfo, string imageKey)
            :base(fileInfo.Name, imageKey)
        {
            FileInfo = fileInfo;
        }
    }
}
