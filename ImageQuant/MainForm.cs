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
#pragma warning disable IDE1006 // 命名スタイル
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

        private List<ConverterResult> converterResultList=new List<ConverterResult>();

        private readonly TabPageManager tabPageManager;

        //bool TabControlUserActive = true;
        bool UserActive = true;

        private ToolStripSpringTextBox pathToolStripTextBox;

        public MainForm()
        {


            InitializeComponent();
            pictureBox.AllowDrop = true;
            pathToolStripTextBox = new ToolStripSpringTextBox();
            pathToolStripTextBox.Click += pathToolStripTextBox_Click;
            toolStrip2.Items.Add(pathToolStripTextBox);

            InitializeInstance();

            //converter = new Converter();




            //listView.LargeImageList = new ImageList
            //{
            //    ColorDepth = ColorDepth.Depth32Bit,
            //    TransparentColor = Color.Transparent
            //};
            //pathToolStripTextBox.ComboBox.Items.AddRange(RefreshRecentoryDirectory());
            //pathToolStripTextBox.ComboBox.Items.Add(converter.TempDir);
            //pathToolStripTextBox.ComboBox.SelectedItem = converter.TempDir;
            tabPageManager= new TabPageManager(tabControl);

            aboveToolStripButton.Visible = false;
            mkdirToolStripButton.Visible = false;


            UpdateButtonEnable();

            UpdatePropertyGrid();

            InitializeUI();

        }

        private void MainForm_Shown(object sender, EventArgs e)
        {

        }


        private void closeToolStripButton_Click(object sender, EventArgs e)
        {
            InitializeInstance();
            UpdateButtonEnable();
            UpdatePropertyGrid();
        }

        private void InitializeInstance()
        {
            converter?.Dispose();
            converter = new Converter();
            pathToolStripTextBox.Text = converter.TempDir;
            converterResultList?.Clear();
            converterResultList = new List<ConverterResult>();
            listView.Clear();
            listView.LargeImageList?.Dispose();
            listView.LargeImageList = new ImageList
            {
                ColorDepth = ColorDepth.Depth32Bit,
                TransparentColor = Color.Transparent
            };
            ThumbnailSize = Settings.Default.ManualThumbnailSize ? Settings.Default.ThumbnailSize : 100;

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Save();
            converter?.Dispose();
        }


        public void InitializeUI()
        { 
            UserActive = false;
            //if (Settings.Default.Preview)
            //{
            //    //tableLayoutPanel.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 50f);
            //    splitContainer.SplitterDistance = this.Width / 2;
            //}
            //else
            //{
            //    //tableLayoutPanel.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 0f);
            //    splitContainer.SplitterDistance = this.Width;
            //}

            this.Size = Settings.Default.FormSize;
            splitContainer.SplitterDistance = Settings.Default.ListViewWidth;
            pictureBox.Size = new Size(pictureBox.Size.Width, Settings.Default.PictureBoxHeight);
            UserActive = true;

        }

        private void UpdateButtonEnable()
        {
            toolStripProgressBar.Visible = false;
            if (converter.DestDir == "")
            {
                this.Text = Application.ProductName + "(入力待ち・一時フォルダ)";
                toolStripStatusLabel.Text = "画像をこのウィンドウにドラッグ＆ドロップしてサイズ変換します。";
                copyToolStripButton.Enabled = false;
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
                pathToolStripTextBox.Text = converter.DestDirChild;
                    if (listView.CheckedItems.Count == 0 && listView.SelectedItems.Count == 0)
                    {
                        this.Text = Application.ProductName + "(変換完了)";
                        //var counter = listView.Items.Cast<ListViewFileItem>().Where(x => x.ConverterResult != null && x.ConverterResult.Success).Count();
                        var counter = converterResultList.Where((x)=>x.Success).Count();
                        toolStripStatusLabel.Text = $"{counter}枚変換完了しました(青・赤文字)。画像を選択/チェックしてください。";
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
            var data = Clipboard.GetDataObject();
            pasteToolStripButton.Enabled = (
                data.GetDataPresent(DataFormats.Bitmap) ||
                data.GetDataPresent(DataFormats.FileDrop));
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

        private async void ConvertFiles(List<string> filenames)
        {
            //converterResultList.Clear();
            toolStripProgressBar.Visible = true;
            toolStripProgressBar.Maximum = filenames.Count;
            toolStripProgressBar.Value = 0;

            ConverterResult[] ret = await ConvertImages(filenames, (count, r) =>
            {
                toolStripProgressBar.Value = count;
            });
            //foreach (var r in ret)
            //{
            //    if (r.Success)
            //    {
            //        if (!listView.Items.Cast<ListViewFileItem>().ToList().Exists(x => x.FileInfo.FullName == r.DestFileInfo.FullName))
            //        {
            //            listView.LargeImageList.Images.Add(r.DestPath, r.Thumbnail);
            //            var item = new ListViewFileItem(r, QImaging.GetThumbnail(r.DestFileInfo.FullName));
            //            listView.Items.Add(item);
            //        }
            //    }
            //}
            converterResultList.AddRange(ret);
            var failedret = ret.Where((x) => !x.Success);
            if (failedret.Count() > 0)
            {
                var faileditems = string.Join("\r\n",
                    failedret.Select((x) => $"{x.SourceFileInfo.FullName}:{x.ErrorMessage}").ToArray());
                MessageBox.Show($"{ret.Length}ファイルのうち、次のファイル(n={failedret.Count()})は変換できませんでした。\r\n{faileditems}",
                    "変換失敗", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

            toolStripProgressBar.Visible = false;
            UpdateListView();
            UpdateButtonEnable();

        }

        private async Task<ConverterResult[]> ConvertImages(List<string> filenames, Action<int, ConverterResult> action)
        {
            var tasks = new List<Task<ConverterResult>>();
            var done = 0;
            for (var i = 0; i < filenames.Count; i++)
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

        public async void UpdateListView(string path = null)
        {
            path ??= converter.DestDirChildE;
            string[] files = Directory.GetFiles(path);
            toolStripProgressBar.Maximum = files.Length;
            toolStripProgressBar.Value = 0;
            toolStripProgressBar.Visible = true;
            ListViewFileItem[] ret = await UpdateListView2(files, (count, r) =>
            {
                toolStripProgressBar.Value = count;
            });
            // store selected/checked items
            var checkeditems = listView.CheckedItems.Cast<ListViewFileItem>()
                .Select((x) => x.FileInfo.FullName).ToArray();
            var selecteditems = listView.SelectedItems.Cast<ListViewFileItem>()
                .Select((x) => x.FileInfo.FullName).ToArray();
            listView.Items.Clear();
            listView.LargeImageList.Images.Clear();
            listView.Items.AddRange(ret);
            foreach (var r in ret)
            {
                listView.LargeImageList.Images.Add(r.FileInfo.FullName, r.Thumbnail);
                r.Checked = checkeditems.Contains(r.FileInfo.FullName);
                r.Selected = selecteditems.Contains(r.FileInfo.FullName);

            }
            toolStripProgressBar.Visible = false;
            listView.ShowItemToolTips = true;
            UpdateButtonEnable();
        }

        private async Task<ListViewFileItem[]> UpdateListView2(string[] files, Action<int, ListViewFileItem> action)
        {
            var tasks = new List<Task<ListViewFileItem>>();
            var done = 0;
            for (int i = 0; i < files.Length; i++)
            {
                var x = i;
                var task = Task.Run<ListViewFileItem>(() =>
                  {
                      ListViewFileItem ret = null;
                      Func<ConverterResult, bool> cond = (y) =>  (y.DestFileInfo != null && y.DestFileInfo.FullName == files[x]);
                      if (converterResultList.Exists(y => cond(y)))
                      {
                          ret= new ListViewFileItem(converterResultList.Last(cond), QImaging.GetThumbnail(files[x]));
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




        private void ListView_DragDrop(object sender, DragEventArgs e)
        {
            ImageDragDrop(e);
        }

        private void ImageDragDrop(DragEventArgs e)
        {
            string[] files;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                files = (string[])e.Data.GetData(DataFormats.FileDrop, true);
            }
            else if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                var bitmap = (Bitmap)e.Data.GetData(DataFormats.Bitmap, true);
                var filename = converter.SaveBmpTemp(bitmap);
                files = new string[] { filename };
            }
            else
            {
                MessageBox.Show($"サポートされていない形式:{String.Join(",", e.Data.GetFormats(true))}");
                return;
            }
            ConvertItems(files);
        }

        private void ConvertItems(string[] files)
        {

            if (converter.DestDir == "" && files.Length == 1 && Directory.Exists(files[0]))
            {
                // expand a directory
                converter.DestDir = files[0];
                var items = Directory.GetFiles(converter.DestDir).ToList();
                ConvertFiles(items);
            }
            else
            {
                // multiple files
                if (converter.DestDir == "")
                {
                    converter.DestDir = Path.GetDirectoryName(files[0]);
                }
                var convfiles = new List<string>();
                foreach (var item in files)
                {
                    if (Directory.Exists(item))
                    {
                        convfiles.AddRange(Directory.GetFiles(item));
                    }
                    else if (File.Exists(item))
                    {
                        convfiles.Add(item);
                    }
                    else
                    {
                        throw new FileNotFoundException(item);
                    }
                }
                ConvertFiles(convfiles);
            }
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
            //    pathToolStripTextBox.ComboBox.Items.Add(dialog.FileName);
            //    pathToolStripTextBox.ComboBox.SelectedItem =
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
            Execution.ClipboardCopy(GetSelectedItems());
        }


        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            var data = Clipboard.GetDataObject();
            if (data.GetDataPresent(DataFormats.FileDrop))
            {
                var files =Clipboard.GetFileDropList();
                string[] filesa = new string[files.Count];
                files.CopyTo(filesa, 0);
                ConvertItems(filesa);
            }
            else if (data.GetDataPresent(DataFormats.Bitmap))
            {
                var bitmap = (Bitmap)data.GetData(DataFormats.Bitmap, true);
                var filename = converter.SaveBmpTemp(bitmap);
                var files = new string[] { filename };
                ConvertItems(files);
            }

        }

        private void trashToolStripButton_Click(object sender, EventArgs e)
        {
            var items = GetSelectedItems();
            Execution.Trash(items);
            foreach (var item in items)
            {
                listView.Items.Remove(listView.Items.Cast<ListViewFileItem>().Where((x) => x.FileInfo.FullName == item).First());
            }
            //UpdateListView();
            //UpdatePictureBox();
            //UpdatePropertyGrid();
        }

        private void aboveToolStripButton_Click(object sender, EventArgs e)
        {
            //converter.DestDir = Path.GetDirectoryName(converter.DestDir);
            //RefreshListView();
            throw new NotImplementedException();
        }

        private void pathToolStripTextBox_Click(object sender, EventArgs e)
        {
            pathToolStripTextBox.SelectAll();
        }

        //private void pathToolStripTextBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (converter.TempDir != pathToolStripTextBox.ComboBox.SelectedItem.ToString())
        //    {
        //        converter.DestDir = pathToolStripTextBox.ComboBox.SelectedItem.ToString();
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
                Process.Start("explorer.exe", converter.DestDirChildE);
            }
        }

        private async void zipToolStripButton_Click(object sender, EventArgs e)
        {
            var items = GetSelectedItems();
            toolStripProgressBar.Visible = true;
            toolStripProgressBar.Style = ProgressBarStyle.Marquee;
            toolStripStatusLabel.Text = $"{items.Length}ファイルを圧縮中";
            var action = new Action<string>((path) => {
                toolStripProgressBar.Visible = false;
                toolStripProgressBar.Style = ProgressBarStyle.Continuous;
                if (path == "")
                {
                    toolStripStatusLabel.Text = $"{items.Length}ファイルを圧縮できませんでした。";
                }
                else
                {
                    toolStripStatusLabel.Text = $"{items.Length}ファイルを圧縮しました。場所:${path}";
                    Process.Start("explorer.exe", "/select," + path);
                    UpdateListView(converter.DestDirChild);
                }
            });
            await Task.Run(() => {
                var zipfile = Execution.Zip(items);
                this.Invoke(action, zipfile);
            });
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



        private void UpdatePictureBox()
        {
            if (listView.SelectedItems.Count > 0)
            {
                var path = ((ListViewFileItem)listView.SelectedItems[0]).FileInfo.FullName;
                if (QImaging.GetFileType(path) == QFileType.Unknown ||
                    QImaging.GetFileType(path) == QFileType.Pdf)
                {

                    pictureBox.Image?.Dispose();
                    pictureBox.Image = null;
                }
                else if(Settings.Default.Preview)
                {
                    pictureBox.ImageLocation = path;
                }
                else
                {
                    pictureBox.Image?.Dispose();
                    pictureBox.Image = null;
                }
            }
            else
            {
                pictureBox.Image?.Dispose();
                pictureBox.Image = null;
                
            }
        }

        private void UpdatePropertyGrid()
        {
            //TabControlUserActive = false;
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
                 //tabControl.TabPages[1].Focus();
           }
            else
            {
                tabPageManager.ChangeTabPageVisible(1, false);
                fileInfoPropertyGrid.SelectedObject = null;
                resultPropertyGrid.SelectedObject = null;
            }

            //TabControlUserActive = true;
        }


        private void MainForm_Activated(object sender, EventArgs e)
        {
            //UpdateListView(converter.DestDirChildE);
            UpdateButtonEnable();
        }

        private void refreshToolStripButton_Click(object sender, EventArgs e)
        {
            UpdateListView();
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtonEnable();
            UpdatePictureBox();
            UpdatePropertyGrid();
            listView.Focus();
        }

        private void listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView.FocusedItem != null)
            {
                Process.Start(((ListViewFileItem)listView.FocusedItem).FileInfo.FullName);
            }
        }

        private void listView_KeyUp(object sender, KeyEventArgs e)
        {
            ListView lv = (ListView)sender;
            //F2キーが離されたときは、フォーカスのあるアイテムの編集を開始
            if (listView.FocusedItem != null)
            {
                if (e.KeyCode == Keys.F2  && lv.LabelEdit)
                {
                    lv.FocusedItem.BeginEdit();
                }
                if (e.KeyCode == Keys.Enter)
                {
                    Process.Start(((ListViewFileItem)listView.FocusedItem).FileInfo.FullName);
                }
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

        private void listView_Click(object sender, EventArgs e)
        {
            
        }

        private void listView_MouseClick(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right)
            //{
            //    var active = (ListViewFileItem)listView.FocusedItem;
            //    if (active != null && active.Bounds.Contains(e.Location))
            //    {
            //        contextMenuStrip.Items.AddRange((ToolStripItemCollection)Execution.GetFileContext(active.FileInfo.FullName));
            //        contextMenuStrip.Show(Cursor.Position);
            //    }
            //}
        }

        private void listView_MouseDown(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Left)
            //{
            //    var lv = (ListView)sender;
            //    var dde=lv.DoDragDrop()
            //}
        }

        private void listView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var data = new DataObject(DataFormats.FileDrop, GetSelectedItems());
            var effect = DragDropEffects.Copy | DragDropEffects.Move;
            listView.DoDragDrop(data, effect);
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (TabControlUserActive)
            //{
            //    ActiveTabPage = tabControl.SelectedIndex;
            //}
        }


        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (UserActive)
            {
                Settings.Default.FormSize = this.Size;
            }
        }

        private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (UserActive)
            {
                Settings.Default.ListViewWidth = splitContainer.SplitterDistance;
            }
        }

        private void pictureBox_Resize(object sender, EventArgs e)
        {
            if (UserActive)
            {
                Settings.Default.PictureBoxHeight = pictureBox.Height;
            }
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }



#pragma warning restore IDE1006 // 命名スタイル
    }
}
