using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace g16_ad_verwaltung
{
    class rowData
    {
        private string rowName;
        private G16_Schueler schuelerObject;
        private DirectoryEntry destination;
        private string[] filepath;

        public rowData(ListViewItem before, G16_Schueler schueler, DirectoryEntry newParent = null, string[] fileSource = null)
        {
            filepath = fileSource ?? new string[0];
            destination = newParent;
            rowName = before.Name;
            schuelerObject = schueler;
            filepath = fileSource;
        }

        public string RowName
        {
            get
            {
                return rowName;
            }
        }

        public G16_Schueler Schueler
        {
            get
            {
                return schuelerObject;
            }
        }

        public DirectoryEntry Destination
        {
            get
            {
                return destination;
            }
        }

        public string[] Filepath
        {
            get
            {
                return filepath;
            }
        }
    }
}
