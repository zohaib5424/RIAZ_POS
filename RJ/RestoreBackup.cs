using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using WinFormAnimation;
using System.IO;

namespace RJ
{
    public partial class RestoreBackup : Form
    {
        public RestoreBackup()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = dlg.SelectedPath;
                    circularProgressBar1.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void Backup_Load(object sender, EventArgs e)
        {
            textBox2.ReadOnly = true;
            label1.Hide();
            circularProgressBar1.Value = 0;
        }

        private void metroTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "SQL SERVER database backup files|*.bak";
                dlg.Title = "Database restore";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = dlg.FileName;
                    circularProgressBar1.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void circularProgressBar1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim().Length <= 0)
            {
                MessageBox.Show("Select backup file to restore");
                return;
            }
            else if(textBox2.Text.Contains(System.IO.Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)).ToString()))
            {
                MessageBox.Show("Restore Backup From Other Than Windows Drive.\nCopy backup at different drive and restore again");
                return;
            }
            try
            {
                //timer1.Start();
                ////MessageBox.Show("Backup Successfully Saved");
                //circularProgressBar1.Enabled = false;
                string database = con.Database.ToString();
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                string sqlStmt2 = string.Format("USE MASTER ALTER DATABASE [" + database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
                SqlCommand bu2 = new SqlCommand(sqlStmt2, con);
                bu2.ExecuteNonQuery();

                string sqlStmt3 = "USE MASTER RESTORE DATABASE [" + database + "] FROM DISK='" + textBox2.Text + "'WITH REPLACE;";
                SqlCommand bu3 = new SqlCommand(sqlStmt3, con);
                bu3.ExecuteNonQuery();

                string sqlStmt4 = string.Format("ALTER DATABASE [" + database + "] SET MULTI_USER");
                SqlCommand bu4 = new SqlCommand(sqlStmt4, con);
                bu4.ExecuteNonQuery();

                con.Close();
                MessageBox.Show("Restoring Successfully Complete");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        int count = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (circularProgressBar1.Value < 100)
                circularProgressBar1.Value += 2;
            if(circularProgressBar1.Value >=100)
            {
                count++;
                if (count == 36)
                {
                    timer1.Stop();
                    count = 0;
                    label1.Hide();
                    circularProgressBar1.Value = 0;
                }
                if(count%2==0 && count >6)
                    label1.Show();
                else
                    label1.Hide();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        int count1 = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            count1++;
            if (count1 % 2 == 0)
                metroTile1.Show();
            else
                metroTile1.Hide();
            if (count1 == 8)
            {
                timer2.Stop();
                count1 = 0;
                metroTile1.Show();
            }
        }
    }
}
