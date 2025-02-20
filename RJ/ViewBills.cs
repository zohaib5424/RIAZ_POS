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
using System.Globalization;

namespace RJ
{
    public partial class ViewBills : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public ViewBills()
        {
            InitializeComponent();
        }

        string bill_type = "";
        public ViewBills(string form)
        {
            InitializeComponent();
            textBox4.Visible = false;
            if (form == "purchase trading")
            {
                metroTextBox2.PromptText = "Enter Transaction #";
                metroRadioButton4.Text = "By Transaction #";
                dataGridView1.Columns[1].HeaderText = "Transaction Id";
                metroRadioButton3.Show();
                metroRadioButton3.Text = "By Vendor Name";
                dataGridView2.Hide();
                metroLabel4.Text = "Purchase Items Bills";
                bill_type = "Purchase Trading";
                metroTile1.Show();
                label1.Show();
            }
            else if (form == "sale trading")
            {
                textBox4.Visible = true;
                metroTextBox2.PromptText = "Enter Transaction #";
                metroRadioButton4.Text = "By Transaction #";
                dataGridView1.Columns[1].HeaderText = "Transaction Id";
                metroRadioButton3.Show();
                metroRadioButton3.Text = "By Sub Customer Name";
                metroLabel4.Text = "Sale Items Bills";
                bill_type = "Sale Trading";
                metroLabel1.Show();
                dataGridView3.Show();
                dataGridView2.Hide();
                metroTile1.Show();
                label1.Show();
            }
            else if (form == "expense")
            {
                metroTextBox2.PromptText = "Enter Expense";
                metroRadioButton4.Text = "By Expense Name";
                metroRadioButton3.Hide();
                dataGridView1.Columns[1].HeaderText = "Expense";
                metroLabel4.Text = "Expenses";
                bill_type = "Expense";
                metroLabel1.Hide();
                dataGridView3.Hide();
                dataGridView2.Hide();
                metroTile1.Hide();
                label1.Hide();
            }
            else if (form == "payment")
            {
                metroTextBox2.PromptText = "Enter Expense";
                metroRadioButton4.Text = "By Expense Name";
                metroRadioButton3.Hide();
                dataGridView1.Columns[1].HeaderText = "Expense";
                metroLabel4.Text = "Expenses";
                bill_type = "Expense";
                metroLabel1.Hide();
                dataGridView3.Hide();
                dataGridView2.Hide();
                metroTile1.Hide();
                label1.Hide();
            }
        }
        AutoCompleteStringCollection sourcetransactionid = new AutoCompleteStringCollection();
        AutoCompleteStringCollection sourcecustomername = new AutoCompleteStringCollection();
        AutoCompleteStringCollection sourcevendorname = new AutoCompleteStringCollection();
        AutoCompleteStringCollection main_sourceCustomer_Or_Vendor = new AutoCompleteStringCollection();
        private void ItemsList_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            try
            {
                metroDateTimeFrom.Value = DateTime.Now.Date;
                metroDateTimeTo.Value = DateTime.Now.Date;
            }
            catch { }
            dataGridView2.Columns[7].DisplayIndex = 6;
            //label5.Hide();
            //metroTile3.Hide();
            //metroRadioButton2.Hide();
            label4.Hide();
            metroDateTimeFrom.MaxDate = metroDateTimeTo.Value;
            metroDateTimeTo.MinDate = metroDateTimeFrom.Value;
            metroDateTime2.MaxDate = metroDateTime1.Value;
            metroDateTime1.MinDate = metroDateTime2.Value;
            groupBox2.Visible = false;
            //dataGridView1.Columns[11].Visible = false;
            try
            {
                sourcetransactionid = new AutoCompleteStringCollection();
                main_sourceCustomer_Or_Vendor = new AutoCompleteStringCollection();
                sourcevendorname = new AutoCompleteStringCollection();
                sourcevendorname = new AutoCompleteStringCollection();
                metroTextBox1.Hide();

                this.AutoScroll = true;

                string customervendortype = "Vendor";
                DataTable dt = gm.GetTable("select * from customer_or_vendor where customer_vendor_type='" + customervendortype + "' and status ='1'");
                foreach (DataRow d in dt.Rows)
                {
                    sourcevendorname.Add(d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")");
                }

                metroTextBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                metroTextBox1.AutoCompleteCustomSource = sourcevendorname;
                metroTextBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

                dt = gm.GetTable("select * from bill where bill_type='" + bill_type + "' and status ='1'");
                foreach (DataRow d in dt.Rows)
                {
                    sourcetransactionid.Add(d["Bill_Id"].ToString());
                }

                metroTextBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                metroTextBox2.AutoCompleteCustomSource = sourcetransactionid;
                metroTextBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;

                dt = gm.GetTable("select * from customer_or_vendor where status ='1' and customer_vendor_type='Customer' and parent=''");
                foreach (DataRow d in dt.Rows)
                {
                    main_sourceCustomer_Or_Vendor.Add(d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")");
                }

                textBox4.DisplayMember = "customer_Or_Vendor_Name";
                textBox4.ValueMember = "id";
                textBox4.DataSource = dt;
                textBox4.AutoCompleteMode = AutoCompleteMode.Suggest;
                textBox4.AutoCompleteSource = AutoCompleteSource.ListItems;
                textBox4.SelectedIndex = -1;

                customervendortype = "Customer";
                dt = gm.GetTable("select * from customer_or_vendor where customer_vendor_type='" + customervendortype + "' and status ='1'");
                foreach (DataRow d in dt.Rows)
                {
                    sourcecustomername.Add(d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")");
                }

                textBox3.DisplayMember = "customer_Or_Vendor_Name";
                textBox3.ValueMember = "id";
                textBox3.DataSource = dt;
                textBox3.AutoCompleteMode = AutoCompleteMode.Suggest;
                textBox3.AutoCompleteSource = AutoCompleteSource.ListItems;
                textBox3.SelectedIndex = -1;

                metroDateTimeFrom.Value = metroDateTimeTo.Value = DateTime.Now;
                metroDateTimeTo.MinDate = metroDateTimeFrom.Value.Date;
                metroLabel3.Hide();
                metroLabel5.Hide();
                if (bill_type == "Purchase Trading")
                {
                    dataGridView2.Hide();
                    dataGridView3.Show();
                }
                if (bill_type == "Sale Trading")
                {
                    dataGridView2.Show();
                    dataGridView3.Hide();
                }

                metroRadioButton1.Checked = true;

                dataGridView1.Columns["Column15"].DisplayIndex = 2;
                dataGridView1.Columns["Column16"].DisplayIndex = 3;
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
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    c.DefaultCellStyle.Font = new Font("Jameel Noori Nastaleeq", 19.5F, GraphicsUnit.Pixel);
                }
            }
            catch { }
        }

        GMDB gm = new GMDB();
        public void getAllRecord()
        {
            try
            {
                string customerid = "";
                string query = "";
                if (textBox4.Text.Trim().Length > 0)
                {
                    string id = "";
                    try//only word if main customer or sub sub customer name is unique
                    {
                        query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + textBox4.Text.Trim() + "' and status='1'";
                        id = gm.GetTable(query).Rows[0][0].ToString();
                    }
                    catch { }
                    if (id == "")
                    {
                        return;
                    }
                    customerid = id;
                }
                query = "Select * from bill where bill_type='" + bill_type + "' and status != '-1' and bill_date='" + (DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString()) + "' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where status='1' and parent='" + customerid + "') order by Bill_Id";

                if (bill_type == "Sale Trading")
                {
                }
                if (customerid != "")
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                        dataGridView1.Rows.Clear();
                    }
                    if (dataGridView2.Rows.Count > 0)
                    {
                        dataGridView2.Rows.Clear();
                    }
                    DataTable dt = gm.GetTable(query);
                    int i = 1;
                    foreach (DataRow d in dt.Rows)
                    {
                        string addedby = d["AddedBy_UserId"].ToString();
                        if (addedby.Trim() != "")
                        {
                            query = "select * from login where id='" + d["AddedBy_UserId"].ToString() + "'";
                            DataTable dtlogins = gm.GetTable(query);
                            addedby = dtlogins.Rows[0][1].ToString() + " (" + dtlogins.Rows[0][0].ToString() + ")";
                        }
                        string bill_date2 = d["bill_date"].ToString();
                        if (bill_date2.Contains(' '))
                        {
                            bill_date2 = bill_date2.Split(' ')[0].ToString().Trim();
                        }
                        string main_Customer = "";
                        string main_Customer_id = "";
                        string sub_Customer = "";
                        string sub_Customer_id = "";

                        try
                        {
                            query = "Select * from Customer_Or_Vendor where id='" + d["Customer_Vendor_Id"].ToString() + "'";
                            DataTable dt_customer_information = gm.GetTable(query);
                            if (dt_customer_information.Rows.Count > 0)
                            {
                                sub_Customer = dt_customer_information.Rows[0]["customer_Or_Vendor_Name"].ToString();
                                sub_Customer_id = dt_customer_information.Rows[0]["id"].ToString();
                                query = "Select * from Customer_Or_Vendor where id='" + dt_customer_information.Rows[0]["parent"].ToString() + "'";
                                dt_customer_information = gm.GetTable(query);
                                if (dt_customer_information.Rows.Count > 0)
                                {
                                    main_Customer = dt_customer_information.Rows[0]["customer_Or_Vendor_Name"].ToString();
                                    main_Customer_id = dt_customer_information.Rows[0]["id"].ToString();
                                }
                            }
                        }
                        catch { }
                        string total_amount = d["total_amount"].ToString();
                        //try
                        //{
                        //    total_amount = ((Math.Round((double.Parse(total_amount)), 0, MidpointRounding.AwayFromZero))).ToString();
                        //}
                        //catch { }
                        dataGridView1.Rows.Add(d["id"].ToString(), d["bill_id"].ToString(), bill_date2, d["bill_time"].ToString(), d["discount_amount"].ToString(), d["tax_amount"].ToString(), d["service_charges"].ToString(), gm.removePointsZero(gm.removePoints(total_amount)), gm.removePointsZero(gm.removePoints(d["paid_amount"].ToString())), d["payment_method"].ToString(), addedby, "Delete Bill", main_Customer, sub_Customer, main_Customer_id, sub_Customer_id);
                        try
                        {
                            DateTime dtm = Convert.ToDateTime(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Value.ToString());
                            if (dtm.Date != DateTime.Now.Date && RJ.Properties.Settings.Default.EnableDeleteButton == "0")
                            {
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[11].Value = "";
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[11].ReadOnly = true;
                            }
                        }
                        catch { }
                        i++;
                        //if (d["status"].ToString() == "1")
                        //    dataGridView1.Rows.Add(d["id"].ToString(), d["username"].ToString(), d["User_Id"].ToString(), d["_date"].ToString(), d["_time"].ToString(), addedby, "Deactive", "Delete");
                        //else
                        //    dataGridView1.Rows.Add(d["id"].ToString(), d["username"].ToString(), d["User_Id"].ToString(), d["_date"].ToString(), d["_time"].ToString(), addedby, "Active", "Delete");
                        if (d["status"].ToString() == "0")
                        {
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                        }
                    }
                    dataGridView1.ClearSelection();
                }
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
            //dataGridView1.Sort(dataGridView1.Columns[13], System.ComponentModel.ListSortDirection.Ascending);
            //dataGridView1.Sort(dataGridView1.Columns[2], System.ComponentModel.ListSortDirection.Descending);
            foreach (DataGridViewRow d in dataGridView1.Rows)
            {
                DateTime dtm = Convert.ToDateTime(d.Cells[2].Value.ToString());
                d.Cells[2].Value = dtm.Date.Day.ToString() + "-" + dtm.Date.Month.ToString() + "-" + dtm.Date.Year.ToString();
            }
        }

        public void getDateRecord()
        {
            try
            {
                string customerid = "";
                string query = "";
                if (textBox4.Text.ToString().Trim().Length > 0)
                {
                    string id = "";
                    try//only word if main customer or sub sub customer name is unique
                    {
                        query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + textBox4.Text.Trim() + "' and status='1'";
                        id = gm.GetTable(query).Rows[0][0].ToString();
                    }
                    catch { }
                    if (id == "")
                    {
                        return;
                    }
                    customerid = id;
                }
                if (customerid != "")
                {
                    string from = metroDateTimeFrom.Value.Date.Year.ToString() + "-" + metroDateTimeFrom.Value.Date.Month.ToString() + "-" + metroDateTimeFrom.Value.Date.Day.ToString();
                    string to = metroDateTimeTo.Value.Date.Year.ToString() + "-" + metroDateTimeTo.Value.Date.Month.ToString() + "-" + metroDateTimeTo.Value.Date.Day.ToString();
                    query = "";// "Select * from bill where bill_type='" + bill_type + "' and bill_date >= '" + from + "' and bill_date<='" + to + "' and status != '-1'";
                    if (bill_type == "Sale Trading")
                    {
                        query = "Select * from bill where bill_type='" + bill_type + "' and bill_date >= '" + from + "' and bill_date<='" + to + "' and status != '-1' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='" + customerid + "')  order by Bill_Id";
                        //query = "Select * from bill where bill_type='" + bill_type + "' and bill_date >= '" + from + "' and bill_date<='" + to + "' and status != '-1' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='" + customerid + "') ORDER BY Bill_Date ASC, Bill_Id ASC";
                    }
                    if (dataGridView1.Rows.Count > 0)
                    {
                        dataGridView1.Rows.Clear();
                    }
                    if (dataGridView2.Rows.Count > 0)
                    {
                        dataGridView2.Rows.Clear();
                    }
                    DataTable dt = gm.GetTable(query);
                    //DataView dv = dt.DefaultView;
                    //dv.Sort = "Bill_Id ASC";
                    //dt = dv.ToTable();
                    //dt.DefaultView.Sort = "Bill_Id ASC"; 
                    int i = 1;
                    foreach (DataRow d in dt.Rows)
                    {
                        string addedby = d["AddedBy_UserId"].ToString();
                        if (addedby.Trim() != "")
                        {
                            query = "select * from login where id='" + d["AddedBy_UserId"].ToString() + "'";
                            DataTable dtlogins = gm.GetTable(query);
                            addedby = dtlogins.Rows[0][1].ToString() + " (" + dtlogins.Rows[0][0].ToString() + ")";
                        }
                        string bill_date2 = d["bill_date"].ToString();
                        if (bill_date2.Contains(' '))
                        {
                            bill_date2 = bill_date2.Split(' ')[0].ToString().Trim();
                        }

                        string main_Customer = "";
                        string main_Customer_id = "";
                        string sub_Customer = "";
                        string sub_Customer_id = "";

                        try
                        {
                            query = "Select * from Customer_Or_Vendor where id='" + d["Customer_Vendor_Id"].ToString() + "'";
                            DataTable dt_customer_information = gm.GetTable(query);
                            if (dt_customer_information.Rows.Count > 0)
                            {
                                sub_Customer = dt_customer_information.Rows[0]["customer_Or_Vendor_Name"].ToString();
                                sub_Customer_id = dt_customer_information.Rows[0]["id"].ToString();
                                query = "Select * from Customer_Or_Vendor where id='" + dt_customer_information.Rows[0]["parent"].ToString() + "'";
                                dt_customer_information = gm.GetTable(query);
                                if (dt_customer_information.Rows.Count > 0)
                                {
                                    main_Customer = dt_customer_information.Rows[0]["customer_Or_Vendor_Name"].ToString();
                                    main_Customer_id = dt_customer_information.Rows[0]["id"].ToString();
                                }
                            }
                        }
                        catch { }

                        dataGridView1.Rows.Add(d["id"].ToString(), d["bill_id"].ToString(), bill_date2, d["bill_time"].ToString(), d["discount_amount"].ToString(), d["tax_amount"].ToString(), d["service_charges"].ToString(), gm.removePointsZero(gm.removePoints(d["total_amount"].ToString())), gm.removePointsZero(gm.removePoints(d["paid_amount"].ToString())), d["payment_method"].ToString(), addedby, "Delete Bill", main_Customer, sub_Customer, main_Customer_id, sub_Customer_id);
                        try
                        {
                            DateTime dtm = Convert.ToDateTime(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Value.ToString());
                            if (dtm.Date != DateTime.Now.Date && RJ.Properties.Settings.Default.EnableDeleteButton == "0")
                            {
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[11].Value = "";
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[11].ReadOnly = true;
                            }
                        }
                        catch { }
                        i++;
                        //if (d["status"].ToString() == "1")
                        //    dataGridView1.Rows.Add(d["id"].ToString(), d["username"].ToString(), d["User_Id"].ToString(), d["_date"].ToString(), d["_time"].ToString(), addedby, "Deactive", "Delete");
                        //else
                        //    dataGridView1.Rows.Add(d["id"].ToString(), d["username"].ToString(), d["User_Id"].ToString(), d["_date"].ToString(), d["_time"].ToString(), addedby, "Active", "Delete");
                        if (d["status"].ToString() == "0")
                        {
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                        }
                    }
                    dataGridView1.ClearSelection();
                }
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
            //dataGridView1.Sort(dataGridView1.Columns[13], System.ComponentModel.ListSortDirection.Ascending);
            //dataGridView1.Sort(dataGridView1.Columns[2], System.ComponentModel.ListSortDirection.Descending);
            foreach (DataGridViewRow d in dataGridView1.Rows)
            {
                DateTime dtm = Convert.ToDateTime(d.Cells[2].Value.ToString());
                d.Cells[2].Value = dtm.Date.Day.ToString() + "-" + dtm.Date.Month.ToString() + "-" + dtm.Date.Year.ToString();
            }
        }

        public void getUserNameRecord(string id)
        {
            try
            {
                string query = "Select * from bill where bill_type='" + bill_type + "' and customer_vendor_id='" + id + "' and status != '-1' order by bill_date ASC";
                if (metroRadioButton8.Checked == true)//between dates
                {
                    string from = metroDateTime2.Value.Date.Year.ToString() + "-" + metroDateTime2.Value.Date.Month.ToString() + "-" + metroDateTime2.Value.Date.Day.ToString();
                    string to = metroDateTime1.Value.Date.Year.ToString() + "-" + metroDateTime1.Value.Date.Month.ToString() + "-" + metroDateTime1.Value.Date.Day.ToString();
                    query = "Select * from bill where bill_type='" + bill_type + "' and bill_date >= '" + from + "' and bill_date<='" + to + "' and customer_vendor_id='" + id + "' and status != '-1' order by bill_date ASC";
                }
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                if (dataGridView2.Rows.Count > 0)
                {
                    dataGridView2.Rows.Clear();
                }
                DataTable dt = gm.GetTable(query);
                int i = 1;
                foreach (DataRow d in dt.Rows)
                {
                    string addedby = d["AddedBy_UserId"].ToString();
                    if (addedby.Trim() != "")
                    {
                        query = "select * from login where id='" + d["AddedBy_UserId"].ToString() + "'";
                        DataTable dtlogins = gm.GetTable(query);
                        addedby = dtlogins.Rows[0][1].ToString() + " (" + dtlogins.Rows[0][0].ToString() + ")";
                    }
                    string bill_date2 = d["bill_date"].ToString();
                    if (bill_date2.Contains(' '))
                    {
                        bill_date2 = bill_date2.Split(' ')[0].ToString().Trim();
                    }
                    string main_Customer = "";
                    string main_Customer_id = "";
                    string sub_Customer = "";
                    string sub_Customer_id = "";

                    try
                    {
                        query = "Select * from Customer_Or_Vendor where id='" + d["Customer_Vendor_Id"].ToString() + "'";
                        DataTable dt_customer_information = gm.GetTable(query);
                        if (dt_customer_information.Rows.Count > 0)
                        {
                            sub_Customer = dt_customer_information.Rows[0]["customer_Or_Vendor_Name"].ToString();
                            sub_Customer_id = dt_customer_information.Rows[0]["id"].ToString();
                            query = "Select * from Customer_Or_Vendor where id='" + dt_customer_information.Rows[0]["parent"].ToString() + "'";
                            dt_customer_information = gm.GetTable(query);
                            if (dt_customer_information.Rows.Count > 0)
                            {
                                main_Customer = dt_customer_information.Rows[0]["customer_Or_Vendor_Name"].ToString();
                                main_Customer_id = dt_customer_information.Rows[0]["id"].ToString();
                            }
                        }
                    }
                    catch { }

                    dataGridView1.Rows.Add(d["id"].ToString(), d["bill_id"].ToString(), bill_date2, d["bill_time"].ToString(), d["discount_amount"].ToString(), d["tax_amount"].ToString(), d["service_charges"].ToString(), gm.removePointsZero(gm.removePoints(d["total_amount"].ToString())), gm.removePointsZero(gm.removePoints(d["paid_amount"].ToString())), d["payment_method"].ToString(), addedby, "Delete Bill", main_Customer, sub_Customer, main_Customer_id, sub_Customer_id);
                    try
                    {
                        DateTime dtm = Convert.ToDateTime(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Value.ToString());
                        if (dtm.Date != DateTime.Now.Date && RJ.Properties.Settings.Default.EnableDeleteButton == "0")
                        {
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[11].Value = "";
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[11].ReadOnly = true;
                        }
                    }
                    catch { }
                    i++;
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
            //dataGridView1.Sort(dataGridView1.Columns[2], System.ComponentModel.ListSortDirection.Descending);
            foreach (DataGridViewRow d in dataGridView1.Rows)
            {
                DateTime dtm = Convert.ToDateTime(d.Cells[2].Value.ToString());
                d.Cells[2].Value = dtm.Date.Day.ToString() + "-" + dtm.Date.Month.ToString() + "-" + dtm.Date.Year.ToString();
            }
        }

        public void getTransactionIdRecord(string id)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.Clear();
            }
            try
            {
                string query = "Select * from bill where bill_type='" + bill_type + "' and bill_id='" + id + "' and status != '-1'";
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                if (dataGridView2.Rows.Count > 0)
                {
                    dataGridView2.Rows.Clear();
                }
                DataTable dt = gm.GetTable(query);
                int i = 1;
                foreach (DataRow d in dt.Rows)
                {
                    string addedby = d["AddedBy_UserId"].ToString();
                    if (addedby.Trim() != "")
                    {
                        query = "select * from login where id='" + d["AddedBy_UserId"].ToString() + "'";
                        DataTable dtlogins = gm.GetTable(query);
                        addedby = dtlogins.Rows[0][1].ToString() + " (" + dtlogins.Rows[0][0].ToString() + ")";
                    }
                    string bill_date2 = d["bill_date"].ToString();
                    if (bill_date2.Contains(' '))
                    {
                        bill_date2 = bill_date2.Split(' ')[0].ToString().Trim();
                    }

                    string main_Customer = "";
                    string main_Customer_id = "";
                    string sub_Customer = "";
                    string sub_Customer_id = "";

                    try
                    {
                        query = "Select * from Customer_Or_Vendor where id='" + d["Customer_Vendor_Id"].ToString() + "'";
                        DataTable dt_customer_information = gm.GetTable(query);
                        if (dt_customer_information.Rows.Count > 0)
                        {
                            sub_Customer = dt_customer_information.Rows[0]["customer_Or_Vendor_Name"].ToString();
                            sub_Customer_id = dt_customer_information.Rows[0]["id"].ToString();
                            query = "Select * from Customer_Or_Vendor where id='" + dt_customer_information.Rows[0]["parent"].ToString() + "'";
                            dt_customer_information = gm.GetTable(query);
                            if (dt_customer_information.Rows.Count > 0)
                            {
                                main_Customer = dt_customer_information.Rows[0]["customer_Or_Vendor_Name"].ToString();
                                main_Customer_id = dt_customer_information.Rows[0]["id"].ToString();
                            }
                        }
                    }
                    catch { }

                    dataGridView1.Rows.Add(d["id"].ToString(), d["bill_id"].ToString(), bill_date2, d["bill_time"].ToString(), d["discount_amount"].ToString(), d["tax_amount"].ToString(), d["service_charges"].ToString(), gm.removePointsZero(gm.removePoints(d["total_amount"].ToString())), gm.removePointsZero(gm.removePoints(d["paid_amount"].ToString())), d["payment_method"].ToString(), addedby, "Delete Bill", main_Customer, sub_Customer, main_Customer_id, sub_Customer_id);
                    try
                    {
                        DateTime dtm = Convert.ToDateTime(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Value.ToString());
                        if (dtm.Date != DateTime.Now.Date && RJ.Properties.Settings.Default.EnableDeleteButton == "0")
                        {
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[11].Value = "";
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[11].ReadOnly = true;
                        }
                    }
                    catch { }
                    i++;
                    if (d["status"].ToString() == "0")
                    {
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
                dataGridView1.ClearSelection();
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows[0].Selected = true;
                }
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
            foreach (DataGridViewRow d in dataGridView1.Rows)
            {
                DateTime dtm = Convert.ToDateTime(d.Cells[2].Value.ToString());
                d.Cells[2].Value = dtm.Date.Day.ToString() + "-" + dtm.Date.Month.ToString() + "-" + dtm.Date.Year.ToString();
            }
        }

        private void metroRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            metroTile2.Visible = true;
            label5.Show();
            metroTile3.Show();
            label4.Visible = false;
            textBox4.Visible = true;
            groupBox2.Visible = false;
            if (dataGridView1.Rows.Count > 0)
                dataGridView1.Rows.Clear();
            if (dataGridView2.Rows.Count > 0)
                dataGridView2.Rows.Clear();
            if (dataGridView3.Rows.Count > 0)
                dataGridView3.Rows.Clear();
            textBox3.Hide();
            metroTextBox2.Hide();
            metroTextBox1.Hide();
            metroLabel3.Hide();
            metroLabel5.Hide();
            metroDateTimeFrom.Hide();
            metroDateTimeTo.Hide();
            getAllRecord();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex == -1)
            //    return;
            try
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    if (bill_type == "Purchase Trading")
                    {
                        if (dataGridView3.Rows.Count > 0)
                            dataGridView3.Rows.Clear();
                        string query = "select * from bill_details where bill_id = '" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString().Trim() + "' and unit_cost!=0";
                        DataTable dt = gm.GetTable(query);
                        foreach (DataRow d in dt.Rows)
                        {
                            string item = "";
                            query = "select * from items where id = '" + d["item_id"].ToString() + "'";
                            DataTable dt2 = gm.GetTable(query);
                            try
                            {
                                item = dt2.Rows[0]["Name"].ToString();
                            }
                            catch { }
                            dataGridView3.Rows.Add(d["item_id"].ToString(), item, d["qty"].ToString(), d["unit_cost"].ToString(), d["total_amount"].ToString(), d["manufacturing_date"].ToString(), d["expiry_date"].ToString(), d["batch_or_lot_number"].ToString());
                        }
                    }
                    if (bill_type == "Sale Trading")
                    {
                        if (e.ColumnIndex == 11)//delete bill
                        {
                            if (dataGridView1.Rows[e.RowIndex].Cells[11].Value.ToString().Trim() != "")
                            {
                                try
                                {
                                    if (MessageBox.Show("Delete Bill.\nAre you sure to delete bill? (yes / no)", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        string query = "update bill set status='-1' where id='" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + "'";
                                        gm.ExecuteNonQuery(query);
                                        MessageBox.Show("Bill Succesffully Deleted");
                                        if (metroRadioButton2.Checked == true)
                                        {
                                            getDateRecord();
                                        }
                                        else if (metroRadioButton1.Checked == true)
                                        {
                                            getAllRecord();
                                        }
                                        else if (metroRadioButton4.Checked == true)
                                        {
                                            try
                                            {
                                                if (metroTextBox2.Text.Trim() != "")
                                                {
                                                    getTransactionIdRecord(metroTextBox2.Text.Trim());
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(ex.Message);
                                            }
                                        }
                                        else if (metroRadioButton3.Checked == true)
                                        {
                                            try
                                            {
                                                string id = "";
                                                try//only word if main customer or sub sub customer name is unique
                                                {
                                                    query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + textBox3.Text.Trim() + "' and status='1'";
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
                                                //if (metroTextBox3.Text.Trim() == "")
                                                //    getAllRecord();
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show(ex.Message);
                                            }
                                        }

                                    }
                                }
                                catch { }
                            }

                        }
                        else
                        {
                            if (dataGridView2.Rows.Count > 0)
                                dataGridView2.Rows.Clear();
                            string query = "select * from bill_details where bill_id = '" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString().Trim() + "' and unit_cost!=0 order by id";
                            DataTable dt = gm.GetTable(query);
                            string total_mazdori1 = "0";
                            foreach (DataRow d in dt.Rows)
                            {
                                string item = "";
                                query = "select * from items where id = '" + d["item_id"].ToString() + "'";
                                DataTable dt2 = gm.GetTable(query);
                                try
                                {
                                    item = dt2.Rows[0]["Name"].ToString();
                                }
                                catch { }
                                string weight = "";
                                query = "select * from items where id = '" + d["item_id"].ToString() + "'";
                                dt2 = gm.GetTable(query);
                                try
                                {
                                    weight = dt2.Rows[0]["weight"].ToString();
                                }
                                catch { }

                                string unit_cost1 = (d["unit_cost"].ToString().Contains('.') ? (d["unit_cost"].ToString().Split('.')[1].ToString() == "000" ? d["unit_cost"].ToString().Split('.')[0].ToString() : d["unit_cost"].ToString()) : d["unit_cost"].ToString());
                                string qty1 = (d["qty"].ToString().Contains('.') ? (d["qty"].ToString().Split('.')[1].ToString() == "000" ? d["qty"].ToString().Split('.')[0].ToString() : d["qty"].ToString()) : d["qty"].ToString());
                                string total_amount1 = (d["total_amount"].ToString().Contains('.') ? (d["total_amount"].ToString().Split('.')[1].ToString() == "000" ? d["total_amount"].ToString().Split('.')[0].ToString() : d["total_amount"].ToString()) : d["total_amount"].ToString());
                                string mazdori1 = (d["mazdori"].ToString().Contains('.') ? (d["mazdori"].ToString().Split('.')[1].ToString() == "000" ? d["mazdori"].ToString().Split('.')[0].ToString() : d["mazdori"].ToString()) : d["mazdori"].ToString());
                                try
                                {
                                    total_amount1 = ((Math.Round((double.Parse(total_amount1) - (double.Parse(mazdori1) * double.Parse(qty1))), 0, MidpointRounding.AwayFromZero))).ToString();
                                }
                                catch { }
                                try
                                {
                                    total_mazdori1 = (double.Parse(total_mazdori1) + (double.Parse(mazdori1) * double.Parse(qty1))).ToString();
                                }
                                catch { }
                                dataGridView2.Rows.Add(d["item_id"].ToString(), item, d["item_type"].ToString(), weight, gm.removePointsZero(gm.removePoints(qty1)), unit_cost1, total_amount1, mazdori1);
                            }
                            if (total_mazdori1 != "" && total_mazdori1 != "0")
                            {
                                dataGridView2.Rows.Add("", "مزدوری", "", "", "", "", total_mazdori1, "");
                            }
                        }
                    }
                }
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
            double total_amount = 0;
            double total_weight = 0;
            double total_weight_in_mun = 0;
            try
            {
                label_Total_Amount.Text = label_Total_Weight.Text = label_Total_Weight_In_Mun.Text = "0";
                foreach (DataGridViewRow d in dataGridView2.Rows)
                {
                    try
                    {
                        total_amount += double.Parse(d.Cells[6].Value.ToString().Trim());
                    }
                    catch { }
                    try
                    {
                        if (d.Cells[2].Value.ToString() == "نارمل" || d.Cells[2].Value.ToString() == "بل میں")
                        {
                            total_weight += (double.Parse(d.Cells[3].Value.ToString().Trim()) * double.Parse(d.Cells[4].Value.ToString().Trim()));
                        }
                    }
                    catch { }
                    total_weight_in_mun = total_weight / 40;
                }
            }
            catch { }
            label_Total_Amount.Text = total_amount.ToString();
            label_Total_Weight.Text = total_weight.ToString();
            label_Total_Weight_In_Mun.Text = total_weight_in_mun.ToString();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
            }
            catch { }
        }

        private void metroRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            metroTextBox2.Hide();
            metroTextBox1.Hide();
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
            metroTile3.Visible = false;
            label5.Visible = false;
            metroTile2.Visible = false;
            //label5.Show();
            //metroTile3.Show();
            //label4.Visible = true;
            textBox4.Visible = true;
            metroRadioButton7.Checked = true;
            groupBox2.Visible = true;
            if (textBox4.Text.ToString().Trim().Length <= 0)
            {
                textBox3.Enabled = false;
            }
            if (dataGridView1.Rows.Count > 0)
                dataGridView1.Rows.Clear();
            if (dataGridView2.Rows.Count > 0)
                dataGridView2.Rows.Clear();
            if (dataGridView3.Rows.Count > 0)
                dataGridView3.Rows.Clear();
            metroTextBox2.Hide();
            metroTextBox1.Hide();
            textBox3.Hide();
            metroLabel3.Hide();
            metroLabel5.Hide();
            metroDateTimeFrom.Hide();
            metroDateTimeTo.Hide();
            if (bill_type == "Purchase Trading")
            {
                metroTextBox1.Show();
                metroTextBox1.Text = string.Empty;
                metroTextBox1.Focus();
            }
            else if (bill_type == "Sale Trading")
            {
                textBox3.Show();
                textBox3.Text = string.Empty;
                textBox3.Focus();
            }
        }

        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string[] s = metroTextBox1.Text.Trim().Split('(');
                string[] a = s[s.Length - 1].Trim().Split(')');
                string id = a[0].ToString();
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

        private void metroRadioButton4_CheckedChanged(object sender, EventArgs e)
        {
            metroTile2.Visible = false;
            label5.Hide();
            metroTile3.Hide();
            //label4.Visible = false;
            textBox4.Visible = false;
            groupBox2.Visible = false;
            if (dataGridView1.Rows.Count > 0)
                dataGridView1.Rows.Clear();
            if (dataGridView2.Rows.Count > 0)
                dataGridView2.Rows.Clear();
            if (dataGridView3.Rows.Count > 0)
                dataGridView3.Rows.Clear();
            textBox3.Hide();
            metroTextBox1.Hide();
            metroLabel3.Hide();
            metroLabel5.Hide();
            metroDateTimeFrom.Hide();
            metroDateTimeTo.Hide();
            metroTextBox2.Show();
            metroTextBox2.Text = string.Empty;
            metroTextBox2.Focus();
        }

        private void metroTextBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (metroTextBox2.Text.Trim() != "")
                {
                    getTransactionIdRecord(metroTextBox2.Text.Trim());
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void metroComboBoxUserType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            if (bill_type == "Expense")
                return;
            //try
            //{
            //    if (dataGridView1.SelectedRows.Count > 0)
            //    {
            //        //string month = dataGridView1.SelectedRows[0].Cells[2].Value.ToString().Split(' ')[0].ToString().Split('/')[0].ToString();
            //        //string day = dataGridView1.SelectedRows[0].Cells[2].Value.ToString().Split(' ')[0].ToString().Split('/')[1].ToString();
            //        //string year = dataGridView1.SelectedRows[0].Cells[2].Value.ToString().Split(' ')[0].ToString().Split('/')[2].ToString();
            //        string date = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            //        //try
            //        //{
            //        //    DateTime dtm = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells[2].Value.ToString());
            //        //    string monthName = dtm.Date.ToString("MMM", CultureInfo.InvariantCulture);
            //        //    date = dtm.Day.ToString() + "-" + dtm.Month.ToString() + "-" + dtm.Year.ToString();
            //        //    //date = dtm.Day.ToString() + " " + monthName + "," + dtm.Year.ToString();
            //        //}
            //        //catch
            //        //{}
            //        if (bill_type == "Purchase Trading")
            //        {
            //            gm.printBill(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), "Purchase Invoice", "Vendor", "Duplicate Receipt",date);
            //        }
            //        if (bill_type == "Sale Trading")
            //        {
            //            gm.printBill(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), "Sale Invoice", "Customer", "Duplicate Receipt",date);
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("select bill to print");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            try
            {
                if (textBox4.Text.Trim().Length > 0)
                {
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        //string date = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                        string date = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
                        try
                        {
                            int iMonthNo = int.Parse(DateTime.Now.Date.Month.ToString());
                            DateTime dtm = new DateTime(2000, iMonthNo, 1);
                            string monthName = dtm.Date.ToString("MMM", CultureInfo.InvariantCulture);
                            //date = DateTime.Now.Date.Year.ToString() + " " + monthName + "," + DateTime.Now.Date.Day.ToString();
                            date = DateTime.Now.Date.Day.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Year.ToString();
                        }
                        catch
                        { }
                        if (bill_type == "Sale Trading")
                        {
                            string query = "";
                            string id = "";
                            try//only word if main customer or sub sub customer name is unique
                            {
                                query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + textBox4.Text.Trim() + "' and status='1'";
                                id = gm.GetTable(query).Rows[0][0].ToString();
                            }
                            catch { }
                            if (id == "")
                            {
                                return;
                            }
                            string main_customerid = id;
                            gm.print_1Customer_All_Bills("Sale Invoice", "Customer", "Original Receipt", date, main_customerid, dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                        }
                    }
                    else
                    {
                        MessageBox.Show("Select customer to print bill");
                    }
                }
                else
                {
                    MessageBox.Show("Enter customer to print bill");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    if (bill_type == "Purchase Trading")
                    {
                        if (dataGridView3.Rows.Count > 0)
                            dataGridView3.Rows.Clear();
                        string query = "select * from bill_details where bill_id = '" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString().Trim() + "' and unit_cost!=0";
                        DataTable dt = gm.GetTable(query);
                        foreach (DataRow d in dt.Rows)
                        {
                            string item = "";
                            query = "select * from items where id = '" + d["item_id"].ToString() + "'";
                            DataTable dt2 = gm.GetTable(query);
                            try
                            {
                                item = dt2.Rows[0]["Name"].ToString();
                            }
                            catch { }
                            dataGridView3.Rows.Add(d["item_id"].ToString(), item, d["unit_cost"].ToString(), d["qty"].ToString(), d["total_amount"].ToString(), d["manufacturing_date"].ToString(), d["expiry_date"].ToString(), d["batch_or_lot_number"].ToString());
                        }
                    }
                    if (bill_type == "Sale Trading")
                    {
                        if (dataGridView2.Rows.Count > 0)
                            dataGridView2.Rows.Clear();
                        string query = "select * from bill_details where bill_id = '" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString().Trim() + "' and unit_cost!=0";
                        DataTable dt = gm.GetTable(query);
                        string total_mazdori1 = "0";
                        foreach (DataRow d in dt.Rows)
                        {
                            string item = "";
                            query = "select * from items where id = '" + d["item_id"].ToString() + "'";
                            DataTable dt2 = gm.GetTable(query);
                            try
                            {
                                item = dt2.Rows[0]["Name"].ToString();
                            }
                            catch { }
                            string weight = "";
                            query = "select * from items where id = '" + d["item_id"].ToString() + "'";
                            dt2 = gm.GetTable(query);
                            try
                            {
                                weight = dt2.Rows[0]["weight"].ToString();
                            }
                            catch { }
                            string unit_cost1 = (d["unit_cost"].ToString().Contains('.') ? (d["unit_cost"].ToString().Split('.')[1].ToString() == "000" ? d["unit_cost"].ToString().Split('.')[0].ToString() : d["unit_cost"].ToString()) : d["unit_cost"].ToString());
                            string qty1 = (d["qty"].ToString().Contains('.') ? (d["qty"].ToString().Split('.')[1].ToString() == "000" ? d["qty"].ToString().Split('.')[0].ToString() : d["qty"].ToString()) : d["qty"].ToString());
                            string total_amount1 = (d["total_amount"].ToString().Contains('.') ? (d["total_amount"].ToString().Split('.')[1].ToString() == "000" ? d["total_amount"].ToString().Split('.')[0].ToString() : d["total_amount"].ToString()) : d["total_amount"].ToString());
                            string mazdori1 = (dt2.Rows[0]["mazdori"].ToString().Contains('.') ? (dt2.Rows[0]["mazdori"].ToString().Split('.')[1].ToString() == "000" ? dt2.Rows[0]["mazdori"].ToString().Split('.')[0].ToString() : dt2.Rows[0]["mazdori"].ToString()) : dt2.Rows[0]["mazdori"].ToString());
                            try
                            {
                                total_amount1 = ((Math.Round((double.Parse(total_amount1) - double.Parse(mazdori1)), 0, MidpointRounding.AwayFromZero))).ToString();
                            }
                            catch { }
                            try
                            {
                                total_mazdori1 = (double.Parse(total_mazdori1) + double.Parse(mazdori1)).ToString();
                            }
                            catch { }
                            dataGridView2.Rows.Add(d["item_id"].ToString(), item, d["item_type"].ToString(), weight, gm.removePointsZero(gm.removePoints(qty1)), unit_cost1, total_amount1, mazdori1);
                        }
                        if (total_mazdori1 != "" && total_mazdori1 != "0")
                        {
                            dataGridView2.Rows.Add("", "مزدوری", "", "", "", "", total_mazdori1, "");
                        }
                    }
                }
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

        private void metroRadioButton2_CheckedChanged_1(object sender, EventArgs e)
        {
            metroTile3.Visible = false;
            label5.Visible = false;
            metroTile2.Visible = false;
            label5.Show();
            metroTile3.Show();
            textBox4.Visible = true;
            groupBox2.Visible = false;
            if (dataGridView1.Rows.Count > 0)
                dataGridView1.Rows.Clear();
            if (dataGridView2.Rows.Count > 0)
                dataGridView2.Rows.Clear();
            if (dataGridView3.Rows.Count > 0)
                dataGridView3.Rows.Clear();
            textBox3.Hide();
            metroTextBox2.Hide();
            metroTextBox1.Hide();
            metroLabel3.Show();
            metroLabel5.Show();
            metroDateTimeFrom.Show();
            metroDateTimeTo.Show();
            //metroDateTimeFrom.Focus();
            getDateRecord();
        }

        private void metroDateTimeFrom_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                metroDateTimeTo.MinDate = metroDateTimeFrom.Value.Date;
                getDateRecord();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroDateTimeTo_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                getDateRecord();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroLabel3_Click(object sender, EventArgs e)
        {

        }

        private void metroLabel5_Click(object sender, EventArgs e)
        {

        }

        private void ViewBills_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                    this.Dispose(true);
                if (e.Modifiers == Keys.Control && e.KeyCode == Keys.P)
                {
                    if (metroTile1.Visible.ToString() == "True")
                    {
                        metroTile1.PerformClick();
                    }
                }
                //if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
                //    metroTile2.PerformClick();
                if (e.Modifiers == Keys.Control && e.KeyCode == Keys.M)
                {
                    if (metroTile3.Visible.ToString() == "True")
                    {
                        metroTile3.PerformClick();
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void metroDateTimeFrom_ValueChanged_1(object sender, EventArgs e)
        {
            metroDateTimeFrom.MaxDate = metroDateTimeTo.Value;
            metroDateTimeTo.MinDate = metroDateTimeFrom.Value;
            try
            {
                metroDateTimeTo.MinDate = metroDateTimeFrom.Value.Date;
                getDateRecord();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroDateTimeTo_ValueChanged_1(object sender, EventArgs e)
        {
            metroDateTimeFrom.MaxDate = metroDateTimeTo.Value;
            metroDateTimeTo.MinDate = metroDateTimeFrom.Value;
            try
            {
                getDateRecord();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroTextBox3_TextChanged(object sender, EventArgs e)
        {
        }

        private void metroTextBox3_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            double total_amount = 0;
            double total_weight = 0;
            double total_weight_in_mun = 0;
            try
            {
                label_Total_Amount.Text = label_Total_Weight.Text = label_Total_Weight_In_Mun.Text = "0";
                foreach (DataGridViewRow d in dataGridView2.Rows)
                {
                    try
                    {
                        total_amount += double.Parse(d.Cells[6].Value.ToString().Trim());
                    }
                    catch { }
                    try
                    {
                        total_weight += (double.Parse(d.Cells[3].Value.ToString().Trim()) * double.Parse(d.Cells[4].Value.ToString().Trim()));
                    }
                    catch { }
                    total_weight_in_mun = total_weight / 40;
                }
            }
            catch { }
            label_Total_Amount.Text = total_amount.ToString();
            label_Total_Weight.Text = total_weight.ToString();
            label_Total_Weight_In_Mun.Text = total_weight_in_mun.ToString();
        }

        private void dataGridView2_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            double total_amount = 0;
            double total_weight = 0;
            double total_weight_in_mun = 0;
            try
            {
                label_Total_Amount.Text = label_Total_Weight.Text = label_Total_Weight_In_Mun.Text = "0";
                foreach (DataGridViewRow d in dataGridView2.Rows)
                {
                    try
                    {
                        total_amount += double.Parse(d.Cells[6].Value.ToString().Trim());
                    }
                    catch { }
                    try
                    {
                        total_weight += (double.Parse(d.Cells[3].Value.ToString().Trim()) * double.Parse(d.Cells[4].Value.ToString().Trim()));
                    }
                    catch { }
                    total_weight_in_mun = total_weight / 40;
                }
            }
            catch { }
            label_Total_Amount.Text = total_amount.ToString();
            label_Total_Weight.Text = total_weight.ToString();
            label_Total_Weight_In_Mun.Text = total_weight_in_mun.ToString();
        }

        private void dataGridView2_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            double total_amount = 0;
            double total_weight = 0;
            double total_weight_in_mun = 0;
            try
            {
                label_Total_Amount.Text = label_Total_Weight.Text = label_Total_Weight_In_Mun.Text = "0";
                foreach (DataGridViewRow d in dataGridView2.Rows)
                {
                    try
                    {
                        total_amount += double.Parse(d.Cells[5].Value.ToString().Trim());
                    }
                    catch { }
                    try
                    {
                        total_weight += (double.Parse(d.Cells[3].Value.ToString().Trim()) * double.Parse(d.Cells[4].Value.ToString().Trim()));
                    }
                    catch { }
                    total_weight_in_mun = total_weight / 40;
                }
            }
            catch { }
            label_Total_Amount.Text = total_amount.ToString();
            label_Total_Weight.Text = total_weight.ToString();
            label_Total_Weight_In_Mun.Text = total_weight_in_mun.ToString();
        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            if (bill_type == "Expense")
                return;
            try
            {
                if (textBox4.Text.Trim().Length > 0)
                {
                    //string date = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                    string date = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
                    try
                    {
                        int iMonthNo = int.Parse(DateTime.Now.Date.Month.ToString());
                        DateTime dtm = new DateTime(2000, iMonthNo, 1);
                        string monthName = dtm.Date.ToString("MMM", CultureInfo.InvariantCulture);
                        //date = DateTime.Now.Date.Year.ToString() + " " + monthName + "," + DateTime.Now.Date.Day.ToString();
                        date = DateTime.Now.Date.Day.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Year.ToString();
                    }
                    catch
                    { }
                    if (bill_type == "Sale Trading")
                    {
                        string query = "";
                        string id = "";
                        try//only word if main customer or sub sub customer name is unique
                        {
                            query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + textBox4.Text.Trim() + "' and status='1'";
                            id = gm.GetTable(query).Rows[0][0].ToString();
                        }
                        catch { }
                        if (id == "")
                        {
                            return;
                        }
                        string main_customerid = id;
                        gm.print_Customer_All_Bills("Sale Invoice", "Customer", "Original Receipt", date, main_customerid);
                    }
                }
                else
                {
                    MessageBox.Show("Enter customer to print bill");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroTile3_Click(object sender, EventArgs e)
        {
            if (bill_type == "Expense")
                return;
            try
            {
                if (textBox4.Text.Trim().Length > 0)
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                        if (metroRadioButton2.Checked == true)
                        {
                            TimeSpan ts = metroDateTimeTo.Value.Date - metroDateTimeFrom.Value.Date;
                            int Days = ts.Days;
                            if (Days > 0)
                            {
                                MessageBox.Show("Print Only One Day Record");
                                return;
                            }
                        }
                        MainCustomerBillReportViewer sc = new MainCustomerBillReportViewer();
                        sc.Bill_Type = "Main Customer Bill";
                        //sc.Main_Customer_Or_Vendor = metroTextBox4.Text.ToString();
                        string query = "";
                        string id = "";
                        try//only word if main customer or sub sub customer name is unique
                        {
                            query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + textBox4.Text.Trim() + "' and status='1'";
                            id = gm.GetTable(query).Rows[0][0].ToString();
                        }
                        catch { }
                        if (id == "")
                        {
                            return;
                        }
                        sc.Main_Customer_Or_Vendor = textBox4.Text.Trim();
                        string date = dataGridView1.Rows[0].Cells[2].Value.ToString();
                        sc.Transaction_Date = date;


                        DataTable dt = new DataTable();
                        dt.Columns.Add("Sr_No");
                        dt.Columns.Add("Customer_Name");
                        dt.Columns.Add("Total_Weight");
                        dt.Columns.Add("Total_Amount");

                        string customer_name = "";
                        string overall_total_weight = "0";
                        string total_weight = "0";
                        string overall_total_amount = "0";
                        DataTable dt1 = new DataTable();
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            total_weight = "0";
                            //query = "select customer_Or_Vendor_Name + '('+id+')' from Customer_Or_Vendor where id in (select Customer_Vendor_Id from bill where status!='-1' and id='" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "')";
                            query = "select customer_Or_Vendor_Name from Customer_Or_Vendor where id in (select Customer_Vendor_Id from bill where status!='-1' and id='" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "')";
                            dt1 = gm.GetTable(query);
                            customer_name = dt1.Rows[0][0].ToString();

                            query = "select * from Bill_Details where Bill_Id='" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "' and unit_cost!=0";
                            dt1 = gm.GetTable(query);
                            foreach (DataRow d2 in dt1.Rows)
                            {
                                query = "select weight from items where id='" + d2["item_id"].ToString() + "'";
                                DataTable dt2 = gm.GetTable(query);
                                try
                                {
                                    if (d2["item_type"].ToString() == "نارمل" || d2["item_type"].ToString() == "بل میں")
                                    {
                                        total_weight = (double.Parse(total_weight) + (double.Parse(dt2.Rows[0][0].ToString()) * double.Parse(d2["Qty"].ToString().Trim()))).ToString();
                                    }
                                }
                                catch { }
                            }
                            try
                            {
                                overall_total_weight = (double.Parse(overall_total_weight.Trim()) + double.Parse(total_weight.Trim())).ToString();
                            }
                            catch { }
                            try
                            {
                                overall_total_amount = (double.Parse(overall_total_amount.Trim()) + double.Parse(dataGridView1.Rows[i].Cells[7].Value.ToString().Trim())).ToString();
                            }
                            catch { }
                            if (total_weight.Contains('.'))
                            {
                                total_weight = (total_weight.Split('.')[1].ToString() != "000" ? total_weight : total_weight.Split('.')[0].ToString());
                            }
                            string total_amount = dataGridView1.Rows[i].Cells[7].Value.ToString();
                            if (total_amount.Contains('.'))
                            {
                                total_amount = (total_amount.Split('.')[1].ToString() != "000" ? total_amount : total_amount.Split('.')[0].ToString());
                            }
                            dt.Rows.Add((i + 1).ToString(), customer_name, total_weight, total_amount);
                        }
                        sc.TotalWeight = total_weight;
                        sc.Overall_Total_Weight = overall_total_weight;
                        if (overall_total_weight.Contains('.'))
                        {
                            sc.Overall_Total_Weight = (overall_total_weight.Split('.')[1].ToString() != "000" ? overall_total_weight : overall_total_weight.Split('.')[0].ToString());
                        }
                        sc.Overall_Total_Amount = overall_total_amount;
                        if (overall_total_amount.Contains('.'))
                        {
                            sc.Overall_Total_Amount = (overall_total_amount.Split('.')[1].ToString() != "000" ? overall_total_amount : overall_total_amount.Split('.')[0].ToString());
                        }

                        string mun = "0";
                        try
                        {
                            //mun = (double.Parse(total_weight) / 40).ToString();
                            mun = (double.Parse(overall_total_weight) / 40).ToString();
                            if (mun.Contains('.'))
                            {
                                mun = (mun.Split('.')[1].ToString() != "000" ? mun : mun.Split('.')[0].ToString());
                            }
                        }
                        catch { }
                        //dt.Rows.Add("", (double.Parse(overall_total_weight) / 40).ToString() + "   من", overall_total_weight.ToString() + "  کل وزن", overall_total_amount.ToString() + "   کل رقم");
                        dt.Rows.Add("", mun + "   من", overall_total_weight.ToString() + "  کل وزن", overall_total_amount.ToString() + "   کل رقم");
                        DataTable dt_1 = new DataTable();
                        dt_1.Columns.Add("Sr_No");
                        dt_1.Columns.Add("Customer_Name");
                        dt_1.Columns.Add("Total_Weight");
                        dt_1.Columns.Add("Total_Amount");

                        DataTable dt_2 = new DataTable();
                        dt_2.Columns.Add("Sr_No");
                        dt_2.Columns.Add("Customer_Name");
                        dt_2.Columns.Add("Total_Weight");
                        dt_2.Columns.Add("Total_Amount");

                        int pageheight = 32;
                        int index = 0;
                        int dtf1 = 0;
                        int dtf2 = 0;
                        int length = dt.Rows.Count;
                        while (length != 0)
                        {
                            int remaining = length - pageheight;
                            if (remaining > 0)
                            {
                                if (remaining >= pageheight)
                                {
                                    if (dtf1 == 0)
                                    {
                                        for (int i = index; i < (index + pageheight); i++)
                                        {
                                            dt_1.Rows.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), dt.Rows[i][3].ToString());
                                        }
                                        index += pageheight;
                                        dtf1 = 1;
                                        dtf2 = 0;

                                    }
                                    else
                                    {
                                        for (int i = index; i < (index + pageheight); i++)
                                        {
                                            dt_2.Rows.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), dt.Rows[i][3].ToString());
                                        }
                                        index += pageheight;
                                        dtf1 = 0;
                                        dtf2 = 1;
                                    }
                                }
                                else
                                {
                                    if (dtf1 == 0)
                                    {
                                        for (int i = index; i < (index + pageheight); i++)
                                        {
                                            dt_1.Rows.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), dt.Rows[i][3].ToString());
                                        }
                                        index += pageheight;
                                        dtf1 = 1;
                                        dtf2 = 0;
                                    }
                                    else
                                    {
                                        for (int i = index; i < (index + pageheight); i++)
                                        {
                                            dt_2.Rows.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), dt.Rows[i][3].ToString());
                                        }
                                        //dt_1.Rows.Add("", "", "", "");
                                        //dt_1.Rows.Add("", "", "", "");
                                        //dt_2.Rows.Add("", "", "", "");
                                        //dt_2.Rows.Add("", "", "", "");
                                        index += (pageheight);
                                        dtf1 = 0;
                                        dtf2 = 1;
                                    }
                                }
                                length -= (pageheight);
                            }
                            else
                            {
                                for (int i = index; i < (index + length); i++)
                                {
                                    if (dtf1 == 0)
                                    {
                                        dt_1.Rows.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), dt.Rows[i][3].ToString());
                                    }
                                    else
                                    {
                                        dt_2.Rows.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), dt.Rows[i][3].ToString());
                                    }
                                }
                                index += length;
                                break;
                            }
                        }









                        sc.dt = dt_1;
                        sc.dt2 = dt_2;



                        sc.Show();
                    }
                    else
                    {
                        MessageBox.Show("No record found to print bill");
                    }
                }
                else
                {
                    MessageBox.Show("Enter customer to print bill");
                }
            }
            catch { }

        }

        private void metroDateTime2_ValueChanged(object sender, EventArgs e)
        {
            metroDateTime2.MaxDate = metroDateTime1.Value;
            metroDateTime1.MinDate = metroDateTime2.Value;
            try
            {
                string query = "";
                string id = "";
                try//only word if main customer or sub sub customer name is unique
                {
                    query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + textBox3.Text.Trim() + "' and status='1'";
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
                //if (metroTextBox3.Text.Trim() == "")
                //    getAllRecord();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroDateTime1_ValueChanged(object sender, EventArgs e)
        {
            metroDateTime2.MaxDate = metroDateTime1.Value;
            metroDateTime1.MinDate = metroDateTime2.Value;
            try
            {
                string query = "";
                string id = "";
                try//only word if main customer or sub sub customer name is unique
                {
                    query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + textBox3.Text.Trim() + "' and status='1'";
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
                //if (metroTextBox3.Text.Trim() == "")
                //    getAllRecord();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroTextBox4_Leave(object sender, EventArgs e)
        {
        }

        private void metroRadioButton8_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Show();
            //metroDateTime1.Value = metroDateTime2.Value = DateTime.Now;
            try
            {
                string query = "";
                string id = "";
                try//only word if main customer or sub sub customer name is unique
                {
                    query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + textBox3.Text.Trim() + "' and status='1'";
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
                //if (metroTextBox3.Text.Trim() == "")
                //    getAllRecord();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroRadioButton7_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Hide();
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

        }

        private void metroLabel7_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (metroRadioButton3.Checked == true)
            {
                textBox3.Focus();
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                textBox3.Enabled = false;
                textBox3.Text = string.Empty;
                //metroTextBoxVendorContactNumber.Text = string.Empty;
                //metroTextBoxVendorAddress.Text = string.Empty;
                if (textBox4.Text.Trim().Length > 0)
                {
                    string query = "";
                    string id = "";
                    try//only word if main customer or sub sub customer name is unique
                    {
                        query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + textBox4.Text.Trim() + "' and status='1'";
                        id = gm.GetTable(query).Rows[0][0].ToString();
                    }
                    catch { }
                    if (id == "")
                    {
                        return;
                    }
                    string customerid = id;
                    sourcecustomername = new AutoCompleteStringCollection();
                    string customervendortype = "Customer";
                    DataTable dt = gm.GetTable("select * from customer_or_vendor where customer_vendor_type='" + customervendortype + "' and status ='1' and parent!='' and parent='" + customerid + "'");
                    foreach (DataRow d in dt.Rows)
                    {
                        sourcecustomername.Add(d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")");
                    }

                    textBox3.DisplayMember = "customer_Or_Vendor_Name";
                    textBox3.ValueMember = "id";
                    textBox3.DataSource = dt;
                    textBox3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    textBox3.AutoCompleteSource = AutoCompleteSource.ListItems;
                    textBox3.SelectedIndex = -1;

                    if (metroRadioButton1.Checked == true)
                    {
                        metroRadioButton1.Checked = false;
                        metroRadioButton1.PerformClick();
                    }
                    else if (metroRadioButton2.Checked == true)
                    {
                        metroRadioButton2.Checked = false;
                        metroRadioButton2.PerformClick();
                    }
                    else if (metroRadioButton3.Checked == true)
                    {
                        metroRadioButton3.Checked = false;
                        metroRadioButton3.PerformClick();
                    }
                    else if (metroRadioButton4.Checked == true)
                    {
                        metroRadioButton4.Checked = false;
                        metroRadioButton4.PerformClick();
                    }
                    textBox3.Enabled = true;
                }
            }
            catch { }
            if (metroRadioButton1.Checked == true)
            {
                getAllRecord();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.Clear();
            }
            try
            {
                string query = "";
                string id = "";
                try//only word if main customer or sub sub customer name is unique
                {
                    query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + textBox3.Text.Trim() + "' and status='1'";
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
                //if (metroTextBox3.Text.Trim() == "")
                //    getAllRecord();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
    }
}
