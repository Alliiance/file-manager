using System;
using System.Collections.Generic;
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

            var view = new ListView(10, 2);


            view.ColumnsWidth = new List<int> { Console.WindowWidth - 20,  -  10, 10 };
            view.Items = new DirectoryInfo("C:\\").GetFileSystemInfos().Select(f =>
            new ListViewItem(f,
            f.Name,
            f.Extension,
            f is FileInfo file ? file.Length.ToString() : "" 
            )).ToList();

            while (true)
            {
                var key = Console.ReadKey();
                view.Update(key);

                view.Render();

            }
        }
    }
}
