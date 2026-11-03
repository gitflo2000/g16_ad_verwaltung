using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace g16_ad_verwaltung
{
    public partial class Form_PickOU : Form
    {
        public DirectoryEntry destination;
        TreeNode origin;

        public Form_PickOU(TreeNode topnode, TreeNode sender)
        {
            InitializeComponent();
            treeView1.Nodes.Add(topnode);
            origin = sender;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                DirectoryEntry newParent = treeView1.SelectedNode.Tag as DirectoryEntry;
                if (newParent != null)
                {
                    destination = newParent;

                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Bitte gültiges Ziel wählen");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form_PickOU_Shown(object sender, EventArgs e)
        {
            treeView1.Nodes[0].Expand();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode.Name == origin.Name)
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }
    }
}
