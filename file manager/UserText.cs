using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_manager
{
    class UserText
    {
        int x = 1;
        int y = 27;

        public string CreateText(string name)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(name);      
            string text = Console.ReadLine();
            Clear(y , 1);
            return text;
        }

        public void Clear(int y , int count)
        {
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < Console.WindowWidth; j++)
                {
                    Console.SetCursorPosition(j, y + i);
                    Console.Write(" ");
                }
            }
        }
    }
}
