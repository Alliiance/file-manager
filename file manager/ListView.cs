using System;
using System.Collections.Generic;
using System.IO;
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

        private int scroll;
        private int x, y, height;

        public ListView(int x , int y, int height)
        {
            this.x = x;
            this.y = y;
            this.height = height;
        }
        public List<int> ColumnsWidth { get; set; }

        public List<ListViewItem> newItems { get; set; }
        public List<ListViewItem> Items { get { return newItems; } set { scroll = 0; newItems = value; } }

        public void Clean()
        {
            selectedIndex = prevSelectedIndex = 0;
            wasPainted = false;
            for (int i = 0; i < Math.Min(height, Items.Count); i++)
            {
                Console.CursorLeft = x;
                Console.CursorTop = i + y;
                Items[i].Clean(ColumnsWidth, i, x, y);
            }
        }

        public ListViewItem SelectedItem => Items[selectedIndex];
        public bool Focused { get; set; }
        public object CurrentState { get; internal set; }

        public void Render()
        {
            for (int i = 0;i < Math.Min(height,Items.Count); i++){
                int elementIndex = i + scroll;

                if (wasPainted) { 
                    if (elementIndex != selectedIndex && elementIndex != prevSelectedIndex)
                        continue;
                }

                var item = Items[elementIndex];
                var savedForeground = Console.ForegroundColor;
                var savedBackground = Console.BackgroundColor;
                if (elementIndex == selectedIndex){
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.CursorLeft = x;
                Console.CursorTop = i + y;
                item.Render(ColumnsWidth , i , x , y);

                Console.ForegroundColor = savedForeground;
                Console.BackgroundColor = savedBackground;
            }
            wasPainted = true;
        }

        public void Update(ConsoleKeyInfo key)
        {
            prevSelectedIndex = selectedIndex;

            if (key.Key == ConsoleKey.UpArrow && selectedIndex > 0)
                selectedIndex--;
            if (key.Key == ConsoleKey.DownArrow && selectedIndex < Items.Count - 1)
                selectedIndex++;
            if (selectedIndex >= height + scroll)
            {
                scroll++;
                wasPainted = false;
            }
            else if (selectedIndex < scroll)
            {
                scroll--;
                wasPainted = false;
            }
            if (key.Key == ConsoleKey.Enter)
                Selected(this, EventArgs.Empty);

            if (key.Key == ConsoleKey.Backspace)  
                MoveBack(this, EventArgs.Empty);

            if (key.Key == ConsoleKey.F1)
                CopyFile(this, EventArgs.Empty);

            if (key.Key == ConsoleKey.F2)
                CutFile(this, EventArgs.Empty);

            if (key.Key == ConsoleKey.F3)
                PasteFile(this, EventArgs.Empty);

            if (key.Key == ConsoleKey.F4)
                Root(this, EventArgs.Empty);

            if (key.Key == ConsoleKey.F5)
                RootDisc(this, EventArgs.Empty);

            if (key.Key == ConsoleKey.F6)
                Properties(this, EventArgs.Empty);

            if (key.Key == ConsoleKey.F7)
                Rename(this, EventArgs.Empty);


            if (key.Key == ConsoleKey.F8)
                Delete(this, EventArgs.Empty);

            if (key.Key == ConsoleKey.F9)
                NewFolder(this, EventArgs.Empty);

        }

        public event EventHandler MoveBack;
        public event EventHandler Selected;
        public event EventHandler CopyFile;
        public event EventHandler CutFile;
        public event EventHandler PasteFile;
        public event EventHandler Root;
        public event EventHandler Properties;
        public event EventHandler Rename;
        public event EventHandler Delete;
        public event EventHandler NewFolder;
        public event EventHandler RootDisc; 
    }
}
