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
                Console.Write(columns[i].PadRight(columnsWidth[i], ' '));
            }
        }
    }
}
