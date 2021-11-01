using System;
using System.Drawing;
using System.Windows.Forms;

namespace ImageQuant
{
    public partial class AskForm : Form
    {
        MessageBoxButtons buttons;

        public AskForm()
        {
            InitializeComponent();
        }


        private void Init(string text, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            Text = title;
            TextBox.Text = text;
            TextBox.Select(0, 0);
            this.buttons = buttons;
            if (buttons == MessageBoxButtons.OK)
            {
                cancelButton.Visible = false;
            }
            else if (buttons == MessageBoxButtons.YesNo)
            {
                OKButton.Text = "はい";
                cancelButton.Text = "いいえ";
            }
            else if (buttons == MessageBoxButtons.RetryCancel)
            {
                OKButton.Text = "再試行";
            }

            var bmp = new Bitmap(48, 48);
            using var g = Graphics.FromImage(bmp);
            g.DrawIcon(icon switch
            {
                MessageBoxIcon.Warning => SystemIcons.Warning,
                MessageBoxIcon.Error => SystemIcons.Error,
                MessageBoxIcon.Information => SystemIcons.Information,
                MessageBoxIcon.Question => SystemIcons.Question,
                _ => SystemIcons.WinLogo
            }, 0, 0);
            pictureBox1.Image = bmp;
            OKButton.Focus();
        }


        //public void Show(Form form, string text, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        //{
        //    Init(text, title, buttons, icon);
        //    this.Show(form);
        //}

        public DialogResult ShowDialog(Form form, string text, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            Init(text, title, buttons, icon);
            return this.ShowDialog(form);
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            DialogResult = buttons switch
            {
                MessageBoxButtons.OK => DialogResult.OK,
                MessageBoxButtons.OKCancel => DialogResult.OK,
                MessageBoxButtons.YesNo => DialogResult.Yes,
                MessageBoxButtons.RetryCancel => DialogResult.Retry,
                _ => DialogResult.OK
            };
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = buttons switch
            {
                MessageBoxButtons.OKCancel => DialogResult.Cancel,
                MessageBoxButtons.YesNo => DialogResult.No,
                MessageBoxButtons.RetryCancel => DialogResult.Cancel,
                _ => DialogResult.Cancel
            };
            this.Close();

        }
    }
}
