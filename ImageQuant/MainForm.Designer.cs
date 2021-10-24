
namespace ImageQuant
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.listView = new System.Windows.Forms.ListView();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.copyToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.pasteToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.selectAllToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.trashToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.aboveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.pathToolStripCombo = new System.Windows.Forms.ToolStripTextBox();
            this.mkdirToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.explorerToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.zipToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.attachMailToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.exportExcelToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar,
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 537);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 20, 0);
            this.statusStrip1.Size = new System.Drawing.Size(915, 34);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(140, 26);
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(167, 28);
            this.toolStripStatusLabel.Text = "toolStripStatusLabel1";
            // 
            // listView
            // 
            this.listView.AllowDrop = true;
            this.listView.CheckBoxes = true;
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.HideSelection = false;
            this.listView.LabelEdit = true;
            this.listView.Location = new System.Drawing.Point(4, 4);
            this.listView.Margin = new System.Windows.Forms.Padding(4);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.tableLayoutPanel.SetRowSpan(this.listView, 2);
            this.listView.ShowItemToolTips = true;
            this.listView.Size = new System.Drawing.Size(449, 502);
            this.listView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView.TabIndex = 2;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView_ItemChecked);
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            this.listView.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView_DragDrop);
            this.listView.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView_DragEnter);
            this.listView.DoubleClick += new System.EventHandler(this.listView_DoubleClick);
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(29, 24);
            this.openToolStripButton.Text = "フォルダーを開く";
            this.openToolStripButton.Click += new System.EventHandler(this.openToolStripButton_Click);
            // 
            // copyToolStripButton
            // 
            this.copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.copyToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripButton.Image")));
            this.copyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolStripButton.Name = "copyToolStripButton";
            this.copyToolStripButton.Size = new System.Drawing.Size(29, 24);
            this.copyToolStripButton.Text = "選択したファイルをクリップボードにコピーする";
            this.copyToolStripButton.Click += new System.EventHandler(this.copyToolStripButton_Click);
            // 
            // pasteToolStripButton
            // 
            this.pasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pasteToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripButton.Image")));
            this.pasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteToolStripButton.Name = "pasteToolStripButton";
            this.pasteToolStripButton.Size = new System.Drawing.Size(29, 24);
            this.pasteToolStripButton.Text = "ファイル・画像をクリップボードから貼り付ける";
            this.pasteToolStripButton.Click += new System.EventHandler(this.pasteToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripButton,
            this.copyToolStripButton,
            this.pasteToolStripButton,
            this.trashToolStripButton,
            this.toolStripSeparator1,
            this.openToolStripButton,
            this.aboveToolStripButton,
            this.pathToolStripCombo,
            this.mkdirToolStripButton,
            this.toolStripSeparator2,
            this.explorerToolStripButton,
            this.zipToolStripButton,
            this.attachMailToolStripButton,
            this.exportExcelToolStripButton,
            this.toolStripButton1,
            this.settingsToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(915, 27);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // selectAllToolStripButton
            // 
            this.selectAllToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.selectAllToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("selectAllToolStripButton.Image")));
            this.selectAllToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.selectAllToolStripButton.Name = "selectAllToolStripButton";
            this.selectAllToolStripButton.Size = new System.Drawing.Size(29, 24);
            this.selectAllToolStripButton.Text = "すべて選択/選択解除";
            this.selectAllToolStripButton.Click += new System.EventHandler(this.selectAllToolStripButton_Click);
            // 
            // trashToolStripButton
            // 
            this.trashToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.trashToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("trashToolStripButton.Image")));
            this.trashToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.trashToolStripButton.Name = "trashToolStripButton";
            this.trashToolStripButton.Size = new System.Drawing.Size(29, 24);
            this.trashToolStripButton.Text = "ごみ箱に移動する";
            this.trashToolStripButton.Click += new System.EventHandler(this.trashToolStripButton_Click);
            // 
            // aboveToolStripButton
            // 
            this.aboveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.aboveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("aboveToolStripButton.Image")));
            this.aboveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.aboveToolStripButton.Name = "aboveToolStripButton";
            this.aboveToolStripButton.Size = new System.Drawing.Size(29, 24);
            this.aboveToolStripButton.Text = "上の階層に移動する";
            this.aboveToolStripButton.Click += new System.EventHandler(this.aboveToolStripButton_Click);
            // 
            // pathToolStripCombo
            // 
            this.pathToolStripCombo.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            this.pathToolStripCombo.Name = "pathToolStripCombo";
            this.pathToolStripCombo.ReadOnly = true;
            this.pathToolStripCombo.Size = new System.Drawing.Size(450, 27);
            this.pathToolStripCombo.ToolTipText = "場所";
            this.pathToolStripCombo.Click += new System.EventHandler(this.pathToolStripCombo_Click);
            // 
            // mkdirToolStripButton
            // 
            this.mkdirToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mkdirToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("mkdirToolStripButton.Image")));
            this.mkdirToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mkdirToolStripButton.Name = "mkdirToolStripButton";
            this.mkdirToolStripButton.Size = new System.Drawing.Size(29, 24);
            this.mkdirToolStripButton.Text = "新しいフォルダーを作成する";
            this.mkdirToolStripButton.Click += new System.EventHandler(this.mkdirToolStripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // explorerToolStripButton
            // 
            this.explorerToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.explorerToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("explorerToolStripButton.Image")));
            this.explorerToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.explorerToolStripButton.Name = "explorerToolStripButton";
            this.explorerToolStripButton.Size = new System.Drawing.Size(29, 24);
            this.explorerToolStripButton.Text = "この場所をWindowsエクスプローラーで開く";
            this.explorerToolStripButton.Click += new System.EventHandler(this.explorerToolStripButton_Click);
            // 
            // zipToolStripButton
            // 
            this.zipToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.zipToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("zipToolStripButton.Image")));
            this.zipToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zipToolStripButton.Name = "zipToolStripButton";
            this.zipToolStripButton.Size = new System.Drawing.Size(29, 24);
            this.zipToolStripButton.Text = "選択した画像を圧縮する";
            this.zipToolStripButton.Click += new System.EventHandler(this.zipToolStripButton_Click);
            // 
            // attachMailToolStripButton
            // 
            this.attachMailToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.attachMailToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("attachMailToolStripButton.Image")));
            this.attachMailToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.attachMailToolStripButton.Name = "attachMailToolStripButton";
            this.attachMailToolStripButton.Size = new System.Drawing.Size(29, 24);
            this.attachMailToolStripButton.Text = "選択した画像をメールに添付する";
            this.attachMailToolStripButton.Click += new System.EventHandler(this.attachMailToolStripButton_Click);
            // 
            // exportExcelToolStripButton
            // 
            this.exportExcelToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.exportExcelToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("exportExcelToolStripButton.Image")));
            this.exportExcelToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exportExcelToolStripButton.Name = "exportExcelToolStripButton";
            this.exportExcelToolStripButton.Size = new System.Drawing.Size(29, 24);
            this.exportExcelToolStripButton.Text = "選択した画像をExcelワークブックにエクスポートする。";
            this.exportExcelToolStripButton.Click += new System.EventHandler(this.exportExcelToolStripButton_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(6, 27);
            // 
            // settingsToolStripButton
            // 
            this.settingsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.settingsToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("settingsToolStripButton.Image")));
            this.settingsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.settingsToolStripButton.Name = "settingsToolStripButton";
            this.settingsToolStripButton.Size = new System.Drawing.Size(29, 24);
            this.settingsToolStripButton.Text = "設定...";
            this.settingsToolStripButton.Click += new System.EventHandler(this.settingsToolStripButton_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(461, 4);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(450, 352);
            this.pictureBox.TabIndex = 3;
            this.pictureBox.TabStop = false;
            this.pictureBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.pictureBox_DragDrop);
            this.pictureBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.pictureBox_DragEnter);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.listView, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.pictureBox, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.dataGridView, 1, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 27);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(915, 510);
            this.tableLayoutPanel.TabIndex = 4;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(460, 363);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersWidth = 51;
            this.dataGridView.RowTemplate.Height = 24;
            this.dataGridView.Size = new System.Drawing.Size(452, 144);
            this.dataGridView.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(915, 571);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(120, 113);
            this.Name = "MainForm";
            this.Text = "ImageQuant";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.StatusStrip statusStrip1;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        public System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        public System.Windows.Forms.ListView listView;
        public System.Windows.Forms.ToolStripButton openToolStripButton;
        public System.Windows.Forms.ToolStripButton copyToolStripButton;
        public System.Windows.Forms.ToolStripButton pasteToolStripButton;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public System.Windows.Forms.ToolStrip toolStrip1;
        public System.Windows.Forms.ToolStripButton trashToolStripButton;
        public System.Windows.Forms.ToolStripButton attachMailToolStripButton;
        public System.Windows.Forms.ToolStripButton aboveToolStripButton;
        public System.Windows.Forms.ToolStripButton mkdirToolStripButton;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        public System.Windows.Forms.ToolStripButton explorerToolStripButton;
        public System.Windows.Forms.ToolStripButton zipToolStripButton;
        public System.Windows.Forms.ToolStripButton exportExcelToolStripButton;
        public System.Windows.Forms.ToolStripButton selectAllToolStripButton;
        public System.Windows.Forms.ToolStripSeparator toolStripButton1;
        public System.Windows.Forms.ToolStripButton settingsToolStripButton;
        public System.Windows.Forms.ToolStripTextBox pathToolStripCombo;
        public System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.DataGridView dataGridView;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
    }
}

