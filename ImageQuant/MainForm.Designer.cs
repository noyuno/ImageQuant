﻿
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
            this.fileInfoPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.fileInfoTabPage = new System.Windows.Forms.TabPage();
            this.resultTabPage = new System.Windows.Forms.TabPage();
            this.resultPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.tabControl.SuspendLayout();
            this.fileInfoTabPage.SuspendLayout();
            this.resultTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
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
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.Margin = new System.Windows.Forms.Padding(4);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.ShowItemToolTips = true;
            this.listView.Size = new System.Drawing.Size(305, 506);
            this.listView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView.TabIndex = 2;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listView_AfterLabelEdit);
            this.listView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView_ItemChecked);
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            this.listView.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView_DragDrop);
            this.listView.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView_DragEnter);
            this.listView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listView_KeyUp);
            this.listView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_MouseDoubleClick);
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(29, 28);
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
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
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
            this.toolStrip1.Size = new System.Drawing.Size(915, 31);
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
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(606, 362);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 3;
            this.pictureBox.TabStop = false;
            this.pictureBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.pictureBox_DragDrop);
            this.pictureBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.pictureBox_DragEnter);
            // 
            // fileInfoPropertyGrid
            // 
            this.fileInfoPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileInfoPropertyGrid.HelpVisible = false;
            this.fileInfoPropertyGrid.Location = new System.Drawing.Point(3, 3);
            this.fileInfoPropertyGrid.Name = "fileInfoPropertyGrid";
            this.fileInfoPropertyGrid.Size = new System.Drawing.Size(592, 106);
            this.fileInfoPropertyGrid.TabIndex = 4;
            this.fileInfoPropertyGrid.ToolbarVisible = false;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.fileInfoTabPage);
            this.tabControl.Controls.Add(this.resultTabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 362);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(606, 144);
            this.tabControl.TabIndex = 4;
            // 
            // fileInfoTabPage
            // 
            this.fileInfoTabPage.Controls.Add(this.fileInfoPropertyGrid);
            this.fileInfoTabPage.Location = new System.Drawing.Point(4, 28);
            this.fileInfoTabPage.Name = "fileInfoTabPage";
            this.fileInfoTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.fileInfoTabPage.Size = new System.Drawing.Size(598, 112);
            this.fileInfoTabPage.TabIndex = 0;
            this.fileInfoTabPage.Text = "ファイル情報";
            this.fileInfoTabPage.UseVisualStyleBackColor = true;
            // 
            // resultTabPage
            // 
            this.resultTabPage.Controls.Add(this.resultPropertyGrid);
            this.resultTabPage.Location = new System.Drawing.Point(4, 28);
            this.resultTabPage.Name = "resultTabPage";
            this.resultTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.resultTabPage.Size = new System.Drawing.Size(598, 116);
            this.resultTabPage.TabIndex = 1;
            this.resultTabPage.Text = "変換結果";
            this.resultTabPage.UseVisualStyleBackColor = true;
            // 
            // resultPropertyGrid
            // 
            this.resultPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultPropertyGrid.HelpVisible = false;
            this.resultPropertyGrid.Location = new System.Drawing.Point(3, 3);
            this.resultPropertyGrid.Name = "resultPropertyGrid";
            this.resultPropertyGrid.Size = new System.Drawing.Size(592, 110);
            this.resultPropertyGrid.TabIndex = 5;
            this.resultPropertyGrid.ToolbarVisible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 31);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitter1);
            this.splitContainer1.Panel2.Controls.Add(this.tabControl);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox);
            this.splitContainer1.Size = new System.Drawing.Size(915, 506);
            this.splitContainer1.SplitterDistance = 305;
            this.splitContainer1.TabIndex = 5;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 362);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(606, 3);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(915, 571);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(120, 113);
            this.Name = "MainForm";
            this.Text = "ImageQuant";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.fileInfoTabPage.ResumeLayout(false);
            this.resultTabPage.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
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
        private System.Windows.Forms.PropertyGrid fileInfoPropertyGrid;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage fileInfoTabPage;
        private System.Windows.Forms.TabPage resultTabPage;
        private System.Windows.Forms.PropertyGrid resultPropertyGrid;
        private System.Windows.Forms.Splitter splitter1;
        public System.Windows.Forms.SplitContainer splitContainer1;
    }
}

