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
    public partial class RegisterCustomerOrVendor : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public RegisterCustomerOrVendor()
        {
            InitializeComponent();
        }

        int abc = -1;
        public RegisterCustomerOrVendor(int a)
        {
            abc = a;
            InitializeComponent();
            if (a == 0)//register customer or vendor
            {

            }
            else if (a == 1)//register sub customer
            {
                metroLabel4.Text = "Register Sub Customer";
                metroLabel6.Visible = false;
                metroComboBoxUserType.Visible = false;
                metroLabel1.Visible = false;
                metroTextBoxPercentage.Visible = false;
                metroTextBoxContactNumber.Visible = false;
                metroTextBoxAddress.Visible = false;
                metroLabel16.Visible = false;
                metroLabel17.Visible = false;
            }
        }

        AutoCompleteStringCollection Customers = new AutoCompleteStringCollection();
        public string customer_id = "";
        private void ClassType_Load(object sender, EventArgs e)
        {
            metroLabel6.Visible = false;
            metroComboBoxUserType.Visible = false;

            metroTextBoxName.Focus();
            metroComboBoxUserType.Items.Add("Customer");
            metroComboBoxUserType.Items.Add("Vendor");
            metroComboBoxUserType.SelectedIndex = 0;
            metroTextBoxPercentage.Text = "0";
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

                metroTextBoxName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                metroTextBoxName.AutoCompleteCustomSource = Customers;
                metroTextBoxName.AutoCompleteSource = AutoCompleteSource.CustomSource;
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
            metroTile5.Text = "SAVE";
            metroComboBoxUserType.SelectedIndex = -1;
            metroTextBoxName.Text = "";
            metroTextBoxContactNumber.Text = "";
            metroTextBoxAddress.Text = "";
            metroTextBoxPercentage.Text = "0";
            metroComboBoxUserType.SelectedIndex = 0;
            metroTextBoxName.Focus();
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

                metroTextBoxName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                metroTextBoxName.AutoCompleteCustomSource = Customers;
                metroTextBoxName.AutoCompleteSource = AutoCompleteSource.CustomSource;
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
        string cid = "";
        private void metroTile5_Click(object sender, EventArgs e)
        {
            try
            {
                if (metroTextBoxName.Text.Trim() == "")
                {
                    MessageBox.Show("Enter Name");
                    return;
                }
                //if (metroTextBoxContactNumber.Text.Trim() == "")
                //{
                //    MessageBox.Show("Enter Contact Number");
                //    return;
                //}
                //if (metroTextBoxAddress.Text.Trim() == "")
                //{
                //    MessageBox.Show("Enter Address");
                //    return;
                //}
                if (metroComboBoxUserType.SelectedIndex<0 && abc == 0)
                {
                    MessageBox.Show("Select Customer Or Vendor");
                    return;
                }
                string query = "";
                DataTable dt = new DataTable();
                if (metroTile5.Text == "UPDATE")
                {
                    if (abc == 1)//sub customer
                    {
                        query = @"select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + metroTextBoxName.Text.Trim().ToString() + "' and parent=N'" + customer_id + "' and status!='-1' and id!='"+cid+"'";
                    }
                    else//main customer
                    {
                        query = @"select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + metroTextBoxName.Text.Trim().ToString() + "' and parent='' and status!='-1' and id!='" + cid + "'";
                    }
                    dt = gm.GetTable(query);
                    if (dt.Rows.Count <= 0)
                    {
                        if (abc == 1)//sub customer
                        {
                            query = "update Customer_Or_Vendor set customer_Or_Vendor_Name=N'" + metroTextBoxName.Text.Trim() + "',Contact_Number=N'" + metroTextBoxContactNumber.Text.Trim() + "',_Address=N'" + metroTextBoxAddress.Text.ToString().Trim() + "',AddedBy_UserId='" + RJ.Properties.Settings.Default.loginid + "',_Date='" + DateTime.Now.ToShortDateString() + "',_Time='" + DateTime.Now.ToShortTimeString() + "' where parent='" + customer_id + "' and id='" + cid + "'";
                        }
                        else//main customer
                        {
                            query = "update Customer_Or_Vendor set customer_Or_Vendor_Name=N'" + metroTextBoxName.Text.Trim() + "',Contact_Number=N'" + metroTextBoxContactNumber.Text.Trim() + "',_Address=N'" + metroTextBoxAddress.Text.ToString().Trim() + "',AddedBy_UserId='" + RJ.Properties.Settings.Default.loginid + "',_Date='" + DateTime.Now.ToShortDateString() + "',_Time='" + DateTime.Now.ToShortTimeString() + "',Percentage='" + (metroTextBoxPercentage.Text.Trim().ToString() == "" ? "0" : metroTextBoxPercentage.Text.Trim().ToString()) + "' where id='" + cid + "'";
                        }
                        gm.ExecuteNonQuery(query);
                        MessageBox.Show("Updated Successfully");
                        reset();
                    }
                    else
                    {
                        MessageBox.Show(metroComboBoxUserType.SelectedItem.ToString() + " Already Exist");
                        return;
                    }
                }
                else if (metroTile5.Text == "SAVE")
                {
                    if (abc == 1)//sub customer
                    {
                        query = @"select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + metroTextBoxName.Text.Trim().ToString() + "' and parent=N'" + customer_id + "' and status!='-1'";
                    }
                    else//main customer
                    {
                        query = @"select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + metroTextBoxName.Text.Trim().ToString() + "' and status!='-1'";
                    }
                    dt = gm.GetTable(query);
                    if (dt.Rows.Count <= 0)
                    {
                        query = "select max(cast(id as int)) from Customer_Or_Vendor";
                        string id = gm.MaxId(query);
                        if (abc == 1)//sub customer
                        {
                            query = "insert into Customer_Or_Vendor values('" + id + "','Customer',N'" + metroTextBoxName.Text.Trim() + "',N'" + metroTextBoxContactNumber.Text.Trim() + "',N'" + metroTextBoxAddress.Text.ToString().Trim() + "','" + RJ.Properties.Settings.Default.loginid + "','" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.ToShortTimeString() + "','1','" + customer_id + "','" + (metroTextBoxPercentage.Text.Trim().ToString() == "" ? "0" : metroTextBoxPercentage.Text.Trim().ToString()) + "')";
                        }
                        else//main customer
                        {
                            query = "insert into Customer_Or_Vendor values('" + id + "','" + metroComboBoxUserType.SelectedItem.ToString().Trim() + "',N'" + metroTextBoxName.Text.Trim() + "',N'" + metroTextBoxContactNumber.Text.Trim() + "',N'" + metroTextBoxAddress.Text.ToString().Trim() + "','" + RJ.Properties.Settings.Default.loginid + "','" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.ToShortTimeString() + "','1','','" + (metroTextBoxPercentage.Text.Trim().ToString() == "" ? "0" : metroTextBoxPercentage.Text.Trim().ToString()) + "')";
                        }
                        gm.ExecuteNonQuery(query);
                        MessageBox.Show("Registered Successfully");
                        reset();
                    }
                    else
                    {
                        MessageBox.Show(metroComboBoxUserType.SelectedItem.ToString() + " Already Exist");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void metroTextBoxPaidAmount_TextChanged(object sender, EventArgs e)
        {
            gm.AcceptDouble(sender, e);
        }

        private void metroTextBoxName_KeyDown(object sender, KeyEventArgs e)
        {
            //try
            //{
            //    cid = "";
            //    metroTile5.Text = "SAVE";
            //    if (e.KeyCode == Keys.Enter)
            //    {


            //        string query = "";
            //        DataTable dt = new DataTable();
            //        if (abc == 1)//sub customer
            //        {

            //            query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + metroTextBoxName.Text.Trim() + "' and parent='"+customer_id+"' and status!='-1'";
            //            dt = gm.GetTable(query);
            //            if (dt.Rows.Count > 0)
            //            {
            //                cid = dt.Rows[0][0].ToString();
            //                metroTextBoxAddress.Text = dt.Rows[0]["_Address"].ToString();
            //                metroTextBoxContactNumber.Text = dt.Rows[0]["Contact_Number"].ToString();
            //                metroTextBoxPercentage.Text = dt.Rows[0]["percentage"].ToString();
            //                metroTile5.Text = "UPDATE";
            //            }

            //        }
            //        else
            //        {
            //            query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + metroTextBoxName.Text.Trim() + "' and parent='' and status!='-1'";
            //            dt = gm.GetTable(query);
            //            if (dt.Rows.Count > 0)
            //            {
            //                cid = dt.Rows[0][0].ToString();
            //                metroTextBoxAddress.Text = dt.Rows[0]["_Address"].ToString();
            //                metroTextBoxContactNumber.Text = dt.Rows[0]["Contact_Number"].ToString();
            //                metroTextBoxPercentage.Text = dt.Rows[0]["percentage"].ToString();
            //                metroTile5.Text = "UPDATE";
            //            }
            //        }



            //    }
            //}
            //catch { }
        }

        private void metroTextBoxName_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBoxName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                cid = "";
                metroTile5.Text = "SAVE";
                //if (e.KeyCode == Keys.Enter)
                {
                    metroTextBoxAddress.Text = "";
                    metroTextBoxContactNumber.Text = "";
                    metroTextBoxPercentage.Text = "";
                    
                    string query = "";
                    DataTable dt = new DataTable();
                    if (abc == 1)//sub customer
                    {

                        //query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + metroTextBoxName.Text.Trim() + "' and parent='" + customer_id + "' and status!='-1'";
                        //dt = gm.GetTable(query);
                        //if (dt.Rows.Count > 0)
                        //{
                        //    cid = dt.Rows[0][0].ToString();
                        //    metroTextBoxAddress.Text = dt.Rows[0]["_Address"].ToString();
                        //    metroTextBoxContactNumber.Text = dt.Rows[0]["Contact_Number"].ToString();
                        //    metroTextBoxPercentage.Text = dt.Rows[0]["percentage"].ToString();
                        //    metroTile5.Text = "UPDATE";
                        //}

                    }
                    else
                    {
                        query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + metroTextBoxName.Text.Trim() + "' and parent='' and status!='-1'";
                        dt = gm.GetTable(query);
                        if (dt.Rows.Count > 0)
                        {
                            cid = dt.Rows[0][0].ToString();
                            metroTextBoxAddress.Text = dt.Rows[0]["_Address"].ToString();
                            metroTextBoxContactNumber.Text = dt.Rows[0]["Contact_Number"].ToString();
                            metroTextBoxPercentage.Text = dt.Rows[0]["percentage"].ToString();
                            metroTile5.Text = "UPDATE";
                        }
                    }



                }
            }
            catch { }
        }
    }
}
