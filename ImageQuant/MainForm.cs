using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using ImageQuant.Properties;
using System.Collections.Specialized;
using Microsoft.WindowsAPICodePack.Dialogs;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace ImageQuant
{
    public partial class MainForm : Form
    {
        public Converter converter;
        public bool recursive = true;
        public int ThumbnailSize = Properties.Settings.Default.ThumbnailSize;
        

        public MainForm()
        {
            InitializeComponent();
            converter = new Converter();


            pathToolStripCombo.ComboBox.Items.AddRange(RefreshRecentoryDirectory());
            Settings.Default.RecentlyDirectory.Add(converter.TempDir);
            pathToolStripCombo.ComboBox.SelectedItem = converter.TempDir;

            initButtonEnable();
        }

        private void initButtonEnable()
        {
            toolStripProgressBar.Visible = false;
            if (converter.DestDir == "")
            {
                toolStripStatusLabel.Text = "画像をこのウィンドウにドラッグ＆ドロップしてサイズ変換します。";
                copyToolStripButton.Enabled = false;
                pasteToolStripButton.Enabled = false;
                trashToolStripButton.Enabled = false;
                aboveToolStripButton.Enabled = false;
                zipToolStripButton.Enabled = false;
                attachMailToolStripButton.Enabled = false;
                exportExcelToolStripButton.Enabled = false;
            }
            else
            {
                aboveToolStripButton.Enabled = true;

                if (listView.Items.Count == 0)
                {
                    toolStripStatusLabel.Text = "画像をこのウィンドウにドラッグ＆ドロップしてサイズ変換します。";
                    copyToolStripButton.Enabled = false;
                    trashToolStripButton.Enabled = false;
                    zipToolStripButton.Enabled = false;
                    attachMailToolStripButton.Enabled = false;
                    exportExcelToolStripButton.Enabled = false;
                }
                else
                {
                    if (listView.SelectedItems.Count == 0)
                    {
                        toolStripStatusLabel.Text = "何か画像を選択してください。";
                        copyToolStripButton.Enabled = false;
                        trashToolStripButton.Enabled = false;
                        zipToolStripButton.Enabled = false;
                        attachMailToolStripButton.Enabled = false;
                        exportExcelToolStripButton.Enabled = false;
                    }
                    else
                    {
                        toolStripStatusLabel.Text = "選択した画像をメールに添付したりエクセルにエクスポートできます。";
                        copyToolStripButton.Enabled = true;
                        trashToolStripButton.Enabled = true;
                        zipToolStripButton.Enabled = true;
                        attachMailToolStripButton.Enabled = true;
                        exportExcelToolStripButton.Enabled = true;
                    }
                }

            }
            // todo: read clipboard
        }

        private object[] RefreshRecentoryDirectory()
        {
            object[] ret = new object[] { };
            if (Settings.Default.RecentlyDirectory == null)
            {
                Settings.Default.RecentlyDirectory = new StringCollection();
            }
            if(Settings.Default.SaveRecentlyDirectory)
            {
                int p = Settings.Default.RecentlyDirectory.Count;
                for (int i = 0; i < p; i++)
                {
                    if (Directory.Exists(Settings.Default.RecentlyDirectory[i]))
                    {
                        ret.Append(Settings.Default.RecentlyDirectory[i]);
                    }
                    else
                    {
                        Settings.Default.RecentlyDirectory.RemoveAt(i--);
                        p--;
                    }
                    
                }
            }
            else
            {
                Settings.Default.RecentlyDirectory.Clear();
            }
            
            return ret;

        }

        private Task convertImages(string[] filenames)
        {
            var tasks = new List<Task>();
            toolStripProgressBar.Maximum = filenames.Length;
            var progress = new Progress<int>((count) => {
                toolStripProgressBar.Value = count;
                });
            var done = 0;
            for (var i = 0; i < filenames.Length; i++)
            {
                var x = i;
                var task = Task.Run(() => {
                    var dest = converter.Convert(filenames[x]);
                    if (dest == "")
                    {
                        // cancel
                    }
                    else
                    {
                        ((IProgress<int>)progress).Report(++done);
                        listView.LargeImageList.Images.Add(dest, FileImaging.GetThumbnail(dest));
                        listView.Items.Add(dest, Path.GetFileName(dest), dest);
                    }
                    });
                tasks.Add(task);
            }
            return Task.WhenAll(tasks);
        }

        private void convertFiles(string[] items)
        {
            var ret = convertImages(items);
            if (ret.IsCompleted)
            {
                MessageBox.Show("完了しました");
            }
            else if (ret.IsFaulted)
            {
                MessageBox.Show("失敗しました");
            }
            else if (ret.IsCanceled)
            {
                MessageBox.Show("キャンセルされました");
            }
        }

        private string getFileSize(long fileSize)
        {
            string ret = fileSize + "B";
            if (fileSize > (1024f * 1024f * 1024f))
            {
                ret = Math.Round((fileSize / 1024f / 1024f / 1024f), 2).ToString() + "GB";
            }
            else if (fileSize > (1024f * 1024f))
            {
                ret = Math.Round((fileSize / 1024f / 1024f), 2).ToString() + "MB";
            }
            else if (fileSize > 1024f)
            {
                ret = Math.Round((fileSize / 1024f)).ToString() + "KB";
            }

            return ret;
        }

        private void refreshListView()
        {
            List<string> files = Directory.GetFiles(converter.DestDir).ToList();
            foreach (String file in files)
            {
                FileInfo info = new FileInfo(file);
                ListViewItem item = new ListViewItem(info.Name);
                item.SubItems.Add(getFileSize(info.Length));
                listView.Items.Add(item);
            }
        }

        private void sendMail()
        {
            var ol = new Outlook.Application();
            Outlook.MailItem mail = ol.CreateItem(Outlook.OlItemType.olMailItem) as Outlook.MailItem;
            foreach (ImageItem item in listView.SelectedItems)
            {
                mail.Attachments.Add(item.DestPath);
            }
            mail.Display();
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (converter != null)
            {
                converter.Dispose();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void listView_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = null;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            }
            else if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                var bitmap = (Bitmap)e.Data.GetData(DataFormats.Bitmap, false);
                var filename = converter.SaveBmp(bitmap);
                files = new string[] { filename };
            }
            else
            {
                throw new NotImplementedException();
            }

            if (converter.DestDir == "" && files.Length == 1 && Directory.Exists(files[0]))
            {
                // expand a directory
                //converter.DestDir = files[0] + @"\" + converter.DestDirName;
                converter.DestDir = files[0];
                if (Directory.Exists(converter.DestDir))
                {
                    refreshListView();
                }
                var items = Directory.GetFiles(converter.DestDir);

                convertFiles(items);
            }
            else
            {
                // multiple files
                if (converter.DestDir == "")
                {
                    converter.DestDir = Path.GetDirectoryName(files[0]);
                }
                //listView.Items.AddRange(Array.ConvertAll(
                //    files, new Converter<string, ListViewItem>((s) => {
                //        return new ListViewItem(new ImageItem(s));
                //    })));
                convertFiles(files);
            }
        }

        private void listView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) ||
                e.Data.GetDataPresent(DataFormats.Bitmap) ||
                e.Data.GetDataPresent(DataFormats.Tiff))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void pictureBox_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void pictureBox_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void listView_DoubleClick(object sender, EventArgs e)
        {

        }

        private void listView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {

        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            var dialog = new CommonOpenFileDialog("フォルダ選択");
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                pathToolStripCombo.ComboBox.Items.Add(dialog.FileName);
                pathToolStripCombo.ComboBox.SelectedItem = dialog.FileName;
            }
        }

        private void selectAllToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void trashToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void aboveToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void pathToolStripCombo_Click(object sender, EventArgs e)
        {

        }

        private void pathToolStripCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            converter.DestDir = pathToolStripCombo.ComboBox.SelectedItem.ToString();
            initButtonEnable();
            refreshListView();
        }

        private void mkdirToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void explorerToolStripButton_Click(object sender, EventArgs e)
        {
            Process.Start(converter.DestDir);
        }

        private void zipToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void attachMailToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void exportExcelToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void settingsToolStripButton_Click(object sender, EventArgs e)
        {
            var form = new SettingForm(this);
            form.ShowDialog();
        }
    }
}
