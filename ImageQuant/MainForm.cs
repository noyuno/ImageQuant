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

namespace ImageQuant
{
    public partial class MainForm : Form
    {
        public Converter converter;
        public bool recursive = true;

        private int _ThumbnailSize;
        public int ThumbnailSize
        {
            set { listView.LargeImageList.ImageSize = new Size(value, value); _ThumbnailSize = value; }
            get { return _ThumbnailSize; }
        }

        List<ConverterResult> converterResultList;

        TabPageManager tabPageManager;

        public MainForm()
        {
            InitializeComponent();
            converter = new Converter();
            converterResultList = new List<ConverterResult>();
            pictureBox.AllowDrop = true;

            listView.LargeImageList = new ImageList();
            listView.LargeImageList.ColorDepth = ColorDepth.Depth32Bit;
            listView.LargeImageList.TransparentColor = Color.Transparent;
            ThumbnailSize = Settings.Default.ManualThumbnailSize ? Settings.Default.ThumbnailSize : 100;
            //pathToolStripCombo.ComboBox.Items.AddRange(RefreshRecentoryDirectory());
            //pathToolStripCombo.ComboBox.Items.Add(converter.TempDir);
            //pathToolStripCombo.ComboBox.SelectedItem = converter.TempDir;
            if (Settings.Default.Preview)
            {
                //tableLayoutPanel.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 50f);
                splitContainer1.SplitterDistance = this.Width / 2;
            }
            else
            {
                //tableLayoutPanel.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 0f);
                splitContainer1.SplitterDistance = this.Width;
            }
            pathToolStripCombo.Text = converter.TempDir;

            openToolStripButton.Visible = false;
            aboveToolStripButton.Visible = false;
            mkdirToolStripButton.Visible = false;

            UpdateButtonEnable();

            tabPageManager = new TabPageManager(tabControl);
            UpdatePropertyGrid();
        }

        private void UpdateButtonEnable()
        {
            toolStripProgressBar.Visible = false;
            if (converter.DestDir == "")
            {
                this.Text = Application.ProductName + "(入力待ち・一時フォルダ)";
                toolStripStatusLabel.Text = "画像をこのウィンドウにドラッグ＆ドロップしてサイズ変換します。";
                copyToolStripButton.Enabled = false;
                pasteToolStripButton.Enabled = false;
                trashToolStripButton.Enabled = false;
                //aboveToolStripButton.Enabled = false;
                zipToolStripButton.Enabled = false;
                attachMailToolStripButton.Enabled = false;
                exportExcelToolStripButton.Enabled = false;
            }
            else
            {
                //aboveToolStripButton.Enabled = true;

                //if (listView.Items.Count == 0)
                //{
                //    toolStripStatusLabel.Text = "画像をこのウィンドウにドラッグ＆ドロップしてサイズ変換します。";
                //    copyToolStripButton.Enabled = false;
                //    trashToolStripButton.Enabled = false;
                //    zipToolStripButton.Enabled = false;
                //    attachMailToolStripButton.Enabled = false;
                //    exportExcelToolStripButton.Enabled = false;
                //}
                //else
                //{
                pathToolStripCombo.Text = converter.DestDirChild;
                    if (listView.CheckedItems.Count == 0 && listView.SelectedItems.Count == 0)
                    {
                        this.Text = Application.ProductName + "(変換完了)";
                        //var counter = listView.Items.Cast<ListViewFileItem>().Where(x => x.ConverterResult != null && x.ConverterResult.Success).Count();
                        var counter = converterResultList.Count;
                        toolStripStatusLabel.Text = $"{counter}枚変換完了しました(青文字)。画像を選択/チェックしてください。";
                        //toolStripStatusLabel.Text = $"{converterResultList.Count}枚変換完了しました。画像を選択/チェックしてください。";
                        copyToolStripButton.Enabled = false;
                        trashToolStripButton.Enabled = false;
                        zipToolStripButton.Enabled = false;
                        attachMailToolStripButton.Enabled = false;
                        exportExcelToolStripButton.Enabled = false;
                    }
                    else
                    {
                        this.Text = Application.ProductName + "(変換完了)";
                        toolStripStatusLabel.Text = "選択した画像をメールに添付したりエクセルにエクスポートできます。";
                        copyToolStripButton.Enabled = true;
                        trashToolStripButton.Enabled = true;
                        zipToolStripButton.Enabled = true;
                        attachMailToolStripButton.Enabled = true;
                        exportExcelToolStripButton.Enabled = true;
                    }
                //}

            }
            // todo: read clipboard
        }

