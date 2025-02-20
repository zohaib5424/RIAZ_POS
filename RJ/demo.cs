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
    public partial class demo : Form
    {
        public demo()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        private void button1_Click(object sender, EventArgs e)
        {
            if (con.State.ToString() == "Closed")
            {
                con.Open();
            }
            string query = @"insert into studentreg values('1','1','" + textBox1.Text + "','','" + textBox2.Text + "','','','','','','','','','','','','','','')";
            SqlCommand cmd = new SqlCommand(query, con);
            int r = cmd.ExecuteNonQuery();
            if (r > 0)
            {
                MessageBox.Show("Successfully Inserted");
            }
        }
    }
}
