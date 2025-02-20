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

namespace SchoolMgmSys
{
    public partial class Classes_mgm : Form
    {
        
        public Classes_mgm()
        {
            InitializeComponent();
        }

        private void Classes_mgm_Load(object sender, EventArgs e)
        {
            groupBox1.Hide();
            try
            {
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }
            }
            catch (Exception ex)
            {

            }
        }
        SqlConnection con = new SqlConnection(SchoolMgmSys.Properties.Settings.Default.Connectionstring);

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBox1.Show();
            try
            {
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("select count(*)",con);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
