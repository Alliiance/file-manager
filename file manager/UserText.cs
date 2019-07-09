using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_manager
{
    class UserText
    {
        int x = 45;
        int y = 27;

        public string CreateText(string name)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(name);      
            string text = Console.ReadLine();
            Clear();
            return text;
        }

        private void Clear()
        {
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.SetCursorPosition(i, y);
                Console.Write(" ");
            }
        }

    }
}
