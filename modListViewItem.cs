using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace g16_ad_verwaltung
{
    class modListViewItem
    {
        private string rowbefore;
        private ListViewItem rowafter;

        public modListViewItem(string before, ListViewItem after)
        {
            rowbefore = before;
            rowafter = after;
        }

        public string Before
        {
            get
            {
                return rowbefore;
            }
        }

        public ListViewItem After
        {
            get
            {
                return rowafter;
            }
        }
    }
}
