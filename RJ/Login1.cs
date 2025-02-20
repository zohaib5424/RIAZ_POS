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
    public partial class Login1 : Form
    {
        public Login1()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        private void Login_Load(object sender, EventArgs e)
        {
            if (con.State.ToString() == "Closed")
            {
                con.Open();
            }
            login = new AutoCompleteStringCollection();
            RJ.Properties.Settings.Default.loginid = "";
            RJ.Properties.Settings.Default.loginuser = "";
            RJ.Properties.Settings.Default.logintype = "";
            RJ.Properties.Settings.Default.loginuserid = "";
            SqlCommand cmd = new SqlCommand("Select * from login where status='1'", con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow d in dt.Rows)
                login.Add(d["user_id"].ToString());
            textBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox1.AutoCompleteCustomSource = login;
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        AutoCompleteStringCollection login;
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text.Trim().Contains(" ") || textBox2.Text.Trim().Contains(" "))
                {
                    MessageBox.Show("Don't use space");
                    return;
                }
                else
                {
                    if (con.State.ToString() == "Closed")
                    {
                        con.Open();
                    }
                    string query = "select * from login where user_id='" + textBox1.Text.Trim() + "' and password='" + textBox2.Text.Trim() + "' and status='1'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    foreach (DataRow d in dt.Rows)
                    {
                        RJ.Properties.Settings.Default.loginid = d["id"].ToString();
                        RJ.Properties.Settings.Default.loginuserid = d["user_id"].ToString();
                        RJ.Properties.Settings.Default.loginuser = d["username"].ToString();
                        RJ.Properties.Settings.Default.logintype = d["usertype"].ToString();
                    }
                    if (dt.Rows.Count > 0)
                    {
                        this.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("Invalid User");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2.PerformClick();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2.PerformClick();
            }
        }
    }
}
