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
        string[] buttons = new string[] { "Button 1", "Button 2", "Button 3" , "Very Long button" };
        private int disc;
        private int countLуtter ;


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
                     f is FileInfo file ? file.Length.ToString() : ""
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
            }

            for (int i = 0; i < view.Length; i++)
                view[i].Render();

            for (int i = 0; i < buttons.Length; i++)
            {
                int letter = buttons[i].Length;
                DrawButtons(countLуtter + 1, 22, buttons[i]);
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
            var info = view.SelectedItem.State;
            view.Items = GetItems(path);
            view.CurrentState = path;
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
    }
}
