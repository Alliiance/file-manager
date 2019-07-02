using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_manager
{
    class ListView
    {
        private int prevSelectedIndex;
        private int selectedIndex;
        private bool wasPainted;

        private int x, y;

        public ListView(int x , int y)
        {
            this.x = x;
            this.y = y;
        }
        public List<int> ColumnsWidth { get; set; }

        public List<ListViewItem> Items { get; set; }

        public void Clean()
        {
            wasPainted = false;
            for (int i = 0; i < Items.Count(); i++)
            {
                Console.CursorLeft = x;
                Console.CursorTop = i + y;
                Console.Write(new string(' ', Items[i].ToString().Length));
            }
        }

        public object SelectedItem { get; set;}
        public bool Focused { get; set; }

        public void Render()
        {
            for (int i = 0;i < Items.Count; i++){

                if (wasPainted) { 
                    if (i != selectedIndex && i != prevSelectedIndex)
                        continue;
                }

                var item = Items[i];
                var savedForeground = Console.ForegroundColor;
                var savedBackground = Console.BackgroundColor;
                if (i == selectedIndex){
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                Console.CursorLeft = x;
                Console.CursorTop = i + y;
                item.Render(ColumnsWidth , i , x , y);
                Console.Write(item); //

                Console.ForegroundColor = savedForeground;
                Console.BackgroundColor = savedBackground;
            }
            wasPainted = true;
        }

        internal void Update(ConsoleKeyInfo key)
        {

            prevSelectedIndex = selectedIndex;

            if (key.Key == ConsoleKey.DownArrow && selectedIndex + 1 < Items.Count())
                selectedIndex++;
            else if (key.Key == ConsoleKey.UpArrow && selectedIndex - 1 != 0)
                selectedIndex--;
        }
    }
}
