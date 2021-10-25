using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.IO.Compression;
using System.IO;
using System.Windows.Forms;
using ImageQuant.Properties;
using System.Diagnostics;

namespace ImageQuant
{
    public static class Execution
    {

        public static void SendMailOutlook(string[] paths)
        {
            var ol = new Outlook.Application();
            Outlook.MailItem mail = ol.CreateItem(Outlook.OlItemType.olMailItem) as Outlook.MailItem;
            foreach (var item in paths)
            {
                mail.Attachments.Add(item);
            }
            mail.Display();
        }

        public static void SendMailMailto(string[] paths)
        {
            Process.Start("mailto:");
        }

        public static string Zip(string[] paths)
        {
            if(paths.Length == 0)
            {
                MessageBox.Show("圧縮したいファイルを1つ以上指定してください。");
                return "";
            }
            string zipfilename;
            var initialdir = Path.GetDirectoryName(paths[0]);
            var initialname = Path.GetFileName(initialdir);
            initialname = initialname == Application.ProductName ? Path.GetFileName(Path.GetDirectoryName(initialdir)) : initialname;
            initialname += $"-{paths.Length}枚の画像.zip";
            if (Settings.Default.ZipDialog)
            {

                var sfd = new SaveFileDialog();
                sfd.FileName = initialname;
                sfd.InitialDirectory = initialdir;
                sfd.Filter = "Zip アーカイブ(*.zip)|*.zip|すべてのファイル(*.*)|*.*";
                sfd.Title = "Zipアーカイブの保存場所を選択してください";
                sfd.OverwritePrompt = Settings.Default.OverwriteConfirm;
                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    return "";
                }
                zipfilename = sfd.FileName;
            }
            else
            {
                zipfilename = Path.Combine(initialdir, initialname);
                var p = zipfilename;
                int i = 2;
                while (File.Exists(zipfilename) || Directory.Exists(zipfilename))
                {
                    zipfilename = Path.Combine(Path.GetDirectoryName(p), Path.GetFileNameWithoutExtension(p) + $"({i++}).zip");
                }
            }


            var tempDir = Path.GetTempFileName();
            File.Delete(tempDir);
            var zipfiledir = Path.Combine(tempDir, Path.GetFileNameWithoutExtension(zipfilename));
            Directory.CreateDirectory(zipfiledir);
            foreach (var path in paths)
            {
                File.Copy(path, Path.Combine(zipfiledir, Path.GetFileName(path)));
            }
            ZipFile.CreateFromDirectory(zipfiledir, zipfilename);
            return zipfilename;
        }

    }
}