        //private object[] RefreshRecentoryDirectory()
        //{
        //    object[] ret = new object[] { };
        //    if (Settings.Default.RecentlyDirectory == null)
        //    {
        //        Settings.Default.RecentlyDirectory = new StringCollection();
        //    }
        //    if(Settings.Default.SaveRecentlyDirectory)
        //    {
        //        int p = Settings.Default.RecentlyDirectory.Count;
        //        for (int i = 0; i < p; i++)
        //        {
        //            if (Directory.Exists(Settings.Default.RecentlyDirectory[i]))
        //            {
        //                ret.Append(Settings.Default.RecentlyDirectory[i]);
        //            }
        //            else
        //            {
        //                Settings.Default.RecentlyDirectory.RemoveAt(i--);
        //                p--;
        //            }
                    
        //        }
        //    }
        //    else
        //    {
        //        Settings.Default.RecentlyDirectory.Clear();
        //    }
            
        //    return ret;

        //}

        private async void ConvertFiles(string[] filenames)
        {
            //converterResultList.Clear();
            toolStripProgressBar.Visible = true;
            toolStripProgressBar.Maximum = filenames.Length;
            toolStripProgressBar.Value = 0;

            var action = new Action<int, ConverterResult>((count, r) =>
            {
                toolStripProgressBar.Value = count;
            });

            ConverterResult[] ret = await ConvertImages(filenames, action);
            foreach (var r in ret)
            {
                if (r.Success)
                {
                    if (!listView.Items.Cast<ListViewFileItem>().ToList().Exists(x => x.FileInfo.FullName == r.DestFileInfo.FullName))
                    {
                        listView.LargeImageList.Images.Add(r.DestPath, r.Thumbnail);
                        var item = new ListViewFileItem(r, r.DestPath);
                        listView.Items.Add(item);
                    }
                }
            }
            converterResultList.AddRange(ret);
            toolStripProgressBar.Visible = false;
            UpdateButtonEnable();

        }

        private async Task<ConverterResult[]> ConvertImages(string[] filenames, Action<int, ConverterResult> action)
        {
            var tasks = new List<Task<ConverterResult>>();
            var done = 0;
            for (var i = 0; i < filenames.Length; i++)
            {
                var x = i;
                var task = Task.Run<ConverterResult>(() =>
                {
                    var ret = converter.Convert(filenames[x]);
                    this.Invoke(action, ++done, ret);
                    return ret;
                });
                tasks.Add(task);
            }
            return await Task.WhenAll(tasks);
        }

        //private void ConvertFiles(string[] items)
        //{
        //    // bug: do not wait running tasks
        //    //var ret = ConvertImages(items);
        //    //if (ret.IsCompleted)
        //    //{
        //    //    MessageBox.Show("完了しました");
        //    //}
        //    //else if (ret.IsFaulted)
        //    //{
        //    //    MessageBox.Show("失敗しました");
        //    //}
        //    //else if (ret.IsCanceled)
        //    //{
        //    //    MessageBox.Show("キャンセルされました");
        //    //}
        //    //else
        //    //{
        //    //    throw new NotImplementedException();
        //    //}
        //    ConvertImages(items);
        //    InitButtonEnable();
        //}


        //private void UpdateListView()
        //{
        //    listView.Items.Clear();
        //    listView.LargeImageList.Images.Clear();

        //    if (Directory.Exists(converter.DestDirChild))
        //    {
        //        List<string> files = Directory.GetFiles(converter.DestDirChild).ToList();
        //        foreach (String file in files)
        //        {
        //            listView.LargeImageList.Images.Add(file, QImaging.GetThumbnail(file));
        //            if (converterResultList.Exists(x => x.DestFileInfo.FullName == file))
        //            {
        //                listView.Items.Add(new ListViewFileItem(converterResultList.Last(x => x.DestFileInfo.FullName == file), file));
        //            }
        //            else
        //            {
        //                listView.Items.Add(new ListViewFileItem(new FileInfo(file), file));
        //            }
        //        }

        //    }
        //}

        private async void UpdateListView(string path)
        {
            listView.Items.Clear();
            listView.LargeImageList.Images.Clear();
            List<string> files = Directory.GetFiles(path).ToList();
            toolStripProgressBar.Maximum = files.Count;
            toolStripProgressBar.Value = 0;
            toolStripProgressBar.Visible = true;
            var action = new Action<int, ListViewFileItem>((count, r) =>
            {
                toolStripProgressBar.Value = count;
            });
            ListViewFileItem[] ret = await UpdateListView2(files, action);
            foreach (var r in ret)
            {
                listView.LargeImageList.Images.Add(r.FileInfo.FullName, r.Thumbnail);
                listView.Items.Add(r);
            }
            toolStripProgressBar.Visible = false;
            UpdateButtonEnable();
        }

