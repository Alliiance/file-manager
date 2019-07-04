using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_manager
{
    class ListViewItem
    {
        private readonly string[] columns;

        public object State { get;  }

        public ListViewItem(object state , params string[] columns)
        {
            State = state;
            this.columns = columns;
        }

        internal void Render(List<int> columnsWidth,int elementIndex, int listViewX, int listViewY)
        {
            for (int i = 0; i < columns.Length; i++)
            {
                Console.CursorTop = elementIndex + listViewY;
                Console.CursorLeft = listViewX + columnsWidth.Take(i).Sum();
                Console.Write(GetStringWithLenght(columns[i], columnsWidth[i]));
            }
        }

        internal void Clean(List<int> columnsWidth, int i , int x , int y)
        {
            Console.CursorTop = i + y;
            Console.CursorLeft = x;
            Console.Write(new string(' ', columnsWidth.Sum()));
        }

        private string GetStringWithLenght(string v1, int maxLenght)
        {
            if (v1.Length < maxLenght)
                return v1.PadRight(maxLenght, ' ');
            else
                return v1.Substring(0, maxLenght - 5) + "[ ]";
        }
    }
}
