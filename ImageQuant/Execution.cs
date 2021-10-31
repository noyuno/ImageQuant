using ImageQuant.Properties;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;

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
            ClipboardCopy(paths);
            Process.Start("mailto:");
        }


        public static string Zip(string[] paths)
        {

            string zipfilename;
            var initialdir = Path.GetDirectoryName(paths[0]);
            var initialname = Path.GetFileName(initialdir);
            initialname = initialname == Application.ProductName ? Path.GetFileName(Path.GetDirectoryName(initialdir)) : initialname;
            initialname += $"-{paths.Length}枚の画像.zip";
            if (Settings.Default.ZipDialog)
            {

                var sfd = new SaveFileDialog
                {
                    FileName = initialname,
                    InitialDirectory = initialdir,
                    Filter = "Zip アーカイブ(*.zip)|*.zip|すべてのファイル(*.*)|*.*",
                    Title = "Zipアーカイブの保存場所を選択してください",
                    OverwritePrompt = Settings.Default.OverwriteConfirm
                };
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


        public static void ClipboardCopy(string[] items)
        {
            var files = new StringCollection();
            foreach (var item in items)
            {
                files.Add(item);
            }
            Clipboard.SetFileDropList(files);
        }

        public static void Trash(string[] vs)
        {
            foreach (var v in vs)
            {
                FileSystem.DeleteFile(v, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
            }
        }

        //public static object GetFileContext(string path, IntPtr hwnd)
        //{
            //if (!(File.Exists(path) || Directory.Exists(path)))
            //{
            //    throw new FileNotFoundException(path);
            //}
            //var instanceType = Type.GetTypeFromProgID("Shell.Application");
            //dynamic shell = Activator.CreateInstance(instanceType);
            //var num = Directory.GetFiles(Path.GetDirectoryName(path)).Length;
            
            //var ns = shell.Namespace(Path.GetDirectoryName(path)).Verb.GetUIObjectOf(shell, hwnd, num, )
            //return verbs;
        //}
    }
}