        private async Task<ListViewFileItem[]> UpdateListView2(List<string> files, Action<int, ListViewFileItem> action)
        {
            var tasks = new List<Task<ListViewFileItem>>();
            var done = 0;
            for (int i = 0; i < files.Count; i++)
            {
                var x = i;
                var task = Task.Run<ListViewFileItem>(() =>
                  {
                      ListViewFileItem ret = null;
                      if (converterResultList.Exists(y => y.DestFileInfo.FullName == files[x]))
                      {
                          ret= new ListViewFileItem(converterResultList.Last(y => y.DestFileInfo.FullName == files[x]), QImaging.GetThumbnail(files[x]));
                      }
                      else
                      {
                          ret= new ListViewFileItem(new FileInfo(files[x]), QImaging.GetThumbnail(files[x]));
                      }
                      this.Invoke(action, ++done, ret);
                      return ret;
                  });
                tasks.Add(task);
            }
            return await Task.WhenAll(tasks);
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (converter != null)
            {
                converter.Dispose();
            }
        }


        private void listView_DragDrop(object sender, DragEventArgs e)
        {
            ImageDragDrop(e);
        }

        private void ImageDragDrop(DragEventArgs e)
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
                //if (Directory.Exists(converter.DestDir))
                //{
                //    RefreshListView();
                //}
                var items = Directory.GetFiles(converter.DestDir);

