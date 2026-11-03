using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace g16_ad_verwaltung
{
    public partial class Form_UserList : Form
    {
        string filename;

        public Form_UserList(string ouName, string userlist)
        {
            InitializeComponent();
            textBox1.Text = userlist;
            filename = ouName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog speichern = new SaveFileDialog();
            speichern.AddExtension = true;
            speichern.DefaultExt = ".txt";
            speichern.Filter = "Text|*.txt|Alle Dateitypen|*";
            speichern.FileName = filename;

            if (speichern.ShowDialog() == DialogResult.OK)
            {
            using (StreamWriter sw = new StreamWriter(speichern.FileName))
                sw.WriteLine(textBox1.Text);
                this.Close();
            }
        }
    }
}
