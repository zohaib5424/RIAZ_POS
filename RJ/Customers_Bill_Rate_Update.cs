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
    public partial class Customers_Bill_Rate_Update : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public Customers_Bill_Rate_Update()
        {
            InitializeComponent();
        }

        protected void treeView1_AfterSelect(object sender, EventArgs e)//usercontrol->itemcategory work ref 1
        {
            try
            {
                if (treeView1.SelectedNode != null)
                {
                    if (treeView1.SelectedNode.Level > 0)
                    {
                        if (treeView1.SelectedNode.Nodes.Count == 0)
                        {
                            getCategoryRecord(treeView1.SelectedNode.Tag.ToString());
                            metroTextBoxCategory.Text = treeView1.SelectedNode.Text.Trim().ToString() + " (" + treeView1.SelectedNode.Tag.ToString() + ")";
                            metroTextBoxCategory.Tag = treeView1.SelectedNode.Tag;
                        }
                    }
                }
                else
                    MessageBox.Show("Select Category");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        TreeView treeView1;

        AutoCompleteStringCollection source = new AutoCompleteStringCollection();
        AutoCompleteStringCollection sourceitemname = new AutoCompleteStringCollection();
        AutoCompleteStringCollection main_sourceCustomer_Or_Vendor = new AutoCompleteStringCollection();
        private void ItemsList_Load(object sender, EventArgs e)
        {
            progressBar1.Hide();
            try
            {
                metroDateTime_From.Value = DateTime.Now.Date;
                metroDateTime_To.Value = DateTime.Now.Date;
                metroDateTime_From.MaxDate = metroDateTime_To.Value;
                metroDateTime_To.MinDate = metroDateTime_From.Value;
            }
            catch { }
            metroTile1.Hide();
            metroTile2.Visible = false;
            label1.Visible = false;
            metroDateTimeFrom.Hide();
            groupBox2.Hide();
            metroLabel8.Hide();

        }

        void collectChildren(TreeNode node)
        {
            if (node.Nodes.Count == 0) source.Add(node.Text + " (" + node.Tag.ToString() + ")");
            else foreach (TreeNode n in node.Nodes) collectChildren(n);
        }

        GMDB gm = new GMDB();
        public void getAllRecord()
        {
            chng = true;
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                string query = "";
                //if (metroTextBox4.Text.ToString().Trim().Length > 0 && metroTextBox4.Text.Contains('(') && metroTextBox4.Text.Contains(')'))
                {
                    string todaydate = "";
                    try
                    {
                        todaydate = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
                    }
                    catch { }
                    //query = "select distinct Item_Id,update_status from Bill_Details where item_type=N'نارمل' and Bill_Id in (select id from Bill where Bill_Date='" + todaydate + "')";
                    query = "select distinct Item_Id,update_status,bill_id from Bill_Details where (item_type=N'نارمل' or item_type=N'سابقہ') and Bill_Id in (select id from Bill where Bill_Date='" + todaydate + "' and status='1')";
                    //MessageBox.Show(customerid);
                    DataTable dtBillItems = gm.GetTable(query);
                    if (dtBillItems.Rows.Count <= 0)
                    {
                        return;
                    }
                    List<string> items_id = new List<string>();
                    foreach (DataRow dbillitems in dtBillItems.Rows)
                    {
                        string bill_id = dbillitems["bill_id"].ToString();
                        query = "select * from Customer_Or_Vendor where status='1' and id in (select parent from Customer_Or_Vendor where status='1' and id in (select Customer_Vendor_Id from Bill where id='" + bill_id + "') )";
                        DataTable dt = gm.GetTable(query);
                        if (dt.Rows.Count > 0)
                        {
                            string customer_vendor_id = dt.Rows[0][0].ToString();
                            //MessageBox.Show(customer_vendor_id);
                            int exist = 0;
                            foreach (string s in items_id)
                            {
                                if (s.Trim() == dbillitems[0].ToString().Trim())
                                {
                                    exist = 1;
                                }
                            }
                            if (exist == 0)
                            {
                                items_id.Add(dbillitems[0].ToString());
                                query = "Select * from items where id='" + dbillitems[0].ToString() + "' and status != '-1' order by Name ASC";
                                dt = gm.GetTable(query);
                                foreach (DataRow d in dt.Rows)
                                {
                                    string item = d["id"].ToString();
                                    query = "Select * from categories where id='" + d["category_id"].ToString() + "'";
                                    DataTable dtcategory = gm.GetTable(query);
                                    string category = dtcategory.Rows[0][2].ToString();
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
                                    double qty = 0;
                                    try
                                    {
                                        string itemid = d["id"].ToString();
                                        string date = metroDateTimeFrom.Value.Year.ToString() + "-" + (metroDateTimeFrom.Value.Month.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Month.ToString()) : metroDateTimeFrom.Value.Month.ToString()) + "-" + (metroDateTimeFrom.Value.Day.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Day.ToString()) : metroDateTimeFrom.Value.Day.ToString());
                                        //query = "select sum(qty) from Bill_Details where item_id='" + itemid + "' and (item_type=N'نارمل' or item_type=N'بل میں') and Bill_Id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "')";
                                        //query = "select sum(qty) from Bill_Details where item_id='" + itemid + "' and (item_type=N'نارمل' or item_type=N'بل میں') and Bill_Id in (select bill.id from Bill inner join Customer_Or_Vendor on Bill.Customer_Vendor_Id=Customer_Or_Vendor.id where bill.Bill_Date='"+date+"' and Customer_Or_Vendor.status='1' and Customer_Or_Vendor.id in (select id from Customer_Or_Vendor where status='1' and parent in (select id from Customer_Or_Vendor where status='1' and id in(select parent from Customer_Or_Vendor where id in (select Customer_Vendor_Id from Bill where id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "' and Bill_Type='sale trading'))))))";
                                        //for also bill_mein//query = "select sum(qty) from Bill_Details where item_id='" + itemid + "' and (item_type=N'نارمل' or item_type=N'بل میں') and Bill_Id in (select bill.id from Customer_Or_Vendor as customers1 inner join Customer_Or_Vendor as customers2 on customers1.id=customers2.parent inner join bill on bill.Customer_Vendor_Id=customers2.id where customers1.status='1' and customers2.status='1' and Bill.Status!='-1' and bill.Bill_Type='sale trading' and bill.Bill_Date='" + date + "')";
                                        query = "select sum(qty) from Bill_Details where item_id='" + itemid + "' and (item_type=N'نارمل' or item_type=N'سابقہ') and Bill_Id in (select bill.id from Customer_Or_Vendor as customers1 inner join Customer_Or_Vendor as customers2 on customers1.id=customers2.parent inner join bill on bill.Customer_Vendor_Id=customers2.id where customers1.status='1' and customers2.status='1' and Bill.Status!='-1' and bill.Bill_Type='sale trading' and bill.Bill_Date='" + date + "')";
                                        DataTable dt2 = gm.GetTable(query);
                                        try
                                        {
                                            qty = double.Parse(dt2.Rows[0][0].ToString());
                                        }
                                        catch { }
                                    }
                                    catch { }
                                    if (d["status"].ToString() == "1")
                                    {
                                        dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), d["weight"].ToString(), brand, unit, int.Parse(gm.removePointsZero(gm.removePoints(Math.Round(double.Parse(dbillitems["unit_cost"].ToString()), 0, MidpointRounding.AwayFromZero).ToString()))), int.Parse(gm.removePointsZero(gm.removePoints(Math.Round(double.Parse(d["Mazdori"].ToString()), 0, MidpointRounding.AwayFromZero).ToString()))), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Disable", "Delete", "0", "", "", "");
                                    }
                                    else
                                    {
                                        dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), d["weight"].ToString(), brand, unit, int.Parse(gm.removePointsZero(gm.removePoints(Math.Round(double.Parse(dbillitems["unit_cost"].ToString()), 0, MidpointRounding.AwayFromZero).ToString()))), int.Parse(gm.removePointsZero(gm.removePoints(Math.Round(double.Parse(d["Mazdori"].ToString()), 0, MidpointRounding.AwayFromZero).ToString()))), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Enable", "Delete", "0", "", "", "");
                                    }
                                    if (dbillitems["update_status"].ToString() == "1")
                                    {
                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Green;
                                    }
                                    if (d["status"].ToString() == "0")
                                    {
                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                    }
                                }
                            }
                        }
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
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    try
                    {
                        row.Cells[14].Value = gm.removePointsZero(gm.removePoints(Math.Round(double.Parse((double.Parse(row.Cells[14].Value.ToString()) + (double.Parse(row.Cells[4].Value.ToString()) * double.Parse(row.Cells[9].Value.ToString()))).ToString()), 0, MidpointRounding.AwayFromZero).ToString()));
                    }
                    catch { }
                }
            }
            catch { }
            chng = false;

        }

        public void getAllRecordSelectedCustomer()
        {
            chng = true;
            string customerquery = "(";
            for (int i = 0; i < selectedmaincustomers.Count - 1; i++)
            {
                if (i == 0)
                {
                    customerquery += " customer_or_vendor_name=N'" + selectedmaincustomers[i].ToString() + "'";
                }
                else
                {
                    customerquery += " or customer_or_vendor_name=N'" + selectedmaincustomers[i].ToString() + "'";
                }
            }
            if (selectedmaincustomers.Count > 1)
            {
                customerquery += " or customer_or_vendor_name=N'" + selectedmaincustomers[selectedmaincustomers.Count - 1].ToString() + "'";
            }
            else
            {
                customerquery += " customer_or_vendor_name=N'" + selectedmaincustomers[selectedmaincustomers.Count - 1].ToString() + "'";
            }
            customerquery += ")";
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                string query = "";
                //if (metroTextBox4.Text.ToString().Trim().Length > 0 && metroTextBox4.Text.Contains('(') && metroTextBox4.Text.Contains(')'))
                {
                    string todaydate = "";
                    try
                    {
                        todaydate = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
                    }
                    catch { }
                    //query = "select distinct Item_Id,update_status from Bill_Details where item_type=N'نارمل' and Bill_Id in (select id from Bill where Bill_Date='" + todaydate + "')";
                    query = "select distinct Item_Id,update_status,bill_id,unit_cost from Bill_Details where (item_type=N'نارمل' or item_type=N'سابقہ') and Bill_Id in (select id from Bill where Bill_Date='" + todaydate + "' and status='1' and customer_vendor_id in (select id from customer_or_vendor where parent in (select id from customer_or_vendor where " + customerquery + ")))";
                    //MessageBox.Show(customerid);
                    DataTable dtBillItems = gm.GetTable(query);
                    if (dtBillItems.Rows.Count <= 0)
                    {
                        return;
                    }
                    List<string> items_id = new List<string>();
                    foreach (DataRow dbillitems in dtBillItems.Rows)
                    {
                        string bill_id = dbillitems["bill_id"].ToString();
                        query = "select * from Customer_Or_Vendor where status='1' and id in (select parent from Customer_Or_Vendor where status='1' and id in (select Customer_Vendor_Id from Bill where id='" + bill_id + "') )";
                        DataTable dt = gm.GetTable(query);
                        if (dt.Rows.Count > 0)
                        {
                            string customer_vendor_id = dt.Rows[0][0].ToString();
                            //MessageBox.Show(customer_vendor_id);
                            int exist = 0;
                            foreach (string s in items_id)
                            {
                                if (s.Trim() == dbillitems[0].ToString().Trim())
                                {
                                    exist = 1;
                                }
                            }
                            if (exist == 0)
                            {
                                items_id.Add(dbillitems[0].ToString());
                                query = "Select * from items where id='" + dbillitems[0].ToString() + "' and status != '-1' order by Name ASC";
                                dt = gm.GetTable(query);
                                foreach (DataRow d in dt.Rows)
                                {
                                    string item = d["id"].ToString();
                                    query = "Select * from categories where id='" + d["category_id"].ToString() + "'";
                                    DataTable dtcategory = gm.GetTable(query);
                                    string category = dtcategory.Rows[0][2].ToString();
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
                                    double qty = 0;
                                    try
                                    {
                                        string itemid = d["id"].ToString();
                                        string date = metroDateTimeFrom.Value.Year.ToString() + "-" + (metroDateTimeFrom.Value.Month.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Month.ToString()) : metroDateTimeFrom.Value.Month.ToString()) + "-" + (metroDateTimeFrom.Value.Day.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Day.ToString()) : metroDateTimeFrom.Value.Day.ToString());
                                        //query = "select sum(qty) from Bill_Details where item_id='" + itemid + "' and (item_type=N'نارمل' or item_type=N'بل میں') and Bill_Id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "')";
                                        //query = "select sum(qty) from Bill_Details where item_id='" + itemid + "' and (item_type=N'نارمل' or item_type=N'بل میں') and Bill_Id in (select bill.id from Bill inner join Customer_Or_Vendor on Bill.Customer_Vendor_Id=Customer_Or_Vendor.id where bill.Bill_Date='"+date+"' and Customer_Or_Vendor.status='1' and Customer_Or_Vendor.id in (select id from Customer_Or_Vendor where status='1' and parent in (select id from Customer_Or_Vendor where status='1' and id in(select parent from Customer_Or_Vendor where id in (select Customer_Vendor_Id from Bill where id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "' and Bill_Type='sale trading'))))))";
                                        //for also bill_mein//query = "select sum(qty) from Bill_Details where item_id='" + itemid + "' and (item_type=N'نارمل' or item_type=N'بل میں') and Bill_Id in (select bill.id from Customer_Or_Vendor as customers1 inner join Customer_Or_Vendor as customers2 on customers1.id=customers2.parent inner join bill on bill.Customer_Vendor_Id=customers2.id where customers1.status='1' and customers2.status='1' and Bill.Status!='-1' and bill.Bill_Type='sale trading' and bill.Bill_Date='" + date + "')";
                                        query = "select sum(qty) from Bill_Details where item_id='" + itemid + "' and (item_type=N'نارمل' or item_type=N'سابقہ') and Bill_Id in (select bill.id from Customer_Or_Vendor as customers1 inner join Customer_Or_Vendor as customers2 on customers1.id=customers2.parent inner join bill on bill.Customer_Vendor_Id=customers2.id where customers1.status='1' and customers2.status='1' and Bill.Status!='-1' and bill.Bill_Type='sale trading' and bill.Bill_Date='" + date + "' and bill.customer_vendor_id in (select id from customer_or_vendor where parent in (select id from customer_or_vendor where " + customerquery + ")))";
                                        DataTable dt2 = gm.GetTable(query);
                                        try
                                        {
                                            qty = double.Parse(dt2.Rows[0][0].ToString());
                                        }
                                        catch { }
                                    }
                                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                                    if (d["status"].ToString() == "1")
                                    {
                                        dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), d["weight"].ToString(), brand, unit, int.Parse(gm.removePointsZero(gm.removePoints(Math.Round(double.Parse(dbillitems["unit_cost"].ToString()), 0, MidpointRounding.AwayFromZero).ToString()))), int.Parse(gm.removePointsZero(gm.removePoints(Math.Round(double.Parse(d["Mazdori"].ToString()), 0, MidpointRounding.AwayFromZero).ToString()))), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Disable", "Delete", "0", "", "", "");
                                    }
                                    else
                                    {
                                        dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), d["weight"].ToString(), brand, unit, int.Parse(gm.removePointsZero(gm.removePoints(Math.Round(double.Parse(dbillitems["unit_cost"].ToString()), 0, MidpointRounding.AwayFromZero).ToString()))), int.Parse(gm.removePointsZero(gm.removePoints(Math.Round(double.Parse(d["Mazdori"].ToString()), 0, MidpointRounding.AwayFromZero).ToString()))), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Enable", "Delete", "0", "", "", "");
                                    }
                                    if (dbillitems["update_status"].ToString() == "1")
                                    {
                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Green;
                                    }
                                    if (d["status"].ToString() == "0")
                                    {
                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                    }
                                }
                            }
                        }
                    }
                }
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    try
                    {
                        row.Cells[14].Value = gm.removePointsZero(gm.removePoints(Math.Round(double.Parse((double.Parse(row.Cells[14].Value.ToString()) + (double.Parse(row.Cells[4].Value.ToString()) * double.Parse(row.Cells[9].Value.ToString()))).ToString()), 0, MidpointRounding.AwayFromZero).ToString()));
                    }
                    catch { }
                }
            }
            catch { }
            chng = false;

        }

        public void getAllRecordBetweenDates()
        {
            chng = true;
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                string query = "";
                //if (metroTextBox4.Text.ToString().Trim().Length > 0 && metroTextBox4.Text.Contains('(') && metroTextBox4.Text.Contains(')'))
                if (textBox4.Text.ToString().Trim().Length > 0)
                {
                    string customerid = "";
                    try
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
                    catch { }

                    string datefrom = "";
                    string dateto = "";
                    string todaydate = "";
                    try
                    {
                        todaydate = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
                    }
                    catch { }
                    try
                    {
                        datefrom = metroDateTime_From.Value.Date.Year.ToString() + "-" + metroDateTime_From.Value.Date.Month.ToString() + "-" + metroDateTime_From.Value.Date.Day.ToString();
                    }
                    catch { }
                    try
                    {
                        dateto = metroDateTime_To.Value.Date.Year.ToString() + "-" + metroDateTime_To.Value.Date.Month.ToString() + "-" + metroDateTime_To.Value.Date.Day.ToString();
                    }
                    catch { }
                    //query = "select distinct Item_Id,update_status from Bill_Details where item_type=N'نارمل' and Bill_Id in (select id from Bill where Bill_Date='" + todaydate + "')";
                    query = "select distinct Item_Id,update_status,bill_id,unit_cost from Bill_Details where (item_type=N'نارمل' or item_type=N'سابقہ') and Bill_Id in (select id from Bill where Bill_Date>='" + datefrom + "' and Bill_Date<='" + dateto + "' and status='1' and Customer_Vendor_Id in ( select id from Customer_Or_Vendor where status='1' and parent='" + customerid + "'))";
                    //MessageBox.Show(customerid);
                    DataTable dtBillItems = gm.GetTable(query);
                    if (dtBillItems.Rows.Count <= 0)
                    {
                        return;
                    }
                    List<string> items_id = new List<string>();
                    foreach (DataRow dbillitems in dtBillItems.Rows)
                    {
                        string bill_id = dbillitems["bill_id"].ToString();
                        query = "select * from Customer_Or_Vendor where status='1' and id in (select parent from Customer_Or_Vendor where status='1' and id in (select Customer_Vendor_Id from Bill where id='" + bill_id + "') )";
                        DataTable dt = gm.GetTable(query);
                        if (dt.Rows.Count > 0)
                        {
                            string customer_vendor_id = dt.Rows[0][0].ToString();
                            //MessageBox.Show(customer_vendor_id);
                            int exist = 0;
                            foreach (string s in items_id)
                            {
                                if (s.Trim() == dbillitems[0].ToString().Trim())
                                {
                                    exist = 1;
                                }
                            }
                            if (exist == 0)
                            {
                                items_id.Add(dbillitems[0].ToString());
                                query = "Select * from items where id='" + dbillitems[0].ToString() + "' and status != '-1' order by Name ASC";
                                dt = gm.GetTable(query);
                                foreach (DataRow d in dt.Rows)
                                {
                                    string item = d["id"].ToString();
                                    query = "Select * from categories where id='" + d["category_id"].ToString() + "'";
                                    DataTable dtcategory = gm.GetTable(query);
                                    string category = dtcategory.Rows[0][2].ToString();
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
                                    double qty = 0;
                                    try
                                    {
                                        string itemid = d["id"].ToString();
                                        string date = metroDateTimeFrom.Value.Year.ToString() + "-" + (metroDateTimeFrom.Value.Month.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Month.ToString()) : metroDateTimeFrom.Value.Month.ToString()) + "-" + (metroDateTimeFrom.Value.Day.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Day.ToString()) : metroDateTimeFrom.Value.Day.ToString());
                                        //query = "select sum(qty) from Bill_Details where item_id='" + itemid + "' and (item_type=N'نارمل' or item_type=N'بل میں') and Bill_Id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "')";
                                        //query = "select sum(qty) from Bill_Details where item_id='" + itemid + "' and (item_type=N'نارمل' or item_type=N'بل میں') and Bill_Id in (select bill.id from Bill inner join Customer_Or_Vendor on Bill.Customer_Vendor_Id=Customer_Or_Vendor.id where bill.Bill_Date='"+date+"' and Customer_Or_Vendor.status='1' and Customer_Or_Vendor.id in (select id from Customer_Or_Vendor where status='1' and parent in (select id from Customer_Or_Vendor where status='1' and id in(select parent from Customer_Or_Vendor where id in (select Customer_Vendor_Id from Bill where id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "' and Bill_Type='sale trading'))))))";
                                        //for also bill_mein//query = "select sum(qty) from Bill_Details where item_id='" + itemid + "' and (item_type=N'نارمل' or item_type=N'بل میں') and Bill_Id in (select bill.id from Customer_Or_Vendor as customers1 inner join Customer_Or_Vendor as customers2 on customers1.id=customers2.parent inner join bill on bill.Customer_Vendor_Id=customers2.id where customers1.status='1' and customers2.status='1' and Bill.Status!='-1' and bill.Bill_Type='sale trading' and bill.Bill_Date='" + date + "')";
                                        query = "select sum(qty) from Bill_Details where item_id='" + itemid + "' and (item_type=N'نارمل' or item_type=N'سابقہ') and Bill_Id in (select bill.id from Customer_Or_Vendor as customers1 inner join Customer_Or_Vendor as customers2 on customers1.id=customers2.parent inner join bill on bill.Customer_Vendor_Id=customers2.id where customers1.status='1' and customers2.status='1' and Bill.Status!='-1' and bill.Bill_Type='sale trading' and bill.Bill_Date>='" + datefrom + "' and bill.Bill_Date<='" + dateto + "' and Customer_Vendor_Id in ( select id from Customer_Or_Vendor where status='1' and parent='" + customerid + "'))";
                                        DataTable dt2 = gm.GetTable(query);
                                        try
                                        {
                                            qty = double.Parse(dt2.Rows[0][0].ToString());
                                        }
                                        catch { }
                                    }
                                    catch { }
                                    if (d["status"].ToString() == "1")
                                    {
                                        dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), d["weight"].ToString(), brand, unit, int.Parse(gm.removePointsZero(gm.removePoints(Math.Round(double.Parse(dbillitems["unit_cost"].ToString()), 0, MidpointRounding.AwayFromZero).ToString()))), int.Parse(gm.removePointsZero(gm.removePoints(Math.Round(double.Parse(d["Mazdori"].ToString()), 0, MidpointRounding.AwayFromZero).ToString()))), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Disable", "Delete", "0", "", "", "");
                                    }
                                    else
                                    {
                                        dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), d["weight"].ToString(), brand, unit, int.Parse(gm.removePointsZero(gm.removePoints(Math.Round(double.Parse(dbillitems["unit_cost"].ToString()), 0, MidpointRounding.AwayFromZero).ToString()))), int.Parse(gm.removePointsZero(gm.removePoints(Math.Round(double.Parse(d["Mazdori"].ToString()), 0, MidpointRounding.AwayFromZero).ToString()))), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Enable", "Delete", "0", "", "", "");
                                    }
                                    //if (dbillitems["update_status"].ToString() == "1")
                                    //{
                                    //    dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Green;
                                    //}
                                    if (d["status"].ToString() == "0")
                                    {
                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                    }
                                }
                            }
                        }
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
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    try
                    {
                        row.Cells[14].Value = (double.Parse(row.Cells[14].Value.ToString()) + (double.Parse(row.Cells[4].Value.ToString()) * double.Parse(row.Cells[9].Value.ToString()))).ToString();
                    }
                    catch { }
                }
            }
            catch { }
            chng = false;
        }

        public void getAllRecordBetweenDatesViewAll()
        {
            chng = true;
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                string query = "";
                //if (metroTextBox4.Text.ToString().Trim().Length > 0 && metroTextBox4.Text.Contains('(') && metroTextBox4.Text.Contains(')'))
                //if (textBox4.Text.ToString().Trim().Length > 0)
                {
                    string datefrom = "";
                    string dateto = "";
                    string todaydate = "";
                    try
                    {
                        todaydate = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
                    }
                    catch { }
                    try
                    {
                        datefrom = metroDateTime_From.Value.Date.Year.ToString() + "-" + metroDateTime_From.Value.Date.Month.ToString() + "-" + metroDateTime_From.Value.Date.Day.ToString();
                    }
                    catch { }
                    try
                    {
                        dateto = metroDateTime_To.Value.Date.Year.ToString() + "-" + metroDateTime_To.Value.Date.Month.ToString() + "-" + metroDateTime_To.Value.Date.Day.ToString();
                    }
                    catch { }
                    //query = "select distinct Item_Id,update_status from Bill_Details where item_type=N'نارمل' and Bill_Id in (select id from Bill where Bill_Date='" + todaydate + "')";
                    query = "select distinct Item_Id,update_status,bill_id,unit_cost from Bill_Details where (item_type=N'نارمل' or item_type=N'سابقہ') and Bill_Id in (select id from Bill where Bill_Date>='" + datefrom + "' and Bill_Date<='" + dateto + "' and status='1')";
                    //MessageBox.Show(customerid);
                    DataTable dtBillItems = gm.GetTable(query);
                    if (dtBillItems.Rows.Count <= 0)
                    {
                        return;
                    }
                    List<string> items_id = new List<string>();
                    foreach (DataRow dbillitems in dtBillItems.Rows)
                    {
                        string bill_id = dbillitems["bill_id"].ToString();
                        //query = "select * from Customer_Or_Vendor where status='1' and id in (select parent from Customer_Or_Vendor where status='1' and id in (select Customer_Vendor_Id from Bill where id='" + bill_id + "') )";
                        //DataTable dt = gm.GetTable(query);
                        //if (dt.Rows.Count > 0)
                        {
                            //string customer_vendor_id = dt.Rows[0][0].ToString();
                            int exist = 0;
                            foreach (string s in items_id)
                            {
                                if (s.Trim() == dbillitems[0].ToString().Trim())
                                {
                                    exist = 1;
                                }
                            }
                            if (exist == 0)
                            {
                                items_id.Add(dbillitems[0].ToString());
                                query = "Select * from items where id='" + dbillitems[0].ToString() + "' and status != '-1' order by Name ASC";
                                DataTable dt = gm.GetTable(query);
                                foreach (DataRow d in dt.Rows)
                                {
                                    string item = d["id"].ToString();
                                    query = "Select * from categories where id='" + d["category_id"].ToString() + "'";
                                    DataTable dtcategory = gm.GetTable(query);
                                    string category = dtcategory.Rows[0][2].ToString();
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
                                    double qty = 0;
                                    try
                                    {
                                        string itemid = d["id"].ToString();
                                        string date = metroDateTimeFrom.Value.Year.ToString() + "-" + (metroDateTimeFrom.Value.Month.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Month.ToString()) : metroDateTimeFrom.Value.Month.ToString()) + "-" + (metroDateTimeFrom.Value.Day.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Day.ToString()) : metroDateTimeFrom.Value.Day.ToString());
                                        //query = "select sum(qty) from Bill_Details where item_id='" + itemid + "' and (item_type=N'نارمل' or item_type=N'بل میں') and Bill_Id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "')";
                                        //query = "select sum(qty) from Bill_Details where item_id='" + itemid + "' and (item_type=N'نارمل' or item_type=N'بل میں') and Bill_Id in (select bill.id from Bill inner join Customer_Or_Vendor on Bill.Customer_Vendor_Id=Customer_Or_Vendor.id where bill.Bill_Date='"+date+"' and Customer_Or_Vendor.status='1' and Customer_Or_Vendor.id in (select id from Customer_Or_Vendor where status='1' and parent in (select id from Customer_Or_Vendor where status='1' and id in(select parent from Customer_Or_Vendor where id in (select Customer_Vendor_Id from Bill where id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "' and Bill_Type='sale trading'))))))";
                                        //for also bill_mein//query = "select sum(qty) from Bill_Details where item_id='" + itemid + "' and (item_type=N'نارمل' or item_type=N'بل میں') and Bill_Id in (select bill.id from Customer_Or_Vendor as customers1 inner join Customer_Or_Vendor as customers2 on customers1.id=customers2.parent inner join bill on bill.Customer_Vendor_Id=customers2.id where customers1.status='1' and customers2.status='1' and Bill.Status!='-1' and bill.Bill_Type='sale trading' and bill.Bill_Date='" + date + "')";
                                        query = "select sum(qty) from Bill_Details where item_id='" + itemid + "' and (item_type=N'نارمل' or item_type=N'سابقہ') and Bill_Id in (select bill.id from Customer_Or_Vendor as customers1 inner join Customer_Or_Vendor as customers2 on customers1.id=customers2.parent inner join bill on bill.Customer_Vendor_Id=customers2.id where customers1.status='1' and customers2.status='1' and Bill.Status!='-1' and bill.Bill_Type='sale trading' and bill.Bill_Date>='" + datefrom + "' and bill.Bill_Date<='" + dateto + "')";
                                        DataTable dt2 = gm.GetTable(query);
                                        try
                                        {
                                            qty = double.Parse(dt2.Rows[0][0].ToString());
                                        }
                                        catch { }
                                    }
                                    catch { }
                                    if (d["status"].ToString() == "1")
                                    {
                                        dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), d["weight"].ToString(), brand, unit, int.Parse(gm.removePointsZero(gm.removePoints(Math.Round(double.Parse(dbillitems["unit_cost"].ToString()), 0, MidpointRounding.AwayFromZero).ToString()))), int.Parse(gm.removePointsZero(gm.removePoints(Math.Round(double.Parse(d["Mazdori"].ToString()), 0, MidpointRounding.AwayFromZero).ToString()))), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Disable", "Delete", "0", "", "", "");
                                    }
                                    else
                                    {
                                        dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), d["weight"].ToString(), brand, unit, int.Parse(gm.removePointsZero(gm.removePoints(Math.Round(double.Parse(dbillitems["unit_cost"].ToString()), 0, MidpointRounding.AwayFromZero).ToString()))), int.Parse(gm.removePointsZero(gm.removePoints(Math.Round(double.Parse(d["Mazdori"].ToString()), 0, MidpointRounding.AwayFromZero).ToString()))), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Enable", "Delete", "0", "", "", "");
                                    }
                                    //if (dbillitems["update_status"].ToString() == "1")
                                    //{
                                    //    dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Green;
                                    //}
                                    if (d["status"].ToString() == "0")
                                    {
                                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                                    }
                                }
                            }
                        }
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
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    try
                    {
                        row.Cells[14].Value = (double.Parse(row.Cells[14].Value.ToString()) + (double.Parse(row.Cells[4].Value.ToString()) * double.Parse(row.Cells[9].Value.ToString()))).ToString();
                    }
                    catch { }
                }
            }
            catch { }
            chng = false;

        }

        public void getCustomerNameRecord()
        {
            chng = true;
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                string query = "";
                if (textBox4.Text.ToString().Trim().Length > 0)
                {
                    string todaydate = "";
                    string datefrom = "";
                    string dateto = "";
                    string customerid = "";
                    try
                    {
                        todaydate = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
                    }
                    catch { }
                    try
                    {
                        datefrom = metroDateTime_From.Value.Date.Year.ToString() + "-" + metroDateTime_From.Value.Date.Month.ToString() + "-" + metroDateTime_From.Value.Date.Day.ToString();
                    }
                    catch { }
                    try
                    {
                        dateto = metroDateTime_To.Value.Date.Year.ToString() + "-" + metroDateTime_To.Value.Date.Month.ToString() + "-" + metroDateTime_To.Value.Date.Day.ToString();
                    }
                    catch { }
                    try
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
                    catch { }
                    query = "select distinct Item_Id,update_status,unit_cost,item_type from Bill_Details where (item_type=N'نارمل' or item_type=N'سابقہ') and Bill_Id in (select id from Bill where Bill_Date='" + todaydate + "' and status='1' and Customer_Vendor_Id in ( select id from Customer_Or_Vendor where status='1' and parent='" + customerid + "'))";
                    //for also billmein//query = "select distinct Item_Id,update_status,unit_cost from Bill_Details where (item_type=N'نارمل' or item_type=N'بل میں') and Bill_Id in (select id from Bill where Bill_Date='" + todaydate + "' and status='1' and Customer_Vendor_Id in ( select id from Customer_Or_Vendor where status='1' and parent='" + customerid + "'))";
                    //MessageBox.Show(customerid);
                    DataTable dtBillItems = gm.GetTable(query);
                    if (dtBillItems.Rows.Count <= 0)
                    {
                        return;
                    }
                    List<ItemAndStatus> Itemandstatus = new List<ItemAndStatus>();
                    foreach (DataRow dbillitems in dtBillItems.Rows)
                    {
                        query = "Select * from items where id='" + dbillitems[0].ToString() + "' and status != '-1' order by Name ASC";
                        DataTable dt = gm.GetTable(query);
                        foreach (DataRow d in dt.Rows)
                        {
                            string item = d["id"].ToString();
                            query = "Select * from categories where id='" + d["category_id"].ToString() + "'";
                            DataTable dtcategory = gm.GetTable(query);
                            string category = dtcategory.Rows[0][2].ToString();
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
                            double qty = 0;
                            try
                            {
                                string itemid = d["id"].ToString();
                                string date = metroDateTimeFrom.Value.Year.ToString() + "-" + (metroDateTimeFrom.Value.Month.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Month.ToString()) : metroDateTimeFrom.Value.Month.ToString()) + "-" + (metroDateTimeFrom.Value.Day.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Day.ToString()) : metroDateTimeFrom.Value.Day.ToString());
                                //if()
                                query = "select sum(qty) from Bill_Details where unit_cost='" + dbillitems["unit_cost"].ToString() + "' and item_type=N'" + dbillitems["item_type"].ToString() + "' and item_id='" + itemid + "' and (item_type=N'نارمل' or item_type=N'سابقہ') and Bill_Id in (select id from Bill where Status!='-1' and Bill_Date='" + todaydate + "' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='" + customerid + "' and status='1'))";
                                DataTable dt2 = gm.GetTable(query);
                                try
                                {
                                    qty = double.Parse(dt2.Rows[0][0].ToString());
                                }
                                catch { }
                            }
                            catch { }
                            //double unit_cost = 0;
                            //try
                            //{
                            //    string itemid = d["id"].ToString();
                            //    string date = metroDateTimeFrom.Value.Year.ToString() + "-" + (metroDateTimeFrom.Value.Month.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Month.ToString()) : metroDateTimeFrom.Value.Month.ToString()) + "-" + (metroDateTimeFrom.Value.Day.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Day.ToString()) : metroDateTimeFrom.Value.Day.ToString());
                            //    query = "select unit_cost from Bill_Details where item_id='" + itemid + "' and (item_type=N'نارمل') and Bill_Id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='" + customerid + "'))";
                            //    DataTable dt2 = gm.GetTable(query);
                            //    try
                            //    {
                            //        unit_cost = double.Parse(dt2.Rows[0][0].ToString());
                            //    }
                            //    catch { }
                            //}
                            //catch { }
                            double billitemmazdori = 0;
                            try
                            {
                                string itemid = d["id"].ToString();
                                string date = metroDateTimeFrom.Value.Year.ToString() + "-" + (metroDateTimeFrom.Value.Month.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Month.ToString()) : metroDateTimeFrom.Value.Month.ToString()) + "-" + (metroDateTimeFrom.Value.Day.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Day.ToString()) : metroDateTimeFrom.Value.Day.ToString());
                                query = "select mazdori from Bill_Details where item_id='" + itemid + "' and (item_type=N'نارمل' or item_type=N'سابقہ') and unit_cost='" + dbillitems["unit_cost"].ToString() + "' and Bill_Id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='" + customerid + "'))";
                                DataTable dt2 = gm.GetTable(query);
                                try
                                {
                                    billitemmazdori = double.Parse(dt2.Rows[0][0].ToString());
                                }
                                catch { }
                            }
                            catch { }
                            int alreadyexist = 0;
                            foreach (ItemAndStatus i in Itemandstatus)
                            {
                                if (i.Id == dbillitems["item_id"].ToString() && i.Rate == dbillitems["unit_cost"].ToString() && i.Item_Type == dbillitems["item_type"].ToString())
                                {
                                    alreadyexist = 1;
                                }
                            }
                            if (alreadyexist == 0)
                            {
                                Itemandstatus.Add(new ItemAndStatus(dbillitems["Item_Id"].ToString(), dbillitems["unit_cost"].ToString(), dbillitems["item_type"].ToString()));

                                if (d["status"].ToString() == "1")
                                {
                                    //item price//dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), d["weight"].ToString(), brand, unit, d["Retail_Price"].ToString(), d["Mazdori"].ToString(), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Disable", "Delete", getTotalWeight(d["id"].ToString()));
                                    //dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), d["weight"].ToString(), brand, unit, gm.removePoints(dbillitems["unit_cost"].ToString()), d["Mazdori"].ToString(), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Disable", "Delete", getTotalWeightByItemUnitCost(d["id"].ToString(), dbillitems["unit_cost"].ToString(), dbillitems["item_type"].ToString()), dbillitems["item_type"].ToString());
                                    dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), d["weight"].ToString(), brand, unit, int.Parse(gm.removePointsZero(gm.removePoints(Math.Round(double.Parse(dbillitems["unit_cost"].ToString()), 0, MidpointRounding.AwayFromZero).ToString()))), int.Parse(gm.removePointsZero(gm.removePoints(Math.Round(double.Parse(billitemmazdori.ToString()), 0, MidpointRounding.AwayFromZero).ToString()))), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Disable", "Delete", "0", dbillitems["item_type"].ToString(), gm.removePoints(dbillitems["unit_cost"].ToString()), "");
                                }
                                else
                                {
                                    //item price//dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), d["weight"].ToString(), brand, unit, d["Retail_Price"].ToString(), d["Mazdori"].ToString(), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Enable", "Delete", getTotalWeight(d["id"].ToString()));
                                    //dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), d["weight"].ToString(), brand, unit, gm.removePoints(unit_cost.ToString()), d["Mazdori"].ToString(), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Enable", "Delete", getTotalWeight(d["id"].ToString()));
                                    //dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), d["weight"].ToString(), brand, unit, gm.removePoints(dbillitems["unit_cost"].ToString()), d["Mazdori"].ToString(), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Enable", "Delete", getTotalWeightByItemUnitCost(d["id"].ToString(), dbillitems["unit_cost"].ToString(), dbillitems["item_type"].ToString()), dbillitems["item_type"].ToString());
                                    dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), d["weight"].ToString(), brand, unit, int.Parse(gm.removePointsZero(gm.removePoints(Math.Round(double.Parse(dbillitems["unit_cost"].ToString()), 0, MidpointRounding.AwayFromZero).ToString()))), int.Parse(gm.removePointsZero(gm.removePoints(Math.Round(double.Parse(billitemmazdori.ToString()), 0, MidpointRounding.AwayFromZero).ToString()))), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Enable", "Delete", "0", dbillitems["item_type"].ToString(), gm.removePoints(dbillitems["unit_cost"].ToString()), "");
                                }
                            }
                            if (dbillitems["update_status"].ToString() == "1")
                            {
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Green;
                            }
                            if (d["status"].ToString() == "0")
                            {
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                            }
                        }
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
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    try
                    {
                        row.Cells[14].Value = (double.Parse(row.Cells[14].Value.ToString()) + (double.Parse(row.Cells[4].Value.ToString()) * double.Parse(row.Cells[9].Value.ToString()))).ToString();
                    }
                    catch { }
                }
            }
            catch { }
            chng = false;

        }

        public string getTotalWeight(string item_id)
        {
            try
            {
                double total_weight = 0;
                try
                {
                    string date = metroDateTimeFrom.Value.Year.ToString() + "-" + (metroDateTimeFrom.Value.Month.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Month.ToString()) : metroDateTimeFrom.Value.Month.ToString()) + "-" + (metroDateTimeFrom.Value.Day.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Day.ToString()) : metroDateTimeFrom.Value.Day.ToString());
                    string query = "";
                    if (metroRadioButton4.Checked == true)
                    {
                        string customerid = "";
                        if (textBox4.Text.ToString().Trim().Length > 0)
                        {
                            try
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
                                    return "";
                                }
                                customerid = id;
                            }
                            catch { }
                        }
                        else
                        {
                            return "";
                        }
                        //query = "select sum(Bill_Details.qty*items.weight) as total_weight from Bill_Details INNER JOIN items on Bill_Details.Item_Id=items.id where Bill_Details.Item_Id='" + item_id + "' and (Bill_Details.item_type=N'نارمل' or Bill_Details.item_type=N'بل میں') and Bill_Details.Bill_Id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='" + customerid + "'))";
                        //query = "select sum(Bill_Details.qty*items.weight) as total_weight from Bill_Details INNER JOIN items on Bill_Details.Item_Id=items.id where Bill_Details.Item_Id='"+item_id+"' and (Bill_Details.item_type=N'نارمل' or Bill_Details.item_type=N'بل میں') and Bill_Details.Bill_Id in (select id from Bill where Status!='-1' and Bill_Date='"+date+"' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='"+customerid+"'))";
                        query = "select sum(Bill_Details.qty*items.weight) as total_weight from Bill_Details INNER JOIN items on Bill_Details.Item_Id=items.id where Bill_Details.Item_Id='" + item_id + "' and (Bill_Details.item_type=N'نارمل' or Bill_Details.item_type=N'سابقہ') and Bill_Details.Bill_Id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='" + customerid + "') and id in (select Bill_Id from Bill_Details where item_id='" + item_id + "' and (item_type=N'نارمل' or item_type=N'سابقہ') and Bill_Id in (select bill.id from Bill inner join Customer_Or_Vendor on Bill.Customer_Vendor_Id=Customer_Or_Vendor.id where bill.Bill_Date='" + date + "' and Customer_Or_Vendor.id in (select id from Customer_Or_Vendor where status='1' and parent in (select id from Customer_Or_Vendor where status='1' and id in(select parent from Customer_Or_Vendor where id in (select Customer_Vendor_Id from Bill where id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "' and Bill_Type='sale trading'))))))))";
                    }
                    else if (metroRadioButton5.Checked == true)
                    {
                        //query = "select sum(Bill_Details.qty*items.weight) as total_weight from Bill_Details INNER JOIN items on Bill_Details.Item_Id=items.id where Bill_Details.Item_Id='" + item_id + "' and (Bill_Details.item_type=N'نارمل' or Bill_Details.item_type=N'بل میں') and Bill_Details.Bill_Id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "')";
                        //query = "select sum(Bill_Details.qty*items.weight) as total_weight from Bill_Details INNER JOIN items on Bill_Details.Item_Id=items.id where Bill_Details.Item_Id='" + item_id + "' and (Bill_Details.item_type=N'نارمل' or Bill_Details.item_type=N'بل میں') and Bill_Details.Bill_Id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "' and id in (select Bill_Id from Bill_Details where item_id='" + item_id + "' and (item_type=N'نارمل' or item_type=N'بل میں') and Bill_Id in (select bill.id from Bill inner join Customer_Or_Vendor on Bill.Customer_Vendor_Id=Customer_Or_Vendor.id where bill.Bill_Date='" + date + "' and Customer_Or_Vendor.id in (select id from Customer_Or_Vendor where status='1' and parent in (select id from Customer_Or_Vendor where status='1' and id in(select parent from Customer_Or_Vendor where id in (select Customer_Vendor_Id from Bill where id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "' and Bill_Type='sale trading'))))))))";
                        query = "select sum(Bill_Details.qty*items.weight) as total_weight from Bill_Details INNER JOIN items on Bill_Details.Item_Id=items.id where Bill_Details.Item_Id='" + item_id + "' and (item_type=N'نارمل' or item_type=N'سابقہ') and Bill_Id in (select bill.id from Customer_Or_Vendor as customers1 inner join Customer_Or_Vendor as customers2 on customers1.id=customers2.parent inner join bill on bill.Customer_Vendor_Id=customers2.id where customers1.status='1' and customers2.status='1' and Bill.Status!='-1' and bill.Bill_Type='sale trading' and bill.Bill_Date='" + date + "')";
                    }
                    else
                    {
                        return "";
                    }
                    DataTable dt2 = gm.GetTable(query);
                    try
                    {
                        total_weight = double.Parse(dt2.Rows[0][0].ToString());
                    }
                    catch { }
                }
                catch { }
                return total_weight.ToString();
            }
            catch
            {
                return "";
            }
        }

        public string getTotalWeightBetweenDates(string item_id)
        {
            try
            {
                double total_weight = 0;
                string datefrom = "";
                string dateto = "";
                try
                {
                    datefrom = metroDateTime_From.Value.Date.Year.ToString() + "-" + metroDateTime_From.Value.Date.Month.ToString() + "-" + metroDateTime_From.Value.Date.Day.ToString();
                }
                catch { }
                try
                {
                    dateto = metroDateTime_To.Value.Date.Year.ToString() + "-" + metroDateTime_To.Value.Date.Month.ToString() + "-" + metroDateTime_To.Value.Date.Day.ToString();
                }
                catch { }
                try
                {
                    string date = metroDateTimeFrom.Value.Year.ToString() + "-" + (metroDateTimeFrom.Value.Month.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Month.ToString()) : metroDateTimeFrom.Value.Month.ToString()) + "-" + (metroDateTimeFrom.Value.Day.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Day.ToString()) : metroDateTimeFrom.Value.Day.ToString());
                    string query = "";
                    //if (metroRadioButton4.Checked == true)
                    {
                        string customerid = "";
                        if (textBox4.Text.ToString().Trim().Length > 0)
                        {
                            try
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
                                    return "";
                                }
                                customerid = id;
                            }
                            catch { }
                        }
                        else
                        {
                            return "";
                        }
                        //query = "select sum(Bill_Details.qty*items.weight) as total_weight from Bill_Details INNER JOIN items on Bill_Details.Item_Id=items.id where Bill_Details.Item_Id='" + item_id + "' and (Bill_Details.item_type=N'نارمل' or Bill_Details.item_type=N'بل میں') and Bill_Details.Bill_Id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='" + customerid + "'))";
                        //query = "select sum(Bill_Details.qty*items.weight) as total_weight from Bill_Details INNER JOIN items on Bill_Details.Item_Id=items.id where Bill_Details.Item_Id='"+item_id+"' and (Bill_Details.item_type=N'نارمل' or Bill_Details.item_type=N'بل میں') and Bill_Details.Bill_Id in (select id from Bill where Status!='-1' and Bill_Date='"+date+"' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='"+customerid+"'))";
                        query = "select sum(Bill_Details.qty*items.weight) as total_weight from Bill_Details INNER JOIN items on Bill_Details.Item_Id=items.id where Bill_Details.Item_Id='" + item_id + "' and (Bill_Details.item_type=N'نارمل' or Bill_Details.item_type=N'سابقہ') and Bill_Details.Bill_Id in (select id from Bill where Status!='-1' and Bill_Date>='" + datefrom + "' and Bill_Date<='" + dateto + "' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='" + customerid + "') and id in (select Bill_Id from Bill_Details where item_id='" + item_id + "' and (item_type=N'نارمل' or item_type=N'سابقہ') and Bill_Id in (select bill.id from Bill inner join Customer_Or_Vendor on Bill.Customer_Vendor_Id=Customer_Or_Vendor.id where bill.Bill_Date='" + date + "' and Customer_Or_Vendor.id in (select id from Customer_Or_Vendor where status='1' and parent in (select id from Customer_Or_Vendor where status='1' and id in(select parent from Customer_Or_Vendor where id in (select Customer_Vendor_Id from Bill where id in (select id from Bill where Status!='-1' and Bill_Date>='" + datefrom + "' and Bill_Date<='" + dateto + "' and Bill_Type='sale trading'))))))))";
                    }
                    DataTable dt2 = gm.GetTable(query);
                    try
                    {
                        total_weight = double.Parse(dt2.Rows[0][0].ToString());
                    }
                    catch { }
                }
                catch { }
                return total_weight.ToString();
            }
            catch
            {
                return "";
            }
        }

        public string getTotalWeightByItemUnitCost(string item_id, string unit_cost, string item_type)
        {
            try
            {
                string datefrom = "";
                string dateto = "";
                try
                {
                    datefrom = metroDateTime_From.Value.Date.Year.ToString() + "-" + metroDateTime_From.Value.Date.Month.ToString() + "-" + metroDateTime_From.Value.Date.Day.ToString();
                }
                catch { }
                try
                {
                    dateto = metroDateTime_To.Value.Date.Year.ToString() + "-" + metroDateTime_To.Value.Date.Month.ToString() + "-" + metroDateTime_To.Value.Date.Day.ToString();
                }
                catch { }

                double total_weight = 0;
                try
                {
                    string date = metroDateTimeFrom.Value.Year.ToString() + "-" + (metroDateTimeFrom.Value.Month.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Month.ToString()) : metroDateTimeFrom.Value.Month.ToString()) + "-" + (metroDateTimeFrom.Value.Day.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Day.ToString()) : metroDateTimeFrom.Value.Day.ToString());
                    string query = "";
                    if (metroRadioButton4.Checked == true)
                    {
                        string customerid = "";
                        if (textBox4.Text.ToString().Trim().Length > 0)
                        {
                            try
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
                                    return "";
                                }
                                customerid = id;
                            }
                            catch { }
                        }
                        else
                        {
                            return "";
                        }
                        //query = "select sum(Bill_Details.qty*items.weight) as total_weight from Bill_Details INNER JOIN items on Bill_Details.Item_Id=items.id where Bill_Details.Item_Id='" + item_id + "' and (Bill_Details.item_type=N'نارمل' or Bill_Details.item_type=N'بل میں') and Bill_Details.Bill_Id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='" + customerid + "'))";
                        //query = "select sum(Bill_Details.qty*items.weight) as total_weight from Bill_Details INNER JOIN items on Bill_Details.Item_Id=items.id where Bill_Details.Item_Id='"+item_id+"' and (Bill_Details.item_type=N'نارمل' or Bill_Details.item_type=N'بل میں') and Bill_Details.Bill_Id in (select id from Bill where Status!='-1' and Bill_Date='"+date+"' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='"+customerid+"'))";
                        query = "select sum(Bill_Details.qty*items.weight) as total_weight from Bill_Details INNER JOIN items on Bill_Details.Item_Id=items.id where Bill_Details.unit_cost='" + unit_cost + "' and Bill_Details.item_type=N'" + item_type + "' and Bill_Details.Item_Id='" + item_id + "' and (Bill_Details.item_type=N'نارمل' or Bill_Details.item_type=N'سابقہ') and Bill_Details.Bill_Id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='" + customerid + "') and id in (select Bill_Id from Bill_Details where item_id='" + item_id + "' and (item_type=N'نارمل' or item_type=N'سابقہ') and Bill_Id in (select bill.id from Bill inner join Customer_Or_Vendor on Bill.Customer_Vendor_Id=Customer_Or_Vendor.id where bill.Bill_Date='" + date + "' and Customer_Or_Vendor.id in (select id from Customer_Or_Vendor where status='1' and parent in (select id from Customer_Or_Vendor where status='1' and id in(select parent from Customer_Or_Vendor where id in (select Customer_Vendor_Id from Bill where id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "' and Bill_Type='sale trading'))))))))";
                    }
                    else if (metroRadioButton5.Checked == true)
                    {
                        //query = "select sum(Bill_Details.qty*items.weight) as total_weight from Bill_Details INNER JOIN items on Bill_Details.Item_Id=items.id where Bill_Details.Item_Id='" + item_id + "' and (Bill_Details.item_type=N'نارمل' or Bill_Details.item_type=N'بل میں') and Bill_Details.Bill_Id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "')";
                        //query = "select sum(Bill_Details.qty*items.weight) as total_weight from Bill_Details INNER JOIN items on Bill_Details.Item_Id=items.id where Bill_Details.Item_Id='" + item_id + "' and (Bill_Details.item_type=N'نارمل' or Bill_Details.item_type=N'بل میں') and Bill_Details.Bill_Id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "' and id in (select Bill_Id from Bill_Details where item_id='" + item_id + "' and (item_type=N'نارمل' or item_type=N'بل میں') and Bill_Id in (select bill.id from Bill inner join Customer_Or_Vendor on Bill.Customer_Vendor_Id=Customer_Or_Vendor.id where bill.Bill_Date='" + date + "' and Customer_Or_Vendor.id in (select id from Customer_Or_Vendor where status='1' and parent in (select id from Customer_Or_Vendor where status='1' and id in(select parent from Customer_Or_Vendor where id in (select Customer_Vendor_Id from Bill where id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "' and Bill_Type='sale trading'))))))))";
                        query = "select sum(Bill_Details.qty*items.weight) as total_weight from Bill_Details INNER JOIN items on Bill_Details.Item_Id=items.id where Bill_Details.unit_cost='" + unit_cost + "' and Bill_Details.item_type=N'" + item_type + "' and Bill_Details.Item_Id='" + item_id + "' and (item_type=N'نارمل' or item_type=N'سابقہ') and Bill_Id in (select bill.id from Customer_Or_Vendor as customers1 inner join Customer_Or_Vendor as customers2 on customers1.id=customers2.parent inner join bill on bill.Customer_Vendor_Id=customers2.id where customers1.status='1' and customers2.status='1' and Bill.Status!='-1' and bill.Bill_Type='sale trading' and bill.Bill_Date='" + date + "')";
                    }
                    else
                    {
                        return "";
                    }
                    DataTable dt2 = gm.GetTable(query);
                    try
                    {
                        total_weight = double.Parse(dt2.Rows[0][0].ToString());
                    }
                    catch { }
                }
                catch { }
                return total_weight.ToString();
            }
            catch
            {
                return "";
            }
        }

        public void getCategoryRecord(string category_id)
        {
            chng = true;
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                string query = "";
                if (textBox4.Text.ToString().Trim().Length > 0)
                {
                    string todaydate = "";
                    string customerid = "";
                    try
                    {
                        todaydate = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
                    }
                    catch { }
                    try
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
                    catch { }
                    query = "select distinct Item_Id,update_status from Bill_Details where item_type=N'نارمل' and Bill_Id in (select id from Bill where Bill_Date='" + todaydate + "' and Customer_Vendor_Id in ( select id from Customer_Or_Vendor where parent='" + customerid + "'))";
                    //MessageBox.Show(customerid);
                    DataTable dtBillItems = gm.GetTable(query);
                    if (dtBillItems.Rows.Count <= 0)
                    {
                        return;
                    }
                    foreach (DataRow dbillitems in dtBillItems.Rows)
                    {
                        query = "Select * from items where id='" + dbillitems[0].ToString() + "' and category_id='" + category_id + "' and status != '-1' order by Name ASC";
                        DataTable dt = gm.GetTable(query);
                        foreach (DataRow d in dt.Rows)
                        {
                            query = "Select * from categories where id='" + d["category_id"].ToString() + "'";
                            DataTable dtcategory = gm.GetTable(query);
                            string category = dtcategory.Rows[0][2].ToString();
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
                            double qty = 0;
                            try
                            {
                                string itemid = d["id"].ToString();
                                string date = metroDateTimeFrom.Value.Year.ToString() + "-" + (metroDateTimeFrom.Value.Month.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Month.ToString()) : metroDateTimeFrom.Value.Month.ToString()) + "-" + (metroDateTimeFrom.Value.Day.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Day.ToString()) : metroDateTimeFrom.Value.Day.ToString());
                                query = "select sum(qty) from Bill_Details where (item_type=N'نارمل' or item_type=N'بل میں') and Bill_Id in (select id from Bill where Status!='-1' and Bill_Date='" + date + "')";
                                DataTable dt2 = gm.GetTable(query);
                                try
                                {
                                    qty = double.Parse(dt2.Rows[0][0].ToString());
                                }
                                catch { }
                            }
                            catch { }

                            if (d["status"].ToString() == "1")
                                dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), d["weight"].ToString(), brand, unit, gm.removePointsZero(gm.removePoints(dbillitems["unit_cost"].ToString())), gm.removePointsZero(gm.removePoints(d["Mazdori"].ToString())), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Disable", "Delete", getTotalWeight(d["id"].ToString()), "");
                            else
                                dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), d["weight"].ToString(), brand, unit, gm.removePointsZero(gm.removePoints(dbillitems["unit_cost"].ToString())), gm.removePointsZero(gm.removePoints(d["Mazdori"].ToString())), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Enable", "Delete", getTotalWeight(d["id"].ToString()), "");
                            if (dbillitems["update_status"].ToString() == "1")
                            {
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Green;
                            }
                            if (d["status"].ToString() == "0")
                            {
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                            }
                        }
                        dataGridView1.ClearSelection();
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
            chng = false;
        }

        public void getItemNameRecord(string id)
        {
            chng = true;
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                string query = "";
                if (textBox4.Text.ToString().Trim().Length > 0)
                {
                    string todaydate = "";
                    string customerid = "";
                    try
                    {
                        todaydate = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
                    }
                    catch { }
                    try
                    {
                        string id2 = "";
                        try//only word if main customer or sub sub customer name is unique
                        {
                            query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + textBox4.Text.Trim() + "' and status='1'";
                            id2 = gm.GetTable(query).Rows[0][0].ToString();
                        }
                        catch { }
                        if (id2 == "")
                        {
                            return;
                        }
                        customerid = id2;
                    }
                    catch { }
                    query = "select distinct Item_Id,update_status from Bill_Details where item_type=N'نارمل' and Bill_Id in (select id from Bill where Bill_Date='" + todaydate + "' and Customer_Vendor_Id in ( select id from Customer_Or_Vendor where parent='" + customerid + "'))";
                    //MessageBox.Show(customerid);
                    DataTable dtBillItems = gm.GetTable(query);
                    if (dtBillItems.Rows.Count <= 0)
                    {
                        return;
                    }
                    foreach (DataRow dbillitems in dtBillItems.Rows)
                    {
                        query = "Select * from items where id='" + dbillitems[0].ToString() + "' and id='" + id + "' and status != '-1' order by Name ASC";
                        DataTable dt = gm.GetTable(query);
                        foreach (DataRow d in dt.Rows)
                        {
                            query = "Select * from categories where id='" + d["category_id"].ToString() + "'";
                            DataTable dtcategory = gm.GetTable(query);
                            string category = dtcategory.Rows[0][2].ToString();
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
                            double qty = 0;
                            try
                            {
                                //try
                                //{
                                //    qty = double.Parse(d["Qty"].ToString());
                                //}
                                //catch { }

                                string itemid = d["id"].ToString();
                                string date = metroDateTimeFrom.Value.Year.ToString() + "-" + (metroDateTimeFrom.Value.Month.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Month.ToString()) : metroDateTimeFrom.Value.Month.ToString()) + "-" + (metroDateTimeFrom.Value.Day.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Day.ToString()) : metroDateTimeFrom.Value.Day.ToString());
                                query = "select sum(Bill_Details.Qty) from Bill_Details inner join Bill on Bill.id=Bill_Details.Bill_Id where Bill.Bill_Date<='" + date + "' and Bill.Bill_Type='purchase trading' and Bill_Details.Item_Id='" + itemid + "' and bill.status='1'";
                                DataTable dt2 = gm.GetTable(query);
                                //+purchase qty
                                try
                                {
                                    qty += double.Parse(dt2.Rows[0][0].ToString());
                                }
                                catch { }

                                query = "select sum(Bill_Details.Qty) from Bill_Details inner join Bill on Bill.id=Bill_Details.Bill_Id where Bill.Bill_Date<='" + date + "' and Bill.Bill_Type='sale trading' and Bill_Details.Item_Id='" + itemid + "' and bill.status='1'";
                                dt2 = gm.GetTable(query);
                                //-sale qty
                                try
                                {
                                    qty -= double.Parse(dt2.Rows[0][0].ToString());
                                }
                                catch { }

                                query = "select sum(Report_Details.Sale_Qty) from Report_Details inner join Report on Report.id=Report_Details.Report_Id where Report.report_date<='" + date + "' and Report_Details.Item_Id='" + itemid + "' and report.status='1'";
                                dt2 = gm.GetTable(query);
                                //-wholesale qty
                                try
                                {
                                    qty -= double.Parse(dt2.Rows[0][0].ToString());
                                }
                                catch { }
                            }
                            catch { }
                            if (d["status"].ToString() == "1")
                                dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), d["weight"].ToString(), brand, unit, gm.removePointsZero(gm.removePoints(dbillitems["unit_cost"].ToString())), gm.removePointsZero(gm.removePoints(d["Mazdori"].ToString())), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Disable", "Delete", getTotalWeight(d["id"].ToString()), "");
                            else
                                dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), d["weight"].ToString(), brand, unit, gm.removePointsZero(gm.removePoints(dbillitems["unit_cost"].ToString())), gm.removePointsZero(gm.removePoints(d["Mazdori"].ToString())), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Enable", "Delete", getTotalWeight(d["id"].ToString()), "");
                            if (dbillitems["update_status"].ToString() == "1")
                            {
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Green;
                            }
                            if (d["status"].ToString() == "0")
                            {
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                            }
                        }
                        dataGridView1.ClearSelection();
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
            chng = false;
        }

        private void metroRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.Clear();
            }
            //metroTile2.Show();
            //label1.Show();
            string query = "";
            itemCategory1.Hide();
            metroTextBoxCategory.Hide();
            metroTextBox1.Hide();
            if (textBox4.Text.ToString().Trim().Length > 0)
            {
                string customerid = "";
                try
                {
                    string todaydate = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
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
                catch { }
                query = "select * from Customer_Or_Vendor where parent='" + customerid + "'";
                DataTable dtCustomers = gm.GetTable(query);
                if (dtCustomers.Rows.Count <= 0)
                {
                    return;
                }
                getAllRecord();
            }
            if (RJ.Properties.Settings.Default.logintype == "User" || RJ.Properties.Settings.Default.logintype == "user")
            {
                metroTile1.Visible = false;
            }
            else
            {
                metroTile1.Visible = true;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                try
                {
                    //dataGridView1.Rows[e.RowIndex].Selected = true;
                    if (e.ColumnIndex == 10)
                    {
                        if (dataGridView1.Rows[e.RowIndex].Cells[13].Value.ToString() == "Enable")
                        {
                            if (con.State.ToString() == "Closed")
                            {
                                con.Open();
                            }
                            string query = @"update items set status='1' where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Successfully Enabled");
                            if (metroRadioButton1.Checked == true)
                                getAllRecord();
                            else if (metroRadioButton2.Checked == true)
                            {
                                try
                                {
                                    string[] s = metroTextBoxCategory.Text.Trim().Split('(');
                                    string[] a = s[s.Length - 1].Trim().Split(')');
                                    string id = a[0].ToString();
                                    if (id.Trim() != "")
                                    {
                                        getCategoryRecord(id);
                                    }
                                    if (metroTextBoxCategory.Text.Trim() == "")
                                        getAllRecord();
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
                                    string[] s = metroTextBox1.Text.Trim().Split('(');
                                    string[] a = s[s.Length - 1].Trim().Split(')');
                                    string id = a[0].ToString();
                                    if (id.Trim() != "")
                                    {
                                        getItemNameRecord(id);
                                    }
                                    if (metroTextBox1.Text.Trim() == "")
                                        getAllRecord();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                        }
                        else
                        {
                            if (con.State.ToString() == "Closed")
                            {
                                con.Open();
                            }
                            string query = @"update items set status='0' where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Successfully Disabled");
                            if (metroRadioButton1.Checked == true)
                                getAllRecord();
                            else if (metroRadioButton2.Checked == true)
                            {
                                try
                                {
                                    string[] s = metroTextBoxCategory.Text.Trim().Split('(');
                                    string[] a = s[s.Length - 1].Trim().Split(')');
                                    string id = a[0].ToString();
                                    if (id.Trim() != "")
                                    {
                                        getCategoryRecord(id);
                                    }
                                    if (metroTextBoxCategory.Text.Trim() == "")
                                        getAllRecord();
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
                                    string[] s = metroTextBox1.Text.Trim().Split('(');
                                    string[] a = s[s.Length - 1].Trim().Split(')');
                                    string id = a[0].ToString();
                                    if (id.Trim() != "")
                                    {
                                        getItemNameRecord(id);
                                    }
                                    if (metroTextBox1.Text.Trim() == "")
                                        getAllRecord();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                        }
                        dataGridView1.ClearSelection();
                    }
                    if (e.ColumnIndex == 11)
                    {
                        DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this item?(yes/no)", "Confirmation", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.No)
                        {
                            return;
                        }
                        string query = @"update items set status='-1' where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
                        gm.ExecuteNonQuery(query);
                        MessageBox.Show("Delete Successfully");
                        if (metroRadioButton1.Checked == true)
                            getAllRecord();
                        else if (metroRadioButton2.Checked == true)
                            metroRadioButton2.Checked = true;
                        dataGridView1.ClearSelection();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            catch { }
        }

        private void metroRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.Clear();
            }
            //metroTile2.Show();
            //label1.Show();
            treeView1.CollapseAll();
            itemCategory1.Show();
            metroTextBoxCategory.Text = string.Empty;
            metroTextBoxCategory.Show();
            metroTextBox1.Hide();
            metroTextBoxCategory.Focus();
            if (RJ.Properties.Settings.Default.logintype == "User" || RJ.Properties.Settings.Default.logintype == "user")
            {
                metroTile1.Visible = false;
            }
            else
            {
                metroTile1.Visible = true;
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
            try
            {
                string[] s = metroTextBoxCategory.Text.Trim().Split('(');
                string[] a = s[s.Length - 1].Trim().Split(')');
                string id = a[0].ToString();
                if (id.Trim() != "")
                {
                    getCategoryRecord(id);
                }
                //if (metroTextBoxCategory.Text.Trim() == "")
                //    getAllRecord();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
            metroTile2.Hide();
            label1.Hide();
            itemCategory1.Hide();
            metroTextBoxCategory.Hide();
            metroTextBox1.Show();
            metroTextBox1.Text = string.Empty;
            metroTextBox1.Focus();
            if (RJ.Properties.Settings.Default.logintype == "User" || RJ.Properties.Settings.Default.logintype == "user")
            {
                metroTile1.Visible = false;
            }
            else
            {
                metroTile1.Visible = true;
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
                    getItemNameRecord(id);
                }
                //if (metroTextBox1.Text.Trim() == "")
                //    getAllRecord();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    AddItem ai = new AddItem(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                    ai.ShowDialog();
                    if (metroRadioButton1.Checked == true)
                    {
                        itemCategory1.Hide();
                        metroTextBoxCategory.Hide();
                        metroTextBox1.Hide();
                        getAllRecord();
                    }
                    else if (metroRadioButton2.Checked == true)
                    {
                        treeView1.CollapseAll();
                        itemCategory1.Show();
                        metroTextBoxCategory.Text = string.Empty;
                        metroTextBoxCategory.Show();
                        metroTextBox1.Hide();
                        metroTextBoxCategory.Focus();
                    }
                    else if (metroRadioButton3.Checked == true)
                    {
                        itemCategory1.Hide();
                        metroTextBoxCategory.Hide();
                        metroTextBox1.Show();
                        metroTextBox1.Text = string.Empty;
                        metroTextBox1.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Select Item To Edit");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ItemsList_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                    this.Dispose(true);
                //if (e.Modifiers == Keys.Control && e.KeyCode == Keys.P)
                //{
                //    if(metroRadioButton3.Checked == false)
                //        metroTile2.PerformClick();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    if (metroRadioButton2.Checked == true)
                    {
                        if (metroTextBoxCategory.Text.Trim() == "")
                        {
                            MessageBox.Show("Enter Category");
                            return;
                        }
                    }
                    string report = "Items List";
                    string category = "";
                    if (metroRadioButton1.Checked == true)
                    {
                        category = "All Categories";
                    }
                    else
                    {
                        category = metroTextBoxCategory.Text.Trim();
                    }
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Sr_No");
                    dt.Columns.Add("Category");
                    dt.Columns.Add("Sku");
                    dt.Columns.Add("Item");
                    dt.Columns.Add("Quantity");
                    dt.Columns.Add("Invoice");
                    dt.Columns.Add("Trade");
                    int i = 0;
                    string invoice = "0";
                    string trade = "0";
                    string totalqty = "0";
                    string totalinvoice = "0";
                    string totaltrade = "0";
                    foreach (DataGridViewRow d in dataGridView1.Rows)
                    {
                        i++;
                        try
                        {
                            invoice = (double.Parse(d.Cells[10].Value.ToString().Trim().ToString()) * double.Parse(d.Cells[11].Value.ToString().Trim().ToString())).ToString();
                        }
                        catch { }
                        try
                        {
                            trade = (double.Parse(d.Cells[10].Value.ToString().Trim().ToString()) * double.Parse(d.Cells[7].Value.ToString().Trim().ToString())).ToString();
                        }
                        catch { }
                        try
                        {
                            totalinvoice = (double.Parse(totalinvoice) + double.Parse(invoice)).ToString();
                        }
                        catch { }
                        try
                        {
                            totaltrade = (double.Parse(totaltrade) + double.Parse(trade)).ToString();
                        }
                        catch { }
                        try
                        {
                            totalqty = (double.Parse(totalqty.ToString().Trim().ToString()) + double.Parse(d.Cells[10].Value.ToString().Trim().ToString())).ToString();
                        }
                        catch { }
                        dt.Rows.Add(i.ToString(), d.Cells[2].Value.ToString(), d.Cells[1].Value.ToString(), d.Cells[3].Value.ToString(), d.Cells[10].Value.ToString(), invoice, trade);
                    }
                    string lowstockreport = "0";
                    string monthName = metroDateTimeFrom.Value.Date.ToString("MMM", CultureInfo.InvariantCulture);
                    string date = metroDateTimeFrom.Value.Day.ToString() + " " + monthName + "," + metroDateTimeFrom.Value.Year.ToString();
                    gm.PrintItemList(dt, report, category, totalqty, totalinvoice, totaltrade, lowstockreport, date);
                }
                else
                {
                    MessageBox.Show("No Record Found To Print");
                    return;
                }
            }
            catch { }
        }

        private void metroDateTimeFrom_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (metroRadioButton1.Checked == true)
                {
                    getAllRecord();
                }
                if (metroRadioButton2.Checked == true)
                {
                    getCategoryRecord(treeView1.SelectedNode.Tag.ToString());
                }
                if (metroRadioButton3.Checked == true)
                {
                    try
                    {
                        string[] s = metroTextBox1.Text.Trim().Split('(');
                        string[] a = s[s.Length - 1].Trim().Split(')');
                        string id = a[0].ToString();
                        if (id.Trim() != "")
                        {
                            getItemNameRecord(id);
                        }
                        if (metroTextBox1.Text.Trim() == "")
                            getAllRecord();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch { }
        }

        private void a(object sender, DataGridViewCellEventArgs e)
        {

        }

        bool chng = true;
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                try
                {
                    if (dataGridView1.Rows.Count > 0 && chng == false)
                        dataGridView1.Rows[e.RowIndex].Cells["Column18"].Value = "1";
                }
                catch { }
                if (e.ColumnIndex == 6)
                {
                    try
                    {
                        //double a = double.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                        //if (a <= 0)
                        //    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "1";
                    }
                    catch
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "0";
                    }
                }
            }
            catch
            { }
        }

        private void metroTile3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count <= 0)
            {
                MessageBox.Show("Item Not Found For Update Rates");
                return;
            }
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to update rates of customer : " + textBox4.Text.ToString() + " ?(yes/no)", "Confirmation", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                return;
            }
            try
            {
                string query = "";
                int ra = 0;
                List<string> bill_ids = new List<string>();
                string todaydate = "";
                string customerid = "";
                string id = "";
                try
                {
                    todaydate = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
                }
                catch { }
                if (textBox4.Text.ToString().Trim().Length > 0)
                {
                    try
                    {
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
                    catch { }

                    query = "select * from Bill_Details where item_type=N'نارمل' and Bill_Id in (select id from Bill where Bill_Date='" + todaydate + "' and Customer_Vendor_Id in ( select id from Customer_Or_Vendor where status='1' and parent='" + customerid + "'))";
                    DataTable dtBillItems = gm.GetTable(query);
                    if (dtBillItems.Rows.Count <= 0)
                    {
                        return;
                    }

                    foreach (DataGridViewRow d in dataGridView1.Rows)
                    {
                        foreach (DataRow dbillitems in dtBillItems.Rows)
                        {
                            if (d.Cells[0].Value.ToString() == dbillitems["Item_Id"].ToString())
                            {
                                double qty = 0;
                                try
                                {
                                    qty = double.Parse(dbillitems["Qty"].ToString());
                                }
                                catch { }
                                double unit_cost = 0;
                                try
                                {
                                    unit_cost = double.Parse(d.Cells[7].Value.ToString());
                                }
                                catch { }
                                double mazdori = 0;
                                try
                                {
                                    if (double.Parse(d.Cells[7].Value.ToString()) == 0)
                                    {
                                        mazdori = 0;
                                        d.Cells[8].Value = "0";
                                    }
                                    else
                                    {
                                        mazdori = double.Parse(d.Cells[8].Value.ToString());
                                    }
                                }
                                catch { }
                                double total_amount = 0;
                                try
                                {
                                    total_amount = qty * (unit_cost + mazdori);
                                    total_amount = (Math.Round((total_amount), 0, MidpointRounding.AwayFromZero));
                                }
                                catch { }
                                string item_cost = "";
                                query = "select retail_price from items where id='" + dbillitems["item_id"].ToString() + "'";
                                try
                                {
                                    item_cost = gm.GetTable(query).Rows[0][0].ToString();
                                }
                                catch { }
                                string bill_item_cost = gm.removePointsZero(gm.removePoints(dbillitems["unit_cost"].ToString()));
                                if (d.Cells[7].Value.ToString() != bill_item_cost)
                                {
                                    query = "update bill_details set Unit_Cost='" + d.Cells[7].Value.ToString() + "',mazdori='" + d.Cells[8].Value.ToString() + "',Total_Amount='" + total_amount.ToString() + "',update_status='1',update_date='" + DateTime.Now.ToShortDateString() + "' where id = '" + dbillitems[0].ToString() + "' and item_type=N'نارمل' and Unit_Cost='" + d.Cells[16].Value.ToString() + "'";
                                    gm.ExecuteNonQuery(query);
                                }
                                else
                                {
                                    query = "update bill_details set mazdori='" + d.Cells[8].Value.ToString() + "',Total_Amount='" + total_amount.ToString() + "' where id = '" + dbillitems[0].ToString() + "' and item_type=N'نارمل' and Unit_Cost='" + d.Cells[16].Value.ToString() + "'";
                                    gm.ExecuteNonQuery(query);
                                }
                                if (!bill_ids.Contains(dbillitems["Bill_Id"].ToString()))
                                {
                                    bill_ids.Add(dbillitems["Bill_Id"].ToString());
                                }
                                query = "update items set Retail_Price='" + d.Cells[7].Value.ToString() + "',mazdori='" + d.Cells[8].Value.ToString() + "',update_status='1',update_date='" + DateTime.Now.ToShortDateString() + "' where id = '" + dbillitems["Item_Id"].ToString() + "'";
                                gm.ExecuteNonQuery(query);
                                ra = 1;
                            }
                        }
                        foreach (string bill_id in bill_ids)
                        {
                            try
                            {
                                query = "select sum(Total_Amount) from Bill_Details where Bill_Id='" + bill_id + "'";
                                DataTable dt = gm.GetTable(query);
                                double total_amount = 0;
                                try
                                {
                                    total_amount = double.Parse(dt.Rows[0][0].ToString());
                                    total_amount = (Math.Round((total_amount), 0, MidpointRounding.AwayFromZero));
                                }
                                catch { }
                                query = "update bill set Net_Total_Amount='" + total_amount.ToString() + "',Total_Amount='" + total_amount.ToString() + "',Balance='" + total_amount.ToString() + "' where id = '" + bill_id + "'";
                                gm.ExecuteNonQuery(query);
                            }
                            catch { }
                        }
                    }
                }
                if (ra == 1)
                {
                    MessageBox.Show("Records Updated Successfully");
                }
                if (metroRadioButton4.Checked == true)
                {
                    try
                    {
                        if (dataGridView1.Rows.Count > 0)
                        {
                            dataGridView1.Rows.Clear();
                        }
                        if (textBox4.Text.ToString().Trim().Length > 0)
                        {
                            customerid = "";
                            try
                            {
                                id = "";
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
                            catch { }
                            if (customerid != "")
                            {
                                getCustomerNameRecord();
                            }
                        }
                    }
                    catch { }
                }
                else if (metroRadioButton5.Checked == true)
                {
                    metroRadioButton5.Checked = false;
                    metroRadioButton5.PerformClick();

                }
            }
            catch { }
        }

        private void metroTextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        POS_Sabzi_MandiEntities db = new POS_Sabzi_MandiEntities();

        private void metroTile4_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count <= 0)
                {
                    MessageBox.Show("Item Not Found For Update Rates");
                    return;
                }
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to update rates of all customers?(yes/no)", "Confirmation", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
                progressBar1.Minimum = 0;
                int changedcount = 0;

                foreach (DataGridViewRow d in dataGridView1.Rows)
                {
                    try
                    {
                        if (d.Cells["Column18"].Value.ToString() == "1")
                            changedcount++;
                    }
                    catch (Exception ex) { MessageBox.Show("r" + ex.Message); }
                }
                //changedcount = dataGridView1.Rows.Count;
                progressBar1.Maximum = changedcount - 1;
                backgroundWorker1.RunWorkerAsync();
            }
            catch { }
        }

        List<string> selectedmaincustomers = new List<string>();
        private void metroRadioButton5_CheckedChanged(object sender, EventArgs e)
        {
            groupBox3.Hide();
            //dataGridView1.Columns[14].Visible = true;
            //metroTile3.Show();
            //metroTile4.Show();
            //dataGridView1.Columns[15].Visible = false;
            //metroTile3.Visible = false;
            //textBox4.Visible = false;
            //getAllRecord();
            //groupBox2.Hide();
        }

        private void metroRadioButton4_CheckedChanged(object sender, EventArgs e)
        {
            groupBox3.Hide();
            listBox1.Items.Clear();
            //dataGridView1.Columns[14].Visible = true;
            metroTile3.Show();
            metroTile4.Hide();
            dataGridView1.Columns[15].Visible = true;
            metroTile3.Visible = true;
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.Clear();
            }
            textBox4.Visible = true;
            textBox4.Text = "";
            try
            {
                metroDateTime_From.Value = DateTime.Now.Date;
                metroDateTime_To.Value = DateTime.Now.Date;
                metroDateTime_From.MaxDate = metroDateTime_To.Value;
                metroDateTime_To.MinDate = metroDateTime_From.Value;
            }
            catch { }
            groupBox2.Hide();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                string query = "";
                if (metroRadioButton4.Checked == true)
                {
                    if (textBox4.Text.ToString().Trim().Length > 0)
                    {
                        string customerid = "";
                        try
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
                        catch { }
                        if (customerid != "")
                        {
                            getCustomerNameRecord();
                        }
                    }
                }
                else if (metroRadioButton6.Checked == true)
                {
                    if (metroRadioButton9.Checked == true)
                    {
                        if (textBox4.Text.ToString().Trim().Length > 0)
                        {
                            string customerid = "";
                            try
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
                            catch { }
                            if (customerid != "")
                            {
                                getAllRecordBetweenDates();
                            }
                        }
                    }
                    else if (metroRadioButton8.Checked == true)
                    {
                        textBox4.Text = "";
                        getAllRecordBetweenDatesViewAll();
                    }
                }
            }
            catch { }
        }

        private void metroDateTime_From_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                metroDateTime_From.MaxDate = metroDateTime_To.Value;
                metroDateTime_To.MinDate = metroDateTime_From.Value;
                if (metroRadioButton9.Checked == true)
                {
                    if (textBox4.Text.ToString().Trim().Length > 0)
                    {
                        string customerid = "";
                        try
                        {
                            string id = "";
                            try//only word if main customer or sub sub customer name is unique
                            {
                                string query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + textBox4.Text.Trim() + "' and status='1'";
                                id = gm.GetTable(query).Rows[0][0].ToString();
                            }
                            catch { }
                            if (id == "")
                            {
                                return;
                            }
                            customerid = id;
                        }
                        catch { }
                        if (customerid != "")
                        {
                            getAllRecordBetweenDates();
                        }
                    }
                }
                else if (metroRadioButton8.Checked == true)
                {
                    textBox4.Text = "";
                    getAllRecordBetweenDatesViewAll();
                }
            }
            catch { }
        }

        private void metroDateTime_To_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                metroDateTime_From.MaxDate = metroDateTime_To.Value;
                metroDateTime_To.MinDate = metroDateTime_From.Value;
                if (metroRadioButton9.Checked == true)
                {
                    if (textBox4.Text.ToString().Trim().Length > 0)
                    {
                        string customerid = "";
                        try
                        {
                            string id = "";
                            try//only word if main customer or sub sub customer name is unique
                            {
                                string query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + textBox4.Text.Trim() + "' and status='1'";
                                id = gm.GetTable(query).Rows[0][0].ToString();
                            }
                            catch { }
                            if (id == "")
                            {
                                return;
                            }
                            customerid = id;
                        }
                        catch { }
                        if (customerid != "")
                        {
                            getAllRecordBetweenDates();
                        }
                    }
                }
                else if (metroRadioButton8.Checked == true)
                {
                    textBox4.Text = "";
                    getAllRecordBetweenDatesViewAll();
                }
            }
            catch { }
        }

        private void metroRadioButton6_CheckedChanged(object sender, EventArgs e)
        {
            metroRadioButton9.Checked = true;
            groupBox3.Show();
            listBox1.Items.Clear();
            try
            {
                //dataGridView1.Columns[14].Visible = false;
                metroTile3.Hide();
                metroTile4.Hide();
                textBox4.Visible = true;
                textBox4.Text = "";
                dataGridView1.Columns[15].Visible = false;
                metroTile3.Visible = false;
                getAllRecordBetweenDates();
                groupBox2.Show();
            }
            catch { }
        }

        private void metroTile5_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    if (metroRadioButton2.Checked == true)
                    {
                        if (metroTextBoxCategory.Text.Trim() == "")
                        {
                            MessageBox.Show("Enter Category");
                            return;
                        }
                    }
                    string report = "Items List";
                    string category = "";
                    if (metroRadioButton1.Checked == true)
                    {
                        category = "All Categories";
                    }
                    else
                    {
                        category = metroTextBoxCategory.Text.Trim();
                    }
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Item");
                    dt.Columns.Add("Unit");
                    dt.Columns.Add("Qty");
                    int i = 0;
                    string invoice = "0";
                    string trade = "0";
                    string totalqty = "0";
                    string totalinvoice = "0";
                    string totaltrade = "0";
                    foreach (DataGridViewRow d in dataGridView1.Rows)
                    {
                        dt.Rows.Add(d.Cells[3].Value.ToString(), d.Cells[6].Value.ToString(), d.Cells[9].Value.ToString());
                    }
                    string lowstockreport = "0";
                    string monthName = metroDateTimeFrom.Value.Date.ToString("MMM", CultureInfo.InvariantCulture);
                    //string date = metroDateTimeFrom.Value.Day.ToString() + " " + monthName + "," + metroDateTimeFrom.Value.Year.ToString();
                    string date = metroDateTimeFrom.Value.Day.ToString() + "-" + metroDateTimeFrom.Value.Month.ToString() + "-" + metroDateTimeFrom.Value.Year.ToString();
                    //gm.PrintBillItems(dt, report, category, totalqty, totalinvoice, totaltrade, lowstockreport, date);
                    string customer = "";
                    try
                    {
                        if (metroRadioButton6.Checked == true)
                        {
                            date = " تاریخ سے " + (metroDateTime_From.Value.Date.Year.ToString() + "-" + metroDateTime_From.Value.Date.Month.ToString() + "-" + metroDateTime_From.Value.Date.Day.ToString()) + " تاریخ تک " + (metroDateTime_To.Value.Date.Year.ToString() + "-" + metroDateTime_To.Value.Date.Month.ToString() + "-" + metroDateTime_To.Value.Date.Day.ToString());
                        }
                        else
                        {
                            date = " تاریخ " + (DateTime.Now.Date.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString());
                        }
                        if (metroRadioButton4.Checked == true || (metroRadioButton6.Checked == true && metroRadioButton9.Checked == true))
                        {
                            customer = textBox4.Text.Trim();
                        }
                        else
                        {
                            customer = "تمام ریکارڈ";
                        }

                    }
                    catch { }
                    gm.PrintBillItems(dt, date, customer);
                }
                else
                {
                    MessageBox.Show("No Record Found To Print");
                    return;
                }
            }
            catch { }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                progressBar1.Show();
                progressBar1.BringToFront();
                int changedcount = 0;

                foreach (DataGridViewRow d in dataGridView1.Rows)
                {
                    try
                    {
                        if (d.Cells["Column18"].Value.ToString() == "1")
                            changedcount++;
                    }
                    catch (Exception ex) { MessageBox.Show("r" + ex.Message); }
                }
                //MessageBox.Show(changedcount.ToString());
                if (changedcount <= 0)
                {
                    MessageBox.Show("No Record To Change");
                    progressBar1.Hide();
                    getAllRecordSelectedCustomer();
                    backgroundWorker1.CancelAsync();
                    return;
                }
                try
                {
                    string customerquery = "(";
                    for (int i = 0; i < selectedmaincustomers.Count - 1; i++)
                    {
                        if (i == 0)
                        {
                            customerquery += " cov.customer_or_vendor_name=N'" + selectedmaincustomers[i].ToString() + "'";
                        }
                        else
                        {
                            customerquery += " or cov.customer_or_vendor_name=N'" + selectedmaincustomers[i].ToString() + "'";
                        }
                    }
                    if (selectedmaincustomers.Count > 1)
                    {
                        customerquery += " or cov.customer_or_vendor_name=N'" + selectedmaincustomers[selectedmaincustomers.Count - 1].ToString() + "'";
                    }
                    else
                    {
                        customerquery += " cov.customer_or_vendor_name=N'" + selectedmaincustomers[selectedmaincustomers.Count - 1].ToString() + "'";
                    }
                    ///ok
                    customerquery += ")";
                    try
                    {
                        string query = "";
                        int ra = 0;
                        List<string> bill_ids = new List<string>();
                        int ii = 0;

                        string todaydate = "";
                        try
                        {
                            todaydate = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
                        }
                        catch { }

                        query = @"select * from Bill_Details 
                                        inner join
                                    Bill on Bill_Details.Bill_Id=bill.id
                                    inner join
                                    customer_or_vendor on customer_or_vendor.id=bill.customer_vendor_id
                                    inner join
                                    customer_or_vendor as cov on customer_or_vendor.parent=cov.id
                                    where Bill_Details.item_type=N'نارمل' and Bill.Bill_Date='" + todaydate + "' and (" + customerquery + ")";
                        DataTable dtBillItems = gm.GetTable(query);
                        if (dtBillItems.Rows.Count <= 0)
                        {
                            return;
                        }

                        foreach (DataGridViewRow d in dataGridView1.Rows)
                        {
                            if (d.Cells["Column18"].Value.ToString() == "1")
                            {
                                backgroundWorker1.ReportProgress(ii);
                                ii++;
                                //query = "select * from Bill_Details where item_type=N'نارمل' and Bill_Id in (select id from Bill where Bill_Date='" + todaydate + "' and id in (select id from bill where Customer_Vendor_Id in (select id from Customer_Or_Vendor where status='1' and parent in (select id from Customer_Or_Vendor where status='1' and id in (select parent from Customer_Or_Vendor where status='1' and id in (select Customer_Vendor_Id from Bill where id in(select Bill_Id from Bill_Details where (item_type=N'نارمل' or item_type=N'بل میں') and Bill_Id in (select id from Bill where Status='1' and Bill_Date='" + todaydate + "' and customer_vendor_id in (select id from customer_or_vendor where parent in (select id from customer_or_vendor where " + customerquery + "))))))))))";
                                //all items not specific by id
                                //query = "select * from Bill_Details where item_type=N'نارمل' and Bill_Id in (select id from Bill where Bill_Date='" + todaydate + "' and customer_vendor_id in (select id from customer_or_vendor where parent in (select id from customer_or_vendor where " + customerquery + ")))";
                                foreach (DataRow dbillitems in dtBillItems.Rows)
                                {
                                    string subCustomer_ItemId = dbillitems["Item_Id"].ToString();
                                    if (d.Cells[0].Value.ToString() == subCustomer_ItemId)
                                    {
                                        double qty = 0;
                                        try
                                        {
                                            qty = double.Parse(dbillitems["Qty"].ToString());
                                        }
                                        catch { }
                                        double unit_cost = 0;
                                        try
                                        {
                                            unit_cost = double.Parse(d.Cells[7].Value.ToString());
                                        }
                                        catch { }
                                        double mazdori = 0;
                                        try
                                        {
                                            mazdori = double.Parse(d.Cells[8].Value.ToString());
                                        }
                                        catch { }
                                        double total_amount = 0;
                                        try
                                        {
                                            total_amount = qty * (unit_cost + mazdori);
                                            total_amount = (Math.Round((total_amount), 0, MidpointRounding.AwayFromZero));
                                        }
                                        catch { }
                                        string item_cost = "";
                                        query = "select retail_price from items where id='" + dbillitems["item_id"].ToString() + "'";
                                        try
                                        {
                                            item_cost = gm.GetTable(query).Rows[0][0].ToString();
                                        }
                                        catch { }
                                        string bill_item_cost = gm.removePointsZero(gm.removePoints(dbillitems["unit_cost"].ToString()));
                                        if (d.Cells[7].Value.ToString() != bill_item_cost)
                                        {
                                            query = "update bill_details set Unit_Cost='" + d.Cells[7].Value.ToString() + "',mazdori='" + d.Cells[8].Value.ToString() + "',Total_Amount='" + total_amount.ToString() + "',update_status='1',update_date='" + DateTime.Now.ToShortDateString() + "' where id = '" + dbillitems[0].ToString() + "' and item_type=N'نارمل'";
                                            gm.ExecuteNonQuery(query);

                                            query = "update bill_details set update_status='' where item_id = '" + subCustomer_ItemId.ToString() + "' and item_type=N'نارمل' and unit_cost!='" + d.Cells[7].Value.ToString() + "' and bill_id in (select id from bill where bill_date='" + todaydate + "')";
                                            gm.ExecuteNonQuery(query);
                                        }
                                        else
                                        {
                                            query = "update bill_details set mazdori='" + d.Cells[8].Value.ToString() + "',Total_Amount='" + total_amount.ToString() + "' where id = '" + dbillitems[0].ToString() + "' and item_type=N'نارمل'";
                                            gm.ExecuteNonQuery(query);
                                        }
                                        if (!bill_ids.Contains(dbillitems["Bill_Id"].ToString()))
                                        {
                                            bill_ids.Add(dbillitems["Bill_Id"].ToString());
                                        }
                                        query = "update items set Retail_Price='" + d.Cells[7].Value.ToString() + "',mazdori='" + d.Cells[8].Value.ToString() + "',update_status='1',update_date='" + DateTime.Now.ToShortDateString() + "' where id = '" + dbillitems["Item_Id"].ToString() + "'";
                                        gm.ExecuteNonQuery(query);
                                        ra = 1;
                                    }
                                }
                                foreach (string bill_id in bill_ids)
                                {
                                    try
                                    {
                                        query = "select sum(Total_Amount) from Bill_Details where Bill_Id='" + bill_id + "'";
                                        DataTable dt = gm.GetTable(query);
                                        double total_amount = 0;
                                        try
                                        {
                                            total_amount = double.Parse(dt.Rows[0][0].ToString());
                                            total_amount = (Math.Round((total_amount), 0, MidpointRounding.AwayFromZero));
                                        }
                                        catch { }
                                        query = "update bill set Net_Total_Amount='" + total_amount.ToString() + "',Total_Amount='" + total_amount.ToString() + "',Balance='" + total_amount.ToString() + "' where id = '" + bill_id + "'";
                                        gm.ExecuteNonQuery(query);
                                    }
                                    catch { }
                                }
                            }
                        }
                        if (ra == 1)
                        {
                            MessageBox.Show("Records Updated Successfully");
                            progressBar1.Hide();
                            getAllRecordSelectedCustomer();
                            backgroundWorker1.CancelAsync();
                        }
                        if (metroRadioButton4.Checked == true)
                        {
                            try
                            {
                                if (dataGridView1.Rows.Count > 0)
                                {
                                    dataGridView1.Rows.Clear();
                                }
                                if (textBox4.Text.ToString().Trim().Length > 0)
                                {
                                    string customerid = "";
                                    try
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
                                    catch { }
                                    if (customerid != "")
                                    {
                                        getCustomerNameRecord();
                                    }
                                }
                            }
                            catch { }
                        }
                        else if (metroRadioButton5.Checked == true)
                        {
                            getCustomerNameRecord();
                            //metroRadioButton5.Checked = false;
                            //metroRadioButton5.PerformClick();
                        }
                    }
                    catch
                    {

                    }
                }
                catch
                {
                    MessageBox.Show("a");
                    progressBar1.Hide();
                    backgroundWorker1.CancelAsync();
                }
            }
            catch { }
            //OLD WORK/////////////////////////////////////////////////////////////////////////////////////////////
            //progressBar1.Show();
            //progressBar1.BringToFront();
            //try
            //{
            //    try
            //    {
            //        string query = "";
            //        int ra = 0;
            //        List<string> bill_ids = new List<string>();
            //        int ii = 0;
            //        foreach (DataGridViewRow d in dataGridView1.Rows)
            //        {
            //            backgroundWorker1.ReportProgress(ii);
            //            ii++;
            //            string todaydate = "";
            //            try
            //            {
            //                todaydate = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
            //            }
            //            catch { }
            //            //query = "select * from Bill_Details where item_type=N'نارمل' and Bill_Id in (select id from Bill where Bill_Date='" + todaydate + "')";
            //            query = "select * from Bill_Details where item_type=N'نارمل' and Bill_Id in (select id from Bill where Bill_Date='" + todaydate + "' and id in (select id from bill where Customer_Vendor_Id in (select id from Customer_Or_Vendor where status='1' and parent in (select id from Customer_Or_Vendor where status='1' and id in (select parent from Customer_Or_Vendor where status='1' and id in (select Customer_Vendor_Id from Bill where id in(select Bill_Id from Bill_Details where (item_type=N'نارمل' or item_type=N'بل میں') and Bill_Id in (select id from Bill where Status='1' and Bill_Date='" + todaydate + "'))))))))";
            //            DataTable dtBillItems = gm.GetTable(query);
            //            if (dtBillItems.Rows.Count <= 0)
            //            {
            //                return;
            //            }
            //            foreach (DataRow dbillitems in dtBillItems.Rows)
            //            {
            //                string subCustomer_ItemId = dbillitems["Item_Id"].ToString();
            //                if (d.Cells[0].Value.ToString() == subCustomer_ItemId)
            //                {
            //                    double qty = 0;
            //                    try
            //                    {
            //                        qty = double.Parse(dbillitems["Qty"].ToString());
            //                    }
            //                    catch { }
            //                    double unit_cost = 0;
            //                    try
            //                    {
            //                        unit_cost = double.Parse(d.Cells[7].Value.ToString());
            //                    }
            //                    catch { }
            //                    double mazdori = 0;
            //                    try
            //                    {
            //                        mazdori = double.Parse(d.Cells[8].Value.ToString());
            //                    }
            //                    catch { }
            //                    double total_amount = 0;
            //                    try
            //                    {
            //                        total_amount = qty * (unit_cost + mazdori);
            //                    }
            //                    catch { }
            //                    string item_cost = "";
            //                    query = "select retail_price from items where id='" + dbillitems["item_id"].ToString() + "'";
            //                    try
            //                    {
            //                        item_cost = gm.GetTable(query).Rows[0][0].ToString();
            //                    }
            //                    catch { }
            //                    string bill_item_cost = gm.removePointsZero(gm.removePoints(dbillitems["unit_cost"].ToString()));
            //                    if (d.Cells[7].Value.ToString() != bill_item_cost)
            //                    {
            //                        query = "update bill_details set Unit_Cost='" + d.Cells[7].Value.ToString() + "',mazdori='" + d.Cells[8].Value.ToString() + "',Total_Amount='" + total_amount.ToString() + "',update_status='1',update_date='" + DateTime.Now.ToShortDateString() + "' where id = '" + dbillitems[0].ToString() + "' and item_type=N'نارمل'";
            //                        gm.ExecuteNonQuery(query);
            //                    }
            //                    if (!bill_ids.Contains(dbillitems["Bill_Id"].ToString()))
            //                    {
            //                        bill_ids.Add(dbillitems["Bill_Id"].ToString());
            //                    }
            //                    query = "update items set Retail_Price='" + d.Cells[7].Value.ToString() + "',mazdori='" + d.Cells[8].Value.ToString() + "',update_status='1',update_date='" + DateTime.Now.ToShortDateString() + "' where id = '" + dbillitems["Item_Id"].ToString() + "'";
            //                    gm.ExecuteNonQuery(query);
            //                    ra = 1;
            //                }
            //            }
            //            foreach (string bill_id in bill_ids)
            //            {
            //                try
            //                {
            //                    query = "select sum(Total_Amount) from Bill_Details where Bill_Id='" + bill_id + "'";
            //                    DataTable dt = gm.GetTable(query);
            //                    double total_amount = 0;
            //                    try
            //                    {
            //                        total_amount = double.Parse(dt.Rows[0][0].ToString());
            //                    }
            //                    catch { }
            //                    query = "update bill set Net_Total_Amount='" + total_amount.ToString() + "',Total_Amount='" + total_amount.ToString() + "',Balance='" + total_amount.ToString() + "' where id = '" + bill_id + "'";
            //                    gm.ExecuteNonQuery(query);
            //                }
            //                catch { }
            //            }
            //        }
            //        if (ra == 1)
            //        {
            //            MessageBox.Show("Records Updated Successfully");
            //            progressBar1.Hide();
            //            backgroundWorker1.CancelAsync();
            //        }
            //        if (metroRadioButton4.Checked == true)
            //        {
            //            try
            //            {
            //                if (dataGridView1.Rows.Count > 0)
            //                {
            //                    dataGridView1.Rows.Clear();
            //                }
            //                if (textBox4.Text.ToString().Trim().Length > 0)
            //                {
            //                    string customerid = "";
            //                    try
            //                    {
            //                        string id = "";
            //                        try//only word if main customer or sub sub customer name is unique
            //                        {
            //                            query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + textBox4.Text.Trim() + "' and status='1'";
            //                            id = gm.GetTable(query).Rows[0][0].ToString();
            //                        }
            //                        catch { }
            //                        if (id == "")
            //                        {
            //                            return;
            //                        }
            //                        customerid = id;
            //                    }
            //                    catch { }
            //                    if (customerid != "")
            //                    {
            //                        getCustomerNameRecord();
            //                    }
            //                }
            //            }
            //            catch { }
            //        }
            //        else if (metroRadioButton5.Checked == true)
            //        {
            //            metroRadioButton5.Checked = false;
            //            metroRadioButton5.PerformClick();
            //        }
            //    }
            //    catch { }
            //}
            //catch
            //{
            //    progressBar1.Hide();
            //    backgroundWorker1.CancelAsync();
            //}
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            progressBar1.Update();
        }

        private void metroRadioButton5_Click(object sender, EventArgs e)
        {
            groupBox3.Hide();
            try
            {
                listBox1.Items.Clear();
                selectedmaincustomers = new List<string>();
                if (metroRadioButton5.Checked == true)
                {
                    MainCustomers z = new MainCustomers();
                    z.ShowDialog();
                    selectedmaincustomers = z.maincustomersnames;
                    foreach (string a in selectedmaincustomers)
                    {
                        listBox1.Items.Add(a);
                    }
                }
                dataGridView1.Columns[14].Visible = true;
                metroTile3.Show();
                metroTile4.Show();
                dataGridView1.Columns[15].Visible = false;
                metroTile3.Visible = false;
                textBox4.Visible = false;
                if (selectedmaincustomers.Count > 0)
                {
                    getAllRecordSelectedCustomer();
                }
                groupBox2.Hide();
            }
            catch { }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void metroRadioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (metroRadioButton8.Checked == true)
            {
                textBox4.Hide();
                textBox4.Text = "";
                getAllRecordBetweenDatesViewAll();
            }
        }

        private void metroRadioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (metroRadioButton9.Checked == true)
            {
                textBox4.Show();
            }
        }

        private void Customers_Bill_Rate_Update_Shown(object sender, EventArgs e)
        {
            try
            {
                //if (Pos_Sabzi_Mandi.Properties.Settings.Default.logintype == "User" || Pos_Sabzi_Mandi.Properties.Settings.Default.logintype == "user")
                //{
                //    metroTile1.Visible = false;
                //    dataGridView1.Columns[12].Visible = false;
                //    dataGridView1.Columns[13].Visible = false;
                //}
                //else
                //{
                metroTile1.Visible = true;
                //dataGridView1.Columns[12].Visible = true;
                //dataGridView1.Columns[13].Visible = true;
                //}
                dataGridView1.Columns[4].Visible = true;
                source = new AutoCompleteStringCollection();
                itemCategory1.treeView1AfterSelect += new EventHandler(treeView1_AfterSelect);//usercontrol->itemcategory work ref 1
                treeView1 = (itemCategory1.Controls["treeView1"] as TreeView);
                metroTextBoxCategory.Focus();
                metroTextBox1.Hide();

                this.AutoScroll = true;
                metroRadioButton1.Checked = true;

                itemCategory1.Hide();
                metroTextBoxCategory.Hide();



                foreach (TreeNode node in treeView1.Nodes) collectChildren(node);


                metroTextBoxCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                metroTextBoxCategory.AutoCompleteCustomSource = source;
                metroTextBoxCategory.AutoCompleteSource = AutoCompleteSource.CustomSource;

                main_sourceCustomer_Or_Vendor = new AutoCompleteStringCollection();
                DataTable dt = gm.GetTable("select * from customer_or_vendor where status ='1' and customer_vendor_type='Customer' and parent=''");
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

                dt = gm.GetTable("select * from items where status !='-1' order by Name ASC");
                foreach (DataRow d in dt.Rows)
                {
                    sourceitemname.Add(d["name"].ToString() + " (" + d["id"].ToString() + ")");
                }

                metroTextBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                metroTextBox1.AutoCompleteCustomSource = sourceitemname;
                metroTextBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            }
            catch { }
            //dataGridView1.Columns[7].ReadOnly = false;
            metroRadioButton1.Checked = true;
            textBox4.Focus();
            metroRadioButton4.Checked = true;
            metroRadioButton5.Checked = false;
            try
            {
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    c.DefaultCellStyle.Font = new Font("Jameel Noori Nastaleeq", 19.5F, GraphicsUnit.Pixel);
                }
            }
            catch { }
        }

        private void metroLabel6_Click(object sender, EventArgs e)
        {

        }
    }

    public class ItemAndStatus
    {
        public ItemAndStatus(string id, string rate, string item_type)
        {
            this.Id = id; this.Rate = rate; this.Item_Type = item_type;
        }
        public string Id
        { get; set; }
        public string Rate
        { get; set; }
        public string Item_Type
        { get; set; }
    }
}
