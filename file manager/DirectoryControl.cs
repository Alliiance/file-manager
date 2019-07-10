using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_manager
{
    class DirectoryControl
    {
        private readonly ListView[] view;
        string[] buttons = new string[] { "F1 - copy", "F2 - cut", "F3 - paste", "F4 - root", "F5 - list of disks", "F6 - properties", "F7 - rename", "F8 - find", "F9 - new folder" };
        private int disc;
        private int countLуtter;
        public string copiedFilePath;
        UserText userText = new UserText();
        public DirectoryControl()
        {
            view = new ListView[2];
        }

        private static List<ListViewItem> GetItems(string path)
        {
            return new DirectoryInfo(path).GetFileSystemInfos()
                .Select(f => new ListViewItem(
                    f,
                    f.Name,
                    f is DirectoryInfo dir ? "<dir>" : f.Extension,
                    f is FileInfo file ? FileSize(file.Length) : "folder"
                  )).ToList();
        }

        internal void Start()
        {
            for (int i = 0; i < view.Length; i++)
            {
                view[i] = new ListView(1 + Console.WindowWidth / view.Length * i, 1, height: 20);
                view[i].ColumnsWidth = new List<int> { 35, 7, 10 };
                view[i].Items = GetItems("C:\\");
                view[i].CurrentState = "C:\\";
                view[i].Selected += View_Selected;
                view[i].MoveBack += View_MoveBack;
                view[i].NewFolder += View_NewFolder;
                view[i].Root += View_Root;
                view[i].RootDisc += View_ListOfDiscs;
                view[i].CopyFile += View_CopyFile;
                view[i].PasteFile += View_PasteFile;
                view[i].Rename += View_Rename;
                view[i].FindFile += View_FindFile;
            }

            for (int i = 0; i < view.Length; i++)
                view[i].Render();

            for (int i = 0; i < buttons.Length; i++)
            {
                int letter = buttons[i].Length;
                DrawButtons(countLуtter + 1, 24, buttons[i]);
                countLуtter += letter + 1;
            }

            while (true)
            {
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.RightArrow && disc < view.Length - 1)
                    disc++;
                else if (key.Key == ConsoleKey.LeftArrow && disc != 0)
                    disc--;
                else
                    view[disc].Update(key);
                view[disc].Render();
            }
        }

        private void View_FindFile(object sender, EventArgs e)
        {
            string text = userText.CreateText("Enter the name of the file you want to find: ");
            //Redraw();
        }

        private void View_Root(object sender, EventArgs e)
        {
           string patch  = ((System.IO.DirectoryInfo)view[0].SelectedItem.State).Root.FullName;
           view[disc].Items = GetItems(patch);
           view[disc].CurrentState = patch;        
        }

        private void View_CopyFile(object sender, EventArgs e)
        {
            copiedFilePath = view[disc].CurrentState + "" + view[disc].SelectedItem.State;
        }

        private void View_PasteFile(object sender, EventArgs e)
        {
            string SourcePath = copiedFilePath;
            string DestinationPath = view[disc].CurrentState + "\\" + view[disc].SelectedItem.State;

            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
        }

        private void View_NewFolder(object sender, EventArgs e)
        {
            string text = userText.CreateText("Create new folter: ");
            string path = view[disc].CurrentState + "/" + text;
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
                dirInfo.Create();

            Redraw();

        }
        
        private void View_Rename(object sender, EventArgs e)
        {
            string newName = userText.CreateText("Create new name: ");
            string path = ((System.IO.DirectoryInfo)view[disc].SelectedItem.State).FullName;
            string newPath =  view[disc].CurrentState + "//";

            try
            {
                Directory.Move(path, newPath + newName);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            Redraw();
        }

        private static void View_Selected(object sender, EventArgs e)
        {
            var view = (ListView)sender;
            var info = view.SelectedItem.State;
            if (info is FileInfo file)
                Process.Start(file.FullName);
            else if (info is DirectoryInfo dir)
            {
                try
                {
                    var items = GetItems(dir.FullName);
                    view.Clean();
                    view.Items = items;
 
                    view.CurrentState = dir.FullName;
                }
                catch (Exception)
                {

                }
            }
        }

        private static void View_MoveBack(object sender, EventArgs e)
        {
            var view = (ListView)sender;
            string path = Path.GetDirectoryName(view.CurrentState.ToString());
            if (path == null)
                return;
            view.Clean();

            view.Items = GetItems(path);
            view.CurrentState = path;
        }

        private static void View_ListOfDiscs(object sender, EventArgs e)
        {
            var listView = (ListView)sender;
            listView.Clean();
            listView.Items = GetDrives();
        }

        private static List<ListViewItem> GetDrives()
        {
            string[] drives = Directory.GetLogicalDrives();
            List<ListViewItem> result = new List<ListViewItem>();

            foreach (var drive in drives)
            {
                result.Add(new ListViewItem(new DirectoryInfo(drive), drive, "<drive>", ""));
            }
            return result;
        }


        private static void DrawButtons(int x , int y, string text)
        {
            Console.SetCursorPosition(x, y);
            var savedBackgroundColor = Console.BackgroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.Write(text);
            Console.BackgroundColor = savedBackgroundColor;
        }

        public static string FileSize(long length)
        {
            if (length < 10240)
                return length.ToString() + " Byte";
            else if (length < 1024 * 1024 * 10)
                return (length / 1024).ToString() + " KB";
            else if (length < (long)1024 * 1024 * 1024 * 10)
                return (length / 1024 / 1024).ToString() + " MB";
            else
                return (length / 1024 / 1024 / 1024).ToString() + " GB";
        }

        public void Redraw()
        {
            view[disc].Clean();
            view[disc].Items = GetItems(view[disc].CurrentState.ToString());
        }
    }
}
