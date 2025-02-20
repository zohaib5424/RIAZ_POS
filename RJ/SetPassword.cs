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
    public partial class SetPassword : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public SetPassword()
        {
            InitializeComponent();
        }

        private void ClassType_Load(object sender, EventArgs e)
        {

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
        public int setpassword = 0;
        private void metroTile5_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text.Trim() == "")
                {
                    MessageBox.Show("Enter Password");
                    return;
                }
                if (textBox3.Text.Trim() == "")
                {
                    MessageBox.Show("Re-Enter Password");
                    return;
                }
                if (textBox2.Text.Trim() != textBox3.Text.Trim() )
                {
                    MessageBox.Show("New Password and Confirm Password Not Match");
                    return;
                }
                {
                    string query = "update login set password='"+textBox2.Text.Trim()+"' where id = '"+RJ.Properties.Settings.Default.loginid+"'";
                    gm.ExecuteNonQuery(query);
                    MessageBox.Show("Password Successfully Set");
                    textBox2.Text = textBox3.Text = string.Empty;
                    setpassword = 1;
                    this.Dispose(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
