using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;


namespace g16_ad_verwaltung
{
    class G16_Schueler
    {
        #region Konstanten

        // Fileserver-Settings
        private const string filepath_prefix = @"\\bs19hh.de\Unterricht";  //@"\\FS01";
        private const string pathTopOU = "G16Schueler";
        private const string pathTopDir = @"\Schuelerdaten";
        private const string homedrive = "H:";
        private const string securityGroupName = "G16Schueler";

        private const string kla_ou = "Kla";

        // User-Settings für DirectoryEntry


        // Debugging von ausserhalb der Domäne
        private static string root_path = null;
        private static string username = null;
        private static string password = null;


        #endregion


        #region Attribute:

        // Muss unbedingt static sein, da durch Systemzeit geseedet.
        private static Random rnd = new Random();

        private DirectoryEntry entry;
        private string type;
        private string filepath;
        private bool ka_account = false;

        #endregion


        #region Konstruktoren:

        public G16_Schueler(string kursname, string nachname, string vorname, string password, bool createDir = true)
        {

            // Alle Zeichen bis zur ersten Zahl auswählen.  Bsp.: "FIT3H" => "FIT"
            string pattern = @"\D*";
            string lehrgangsname = new Regex(pattern).Match(kursname).Value;

            if (lehrgangsname != "")
            {
                DirectoryEntry lehrgangsOU = CreateOU(GetTopOU(), lehrgangsname);
                DirectoryEntry kursOU;

                if (lehrgangsname == kursname)
                {
                    kursOU = lehrgangsOU;
                }
                else
                {
                    kursOU = CreateOU(lehrgangsOU, kursname);
                }
                 

                CreateGroup(kursOU, createDir);
                CreateUser(kursOU, nachname, vorname, password, createDir);
            }
           
        }

        public G16_Schueler(DirectoryEntry parentEntry, string nachname, string vorname, string password)
        {

            CreateUser(parentEntry, nachname, vorname, password);

        }

        public G16_Schueler(DirectoryEntry parentEntry, bool group_1_Ou_0, string ou_bezeichnung = "")
        {
            if (group_1_Ou_0)
            {
                type = "group";
                entry = CreateGroup(parentEntry);
            }
            else
            {
                entry = CreateOU(parentEntry, ou_bezeichnung);
            }
        }

        public G16_Schueler(DirectoryEntry currentEntry)
        {
            //vorhandenen Eintrag übernehmen
            entry = currentEntry;

        }

        #endregion



        #region Accesors:

        public G16_Schueler_Properties Properties
        {
            get
            {
                return new G16_Schueler_Properties(this);
            }
        }

        public string Type
        {
            get
            {

                if (type == null)
                {
                    try
                    {
                        type = GetEntryType();
                    }
                    catch
                    {
                        type = "Fehler";
                    }
                }
                return type;
            }
        }

        public DirectoryEntry Entry
        {
            get
            {
                return entry;
            }
        }

        public string FilePath
        {
            get
            {
                if (filepath == null)
                {
                    filepath = GetFilePath();
                }

                return filepath;
            }
        }

        public string PermissionIntegrity
        {
            get
            {
                try
                {
                    if (type == "user")
                    {
                        if (this.Properties.DriveLetter != homedrive)
                        {
                            return "Falscher Laufwerksbuchstabe";
                        }
                        else if (!String.Equals(this.Properties.HomeDirectory, FilePath,StringComparison.OrdinalIgnoreCase))
                        {
                            return "Falscher Pfad";
                        }
                    }
                    
                    if (!CheckDirectory())
                    {
                        return "Nicht vorhanden";
                    }
                    else if (!CheckDirectoryPermissions())
                    {
                        return "Keine Leseberechtigung";
                    }
                    else if (!ComparePermissions())
                    {
                        return "Rechte weichen ab";
                    }
                    else
                    {
                        return "ok";
                    }
                }
                catch
                {
                    return "Fehler";
                }
                
            }
            
        }

