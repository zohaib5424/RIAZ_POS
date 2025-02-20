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
    public partial class SetPattern1 : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public SetPattern1()
        {
            InitializeComponent();
        }

        public string gmcode = "";
        public int ra = -1;
        private void ClassType_Load(object sender, EventArgs e)
        {
            //string[] gmcodesplit = gmcode.Split(' ');
            //List<int> gmcode2 = new List<int>();
            //foreach (string s in gmcodesplit)
            //{
            //    try
            //    {
            //        gmcode2.Add(int.Parse(s.Trim()));
            //    }catch{}
            //}
            //int[] gmcode1 = new int[gmcode2.Count];
            //int index = 0 ;
            //foreach (int i in gmcode2)
            //{
            //    gmcode1[index] = i;
            //    index++;
            //}

            try
            {
                string query = "update login set pattern='" + gmcode + "' where id='" + RJ.Properties.Settings.Default.loginid + "'";
                ra = gm.ExecuteNonQuery(query);
                this.Dispose(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

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

        private void lockScreenControl1_PassCodeSubmitted(object sender, GestureLockApp.GestureLockControl.PassCodeSubmittedEventArgs e)
        {
            if (e.Valid)
            {
                string code = "";
                foreach (int i in e.Code)
                {
                    code += i.ToString();
                }
            }
            else
                MessageBox.Show("error");
        }
    }
}
