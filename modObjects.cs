using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace g16_ad_verwaltung
{
    enum ObjectOperations { createDir, setPermissions, joinGroup, move, delete, group, randomPW, copyFrom };

    class modObjects
    {
        private List<rowData> objectcollection;
        private ObjectOperations operation;

        public modObjects(ObjectOperations job, List<rowData> listViewItems)
        {
            objectcollection = listViewItems;
            operation = job;
        }

        public ObjectOperations Operation
        {
            get
            {
                return operation;
            }
        }

        public List<rowData> ObjectCollection
        {
            get
            {
                return objectcollection;
            }
        }
    }
}
