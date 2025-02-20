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
    public partial class CustomersOrVendorsList : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public CustomersOrVendorsList()
        {
            InitializeComponent();
        }

        int abc = -1;
        public CustomersOrVendorsList(int a)
        {
            abc = a;
            InitializeComponent();
            if (a == 0)//customer or vendor list
            {
                label1.Visible = false;
                metroTile2.Visible = false;
            }
            else if (a == 1)//register sub customer
            {
                metroRadioButton2.Visible = false;
                metroLabel1.Text = "Customers List";
            }
        }

        AutoCompleteStringCollection source = new AutoCompleteStringCollection();
        AutoCompleteStringCollection sourceusername = new AutoCompleteStringCollection();
        AutoCompleteStringCollection sourcesubcustomername = new AutoCompleteStringCollection();
        AutoCompleteStringCollection sourceuserid = new AutoCompleteStringCollection();
        private void ItemsList_Load(object sender, EventArgs e)
        {
            try
            {
                groupBox3.Hide();
                metroRadioButton2.Visible = false;
                metroComboBoxUserType.Visible = false;
                
                metroComboBoxUserType.Items.Add("Customer");
                metroComboBoxUserType.Items.Add("Vendor");
                source = new AutoCompleteStringCollection();
                sourceusername = new AutoCompleteStringCollection();
                sourceuserid = new AutoCompleteStringCollection();
                textBox1.Hide();
                textBox2.Hide();

                this.AutoScroll = true;
                metroRadioButton3.Checked = true;

                metroComboBoxUserType.Hide();


                DataTable dt = new DataTable();
                if (abc == 1)
                {
                    dt = gm.GetTable("select * from Customer_Or_Vendor where status !='-1' and Customer_Vendor_Type='" + "Customer" + "' and (Parent='Null' or Parent='') and Customer_Vendor_Type='Customer'");
                }
                else
                {
                    dt = gm.GetTable("select * from Customer_Or_Vendor where status !='-1' and (Parent='Null' or Parent='') and Customer_Vendor_Type='Customer'");
                    //dt = gm.GetTable("select * from Customer_Or_Vendor where status !='-1'");
                }
                foreach (DataRow d in dt.Rows)
                {
                    sourceusername.Add(d["customer_Or_Vendor_Name"].ToString() + " (" + d["id"].ToString() + ")");
                }

                textBox1.DisplayMember = "customer_Or_Vendor_Name";
                textBox1.ValueMember = "id";
                textBox1.DataSource = dt;
                textBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
                textBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
                textBox1.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        GMDB gm = new GMDB();
        public void getAllRecord()
        {
            try
            {
                string query = "select * from Customer_Or_Vendor where status !='-1' and (Parent='Null' or Parent='') and Customer_Vendor_Type='Customer' order by customer_Or_Vendor_Name ASC";
                //string query = "Select * from Customer_Or_Vendor where status != '-1'";
                if (abc == 1)
                {
                    query = "Select * from Customer_Or_Vendor where status != '-1' and Customer_Vendor_Type='" + "Customer" + "' and (Parent='NULL' or Parent='" + DBNull.Value + "' or Parent='') order by customer_Or_Vendor_Name ASC";
                }
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                DataTable dt = gm.GetTable(query);
                foreach (DataRow d in dt.Rows)
                {
                    string addedby = d["AddedBy_UserId"].ToString();
                    if (addedby.Trim() != "")
                    {
                        query = "select * from login where id='" + d["AddedBy_UserId"].ToString() + "'";
                        DataTable dtlogins = gm.GetTable(query);
                        addedby = dtlogins.Rows[0][1].ToString();
                    }
                    if (d["status"].ToString() == "1")
                        dataGridView1.Rows.Add(d["id"].ToString(), d["Customer_Vendor_Type"].ToString(), d["customer_Or_Vendor_Name"].ToString(), d["Contact_Number"].ToString(), d["_Address"].ToString(), d["Percentage"].ToString(), d["_date"].ToString(), d["_time"].ToString(), addedby, "Deactive", "Delete");
                    else
                        dataGridView1.Rows.Add(d["id"].ToString(), d["Customer_Vendor_Type"].ToString(), d["customer_Or_Vendor_Name"].ToString(), d["Contact_Number"].ToString(), d["_Address"].ToString(), d["Percentage"].ToString(), d["_date"].ToString(), d["_time"].ToString(), addedby, "Active", "Delete");
                    if (d["status"].ToString() == "0")
                    {
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            try
            {
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    c.DefaultCellStyle.Font = new Font("Jameel Noori Nastaleeq", 19.5F, GraphicsUnit.Pixel);
                }
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Height = 32;
                }
            }
            catch { }
        }

        public void getCategoryRecord(string category_id)
        {
            try
            {
                string query = "Select * from items where category_id='"+category_id+"' and status != '-1'";
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                DataTable dt = gm.GetTable(query);
                foreach (DataRow d in dt.Rows)
                {
                    query = "Select * from categories where id='" + d["category_id"].ToString() + "'";
                    DataTable dtcategory = gm.GetTable(query);
                    string category = dtcategory.Rows[0][1].ToString();
                    string brand = d["Brand_Id"].ToString();
                    if (d["brand_id"].ToString() != "Other")
                    {
                        query = "Select * from brands where id='" + d["brand_id"].ToString() + "'";
                        DataTable dtbrand = gm.GetTable(query);
                        brand = dtbrand.Rows[0][1].ToString();
                    }
                    string unit = d["Unit_Id"].ToString();
                    if (d["unit_id"].ToString() != "Other")
                    {
                        query = "Select * from units where id='" + d["unit_id"].ToString() + "'";
                        DataTable dtunit = gm.GetTable(query);
                        unit = dtunit.Rows[0][1].ToString();
                    }
                    if (d["status"].ToString() == "1")
                        dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), brand, unit, d["Retail_Price"].ToString(), d["Purchase_Price"].ToString(), d["Qty"].ToString(), d["Barcode"].ToString(), "Deactive");
                    else
                        dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), brand, unit, d["Retail_Price"].ToString(), d["Purchase_Price"].ToString(), d["Qty"].ToString(), d["Barcode"].ToString(), "Active");
                    if (d["status"].ToString() == "0")
                    {
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            try
            {
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    c.DefaultCellStyle.Font = new Font("Jameel Noori Nastaleeq", 19.5F, GraphicsUnit.Pixel);
                }
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Height = 32;
                }
            }
            catch { }
        }

        public void getUserNameRecord(string id)
        {
            try
            {
                string query = "Select * from Customer_Or_Vendor where id='" + id + "' and status != '-1' order by customer_Or_Vendor_Name ASC";
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                DataTable dt = gm.GetTable(query);
                foreach (DataRow d in dt.Rows)
                {
                    string addedby = d["AddedBy_UserId"].ToString();
                    if (addedby.Trim() != "")
                    {
                        query = "select * from login where id='" + d["AddedBy_UserId"].ToString() + "'";
                        DataTable dtlogins = gm.GetTable(query);
                        addedby = dtlogins.Rows[0][1].ToString();
                    }
                    if (d["status"].ToString() == "1")
                        dataGridView1.Rows.Add(d["id"].ToString(), d["Customer_Vendor_Type"].ToString(), d["customer_Or_Vendor_Name"].ToString(), d["Contact_Number"].ToString(), d["_Address"].ToString(), d["Percentage"].ToString(), d["_date"].ToString(), d["_time"].ToString(), addedby, "Deactive", "Delete");
                    else
                        dataGridView1.Rows.Add(d["id"].ToString(), d["Customer_Vendor_Type"].ToString(), d["customer_Or_Vendor_Name"].ToString(), d["Contact_Number"].ToString(), d["_Address"].ToString(), d["Percentage"].ToString(), d["_date"].ToString(), d["_time"].ToString(), addedby, "Active", "Delete");
                    if (d["status"].ToString() == "0")
                    {
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows[0].Selected = true;
                    getSelectRowRecord();
                }
                //dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            try
            {
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    c.DefaultCellStyle.Font = new Font("Jameel Noori Nastaleeq", 19.5F, GraphicsUnit.Pixel);
                }
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Height = 32;
                }
            }
            catch { }
        }

        public void getSubUserNameRecord(string id)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count <= 0)
                {
                    return;
                }
                string query = "Select * from Customer_Or_Vendor where status != '-1' and id='" + id + "'";
                if (dataGridView2.Rows.Count > 0)
                {
                    dataGridView2.Rows.Clear();
                }
                DataTable dt = gm.GetTable(query);
                foreach (DataRow d in dt.Rows)
                {
                    string addedby = d["AddedBy_UserId"].ToString();
                    if (addedby.Trim() != "")
                    {
                        query = "select * from login where id='" + d["AddedBy_UserId"].ToString() + "'";
                        DataTable dtlogins = gm.GetTable(query);
                        addedby = dtlogins.Rows[0][1].ToString();
                    }
                    if (d["status"].ToString() == "1")
                        dataGridView2.Rows.Add(d["id"].ToString(), d["Customer_Vendor_Type"].ToString(), d["customer_Or_Vendor_Name"].ToString(), d["Contact_Number"].ToString(), d["_Address"].ToString(), d["_date"].ToString(), d["_time"].ToString(), addedby, "Deactive", "Delete");
                    else
                        dataGridView2.Rows.Add(d["id"].ToString(), d["Customer_Vendor_Type"].ToString(), d["customer_Or_Vendor_Name"].ToString(), d["Contact_Number"].ToString(), d["_Address"].ToString(), d["_date"].ToString(), d["_time"].ToString(), addedby, "Active", "Delete");
                    if (d["status"].ToString() == "0")
                    {
                        dataGridView2.Rows[dataGridView2.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
                dataGridView2.ClearSelection();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            try
            {
                foreach (DataGridViewColumn c in dataGridView2.Columns)
                {
                    c.DefaultCellStyle.Font = new Font("Jameel Noori Nastaleeq", 19.5F, GraphicsUnit.Pixel);
                }
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    row.Height = 32;
                }
            }
            catch { }
        }

        public void getSelectRowRecord()
        {
            try
            {
                try
                {
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        sourcesubcustomername = new AutoCompleteStringCollection();
                        DataTable dt = new DataTable();
                        dt = gm.GetTable("select * from Customer_Or_Vendor where status !='-1' and Customer_Vendor_Type='" + "Customer" + "' and Parent='" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "'");
                        foreach (DataRow d in dt.Rows)
                        {
                            sourcesubcustomername.Add(d["customer_Or_Vendor_Name"].ToString() + " (" + d["id"].ToString() + ")");
                        }

                        textBox2.DisplayMember = "customer_Or_Vendor_Name";
                        textBox2.ValueMember = "id";
                        textBox2.DataSource = dt;
                        textBox2.AutoCompleteMode = AutoCompleteMode.Suggest;
                        textBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
                        textBox2.SelectedIndex = -1;

                        string query = "Select * from Customer_Or_Vendor where status != '-1' and parent='" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "'";
                        if (dataGridView2.Rows.Count > 0)
                        {
                            dataGridView2.Rows.Clear();
                        }
                        dt = gm.GetTable(query);
                        foreach (DataRow d in dt.Rows)
                        {
                            string addedby = d["AddedBy_UserId"].ToString();
                            if (addedby.Trim() != "")
                            {
                                query = "select * from login where id='" + d["AddedBy_UserId"].ToString() + "'";
                                DataTable dtlogins = gm.GetTable(query);
                                addedby = dtlogins.Rows[0][1].ToString();
                            }
                            if (d["status"].ToString() == "1")
                                dataGridView2.Rows.Add(d["id"].ToString(), d["Customer_Vendor_Type"].ToString(), d["customer_Or_Vendor_Name"].ToString(), d["Contact_Number"].ToString(), d["_Address"].ToString(), d["_date"].ToString(), d["_time"].ToString(), addedby, "Deactive", "Delete");
                            else
                                dataGridView2.Rows.Add(d["id"].ToString(), d["Customer_Vendor_Type"].ToString(), d["customer_Or_Vendor_Name"].ToString(), d["Contact_Number"].ToString(), d["_Address"].ToString(), d["_date"].ToString(), d["_time"].ToString(), addedby, "Active", "Delete");
                            if (d["status"].ToString() == "0")
                            {
                                dataGridView2.Rows[dataGridView2.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                            }
                        }
                        dataGridView2.ClearSelection();
                        metroRadioButton5.Checked = true;
                        try
                        {
                            if (dataGridView2.Rows.Count > 0)
                            {
                                groupBox3.Show();
                            }
                            else
                            {
                                groupBox3.Hide();
                            }
                        }
                        catch { }
                    }
                }
                catch (Exception ex1) {
                //    MessageBox.Show(ex1.Message); 
                }
                //dataGridView1.SelectedRows[0].Selected = true;
            }
            catch (Exception ex) { 
                //MessageBox.Show(ex.Message); 
            }
            try
            {
                foreach (DataGridViewColumn c in dataGridView2.Columns)
                {
                    c.DefaultCellStyle.Font = new Font("Jameel Noori Nastaleeq", 19.5F, GraphicsUnit.Pixel);
                }
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    row.Height = 32;
                }
            }
            catch { }
        }

        public void getUserIdRecord(string id)
        {
            try
            {
                string query = "Select * from Login where id='" + id + "' and status != '-1'";
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                DataTable dt = gm.GetTable(query);
                foreach (DataRow d in dt.Rows)
                {
                    string addedby = d["AddedBy_UserId"].ToString();
                    if (addedby.Trim() != "")
                    {
                        query = "select * from login where id='" + d["AddedBy_UserId"].ToString() + "'";
                        DataTable dtlogins = gm.GetTable(query);
                        addedby = dtlogins.Rows[0][1].ToString();
                    }
                    if (d["status"].ToString() == "1")
                        dataGridView1.Rows.Add(d["id"].ToString(), d["username"].ToString(), d["User_Id"].ToString(), d["_date"].ToString(), d["_time"].ToString(), addedby, "Deactive", "Delete");
                    else
                        dataGridView1.Rows.Add(d["id"].ToString(), d["username"].ToString(), d["User_Id"].ToString(), d["_date"].ToString(), d["_time"].ToString(), addedby, "Active", "Delete");
                    if (d["status"].ToString() == "0")
                    {
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            try
            {
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    c.DefaultCellStyle.Font = new Font("Jameel Noori Nastaleeq", 19.5F, GraphicsUnit.Pixel);
                }
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Height = 32;
                }
            }
            catch { }
        }

        public void getUserTypeRecord(string type)
        {
            try
            {
                string query = "Select * from Customer_Or_Vendor where Customer_Vendor_Type='" + type + "' and status != '-1'";
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                DataTable dt = gm.GetTable(query);
                foreach (DataRow d in dt.Rows)
                {
                    string addedby = d["AddedBy_UserId"].ToString();
                    if (addedby.Trim() != "")
                    {
                        query = "select * from login where id='" + d["AddedBy_UserId"].ToString() + "'";
                        DataTable dtlogins = gm.GetTable(query);
                        addedby = dtlogins.Rows[0][1].ToString();
                    }
                    if (d["status"].ToString() == "1")
                        dataGridView1.Rows.Add(d["id"].ToString(), d["Customer_Vendor_Type"].ToString(), d["customer_Or_Vendor_Name"].ToString(), d["Contact_Number"].ToString(), d["_Address"].ToString(), d["Percentage"].ToString(), d["_date"].ToString(), d["_time"].ToString(), addedby, "Deactive", "Delete");
                    else
                        dataGridView1.Rows.Add(d["id"].ToString(), d["Customer_Vendor_Type"].ToString(), d["customer_Or_Vendor_Name"].ToString(), d["Contact_Number"].ToString(), d["_Address"].ToString(), d["Percentage"].ToString(), d["_date"].ToString(), d["_time"].ToString(), addedby, "Active", "Delete");
                    if (d["status"].ToString() == "0")
                    {
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            try
            {
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    c.DefaultCellStyle.Font = new Font("Jameel Noori Nastaleeq", 19.5F, GraphicsUnit.Pixel);
                }
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Height = 32;
                }
            }
            catch { }
        }

        private void metroRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (metroRadioButton1.Checked == true)
            {
                metroComboBoxUserType.Hide();
                textBox1.Hide();
                getAllRecord();
                if (dataGridView2.Rows.Count > 0)
                {
                    dataGridView2.Rows.Clear();
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                dataGridView2.Rows.Clear();
                metroRadioButton5.Checked = false;
                metroRadioButton4.Checked = false;
                try
                {
                    metroTextBoxName.Text = "";
                    metroTextBoxName.Tag = -1;
                    metroTextBoxContactNumber.Text = "";
                    metroTextBoxAddress.Text = "";
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                    metroTextBoxName.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                    metroTextBoxName.Tag = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                    metroTextBoxContactNumber.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                    metroTextBoxAddress.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                    if (e.ColumnIndex == 9)
                    {
                        if (dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString() == "Active")
                        {
                            if (con.State.ToString() == "Closed")
                            {
                                con.Open();
                            }
                            string query = @"update Customer_Or_Vendor set status='1' where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Successfully Activated");
                            if (metroRadioButton1.Checked == true)
                                getAllRecord();
                            else if (metroRadioButton2.Checked == true)
                                getUserTypeRecord(metroComboBoxUserType.SelectedItem.ToString());
                            else if (metroRadioButton3.Checked == true)
                            {
                                if (dataGridView1.Rows.Count > 0)
                                {
                                    dataGridView1.Rows.Clear();
                                }
                                try
                                {
                                    string id = "";
                                    try//only word if main customer or sub sub customer name is unique
                                    {
                                        query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + textBox1.Text.Trim() + "' and status!='-1'";
                                        id = gm.GetTable(query).Rows[0][0].ToString();
                                    }
                                    catch { }
                                    if (id == "")
                                    {
                                        return;
                                    }
                                    if (id.Trim() != "")
                                    {
                                        getUserNameRecord(id);
                                    }
                                    //if (metroTextBox1.Text.Trim() == "")
                                    //    getAllRecord();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                        }
                        else
                        {
                            //if (dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() == Pos_Sabzi_Mandi.Properties.Settings.Default.loginid)
                            //{
                            //    MessageBox.Show("You Can't Deactive yourself");
                            //    return;
                            //}
                            if (con.State.ToString() == "Closed")
                            {
                                con.Open();
                            }
                            string query = @"update Customer_Or_Vendor set status='0' where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Successfully Deactivated");
                            if (metroRadioButton1.Checked == true)
                            {
                                metroRadioButton1.Checked = false;
                                metroRadioButton1.PerformClick();
                                //getAllRecord();
                            }
                            else if (metroRadioButton2.Checked == true)
                                getUserTypeRecord(metroComboBoxUserType.SelectedItem.ToString());
                            else if (metroRadioButton3.Checked == true)
                            {
                                try
                                {
                                    string id = "";
                                    try//only word if main customer or sub sub customer name is unique
                                    {
                                        query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + textBox1.Text.Trim() + "' and status!='-1'";
                                        id = gm.GetTable(query).Rows[0][0].ToString();
                                    }
                                    catch { }
                                    if (id == "")
                                    {
                                        return;
                                    }
                                    if (id.Trim() != "")
                                    {
                                        getUserNameRecord(id);
                                    }
                                    //if (metroTextBox1.Text.Trim() == "")
                                    //    getAllRecord();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                        }
                        dataGridView1.ClearSelection();
                    }
                    if (e.ColumnIndex == 10)
                    {
                        //if (dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() == Pos_Sabzi_Mandi.Properties.Settings.Default.loginid)
                        //{
                        //    MessageBox.Show("You Can't Delete yourself");
                        //    return;
                        //}
                        DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this user?(yes/no)", "Confirmation", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.No)
                        {
                            return;
                        }
                        string query = @"update Customer_Or_Vendor set status='-1' where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
                        gm.ExecuteNonQuery(query);
                        MessageBox.Show("Delete Successfully");
                        try
                        {
                            sourceusername = new AutoCompleteStringCollection();
                            DataTable dt = new DataTable();
                            if (abc == 1)
                            {
                                dt = gm.GetTable("select * from Customer_Or_Vendor where status !='-1' and Customer_Vendor_Type='" + "Customer" + "' and (Parent='Null' or Parent='') and Customer_Vendor_Type='Customer'");
                            }
                            else
                            {
                                dt = gm.GetTable("select * from Customer_Or_Vendor where status !='-1' and (Parent='Null' or Parent='') and Customer_Vendor_Type='Customer'");
                                //dt = gm.GetTable("select * from Customer_Or_Vendor where status !='-1'");
                            }
                            foreach (DataRow d in dt.Rows)
                            {
                                sourceusername.Add(d["customer_Or_Vendor_Name"].ToString() + " (" + d["id"].ToString() + ")");
                            }

                            textBox1.DisplayMember = "customer_Or_Vendor_Name";
                            textBox1.ValueMember = "id";
                            textBox1.DataSource = dt;
                            textBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
                            textBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
                            textBox1.SelectedIndex = -1;
                        }
                        catch { }
                        if (metroRadioButton1.Checked == true)
                        {
                            metroRadioButton1.Checked = false;
                            metroRadioButton1.PerformClick();
                            //getAllRecord();
                        }
                        else if (metroRadioButton2.Checked == true)
                            getUserTypeRecord(metroComboBoxUserType.SelectedItem.ToString());
                        else if (metroRadioButton3.Checked == true)
                        {
                            try
                            {
                                string id = "";
                                try//only word if main customer or sub sub customer name is unique
                                {
                                    query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + textBox1.Text.Trim() + "' and status='1'";
                                    id = gm.GetTable(query).Rows[0][0].ToString();
                                }
                                catch { }
                                if (id == "")
                                {
                                    return;
                                }
                                if (id.Trim() != "")
                                {
                                    getUserNameRecord(id);
                                }
                                textBox1.Text = "";
                                textBox1.Focus();
                                //if (metroTextBox1.Text.Trim() == "")
                                //    getAllRecord();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        dataGridView1.ClearSelection();
                    }
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                }
                catch (Exception ex)
                {
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                    //MessageBox.Show(ex.Message);
                }
                try
                {
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        metroRadioButton5.Checked = true;

                        sourcesubcustomername = new AutoCompleteStringCollection();
                        DataTable dt = new DataTable();
                        dt = gm.GetTable("select * from Customer_Or_Vendor where status !='-1' and Customer_Vendor_Type='" + "Customer" + "' and Parent='" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "'");
                        foreach (DataRow d in dt.Rows)
                        {
                            sourcesubcustomername.Add(d["customer_Or_Vendor_Name"].ToString() + " (" + d["id"].ToString() + ")");
                        }

                        textBox2.DisplayMember = "customer_Or_Vendor_Name";
                        textBox2.ValueMember = "id";
                        textBox2.DataSource = dt;
                        textBox2.AutoCompleteMode = AutoCompleteMode.Suggest;
                        textBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
                        textBox2.SelectedIndex = -1;
                        getAllSubCustomersRecord();
                    }
                }
                catch { }
            }
            catch { dataGridView1.ClearSelection(); }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void metroRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (metroRadioButton2.Checked == true)
            {
                metroComboBoxUserType.Show();
                textBox1.Hide();
                metroComboBoxUserType.Focus();
                if (dataGridView2.Rows.Count > 0)
                {
                    dataGridView2.Rows.Clear();
                }
            }
        }

        private void itemCategory1_Load(object sender, EventArgs e)
        {

        }

        private void metroTextBoxCategory_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBoxCategory_TextChanged(object sender, EventArgs e)
        {

        }

        private void metroPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void metroRadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.Clear();
            }
            if (metroRadioButton3.Checked == true)
            {
                metroComboBoxUserType.Hide();
                textBox1.Show();
                textBox1.Text = string.Empty;
                textBox1.Focus();
                if (dataGridView2.Rows.Count > 0)
                {
                    dataGridView2.Rows.Clear();
                }
            }
        }

        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void metroRadioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void metroComboBoxUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                getUserTypeRecord(metroComboBoxUserType.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void metroTextBoxVendorName_TextChanged(object sender, EventArgs e)
        {

        }

        private void metroTextBoxVendorName_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBoxVendorName_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            try
            {
                call_Register_sub_customer();
            }
            catch { }
        }

        public void call_Register_sub_customer()
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    RegisterCustomerOrVendor rcv = new RegisterCustomerOrVendor(1);
                    rcv.customer_id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                    rcv.ShowDialog();
                    metroRadioButton5.Checked = false;
                    metroRadioButton4.Checked = false;
                    metroRadioButton5.Checked = true;
                }
                else
                {
                    MessageBox.Show("Select Customer First");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void call_Register_sub_customerList()
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    RegisterCustomerOrVendorList rcv = new RegisterCustomerOrVendorList(1);
                    rcv.customer_id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                    rcv.ShowDialog();
                    metroRadioButton5.Checked = false;
                    metroRadioButton4.Checked = false;
                    metroRadioButton5.Checked = true;
                }
                else
                {
                    MessageBox.Show("Select Customer First");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            call_Register_sub_customer();
        }

        public void getAllSubCustomersRecord()
        {
            try
            {
                if (dataGridView2.Rows.Count > 0)
                {
                    dataGridView2.Rows.Clear();
                }
                if (dataGridView1.SelectedRows.Count <= 0)
                {
                    return;
                }
                string query = "Select * from Customer_Or_Vendor where status != '-1' and parent='" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "'order by customer_Or_Vendor_Name ASC";
                if (dataGridView2.Rows.Count > 0)
                {
                    dataGridView2.Rows.Clear();
                }
                DataTable dt = gm.GetTable(query);
                foreach (DataRow d in dt.Rows)
                {
                    string addedby = d["AddedBy_UserId"].ToString();
                    if (addedby.Trim() != "")
                    {
                        query = "select * from login where id='" + d["AddedBy_UserId"].ToString() + "'";
                        DataTable dtlogins = gm.GetTable(query);
                        addedby = dtlogins.Rows[0][1].ToString();
                    }
                    if (d["status"].ToString() == "1")
                        dataGridView2.Rows.Add(d["id"].ToString(), d["Customer_Vendor_Type"].ToString(), d["customer_Or_Vendor_Name"].ToString(), d["Contact_Number"].ToString(), d["_Address"].ToString(), d["_date"].ToString(), d["_time"].ToString(), addedby, "Deactive", "Delete");
                    else
                        dataGridView2.Rows.Add(d["id"].ToString(), d["Customer_Vendor_Type"].ToString(), d["customer_Or_Vendor_Name"].ToString(), d["Contact_Number"].ToString(), d["_Address"].ToString(), d["_date"].ToString(), d["_time"].ToString(), addedby, "Active", "Delete");
                    if (d["status"].ToString() == "0")
                    {
                        dataGridView2.Rows[dataGridView2.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
                dataGridView2.ClearSelection();
                try
                {
                    if (dataGridView2.Rows.Count > 0)
                    {
                        groupBox3.Show();
                    }
                    else
                    {
                        groupBox3.Hide();
                    }
                }
                catch { }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            try
            {
                foreach (DataGridViewColumn c in dataGridView2.Columns)
                {
                    c.DefaultCellStyle.Font = new Font("Jameel Noori Nastaleeq", 19.5F, GraphicsUnit.Pixel);
                }
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    row.Height = 32;
                }
            }
            catch { }
        }

        private void metroRadioButton5_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (metroRadioButton5.Checked == true)
                {
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        getAllSubCustomersRecord();
                        textBox2.Hide();
                    }
                }
            }
            catch { }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //dataGridView2.Rows.Clear();
                //metroRadioButton5.Checked = false;
                //metroRadioButton4.Checked = false;
                try
                {
                    metroTextBox5.Text = "";
                    metroTextBox5.Tag = -1;
                    metroTextBox4.Text = "";
                    metroTextBox3.Text = "";
                    dataGridView2.Rows[e.RowIndex].Selected = true;
                    metroTextBox5.Text = dataGridView2.SelectedRows[0].Cells[2].Value.ToString();
                    metroTextBox5.Tag = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
                    metroTextBox4.Text = dataGridView2.SelectedRows[0].Cells[3].Value.ToString();
                    metroTextBox3.Text = dataGridView2.SelectedRows[0].Cells[4].Value.ToString();
                    if (e.ColumnIndex == 8)
                    {
                        if (dataGridView2.Rows[e.RowIndex].Cells[8].Value.ToString() == "Active")
                        {
                            if (con.State.ToString() == "Closed")
                            {
                                con.Open();
                            }
                            string query = @"update Customer_Or_Vendor set status='1' where id = '" + dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Successfully Activated");
                            if (metroRadioButton5.Checked == true)
                            {
                                getAllSubCustomersRecord();
                            }
                            else if (metroRadioButton4.Checked == true)
                            {
                                try
                                {
                                    string id = "";
                                    try//only word if main customer or sub sub customer name is unique
                                    {
                                        query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + textBox2.Text.Trim() + "' and status='1'";
                                        id = gm.GetTable(query).Rows[0][0].ToString();
                                    }
                                    catch { }
                                    if (id == "")
                                    {
                                        return;
                                    }
                                    if (id.Trim() != "")
                                    {
                                        getSubUserNameRecord(id);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                        }
                        else
                        {
                            string query = @"select * from bill where status='1' and customer_vendor_id = '" + dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "' and bill_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                            DataTable d_t = gm.GetTable(query);
                            if (d_t.Rows.Count > 0)
                            {
                                MessageBox.Show("Customer have Bill of current date.\nFirst delete bill of this customer and deactive again");
                                return;
                            }
                            query = @"select * from customer_or_vendor where status='1' and id in (select parent from customer_or_vendor where id = '" + dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "')";
                            d_t = gm.GetTable(query);
                            if (d_t.Rows.Count <= 0)
                            {
                                MessageBox.Show("Customer Parent/Main Customer is deactivate.\nFirst Active Parent/Main Customer of this customer and deactive again");
                                return;
                            }
                            //if (dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() == Pos_Sabzi_Mandi.Properties.Settings.Default.loginid)
                            //{
                            //    MessageBox.Show("You Can't Deactive yourself");
                            //    return;
                            //}
                            if (con.State.ToString() == "Closed")
                            {
                                con.Open();
                            }
                            query = @"update Customer_Or_Vendor set status='0' where id = '" + dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Successfully Deactivated");
                            if (metroRadioButton5.Checked == true)
                            {
                                getAllSubCustomersRecord();
                            }
                            else if (metroRadioButton4.Checked == true)
                            {
                                try
                                {
                                    string id = "";
                                    try//only word if main customer or sub sub customer name is unique
                                    {
                                        query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + textBox2.Text.Trim() + "' and status='1'";
                                        id = gm.GetTable(query).Rows[0][0].ToString();
                                    }
                                    catch { }
                                    if (id == "")
                                    {
                                        return;
                                    }
                                    if (id.Trim() != "")
                                    {
                                        getSubUserNameRecord(id);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                        }
                        dataGridView2.ClearSelection();
                    }
                    if (e.ColumnIndex == 9)
                    {
                        string query = @"select * from bill where status='1' and customer_vendor_id = '" + dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "' and bill_date='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                        DataTable d_t = gm.GetTable(query);
                        if (d_t.Rows.Count > 0)
                        {
                            MessageBox.Show("Customer have Bill of current date.\nFirst delete bill of this customer and delete again");
                            return;
                        }
                        query = @"select * from customer_or_vendor where status='1' and id in (select parent from customer_or_vendor where id = '" + dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "')";
                        d_t = gm.GetTable(query);
                        if (d_t.Rows.Count <= 0)
                        {
                            MessageBox.Show("Customer Parent/Main Customer is deactivate.\nFirst Active Parent/Main Customer of this customer and delete again");
                            return;
                        }
                        //if (dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() == Pos_Sabzi_Mandi.Properties.Settings.Default.loginid)
                        //{
                        //    MessageBox.Show("You Can't Delete yourself");
                        //    return;
                        //}
                        DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this user?(yes/no)", "Confirmation", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.No)
                        {
                            return;
                        }
                        query = @"update Customer_Or_Vendor set status='-1' where id = '" + dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
                        gm.ExecuteNonQuery(query);
                        MessageBox.Show("Delete Successfully");
                        try
                        {
                            sourcesubcustomername = new AutoCompleteStringCollection();
                            DataTable dt = new DataTable();
                            dt = gm.GetTable("select * from Customer_Or_Vendor where status !='-1' and Customer_Vendor_Type='" + "Customer" + "' and Parent='" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "'");
                            foreach (DataRow d in dt.Rows)
                            {
                                sourcesubcustomername.Add(d["customer_Or_Vendor_Name"].ToString() + " (" + d["id"].ToString() + ")");
                            }

                            textBox2.DisplayMember = "customer_Or_Vendor_Name";
                            textBox2.ValueMember = "id";
                            textBox2.DataSource = dt;
                            textBox2.AutoCompleteMode = AutoCompleteMode.Suggest;
                            textBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
                            textBox2.SelectedIndex = -1;
                        }
                        catch { }
                        if (metroRadioButton5.Checked == true)
                        {
                            getAllSubCustomersRecord();
                        }
                        else if (metroRadioButton4.Checked == true)
                        {
                            try
                            {
                                string id = "";
                                try//only word if main customer or sub sub customer name is unique
                                {
                                    query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + textBox2.Text.Trim() + "' and status='1'";
                                    id = gm.GetTable(query).Rows[0][0].ToString();
                                }
                                catch { }
                                if (id == "")
                                {
                                    return;
                                }
                                if (id.Trim() != "")
                                {
                                    getSubUserNameRecord(id);
                                }
                                textBox2.Text = "";
                                textBox2.Focus();
                                //if (metroTextBox1.Text.Trim() == "")
                                //    getAllRecord();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        dataGridView2.ClearSelection();
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
                //metroRadioButton5.Checked = true;
                //dataGridView2.Rows[e.RowIndex].Selected = true;
            }
            catch { dataGridView2.ClearSelection(); }
        }

        private void metroRadioButton4_CheckedChanged_1(object sender, EventArgs e)
        {
            try
            {
                sourcesubcustomername = new AutoCompleteStringCollection();
                DataTable dt = new DataTable();
                dt = gm.GetTable("select * from Customer_Or_Vendor where status !='-1' and Customer_Vendor_Type='" + "Customer" + "' and Parent='" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "' order by customer_Or_Vendor_Name ASC");
                foreach (DataRow d in dt.Rows)
                {
                    sourcesubcustomername.Add(d["customer_Or_Vendor_Name"].ToString() + " (" + d["id"].ToString() + ")");
                }

                textBox2.DisplayMember = "customer_Or_Vendor_Name";
                textBox2.ValueMember = "id";
                textBox2.DataSource = dt;
                textBox2.AutoCompleteMode = AutoCompleteMode.Suggest;
                textBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
                textBox2.SelectedIndex = -1;
            }
            catch { }
            try
            {
                if (dataGridView2.Rows.Count > 0)
                {
                    dataGridView2.Rows.Clear();
                }
                if (metroRadioButton4.Checked == true)
                {
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        textBox2.Text = "";
                        textBox2.Show();
                    }
                }
            }
            catch { }
        }

        int panelheight = 0;
        int formheight = 0;
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Rows[dataGridView1.SelectedRows[0].Cells[0].RowIndex].Selected = true;
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    metroRadioButton5.Checked = true;

                    sourcesubcustomername = new AutoCompleteStringCollection();
                    DataTable dt = new DataTable();
                    dt = gm.GetTable("select * from Customer_Or_Vendor where status !='-1' and Customer_Vendor_Type='" + "Customer" + "' and Parent='" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "'");
                    foreach (DataRow d in dt.Rows)
                    {
                        sourcesubcustomername.Add(d["customer_Or_Vendor_Name"].ToString() + " (" + d["id"].ToString() + ")");
                    }

                    textBox2.DisplayMember = "customer_Or_Vendor_Name";
                    textBox2.ValueMember = "id";
                    textBox2.DataSource = dt;
                    textBox2.AutoCompleteMode = AutoCompleteMode.Suggest;
                    textBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
                    textBox2.SelectedIndex = -1;
                    getAllSubCustomersRecord();
                }
            }
            catch { }
        }

        private void metroTextBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void metroTile6_Click(object sender, EventArgs e)
        {
            try
            {
                call_Register_sub_customerList();
            }
            catch { }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.Clear();
            }
            if (dataGridView2.Rows.Count > 0)
            {
                dataGridView2.Rows.Clear();
            }
            try
            {
                string id = "";
                try//only word if main customer or sub sub customer name is unique
                {
                    string query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + textBox1.Text.Trim() + "' and status!='-1' order by customer_Or_Vendor_Name ASC";
                    id = gm.GetTable(query).Rows[0][0].ToString();
                }
                catch { }
                if (id == "")
                {
                    return;
                }
                if (id.Trim() != "")
                {
                    getUserNameRecord(id);
                }
                //if (metroTextBox1.Text.Trim() == "")
                //    getAllRecord();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count > 0)
            {
                dataGridView2.Rows.Clear();
            }
            try
            {
                string id = "";
                try//only word if main customer or sub sub customer name is unique
                {
                    string query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + textBox2.Text.Trim() + "' and status='1'";
                    id = gm.GetTable(query).Rows[0][0].ToString();
                }
                catch { }
                if (id == "")
                {
                    return;
                }
                if (id.Trim() != "")
                {
                    getSubUserNameRecord(id);
                }
                //if (metroTextBox1.Text.Trim() == "")
                //    getAllRecord();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
    }
}
