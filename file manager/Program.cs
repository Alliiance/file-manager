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
            DirectoryControl directoryControl = new DirectoryControl();
            directoryControl.Start();
        }
    }
}
