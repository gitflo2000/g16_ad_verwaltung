using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace g16_ad_verwaltung
{
    class G16_Schueler_Properties
    {
        private G16_Schueler schueler;
        
        public G16_Schueler_Properties(G16_Schueler schuelerObject)
        {
            schueler = schuelerObject;
        }

        private string GetValue(string property)
        {
            try
            {
                return schueler.Entry.Properties[property].Value.ToString();
            }
            catch
            {
                
            }

            return "";
        }

        public string Username
        {
            get
            {
                return GetValue("sAMAccountName");
            }
        }

        public string Firstname
        {
            get
            {
                return GetValue("givenName");
            }
        }

        public string Lastname
        {
            get
            {
                return GetValue("sn");
            }
        }

        public string Fullname
        {
            get
            {
                return GetValue("displayName");
            }
        }

        public string HomeDirectory
        {
            get
            {
                return GetValue("HomeDirectory");
            }
        }

        public string DriveLetter
        {
            get
            {
                return GetValue("HomeDrive");
            }
        }

    }
}
