using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_manager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            var view = new ListView(20, 2, height: 20);
            view.ColumnsWidth = new List<int> { 50, 10, 10 };

            view.Items = GetItems("C:\\");
            view.Selected += View_Selected;

            while (true)
            {
                var key = Console.ReadKey();
                view.Update(key);

                view.Render();
            }

            private static List<ListViewItem> GetItems(string path){
                return new DirectoryInfo(path).GetFileSystemInfos()
                    .Select(f => new ListViewItem(
                        f,
                        f.Name,
                        f is DirectoryInfo dir ? "<dir>" : f.Extension,
                        f is FileInfo file ? file.Length.ToString() : ""
                      )).ToList();
            }

            private static void View_Selected(object sender, EventArgs e)
            {
                var view = (ListView)sender;
                var info = view.SelectedItem.State;
                if (info is FileInfo file)
                    Process.Start(file.FullName);
                else if(info is DirectoryInfo dir)
                {
                    view.Clean();
                    view.Items = GetItems(dir.FullName);
                }

            }
        }
    }
}