        public bool GroupMembership
        {
            // Prüfen ob in Klassen-Gruppe
            get
            {
                try
                {
                    return IsMemberOfCourse();
                }
                catch
                {
                    return false;
                }
            }
        }

        #endregion



        #region Public-Methoden:

        public static string GenerateKA(string klaOuName, string klaLehrername, string klaSuffix, int count)
        {
            string safeLastname = ReplaceSpecialChars(RemoveSpaces(klaLehrername));
            string safeOuName = ReplaceSpecialChars(RemoveSpaces(klaOuName));

            DirectoryEntry klaOu = CreateOU(GetTopOU(), kla_ou);

            DirectoryEntry currentOU = CreateOU(klaOu, safeOuName);

            CreateGroup(currentOU);
            
            int existentAccounts = 0;
            int nameSuffix = 1;
            string userList = "";
            //fgdfgdfgdfgd
            
            while(existentAccounts < count)
            {
                string accountName = safeOuName + nameSuffix.ToString("00");
                if (!AccountNameExists(accountName))
                {
                    // Account erstellen
                    string vorName = klaSuffix + nameSuffix.ToString("00");
                    string nachName = safeLastname;
                    string password = GeneratePW();
                    try
                    {
                        DirectoryEntry newKa = SetProperties(currentOU, vorName, nachName, accountName, password);

                        G16_Schueler temp_schueler = new G16_Schueler(newKa);
                        temp_schueler.CreateDirectory();
                        temp_schueler.Join();

                        userList += accountName + ": " + password + Environment.NewLine;
                    }
                    catch
                    {
                        break;
                    }
                }

                nameSuffix++;
                existentAccounts++;
            }

            return userList;

        }

        public static DirectoryEntry FindGroupThisLevel(DirectoryEntry parent)
        {

            DirectorySearcher dirSearcher = new DirectorySearcher(parent);

            dirSearcher.SearchScope = SearchScope.OneLevel;
            dirSearcher.Filter = "(&(objectClass=group)(cn=" + parent.Properties["ou"].Value.ToString() + "))";
            SearchResult result = dirSearcher.FindOne();

            if (result == null)
            {
                return null;
            }

            return result.GetDirectoryEntry();

        }

        public static DirectoryEntry FindOU(DirectoryEntry parentEntry, string name, bool search_subtree = false)
        {
            DirectorySearcher dirSearcher = new DirectorySearcher(parentEntry);

            if (search_subtree)
            {
                dirSearcher.SearchScope = SearchScope.Subtree;
            }
            else
            {
                dirSearcher.SearchScope = SearchScope.OneLevel;
            }
            dirSearcher.Filter = "(&(objectClass=organizationalUnit)(ou=" + name + "))";
            SearchResult result = dirSearcher.FindOne();

            if (result == null)
            {
                return null;
            }

            return result.GetDirectoryEntry();
        }

        public static DirectoryEntry CreateOU(DirectoryEntry parentEntry, string bezeichnung)
        {
            // Falls OU auf diesem Level schon vorhanden, wird diese zurückgegeben
            DirectoryEntry new_OU = FindOU(parentEntry, bezeichnung);
            if (new_OU == null)
            {
                new_OU = parentEntry.Children.Add("OU=" + bezeichnung, "organizationalUnit");
                new_OU.CommitChanges();
            }

            return new_OU;
        }

        public static DirectoryEntry CreateGroup(DirectoryEntry parentEntry, bool createDir = true)
        {
            // Prüfen ob in der aktuellen OU schon eine entsprechende Sicherheitsgruppe existiert
            DirectoryEntry new_group = FindGroupThisLevel(parentEntry);

            if (new_group == null)
            {
                string name = parentEntry.Properties["ou"].Value.ToString();

                try
                {
                    new_group = parentEntry.Children.Add("CN=" + name, "group");
                    new_group.Properties["sAmAccountName"].Value = name;
                    new_group.Properties["mail"].Value = "Klasse_" + name + "@bs19hh.de";  // E-Mail enabled Group
                    new_group.CommitChanges();

                    // Beim Erstellen der Sicherheitsgruppe das Temp-Verzeichnis direkt mit erstellen
                    G16_Schueler temp_schueler = new G16_Schueler(new_group);

                    if (createDir)
                    {
                        temp_schueler.CreateDirectory();
                    }
                    temp_schueler.Join();
                    return new_group;
                }
                catch
                {

                }
            }

            return new_group;
        }

