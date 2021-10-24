using System;
using System.Collections.Generic;
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
        public ConverterResult ConverterResult;
        public FileInfo FileInfo;

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