                ConvertFiles(items);
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
                ConvertFiles(files);
            }
            UpdateListView();
        }

        private void listView_DragEnter(object sender, DragEventArgs e)
        {
            ImageDragEnter(e);
        }

        private static void ImageDragEnter(DragEventArgs e)
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
            ImageDragEnter(e);
        }

        private void pictureBox_DragDrop(object sender, DragEventArgs e)
        {
            ImageDragDrop(e);
        }

        private void listView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UpdateButtonEnable();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            //var dialog = new CommonOpenFileDialog("フォルダ選択");
            //dialog.IsFolderPicker = true;
            //if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            //{
            //    pathToolStripCombo.ComboBox.Items.Add(dialog.FileName);
            //    pathToolStripCombo.ComboBox.SelectedItem =
            //    dialog.FileName;
            //}
            throw new NotImplementedException();
        }

        private void selectAllToolStripButton_Click(object sender, EventArgs e)
        {
            var checkedFlag = false;
            foreach (ListViewItem item in listView.Items)
            {
                if (!item.Checked)
                {
                    item.Checked = true;
                    checkedFlag = true;
                }
            }
            if (!checkedFlag)
            {
                foreach (ListViewItem item in listView.Items)
                {
                    item.Checked = false;
                }
            }
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
            //converter.DestDir = Path.GetDirectoryName(converter.DestDir);
            //RefreshListView();
            throw new NotImplementedException();
        }

        private void pathToolStripCombo_Click(object sender, EventArgs e)
        {
            pathToolStripCombo.SelectAll();
        }

        //private void pathToolStripCombo_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (converter.TempDir != pathToolStripCombo.ComboBox.SelectedItem.ToString())
        //    {
        //        converter.DestDir = pathToolStripCombo.ComboBox.SelectedItem.ToString();
        //        InitButtonEnable();
        //        RefreshListView();
        //    }
        //}

        private void mkdirToolStripButton_Click(object sender, EventArgs e)
        {
            //var targetDir = Path.Combine(converter.DestDirChild, "新しいフォルダ");
            //var t = targetDir;
            //var i = 2;
            //while(Directory.Exists(targetDir) || File.Exists(targetDir))
            //{
            //    targetDir = t + $"({i++}";
            //}
            //Directory.CreateDirectory(targetDir);
            //UpdateListView();
            //listView.Items.Cast<ListViewFileItem>().First(x => x.FileInfo.FullName == targetDir).BeginEdit();
        }

        private void explorerToolStripButton_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                Process.Start("explorer.exe", "/select," + ((ListViewFileItem)listView.SelectedItems[0]).FileInfo.FullName);
            }
            else
            {
                var d = Directory.Exists(converter.DestDirChild) ? converter.DestDirChild : Path.GetDirectoryName(converter.DestDirChild);
                Process.Start("explorer.exe", d);
            }
        }

        private void zipToolStripButton_Click(object sender, EventArgs e)
        {
            var zipfile = Execution.Zip(GetSelectedItems());
            if (zipfile !="")
            {
                UpdateListView();
                Process.Start("explorer.exe", "/select," + zipfile);
            }
        }

        private void attachMailToolStripButton_Click(object sender, EventArgs e)
        {
            if (Settings.Default.Mailer == "outlook")
            {

            Execution.SendMailOutlook(GetSelectedItems());
            }
            else if (Settings.Default.Mailer=="mailto")
            {
                Execution.SendMailMailto(GetSelectedItems());

            }
        }

        private string[] GetSelectedItems()
        {
            IEnumerable<ListViewFileItem> enu = listView.CheckedItems.Count == 0 ?
                listView.SelectedItems.Cast<ListViewFileItem>() :
                listView.CheckedItems.Cast<ListViewFileItem>();
            var paths = Enumerable.Select(enu, (item) => { return item.FileInfo.FullName; }).ToArray();
            return paths;
        }

        private void exportExcelToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void settingsToolStripButton_Click(object sender, EventArgs e)
        {
            var form = new SettingForm(this);
            form.ShowDialog();
        }


        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtonEnable();
            UpdatePictureBox();
            UpdatePropertyGrid();
        }

        private void UpdatePictureBox()
        {
            if (listView.SelectedItems.Count > 0)
            {
                var path = ((ListViewFileItem)listView.SelectedItems[0]).FileInfo.FullName;
                if (QImaging.GetFileType(path) == QFileType.Unknown)
                {
                    if (pictureBox.Image != null)
                    {
                        pictureBox.Image.Dispose();
                        pictureBox.Image = null;
                    }
                }
                else
                {
                    pictureBox.ImageLocation = path;
                }
            }
            else
            {
                if (pictureBox.Image != null)
                {
                    pictureBox.Image.Dispose();
                    pictureBox.Image = null;
                }
            }
        }

        private void UpdatePropertyGrid()
        {
            if (listView.SelectedItems.Count > 0)
            {
                //propertyGrid.PropertyTabs.AddTabType()
                fileInfoPropertyGrid.SelectedObject = ((ListViewFileItem)listView.SelectedItems[0]).FileInfo;
                if (((ListViewFileItem)listView.SelectedItems[0]).ConverterResult == null)
                {

                    tabPageManager.ChangeTabPageVisible(1, false);
                }
                else
                {
                    tabPageManager.ChangeTabPageVisible(1, true);
                    resultPropertyGrid.SelectedObject = ((ListViewFileItem)listView.SelectedItems[0]).ConverterResult;
                }
            }
            else
            {
                tabPageManager.ChangeTabPageVisible(1, false);
                fileInfoPropertyGrid.SelectedObject = null;
                resultPropertyGrid.SelectedObject = null;
            }
        }

        private void listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                Process.Start(((ListViewFileItem)listView.SelectedItems[0]).FileInfo.FullName);
            }
        }

        private void listView_KeyUp(object sender, KeyEventArgs e)
        {
            ListView lv = (ListView)sender;
            //F2キーが離されたときは、フォーカスのあるアイテムの編集を開始
            if (e.KeyCode == Keys.F2 && lv.FocusedItem != null && lv.LabelEdit)
            {
                lv.FocusedItem.BeginEdit();
            }
        }

        private void listView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            //ラベルが変更されたか調べる
            //e.Labelがnullならば、変更されていない
            if (e.Label != null)
            {
                ListView lv = (ListView)sender;
                //同名のアイテムがあるか調べる
                foreach (ListViewFileItem lvi in lv.Items)
                {
                    //同名のアイテムがあるときは編集をキャンセルする
                    if (lvi.Index != e.Item && lvi.Text == e.Label)
                    {
                        MessageBox.Show("同名のアイテムがすでにあります。");
                        //編集をキャンセルして元に戻す
                        e.CancelEdit = true;
                        return;
                    }
                }
                var lvfi = ((ListViewFileItem)lv.Items[e.Item]);
                var newFilename = Path.Combine(lvfi.FileInfo.DirectoryName, e.Label);
                try
                {
                    File.Move(lvfi.FileInfo.FullName, newFilename);
                    lvfi.FileInfo = new FileInfo(newFilename);
                }
                catch(Exception ex)
                {
                    e.CancelEdit = true;
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            UpdateListView();
        }
    }
}
