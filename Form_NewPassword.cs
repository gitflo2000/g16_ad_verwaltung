using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace g16_ad_verwaltung
{
    public partial class Form_NewPassword : Form
    {
        public string newpassword;

        public Form_NewPassword()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                newpassword = textBox1.Text;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            this.Close();
        }

        private void Form_NewPassword_Shown(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
    }
}
