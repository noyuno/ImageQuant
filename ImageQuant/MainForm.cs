using ImageQuant.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

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

        bool TabControlUserActive = true;
        int ActiveTabPage = 0;
        bool FormSizeUserActive = false;
        bool Processing = false;
        CancellationTokenSource ConvertCancellationToken;
        CancellationTokenSource UpdateListViewCancellationToken;

        private readonly ToolStripSpringTextBox pathToolStripTextBox;

        private List<ListViewFileItem> ThumbnailFallbackItems = new List<ListViewFileItem>();
        //private System.Windows.Forms.Timer ThumbnailFallbackTimer = new System.Windows.Forms.Timer();

        public MainForm()
        {
            InitializeComponent();
            pictureBox.AllowDrop = true;
            pathToolStripTextBox = new ToolStripSpringTextBox();
            pathToolStripTextBox.Click += pathToolStripTextBox_Click;
            toolStrip2.Items.Add(pathToolStripTextBox);
            InitializeInstance();
            tabPageManager = new TabPageManager(tabControl);
            aboveToolStripButton.Visible = false;
            mkdirToolStripButton.Visible = false;
            UpdateButtonEnable();
            UpdatePropertyGrid();
            InitializeUI();

            //InitializeThumbnailFallbackTimer();
        }

        //private void InitializeThumbnailFallbackTimer()
        //{
        //    ThumbnailFallbackTimer.Interval = Settings.Default.ThumbnailFallbackInterval * 1000;
        //    ThumbnailFallbackTimer.Tick += (sender, e) =>
        //    {
        //        listView.BeginUpdate();
        //        for (int i = 0; i < ThumbnailFallbackItems.Count; i++)
        //        {
        //            if (ThumbnailFallbackItems[i].ThumbnailFallback > 0)
        //            {
        //                GetThumbnail(ThumbnailFallbackItems[i]);
        //                if (listView.LargeImageList.Images.ContainsKey(ThumbnailFallbackItems[i].FileInfo.FullName))
        //                {
        //                    listView.LargeImageList.Images.RemoveByKey(ThumbnailFallbackItems[i].FileInfo.FullName);
        //                }
        //                listView.LargeImageList.Images.Add(ThumbnailFallbackItems[i].FileInfo.FullName, ThumbnailFallbackItems[i].Thumbnail);
        //                if (ThumbnailFallbackItems[i].ThumbnailFallback == 0)
        //                {
        //                    ThumbnailFallbackItems.RemoveAt(i);
        //                }
        //            }
        //        }
        //        listView.EndUpdate();
        //    };
        //    ThumbnailFallbackTimer.Start();
        //}

        private void MainForm_Shown(object sender, EventArgs e)
        {
        }


        private void closeToolStripButton_Click(object sender, EventArgs e)
        {
            InitializeInstance();
            UpdateButtonEnable();
            UpdatePropertyGrid();
            UpdatePictureBox();
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
            bool needwait = false;
            if (ConvertCancellationToken != null && !ConvertCancellationToken.IsCancellationRequested)
            {
                ConvertCancellationToken.Cancel();
                needwait = true;
            }
            if (UpdateListViewCancellationToken != null && !UpdateListViewCancellationToken.IsCancellationRequested)
            {
                UpdateListViewCancellationToken.Cancel();
                needwait = true;
            }
            if(needwait)
            {
                toolStripStatusLabel.Text = this.Text = "処理を中止するまで3秒程度お待ちください...";
                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                timer.Interval = 3000;
                timer.Tick += (sender, e) =>
                  {
                      this.Close();
                  };
                timer.Start();
                e.Cancel = true;
            }
            converter?.Dispose();
        }


        public void InitializeUI()
        { 
            FormSizeUserActive = false;
            this.Size = Settings.Default.FormSize;
            splitContainer.SplitterDistance = Settings.Default.ListViewWidth;
            pictureBox.Size = new Size(pictureBox.Size.Width, Settings.Default.PictureBoxHeight);

            
            FormSizeUserActive = true;

        }

        private void UpdateButtonEnable()
        {
            toolStripProgressBar.Visible = false;
            if (Processing)
            {
                this.Text = Application.ProductName + "(変換中)";
            }
            else
            {
                if (converter.DestDir == "")
                {
                    this.Text = Application.ProductName + "(入力待ち・一時フォルダ)";
                    toolStripStatusLabel.Text = "画像をこのウィンドウにドラッグ＆ドロップしてサイズ変換します。";
                    pathToolStripTextBox.Text = converter.DestDirChild;
                }
                else
                {
                    pathToolStripTextBox.Text = converter.DestDirChild;
                    if (listView.CheckedItems.Count == 0 && listView.SelectedItems.Count == 0)
                    {
                        this.Text = Application.ProductName + "(変換完了)";
                        var counter = converterResultList.Where((x) => x.Success).Count();
                        toolStripStatusLabel.Text = $"{counter}枚変換完了しました(青・赤文字)。画像を選択/チェックしてください。";
                    }
                    else
                    {
                        this.Text = Application.ProductName + "(変換完了)";
                        toolStripStatusLabel.Text = "選択した画像をメールに添付したりエクセルにエクスポートできます。";
                    }
                }
            }

            if (listView.CheckedItems.Count == 0 && listView.SelectedItems.Count == 0)
            {
                copyToolStripButton.Enabled = false;
                trashToolStripButton.Enabled = false;
                zipToolStripButton.Enabled = false;
                attachMailToolStripButton.Enabled = false;
                exportExcelToolStripButton.Enabled = false;
                cwToolStripButton.Enabled = false;
                ccwtoolStripButton.Enabled = false;
            }
            else
            {
                copyToolStripButton.Enabled = true;
                trashToolStripButton.Enabled = true;
                zipToolStripButton.Enabled = true;
                attachMailToolStripButton.Enabled = true;
                exportExcelToolStripButton.Enabled = true;
                cwToolStripButton.Enabled = true;
                ccwtoolStripButton.Enabled = true;
            }

            toolStrip1.Enabled = toolStrip2.Enabled = !Processing;
            
            var data = Clipboard.GetDataObject();
            pasteToolStripButton.Enabled = (
                data.GetDataPresent(DataFormats.Bitmap) ||
                data.GetDataPresent(DataFormats.FileDrop));
        }

        private void ConvertItems(string[] files)
        {
            Processing = true;
            UpdateButtonEnable();

            var oldDestDir = converter.DestDir;
            List<string> items;
            if (converter.DestDir == "" && files.Length == 1 && Directory.Exists(files[0]))
            {
                // expand a directory
                converter.DestDir = Settings.Default.SaveManualPath ? Settings.Default.SavePath : files[0];
                items = Directory.GetFiles(files[0]).ToList() ;
                //if (!CheckOverwrite(items))
                //{
                //    converter.DestDir = oldDestDir;
                //    Processing = false;
                //    UpdateButtonEnable();
                //    return;
                //}
                //ConvertFiles(items);
            }
            else
            {
                // multiple files
                if (converter.DestDir == "")
                {
                    converter.DestDir = Settings.Default.SaveManualPath ? Settings.Default.SavePath : Path.GetDirectoryName(files[0]);
                }
                //var convfiles = new List<string>();
                items = new List<string>();
                foreach (var item in files)
                {
                    if (Directory.Exists(item))
                    {
                        items.AddRange(Directory.GetFiles(item));
                    }
                    else if (File.Exists(item))
                    {
                        items.Add(item);
                    }
                    else
                    {
                        throw new FileNotFoundException(item);
                    }
                }
                //if (!CheckOverwrite(convfiles.ToArray()))
                //{
                //    converter.DestDir = oldDestDir;
                //    Processing = false;
                //    UpdateButtonEnable();
                //    return;
                //}
                //ConvertFiles(convfiles.ToArray());
            }
            if (!CheckOverwrite(items))
            {
                converter.DestDir = oldDestDir;
                Processing = false;
                UpdateButtonEnable();
                return;
            }
            ConvertFiles(items);
        }

        public bool CheckOverwrite(List<string> sourceFilename)
        {
            bool ignore = false;
            bool overwrite = false;
            var message = new List<string>();
            foreach (var item in sourceFilename)
            {
                var sourceFileType = QImaging.GetFileType(item);
                if (sourceFileType == QFileType.Unknown)
                {
                    message.Add($"{item}: 画像ファイルではないため無視されます。");
                    ignore = true;
                }
                else
                {
                    converter.MakeDest(item, out _, out var dest);
                    if (File.Exists(dest))
                    {
                        message.Add($"{dest}: 上書きされます。");
                        overwrite = true;
                    }
                }
            }

            if (ignore || (Settings.Default.OverwriteConfirm && overwrite))
            {
                this.Activate();
                return (new AskForm().ShowDialog(this, "変換前に確認してください。続行する場合はOK、中止する場合はキャンセルを押してください。\r\n"
                    + string.Join("\r\n", message),
                    "ImageQuant", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK);
            }
            else
            {
                return true;
            }
        }


        private async void ConvertFiles(List<string> filenames)
        {
            //converterResultList.Clear();
            toolStripProgressBar.Visible = true;
            toolStripProgressBar.Maximum = filenames.Count;
            toolStripProgressBar.Value = 0;
            ConvertCancellationToken = new CancellationTokenSource();

            try
            {
                var ret = new List<ConverterResult>();
                for (int i = 0; i < filenames.Count; i += Settings.Default.ParallelProcess)
                {
                    var num = Math.Min(Settings.Default.ParallelProcess, filenames.Count - i);
                    var f = new List<string>();
                    for (int fi = 0; fi < filenames.Count && fi < num; fi++)
                    {
                        f.Add(filenames[i]);
                    }

                    var convertimages = await ConvertImages(f, (count, r) =>
                    {
                        toolStripProgressBar.Visible = true;
                        toolStripProgressBar.Value = count + i;
                        toolStripStatusLabel.Text = $"{count + i} / {filenames.Count}: 変換中...";
                    }, ConvertCancellationToken.Token);
                    ret.AddRange(convertimages);

                }

                converterResultList.AddRange(ret);
                var failedret = ret.Where((x) => !x.Success);
                if (failedret.Count() > 0)
                {
                    var faileditems = string.Join("\r\n",
                        failedret.Select((x) => $"{x.SourcePath}:{x.ErrorMessage}").ToArray());
                    this.Activate();
                    new AskForm().Show(this, $"{ret.Count}ファイルのうち、次のファイル(n={failedret.Count()})は変換できませんでした。\r\n{faileditems}",
                        "変換失敗", MessageBoxButtons.OK,MessageBoxIcon.Error);
                }

                toolStripProgressBar.Visible = false;
                Processing = false;
                UpdateListView();
                UpdateButtonEnable();
            }
            catch(OperationCanceledException e)
            {
                new AskForm().Show(this, "処理はユーザにより中止されました。", "中止", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                ConvertCancellationToken?.Dispose();
                ConvertCancellationToken = null;
            }
        }


        private async Task<ConverterResult[]> ConvertImages(List<string> filenames, Action<int, ConverterResult> action, CancellationToken ct)
        {
            var tasks = new List<Task<ConverterResult>>();
            var done = 0;

            for (var i = 0; i < filenames.Count; i++)
            {
                var x = i;
                var task = Task.Run<ConverterResult>(() =>
                {
                    try
                    {
                        var ret = converter.Convert(filenames[x]);
                        if (ct.IsCancellationRequested)
                        {
                            ct.ThrowIfCancellationRequested();
                        }
                        if (!this.IsDisposed)
                        {
                            this.Invoke(action, ++done, ret);
                        }
                        return ret;
                    }
                    catch(OperationCanceledException ex)
                    {
                        //new AskForm().Show(this, e.Message, "OperationCanceledException", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return new ConverterResult(false, ex.Message, filenames[x], "", 0, 0, 0, 0, null, null, QFileType.Unknown, 0, 0, false, false, RotateFlipType.RotateNoneFlipNone, false);
                    }
                    catch(Exception ex)
                    {
                        //new ThreadExceptionDialog(ex).ShowDialog();
                        return new ConverterResult(false, ex.Message, filenames[x], "", 0, 0, 0, 0, null, null, QFileType.Unknown, 0, 0, false, false, RotateFlipType.RotateNoneFlipNone, false);
                    }
                    
                }, ct);
                tasks.Add(task);
                if (ct.IsCancellationRequested)
                {
                    ct.ThrowIfCancellationRequested();
                }
            }
            return await Task.WhenAll(tasks);
        }

        public async void UpdateListView(string path = null)
        {
            if (Processing)
            {
                return;
            }
            path ??= converter.DestDirChildE;
            string[] files = Directory.GetFiles(path);
            //toolStripProgressBar.Maximum = files.Length;
            //toolStripProgressBar.Value = 0;
            //toolStripProgressBar.Visible = true;
            UpdateListViewCancellationToken = new CancellationTokenSource();
            try
            {
                var ret = (await UpdateListView2(files, (count, r) =>
                {
                    //toolStripProgressBar.Value = count;
                }, UpdateListViewCancellationToken.Token)).ToList();
                // store selected/checked items
                var checkeditems = listView.CheckedItems.Cast<ListViewFileItem>()
                    .Select((x) => x.FileInfo.FullName).ToArray();
                var selecteditems = listView.SelectedItems.Cast<ListViewFileItem>()
                    .Select((x) => x.FileInfo.FullName).ToArray();
                var focused = listView.FocusedItem != null ? listView.FocusedItem.Index : 0;
                focused = focused == 0 && listView.SelectedItems.Count > 0 ? listView.SelectedItems[0].Index : focused;
                focused = focused == 0 && listView.CheckedItems.Count > 0 ? listView.CheckedItems[0].Index : focused;
                listView.BeginUpdate();
                listView.Items.Clear();
                listView.LargeImageList.Images.Clear();

                var failedret = new List<string>();
                for (int i = 0; i < ret.Count; i++)
                {
                    if (ret[i].Success)
                    {
                        
                        listView.LargeImageList.Images.Add(ret[i].FileInfo.FullName, ret[i].Thumbnail);

                        ret[i].Checked = checkeditems.Contains(ret[i].FileInfo.FullName);
                        ret[i].Selected = selecteditems.Contains(ret[i].FileInfo.FullName);
                        listView.Items.Add(ret[i]);
                    }
                    else
                    {
                        failedret.Add($"{ret[i].FileName}:{ret[i].Message}");
                    }
                }
                if (failedret.Count > 0)
                {
                    new AskForm().Show(this, $"{ret.Count}ファイルのうち、次のファイル(n={failedret.Count})は取得できませんでした。\r\n{string.Join("\r\n", failedret)}", "取得失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //listView.Items.AddRange(ret);
                //foreach (var r in ret)
                //{
                    //if (r.ThumbnailFallback > 0)
                    //{
                    //    GetThumbnail(r);
                    //    listView.LargeImageList.Images.Add(r.FileInfo.FullName, r.Thumbnail);
                    //    if (r.ThumbnailFallback > 0)
                    //    {
                    //        ThumbnailFallbackItems.Add(r);
                    //    }
                    //}
                    //else
                    //{
                    //    listView.LargeImageList.Images.Add(r.FileInfo.FullName, r.Thumbnail);
                    //}
                //}

                //toolStripProgressBar.Visible = false;
                listView.ShowItemToolTips = true;
                if (listView.Items.Count > focused && focused > 0)
                {
                    listView.EnsureVisible(focused);
                }
                listView.EndUpdate();
            }
            catch(OperationCanceledException e)
            {
                new AskForm().Show(this, e.Message, "OperationCanceledException", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception ex)
            {
                new ThreadExceptionDialog(ex).ShowDialog();
            }
            finally
            {
                UpdateListViewCancellationToken?.Dispose();
                UpdateListViewCancellationToken = null;
            }
        }

        private static void GetThumbnail(ListViewFileItem lvfi)
        {
            if (lvfi.ConverterResult != null)
            {
                (lvfi.Thumbnail, lvfi.ThumbnailFallback) = QImaging.GetThumbnail(lvfi.FileInfo.FullName, lvfi.ConverterResult.DestWidth, lvfi.ConverterResult.DestHeight, lvfi.ConverterResult.Rotate, Resources.general_file);
                
            }
            else
            {
                (lvfi.Thumbnail, lvfi.ThumbnailFallback) = QImaging.GetThumbnail(lvfi.FileInfo.FullName, Resources.general_file);
            }
        }

        private async Task<ListViewFileItem[]> UpdateListView2(string[] files, Action<int, ListViewFileItem> action, CancellationToken ct)
        {
            var tasks = new List<Task<ListViewFileItem>>();
            var done = 0;
            for (int i = 0; i < files.Length; i++)
            {
                var x = i;
                var task = Task.Run<ListViewFileItem>(() =>
                  {
                      try
                      {
                          ListViewFileItem ret = null;
                          bool cond(ConverterResult y) => (y.DestPath == files[x]);
                          if (converterResultList.Exists(y => cond(y)))
                          {
                              ret = new ListViewFileItem(converterResultList.Last(cond));
                          }
                          else
                          {
                              ret = new ListViewFileItem(new FileInfo(files[x]));
                          }
                          GetThumbnail(ret);
                          if (ret.ThumbnailFallback > 0)
                          {
                              ThumbnailFallbackItems.Add(ret);
                          }
                          if (ct.IsCancellationRequested)
                              ct.ThrowIfCancellationRequested();
                          if (!this.IsDisposed)
                              this.Invoke(action, ++done, ret);
                          ret.Success = true;
                          return ret;
                      }
                      catch (OperationCanceledException e)
                      {
                          return new ListViewFileItem(false, files[i], e.Message);
                      }
                      catch (Exception ex)
                      {
                          return new ListViewFileItem(false, files[i], ex.Message);
                      }
                      return new ListViewFileItem(false, files[i], "情報なし");
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
                this.Activate();
                new AskForm().Show(this, $"サポートされていない形式:{String.Join(",", e.Data.GetFormats(true))}", "ドラッグドロップエラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
            ConvertItems(files);
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
            Execution.ClipboardCopy(GetSelectedPaths());
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
            Execution.Trash(items.Select((x)=>x.FileInfo.FullName).ToArray());
            foreach (var item in items)
            {
                listView.Items.Remove(item);
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

        private void mkdirToolStripButton_Click(object sender, EventArgs e)
        {
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
            var items = GetSelectedPaths();
            if (items.Length == 0)
            {
                new AskForm().Show(this, "圧縮したいファイルを1つ以上指定してください。", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
                try
                {
                    var zipfile = Execution.Zip(items);
                    this.Invoke(action, zipfile);
                }
                catch(Exception ex)
                {
                    new ThreadExceptionDialog(ex).ShowDialog();
                }
            });
        }

        private void attachMailToolStripButton_Click(object sender, EventArgs e)
        {
            if (Settings.Default.Mailer == "outlook")
            {

            Execution.SendMailOutlook(GetSelectedPaths());
            }
            else if (Settings.Default.Mailer=="mailto")
            {
                Execution.SendMailMailto(GetSelectedPaths());

            }
        }

        private string[] GetSelectedPaths()
        {
            return GetSelectedItems().Select((item) => { return item.FileInfo.FullName; }).ToArray();
        }

        private ListViewFileItem[] GetSelectedItems()
        {
            IEnumerable<ListViewFileItem> enu = listView.CheckedItems.Count == 0 ?
                listView.SelectedItems.Cast<ListViewFileItem>() :
                listView.CheckedItems.Cast<ListViewFileItem>();
            return enu.ToArray();
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
                    var action = new Action<Image>((image) =>
                    {
                        pictureBox.Image = image;
                        pictureBox.Image.RotateFlip(QImaging.ReadOrientation(pictureBox.Image));
                    });
                    var task = Task.Run<Image>(() =>
                    {
                        try
                        {
                            using var stream = new MemoryStream(File.ReadAllBytes(path));
                            var image = Image.FromStream(stream);

                            if (!this.IsDisposed)
                            {
                                this.Invoke(action, image);
                            }
                            return image;
                        }
                        catch(Exception ex)
                        {
                            new ThreadExceptionDialog(ex).ShowDialog();
                        }
                        return null;
                    });
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
            TabControlUserActive = false;
            if (listView.SelectedItems.Count > 0)
            {
                fileInfoPropertyGrid.SelectedObject = ((ListViewFileItem)listView.SelectedItems[0]).FileInfo;
                if (((ListViewFileItem)listView.SelectedItems[0]).ConverterResult == null)
                {

                    tabPageManager.ChangeTabPageVisible(1, false);
                }
                else
                {
                    tabPageManager.ChangeTabPageVisible(1, true);
                    resultPropertyGrid.SelectedObject = ((ListViewFileItem)listView.SelectedItems[0]).ConverterResult;
                    if (ActiveTabPage == 1)
                    {
                        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                        timer.Interval = 10;
                        timer.Tick += (sender, e) =>
                         {
                             timer.Stop();
                             if (tabControl != null && tabControl.TabCount == 2 && ActiveTabPage == 1)
                             {
                                 TabControlUserActive = false;
                                 tabControl.SelectedIndex = 1;
                                 listView.Focus();
                                 TabControlUserActive = true;
                             }
                         };
                        timer.Start();
                    }
                }
           }
            else
            {
                tabPageManager.ChangeTabPageVisible(1, false);
                fileInfoPropertyGrid.SelectedObject = null;
                resultPropertyGrid.SelectedObject = null;
            }

            TabControlUserActive = true;
        }


        private void MainForm_Activated(object sender, EventArgs e)
        {
            UpdateButtonEnable();
        }

        private void refreshToolStripButton_Click(object sender, EventArgs e)
        {
            UpdateListView();
            UpdateButtonEnable();
            UpdatePropertyGrid();
            listView.Focus();
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
                if (e.KeyCode == Keys.F5)
                {
                    UpdateListView();
                    UpdateButtonEnable();
                    UpdatePictureBox();
                    UpdatePropertyGrid();
                    listView.Focus();
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
                        new AskForm().Show(this, "同名のアイテムがすでにあります。", "名前変更エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
                    new AskForm().Show(this, ex.Message, "名前変更エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void listView_Click(object sender, EventArgs e)
        {
            
        }

        private void listView_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void listView_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void listView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // 誤動作をよく起こすので、廃止
            //var data = new DataObject(DataFormats.FileDrop, GetSelectedItems());
            //var effect = DragDropEffects.Copy | DragDropEffects.Move;
            //listView.DoDragDrop(data, effect);
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabControlUserActive)
            {
                ActiveTabPage = tabControl.SelectedIndex;
            }
        }


        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (FormSizeUserActive)
            {
                Settings.Default.FormSize = this.Size;
            }
        }

        private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (FormSizeUserActive)
            {
                Settings.Default.ListViewWidth = splitContainer.SplitterDistance;
            }
        }

        private void pictureBox_Resize(object sender, EventArgs e)
        {
            if (FormSizeUserActive)
            {
                Settings.Default.PictureBoxHeight = pictureBox.Height;
            }
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void ccwtoolStripButton_Click(object sender, EventArgs e)
        {
            Rotate(RotateFlipType.Rotate270FlipNone);
        }

        private void Rotate(RotateFlipType rotation)
        {
            var items = GetSelectedItems();
            if (items.Length > 0)
            {
                var failedret = new List<string>();
                foreach (var item in items)
                {
                    var name = item.FileInfo.FullName;

                    if (QImaging.GetFileType(name) == QFileType.Jpeg)
                    {
                        using (var currentImage = Image.FromFile(item.FileInfo.FullName))
                            rotation = QImaging.ReadOrientation(currentImage, rotation);
                        if (QImaging.RotateExif(name, rotation))
                        {
                            if (item.ConverterResult != null)
                            {
                                item.ConverterResult.Rotate = rotation;
                            }
                            if (listView.LargeImageList.Images.ContainsKey(name))
                            {
                                listView.LargeImageList.Images.RemoveByKey(name);
                            }
                            GetThumbnail(item);
                            if (item.ThumbnailFallback > 0)
                            {
                                ThumbnailFallbackItems.Add(item);
                            }
                            listView.LargeImageList.Images.Add(name, item.Thumbnail);
                        }
                        else
                        {
                            failedret.Add($"{name}: Exifデータの編集に失敗しました。");
                        }
                    }
                    else if (QImaging.GetFileType(name) == QFileType.Png)
                    {
                        var ret = converter.RotatePng(name, item.ConverterResult, rotation);
                        if (ret.Success)
                        {
                            converterResultList.Remove(item.ConverterResult);
                            converterResultList.Add(ret);
                            item.ConverterResult = ret;
                            GetThumbnail(item);
                            if (item.ThumbnailFallback > 0)
                            {
                                ThumbnailFallbackItems.Add(item);
                            }
                            if (listView.LargeImageList.Images.ContainsKey(name))
                            {
                                listView.LargeImageList.Images.RemoveByKey(name);
                            }
                            listView.LargeImageList.Images.Add(name, item.Thumbnail);
                            //item.ImageIndex = listView.LargeImageList.Images.Count - 1;

                        }
                        else
                        {
                            failedret.Add($"{name}: 画像の回転に失敗しました。");
                        }
                    }
                    else
                    {
                        failedret.Add($"{name}: サポートされていません。");
                    }

                }
                if (failedret.Count > 0)
                {
                    new AskForm().Show(this, $"{items.Length}ファイルのうち、次のファイル(n={failedret.Count()})は回転できませんでした。\r\n{string.Join("\r\n", failedret)}",
                        "変換失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                UpdateButtonEnable();
                UpdatePictureBox();
                UpdatePropertyGrid();
                listView.Focus();
            }
        }

        private void cwToolStripButton_Click(object sender, EventArgs e)
        {
            Rotate(RotateFlipType.Rotate90FlipNone);

        }



#pragma warning restore IDE1006 // 命名スタイル
    }
}
