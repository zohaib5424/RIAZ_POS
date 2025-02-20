using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace RJ
{
    public partial class Backup : Form
    {
        public Backup()
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
                    textBox1.Text = dlg.SelectedPath;
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
            textBox1.ReadOnly = true;
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
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = dlg.SelectedPath;
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
            try
            {
                string database = con.Database.ToString();
                if (textBox1.Text == string.Empty)
                {
                    MessageBox.Show("please select location first");
                    timer2.Start();
                }
                else
                {
                    string query = "BACKUP DATABASE [" + database + "] TO DISK='" + textBox1.Text + "\\" + "database" + "-" + DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss") + ".bak'";

                    if (con.State.ToString() == "Closed")
                    {
                        con.Open();
                    }
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    timer1.Start();
                    //MessageBox.Show("Backup Successfully Saved");
                    circularProgressBar1.Enabled = false;
                    textBox1.Text = "";
                }

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

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
