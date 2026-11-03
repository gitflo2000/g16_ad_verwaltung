using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace g16_ad_verwaltung
{
    class KaGenInfo
    {
        public static string pwList = "";

        public string lehrerName;
        public string suffix;
        public string ouName;
        public int count;

        public KaGenInfo(string teacherName, string suff, string nameOfOu, int number)
        {
            lehrerName = teacherName;
            suffix = suff;
            ouName = nameOfOu;
            count = number;
        }
    }
}
