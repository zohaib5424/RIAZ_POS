using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace RJ
{
    public partial class SetPattern : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public SetPattern()
        {
            InitializeComponent();
        }

        private void ClassType_Load(object sender, EventArgs e)
        {
            lockScreenControl1.SetPassCode(gmpasscode);
        }

        int[] gmpasscode = { 9};

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (panel2.Location.X >= (this.Width - panel2.Width))
            //{
            //    panel2.Location = new Point(panel2.Location.X - 10, panel2.Location.Y);
            //}
        }


        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void Brands_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose(true);
            }
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose(true);
        }

        GMDB gm = new GMDB();
        public int setpattern = 0;
        int attemptcount = 0;
        private void metroTile5_Click(object sender, EventArgs e)
        {
            try
            {
                {
                    //string query = "update login set password='"+textBox2.Text.Trim()+"' where id = '"+I_Bee_Mini_Mart.Properties.Settings.Default.loginid+"'";
                    //gm.ExecuteNonQuery(query);
                    //MessageBox.Show("Password Successfully Set");
                    //setpattern = 1;
                    //this.Dispose(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroLabel4_Click(object sender, EventArgs e)
        {

        }

        string code1 = "";
        string code2 = "";
        private void lockScreenControl1_PassCodeSubmitted(object sender, GestureLockApp.GestureLockControl.PassCodeSubmittedEventArgs e)
        {
            if (e.Valid)
            {
            }
            else
            {
                if (attemptcount == 0)
                {
                    for (int i = 0; i < e.Code.Count(); i++)
                    {
                        if (i == e.Code.Count() - 1)
                        {
                            code1 += e.Code[i].ToString();
                        }
                        else
                        {
                            code1 += e.Code[i].ToString()+",";
                        }
                    }
                    metroLabel1.Text = "Re-Draw Your Pattern";
                }
                if (attemptcount == 1)
                {

                    for (int i = 0; i < e.Code.Count(); i++)
                    {
                        if (i == e.Code.Count() - 1)
                        {
                            code2 += e.Code[i].ToString();
                        }
                        else
                        {
                            code2 += e.Code[i].ToString() + ",";
                        }
                    }
                    if (code1 == code2)
                    {
                        SetPattern1 s1 = new SetPattern1();
                        s1.gmcode = code1;
                        this.Close();
                        s1.ShowDialog();
                        if (s1.ra > 0)
                        {
                            MessageBox.Show("Successfully Saved");
                            setpattern = 1;
                            return;
                        }
                    }
                    else
                    {
                        code1 = code2 = "";
                        attemptcount = 0;
                        MessageBox.Show("Pattern Not Match Try Again");
                        metroLabel1.Text = "Draw Your Pattern";
                        return;
                    }
                }
                attemptcount++;
            }
        }
    }
}
