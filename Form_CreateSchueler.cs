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
    public partial class Form_CreateSchueler : Form
    {
        DataTable userTable = new DataTable();

        string[] column_text = { "Kursname", "Nachname", "Vorname", "Passwort" };
        string[] column_names = { "coursename", "lastname", "firstname", "password" };
        string[] file_columns = { "S_K", "Name", "Vorname", "Geburtsdat" };

        bool createdSchueler = false;


        public Form_CreateSchueler()
        {
            InitializeComponent();

            dataGridView1.DataSource = userTable;

            for (int i = 0; i < column_names.Length; i++)
            {
                DataColumn column = userTable.Columns.Add();
                column.ColumnName = column_names[i];
                dataGridView1.Columns[i].HeaderText = column_text[i];
            }

            dataGridView1.Columns["coursename"].FillWeight = 75;
            dataGridView1.RowHeadersWidth = 25;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            OpenFileDialog fileDialog = sender as OpenFileDialog;

            foreach (string filename in fileDialog.FileNames)
            {
                addFileContent(userTable, filename);
            }
        }

        private void addFileContent(DataTable table, string filename)
        {
            try
            {
                string fileContent = System.IO.File.ReadAllText(filename);

                string[] delim = { "\r\n", "\n" };
                string[] fileRows = null;
                string[] headlines = null;

                fileRows = fileContent.Split(delim, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < fileRows.Length; i++)
                {
                    if (i == 0)
                    {
                        headlines = fileRows[0].Split(';');
                    }
                    else
                    {
                        DataRow newRow = userTable.Rows.Add();


                        string[] rowContents = fileRows[i].Split(';');

                        for (int k = 0; k < rowContents.Length; k++)
                        {
                            newRow[getTableColumnName(headlines[k])] = rowContents[k];
                        }
                    }
                }
            }
            catch
            {
                string errorMsg = "Ungültiges Format." + Environment.NewLine + Environment.NewLine;
                errorMsg += "Datei muss aus folgenden " + file_columns.Length.ToString();
                errorMsg += " Spalten bestehen:" + Environment.NewLine;

                foreach (string name in file_columns)
                {
                    errorMsg += name + " ";
                }
                errorMsg += Environment.NewLine + Environment.NewLine + "Das Trennzeichen ist \";\"";

                MessageBox.Show(errorMsg);
            }

        }

        private string getTableColumnName(string headline)
        {
            for (int i = 0; i < file_columns.Length; i++)
            {
                if (file_columns[i] == headline)
                {
                    return column_names[i];
                }
            }

            return "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Schuelerobjekte anlegen

            List<DataRow> rowList = new List<DataRow>();

            foreach (DataRow row in userTable.Rows)
            {
                bool notEmpty = true;

                foreach (string name in column_names)
                {
                    if (row[name].ToString() == "")
                    {
                        notEmpty = false;
                        break;
                    }
                }

                if (notEmpty)
                {
                    rowList.Add(row);
                }
                else
                {
                    continue;
                }
            }

            

            List<UserParams> userList = new List<UserParams>();

            foreach (DataRow row in rowList)
            {
                createdSchueler = true;
                string kurs = row["coursename"].ToString();
                string nachname = row["lastname"].ToString();
                string firstname = row["firstname"].ToString();
                string password = row["password"].ToString();

                userList.Add(new UserParams(kurs, nachname, firstname, password));

                row.Delete();
            }


            
            bgW_CreateSchueler.RunWorkerAsync(userList);

            if (createdSchueler)
            {
                wait.UserCreate.ShowDialog();

                if (userTable.Rows.Count == 0)
                {
                    this.Close();
                }
            }

        }

        private void bgW_CreateSchueler_DoWork(object sender, DoWorkEventArgs e)
        {
            List<UserParams> userList = e.Argument as List<UserParams>;

            foreach (UserParams Params in userList)
            {
                G16_Schueler temp_schueler = new G16_Schueler(Params.CourseName, Params.LastName, Params.FirstName, Params.Password);
            }
        }

        private void bgW_CreateSchueler_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            wait.UserCreate.Visible = false;
        }

        private void Form_CreateSchueler_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (createdSchueler)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}
