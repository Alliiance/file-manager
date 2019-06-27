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

            var view = new ListView(10 , 2);
            view.Items = new DirectoryInfo("C:\\").GetFileSystemInfos().Select(f => f.Name).Cast<object>().ToList();

            while (true)
            {
                var key = Console.ReadKey();
                view.Update(key);

                view.Render();

            }
        }
    }
}
