namespace g16_ad_verwaltung
{
    partial class Form_ADBrowser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.benutzerErstellenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kABenutzerErstellenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aktualisierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.treeView_OUs = new System.Windows.Forms.TreeView();
            this.listView_Objects = new System.Windows.Forms.ListView();
            this.username = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.directoryStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.inGroup = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuObjectList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ordnerÖffnenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ordnerErstellenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rechteSetzenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gruppeBeitretenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.passwortÄndernToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zufallsPasswortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.hierherKopierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dateienToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ordnerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verschiebenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.löschenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuOuList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.oUErstellenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gruppeErstellenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.löschenToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.bgW_TreeOU = new System.ComponentModel.BackgroundWorker();
            this.bgW_ObjectList = new System.ComponentModel.BackgroundWorker();
            this.bgW_FirstConnect = new System.ComponentModel.BackgroundWorker();
            this.bgW_ObjectOperations = new System.ComponentModel.BackgroundWorker();
            this.bgW_KACreation = new System.ComponentModel.BackgroundWorker();
            this.bgW_TreeOperations = new System.ComponentModel.BackgroundWorker();
            this.HomePath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DriveLetter = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.contextMenuObjectList.SuspendLayout();
            this.contextMenuOuList.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.benutzerErstellenToolStripMenuItem,
            this.kABenutzerErstellenToolStripMenuItem,
            this.aktualisierenToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // benutzerErstellenToolStripMenuItem
            // 
            this.benutzerErstellenToolStripMenuItem.Name = "benutzerErstellenToolStripMenuItem";
            this.benutzerErstellenToolStripMenuItem.Size = new System.Drawing.Size(112, 20);
            this.benutzerErstellenToolStripMenuItem.Text = "Benutzer erstellen";
            this.benutzerErstellenToolStripMenuItem.Click += new System.EventHandler(this.benutzerErstellenToolStripMenuItem_Click);
            // 
            // kABenutzerErstellenToolStripMenuItem
            // 
            this.kABenutzerErstellenToolStripMenuItem.Name = "kABenutzerErstellenToolStripMenuItem";
            this.kABenutzerErstellenToolStripMenuItem.Size = new System.Drawing.Size(132, 20);
            this.kABenutzerErstellenToolStripMenuItem.Text = "KA-Benutzer erstellen";
            this.kABenutzerErstellenToolStripMenuItem.Click += new System.EventHandler(this.kABenutzerErstellenToolStripMenuItem_Click);
            // 
            // aktualisierenToolStripMenuItem
            // 
            this.aktualisierenToolStripMenuItem.Name = "aktualisierenToolStripMenuItem";
            this.aktualisierenToolStripMenuItem.Size = new System.Drawing.Size(87, 20);
            this.aktualisierenToolStripMenuItem.Text = "Aktualisieren";
            this.aktualisierenToolStripMenuItem.Click += new System.EventHandler(this.aktualisierenToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 708);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1008, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(22, 17);
            this.toolStripStatusLabel1.Text = "     ";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 24);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.treeView_OUs);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.listView_Objects);
            this.splitContainer2.Size = new System.Drawing.Size(1008, 684);
            this.splitContainer2.SplitterDistance = 167;
            this.splitContainer2.TabIndex = 0;
            // 
            // treeView_OUs
            // 
            this.treeView_OUs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_OUs.HideSelection = false;
            this.treeView_OUs.HotTracking = true;
            this.treeView_OUs.Location = new System.Drawing.Point(0, 0);
            this.treeView_OUs.Name = "treeView_OUs";
            this.treeView_OUs.Size = new System.Drawing.Size(167, 684);
            this.treeView_OUs.TabIndex = 0;
            this.treeView_OUs.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_OUs_AfterSelect);
            this.treeView_OUs.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_OUs_NodeMouseClick);
            // 
            // listView_Objects
            // 
            this.listView_Objects.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.username,
            this.name,
            this.type,
            this.inGroup,
            this.directoryStatus,
            this.DriveLetter,
            this.HomePath});
            this.listView_Objects.ContextMenuStrip = this.contextMenuObjectList;
            this.listView_Objects.Cursor = System.Windows.Forms.Cursors.Default;
            this.listView_Objects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_Objects.FullRowSelect = true;
            this.listView_Objects.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView_Objects.HideSelection = false;
            this.listView_Objects.Location = new System.Drawing.Point(0, 0);
            this.listView_Objects.Name = "listView_Objects";
            this.listView_Objects.ShowGroups = false;
            this.listView_Objects.Size = new System.Drawing.Size(837, 684);
            this.listView_Objects.TabIndex = 0;
            this.listView_Objects.UseCompatibleStateImageBehavior = false;
            this.listView_Objects.View = System.Windows.Forms.View.Details;
            // 
            // username
            // 
            this.username.Text = "Username";
            this.username.Width = 96;
            // 
            // name
            // 
            this.name.Text = "Name";
            this.name.Width = 149;
            // 
            // type
            // 
            this.type.Text = "Typ";
            // 
            // directoryStatus
            // 
            this.directoryStatus.Text = "Dir-Status";
            this.directoryStatus.Width = 58;
            // 
            // inGroup
            // 
            this.inGroup.Text = "In Gruppe";
            this.inGroup.Width = 62;
            // 
            // contextMenuObjectList
            // 
            this.contextMenuObjectList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ordnerÖffnenToolStripMenuItem,
            this.toolStripSeparator1,
            this.ordnerErstellenToolStripMenuItem,
            this.rechteSetzenToolStripMenuItem,
            this.gruppeBeitretenToolStripMenuItem,
            this.passwortÄndernToolStripMenuItem,
            this.zufallsPasswortToolStripMenuItem,
            this.toolStripSeparator2,
            this.hierherKopierenToolStripMenuItem,
            this.verschiebenToolStripMenuItem,
            this.löschenToolStripMenuItem});
            this.contextMenuObjectList.Name = "contextMenuObjectList";
            this.contextMenuObjectList.Size = new System.Drawing.Size(171, 214);
            this.contextMenuObjectList.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuObjectList_Opening);
            // 
            // ordnerÖffnenToolStripMenuItem
            // 
            this.ordnerÖffnenToolStripMenuItem.Name = "ordnerÖffnenToolStripMenuItem";
            this.ordnerÖffnenToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.ordnerÖffnenToolStripMenuItem.Text = "Ordner öffnen";
            this.ordnerÖffnenToolStripMenuItem.Click += new System.EventHandler(this.ordnerÖffnenToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(167, 6);
            // 
            // ordnerErstellenToolStripMenuItem
            // 
            this.ordnerErstellenToolStripMenuItem.Name = "ordnerErstellenToolStripMenuItem";
            this.ordnerErstellenToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.ordnerErstellenToolStripMenuItem.Text = "Ordner erstellen";
            this.ordnerErstellenToolStripMenuItem.Click += new System.EventHandler(this.ordnerErstellenToolStripMenuItem_Click);
            // 
            // rechteSetzenToolStripMenuItem
            // 
            this.rechteSetzenToolStripMenuItem.Name = "rechteSetzenToolStripMenuItem";
            this.rechteSetzenToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.rechteSetzenToolStripMenuItem.Text = "Rechte setzen";
            this.rechteSetzenToolStripMenuItem.Click += new System.EventHandler(this.rechteSetzenToolStripMenuItem_Click);
            // 
            // gruppeBeitretenToolStripMenuItem
            // 
            this.gruppeBeitretenToolStripMenuItem.Name = "gruppeBeitretenToolStripMenuItem";
            this.gruppeBeitretenToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.gruppeBeitretenToolStripMenuItem.Text = "Gruppe beitreten";
            this.gruppeBeitretenToolStripMenuItem.Click += new System.EventHandler(this.gruppeBeitretenToolStripMenuItem_Click);
            // 
            // passwortÄndernToolStripMenuItem
            // 
            this.passwortÄndernToolStripMenuItem.Name = "passwortÄndernToolStripMenuItem";
            this.passwortÄndernToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.passwortÄndernToolStripMenuItem.Text = "Passwort ändern...";
            this.passwortÄndernToolStripMenuItem.Click += new System.EventHandler(this.passwortÄndernToolStripMenuItem_Click);
            // 
            // zufallsPasswortToolStripMenuItem
            // 
            this.zufallsPasswortToolStripMenuItem.Name = "zufallsPasswortToolStripMenuItem";
            this.zufallsPasswortToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.zufallsPasswortToolStripMenuItem.Text = "Zufalls-Passwort";
            this.zufallsPasswortToolStripMenuItem.Visible = false;
            this.zufallsPasswortToolStripMenuItem.Click += new System.EventHandler(this.zufallsPasswortToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(167, 6);
            // 
            // hierherKopierenToolStripMenuItem
            // 
            this.hierherKopierenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateienToolStripMenuItem,
            this.ordnerToolStripMenuItem});
            this.hierherKopierenToolStripMenuItem.Name = "hierherKopierenToolStripMenuItem";
            this.hierherKopierenToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.hierherKopierenToolStripMenuItem.Text = "Hierher kopieren";
            this.hierherKopierenToolStripMenuItem.Visible = false;
            this.hierherKopierenToolStripMenuItem.Click += new System.EventHandler(this.hierherKopierenToolStripMenuItem_Click);
            // 
            // dateienToolStripMenuItem
            // 
            this.dateienToolStripMenuItem.Name = "dateienToolStripMenuItem";
            this.dateienToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.dateienToolStripMenuItem.Text = "Dateien...";
            this.dateienToolStripMenuItem.Click += new System.EventHandler(this.dateienToolStripMenuItem_Click);
            // 
            // ordnerToolStripMenuItem
            // 
            this.ordnerToolStripMenuItem.Name = "ordnerToolStripMenuItem";
            this.ordnerToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.ordnerToolStripMenuItem.Text = "Ordner...";
            this.ordnerToolStripMenuItem.Click += new System.EventHandler(this.ordnerToolStripMenuItem_Click);
            // 
            // verschiebenToolStripMenuItem
            // 
            this.verschiebenToolStripMenuItem.Name = "verschiebenToolStripMenuItem";
            this.verschiebenToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.verschiebenToolStripMenuItem.Text = "Verschieben...";
            this.verschiebenToolStripMenuItem.Click += new System.EventHandler(this.verschiebenToolStripMenuItem_Click);
            // 
            // löschenToolStripMenuItem
            // 
            this.löschenToolStripMenuItem.Name = "löschenToolStripMenuItem";
            this.löschenToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.löschenToolStripMenuItem.Text = "Löschen...";
            this.löschenToolStripMenuItem.Click += new System.EventHandler(this.löschenToolStripMenuItem_Click);
            // 
            // contextMenuOuList
            // 
            this.contextMenuOuList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oUErstellenToolStripMenuItem,
            this.gruppeErstellenToolStripMenuItem,
            this.toolStripSeparator3,
            this.löschenToolStripMenuItem1});
            this.contextMenuOuList.Name = "contextMenuOuList";
            this.contextMenuOuList.Size = new System.Drawing.Size(161, 76);
            // 
            // oUErstellenToolStripMenuItem
            // 
            this.oUErstellenToolStripMenuItem.Name = "oUErstellenToolStripMenuItem";
            this.oUErstellenToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.oUErstellenToolStripMenuItem.Text = "OU erstellen...";
            this.oUErstellenToolStripMenuItem.Click += new System.EventHandler(this.oUErstellenToolStripMenuItem_Click);
            // 
            // gruppeErstellenToolStripMenuItem
            // 
            this.gruppeErstellenToolStripMenuItem.Name = "gruppeErstellenToolStripMenuItem";
            this.gruppeErstellenToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.gruppeErstellenToolStripMenuItem.Text = "Gruppe erstellen";
            this.gruppeErstellenToolStripMenuItem.Click += new System.EventHandler(this.gruppeErstellenToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(157, 6);
            // 
            // löschenToolStripMenuItem1
            // 
            this.löschenToolStripMenuItem1.Name = "löschenToolStripMenuItem1";
            this.löschenToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
            this.löschenToolStripMenuItem1.Text = "Löschen...";
            this.löschenToolStripMenuItem1.Click += new System.EventHandler(this.löschenToolStripMenuItem1_Click);
            // 
            // bgW_TreeOU
            // 
            this.bgW_TreeOU.WorkerSupportsCancellation = true;
            this.bgW_TreeOU.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgW_TreeOU_DoWork);
            // 
            // bgW_ObjectList
            // 
            this.bgW_ObjectList.WorkerReportsProgress = true;
            this.bgW_ObjectList.WorkerSupportsCancellation = true;
            this.bgW_ObjectList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgW_ObjectList_DoWork);
            this.bgW_ObjectList.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgW_ObjectList_ProgressChanged);
            this.bgW_ObjectList.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgW_ObjectList_RunWorkerCompleted);
            // 
            // bgW_FirstConnect
            // 
            this.bgW_FirstConnect.WorkerReportsProgress = true;
            this.bgW_FirstConnect.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgW_FirstConnect_DoWork);
            this.bgW_FirstConnect.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgW_FirstConnect_ProgressChanged);
            // 
            // bgW_ObjectOperations
            // 
            this.bgW_ObjectOperations.WorkerReportsProgress = true;
            this.bgW_ObjectOperations.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgW_ObjectOperations_DoWork);
            this.bgW_ObjectOperations.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgW_ObjectOperations_ProgressChanged);
            this.bgW_ObjectOperations.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgW_ObjectOperations_RunWorkerCompleted);
            // 
            // bgW_KACreation
            // 
            this.bgW_KACreation.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgW_KACreation_DoWork);
            this.bgW_KACreation.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgW_KACreation_RunWorkerCompleted);
            // 
            // bgW_TreeOperations
            // 
            this.bgW_TreeOperations.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgW_TreeOperations_DoWork);
            this.bgW_TreeOperations.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgW_TreeOperations_RunWorkerCompleted);
            // 
            // HomePath
            // 
            this.HomePath.Text = "Pfad";
            this.HomePath.Width = 350;
            // 
            // DriveLetter
            // 
            this.DriveLetter.Text = "";
            this.DriveLetter.Width = 23;
            // 
            // Form_ADBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "Form_ADBrowser";
            this.Text = "ADBrowser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_ADBrowser_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.contextMenuObjectList.ResumeLayout(false);
            this.contextMenuOuList.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TreeView treeView_OUs;
        private System.Windows.Forms.ListView listView_Objects;
        private System.Windows.Forms.ColumnHeader name;
        private System.Windows.Forms.ColumnHeader type;
        private System.Windows.Forms.ColumnHeader directoryStatus;
        private System.Windows.Forms.ColumnHeader inGroup;
        private System.ComponentModel.BackgroundWorker bgW_TreeOU;
        private System.ComponentModel.BackgroundWorker bgW_ObjectList;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.ComponentModel.BackgroundWorker bgW_FirstConnect;
        private System.Windows.Forms.ColumnHeader username;
        private System.Windows.Forms.ToolStripMenuItem benutzerErstellenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kABenutzerErstellenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aktualisierenToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuObjectList;
        private System.Windows.Forms.ToolStripMenuItem ordnerÖffnenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ordnerErstellenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rechteSetzenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gruppeBeitretenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem passwortÄndernToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip contextMenuOuList;
        private System.Windows.Forms.ToolStripMenuItem löschenToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem verschiebenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem löschenToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker bgW_ObjectOperations;
        private System.Windows.Forms.ToolStripMenuItem gruppeErstellenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oUErstellenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.ComponentModel.BackgroundWorker bgW_KACreation;
        private System.ComponentModel.BackgroundWorker bgW_TreeOperations;
        private System.Windows.Forms.ToolStripMenuItem zufallsPasswortToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hierherKopierenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dateienToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ordnerToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader HomePath;
        private System.Windows.Forms.ColumnHeader DriveLetter;


    }
}

