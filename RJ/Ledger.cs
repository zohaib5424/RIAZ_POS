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
    public partial class Ledger : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public Ledger()
        {
            InitializeComponent();
        }

        string bill_type = "";

        AutoCompleteStringCollection source = new AutoCompleteStringCollection();
        AutoCompleteStringCollection sourceusername = new AutoCompleteStringCollection();
        private void ItemsList_Load(object sender, EventArgs e)
        {
            try
            {
                metroDateTimeFrom.Value = DateTime.Now.Date;
                metroDateTimeTo.Value = DateTime.Now.Date;
                metroDateTimeFrom.MaxDate = metroDateTimeTo.Value;
                metroDateTimeTo.MinDate = metroDateTimeFrom.Value;
            }catch
            {}
            dataGridView1.Columns[0].DefaultCellStyle.Format = "yyyy-MM-dd.";
            //metroTile1.Hide();
            //label1.Hide();
            try
            {
                source = new AutoCompleteStringCollection();
                sourceusername = new AutoCompleteStringCollection();
                textBox1.Hide();

                this.AutoScroll = true;

                string customervendortype = "Vendor";
                DataTable dt = gm.GetTable("select * from customer_or_vendor where status ='1' and (parent='' or parent='NULL')");
                foreach (DataRow d in dt.Rows)
                {
                    sourceusername.Add(d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")");
                }

                textBox1.DisplayMember = "customer_Or_Vendor_Name";
                textBox1.ValueMember = "id";
                textBox1.DataSource = dt;
                textBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
                textBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
                textBox1.SelectedIndex = -1;

                metroDateTimeFrom.Value = metroDateTimeTo.Value = DateTime.Now;
                metroDateTimeTo.MinDate = metroDateTimeFrom.Value.Date;
                metroLabel3.Hide();
                metroLabel5.Hide();
                if(bill_type == "Purchase Trading")
                {
                    dataGridView2.Hide();
                }
                if (bill_type == "Sale Trading")
                {
                    dataGridView2.Show();
                }

                textBox1.Show();
                textBox1.Focus();

                DataGridViewTextBoxColumn t = new DataGridViewTextBoxColumn();
                t.Name = "t";
                dataGridView2.Columns.Add(t);
                DataGridViewTextBoxColumn g = new DataGridViewTextBoxColumn();
                g.Name = "g";
                dataGridView2.Columns.Add(g);
                dataGridView2.Columns[0].Width = 150;
                dataGridView2.ColumnHeadersVisible = false;
                metroRadioButton1.Checked = true;

                metroLabel3.Hide();
                metroLabel5.Hide();
                metroDateTimeFrom.Hide();
                metroDateTimeTo.Hide();
                textBox1.Focus();
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
            lable_Tota_Dr.Text = "0";
            lable_Tota_Cr.Text = "0";
            lable_Balance.Text = "0";
            try
            {
                string query = "Select * from bill where bill_type='"+bill_type+"' and status != '-1'";
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
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
                        addedby = dtlogins.Rows[0][1].ToString() +" ("+dtlogins.Rows[0][0].ToString()+")";
                    }
                    dataGridView1.Rows.Add(d["id"].ToString(), d["bill_id"].ToString(), d["bill_date"].ToString(), d["bill_time"].ToString(), d["discount_amount"].ToString(), d["tax_amount"].ToString(), d["service_charges"].ToString(), d["total_amount"].ToString(), d["paid_amount"].ToString(), d["payment_method"].ToString(), addedby, "Return");
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

        public void getDateRecord()
        {
            lable_Tota_Dr.Text = "0";
            lable_Tota_Cr.Text = "0";
            lable_Balance.Text = "0";
            try
            {
                string from = metroDateTimeFrom.Value.Date.Year.ToString() +"-"+metroDateTimeFrom.Value.Date.Month.ToString() +"-"+metroDateTimeFrom.Value.Date.Day.ToString();
                string to = metroDateTimeTo.Value.Date.Year.ToString() +"-"+metroDateTimeTo.Value.Date.Month.ToString() +"-"+metroDateTimeTo.Value.Date.Day.ToString();
                string query = "Select * from bill where bill_type='" + bill_type + "' and bill_date >= '"+from+"' and bill_date<='"+to+"' and status != '-1'";
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
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
                        addedby = dtlogins.Rows[0][1].ToString() + " (" + dtlogins.Rows[0][0].ToString() + ")";
                    }
                    dataGridView1.Rows.Add(d["id"].ToString(), d["bill_id"].ToString(), d["bill_date"].ToString(), d["bill_time"].ToString(), d["discount_amount"].ToString(), d["tax_amount"].ToString(), d["service_charges"].ToString(), d["total_amount"].ToString(), d["paid_amount"].ToString(), d["payment_method"].ToString(), addedby, "Return");
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
            lable_Tota_Dr.Text = "0";
            lable_Tota_Cr.Text = "0";
            lable_Balance.Text = "0";
            try
            {
                string query = "Select * from bill where bill_type='" + bill_type + "' and customer_vendor_id='" + id + "' and status != '-1'";
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
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
                        addedby = dtlogins.Rows[0][1].ToString() + " (" + dtlogins.Rows[0][0].ToString() + ")";
                    }
                    dataGridView1.Rows.Add(d["id"].ToString(), d["bill_id"].ToString(), d["bill_date"].ToString(), d["bill_time"].ToString(), d["discount_amount"].ToString(), d["tax_amount"].ToString(), d["service_charges"].ToString(), d["total_amount"].ToString(), d["paid_amount"].ToString(), d["payment_method"].ToString(), addedby, "Return");
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

        public void getTransactionIdRecord(string id)
        {
            lable_Tota_Dr.Text = "0";
            lable_Tota_Cr.Text = "0";
            lable_Balance.Text = "0";
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
                foreach (DataRow d in dt.Rows)
                {
                    string addedby = d["AddedBy_UserId"].ToString();
                    if (addedby.Trim() != "")
                    {
                        query = "select * from login where id='" + d["AddedBy_UserId"].ToString() + "'";
                        DataTable dtlogins = gm.GetTable(query);
                        addedby = dtlogins.Rows[0][1].ToString() + " (" + dtlogins.Rows[0][0].ToString() + ")";
                    }
                    dataGridView1.Rows.Add(d["id"].ToString(), d["bill_id"].ToString(), d["bill_date"].ToString(), d["bill_time"].ToString(), d["discount_amount"].ToString(), d["tax_amount"].ToString(), d["service_charges"].ToString(), d["total_amount"].ToString(), d["paid_amount"].ToString(), d["payment_method"].ToString(), addedby, "Return");
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
            metroLabel3.Hide();
            metroLabel5.Hide();
            metroDateTimeFrom.Hide();
            metroDateTimeTo.Hide();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                metroListView1.Items.Clear();
                try
                {
                    string[] items = dataGridView1.SelectedRows[0].Cells[5].Value.ToString().Trim().Split(',');
                    foreach (String s in items)
                    {
                        metroListView1.Items.Add(s);
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

        }

        private void metroRadioButton2_CheckedChanged(object sender, EventArgs e)
        {

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

        }

        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lable_Tota_Dr.Text = "0";
                lable_Tota_Cr.Text = "0";
                lable_Balance.Text = "0";
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroRadioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void metroTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void metroComboBoxUserType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        int radio = 0;
        private void metroTile1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    string date = "";
                    if (radio == 1)
                        date = "تمام ریکارڈ";
                    else if(radio == 2)
                    {
                        string frommonthName = metroDateTimeFrom.Value.Date.ToString("MMM", CultureInfo.InvariantCulture);
                        string tomonthName = metroDateTimeTo.Value.Date.ToString("MMM", CultureInfo.InvariantCulture);
                        date = "تاریخ سے ";
                        date += metroDateTimeFrom.Value.Date.Day.ToString() + "-" + metroDateTimeFrom.Value.Date.Month.ToString() + "-" + metroDateTimeFrom.Value.Date.Year.ToString();
                        date += " تاریخ تک " + metroDateTimeTo.Value.Date.Day.ToString() + "-" + metroDateTimeTo.Value.Date.Month.ToString() + "-" + metroDateTimeTo.Value.Date.Year.ToString();
                    }
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Date");
                    dt.Columns.Add("Bill_Type");
                    dt.Columns.Add("Dr");
                    dt.Columns.Add("Cr");
                    dt.Columns.Add("Balance");
                    foreach (DataGridViewRow d in dataGridView1.Rows)
                    {
                        dt.Rows.Add(d.Cells[0].Value.ToString(), d.Cells[1].Value.ToString(), d.Cells[2].Value.ToString(), d.Cells[3].Value.ToString(), d.Cells[4].Value.ToString());
                    }
                    try
                    {
                        ledgerof = textBox1.Text.Trim().Split('(')[0].ToString().Trim();
                    }
                    catch { }
                    gm.PrintLedger("", date, dt, ledgerof);
                }
                else
                {
                    MessageBox.Show("No Data Found To Print Ledger");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void metroRadioButton2_CheckedChanged_1(object sender, EventArgs e)
        {
            metroLabel3.Show();
            metroLabel5.Show();
            metroDateTimeFrom.Show();
            metroDateTimeTo.Show();
        }

        private void metroDateTimeFrom_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                metroDateTimeFrom.MaxDate = metroDateTimeTo.Value;
                metroDateTimeTo.MinDate = metroDateTimeFrom.Value;
            }
            catch
            { }
            try
            {
                metroDateTimeTo.MinDate = metroDateTimeFrom.Value.Date;
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
                metroDateTimeFrom.MaxDate = metroDateTimeTo.Value;
                metroDateTimeTo.MinDate = metroDateTimeFrom.Value;
            }
            catch { }
            try
            {

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
                    metroTile1.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void metroLabel1_Click(object sender, EventArgs e)
        {

        }

        public void getPreviousBalance(string customer_vendor_id)
        {
            try
            {
                string from = metroDateTimeFrom.Value.Date.Year.ToString() + "-" + metroDateTimeFrom.Value.Date.Month.ToString() + "-" + metroDateTimeFrom.Value.Date.Day.ToString();
                string to = metroDateTimeTo.Value.Date.Year.ToString() + "-" + metroDateTimeTo.Value.Date.Month.ToString() + "-" + metroDateTimeTo.Value.Date.Day.ToString();
                //select sum(total_amount) from Bill where Bill_Type='sale trading' and Status='1' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='55')
                //select sum(Paid_Amount+Service_Charges) from Bill where Bill_Type='receiving' and Status='1' and Customer_Vendor_Id='55'
                string query = "select sum(total_amount) from Bill where Bill_Type='sale trading' and Status='1' and bill_date<'" + from+"' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='"+customer_vendor_id+"')";
                DataTable dt = gm.GetTable(query);
                double sales = 0;
                try
                {
                    sales = double.Parse(dt.Rows[0][0].ToString().Trim());
                }catch
                {}
                query = "select sum(Paid_Amount+Service_Charges) from Bill where Bill_Type='receiving' and Status='1' and bill_date<'" + from + "' and Customer_Vendor_Id='" + customer_vendor_id + "'";
                dt = gm.GetTable(query);
                double receivings = 0;
                try
                {
                    receivings = double.Parse(dt.Rows[0][0].ToString().Trim());
                }
                catch
                { }
                double remaining = sales - receivings;
                //remaining = remaining;
                if (remaining > 0)
                {
                    ledger.Rows.Add("", "سابقہ کھاتا", gm.removePoints((remaining).ToString()), "0");
                }
                else if (remaining < 0)
                {
                    ledger.Rows.Add("", "سابقہ کھاتا", "0", gm.removePoints((-remaining).ToString()));
                }
                else
                {
                    ledger.Rows.Add("", "سابقہ کھاتا", "0", "0");
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        public void getSaleTradingItemsBetweenDates(string customer_vendor_id)
        {
                try
                {
                    string from = metroDateTimeFrom.Value.Date.Year.ToString() + "-" + metroDateTimeFrom.Value.Date.Month.ToString() + "-" + metroDateTimeFrom.Value.Date.Day.ToString();
                    string to = metroDateTimeTo.Value.Date.Year.ToString() + "-" + metroDateTimeTo.Value.Date.Month.ToString() + "-" + metroDateTimeTo.Value.Date.Day.ToString();
                    bill_type = "Sale Trading";

                    DateTime dfrom = Convert.ToDateTime(from);
                    DateTime dto = Convert.ToDateTime(to);
                    while (dfrom.Date <= dto.Date)
                    {
                        string query = "select * from bill where status='1' and bill_type='" + bill_type + "' and bill_date='" + (dfrom.Date.Year.ToString() + "-" + dfrom.Date.Month.ToString() + "-" + dfrom.Date.Day.ToString()) + "'";
                        DataTable dt = gm.GetTable(query);
                        if (dt.Rows.Count > 0)
                        {
                            string dr = "0";
                            try
                            {
                                dr = getCustomerTodayBill(customer_vendor_id, (dfrom.Date.Year.ToString() + "-" + dfrom.Date.Month.ToString() + "-" + dfrom.Date.Day.ToString()), (dfrom.Date.Year.ToString() + "-" + dfrom.Date.Month.ToString() + "-" + dfrom.Date.Day.ToString()));
                                if (dr == "")
                                    dr = "0";
                            }
                            catch { }
                            string cr = "0";
                            try
                            {
                                if (cr == "")
                                    cr = "0";
                            }
                            catch { }
                            bill_type = "خریداری";
                            int zerovalue = 0;
                            try
                            {

                                if (double.Parse((dr == "" ? "0" : dr)) == 0 && double.Parse((cr == "" ? "0" : cr)) == 0)
                                {
                                    zerovalue = 1;
                                }
                            }
                            catch { }
                            if (zerovalue == 0)
                            {
                                ledger.Rows.Add((dfrom.Date.Year.ToString() + "-" + (dfrom.Date.Month.ToString().Length > 1 ? dfrom.Date.Month.ToString() : "0" + dfrom.Date.Month.ToString()) + "-" + (dfrom.Date.Day.ToString().Length > 1 ? dfrom.Date.Day.ToString() : "0" + dfrom.Date.Day.ToString())), bill_type, gm.removePoints(dr), gm.removePoints(cr));
                            }
                            bill_type = "Sale Trading";
                        }
                        dfrom = dfrom.Date.AddDays(1);

                    }
                    //MessageBox.Show("DFROM : " + dfrom.Date.Year.ToString() + "-" + dfrom.Date.Month.ToString() + "-" + dfrom.Date.Day.ToString());
                    //MessageBox.Show("DTO : " + dto.Date.Year.ToString() + "-" + dto.Date.Month.ToString() + "-" + dto.Date.Day.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        public void getPurchaseTradingItemsBetweenDates(string customer_vendor_id)
        {
            try
            {
                string from = metroDateTimeFrom.Value.Date.Year.ToString() + "-" + metroDateTimeFrom.Value.Date.Month.ToString() + "-" + metroDateTimeFrom.Value.Date.Day.ToString();
                string to = metroDateTimeTo.Value.Date.Year.ToString() + "-" + metroDateTimeTo.Value.Date.Month.ToString() + "-" + metroDateTimeTo.Value.Date.Day.ToString();
                bill_type = "Purchase Trading";
                string query = "Select * from bill where bill_type='" + bill_type + "' and bill_date>='"+from+"' and bill_Date<='"+to+"' and customer_vendor_id = '"+customer_vendor_id+"' and status = '1'";
                DataTable dt = gm.GetTable(query);
                foreach (DataRow d in dt.Rows)
                {
                    string dr = "0";
                    try
                    {
                        dr = d["Paid_Amount"].ToString().Trim();
                        if (dr == "")
                            dr = "0";
                    }
                    catch { }
                    string cr = "0";
                    try
                    {
                        cr = d["Total_Amount"].ToString().Trim();
                        if (cr == "")
                            cr = "0";
                    }
                    catch { }
                    string details = "";
                    query = "select * from bill_details where bill_id='"+d["id"].ToString()+"' and status='1'";
                    DataTable dt2 = gm.GetTable(query);
                    foreach (DataRow d2 in dt2.Rows)
                    {
                        string itemname = "";
                        query = "select * from items where id = '"+d2["item_id"].ToString()+"'";
                        DataTable dt3 = gm.GetTable(query);
                        try
                        {
                            itemname = dt3.Rows[0]["name"].ToString().Trim();
                        }catch{}
                        details += itemname + "\n(qty:" + d2["Qty"].ToString() + " rate:" + d2["Unit_Cost"].ToString() + ") ,\n";
                    }

                    ledger.Rows.Add(d["id"].ToString(), d["bill_date"].ToString(), bill_type, dr,cr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void getPurchaseTradingItems(string customer_vendor_id)
        {
            try
            {
                bill_type = "Purchase Trading";
                string query = "Select * from bill where bill_type='" + bill_type + "' and customer_vendor_id = '"+customer_vendor_id+"' and status = '1'";
                DataTable dt = gm.GetTable(query);
                foreach (DataRow d in dt.Rows)
                {
                    string dr = "0";
                    try
                    {
                        dr = d["Paid_Amount"].ToString().Trim();
                        if (dr == "")
                            dr = "0";
                    }
                    catch { }
                    string cr = "0";
                    try
                    {
                        cr = d["Total_Amount"].ToString().Trim();
                        if (cr == "")
                            cr = "0";
                    }
                    catch { }
                    string details = "";
                    query = "select * from bill_details where bill_id='"+d["id"].ToString()+"' and status='1'";
                    DataTable dt2 = gm.GetTable(query);
                    foreach (DataRow d2 in dt2.Rows)
                    {
                        string itemname = "";
                        query = "select * from items where id = '"+d2["item_id"].ToString()+"'";
                        DataTable dt3 = gm.GetTable(query);
                        try
                        {
                            itemname = dt3.Rows[0]["name"].ToString().Trim();
                        }catch{}
                        details += itemname + "\n(qty:" + d2["Qty"].ToString() + " rate:" + d2["Unit_Cost"].ToString() + ") ,\n";
                    }

                    ledger.Rows.Add(d["id"].ToString(), d["bill_date"].ToString() , bill_type, dr,cr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public string getCustomerTodayReceiving(string customer_id, string datefrom, string dateto)
        {
            try
            {
                string query = "select sum(Paid_Amount+Service_Charges) from Bill where status!='-1' and Bill_Type='Receiving' and Bill_Date>='" + datefrom + "' and Bill_Date<='" + dateto + "' and Customer_Vendor_Id='" + customer_id + "'";
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


        public string getCustomerTodayBill(string customer_id,string datefrom,string dateto)
        {
            try
            {
                string query = "select sum(Total_Amount) from Bill where status!='-1' and Bill_Type='sale trading' and Bill_Date>='" +datefrom + "' and Bill_Date<='" + dateto + "' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='" + customer_id + "')";
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

        public void getSaleTradingItems(string customer_vendor_id)
        {
            try
            {
                bill_type = "Sale Trading";

                string querydate = "select min(Bill_Date) from bill";
                DataTable dtdate = gm.GetTable(querydate);
                DateTime dfrom = Convert.ToDateTime(dtdate.Rows[0][0].ToString());
                querydate = "select max(Bill_Date) from bill";
                dtdate = gm.GetTable(querydate);
                DateTime dto = Convert.ToDateTime(dtdate.Rows[0][0].ToString());
                while (dfrom.Date <= dto.Date)
                {
                    string query = "select * from bill where status='1' and bill_type='" + bill_type + "' and bill_date='" + (dfrom.Date.Year.ToString() + "-" + dfrom.Date.Month.ToString() + "-" + dfrom.Date.Day.ToString()) + "'";
                    DataTable dt = gm.GetTable(query);
                    if (dt.Rows.Count > 0)
                    {
                        string dr = "0";
                        try
                        {
                            dr = getCustomerTodayBill(customer_vendor_id, (dfrom.Date.Year.ToString() + "-" + dfrom.Date.Month.ToString() + "-" + dfrom.Date.Day.ToString()), (dfrom.Date.Year.ToString() + "-" + dfrom.Date.Month.ToString() + "-" + dfrom.Date.Day.ToString()));
                            if (dr == "")
                                dr = "0";
                        }
                        catch { }
                        string cr = "0";
                        try
                        {
                            if (cr == "")
                                cr = "0";
                        }
                        catch { }
                        bill_type = "خریداری";
                        int zerovalue = 0;
                        try
                        {

                            if (double.Parse((dr == "" ? "0" : dr))==0 && double.Parse((cr == "" ? "0" : cr)) ==0)
                            {
                                zerovalue = 1;
                            }
                        }
                        catch { }
                        if (zerovalue == 0)
                        {
                            ledger.Rows.Add((dfrom.Date.Year.ToString() + "-" + (dfrom.Date.Month.ToString().Length > 1 ? dfrom.Date.Month.ToString() : "0" + dfrom.Date.Month.ToString()) + "-" + (dfrom.Date.Day.ToString().Length > 1 ? dfrom.Date.Day.ToString() : "0" + dfrom.Date.Day.ToString())), bill_type, gm.removePoints(dr), gm.removePoints(cr));
                        }
                        bill_type = "Sale Trading";
                    }
                    dfrom = dfrom.Date.AddDays(1);
                
                }
                //MessageBox.Show("DFROM : " + dfrom.Date.Year.ToString() + "-" + dfrom.Date.Month.ToString() + "-" + dfrom.Date.Day.ToString());
                //MessageBox.Show("DTO : " + dto.Date.Year.ToString() + "-" + dto.Date.Month.ToString() + "-" + dto.Date.Day.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void getPayemnts(string customer_vendor_id)
        {
            try
            {
                bill_type = "Payment";
                string query = "Select * from bill where bill_type='" + bill_type + "' and customer_vendor_id = '" + customer_vendor_id + "' and status = '1'";
                DataTable dt = gm.GetTable(query);
                foreach (DataRow d in dt.Rows)
                {
                    string dr = "0";
                    try
                    {
                        dr = d["Paid_Amount"].ToString().Trim();
                        if (dr == "")
                            dr = "0";
                    }
                    catch { }
                    string cr = "0";
                    try
                    {
                        //cr = d["Total_Amount"].ToString().Trim();
                        cr = "0";
                        if (cr == "")
                            cr = "0";
                    }
                    catch { }
                    string details = "";
                    query = "select * from bill_details where bill_id='" + d["id"].ToString() + "' and status='1'";
                    DataTable dt2 = gm.GetTable(query);
                    foreach (DataRow d2 in dt2.Rows)
                    {
                        string itemname = "";
                        query = "select * from items where id = '" + d2["item_id"].ToString() + "'";
                        DataTable dt3 = gm.GetTable(query);
                        try
                        {
                            itemname = dt3.Rows[0]["name"].ToString().Trim();
                        }
                        catch { }
                        details += itemname + "\n(qty:" + d2["Qty"].ToString() + " rate:" + d2["Unit_Cost"].ToString() + ") ,\n";
                    }

                    ledger.Rows.Add(d["id"].ToString(), d["bill_date"].ToString(), bill_type, dr, cr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void getPayemntsBetweenDates(string customer_vendor_id)
        {
            try
            {
                string from = metroDateTimeFrom.Value.Date.Year.ToString() + "-" + metroDateTimeFrom.Value.Date.Month.ToString() + "-" + metroDateTimeFrom.Value.Date.Day.ToString();
                string to = metroDateTimeTo.Value.Date.Year.ToString() + "-" + metroDateTimeTo.Value.Date.Month.ToString() + "-" + metroDateTimeTo.Value.Date.Day.ToString();
                bill_type = "Payment";
                string query = "Select * from bill where bill_type='" + bill_type + "' and customer_vendor_id = '" + customer_vendor_id + "' and status = '1'";
                DataTable dt = gm.GetTable(query);
                foreach (DataRow d in dt.Rows)
                {
                    string dr = "0";
                    try
                    {
                        dr = d["Paid_Amount"].ToString().Trim();
                        if (dr == "")
                            dr = "0";
                    }
                    catch { }
                    string cr = "0";
                    try
                    {
                        //cr = d["Total_Amount"].ToString().Trim();
                        cr = "0";
                        if (cr == "")
                            cr = "0";
                    }
                    catch { }
                    string details = "";
                    query = "select * from bill_details where bill_id='" + d["id"].ToString() + "' and status='1'";
                    DataTable dt2 = gm.GetTable(query);
                    foreach (DataRow d2 in dt2.Rows)
                    {
                        string itemname = "";
                        query = "select * from items where id = '" + d2["item_id"].ToString() + "'";
                        DataTable dt3 = gm.GetTable(query);
                        try
                        {
                            itemname = dt3.Rows[0]["name"].ToString().Trim();
                        }
                        catch { }
                        details += itemname + "\n(qty:" + d2["Qty"].ToString() + " rate:" + d2["Unit_Cost"].ToString() + ") ,\n";
                    }

                    ledger.Rows.Add(d["id"].ToString(), d["bill_date"].ToString(), bill_type, dr, cr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void getReceivings(string customer_vendor_id)
        {
            try
            {
                bill_type = "Receiving";

                string querydate = "select min(Bill_Date) from bill";
                DataTable dtdate = gm.GetTable(querydate);
                DateTime dfrom = Convert.ToDateTime(dtdate.Rows[0][0].ToString());
                querydate = "select max(Bill_Date) from bill";
                dtdate = gm.GetTable(querydate);
                DateTime dto = Convert.ToDateTime(dtdate.Rows[0][0].ToString());
                while (dfrom.Date <= dto.Date)
                {
                    string query = "select * from bill where status='1' and bill_type='"+bill_type+"' and bill_date='" + (dfrom.Date.Year.ToString() + "-" + dfrom.Date.Month.ToString() + "-" + dfrom.Date.Day.ToString()) + "'";
                    DataTable dt = gm.GetTable(query);
                    if (dt.Rows.Count > 0)
                    {
                        string dr = "0";
                        try
                        {
                            dr = "0";
                            if (dr == "")
                                dr = "0";
                        }
                        catch { }
                        string cr = "0";
                        try
                        {
                            cr = getCustomerTodayReceiving(customer_vendor_id, (dfrom.Date.Year.ToString() + "-" + dfrom.Date.Month.ToString() + "-" + dfrom.Date.Day.ToString()), (dfrom.Date.Year.ToString() + "-" + dfrom.Date.Month.ToString() + "-" + dfrom.Date.Day.ToString()));
                            if (cr == "")
                                cr = "0";
                        }
                        catch { }

                        bill_type = "وصولی";
                        int zerovalue = 0;
                        try
                        {

                            if (double.Parse((dr == "" ? "0" : dr))==0 && double.Parse((cr == "" ? "0" : cr)) ==0)
                            {
                                zerovalue = 1;
                            }
                        }
                        catch { }
                        if(zerovalue == 0)
                        {
                            ledger.Rows.Add((dfrom.Date.Year.ToString() + "-" + (dfrom.Date.Month.ToString().Length > 1 ? dfrom.Date.Month.ToString() : "0" + dfrom.Date.Month.ToString()) + "-" + (dfrom.Date.Day.ToString().Length > 1 ? dfrom.Date.Day.ToString() : "0" + dfrom.Date.Day.ToString())), bill_type, gm.removePoints(dr), gm.removePoints(cr));
                        }
                        bill_type = "Receiving";
                    }
                    dfrom = dfrom.Date.AddDays(1);

                }
                //MessageBox.Show("DFROM : " + dfrom.Date.Year.ToString() + "-" + dfrom.Date.Month.ToString() + "-" + dfrom.Date.Day.ToString());
                //MessageBox.Show("DTO : " + dto.Date.Year.ToString() + "-" + dto.Date.Month.ToString() + "-" + dto.Date.Day.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void getReceivingsBetweenDates(string customer_vendor_id)
        {
            try
            {
                string from = metroDateTimeFrom.Value.Date.Year.ToString() + "-" + metroDateTimeFrom.Value.Date.Month.ToString() + "-" + metroDateTimeFrom.Value.Date.Day.ToString();
                string to = metroDateTimeTo.Value.Date.Year.ToString() + "-" + metroDateTimeTo.Value.Date.Month.ToString() + "-" + metroDateTimeTo.Value.Date.Day.ToString();

                bill_type = "Receiving";

                DateTime dfrom = Convert.ToDateTime(from);
                DateTime dto = Convert.ToDateTime(to);
                while (dfrom.Date <= dto.Date)
                {
                    string query = "select * from bill where status='1' and bill_type='" + bill_type + "' and bill_date='" + (dfrom.Date.Year.ToString() + "-" + dfrom.Date.Month.ToString() + "-" + dfrom.Date.Day.ToString()) + "'";
                    DataTable dt = gm.GetTable(query);
                    if (dt.Rows.Count > 0)
                    {
                        string dr = "0";
                        try
                        {
                            dr = "0";
                            if (dr == "")
                                dr = "0";
                        }
                        catch { }
                        string cr = "0";
                        try
                        {
                            cr = getCustomerTodayReceiving(customer_vendor_id, (dfrom.Date.Year.ToString() + "-" + dfrom.Date.Month.ToString() + "-" + dfrom.Date.Day.ToString()), (dfrom.Date.Year.ToString() + "-" + dfrom.Date.Month.ToString() + "-" + dfrom.Date.Day.ToString()));
                            if (cr == "")
                                cr = "0";
                        }
                        catch { }
                        bill_type = "وصولی";
                        int zerovalue = 0;
                        try
                        {

                            if (double.Parse((dr == "" ? "0" : dr))==0 && double.Parse((cr == "" ? "0" : cr)) ==0)
                            {
                                zerovalue = 1;
                            }
                        }
                        catch { }
                        if (zerovalue == 0)
                        {
                            ledger.Rows.Add((dfrom.Date.Year.ToString() + "-" + (dfrom.Date.Month.ToString().Length > 1 ? dfrom.Date.Month.ToString() : "0" + dfrom.Date.Month.ToString()) + "-" + (dfrom.Date.Day.ToString().Length > 1 ? dfrom.Date.Day.ToString() : "0" + dfrom.Date.Day.ToString())), bill_type, gm.removePoints(dr), gm.removePoints(cr));
                        }
                        bill_type = "Receiving";
                    }
                    dfrom = dfrom.Date.AddDays(1);

                }
                //MessageBox.Show("DFROM : " + dfrom.Date.Year.ToString() + "-" + dfrom.Date.Month.ToString() + "-" + dfrom.Date.Day.ToString());
                //MessageBox.Show("DTO : " + dto.Date.Year.ToString() + "-" + dto.Date.Month.ToString() + "-" + dto.Date.Day.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        DataTable ledger = new DataTable();
        string ledgerof = "";
        private void metroTile2_Click(object sender, EventArgs e)
        {
            //payment ادائیگی
            //purchasing خریداری

            //receiving وصولی
            //selling فروخت

            try
            {
                lable_Tota_Dr.Text = "0";
                lable_Tota_Cr.Text = "0";
                lable_Balance.Text = "0";
                radio = 0;
                ledgerof = "";
                ledger = new DataTable();
                ledger.Columns.Add("Bill_Date");
                ledger.Columns.Add("Bill_Type");
                ledger.Columns.Add("Dr");
                ledger.Columns.Add("Cr");
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                if (dataGridView2.Rows.Count > 0)
                {
                    dataGridView2.Rows.Clear();
                }
                if (metroListView1.Items.Count > 0)
                {
                    metroListView1.Items.Clear();
                }

                //string[] s = textBox1.Text.Trim().Split('(');
                //string[] a = s[s.Length - 1].Trim().Split(')');
                //string id = a[0].ToString();
                string query = "";
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
                string user_id = id;
                if (id.Trim() != "")
                {
                    user_id = id.Trim();
                }
                if (textBox1.Text.Trim() == "")
                {
                    MessageBox.Show("Enter Name");
                    return;
                }
                if (user_id == "")
                {
                    MessageBox.Show("Invalid Name");
                    return;
                }

                if (metroRadioButton1.Checked == true)
                {
                    radio = 1;
                    //getPurchaseTradingItems(user_id);
                    getSaleTradingItems(user_id);
                    //getPayemnts(user_id);
                    getReceivings(user_id);
                }
                else if (metroRadioButton2.Checked == true)
                {
                    radio = 2;
                    //getPurchaseTradingItemsBetweenDates(user_id);
                    getPreviousBalance(user_id);
                    getSaleTradingItemsBetweenDates(user_id);
                    //getPayemntsBetweenDates(user_id);
                    getReceivingsBetweenDates(user_id);
                }
                else
                {
                    MessageBox.Show("Select View All or Between Dates Option");
                    return;
                }

                int srno = 0;
                foreach (DataRow d in ledger.Rows)
                {
                    srno ++;
                    dataGridView1.Rows.Add(d["Bill_Date"].ToString(), d["Bill_Type"].ToString(), d["Dr"].ToString(), d["Cr"].ToString(), "0");
                }
                dataGridView1.ClearSelection();
                //double balance1 = 0;
                //foreach (DataGridViewRow d in dataGridView1.Rows)
                //{
                //    double balance = 0;
                //    double dr = 0;
                //    double cr = 0;
                //    try
                //    {
                //        dr = double.Parse(d.Cells[2].Value.ToString().Trim());
                //    }
                //    catch { }
                //    try
                //    {
                //        cr = double.Parse(d.Cells[3].Value.ToString().Trim());
                //    }
                //    catch { }
                //    try
                //    {
                //        balance = double.Parse(d.Cells[4].Value.ToString().Trim());
                //    }
                //    catch { }
                //    try
                //    {
                //        balance = ((balance - dr) + cr);
                //        d.Cells[4].Value = gm.removePoints(balance.ToString());
                //    }
                //    catch { }

                //    //if (balance > 0)
                //    //{
                //    //    d.Cells[4].Value = balance.ToString() + " Cr";
                //    //}
                //    //else if (balance < 0)
                //    //{
                //    //    d.Cells[4].Value = (-balance).ToString() + " Dr";
                //    //}
                //    balance1 += balance;
                //}

                //double totalNoOfSales = 0;
                //double totalSalesQty = 0;
                //double totalSalesAmount = 0;
                //double totalNoOfPurchases = 0;
                //double totalPurchaseQty = 0;
                //double totalPurchaseAmount = 0;
                //double totalReceivedAmount = 0;
                //double totalPaidAmount = 0;
                //double totalBalanceReceivable = 0;
                //double totalBalancePayable = 0;
                //try
                //{





                //    string from = metroDateTimeFrom.Value.Date.Year.ToString() + "-" + metroDateTimeFrom.Value.Date.Month.ToString() + "-" + metroDateTimeFrom.Value.Date.Day.ToString();
                //    string to = metroDateTimeTo.Value.Date.Year.ToString() + "-" + metroDateTimeTo.Value.Date.Month.ToString() + "-" + metroDateTimeTo.Value.Date.Day.ToString();

                //    string datequery = " and bill_date>='" + from + "' and bill_Date<='" + to + "'";

                //    string query = "Select count(*) from bill where bill_type='" + "Sale Trading" + "' and customer_vendor_id = '" + user_id + "' and status = '1'"+(metroRadioButton2.Checked==true?datequery:"");
                //    DataTable dt = gm.GetTable(query);
                //    try
                //    {
                //        totalNoOfSales = double.Parse(dt.Rows[0][0].ToString().Trim());
                //    }
                //    catch { }

                //    query = "Select * from bill where bill_type='" + "Sale Trading" + "' and customer_vendor_id = '" + user_id + "' and status = '1'" + (metroRadioButton2.Checked == true ? datequery : "");
                //    dt = gm.GetTable(query);
                //    try
                //    {
                //        foreach (DataRow d in dt.Rows)
                //        {
                //            query = "Select sum(cast(qty as decimal(18,3))) from bill_details where bill_id='" + d["id"].ToString() + "'";
                //            DataTable dt2 = gm.GetTable(query);
                //            totalSalesQty += double.Parse(dt2.Rows[0][0].ToString().Trim());
                //        }
                //    }
                //    catch { }

                //    query = "Select sum(cast(total_amount as decimal(18,3))) from bill where bill_type='" + "Sale Trading" + "' and customer_vendor_id = '" + user_id + "' and status = '1'" + (metroRadioButton2.Checked == true ? datequery : "");
                //    dt = gm.GetTable(query);
                //    try
                //    {
                //        totalSalesAmount = double.Parse(dt.Rows[0][0].ToString().Trim());
                //    }
                //    catch { }

                //    query = "Select count(*) from bill where bill_type='" + "Purchase Trading" + "' and customer_vendor_id = '" + user_id + "' and status = '1'" + (metroRadioButton2.Checked == true ? datequery : "");
                //    dt = gm.GetTable(query);
                //    try
                //    {
                //        totalNoOfPurchases = double.Parse(dt.Rows[0][0].ToString().Trim());
                //    }
                //    catch { }

                //    query = "Select * from bill where bill_type='" + "Purchase Trading" + "' and customer_vendor_id = '" + user_id + "' and status = '1'" + (metroRadioButton2.Checked == true ? datequery : "");
                //    dt = gm.GetTable(query);
                //    try
                //    {
                //        foreach (DataRow d in dt.Rows)
                //        {
                //            query = "Select sum(cast(qty as decimal(18,3))) from bill_details where bill_id='" + d["id"].ToString() + "'";
                //            DataTable dt2 = gm.GetTable(query);
                //            totalPurchaseQty += double.Parse(dt2.Rows[0][0].ToString().Trim());
                //        }
                //    }
                //    catch { }

                //    query = "Select sum(cast(total_amount as decimal(18,3))) from bill where bill_type='" + "Purchase Trading" + "' and customer_vendor_id = '" + user_id + "' and status = '1'" + (metroRadioButton2.Checked == true ? datequery : "");
                //    dt = gm.GetTable(query);
                //    try
                //    {
                //        totalPurchaseAmount = double.Parse(dt.Rows[0][0].ToString().Trim());
                //    }
                //    catch { }

                //    query = "Select sum(cast(paid_amount as decimal(18,3))) from bill where (bill_type='" + "Purchase Trading" + "' or bill_type='" + "Payment" + "') and customer_vendor_id = '" + user_id + "' and status = '1'" + (metroRadioButton2.Checked == true ? datequery : "");
                //    dt = gm.GetTable(query);
                //    try
                //    {
                //        totalPaidAmount = double.Parse(dt.Rows[0][0].ToString().Trim());
                //    }
                //    catch { }

                //    query = "Select sum(cast(paid_amount as decimal(18,3))) from bill where (bill_type='" + "Sale Trading" + "' or bill_type='" + "Receiving" + "') and customer_vendor_id = '" + user_id + "' and status = '1'" + (metroRadioButton2.Checked == true ? datequery : "");
                //    dt = gm.GetTable(query);
                //    try
                //    {
                //        totalReceivedAmount = double.Parse(dt.Rows[0][0].ToString().Trim());
                //    }
                //    catch { }
                //    if (balance1 < 0)
                //    {
                //        totalBalanceReceivable = (-balance1);
                //    }
                //    else if (balance1 > 0)
                //    {
                //        totalBalancePayable = (balance1);
                //    }
                //    ledgerof = metroTextBox1.Text.Trim();
                //}
                //catch { }

                //dataGridView2.Rows.Add("Total No Sales", totalNoOfSales.ToString());
                //dataGridView2.Rows.Add("Total Sales Quantity", totalSalesQty.ToString());
                //dataGridView2.Rows.Add("Total Sales Amount", totalSalesAmount.ToString());
                //dataGridView2.Rows.Add("Total No Purchases", totalNoOfPurchases.ToString());
                //dataGridView2.Rows.Add("Total Purchases Quantity", totalPurchaseQty.ToString());
                //dataGridView2.Rows.Add("Total Purchases Amount", totalPurchaseAmount.ToString());
                //dataGridView2.Rows.Add("Total Paid Amount", totalPaidAmount.ToString());
                //dataGridView2.Rows.Add("Total Received Amount", totalReceivedAmount.ToString());
                //dataGridView2.Rows.Add("Total Balance Payable", totalBalancePayable.ToString());
                //dataGridView2.Rows.Add("Total Balance Receivable", totalBalanceReceivable.ToString());

                //for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
                //{
                //    double debit = 0;
                //    try
                //    {
                //        debit = double.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString().Trim());
                //    }
                //    catch { }
                //    double credit = 0;
                //    try
                //    {
                //        credit = double.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString().Trim());
                //    }
                //    catch { }
                //    double previousBalance = 0;
                //    try
                //    {
                //        previousBalance = (double.Parse(dataGridView1.Rows[i - 1].Cells[4].Value.ToString().Trim().Split(' ')[0].ToString().Trim()));
                //        //if (dataGridView1.Rows[i - 1].Cells[4].Value.ToString().Trim().Contains("Dr"))
                //        //{
                //        //    previousBalance = -(double.Parse(dataGridView1.Rows[i - 1].Cells[4].Value.ToString().Trim().Split(' ')[0].ToString().Trim()));
                //        //}
                //    }
                //    catch { }
                //    double newBalance = (previousBalance + debit) - credit;
                //    string drOrCr = "";
                //    //if (newBalance > 0)
                //    //    drOrCr = " Cr";
                //    //else if (newBalance < 0)
                //    //{
                //    //    newBalance = -newBalance;
                //    //    drOrCr = " Dr";
                //    //}
                //    dataGridView1.Rows[i].Cells[4].Value = gm.removePoints(newBalance.ToString()) + drOrCr;
                //}
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
            //dataGridView1.Sort(dataGridView1.Columns[0], System.ComponentModel.ListSortDirection.Descending);
            dataGridView1.Sort(dataGridView1.Columns[0], System.ComponentModel.ListSortDirection.Ascending);
            int row1 = 0;
            foreach (DataGridViewRow d in dataGridView1.Rows)
            {
                if (metroRadioButton2.Checked == true && row1 != 0)
                {
                    DateTime dtm = Convert.ToDateTime(d.Cells[0].Value.ToString());
                    d.Cells[0].Value = (dtm.Date.Day.ToString().Length > 1 ? dtm.Date.Day.ToString() : "0" + dtm.Date.Day.ToString()) + "-" + (dtm.Date.Month.ToString().Length > 1 ? dtm.Date.Month.ToString() : "0" + dtm.Date.Month.ToString()) + "-" + dtm.Date.Year.ToString();
                }
                else if(metroRadioButton1.Checked==true)
                {
                    DateTime dtm = Convert.ToDateTime(d.Cells[0].Value.ToString());
                    d.Cells[0].Value = (dtm.Date.Day.ToString().Length > 1 ? dtm.Date.Day.ToString() : "0" + dtm.Date.Day.ToString()) + "-" + (dtm.Date.Month.ToString().Length > 1 ? dtm.Date.Month.ToString() : "0" + dtm.Date.Month.ToString()) + "-" + dtm.Date.Year.ToString();
                }
                row1++;
            }
            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    double debit = 0;
                    try
                    {
                        debit = double.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString().Trim());
                    }
                    catch { }
                    double credit = 0;
                    try
                    {
                        credit = double.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString().Trim());
                    }
                    catch { }
                    double previousBalance = 0;
                    try
                    {
                        previousBalance = (double.Parse(dataGridView1.Rows[i - 1].Cells[4].Value.ToString().Trim().Split(' ')[0].ToString().Trim()));
                        //if (dataGridView1.Rows[i - 1].Cells[4].Value.ToString().Trim().Contains("Dr"))
                        //{
                        //    previousBalance = -(double.Parse(dataGridView1.Rows[i - 1].Cells[4].Value.ToString().Trim().Split(' ')[0].ToString().Trim()));
                        //}
                    }
                    catch { }
                    double newBalance = (previousBalance + debit) - credit;
                    string drOrCr = "";
                    //if (newBalance > 0)
                    //    drOrCr = " Cr";
                    //else if (newBalance < 0)
                    //{
                    //    newBalance = -newBalance;
                    //    drOrCr = " Dr";
                    //}
                    dataGridView1.Rows[i].Cells[4].Value = gm.removePoints(newBalance.ToString()) + drOrCr;
                }
            }
            catch { }
            try
            {
                double total_dr = 0;
                double total_cr = 0;
                double balance = 0;
                try
                {
                    foreach (DataGridViewRow d in dataGridView1.Rows)
                    {
                        try
                        {
                            total_dr += double.Parse(d.Cells[2].Value.ToString());
                            if (d.Cells[2].Value.ToString().Trim() == "0")
                            {
                                d.Cells[2].Value = "";
                            }
                        }
                        catch { }
                        try
                        {
                            total_cr += double.Parse(d.Cells[3].Value.ToString());
                            if (d.Cells[3].Value.ToString().Trim() == "0")
                            {
                                d.Cells[3].Value = "";
                            }
                        }
                        catch { }
                        try
                        {
                            balance += double.Parse(d.Cells[4].Value.ToString());
                            if (d.Cells[4].Value.ToString().Trim() == "0")
                            {
                                d.Cells[4].Value = "";
                            }
                        }
                        catch { }
                        //try//change sign of balance
                        //{
                        //    balance = -double.Parse(d.Cells[4].Value.ToString());
                        //    d.Cells[4].Value = balance.ToString();
                        //}
                        //catch { }
                    }
                    lable_Tota_Dr.Text = gm.removePoints(total_dr.ToString());
                    lable_Tota_Cr.Text = gm.removePoints(total_cr.ToString());
                    lable_Balance.Text = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[4].Value.ToString();// gm.removePoints(balance.ToString());
                }
                catch { }
            }
            catch { }
        }

        private void dataGridView1_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {

        }
    }
}
