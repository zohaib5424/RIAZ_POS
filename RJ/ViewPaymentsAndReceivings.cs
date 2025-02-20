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
    public partial class ViewPaymentsAndReceivings : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public ViewPaymentsAndReceivings()
        {
            InitializeComponent();
        }

        string bill_type = "";
        public ViewPaymentsAndReceivings(string form)
        {
            InitializeComponent();
            if (form == "payment")
            {

                metroTextBox2.PromptText = "Enter Transaction #";
                metroRadioButton4.Text = "By Transaction #";
                //dataGridView1.Columns[1].HeaderText= "Transaction Id";
                metroRadioButton3.Show();
                metroLabel4.Text = "Payments";
                bill_type = "Payment";
                metroTile1.Show();
                label1.Show();
            }
            else if (form == "receiving")
            {
                metroTextBox2.PromptText = "Enter Transaction #";
                metroRadioButton4.Text = "By Transaction #";
                //dataGridView1.Columns[1].HeaderText = "Transaction Id";
                metroRadioButton3.Show();
                metroLabel4.Text = "Receivings";
                bill_type = "Receiving";
                metroTile1.Show();
                label1.Show();
            }
        }
        bool isFirst = false;
        AutoCompleteStringCollection sourcetransactionid = new AutoCompleteStringCollection();
        AutoCompleteStringCollection sourcecustomername = new AutoCompleteStringCollection();
        AutoCompleteStringCollection sourcevendorname = new AutoCompleteStringCollection();
        private void ItemsList_Load(object sender, EventArgs e)
        {
            metroTile3.Hide();
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            metroRadioButton4.Visible = false;
            dataGridView1.Columns[11].DisplayIndex = 5;
            dataGridView1.Columns[12].DisplayIndex = 6;
            dataGridView1.Columns[13].DisplayIndex = 7;
            dataGridView1.Columns[5].Visible =false;
            //try
            //{
            //    metroDateTimeFrom.Value = metroDateTimeTo.Value = DateTime.Now;
            //    metroDateTimeFrom.MaxDate = metroDateTimeTo.Value;
            //    metroDateTimeTo.MinDate = metroDateTimeFrom.Value;
            //}
            //catch { }
            //try
            //{
            //    metroDateTime2.Value = metroDateTime1.Value = DateTime.Now;
            //    metroDateTime2.MaxDate = metroDateTime1.Value;
            //    metroDateTime1.MinDate = metroDateTime2.Value;
            //}
            //catch { }
            try
            {
                metroRadioButton5.Visible = false;
                metroRadioButton6.Visible = false;
                metroTile1.Hide();
                label1.Hide();
                metroLabel12.Hide();// = "Vendor Information";
                //metroLabelNameH.Hide();//Text = "Vendor Name : ";
                labelName.Hide();//Text = dt.Rows[0]["customer_Or_Vendor_Name"].ToString();
                //metroLabelContactNumberH.Hide();//Text = "Vendor Name : ";
                labelContactNumber.Hide();//Text = dt.Rows[0]["Contact_Number"].ToString(); ;
                //metroLabelAddressH.Hide();//Text = "Vendor Name : ";
                labelAddress.Hide();//Text = dt.Rows[0]["_Address"].ToString(); ;
                label5.Hide();
                label7.Hide();
                label8.Hide();

                sourcetransactionid = new AutoCompleteStringCollection();
                sourcevendorname = new AutoCompleteStringCollection();
                sourcevendorname = new AutoCompleteStringCollection();
                comboBox_MainCustomer.Hide();

                isFirst = true;
                this.AutoScroll = true;

                //string customervendortype = "Vendor";
                DataTable dt = gm.GetTable("select * from customer_or_vendor where status ='1' and (parent='' or parent='NULL')");
                foreach (DataRow d in dt.Rows)
                {
                    sourcevendorname.Add(d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")");
                }

                comboBox_MainCustomer.DisplayMember = "customer_Or_Vendor_Name";
                comboBox_MainCustomer.ValueMember = "id";
                comboBox_MainCustomer.DataSource = dt;
                comboBox_MainCustomer.AutoCompleteMode = AutoCompleteMode.Suggest;
                comboBox_MainCustomer.AutoCompleteSource = AutoCompleteSource.ListItems;
                comboBox_MainCustomer.SelectedIndex = -1;

                dt = gm.GetTable("select * from bill where bill_type='" + bill_type + "' and status ='1' order by Bill_Date DESC");
                foreach (DataRow d in dt.Rows)
                {
                    sourcetransactionid.Add(d["Bill_Id"].ToString());
                }

                metroTextBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                metroTextBox2.AutoCompleteCustomSource = sourcetransactionid;
                metroTextBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;

                metroDateTimeFrom.Value = metroDateTimeTo.Value = DateTime.Now;
                metroDateTimeTo.MinDate = metroDateTimeFrom.Value.Date;
                metroLabel3.Hide();
                metroLabel5.Hide();

                metroRadioButton1.Checked = true;
                label2.Text = "Ctrl + P";

                isFirst = false;

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
            }
            catch { }
        }

        GMDB gm = new GMDB();
        public void getAllRecord()
        {
            try
            {
                string todaydate = DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Month.ToString().Length == 1 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString()) + "-" + (DateTime.Now.Day.ToString().Length == 1 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString());
                string query = "Select * from bill where bill_type='" + bill_type + "' and status != '-1' and bill_date='" + todaydate + "' order by Bill_Date DESC";
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
                        addedby = dtlogins.Rows[0][1].ToString() +" ("+dtlogins.Rows[0][0].ToString()+")";
                    }
                    double paymentduebefore = 0;
                    //try
                    //{
                    //    paymentduebefore -= double.Parse(d["Total_Amount"].ToString().Trim());
                    //}
                    //catch { }
                    try
                    {
                        paymentduebefore = double.Parse(d["Total_Amount"].ToString().Trim());
                    }
                    catch { }
                    double paymentdueafter = 0;
                    string customerorvendor = "";
                    string customerorvendorid = "";
                    try
                    {
                        query = "select * from customer_or_vendor where id=N'" + d["customer_vendor_id"].ToString() + "' and status!='-1'";
                        DataTable dt2 = gm.GetTable(query);
                        if (dt2.Rows.Count > 0)
                        {
                            customerorvendor = dt2.Rows[0]["customer_or_vendor_name"].ToString();
                            customerorvendorid = dt2.Rows[0]["id"].ToString();
                        }
                    }
                    catch { }
                    try
                    {
                        if (d["bill_type"].ToString().Trim() == "Payment")
                        {
                            paymentdueafter = paymentduebefore + (double.Parse(d["paid_amount"].ToString().Trim()) + double.Parse(d["service_charges"].ToString().Trim()));
                        }
                        else if (d["bill_type"].ToString().Trim() == "Receiving")
                        {
                            //MessageBox.Show(double.Parse(getCustomerTodayBill(dataGridView1.SelectedRows[0].Cells[10].Value.ToString().Trim())).ToString());
                            paymentdueafter = (paymentduebefore - (double.Parse(d["paid_amount"].ToString().Trim()) + double.Parse(d["service_charges"].ToString().Trim())));
                        }
                    }
                    catch (Exception ex) { 
                        //MessageBox.Show(ex.Message); 
                    }
                    string bill_date2 = d["bill_date"].ToString();
                    if (bill_date2.Contains(' '))
                    {
                        bill_date2 = bill_date2.Split(' ')[0].ToString().Trim();
                    }
                    //dataGridView1.Rows.Add(d["id"].ToString(), d["bill_id"].ToString(), customerorvendor, bill_date2, d["bill_time"].ToString(), (double.Parse((d["paid_amount"].ToString().Trim()!=""?d["paid_amount"].ToString().Trim():"0") + double.Parse(d["service_charges"].ToString().Trim()!=""?d["service_charges"].ToString().Trim():"0")).ToString().ToString()), paymentduebefore.ToString(), paymentdueafter.ToString(), d["payment_method"].ToString(), addedby, customerorvendorid, getCustomerTodayBill(customerorvendorid, d["bill_date"].ToString()), d["paid_amount"].ToString(), d["service_charges"].ToString());
                    string paid_amount = "0";
                    try
                    {
                        paid_amount = gm.removePoints((double.Parse(d["paid_amount"].ToString().Trim()) + double.Parse(d["service_charges"].ToString().Trim())).ToString());
                    }
                    catch { }
                    dataGridView1.Rows.Add(d["id"].ToString(), d["bill_id"].ToString(), customerorvendor, bill_date2, d["bill_time"].ToString(), paid_amount, paymentduebefore.ToString(), paymentdueafter.ToString(), d["payment_method"].ToString(), addedby, customerorvendorid, getCustomerTodayBill(customerorvendorid, d["bill_date"].ToString()), d["paid_amount"].ToString(), d["service_charges"].ToString());
                    if (d["status"].ToString() == "0")
                    {
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                    }
                    try
                    {
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[11].Value = gm.removePoints(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[11].Value.ToString());
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[12].Value = gm.removePoints(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[12].Value.ToString());
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[13].Value = gm.removePoints(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[13].Value.ToString());
                    }
                    catch { }
                }
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            label3.Text = "0";
            label11.Text = "0";
            label6.Text = "0";
            double total_rec = 0;
            double total_bill_ki_raqam = 0;
            double total_com = 0;
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    foreach (DataGridViewRow d in dataGridView1.Rows)
                    {
                        try
                        {
                            total_rec += double.Parse(d.Cells[12].Value.ToString());
                        }
                        catch { }
                        try
                        {
                            total_bill_ki_raqam += double.Parse(d.Cells[11].Value.ToString());
                        }
                        catch { }
                        try
                        {
                            total_com += double.Parse(d.Cells[13].Value.ToString());
                        }
                        catch { }
                    }
                }
            }
            catch { }
            label3.Text = total_rec.ToString();
            label11.Text = total_rec.ToString();
            label6.Text = total_com.ToString();
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
            dataGridView1.Sort(dataGridView1.Columns[3], System.ComponentModel.ListSortDirection.Descending);
            foreach (DataGridViewRow d in dataGridView1.Rows)
            {
                DateTime dtm = Convert.ToDateTime(d.Cells[3].Value.ToString());
                d.Cells[3].Value = (dtm.Date.Day.ToString().Length == 1 ? ("0" + dtm.Date.Day.ToString()) : dtm.Date.Day.ToString()) + "-" + (dtm.Date.Month.ToString().Length == 1 ? ("0" + dtm.Date.Month.ToString()) : dtm.Date.Month.ToString()) + "-" + dtm.Date.Year.ToString();
            }
        }

        public void getAllVendorRecord()
        {
            try
            {
                string query = "Select * from bill where bill_type='" + bill_type + "' and status != '-1' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where status='1' and Customer_Vendor_Type='Vendor') order by Bill_Date DESC";
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
                        addedby = dtlogins.Rows[0][1].ToString() +" ("+dtlogins.Rows[0][0].ToString()+")";
                    }
                    double paymentduebefore = 0;
                    try
                    {
                        paymentduebefore -= double.Parse(d["Total_Amount"].ToString().Trim());
                    }
                    catch { }
                    double paymentdueafter = 0;
                    try
                    {
                        if (d["bill_type"].ToString().Trim() == "Payment")
                        {
                            paymentdueafter = paymentduebefore - double.Parse(d["paid_amount"].ToString().Trim());
                        }
                        else if (d["bill_type"].ToString().Trim() == "Receiving")
                        {
                            paymentdueafter = paymentduebefore + double.Parse(d["paid_amount"].ToString().Trim());
                        }
                    }catch{}
                    string customerorvendor = "";
                    string customerorvendorid = "";
                    try
                    {
                        query = "select * from customer_or_vendor where id=N'"+d["customer_vendor_id"].ToString()+"' and status!='-1'";
                        DataTable dt2 = gm.GetTable(query);
                        if (dt2.Rows.Count > 0)
                        {
                            customerorvendor = dt2.Rows[0]["customer_or_vendor_name"].ToString();
                            customerorvendorid = dt2.Rows[0]["id"].ToString();
                        }
                    }
                    catch { }
                    string bill_date2 = d["bill_date"].ToString();
                    if (bill_date2.Contains(' '))
                    {
                        bill_date2 = bill_date2.Split(' ')[0].ToString().Trim();
                    }
                    dataGridView1.Rows.Add(d["id"].ToString(), d["bill_id"].ToString(), customerorvendor, bill_date2, d["bill_time"].ToString(), d["paid_amount"].ToString(), paymentduebefore.ToString(), paymentdueafter, d["payment_method"].ToString(), addedby, customerorvendorid, getCustomerTodayBill(customerorvendorid, d["bill_date"].ToString()), d["paid_amount"].ToString(), d["service_charges"].ToString());
                    if (d["status"].ToString() == "0")
                    {
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                    }
                    try
                    {
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[11].Value = gm.removePoints(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[11].Value.ToString());
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[12].Value = gm.removePoints(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[12].Value.ToString());
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[13].Value = gm.removePoints(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[13].Value.ToString());
                    }
                    catch { }
                }
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            label3.Text = "0";
            label11.Text = "0";
            label6.Text = "0";
            double total_rec = 0;
            double total_bill_ki_raqam = 0;
            double total_com = 0;
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    foreach (DataGridViewRow d in dataGridView1.Rows)
                    {
                        try
                        {
                            total_rec += double.Parse(d.Cells[12].Value.ToString());
                        }
                        catch { }
                        try
                        {
                            total_bill_ki_raqam += double.Parse(d.Cells[11].Value.ToString());
                        }
                        catch { }
                        try
                        {
                            total_com += double.Parse(d.Cells[13].Value.ToString());
                        }
                        catch { }
                    }
                }
            }
            catch { }
            label3.Text = total_rec.ToString();
            label11.Text = total_bill_ki_raqam.ToString();
            label6.Text = total_com.ToString();
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

        public void getAllCustomerRecord()
        {
            try
            {
                string query = "Select * from bill where bill_type='" + bill_type + "' and status != '-1' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where status='1' and Customer_Vendor_Type='Customer') order by Bill_Date DESC";
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
                        addedby = dtlogins.Rows[0][1].ToString() + " (" + dtlogins.Rows[0][0].ToString() + ")";
                    }
                    double paymentduebefore = 0;
                    try
                    {
                        paymentduebefore -= double.Parse(d["Total_Amount"].ToString().Trim());
                    }
                    catch { }
                    double paymentdueafter = 0;
                    try
                    {
                        if (d["bill_type"].ToString().Trim() == "Payment")
                        {
                            paymentdueafter = paymentduebefore - double.Parse(d["paid_amount"].ToString().Trim());
                        }
                        else if (d["bill_type"].ToString().Trim() == "Receiving")
                        {
                            paymentdueafter = paymentduebefore + double.Parse(d["paid_amount"].ToString().Trim());
                        }
                    }
                    catch { }
                    string customerorvendor = "";
                    string customerorvendorid = "";
                    try
                    {
                        query = "select * from customer_or_vendor where id=N'" + d["customer_vendor_id"].ToString() + "' and status!='-1'";
                        DataTable dt2 = gm.GetTable(query);
                        if (dt2.Rows.Count > 0)
                        {
                            customerorvendor = dt2.Rows[0]["customer_or_vendor_name"].ToString();
                            customerorvendorid = dt2.Rows[0]["id"].ToString();
                        }
                    }
                    catch { }
                    string bill_date2 = d["bill_date"].ToString();
                    if (bill_date2.Contains(' '))
                    {
                        bill_date2 = bill_date2.Split(' ')[0].ToString().Trim();
                    }
                    dataGridView1.Rows.Add(d["id"].ToString(), d["bill_id"].ToString(), customerorvendor, bill_date2, d["bill_time"].ToString(), d["paid_amount"].ToString(), paymentduebefore.ToString(), paymentdueafter, d["payment_method"].ToString(), addedby, customerorvendorid, getCustomerTodayBill(customerorvendorid, d["bill_date"].ToString()), d["paid_amount"].ToString(), d["service_charges"].ToString());
                    if (d["status"].ToString() == "0")
                    {
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                    }
                    try
                    {
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[11].Value = gm.removePoints(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[11].Value.ToString());
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[12].Value = gm.removePoints(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[12].Value.ToString());
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[13].Value = gm.removePoints(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[13].Value.ToString());
                    }
                    catch { }
                }
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            label3.Text = "0";
            label11.Text = "0";
            label6.Text = "0";
            double total_rec = 0;
            double total_bill_ki_raqam = 0;
            double total_com = 0;
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    foreach (DataGridViewRow d in dataGridView1.Rows)
                    {
                        try
                        {
                            total_rec += double.Parse(d.Cells[12].Value.ToString());
                        }
                        catch { }
                        try
                        {
                            total_bill_ki_raqam += double.Parse(d.Cells[11].Value.ToString());
                        }
                        catch { }
                        try
                        {
                            total_com += double.Parse(d.Cells[13].Value.ToString());
                        }
                        catch { }
                    }
                }
            }
            catch { }
            label3.Text = total_rec.ToString();
            label11.Text = total_bill_ki_raqam.ToString();
            label6.Text = total_com.ToString();
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

        public void getDateRecord()
        {
            try
            {
                string from = metroDateTimeFrom.Value.Date.Year.ToString() +"-"+metroDateTimeFrom.Value.Date.Month.ToString() +"-"+metroDateTimeFrom.Value.Date.Day.ToString();
                string to = metroDateTimeTo.Value.Date.Year.ToString() +"-"+metroDateTimeTo.Value.Date.Month.ToString() +"-"+metroDateTimeTo.Value.Date.Day.ToString();
                string query = "Select * from bill where bill_type='" + bill_type + "' and bill_date >= '" + from + "' and bill_date<='" + to + "' and status != '-1' order by Bill_Date DESC";
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
                        addedby = dtlogins.Rows[0][1].ToString() + " (" + dtlogins.Rows[0][0].ToString() + ")";
                    }
                    double paymentduebefore = 0;
                    //try
                    //{
                    //    paymentduebefore -= double.Parse(d["Total_Amount"].ToString().Trim());
                    //}
                    //catch { }
                    try
                    {
                        paymentduebefore = double.Parse(d["Total_Amount"].ToString().Trim());
                    }
                    catch { }
                    double paymentdueafter = 0;
                    string customerorvendor = "";
                    string customerorvendorid = "";
                    try
                    {
                        query = "select * from customer_or_vendor where id=N'" + d["customer_vendor_id"].ToString() + "' and status!='-1'";
                        DataTable dt2 = gm.GetTable(query);
                        if (dt2.Rows.Count > 0)
                        {
                            customerorvendor = dt2.Rows[0]["customer_or_vendor_name"].ToString();
                            customerorvendorid = dt2.Rows[0]["id"].ToString();
                        }
                    }
                    catch { }
                    try
                    {
                        if (d["bill_type"].ToString().Trim() == "Payment")
                        {
                            paymentdueafter = paymentduebefore + (double.Parse(d["paid_amount"].ToString().Trim()) + double.Parse(d["service_charges"].ToString().Trim()));
                        }
                        else if (d["bill_type"].ToString().Trim() == "Receiving")
                        {
                            paymentdueafter = (paymentduebefore + double.Parse(getCustomerTodayBill(customerorvendorid, d["bill_date"].ToString()))) - (double.Parse(d["paid_amount"].ToString().Trim()) + double.Parse(d["service_charges"].ToString().Trim()));
                        }
                    }
                    catch { }
                    string bill_date2 = d["bill_date"].ToString();
                    if (bill_date2.Contains(' '))
                    {
                        bill_date2 = bill_date2.Split(' ')[0].ToString().Trim();
                    }
                    dataGridView1.Rows.Add(d["id"].ToString(), d["bill_id"].ToString(), customerorvendor, bill_date2, d["bill_time"].ToString(), gm.removePoints((double.Parse(d["paid_amount"].ToString().Trim()) + double.Parse(d["service_charges"].ToString().Trim())).ToString()), gm.removePoints(paymentduebefore.ToString()), gm.removePoints(paymentdueafter.ToString()), gm.removePoints(d["payment_method"].ToString()), addedby, customerorvendorid, getCustomerTodayBill(customerorvendorid, d["bill_date"].ToString()), d["paid_amount"].ToString(), d["service_charges"].ToString());
                    if (d["status"].ToString() == "0")
                    {
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                    }
                    try
                    {
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[11].Value = gm.removePoints(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[11].Value.ToString());
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[12].Value = gm.removePoints(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[12].Value.ToString());
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[13].Value = gm.removePoints(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[13].Value.ToString());
                    }
                    catch { }
                }
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            label3.Text = "0";
            label11.Text = "0";
            label6.Text = "0";
            double total_rec = 0;
            double total_bill_ki_raqam = 0;
            double total_com = 0;
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    foreach (DataGridViewRow d in dataGridView1.Rows)
                    {
                        try
                        {
                            total_rec += double.Parse(d.Cells[12].Value.ToString());
                        }
                        catch { }
                        try
                        {
                            total_bill_ki_raqam += double.Parse(d.Cells[11].Value.ToString());
                        }
                        catch { }
                        try
                        {
                            total_com += double.Parse(d.Cells[13].Value.ToString());
                        }
                        catch { }
                    }
                }
            }
            catch { }
            label3.Text = total_rec.ToString();
            label11.Text = total_bill_ki_raqam.ToString();
            label6.Text = total_com.ToString();
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
            //dataGridView1.Sort(dataGridView1.Columns[3], System.ComponentModel.ListSortDirection.Descending);
            foreach (DataGridViewRow d in dataGridView1.Rows)
            {
                DateTime dtm = Convert.ToDateTime(d.Cells[3].Value.ToString());
                d.Cells[3].Value = (dtm.Date.Day.ToString().Length == 1 ? ("0" + dtm.Date.Day.ToString()) : dtm.Date.Day.ToString()) + "-" + (dtm.Date.Month.ToString().Length == 1 ? ("0" + dtm.Date.Month.ToString()) : dtm.Date.Month.ToString()) + "-" + dtm.Date.Year.ToString();
            }
        }

        public void getUserNameRecord(string id)
        {
            try
            {
                string query = "Select * from bill where bill_type=N'" + bill_type + "' and customer_vendor_id='" + id + "' and status != '-1' order by Bill_Date DESC";
                if (metroRadioButton8.Checked == true)//between dates
                {
                    string from = metroDateTime2.Value.Date.Year.ToString() + "-" + metroDateTime2.Value.Date.Month.ToString() + "-" + metroDateTime2.Value.Date.Day.ToString();
                    string to = metroDateTime1.Value.Date.Year.ToString() + "-" + metroDateTime1.Value.Date.Month.ToString() + "-" + metroDateTime1.Value.Date.Day.ToString();
                    query = "Select * from bill where bill_type='" + bill_type + "' and bill_date >= '" + from + "' and bill_date<='" + to + "' and customer_vendor_id='" + id + "' and status != '-1' order by Bill_Date DESC";
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
                        addedby = dtlogins.Rows[0][1].ToString() + " (" + dtlogins.Rows[0][0].ToString() + ")";
                    }
                    double paymentduebefore = 0;
                    //try
                    //{
                    //    paymentduebefore -= double.Parse(d["Total_Amount"].ToString().Trim());
                    //}
                    //catch { }
                    try
                    {
                        paymentduebefore = double.Parse(d["Total_Amount"].ToString().Trim());
                    }
                    catch { }
                    double paymentdueafter = 0;
                    string customerorvendor = "";
                    string customerorvendorid = "";
                    try
                    {
                        query = "select * from customer_or_vendor where id=N'" + d["customer_vendor_id"].ToString() + "' and status!='-1'";
                        DataTable dt2 = gm.GetTable(query);
                        if (dt2.Rows.Count > 0)
                        {
                            customerorvendor = dt2.Rows[0]["customer_or_vendor_name"].ToString();
                            customerorvendorid = dt2.Rows[0]["id"].ToString();
                        }
                    }
                    catch { }
                    try
                    {
                        if (d["bill_type"].ToString().Trim() == "Payment")
                        {
                            paymentdueafter = paymentduebefore + (double.Parse(d["paid_amount"].ToString().Trim()) + double.Parse(d["service_charges"].ToString().Trim()));
                        }
                        else if (d["bill_type"].ToString().Trim() == "Receiving")
                        {
                            paymentdueafter = (paymentduebefore + double.Parse(getCustomerTodayBill(customerorvendorid, d["bill_date"].ToString()))) - (double.Parse(d["paid_amount"].ToString().Trim()) + double.Parse(d["service_charges"].ToString().Trim()));
                        }
                    }
                    catch { }
                    string bill_date2 = d["bill_date"].ToString();
                    if (bill_date2.Contains(' '))
                    {
                        bill_date2 = bill_date2.Split(' ')[0].ToString().Trim();
                    }
                    dataGridView1.Rows.Add(d["id"].ToString(), d["bill_id"].ToString(), customerorvendor, bill_date2, d["bill_time"].ToString(), gm.removePoints((double.Parse(d["paid_amount"].ToString().Trim()) + double.Parse(d["service_charges"].ToString().Trim())).ToString()), gm.removePoints(paymentduebefore.ToString()), gm.removePoints(paymentdueafter.ToString()), gm.removePoints(d["payment_method"].ToString()), addedby, customerorvendorid, getCustomerTodayBill(customerorvendorid, d["bill_date"].ToString()), d["paid_amount"].ToString(), d["service_charges"].ToString());
                    if (d["status"].ToString() == "0")
                    {
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                    }
                    try
                    {
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[11].Value = gm.removePoints(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[11].Value.ToString());
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[12].Value = gm.removePoints(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[12].Value.ToString());
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[13].Value = gm.removePoints(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[13].Value.ToString());
                    }
                    catch { }
                }
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            label3.Text = "0";
            label11.Text = "0";
            label6.Text = "0";
            double total_rec = 0;
            double total_bill_ki_raqam = 0;
            double total_com = 0;
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    foreach (DataGridViewRow d in dataGridView1.Rows)
                    {
                        try
                        {
                            total_rec += double.Parse(d.Cells[12].Value.ToString());
                        }
                        catch { }
                        try
                        {
                            total_bill_ki_raqam += double.Parse(d.Cells[11].Value.ToString());
                        }
                        catch { }
                        try
                        {
                            total_com += double.Parse(d.Cells[13].Value.ToString());
                        }
                        catch { }
                    }
                }
            }
            catch { }
            label3.Text = total_rec.ToString();
            label11.Text = total_bill_ki_raqam.ToString();
            label6.Text = total_com.ToString();
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
            //dataGridView1.Sort(dataGridView1.Columns[3], System.ComponentModel.ListSortDirection.Descending);
            foreach (DataGridViewRow d in dataGridView1.Rows)
            {
                DateTime dtm = Convert.ToDateTime(d.Cells[3].Value.ToString());
                d.Cells[3].Value = (dtm.Date.Day.ToString().Length == 1 ? ("0" + dtm.Date.Day.ToString()) : dtm.Date.Day.ToString()) + "-" + (dtm.Date.Month.ToString().Length == 1 ? ("0" + dtm.Date.Month.ToString()) : dtm.Date.Month.ToString()) + "-" + dtm.Date.Year.ToString();
            }
        }

        public void getTransactionIdRecord(string id)
        {
            try
            {
                string query = "Select * from bill where bill_type='" + bill_type + "' and bill_id='" + id + "' and status != '-1' order by Bill_Date DESC";
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
                        addedby = dtlogins.Rows[0][1].ToString() + " (" + dtlogins.Rows[0][0].ToString() + ")";
                    }
                    double paymentduebefore = 0;
                    string customerorvendor = "";
                    string customerorvendorid = "";
                    try
                    {
                        query = "select * from customer_or_vendor where id=N'" + d["customer_vendor_id"].ToString() + "' and status!='-1'";
                        DataTable dt2 = gm.GetTable(query);
                        if (dt2.Rows.Count > 0)
                        {
                            customerorvendor = dt2.Rows[0]["customer_or_vendor_name"].ToString();
                            customerorvendorid = dt2.Rows[0]["id"].ToString();
                        }
                    }
                    catch { }
                    //try
                    //{
                    //    paymentduebefore -= double.Parse(d["Total_Amount"].ToString().Trim());
                    //}
                    //catch { }
                    try
                    {
                        paymentduebefore = double.Parse(d["Total_Amount"].ToString().Trim());
                    }
                    catch { }
                    double paymentdueafter = 0;
                    try
                    {
                        if (d["bill_type"].ToString().Trim() == "Payment")
                        {
                            paymentdueafter = paymentduebefore + (double.Parse(d["paid_amount"].ToString().Trim()) + double.Parse(d["service_charges"].ToString().Trim()));
                        }
                        else if (d["bill_type"].ToString().Trim() == "Receiving")
                        {
                            paymentdueafter = (paymentduebefore + double.Parse(getCustomerTodayBill(customerorvendorid, d["bill_date"].ToString()))) - (double.Parse(d["paid_amount"].ToString().Trim()) + double.Parse(d["service_charges"].ToString().Trim()));
                        }
                    }
                    catch { }
                    string bill_date2 = d["bill_date"].ToString();
                    if (bill_date2.Contains(' '))
                    {
                        bill_date2 = bill_date2.Split(' ')[0].ToString().Trim();                        
                    }
                    dataGridView1.Rows.Add(d["id"].ToString(), d["bill_id"].ToString(), customerorvendor, bill_date2, d["bill_time"].ToString(), gm.removePoints((double.Parse(d["paid_amount"].ToString().Trim()) + double.Parse(d["service_charges"].ToString().Trim())).ToString()), gm.removePoints(paymentduebefore.ToString()), gm.removePoints(paymentdueafter.ToString()), gm.removePoints(d["payment_method"].ToString()), addedby, customerorvendorid, getCustomerTodayBill(customerorvendorid, d["bill_date"].ToString()), d["paid_amount"].ToString(), d["service_charges"].ToString());
                    if (d["status"].ToString() == "0")
                    {
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                    }
                    try
                    {
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[11].Value = gm.removePoints(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[11].Value.ToString());
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[12].Value = gm.removePoints(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[12].Value.ToString());
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[13].Value = gm.removePoints(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[13].Value.ToString());
                    }
                    catch { }
                }
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            label3.Text = "0";
            label11.Text = "0";
            label6.Text = "0";
            double total_rec = 0;
            double total_bill_ki_raqam = 0;
            double total_com = 0;
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    foreach (DataGridViewRow d in dataGridView1.Rows)
                    {
                        try
                        {
                            total_rec += double.Parse(d.Cells[12].Value.ToString());
                        }
                        catch { }
                        try
                        {
                            total_bill_ki_raqam += double.Parse(d.Cells[11].Value.ToString());
                        }
                        catch { }
                        try
                        {
                            total_com += double.Parse(d.Cells[13].Value.ToString());
                        }
                        catch { }
                    }
                }
            }
            catch { }
            label3.Text = total_rec.ToString();
            label11.Text = total_bill_ki_raqam.ToString();
            label6.Text = total_com.ToString();
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
            //dataGridView1.Sort(dataGridView1.Columns[3], System.ComponentModel.ListSortDirection.Descending);
            foreach (DataGridViewRow d in dataGridView1.Rows)
            {
                DateTime dtm = Convert.ToDateTime(d.Cells[3].Value.ToString());
                d.Cells[3].Value = (dtm.Date.Day.ToString().Length == 1 ? ("0" + dtm.Date.Day.ToString()) : dtm.Date.Day.ToString()) + "-" + (dtm.Date.Month.ToString().Length == 1 ? ("0" + dtm.Date.Month.ToString()) : dtm.Date.Month.ToString()) + "-" + dtm.Date.Year.ToString();
            }
        }

        private void metroRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
            metroLabel12.Hide();// = "Vendor Information";
            //metroLabelNameH.Hide();//Text = "Vendor Name : ";
            labelName.Hide();//Text = dt.Rows[0]["customer_Or_Vendor_Name"].ToString();
            //metroLabelContactNumberH.Hide();//Text = "Vendor Name : ";
            labelContactNumber.Hide();//Text = dt.Rows[0]["Contact_Number"].ToString(); ;
            //metroLabelAddressH.Hide();//Text = "Vendor Name : ";
            labelAddress.Hide();//Text = dt.Rows[0]["_Address"].ToString(); ;
            label5.Hide();
            label7.Hide();
            label8.Hide();

            if (dataGridView1.Rows.Count > 0)
                dataGridView1.Rows.Clear();
            metroTextBox2.Hide();
            comboBox_MainCustomer.Hide();
            metroLabel3.Hide();
            metroLabel5.Hide();
            metroDateTimeFrom.Hide();
            metroDateTimeTo.Hide();
            getAllRecord();
            label3.Text = "0";
            label11.Text = "0";
            label6.Text = "0";
            double total_rec = 0;
            double total_bill_ki_raqam = 0;
            double total_com = 0;
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    foreach (DataGridViewRow d in dataGridView1.Rows)
                    {
                        try
                        {
                            total_rec += double.Parse(d.Cells[12].Value.ToString());
                        }
                        catch { }
                        try
                        {
                            total_bill_ki_raqam += double.Parse(d.Cells[11].Value.ToString());
                        }
                        catch { }
                        try
                        {
                            total_com += double.Parse(d.Cells[13].Value.ToString());
                        }
                        catch { }
                    }
                }
            }
            catch { }
            label3.Text = total_rec.ToString();
            label11.Text = total_bill_ki_raqam.ToString();
            label6.Text = total_com.ToString();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            try
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                try
                {
                    string id = dataGridView1.SelectedRows[0].Cells[10].Value.ToString();
                    string query = "select * from customer_or_vendor where id=N'" + id + "'";
                    DataTable dt = gm.GetTable(query);
                    if (dt.Rows.Count > 0)
                    {
                        metroLabel12.Show();// = "Vendor Information";
                        //metroLabelNameH.Show();//Text = "Vendor Name : ";
                        labelName.Show();//Text = dt.Rows[0]["customer_Or_Vendor_Name"].ToString();
                        //metroLabelContactNumberH.Show();//Text = "Vendor Name : ";
                        labelContactNumber.Show();//Text = dt.Rows[0]["Contact_Number"].ToString(); ;
                        //metroLabelAddressH.Show();//Text = "Vendor Name : ";
                        label5.Show();
                        label7.Show();
                        label8.Show();
                        labelAddress.Show();//Text = dt.Rows[0]["_Address"].ToString(); ;

                        if (dt.Rows[0]["Customer_Vendor_Type"].ToString() == "Vendor")
                        {
                            //metroLabel12.Text = "Vendor Information";
                            //metroLabelNameH.Text = "Vendor Name : ";
                            labelName.Text = dt.Rows[0]["customer_Or_Vendor_Name"].ToString();
                            //metroLabelContactNumberH.Text = "Contact Number : ";
                            labelContactNumber.Text = dt.Rows[0]["Contact_Number"].ToString();
                            //metroLabelAddressH.Text = "Address : ";
                            labelAddress.Text = dt.Rows[0]["_Address"].ToString();
                        }
                        else if (dt.Rows[0]["Customer_Vendor_Type"].ToString() == "Customer")
                        {
                            //metroLabel12.Text = "Customer Information";
                            //metroLabelNameH.Text = "Customer Name : ";
                            labelName.Text = dt.Rows[0]["customer_Or_Vendor_Name"].ToString();
                            //metroLabelContactNumberH.Text = "Contact Number : ";
                            labelContactNumber.Text = dt.Rows[0]["Contact_Number"].ToString();
                            //metroLabelAddressH.Text = "Address : ";
                            labelAddress.Text = dt.Rows[0]["_Address"].ToString();
                        }
                    }
                }
                catch { }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
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
            comboBox_MainCustomer.Hide();
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
            try
            {
                groupBox2.Visible = true;
                metroRadioButton8.Checked = true;
                metroLabel3.Hide();// = "DateFromLabel Information";
                metroLabel12.Hide();// = "Vendor Information";
                //metroLabelNameH.Hide();//Text = "Vendor Name : ";
                labelName.Hide();//Text = dt.Rows[0]["customer_Or_Vendor_Name"].ToString();
                //metroLabelContactNumberH.Hide();//Text = "Vendor Name : ";
                labelContactNumber.Hide();//Text = dt.Rows[0]["Contact_Number"].ToString(); ;
                //metroLabelAddressH.Hide();//Text = "Vendor Name : ";
                labelAddress.Hide();//Text = dt.Rows[0]["_Address"].ToString(); ;
                label5.Hide();
                label7.Hide();
                label8.Hide();

                if (dataGridView1.Rows.Count > 0)
                    dataGridView1.Rows.Clear();
                metroTextBox2.Hide();
                comboBox_MainCustomer.Hide();
                metroLabel5.Hide();
                metroDateTimeFrom.Hide();
                metroDateTimeTo.Hide();
                comboBox_MainCustomer.Show();
                comboBox_MainCustomer.Text = string.Empty;
                comboBox_MainCustomer.SelectedIndex = -1;
                comboBox_MainCustomer.Focus();
                label3.Text = "0";
                label11.Text = "0";
                label6.Text = "0";
                double total_rec = 0;
                double total_bill_ki_raqam = 0;
                double total_com = 0;
                try
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow d in dataGridView1.Rows)
                        {
                            try
                            {
                                total_rec += double.Parse(d.Cells[12].Value.ToString());
                            }
                            catch { }
                            try
                            {
                                total_bill_ki_raqam += double.Parse(d.Cells[11].Value.ToString());
                            }
                            catch { }
                            try
                            {
                                total_com += double.Parse(d.Cells[13].Value.ToString());
                            }
                            catch { }
                        }
                    }
                }
                catch { }
                label3.Text = total_rec.ToString();
                label11.Text = total_bill_ki_raqam.ToString();
                label6.Text = total_com.ToString();
            }
            catch { }
        }

        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            metroLabel12.Hide();// = "Vendor Information";
            //metroLabelNameH.Hide();//Text = "Vendor Name : ";
            labelName.Hide();//Text = dt.Rows[0]["customer_Or_Vendor_Name"].ToString();
            //metroLabelContactNumberH.Hide();//Text = "Vendor Name : ";
            labelContactNumber.Hide();//Text = dt.Rows[0]["Contact_Number"].ToString(); ;
            //metroLabelAddressH.Hide();//Text = "Vendor Name : ";
            labelAddress.Hide();//Text = dt.Rows[0]["_Address"].ToString(); ;
            label5.Hide();
            label7.Hide();
            label8.Hide();

            try
            {
                string id = comboBox_MainCustomer.SelectedValue.ToString();
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
            groupBox2.Visible = false;
            metroLabel12.Hide();// = "Vendor Information";
            //metroLabelNameH.Hide();//Text = "Vendor Name : ";
            labelName.Hide();//Text = dt.Rows[0]["customer_Or_Vendor_Name"].ToString();
            //metroLabelContactNumberH.Hide();//Text = "Vendor Name : ";
            labelContactNumber.Hide();//Text = dt.Rows[0]["Contact_Number"].ToString(); ;
            //metroLabelAddressH.Hide();//Text = "Vendor Name : ";
            labelAddress.Hide();//Text = dt.Rows[0]["_Address"].ToString(); ;
            label5.Hide();
            label7.Hide();
            label8.Hide();

            if (dataGridView1.Rows.Count > 0)
                dataGridView1.Rows.Clear();
            comboBox_MainCustomer.Hide();
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
            metroLabel12.Hide();// = "Vendor Information";
            //metroLabelNameH.Hide();//Text = "Vendor Name : ";
            labelName.Hide();//Text = dt.Rows[0]["customer_Or_Vendor_Name"].ToString();
            //metroLabelContactNumberH.Hide();//Text = "Vendor Name : ";
            labelContactNumber.Hide();//Text = dt.Rows[0]["Contact_Number"].ToString(); ;
            //metroLabelAddressH.Hide();//Text = "Vendor Name : ";
            labelAddress.Hide();//Text = dt.Rows[0]["_Address"].ToString(); ;
            label5.Hide();
            label7.Hide();
            label8.Hide();

            try
            {
                if (metroTextBox2.Text.Trim() != "")
                {
                    getTransactionIdRecord(metroTextBox2.Text.Trim());
                }
                if (metroTextBox2.Text.Trim() == "")
                    getAllRecord();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroComboBoxUserType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            if (bill_type == "Expense")
                return;
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    //string month = dataGridView1.SelectedRows[0].Cells[2].Value.ToString().Split(' ')[0].ToString().Split('/')[0].ToString();
                    //string day = dataGridView1.SelectedRows[0].Cells[2].Value.ToString().Split(' ')[0].ToString().Split('/')[1].ToString();
                    //string year = dataGridView1.SelectedRows[0].Cells[2].Value.ToString().Split(' ')[0].ToString().Split('/')[2].ToString();
                    string date = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                    try
                    {
                        DateTime dtm = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells[2].Value.ToString());
                        string monthName = dtm.Date.ToString("MMM", CultureInfo.InvariantCulture);
                        date = dtm.Day.ToString() + " " + monthName + "," + dtm.Year.ToString();
                    }catch
                    {}
                    if (bill_type == "Purchase Trading")
                    {
                        gm.printBill(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), "Purchase Invoice", "Vendor", "Duplicate Receipt",date);
                    }
                    if (bill_type == "Sale Trading")
                    {
                        gm.printBill(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), "Sale Invoice", "Customer", "Duplicate Receipt",date);
                    }
                }
                else
                {
                    MessageBox.Show("select bill to print");
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
                dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Selected = true;
            }
            catch { }
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    if (bill_type == "Purchase Trading")
                    {
                        string query = "select * from bill_details where bill_id = '" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString().Trim() + "'";
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
                        }
                    }
                    if (bill_type == "Sale Trading")
                    {
                        string query = "select * from bill_details where bill_id = '" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString().Trim() + "'";
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
                try
                {
                    string id = dataGridView1.SelectedRows[0].Cells[10].Value.ToString();
                    string query = "select * from customer_or_vendor where id=N'" + id + "'";
                    DataTable dt = gm.GetTable(query);
                    if (dt.Rows.Count > 0)
                    {
                        metroLabel12.Show();// = "Vendor Information";
                        //metroLabelNameH.Show();//Text = "Vendor Name : ";
                        labelName.Show();//Text = dt.Rows[0]["customer_Or_Vendor_Name"].ToString();
                        //metroLabelContactNumberH.Show();//Text = "Vendor Name : ";
                        labelContactNumber.Show();//Text = dt.Rows[0]["Contact_Number"].ToString(); ;
                        //metroLabelAddressH.Show();//Text = "Vendor Name : ";
                        label5.Show();
                        label7.Show();
                        label8.Show();
                        labelAddress.Show();//Text = dt.Rows[0]["_Address"].ToString(); ;

                        if (dt.Rows[0]["Customer_Vendor_Type"].ToString() == "Vendor")
                        {
                            //metroLabel12.Text = "Vendor Information";
                            //metroLabelNameH.Text = "Vendor Name : ";
                            labelName.Text = dt.Rows[0]["customer_Or_Vendor_Name"].ToString();
                            //metroLabelContactNumberH.Text = "Contact Number : ";
                            labelContactNumber.Text = dt.Rows[0]["Contact_Number"].ToString();
                            //metroLabelAddressH.Text = "Address : ";
                            labelAddress.Text = dt.Rows[0]["_Address"].ToString();
                        }
                        else if (dt.Rows[0]["Customer_Vendor_Type"].ToString() == "Customer")
                        {
                            //metroLabel12.Text = "Customer Information";
                            //metroLabelNameH.Text = "Customer Name : ";
                            labelName.Text = dt.Rows[0]["customer_Or_Vendor_Name"].ToString();
                            //metroLabelContactNumberH.Text = "Contact Number : ";
                            labelContactNumber.Text = dt.Rows[0]["Contact_Number"].ToString();
                            //metroLabelAddressH.Text = "Address : ";
                            labelAddress.Text = dt.Rows[0]["_Address"].ToString();
                        }
                    }
                }
                catch { }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroRadioButton2_CheckedChanged_1(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
            metroLabel12.Hide();// = "Vendor Information";
            //metroLabelNameH.Hide();//Text = "Vendor Name : ";
            labelName.Hide();//Text = dt.Rows[0]["customer_Or_Vendor_Name"].ToString();
            //metroLabelContactNumberH.Hide();//Text = "Vendor Name : ";
            labelContactNumber.Hide();//Text = dt.Rows[0]["Contact_Number"].ToString(); ;
            //metroLabelAddressH.Hide();//Text = "Vendor Name : ";
            labelAddress.Hide();//Text = dt.Rows[0]["_Address"].ToString(); ;
            label5.Hide();
            label7.Hide();
            label8.Hide();

            if (dataGridView1.Rows.Count > 0)
                dataGridView1.Rows.Clear();
            metroTextBox2.Hide();
            comboBox_MainCustomer.Hide();
            metroLabel3.Show();
            metroLabel5.Show();
            metroDateTimeFrom.Show();
            metroDateTimeTo.Show();
            metroDateTimeFrom.Focus();
            getDateRecord();
            label3.Text = "0";
            label11.Text = "0";
            label6.Text = "0";
            double total_rec = 0;
            double total_bill_ki_raqam = 0;
            double total_com = 0;
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    foreach (DataGridViewRow d in dataGridView1.Rows)
                    {
                        try
                        {
                            total_rec += double.Parse(d.Cells[12].Value.ToString());
                        }
                        catch { }
                        try
                        {
                            total_bill_ki_raqam += double.Parse(d.Cells[11].Value.ToString());
                        }
                        catch { }
                        try
                        {
                            total_com += double.Parse(d.Cells[13].Value.ToString());
                        }
                        catch { }
                    }
                }
            }
            catch { }
            label3.Text = total_rec.ToString();
            label11.Text = total_bill_ki_raqam.ToString();
            label6.Text = total_com.ToString();
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
                //if (e.Modifiers == Keys.Control && e.KeyCode == Keys.P)
                //    metroTile1.PerformClick();
                if (e.Modifiers == Keys.Control && e.KeyCode == Keys.P)
                    metroTile2.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void metroRadioButton5_CheckedChanged(object sender, EventArgs e)
        {
            metroLabel12.Hide();// = "Vendor Information";
            //metroLabelNameH.Hide();//Text = "Vendor Name : ";
            labelName.Hide();//Text = dt.Rows[0]["customer_Or_Vendor_Name"].ToString();
            //metroLabelContactNumberH.Hide();//Text = "Vendor Name : ";
            labelContactNumber.Hide();//Text = dt.Rows[0]["Contact_Number"].ToString(); ;
            //metroLabelAddressH.Hide();//Text = "Vendor Name : ";
            labelAddress.Hide();//Text = dt.Rows[0]["_Address"].ToString(); ;
            label5.Hide();
            label7.Hide();
            label8.Hide();

            if (dataGridView1.Rows.Count > 0)
                dataGridView1.Rows.Clear();
            metroTextBox2.Hide();
            comboBox_MainCustomer.Hide();
            metroLabel3.Hide();
            metroLabel5.Hide();
            metroDateTimeFrom.Hide();
            metroDateTimeTo.Hide();
            getAllVendorRecord();
        }

        private void metroRadioButton6_CheckedChanged(object sender, EventArgs e)
        {
            metroLabel12.Hide();// = "Vendor Information";
            //metroLabelNameH.Hide();//Text = "Vendor Name : ";
            labelName.Hide();//Text = dt.Rows[0]["customer_Or_Vendor_Name"].ToString();
            //metroLabelContactNumberH.Hide();//Text = "Vendor Name : ";
            labelContactNumber.Hide();//Text = dt.Rows[0]["Contact_Number"].ToString(); ;
            //metroLabelAddressH.Hide();//Text = "Vendor Name : ";
            labelAddress.Hide();//Text = dt.Rows[0]["_Address"].ToString(); ;
            label5.Hide();
            label7.Hide();
            label8.Hide();

            if (dataGridView1.Rows.Count > 0)
                dataGridView1.Rows.Clear();
            metroTextBox2.Hide();
            comboBox_MainCustomer.Hide();
            metroLabel3.Hide();
            metroLabel5.Hide();
            metroDateTimeFrom.Hide();
            metroDateTimeTo.Hide();
            getAllCustomerRecord();
        }

        public string getCustomerTodayBill(string customer_id)
        {
            try
            {
                string npacked = DateTime.ParseExact(dataGridView1.SelectedRows[0].Cells[3].Value.ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");
                DateTime dtm = Convert.ToDateTime(npacked);
                //string monthName = dtm.Date.ToString("MMM", CultureInfo.InvariantCulture);
                string date = dtm.Year.ToString() + "-" + dtm.Month.ToString() + "-" + dtm.Day.ToString();
                //string todaydate= DateTime.Now.Year.ToString()+"-"+DateTime.Now.Month.ToString()+"-"+DateTime.Now.Day.ToString();
                string query = "select sum(Total_Amount) from Bill where status!='-1' and Bill_Type='sale trading' and Bill_Date>='" + date + "' and Bill_Date<='" + date + "' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='" + customer_id + "')";
                DataTable dt = gm.GetTable(query);
                if (dt.Rows.Count > 0)
                {
                    if(dt.Rows[0][0].ToString().Trim() != "")
                        return dt.Rows[0][0].ToString();
                    return "0";
                }
                return "0";
            }
            catch
            {
                return "0";
            }
        }

        public string getTotalSaleAmount(string customer_id)
        {
            try
            {
                string npacked = DateTime.ParseExact(dataGridView1.SelectedRows[0].Cells[3].Value.ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");
                DateTime dtm = Convert.ToDateTime(npacked);
                //string monthName = dtm.Date.ToString("MMM", CultureInfo.InvariantCulture);
                string date = dtm.Year.ToString() + "-" + dtm.Month.ToString() + "-" + dtm.Day.ToString();
                string query = "select sum(Total_Amount) from Bill where status!='-1' and Bill_Type='sale trading' and Bill_Date<='" + date + "' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='" + customer_id + "')";
                //string query = "select sum(Total_Amount) from Bill where status!='-1' and Bill_Type='sale trading' and Bill_Date>='" + date + "' and Bill_Date<='" + date + "' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='" + customer_id + "')";
                DataTable dt = gm.GetTable(query);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString().Trim() != "")
                        return dt.Rows[0][0].ToString();
                    return "0";
                }
                return "0";
            }
            catch
            {
                return "0";
            }
        }

        public string getTotalReceivingAmount(string customer_id,string id)
        {
            try
            {
                string npacked = DateTime.ParseExact(dataGridView1.SelectedRows[0].Cells[3].Value.ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");
                DateTime dtm = Convert.ToDateTime(npacked);
                //string monthName = dtm.Date.ToString("MMM", CultureInfo.InvariantCulture);
                string date = dtm.Year.ToString() + "-" + dtm.Month.ToString() + "-" + dtm.Day.ToString();
                string query = "select sum(-balance) from Bill where status!='-1' and Bill_Type='receiving' and Bill_Date<='" + date + "' and Customer_Vendor_Id='" + customer_id + "'";
                //string query = "select sum(-balance) from Bill where status!='-1' and Bill_Type='receiving' and Bill_Date>='" + date + "' and Bill_Date<='" + date + "' and Customer_Vendor_Id='" + customer_id + "' and id<='"+id+"'";
                DataTable dt = gm.GetTable(query);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString().Trim() != "")
                        return dt.Rows[0][0].ToString();
                    return "0";
                }
                return "0";
            }
            catch
            {
                return "0";
            }
        }

        public string billamountwithouttoday(string customer_id)
        {
            try
            {
                string npacked = DateTime.ParseExact(dataGridView1.SelectedRows[0].Cells[3].Value.ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");
                DateTime dtm = Convert.ToDateTime(npacked);
                //string monthName = dtm.Date.ToString("MMM", CultureInfo.InvariantCulture);
                string date = dtm.Year.ToString() + "-" + dtm.Month.ToString() + "-" + dtm.Day.ToString();
                string query = "select sum(Total_Amount) from Bill where status!='-1' and Bill_Type='sale trading' and Bill_Date<'" + date + "' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='" + customer_id + "')";
                DataTable dt = gm.GetTable(query);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString().Trim() != "")
                        return dt.Rows[0][0].ToString();
                    return "0";
                }
                return "0";
            }
            catch
            {
                return "0";
            }
        }

        public string receivingamountwithouttoday(string customer_id)
        {
            try
            {
                string npacked = DateTime.ParseExact(dataGridView1.SelectedRows[0].Cells[3].Value.ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");
                DateTime dtm = Convert.ToDateTime(npacked);
                //string monthName = dtm.Date.ToString("MMM", CultureInfo.InvariantCulture);
                string date = dtm.Year.ToString() + "-" + dtm.Month.ToString() + "-" + dtm.Day.ToString();
                string query = "select sum(-balance) from Bill where status!='-1' and Bill_Type='receiving' and Bill_Date<'" + date + "' and Customer_Vendor_Id='" + customer_id + "'";
                DataTable dt = gm.GetTable(query);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString().Trim() != "")
                        return dt.Rows[0][0].ToString();
                    return "0";
                }
                return "0";
            }
            catch
            {
                return "0";
            }
        }

        public string todayReceiving(string customer_id,string id)
        {
            try
            {
                string npacked = DateTime.ParseExact(dataGridView1.SelectedRows[0].Cells[3].Value.ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");
                DateTime dtm = Convert.ToDateTime(npacked);
                string date = dtm.Year.ToString() + "-" + dtm.Month.ToString() + "-" + dtm.Day.ToString();
                //string query = "select sum(-balance) from Bill where status!='-1' and Bill_Type='receiving' and Bill_Date>='" + date + "' and Bill_Date<='" + date + "' and Customer_Vendor_Id='" + customer_id + "' and id<'"+id+"'";
                string query = "select sum(-balance) from Bill where status!='-1' and Bill_Type='receiving' and Bill_Date>='" + date + "' and Bill_Date<='" + date + "' and Customer_Vendor_Id='" + customer_id + "' and id!='" + id + "'";
                DataTable dt = gm.GetTable(query);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString().Trim() != "")
                        return dt.Rows[0][0].ToString();
                    return "0";
                }
                return "0";
            }
            catch
            {
                return "0";
            }
        }

        public string getCustomerTodayBill(string customer_id, string date1)
        {
            try
            {
                DateTime dtm = Convert.ToDateTime(date1);
                //string monthName = dtm.Date.ToString("MMM", CultureInfo.InvariantCulture);
                string date = dtm.Year.ToString() + "-" + dtm.Month.ToString() + "-" + dtm.Day.ToString();
                string query = "select sum(Total_Amount) from Bill where status!='-1' and Bill_Type='sale trading' and Bill_Date>='" + date + "' and Bill_Date<='" + date + "' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='" + customer_id + "')";
                DataTable dt = gm.GetTable(query);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString().Trim() != "")
                        return dt.Rows[0][0].ToString();
                    return "0";
                }
                return "0";
            }
            catch
            {
                return "0";
            }
        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    string npacked = DateTime.ParseExact(dataGridView1.SelectedRows[0].Cells[3].Value.ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd"); 
                    DateTime dtm = Convert.ToDateTime(npacked);
                    string monthName = dtm.Date.ToString("MMM", CultureInfo.InvariantCulture);
                    string date = dtm.Day.ToString() + "-" + dtm.Month.ToString() + "-" + dtm.Year.ToString();
                    string report = (bill_type == "Payment" ? "Payment Receipt" : "Receiving Receipt");
                    string PaymentToOrReceivingFrom = (bill_type == "Payment" ? "Payment To : " : "Receiving From : ");
                    string customerOrVendorname = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                    string query = "";
                    string customerOrVendorid = "";
                    try
                    {
                        customerOrVendorid = dataGridView1.SelectedRows[0].Cells[10].Value.ToString();
                    }
                    catch { }
                    string PaymentToOrReceivingFromValue = customerOrVendorname;
                    string Amount = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                    try
                    {
                        Amount = (double.Parse(Amount) + double.Parse(todayReceiving(dataGridView1.SelectedRows[0].Cells[10].Value.ToString().Trim(), dataGridView1.SelectedRows[0].Cells[0].Value.ToString()))).ToString();
                    }
                    catch { }
                    string PaymentDueBefore = "Amount Due Before " + bill_type + " : ";
                    double PaymentDueBeforeValue1 = 0;
                    try
                    {
                        double billamountwithouttoday2 = Math.Round(double.Parse(billamountwithouttoday(dataGridView1.SelectedRows[0].Cells[10].Value.ToString().Trim())), 3);
                        double receivingamountwithouttoday2 = Math.Round(double.Parse(receivingamountwithouttoday(dataGridView1.SelectedRows[0].Cells[10].Value.ToString().Trim())), 3);
                        PaymentDueBeforeValue1 = (billamountwithouttoday2 - receivingamountwithouttoday2);
                        PaymentDueBeforeValue1 = Math.Round(PaymentDueBeforeValue1, 3);
                    }
                    catch { }
                    string PaymentDueBeforeValue = PaymentDueBeforeValue1.ToString();
                    string PaymentDueAfter = "Amount Due After " + bill_type+ " : ";
                    double PaymentDueAfterValue1 = 0;
                    try
                    {
                        PaymentDueAfterValue1 = double.Parse(dataGridView1.SelectedRows[0].Cells[7].Value.ToString().Trim());
                    }
                    catch { }
                    double getTotalSaleAmount2 = double.Parse(getTotalSaleAmount(dataGridView1.SelectedRows[0].Cells[10].Value.ToString()));
                    double getTotalReceivingAmount2 = double.Parse(getTotalReceivingAmount(dataGridView1.SelectedRows[0].Cells[10].Value.ToString(), dataGridView1.SelectedRows[0].Cells[0].Value.ToString()));
                    string PaymentDueAfterValue = (getTotalSaleAmount2 - getTotalReceivingAmount2).ToString();
                    try
                    {
                        PaymentDueAfterValue = (Math.Round(double.Parse(PaymentDueAfterValue), 3)).ToString();
                    }
                    catch
                    { }

                    gm.printPaymentReceivingReceipt(monthName, date, report, dataGridView1.SelectedRows[0].Cells[1].Value.ToString().Trim(), date, PaymentToOrReceivingFrom, customerOrVendorname, Amount, PaymentDueBefore, PaymentDueBeforeValue, PaymentDueAfter, PaymentDueAfterValue, customerOrVendorid,gm.removePoints(getCustomerTodayBill(customerOrVendorid)));
                }
                else
                {
                    MessageBox.Show("Select "+bill_type+" From List To Print");
                }
            }
            catch (Exception ex) {
                //MessageBox.Show(ex.Message);
            }
        }

        private void metroRadioButton7_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (metroRadioButton7.Checked == true)
                {
                    groupBox1.Visible = false;
                    try
                    {
                        string id = comboBox_MainCustomer.SelectedValue.ToString();
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
            }
            catch { }
            label3.Text = "0";
            label11.Text = "0";
            label6.Text = "0";
            double total_rec = 0;
            double total_bill_ki_raqam = 0;
            double total_com = 0;
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    foreach (DataGridViewRow d in dataGridView1.Rows)
                    {
                        try
                        {
                            total_rec += double.Parse(d.Cells[12].Value.ToString());
                        }
                        catch { }
                        try
                        {
                            total_bill_ki_raqam += double.Parse(d.Cells[11].Value.ToString());
                        }
                        catch { }
                        try
                        {
                            total_com += double.Parse(d.Cells[13].Value.ToString());
                        }
                        catch { }
                    }
                }
            }
            catch { }
            label3.Text = total_rec.ToString();
            label11.Text = total_bill_ki_raqam.ToString();
            label6.Text = total_com.ToString();
        }

        private void metroRadioButton8_CheckedChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    metroDateTime1.Value = DateTime.Now;
            //    metroDateTime2.Value = DateTime.Now;
            //}
            //catch { }
            groupBox1.Visible = true;
        }

        private void metroDateTime2_ValueChanged(object sender, EventArgs e)
        {
            metroDateTime2.MaxDate = metroDateTime1.Value;
            metroDateTime1.MinDate = metroDateTime2.Value;
            try
            {
                string id = comboBox_MainCustomer.SelectedValue.ToString();
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
            label3.Text = "0";
            label11.Text = "0";
            label6.Text = "0";
            double total_rec = 0;
            double total_bill_ki_raqam = 0;
            double total_com = 0;
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    foreach (DataGridViewRow d in dataGridView1.Rows)
                    {
                        try
                        {
                            total_rec += double.Parse(d.Cells[12].Value.ToString());
                        }
                        catch { }
                        try
                        {
                            total_bill_ki_raqam += double.Parse(d.Cells[11].Value.ToString());
                        }
                        catch { }
                        try
                        {
                            total_com += double.Parse(d.Cells[13].Value.ToString());
                        }
                        catch { }
                    }
                }
            }
            catch { }
            label3.Text = total_rec.ToString();
            label11.Text = total_bill_ki_raqam.ToString();
            label6.Text = total_com.ToString();
        }

        private void metroDateTime1_ValueChanged(object sender, EventArgs e)
        {
            metroDateTime2.MaxDate = metroDateTime1.Value;
            metroDateTime1.MinDate = metroDateTime2.Value;
            try
            {
                string id = comboBox_MainCustomer.SelectedValue.ToString();
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
            label3.Text = "0";
            label11.Text = "0";
            label6.Text = "0";
            double total_rec = 0;
            double total_bill_ki_raqam = 0;
            double total_com = 0;
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    foreach (DataGridViewRow d in dataGridView1.Rows)
                    {
                        try
                        {
                            total_rec += double.Parse(d.Cells[12].Value.ToString());
                        }
                        catch { }
                        try
                        {
                            total_bill_ki_raqam += double.Parse(d.Cells[11].Value.ToString());
                        }
                        catch { }
                        try
                        {
                            total_com += double.Parse(d.Cells[13].Value.ToString());
                        }
                        catch { }
                    }
                }
            }
            catch { }
            label3.Text = total_rec.ToString();
            label11.Text = total_bill_ki_raqam.ToString();
            label6.Text = total_com.ToString();
        }

        private void comboBox_MainCustomer_TextChanged(object sender, EventArgs e)
        {
            if (isFirst == true)
                return;
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.Clear();
            }
            metroLabel12.Hide();// = "Vendor Information";
            //metroLabelNameH.Hide();//Text = "Vendor Name : ";
            labelName.Hide();//Text = dt.Rows[0]["customer_Or_Vendor_Name"].ToString();
            //metroLabelContactNumberH.Hide();//Text = "Vendor Name : ";
            labelContactNumber.Hide();//Text = dt.Rows[0]["Contact_Number"].ToString(); ;
            //metroLabelAddressH.Hide();//Text = "Vendor Name : ";
            labelAddress.Hide();//Text = dt.Rows[0]["_Address"].ToString(); ;
            label5.Hide();
            label7.Hide();
            label8.Hide();

            try
            {
                if (comboBox_MainCustomer.Text.Trim().Length > 0)
                {
                    //string id = comboBox_MainCustomer.SelectedValue.ToString();
                    string id = "";
                    try//only word if main customer or sub sub customer name is unique
                    {
                        string query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + comboBox_MainCustomer.Text.Trim() + "' and status='1'";
                        id = gm.GetTable(query).Rows[0][0].ToString();
                    }
                    catch { }
                    if (id.Trim() != "")
                    {
                        getUserNameRecord(id);
                    }
                }
                //if (metroTextBox1.Text.Trim() == "")
                //    getAllRecord();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void comboBox_MainCustomer_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (comboBox_MainCustomer.Text.Trim().Length > 0)
                {
                    //string id = comboBox_MainCustomer.SelectedValue.ToString();
                    string id = "";
                    try//only word if main customer or sub sub customer name is unique
                    {
                        string query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + comboBox_MainCustomer.Text.Trim() + "' and status='1'";
                        id = gm.GetTable(query).Rows[0][0].ToString();
                    }
                    catch { }
                    if (id.Trim() != "")
                    {
                        getUserNameRecord(id);
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void comboBox_MainCustomer_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private void comboBox_MainCustomer_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void metroTile3_Click(object sender, EventArgs e)
        {

        }

        private void metroTile3_Click_1(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Bill_Number");
            dt.Columns.Add("Naam");
            dt.Columns.Add("Tareekh");
            dt.Columns.Add("Waqt");
            dt.Columns.Add("Bill_Ki_Raqam");
            dt.Columns.Add("Raqam_Wasool");
            dt.Columns.Add("Commission");
            foreach (DataGridViewRow d in dataGridView1.Rows)
                dt.Rows.Add(d.Cells[1].ToString(), d.Cells[2].ToString(), d.Cells[3].ToString(), d.Cells[4].ToString(), d.Cells[11].ToString(), d.Cells[12].ToString(), d.Cells[13].ToString());
            gm.printPaymentReceivingList(dt);
        }

    }
}