        public static string GeneratePW()
        {
            int pwLength = 8;
            List<int[]> arrList = new List<int[]>();

            // Zeichen für Passwörter auswählen:
            // 0 - 9 (48 - 57)
            arrList.Add(Enumerable.Range(48, 10).ToArray());
            // A - Z (65 - 90)
            arrList.Add(Enumerable.Range(65, 26).ToArray());
            // a - z (97 - 122)
            arrList.Add(Enumerable.Range(97, 26).ToArray());

            // Alle int-Arrays mergen
            // (bzw. Union, entfernt doppelte Einträge)
            int[] finArr = { };
            foreach (int[] arr in arrList)
            {
                finArr = finArr.Union(arr).ToArray();
            }


            StringBuilder strBuild = new StringBuilder();

            while (strBuild.Length < pwLength)
            {
                strBuild.Append((Char)finArr[rnd.Next(0, finArr.Length)]);
            }


            return strBuild.ToString();
        }

        public void Move(DirectoryEntry newParent)
        {
            try
            {
                string oldFilePath = GetFilePath();
                filepath = null;
                if (Type == "user")
                {
                    LeaveGroup(FindGroupThisLevel(entry.Parent));
                    entry.MoveTo(newParent);
                    entry.CommitChanges();
                    entry.RefreshCache();
                    JoinGroup(FindGroupThisLevel(newParent));
                    string newParentFilePath = System.IO.Path.GetDirectoryName(FilePath);
                    if (!Directory.Exists(newParentFilePath))
                    {
                        G16_Schueler temp_schueler = new G16_Schueler(entry);
                        temp_schueler.CreateDirectory();
                        Directory.Delete(temp_schueler.FilePath);

                        // kleiner Work-Around um Permissions nicht nochmal explizit definieren zu müssen
                        // Stattdessen neues Objekt anlegen, Ordner erstellen und dann löschen
                    }
                    Directory.Move(oldFilePath, FilePath);
                    SetPermissions();
                    SetHomeDirectory();
                }
                else if (Type == "group")
                {
                    // Schwieriges Thema, wie mit dem Moven vom Temp-Verzeichnis umgegangen werden soll
                    // Directory.Move wirft eine Exception, wenn Quell- und Zielverzeichnis den selben Namen haben.
                    // Aber blind überschreiben ist auch doof. Hier ist händisch verschieben wohl sinnvoller.

                    /*
                    DirectoryEntry destinationGroup = CreateGroup(newParent);
                    G16_Schueler temp_schueler = new G16_Schueler(destinationGroup);
                    Directory.Move(oldFilePath, temp_schueler.FilePath + @"\..");
                    DirectoryEntry currentParent = entry.Parent;
                    currentParent.Children.Remove(entry);
                    currentParent.CommitChanges();
                     */
                }
                else
                {
                    // Ein OU-Objekt verschieben ? Ernsthaft ?
                }
            }
            catch
            {
                
            }
        }

        public void SetPermissions()
        {
            try
            {
                if (CheckDirectoryPermissions())
                {
                    Directory.SetAccessControl(FilePath, BuildPermissions());
                }
            }
            catch
            {

            }
        }

