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
    public partial class ChangeLockSetting : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public ChangeLockSetting()
        {
            InitializeComponent();
        }


        int load = 0;
        private void ClassType_Load(object sender, EventArgs e)
        {
            try
            {
                string query = "select * from login where id='"+RJ.Properties.Settings.Default.loginid+"'";
                DataTable dt = gm.GetTable(query);
                foreach (DataRow d in dt.Rows)
                {
                    if (d["active_password_type"].ToString() == "Password")
                    {
                        switchButton1.Value = true;
                        switchButton2.Value = false;
                    }
                    if (d["active_password_type"].ToString() == "Pattern")
                    {
                        switchButton1.Value = false;
                        switchButton2.Value = true;
                    }
                }
                load = 1;
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
        private void metroTile5_Click(object sender, EventArgs e)
        {
            try
            {
                if (switchButton1.Value == true || switchButton2.Value == true)
                {
                    string active = "";
                    if(switchButton1.Value == true)
                        active = "Password";
                    if(switchButton2.Value == true)
                        active = "Pattern";
                    string query = "update login set active_password_type=N'" + active + "' where id = N'"+RJ.Properties.Settings.Default.loginid+"'";
                    int ra = gm.ExecuteNonQuery(query);
                    if(ra>0)
                        MessageBox.Show("Successfully Active");
                }
                else
                {
                    MessageBox.Show("First active any lock setting to save");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void switchButton1_ValueChanged(object sender, EventArgs e)
        {
            if (load == 0)
            {
                return;
            }
            try
            {
                if (switchButton1.Value == true)
                {
                    try
                    {
                        string query = "select * from login where id = N'" + RJ.Properties.Settings.Default.loginid + "'";
                        DataTable dt = gm.GetTable(query);
                        foreach (DataRow d in dt.Rows)
                        {
                            if (d["password"].ToString().Trim() == "")
                            {
                                SetPassword sp = new SetPassword();
                                sp.ShowDialog();
                                //call setpassword form
                                if (sp.setpassword == 0)
                                {
                                    switchButton1.Value = false;
                                    return;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    switchButton2.Value = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void switchButton2_ValueChanged(object sender, EventArgs e)
        {
            if (load == 0)
            {
                return;
            }
            try
            {
                if (switchButton2.Value == true)//pattern
                {
                    try
                    {
                        string query = "select * from login where id = '"+RJ.Properties.Settings.Default.loginid+"'";
                        DataTable dt = gm.GetTable(query);
                        foreach (DataRow d in dt.Rows)
                        {
                            //if (d["pattern"].ToString().Trim() == "" && d["active_password_type"].ToString() != "Pattern")
                            {
                                SetPattern sp = new SetPattern();
                                sp.ShowDialog();
                                if (sp.setpattern == 0)
                                {
                                    switchButton2.Value = false;
                                    return;
                                }
                                else
                                {
                                    switchButton1.Value = false;
                                    switchButton2.Value = true;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    switchButton1.Value = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
