using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace g16_ad_verwaltung
{
    class wait
    {
        static Form_Wait objectList = new Form_Wait();
        public static Form_Wait ObjectList
        {
            get
            {
                return objectList;
            }
        }

        static Form_Wait treeOU = new Form_Wait();
        public static Form_Wait OuTree
        {
            get
            {
                return treeOU;
            }
        }

        static Form_Wait kaGen = new Form_Wait();
        public static Form_Wait KaGen
        {
            get
            {
                return kaGen;
            }
        }

        static Form_Wait userCreate = new Form_Wait();
        public static Form_Wait UserCreate
        {
            get
            {
                return userCreate;
            }
        }
    }
}
