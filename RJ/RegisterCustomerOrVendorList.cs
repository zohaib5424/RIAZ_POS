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
    public partial class RegisterCustomerOrVendorList : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public RegisterCustomerOrVendorList()
        {
            InitializeComponent();
        }

        int abc = -1;
        public RegisterCustomerOrVendorList(int a)
        {
            abc = a;
            InitializeComponent();
            if (a == 0)//register customer or vendor
            {

            }
            else if (a == 1)//register sub customer
            {
                metroLabel4.Text = "Register Sub Customer";
            }
        }

        AutoCompleteStringCollection Customers = new AutoCompleteStringCollection();
        public string customer_id = "";
        private void ClassType_Load(object sender, EventArgs e)
        {
            richTextBox2.ReadOnly = true;
            try
            {
                Customers = new AutoCompleteStringCollection();
                DataTable dt = new DataTable();
                if (abc == 1)//sub customer
                {
                    dt = gm.GetTable("select * from customer_or_vendor where status ='1' and customer_vendor_type='Customer' and parent='"+customer_id+"'");
                    foreach (DataRow d in dt.Rows)
                    {
                        Customers.Add(d["customer_or_vendor_name"].ToString());
                    }
                }
                else//main customer abc=""
                {
                    dt = gm.GetTable("select * from customer_or_vendor where status ='1' and customer_vendor_type='Customer' and parent=''");
                    foreach (DataRow d in dt.Rows)
                    {
                        Customers.Add(d["customer_or_vendor_name"].ToString());
                    }
                }

            }
            catch { }
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
            try
            {
                Customers = new AutoCompleteStringCollection();
                DataTable dt = new DataTable();
                if (abc == 1)//sub customer
                {
                    dt = gm.GetTable("select * from customer_or_vendor where status ='1' and customer_vendor_type='Customer' and parent='" + customer_id + "'");
                    foreach (DataRow d in dt.Rows)
                    {
                        Customers.Add(d["customer_or_vendor_name"].ToString());
                    }
                }
                else//main customer abc=""
                {
                    dt = gm.GetTable("select * from customer_or_vendor where status ='1' and customer_vendor_type='Customer' and parent=''");
                    foreach (DataRow d in dt.Rows)
                    {
                        Customers.Add(d["customer_or_vendor_name"].ToString());
                    }
                }
            }
            catch { }
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
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.S)
            {
                metroTile5.PerformClick();
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
                richTextBox2.Text = "";
                string query = "";
                int namescount = 0;
                try
                {
                    string[] a = richTextBox1.Text.Trim().Split('\n').Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    namescount = a.Length;
                }
                catch { }
                if (namescount > 0)
                {
                    try
                    {
                        int ok = 0;
                        string[] a = richTextBox1.Text.Trim().Split('\n').Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();
                        for (int i = 0; i < a.Length; i++)
                        {
                            try
                            {
                                query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + a[i].ToString().Trim() + "' and status!='-1' and parent=N'" + customer_id + "'";
                                DataTable dt = gm.GetTable(query);
                                if (dt.Rows.Count <= 0)
                                {
                                    query = "select max(cast(id as int)) from Customer_Or_Vendor";
                                    string id = gm.MaxId(query);
                                    query = "insert into Customer_Or_Vendor values(N'" + id + "','Customer',N'" + a[i].ToString().Trim() + "',N'" + "" + "',N'" + "" + "',N'" + RJ.Properties.Settings.Default.loginid + "','" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.ToShortTimeString() + "','1',N'" + customer_id + "','0')";
                                    gm.ExecuteNonQuery(query);
                                    ok = 1;
                                }
                                else
                                {
                                    richTextBox2.Text += a[i].ToString().Trim() + "\n";
                                }
                            }
                            catch { }
                        }
                        if (ok == 1)
                        {
                            MessageBox.Show("Customers list successfully Saved");
                        }
                        else
                        {
                            MessageBox.Show("Unable to Save list");
                        }
                    }
                    catch { }
                }
                else
                {
                    MessageBox.Show("No Name Found To Save.\nEnter Name To Save List Of Customers");
                }
            }
            catch { }
        }

        private void metroTextBoxPaidAmount_TextChanged(object sender, EventArgs e)
        {
            gm.AcceptDouble(sender, e);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int totalnames = 0;
                label4.Text = totalnames.ToString();
                try
                {
                    string[] a = richTextBox1.Text.Trim().Split('\n').Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    totalnames = a.Length;
                }
                catch { }
                label4.Text = totalnames.ToString();
            }
            catch { }
            //richTextBox1.Font = new Font("Jameel Noori Nastaleeq", 18F, GraphicsUnit.Pixel);
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int totalnames = 0;
                label5.Text = totalnames.ToString();
                try
                {
                    string[] a = richTextBox2.Text.Trim().Split('\n').Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    totalnames = a.Length;
                }
                catch { }
                label5.Text = totalnames.ToString();
            }
            catch { }
            //richTextBox2.Font = new Font("Jameel Noori Nastaleeq", 18F, GraphicsUnit.Pixel);
        }

        private void metroLabel18_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBox1.Text = "";
                richTextBox2.Text = "";
            }
            catch { }
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
