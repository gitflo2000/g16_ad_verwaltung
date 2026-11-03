using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace g16_ad_verwaltung
{
    public partial class Form_ADBrowser : Form
    {
        DirectoryEntry topOUEntry;
        List<BackgroundWorker> allWorkers = new List<BackgroundWorker>();

        Boolean connectAccomplished = false;

        // Debugging von ausserhalb der Domäne
        string entry_path = null;
        string entry_username = null;
        string entry_password = null;

        bool use_credentials = false;

        public Form_ADBrowser()
        {
            InitializeComponent();


            allWorkers.Add(bgW_TreeOU);
            allWorkers.Add(bgW_ObjectList);

            PropertyInfo aProp = typeof(ListView).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
            aProp.SetValue(listView_Objects, true, null);

            bgW_FirstConnect.RunWorkerAsync();
        }


        private void connectToAD()
        {
            if (use_credentials)
            {
                topOUEntry = new DirectoryEntry(entry_path, entry_username, entry_password);
            }
            else
            {
                topOUEntry = new DirectoryEntry();
            }

            foreach(SearchResult result in searchDirectory(topOUEntry, "(&(objectClass=organizationalUnit)(ou=G16Schueler))", SearchScope.OneLevel))
            {
                topOUEntry = result.GetDirectoryEntry();
                return;
            }

            MessageBox.Show("OU G16Schueler nicht gefunden.");
        }

        private SearchResultCollection searchDirectory(DirectoryEntry entry, string filter = "((objectClass=organizationalUnit))", SearchScope scope = SearchScope.Subtree, string sortPropertyName = "ou")
        {
            DirectorySearcher dirSearch = new DirectorySearcher(entry);
            dirSearch.SearchScope = scope;
            dirSearch.Filter = filter;

            if (entry.SchemaClassName != "organizationalUnit")
            {
                dirSearch.Sort = new SortOption(sortPropertyName, SortDirection.Ascending);
            }
            

            return dirSearch.FindAll();
        }


        private void bgW_TreeOU_DoWork(object sender, DoWorkEventArgs e)
        {
            if (connectAccomplished)
            {
                DirectoryEntry dirEntry = topOUEntry;
                TreeNode topNode = new TreeNode();

                topNode.Tag = dirEntry;
                topNode.Text = dirEntry.Properties["ou"].Value.ToString();
                //topNode.Name = dirEntry.Path;
                topNode.Name = dirEntry.Guid.ToString();


                this.Invoke(new EventHandler(delegate
                {
                    treeView_OUs.Nodes.Add(topNode);
                }));

                bool first = true;

                foreach (SearchResult result in searchDirectory(dirEntry, "(&(objectClass=organizationalUnit)(!ou=G16Schueler))"))
                {
                    DirectoryEntry entry = result.GetDirectoryEntry();
                    TreeNode childNode = new TreeNode();

                    childNode.Tag = entry;
                    childNode.Text = entry.Properties["ou"].Value.ToString();
                    childNode.Name = entry.Guid.ToString();
                    //childNode.Name = entry.Path;
                    //string parent_path = entry.Parent.Path;
                    string parent_path = entry.Parent.Guid.ToString();



                    this.Invoke(new EventHandler(delegate
                    {
                        TreeNode[] tnC; 
                        tnC = treeView_OUs.Nodes.Find(parent_path, true);

                        if (tnC.Length != 0)
                        {
                            tnC[0].Nodes.Add(childNode);
                        }
                        else
                        {
                            treeView_OUs.Nodes.Add(childNode);
                            MessageBox.Show(treeView_OUs.Nodes.Find(parent_path, true).Length.ToString());
                        }
                                               
                    }));


                    if (first)
                    {
                        this.Invoke(new EventHandler(delegate
                        {
                            treeView_OUs.Nodes[0].Expand();
                        }));
                        first = false;
                    }
                }
                Action act = () => treeView_OUs.Sort();
                treeView_OUs.Invoke(act);
            }
        }

        private void treeView_OUs_AfterSelect(object sender, TreeViewEventArgs e)
        {
            updateListView();
        }

        private void updateListView()
        {
            if (bgW_ObjectOperations.IsBusy)
            {
                wait.ObjectList.ShowDialog();
            }

            if (bgW_ObjectList.IsBusy)
            {
                bgW_ObjectList.CancelAsync();

                while (bgW_ObjectList.IsBusy)
                {
                    Application.DoEvents();
                }
            }
            listView_Objects.Items.Clear();

            TreeNode selectedNode = treeView_OUs.SelectedNode;

            if (selectedNode != null)
            {
                bgW_ObjectList.RunWorkerAsync(selectedNode.Tag);
            }
        }

        private void bgW_ObjectList_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker current_bgW = sender as BackgroundWorker;
            DirectoryEntry selected_entry = e.Argument as DirectoryEntry;



            SearchResultCollection results = searchDirectory(selected_entry, "(|(objectClass=user)(objectClass=group))", SearchScope.OneLevel, "sAMAccountName");

            int i = 1;
            int result_count = results.Count;

            foreach (SearchResult result in results)
            {
                DirectoryEntry entry = result.GetDirectoryEntry();
                G16_Schueler schueler = new G16_Schueler(entry);




                current_bgW.ReportProgress((int)((float)i / (float)result_count * 100), createRow(schueler));

                if (current_bgW.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                i++;
            }

        }

        private void Form_ADBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {


            foreach (BackgroundWorker bgW in allWorkers)
            {
                if (bgW.IsBusy)
                {
                    bgW.CancelAsync();
                }
            }

            if (bgW_ObjectOperations.IsBusy)
            {
                wait.ObjectList.ShowDialog();
            }

            if (bgW_KACreation.IsBusy)
            {
                wait.KaGen.Show();
            }

            if (bgW_TreeOperations.IsBusy)
            {
                wait.OuTree.Show();
            }


            foreach (BackgroundWorker bgw in allWorkers)
            {
                while (bgw.IsBusy)
                {
                    Application.DoEvents();
                }
            }
        }

        private void bgW_ObjectList_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ListViewItem row = e.UserState as ListViewItem;

            listView_Objects.Items.Add(row);
            toolStripStatusLabel1.Text = e.ProgressPercentage.ToString("00") + "%";
            statusStrip1.Refresh();

        }

        private void bgW_ObjectList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            updateStatusLabel();
        }

        private void updateStatusLabel()
        {
            int count = listView_Objects.Items.Count;
            toolStripStatusLabel1.Text = count.ToString() + " Objekt" + (count == 1 ? "" : "e");
            statusStrip1.Refresh();
        }

        private void bgW_FirstConnect_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                connectToAD();

                BackgroundWorker current_bgW = sender as BackgroundWorker;
                current_bgW.ReportProgress(0,true);
                if (!bgW_TreeOU.IsBusy)
                {
                    bgW_TreeOU.RunWorkerAsync();
                }
            }
            catch(Exception f)
            {
                MessageBox.Show(f.Message);
            }
        }

        private void bgW_FirstConnect_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            connectAccomplished = (Boolean)e.UserState;
        }

        private void aktualisierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bgW_ObjectOperations.IsBusy)
            {
                wait.ObjectList.ShowDialog();
            }
            aktualisieren();
        }

        private void aktualisieren()
        {
            // WIP:
            // Problemstelle, falls Server während der Laufzeit herunterfährt
            // Dabei werden die Antworten etwas wirr
            
            if (connectAccomplished)
            {
                if (!bgW_TreeOU.IsBusy && !bgW_ObjectOperations.IsBusy)
                {
                    listView_Objects.Items.Clear();
                    treeView_OUs.Nodes.Clear();
                    bgW_TreeOU.RunWorkerAsync();
                }
            }
            else
            {
                if (!bgW_FirstConnect.IsBusy)
                {
                    bgW_FirstConnect.RunWorkerAsync();
                }
            }
        }

        private void ordnerÖffnenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            G16_Schueler temp_schueler =  listView_Objects.SelectedItems[0].Tag as G16_Schueler;

            string filepath = temp_schueler.Type == "user" ? temp_schueler.Properties.HomeDirectory : temp_schueler.FilePath;
            if (filepath != "")
            {
                Process.Start("explorer.exe", filepath);
            }
        }

        private void contextMenuObjectList_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip currentContextMenu = (ContextMenuStrip)sender;
            ListView currentListView = (ListView)currentContextMenu.SourceControl;

            if (currentListView.SelectedItems.Count == 0)
            {
                e.Cancel = true;
            }
            else if (currentListView.SelectedItems.Count > 1)
            {
                currentContextMenu.Items["ordnerÖffnenToolStripMenuItem"].Enabled = false;
                currentContextMenu.Items["passwortÄndernToolStripMenuItem"].Enabled = false;
            }
            else
            {
                G16_Schueler temp_schueler =  listView_Objects.SelectedItems[0].Tag as G16_Schueler;
                if (temp_schueler.CheckDirectory())
                {
                    currentContextMenu.Items["ordnerÖffnenToolStripMenuItem"].Enabled = true;
                }
                else
                {
                    currentContextMenu.Items["ordnerÖffnenToolStripMenuItem"].Enabled = false;
                }

                if (temp_schueler.Type == "user")
                {
                    currentContextMenu.Items["passwortÄndernToolStripMenuItem"].Enabled = true;
                }
                else
                {
                    currentContextMenu.Items["passwortÄndernToolStripMenuItem"].Enabled = false;
                }
            }
        }

        private void löschenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (showConfirmationForm())
            {
                DirectoryEntry currentOU = treeView_OUs.SelectedNode.Tag as DirectoryEntry;

                bgW_TreeOperations.RunWorkerAsync(currentOU);

                wait.OuTree.Show();

                treeView_OUs.SelectedNode.Remove();
            }
        }

        private void treeView_OUs_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                treeView_OUs.SelectedNode = e.Node;
                if (treeView_OUs.SelectedNode == treeView_OUs.Nodes[0])
                {
                    contextMenuOuList.Items[3].Enabled = false;
                }
                else
                {
                    contextMenuOuList.Items[3].Enabled = true;
                }
                contextMenuOuList.Show(Cursor.Position);
            }
        }

        private void bgW_ObjectOperations_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = false;

            bool first = true;

            modObjects job = e.Argument as modObjects;


            foreach (rowData row in job.ObjectCollection)
            {
                string before = row.RowName;
                ListViewItem after = null;

                if (job.Operation == ObjectOperations.createDir)
                {
                    row.Schueler.CreateDirectory();
                    after = createRow(row.Schueler);
                }
                else if (job.Operation == ObjectOperations.setPermissions)
                {
                    row.Schueler.SetPermissions();
                    after = createRow(row.Schueler);
                }
                else if (job.Operation == ObjectOperations.joinGroup)
                {
                    if (first)
                    {
                        if (G16_Schueler.FindGroupThisLevel(row.Schueler.Entry.Parent) == null)
                        {
                            e.Result = true;
                        }
                        first = false;
                    }
                    row.Schueler.Join();
                    after = createRow(row.Schueler);
                }
                else if (job.Operation == ObjectOperations.move)
                {
                    if (row.Schueler.Type == "user")
                    {
                        row.Schueler.Move(row.Destination);
                        after = null;
                    }
                    else
                    {
                        after = createRow(row.Schueler);
                    }
                }
                else if (job.Operation == ObjectOperations.delete)
                {
                    if (row.Schueler.Type == "group")
                    {
                        e.Result = true;
                    }
                    row.Schueler.Delete();
                    after = null;
                }
                else if (job.Operation == ObjectOperations.randomPW)
                {
                    row.Schueler.ChangePassword(G16_Schueler.GeneratePW());
                }


                if (after != null || job.Operation == ObjectOperations.delete || job.Operation == ObjectOperations.move)
                {
                    bgW_ObjectOperations.ReportProgress(0, new modListViewItem(before, after));
                }
            }


        }

        private void bgW_ObjectOperations_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            modListViewItem rows = e.UserState as modListViewItem;
            ListViewItem rowBefore = listView_Objects.Items[rows.Before];

            if (rows.After == null)
            {
                rowBefore.Remove();
            }
            else
            {
                listView_Objects.Items[rowBefore.Index] = rows.After;
            }

            updateStatusLabel();

        }

        private List<rowData> createRowData(DirectoryEntry destination = null, string[] sourcePath = null)
        {
            sourcePath = sourcePath ?? new string[0];
            List<rowData> liste = new List<rowData>();

            foreach (ListViewItem item in listView_Objects.SelectedItems)
            {
                G16_Schueler temp_schueler = item.Tag as G16_Schueler;
                liste.Add(new rowData(item, temp_schueler, destination, sourcePath));
            }

            return liste;
        }

        private ListViewItem createRow(G16_Schueler schueler)
        {

            ListViewItem row = new ListViewItem(schueler.Properties.Username);

            row.Name = schueler.Entry.Path;
            row.SubItems.Add(schueler.Properties.Fullname);
            row.SubItems.Add(schueler.Type);
            row.SubItems.Add(schueler.GroupMembership ? "ja" : "Nein");
            row.SubItems.Add(schueler.PermissionIntegrity);
            row.SubItems.Add(schueler.Properties.DriveLetter);
            row.SubItems.Add(schueler.Properties.HomeDirectory);
            row.Tag = schueler;
            // Laufwerksbuchstabe
            // Laufwerkspfad
            // 

            return row;

        }

        private void ObjectToolstripSend(ObjectOperations operation)
        {
            if (bgW_ObjectOperations.IsBusy)
            {
                wait.ObjectList.ShowDialog();
            }

            bgW_ObjectOperations.RunWorkerAsync(new modObjects(operation, createRowData()));
        }

        private void ordnerErstellenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectToolstripSend(ObjectOperations.createDir);
        }

        private void rechteSetzenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectToolstripSend(ObjectOperations.setPermissions);
        }

        private void gruppeBeitretenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectToolstripSend(ObjectOperations.joinGroup);
        }

        private void passwortÄndernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bgW_ObjectOperations.IsBusy)
            {
                wait.ObjectList.ShowDialog();
            }

            Form_NewPassword passwordForm = new Form_NewPassword();
            passwordForm.ShowDialog();

            if (passwordForm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                G16_Schueler temp_schueler = listView_Objects.SelectedItems[0].Tag as G16_Schueler;


                if (temp_schueler.ChangePassword(passwordForm.newpassword))
                {
                    MessageBox.Show("Passwort erfolgreich geändert!");
                }
                else
                {
                    MessageBox.Show("Fehler!" + Environment.NewLine + "Werden die Komplexitätsanforderungen erfüllt?");
                }

            }
        }

        private void zufallsPasswortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectToolstripSend(ObjectOperations.randomPW);
        }

        private void verschiebenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bgW_ObjectOperations.IsBusy)
            {
                wait.ObjectList.ShowDialog();
            }

            TreeNode topNode = treeView_OUs.Nodes[0];
            if (topNode != null)
            {
                Form_PickOU ziel = new Form_PickOU(topNode.Clone() as TreeNode, treeView_OUs.SelectedNode);
                ziel.ShowDialog();

                if (ziel.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    DirectoryEntry newParent = ziel.destination;
                    bgW_ObjectOperations.RunWorkerAsync(new modObjects(ObjectOperations.move, createRowData(newParent)));
                }
            }
        }

        private void löschenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bgW_ObjectOperations.IsBusy)
            {
                wait.ObjectList.ShowDialog();
            }

            if (showConfirmationForm())
            {
                bgW_ObjectOperations.RunWorkerAsync(new modObjects(ObjectOperations.delete, createRowData()));
            }
        }

        private bool showConfirmationForm()
        {
            Form_ConfirmDelete deleteDialog = new Form_ConfirmDelete();
            deleteDialog.ShowDialog();

            return (deleteDialog.DialogResult == System.Windows.Forms.DialogResult.Yes);
        }

        private void kABenutzerErstellenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bgW_ObjectOperations.IsBusy)
            {
                wait.ObjectList.ShowDialog();
            }

            if (connectAccomplished)
            {
                Form_GenerateKA klassenarbeit = new Form_GenerateKA();
                klassenarbeit.ShowDialog();

                if (klassenarbeit.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    string lehrerName = klassenarbeit.teachername;
                    string suffix = klassenarbeit.suffix;
                    string ouName = "Ka" + lehrerName + suffix;
                    int count = klassenarbeit.count;


                    bgW_KACreation.RunWorkerAsync(new KaGenInfo(lehrerName, suffix, ouName, count));
                    wait.KaGen.ShowDialog();

                    DirectoryEntry klaOU = G16_Schueler.FindOU(topOUEntry, "Kla");
                    if (klaOU != null)
                    {
                        DirectoryEntry createdOU = G16_Schueler.FindOU(klaOU, ouName);
                        if (createdOU != null)
                        {
                            TreeNode[] klaNode = treeView_OUs.Nodes.Find(klaOU.Guid.ToString(), true);

                            if (klaNode != null && klaNode.Count() > 0)
                            {
                                TreeNode[] existentOUs = treeView_OUs.Nodes.Find(createdOU.Guid.ToString(), true);

                                if (existentOUs.Length == 0)
                                {
                                    TreeNode newOu = new TreeNode();
                                    newOu.Tag = createdOU;
                                    newOu.Text = createdOU.Properties["ou"].Value.ToString();
                                    newOu.Name = createdOU.Guid.ToString();
                                    klaNode[0].Nodes.Add(newOu);

                                    treeView_OUs.SelectedNode = newOu;
                                }
                                treeView_OUs.SelectedNode = null;
                                treeView_OUs.SelectedNode = treeView_OUs.Nodes.Find(createdOU.Guid.ToString(), true).First();
                            }
                            else
                            {
                                aktualisieren();
                            }
                        }
                    }

                    if (KaGenInfo.pwList != "")
                    {
                        Form_UserList passwords = new Form_UserList(ouName, KaGenInfo.pwList);
                        passwords.ShowDialog();
                    }
                }

            }
        }

        private void gruppeErstellenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DirectoryEntry parent = treeView_OUs.SelectedNode.Tag as DirectoryEntry;
            
            if (G16_Schueler.FindGroupThisLevel(parent) == null)
            {
                G16_Schueler newGroup = new G16_Schueler(G16_Schueler.CreateGroup(parent));
                ListViewItem newRow = createRow(newGroup);
                listView_Objects.Items.Add(newRow);
            }
            
        }

        private void oUErstellenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bgW_ObjectOperations.IsBusy)
            {
                wait.ObjectList.Show();
            }

            TreeNode currentNode = treeView_OUs.SelectedNode;
            DirectoryEntry parentEntry = currentNode.Tag as DirectoryEntry;
            Form_AddOU ouForm = new Form_AddOU();
            ouForm.ShowDialog();

            if (ouForm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                DirectoryEntry searchOU = G16_Schueler.FindOU(parentEntry, ouForm.ouName);
                if (searchOU != null)
                {
                    MessageBox.Show("OU bereits vorhanden!");
                }
                else
                {
                    DirectoryEntry newOU = G16_Schueler.CreateOU(parentEntry, ouForm.ouName);
                    TreeNode newNode = currentNode.Nodes.Add(ouForm.ouName);
                    newNode.Tag = newOU;
                    newNode.Name = newOU.Path;

                    currentNode.TreeView.Sort();
                    currentNode.TreeView.SelectedNode = newNode;
                }
                
            }
        }

        private void bgW_ObjectOperations_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            wait.ObjectList.Visible = false;

            if ((bool) e.Result == true)
            {
                updateListView();
            }
            
        }

        private void bgW_KACreation_DoWork(object sender, DoWorkEventArgs e)
        {
            KaGenInfo input = e.Argument as KaGenInfo;
            KaGenInfo.pwList = G16_Schueler.GenerateKA(input.ouName, input.lehrerName, input.suffix, input.count);
        }

        private void bgW_KACreation_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            wait.KaGen.Visible = false;
        }

        private void bgW_TreeOperations_DoWork(object sender, DoWorkEventArgs e)
        {
            DirectoryEntry currentOU = e.Argument as DirectoryEntry;
            G16_Schueler temp_schueler = new G16_Schueler(currentOU);
            temp_schueler.Delete();
        }

        private void bgW_TreeOperations_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            wait.OuTree.Visible = false;
        }

        private void benutzerErstellenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (connectAccomplished)
            {
                if (bgW_ObjectOperations.IsBusy)
                {
                    wait.ObjectList.ShowDialog();
                }

                if (bgW_TreeOperations.IsBusy)
                {
                    wait.OuTree.Show();
                }

                Form_CreateSchueler createDialog = new Form_CreateSchueler();
                createDialog.ShowDialog();

                if (createDialog.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    aktualisieren();
                }

            }
        }

        private void hierherKopierenToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dateienToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (bgW_ObjectOperations.IsBusy)
            {
                wait.ObjectList.ShowDialog();
            }

            OpenFileDialog openFile = new OpenFileDialog();

            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                bgW_ObjectOperations.RunWorkerAsync(new modObjects(ObjectOperations.copyFrom, createRowData(null, openFile.FileNames)));
            }
        }

        private void ordnerToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (bgW_ObjectOperations.IsBusy)
            {
                wait.ObjectList.ShowDialog();
            }

            FolderBrowserDialog openFile = new FolderBrowserDialog();

            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                bgW_ObjectOperations.RunWorkerAsync(new modObjects(ObjectOperations.copyFrom, createRowData(null, new string[] { openFile.SelectedPath })));
            }
        }
   
    }

}