        public bool CheckDirectory()
        {
            try
            {
                if (Directory.Exists(FilePath))
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
            
        }

        public void CreateDirectory()
        {
            try
            {
                Directory.CreateDirectory(FilePath);
                SetPermissions();
                SetHomeDirectory();
            }
            catch
            {

            }

        }

        public bool ChangePassword(string password)
        {
            try
            {
                if (SetPassword(entry, password))
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public void Join()
        {
            try
            {
                if (Type == "user")
                {
                    JoinGroup(CreateGroup(entry.Parent));
                }
                else if (Type == "group")
                {
                    JoinGroup(FindGroupThisLevel(GetTopOU()));
                }
            }
            catch
            {

            }


        }

        public void Delete()
        {
            if (CheckDirectory())
            {
                Directory.Delete(FilePath, true);
            }

            entry.DeleteTree();
        }

        #endregion



        #region Private-Methoden:

        private string PathRecursion(DirectoryEntry adEntry, string path)
        {
            DirectoryEntry parent = adEntry.Parent;

            if(parent != null)
            {
                string entry_ou = parent.Properties["ou"].Value.ToString();
                if (entry_ou == pathTopOU)
                {
                    path = pathTopDir;
                }
                else
                {
                    if (entry_ou == kla_ou)
                    {
                        ka_account = true;
                    }
                    path = PathRecursion(parent, path);
                    path += @"\" + entry_ou;
                }
            }
            parent.Close();
            return path;
        }

        private string GetFilePath()
        {
            return filepath_prefix + PathRecursion(entry, filepath_prefix) + GetDirName();
        }

        private string GetDirName()
        {
            string dir_name = "";

            if (Type == "user")
            {
                dir_name = entry.Properties["sAMAccountName"][0].ToString();
            }
            else if (Type == "group")
            {
                dir_name = "temp";
            }
            else if (Type == "organizationalUnit")
            {
                dir_name = entry.Properties["ou"].Value.ToString();
            }
            else
            {
                dir_name = "";
            }


            return @"\" + dir_name;
        }

        private string GetEntryType()
        {
            return entry.SchemaClassName;
        }

        private bool CheckDirectoryPermissions()
        {
            if (CheckDirectory())
            {
                try
                {
                    DirectorySecurity dir_sec = Directory.GetAccessControl(FilePath);
                    return true;
                }
                catch
                {
                    return false;
                }

            }
            return false;
        }

        private SecurityIdentifier GetOwnSID()
        {
            SecurityIdentifier sid = new SecurityIdentifier(entry.Properties["objectSid"][0] as byte[], 0);

            return sid;
        }

        private SecurityIdentifier FindSID(string name, bool group_1_user_0)
        {
            string art = "";

            if (group_1_user_0)
            {
                art = "group";
            }
            else
            {
                art = "user";
            }


            DirectorySearcher dirSearcher = new DirectorySearcher(GetRootEntry());

            dirSearcher.PropertiesToLoad.Add("objectSid");
            dirSearcher.SearchScope = SearchScope.Subtree;
            dirSearcher.Filter = "(&(objectClass=" + art + ")(cn="+ name + "))";
            SearchResult result = dirSearcher.FindOne();
            
            SecurityIdentifier group_sid;
            if(result != null)
            {
                 group_sid = new SecurityIdentifier(result.Properties["objectSid"][0] as byte[], 0);
            }
            else
            {
                group_sid = null;
            }

            dirSearcher.Dispose();

            return group_sid;
        }

        private DirectorySecurity BuildPermissions()
        {
            DirectorySecurity dir_sec = new DirectorySecurity();

            if(Type == "user" || Type == "group")
            {
             /*
                // Für Temp- und User-Ordner Vererbung deaktivieren
                dir_sec.SetAccessRuleProtection(true, false);


                // Lokale Gruppe ADMINISTRATOREN
                dir_sec.AddAccessRule
                    (
                    new FileSystemAccessRule
                        (
                        new SecurityIdentifier("S-1-5-32-544"),
                        FileSystemRights.FullControl,
                        InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                        PropagationFlags.None,
                        AccessControlType.Allow
                        )
                    );

                // Gruppe Lehrer
                dir_sec.AddAccessRule
                    (
                    new FileSystemAccessRule
                        (
                        FindSID("Lehrer", true),
                        FileSystemRights.FullControl,
                        InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                        PropagationFlags.None,
                        AccessControlType.Allow
                        )
                    );

                */
                if (Type == "user")
                {
                    dir_sec.AddAccessRule
                        (
                        new FileSystemAccessRule
                            (
                            GetOwnSID(),
                            FileSystemRights.Modify,
                            InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                            PropagationFlags.None,
                            AccessControlType.Allow
                            )
                        );
                }

                if (Type == "group" && ka_account)
                {
                    dir_sec.AddAccessRule
                        (
                        new FileSystemAccessRule
                            (
                            GetOwnSID(),
                            FileSystemRights.ReadAndExecute,
                            InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                            PropagationFlags.None,
                            AccessControlType.Allow
                            )
                        );
                }

                if (Type == "group" && !ka_account)
                {

                 /*   // Lokale Gruppe EIGENTÜMERRECHTE (OWNER RIGHTS)
                    dir_sec.AddAccessRule
                        (
                        new FileSystemAccessRule
                            (
                            new SecurityIdentifier("S-1-3-4"),
                            FileSystemRights.Modify,
                            InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                            PropagationFlags.None,
                            AccessControlType.Allow
                            )
                        );
                 */
                    
                    // Gruppe mit Lesen/Ausführen
                    dir_sec.AddAccessRule
                        (
                        new FileSystemAccessRule
                            (
                            GetOwnSID(),
                            FileSystemRights.ReadAndExecute,
                            InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                            PropagationFlags.None,
                            AccessControlType.Allow
                            )
                        );

                    // Gruppe speziell nur für Ordner und Unterordner
                    dir_sec.AddAccessRule
                        (
                        new FileSystemAccessRule(
                           GetOwnSID(),
                           FileSystemRights.WriteData
                           | FileSystemRights.AppendData,
                           InheritanceFlags.ContainerInherit,
                           PropagationFlags.None,
                           AccessControlType.Allow
                           )
                        );

                 /*   // ERSTELLER-BESITZER
                    dir_sec.AddAccessRule
                        (
                        new FileSystemAccessRule
                            (
                            new SecurityIdentifier("S-1-3-0"),
                            FileSystemRights.ExecuteFile
                            | FileSystemRights.ListDirectory
                            | FileSystemRights.ReadAttributes
                            | FileSystemRights.ReadExtendedAttributes
                            | FileSystemRights.WriteData
                            | FileSystemRights.AppendData
                            | FileSystemRights.ReadPermissions
                            | FileSystemRights.WriteAttributes
                            | FileSystemRights.WriteExtendedAttributes
                            | FileSystemRights.Delete,
                            InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                            PropagationFlags.InheritOnly,
                            AccessControlType.Allow
                            )
                        );*/
                }
            }
            else
            {
                // Bei OUs Vererbung aktivieren
                dir_sec.SetAccessRuleProtection(false,true);
            }        

            return dir_sec;
        }

        private bool ComparePermissions()
        {
            if (!CheckDirectoryPermissions())
            {
                return false;
            }
            else
            {
                DirectorySecurity dir_sec_ist = Directory.GetAccessControl(FilePath);
                DirectorySecurity dir_sec_soll = BuildPermissions();

                foreach (FileSystemAccessRule rule_ist in dir_sec_ist.GetAccessRules(true, true, typeof(NTAccount)))
                {
                    foreach (FileSystemAccessRule rule_soll in dir_sec_soll.GetAccessRules(true, true, typeof(NTAccount)))
                    {
                        if (rule_ist.IdentityReference.Value == rule_soll.IdentityReference.Value 
                            && rule_ist.AccessControlType == rule_soll.AccessControlType 
                            && rule_ist.FileSystemRights == rule_soll.FileSystemRights 
                            && rule_ist.IsInherited == rule_soll.IsInherited 
                            && rule_ist.InheritanceFlags == rule_soll.InheritanceFlags)
                        {
                            try
                            {
                                dir_sec_ist.RemoveAccessRuleSpecific(rule_ist);
                                dir_sec_soll.RemoveAccessRuleSpecific(rule_soll);
                            }
                            catch
                            {
                                return false;                            
                            }
                        }
                    }
                }

                if (dir_sec_ist.GetAccessRules(true, true, typeof(NTAccount)).Count == 0 && dir_sec_soll.GetAccessRules(true, true, typeof(NTAccount)).Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }

        private void JoinGroup(DirectoryEntry group)
        {
            if (group != null)
            {
                group.Properties["member"].Add(entry.Properties["distinguishedName"].Value);
                group.CommitChanges();
                group.Close();
            }
        }

        private void LeaveGroup(DirectoryEntry group)
        {
            if (group != null)
            {
                group.Properties["member"].Remove(entry.Properties["distinguishedName"].Value);
                group.CommitChanges();
                group.Close();
            }
        }

        private void SetHomeDirectory()
        {
            if (Type == "user")
            {
                try
                {
                    entry.Properties["HomeDirectory"].Value = FilePath;
                    entry.Properties["HomeDrive"].Value = homedrive;

                    entry.CommitChanges();
                }
                catch
                {

                }
            }
        }

        private static bool AccountNameExists(string name)
        {
            DirectorySearcher dirSearcher = new DirectorySearcher(GetRootEntry());

            dirSearcher.SearchScope = SearchScope.Subtree;
            dirSearcher.Filter = "((samaccountname=" + name + "))";
            SearchResult result = dirSearcher.FindOne();

            if (result == null)
            {
                return false;
            }

            return true;
        }

        private static DirectoryEntry GetTopOU()
        {
            return FindOU(GetRootEntry(),pathTopOU, true);
        }

        private static DirectoryEntry GetRootEntry()
        {
            if (username == null || password == null)
            {
                return new DirectoryEntry();
            }
            else if (root_path != null)
            {
                return new DirectoryEntry(root_path, username, password);
            }
            

            return new DirectoryEntry(root_path);
            
        }

        private void CreateUser(DirectoryEntry parentEntry, string nachname, string vorname, string password, bool createDir = true)
        {
            string firstname = ReplaceSpecialChars(vorname);
            string lastname = ReplaceSpecialChars(nachname);
            string accountname = GenerateUserName(lastname, firstname);

            firstname = RemoveSpaces(firstname);
            lastname = RemoveSpaces(lastname);


            string offset = "";
            int count = 2;

            while (AccountNameExists(accountname + offset))
            {
                offset = count.ToString();
                count++;
            }

            accountname += offset;
            firstname += offset;


            entry = SetProperties(parentEntry, firstname, lastname, accountname, password);

            // Beim Erstellen des Users Home-Verzeichnis direkt mit erstellen
            if (createDir)
            {
                CreateDirectory();
            }

            // Beim Erstellen in die Gruppe O365_Schueler_A1 (CN=O365_Schueler_A1,OU=G16Schueler,DC=bs19hh,DC=de)
            
            DirectoryEntry o365_group = new DirectoryEntry("LDAP://CN=O365_Schueler_A1,OU=G16Schueler,DC=bs19hh,DC=de"); 
            
                JoinGroup(o365_group);
            

            DirectoryEntry group = FindGroupThisLevel(entry.Parent);
            if (group != null)
            {
                JoinGroup(group);
            }

        }

        private static DirectoryEntry SetProperties(DirectoryEntry parentEntry, string firstname, string lastname, string accountname, string password)
        {
            DirectoryEntry newEntry;
            
            newEntry = parentEntry.Children.Add("CN=" + lastname + firstname, "user");
            newEntry.Properties["cn"].Value = lastname + firstname;
            newEntry.Properties["company"].Value = "Berufliche Schule Farmsen BS19";
            newEntry.Properties["physicalDeliveryOfficeName"].Value = newEntry.Parent.Properties["ou"].Value;
            newEntry.Properties["department"].Value = "Schüler";
            newEntry.Properties["description"].Value = newEntry.Parent.Properties["ou"].Value;
            newEntry.Properties["displayName"].Value = firstname + " " + lastname;
            newEntry.Properties["givenName"].Value = firstname;
            newEntry.Properties["name"].Value = lastname + firstname;
            newEntry.Properties["sAmAccountName"].Value = accountname;
            newEntry.Properties["sn"].Value = lastname;
            newEntry.Properties["title"].Value = "Schüler";
            newEntry.Properties["userPrincipalName"].Value = accountname + "@bs19hh.de";
            newEntry.Properties["mail"].Value = accountname + "@bs19hh.de";  // set @bs19-E-Mail-Address

            newEntry.CommitChanges();

            SetPassword(newEntry, password);

            int val = (int)newEntry.Properties["userAccountControl"].Value;
            newEntry.Properties["userAccountControl"].Value = val & ~0x2;
            
            newEntry.CommitChanges();

            return newEntry;
        }

        private string GenerateUserName(string nachname, string vorname)
        {           
            string accountname = new Regex("([a-zA-Z]{1,6})[a-zA-Z]*$").Match(nachname).Groups[1].Value 
                                + new Regex("^([a-zA-Z]{1,3})[a-zA-Z]*").Match(vorname).Groups[1].Value;

            return accountname;
        }

        private static string ReplaceSpecialChars(string text)
        {
            // Umlaute + ß entfernen

            text = text.Replace("ä", "ae");
            text = text.Replace("ö", "oe");
            text = text.Replace("ü", "ue");
            text = text.Replace("Ä", "Ae");
            text = text.Replace("Ö", "Oe");
            text = text.Replace("Ü", "Ue");
            text = text.Replace("ß", "ss");

            // Akzentuierung entfernen
            // http://code.commongroove.com/2011/04/29/c-string-extension-to-replace-accented-characters/

            string sourceInFormD = text.Normalize(NormalizationForm.FormD);

            var output = new StringBuilder();
            foreach (char c in sourceInFormD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                    output.Append(c);
            }

            return (output.ToString().Normalize(NormalizationForm.FormC));
        
        }

        private static string RemoveSpaces(string text)
        {
            return text.Replace(" ","_");
        }

        private static bool SetPassword(DirectoryEntry userEntry, string password)
        {
            if (userEntry.SchemaClassName == "user")
            {
                try
                {
                    userEntry.Invoke("SetPassword", new object[] { password });
                    userEntry.Properties["LockOutTime"].Value = 0;
                    // userEntry.Properties["pwdLastSet"].Value = 0;
                    userEntry.CommitChanges();
                    return true;
                }
                catch
                {

                }
                return false;
            }
            return false;
        }

        private bool IsMemberOfCourse()
        {
            string path;

            if (root_path != null)
            {
                path = root_path + "/";
            }
            else
            {
                path = "LDAP://";
            }

            if (Type == "user")
            {

                DirectoryEntry group = FindGroupThisLevel(entry.Parent);

                foreach (string item in GetGroupMemberships(new ArrayList(), false))
                {
                    if (group.Path == path + item)
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                DirectoryEntry group = FindGroupThisLevel(GetTopOU());

                if (group.Path == entry.Path)
                {
                    return true;
                }

                foreach (string item in GetGroupMemberships(new ArrayList(), false))
                {
                    if (group.Path == path + item)
                    {
                        return true;
                    }
                }
            }


            return false;
        }

        private ArrayList GetGroupMemberships(ArrayList memberships, bool recursion)
        {
            entry.RefreshCache(new string [1] {"memberOf"});
            PropertyValueCollection ValueCollection = entry.Properties["memberOf"];
            IEnumerator enumeration = ValueCollection.GetEnumerator();

            while (enumeration.MoveNext())
            {
                if (enumeration.Current != null)
                {
                    if (!memberships.Contains(enumeration.Current.ToString()))
                    {
                        memberships.Add(enumeration.Current.ToString());
                        if (recursion)
                        {
                            GetGroupMemberships(memberships, true);
                        }
                    }
                }
            }

            return memberships;
        }

        #endregion


    }
}
