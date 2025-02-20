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
    public partial class RegisterUser : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public RegisterUser()
        {
            InitializeComponent();
        }

        private void ClassType_Load(object sender, EventArgs e)
        {
            metroComboBoxUserType.Items.Add("User");
            metroComboBoxUserType.Items.Add("Admin");
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

        public void reset()
        {
            textBox1.Text = "";
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
        private void metroTile5_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox4.Text.Trim() == "")
                {
                    MessageBox.Show("Enter User Name");
                    return;
                }
                if (textBox1.Text.Trim() == "")
                {
                    MessageBox.Show("Enter User_Id");
                    return;
                }
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
                if (metroComboBoxUserType.SelectedIndex < 0)
                {
                    MessageBox.Show("Select User Type");
                    return;
                }
                if (textBox2.Text.Trim() != textBox3.Text.Trim() )
                {
                    MessageBox.Show("Password and Confirm Password Not Match");
                    return;
                }
                string query = @"select * from login where user_id=N'"+textBox1.Text.Trim().ToString()+"'";
                DataTable dt = gm.GetTable(query);
                if (dt.Rows.Count <= 0)
                {
                    query = "select max(cast(id as int)) from login";
                    string id = gm.MaxId(query);
                    query = "insert into login values('"+id+"',N'"+textBox4.Text.Trim()+"',N'" + textBox1.Text.Trim() + "',N'"+textBox2.Text.Trim()+"','',N'"+metroComboBoxUserType.SelectedItem.ToString()+"',N'" + RJ.Properties.Settings.Default.loginid + "','"+DateTime.Now.ToShortDateString()+"','"+DateTime.Now.ToShortTimeString()+"','1',N'"+"Password"+"')";
                    gm.ExecuteNonQuery(query);
                    MessageBox.Show("User Registered Successfully");
                    textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = string.Empty;
                    metroComboBoxUserType.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("User_Id Already Exist");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
