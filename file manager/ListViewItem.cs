using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_manager
{
    class ListViewItem
    {
        public object State { get;  }

        public ListViewItem(object state , params string[] columns)
        {
            State = state;
        }


    }
}
