using ImageQuant.Properties;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageQuant
{
    public partial class SettingForm : Form
    {
        private MainForm mainForm;
        public readonly int[] ThumbnailSizeList = new int[] { 25, 50, 75, 100, 150, 200 };
        public const int DefaultThumbnailSize = 100;

        public readonly int[] SizeList = new int[] { 500, 750, 1000, 1500, 2000, 2500, 3000 };

        public readonly int[] QualityJpgList = new int[] { 40, 50, 60, 70, 80, 85, 90, 95, 100 };
        public readonly int[] QualityPngList = new int[] { 40, 50, 60, 70, 80, 85, 90, 95, 100 };
        public readonly int[] ColorDepthList = new int[] { 8, 16, 24, 32 };

        bool loading = false;

        public SettingForm(MainForm main)
        {
            InitializeComponent();
            mainForm = main;

            LoadSettings();
        }

        private void LoadSettings()
        {
            loading = true;

            topMostCheckBox.Checked = this.TopMost = mainForm.TopMost = Settings.Default.TopMost;
            thumbnailSizeCheckBox.Checked = thumbnailSizeComboBox.Enabled = Settings.Default.ManualThumbnailSize;
            thumbnailSizeComboBox.DataSource = ThumbnailSizeList;
            thumbnailSizeComboBox.SelectedItem = Settings.Default.ThumbnailSize;
            previewCheckBox.Checked = Settings.Default.Preview;
            overwriteDialogCheckBox.Checked = Settings.Default.OverwriteConfirm;
            //saveRecentlyDirectoryCheckBox.Checked = Settings.Default.SaveRecentlyDirectory;
            saveRecentlyDirectoryCheckBox.Visible = false;

            currentDirectoryRadioButton.Checked = !Settings.Default.SaveManualPath;
            manualDirectoryRadioButton.Checked = savePathTextBox.Enabled = savePathDialogButton.Enabled =
                Settings.Default.SaveManualPath;
            savePathTextBox.Text = Settings.Default.SavePath;
            //
            createChildDirectoryCheckBox.Checked = childDirectoryTextBox.Enabled = Settings.Default.CreateChildDirectory;
            childDirectoryTextBox.Text = Settings.Default.ChildDirectory;

            prefixCheckBox.Checked = prefixTextBox.Enabled = Settings.Default.Prefix;
            prefixTextBox.Text = Settings.Default.PrefixName;

            suffixCheckBox.Checked = suffixTextBox.Enabled = Settings.Default.Suffix;
            suffixTextBox.Text = Settings.Default.SuffixName;
            //
            sizeCheckBox.Checked = sizeComboBox.Enabled = Settings.Default.Resize;
            sizeComboBox.DataSource = SizeList;
            sizeComboBox.SelectedItem = Settings.Default.MaximumSize;

            formatCheckBox.Checked = formatComboBox.Enabled = Settings.Default.ChangeFormat;
            formatComboBox.DataSource = mainForm.converter.ImageFormatList;
            formatComboBox.SelectedItem = Settings.Default.Format;

            qualityJpgCheckBox.Checked = qualityJpgComboBox.Enabled = Settings.Default.ChangeJpgQuality;
            qualityJpgComboBox.DataSource = QualityJpgList;
            qualityJpgComboBox.SelectedItem = Settings.Default.JpgQuality;

            qualityPngCheckBox.Checked = qualityPngComboBox.Enabled = Settings.Default.ChangePngQuality;
            qualityPngComboBox.DataSource = QualityPngList;
            qualityPngComboBox.SelectedItem = Settings.Default.PngQuality;

            colorDepthCheckBox.Checked = colorDepthComboBox.Enabled = Settings.Default.ChangeColorDepth;
            colorDepthComboBox.DataSource = ColorDepthList;
            colorDepthComboBox.SelectedItem = Settings.Default.ColorDepth;
            //
            SetMailer();
            zipDialogCheckBox.Checked = Settings.Default.ZipDialog;
            //
            versionLabel.Text = $"{Application.ProductName}  {Application.ProductVersion}";

            loading = false;
        }

        private void SetMailer()
        {
            if (mailtoRadioButton.Checked) Settings.Default.Mailer = "mailto";
            else if (outlookRadioButton.Checked) Settings.Default.Mailer = "outlook";
            else outlookRadioButton.Checked = true;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            // do not anything
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            Settings.Default.Reset();
            LoadSettings();
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("まだありません");
        }

        private void SettingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Save();
        }

        //-------------------------------------------------------------------------------

        private void topMostCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.TopMost =
                    this.TopMost = 
                    mainForm.TopMost =
                    topMostCheckBox.Checked;
            }
        }

        private void thumbnailSizeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.ManualThumbnailSize = thumbnailSizeComboBox.Enabled =
                  thumbnailSizeCheckBox.Checked;
            }
        }

        private void thumbnailSizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.ThumbnailSize =
                    mainForm.ThumbnailSize =
                    int.Parse(thumbnailSizeComboBox.SelectedItem.ToString());
            }
        }

        private void previewCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.Preview = previewCheckBox.Checked;
                if(Settings.Default.Preview)
                {
                    //mainForm.tableLayoutPanel.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 50f);
                    mainForm.splitContainer1.SplitterDistance = mainForm.Width / 2;

                }
                else
                {
                    //mainForm.tableLayoutPanel.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 0f);
                    mainForm.splitContainer1.SplitterDistance = mainForm.Width;

                }
            }
        }

        private void overwriteDialogCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.OverwriteConfirm = overwriteDialogCheckBox.Checked;
            }
        }

        private void saveRecentlyDirectoryCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //if (!loading)
            //{
            //    Settings.Default.SaveRecentlyDirectory = saveRecentlyDirectoryCheckBox.Checked;
            //    if (!saveRecentlyDirectoryCheckBox.Checked)
            //    {
            //        Settings.Default.RecentlyDirectory.Clear();
            //        mainForm.pathToolStripCombo.ComboBox.Items.Clear();
            //    }            
            //}
            throw new NotImplementedException();

        }

        //-------------------------------------------------------------------------------

        private void currentDirectoryRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            // do not anything
        }

        private void manualDirectoryRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.SaveManualPath =
                    savePathTextBox.Enabled =
                    savePathDialogButton.Enabled =
                    manualDirectoryRadioButton.Checked;
            }
        }

        private void savePathTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.SavePath = savePathTextBox.Text;
            }
        }

        private void savePathDialogButton_Click(object sender, EventArgs e)
        {
            var dialog = new CommonOpenFileDialog("フォルダ選択");
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                savePathTextBox.Text = dialog.FileName;
            }
        }

        private void createChildDirectoryCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.CreateChildDirectory = 
                    childDirectoryTextBox.Enabled =
                    createChildDirectoryCheckBox.Checked;
            }
        }

        private void childDirectoryTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.ChildDirectory = childDirectoryTextBox.Text;
            }
        }

        private void prefixCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.Prefix =
                    prefixTextBox.Enabled =
                    prefixCheckBox.Checked;
            }
        }

        private void prefixTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.PrefixName = prefixTextBox.Text;
            }
        }

        private void suffixCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.Suffix = 
                    suffixTextBox.Enabled = 
                    suffixCheckBox.Checked;
            }
        }

        private void suffixTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.SuffixName = suffixTextBox.Text;
            }
        }

        //-------------------------------------------------------------------------------

        private void sizeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.Resize = sizeComboBox.Enabled = sizeCheckBox.Checked;
            }
        }

        private void sizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.MaximumSize = int.Parse(sizeComboBox.SelectedItem.ToString());
            }
        }


        private void formatCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.ChangeFormat = formatComboBox.Enabled = formatCheckBox.Checked;
            }
        }

        private void formatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.Format = formatComboBox.SelectedItem.ToString();
            }
        }

        private void qualityJpgCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.ChangeJpgQuality = qualityJpgComboBox.Enabled = qualityJpgCheckBox.Checked;
            }
        }

        private void qualityJpgComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.JpgQuality = int.Parse(qualityJpgComboBox.SelectedItem.ToString());
            }
        }

        private void qualityPngCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.ChangePngQuality = qualityPngComboBox.Enabled = qualityPngCheckBox.Checked;
            }
        }

        private void qualityPngComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.PngQuality = int.Parse(qualityPngComboBox.SelectedItem.ToString());
            }
        }

        private void colorDepthCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.ChangeColorDepth = colorDepthCheckBox.Checked;
            }
        }

        private void colorDepthComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.ColorDepth = int.Parse(colorDepthComboBox.SelectedItem.ToString());
            }
        }

        //-------------------------------------------------------------------------------

        private void mailtoRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            SetMailer();
        }


        private void outlookRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            SetMailer();
        }


        private void openExcelTemplateButton_Click(object sender, EventArgs e)
        {

        }

        private void zipDialogCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Settings.Default.ZipDialog = zipDialogCheckBox.Checked;
            }
        }

        private void githubLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/noyuno/ImageQuant");
        }
    }
}
