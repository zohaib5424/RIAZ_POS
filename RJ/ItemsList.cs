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
    public partial class ItemsList : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public ItemsList()
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
                            metroTextBoxCategory.Text = treeView1.SelectedNode.Text.Trim().ToString() +" ("+treeView1.SelectedNode.Tag.ToString()+")";
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
        private void ItemsList_Load(object sender, EventArgs e)
        {
            metroTile1.Hide();
            //label1.Hide();
            dataGridView1.Columns["Column15"].DisplayIndex = 4;
            metroDateTimeFrom.Hide();
            metroLabel8.Hide();
            try
            {
                //if (Pos_Sabzi_Mandi.Properties.Settings.Default.logintype == "User" || Pos_Sabzi_Mandi.Properties.Settings.Default.logintype == "user")
                //{
                    //metroTile1.Visible = false;
                    //dataGridView1.Columns[8].Visible = false;
                    //dataGridView1.Columns[12].Visible = false;
                    //dataGridView1.Columns[13].Visible = false;
                //}
                //else
                //{
                metroTile1.Visible = true;
                //dataGridView1.Columns[8].Visible = true;
                dataGridView1.Columns[12].Visible = true;
                dataGridView1.Columns[13].Visible = true;
                //}
                source = new AutoCompleteStringCollection();
                itemCategory1.treeView1AfterSelect += new EventHandler(treeView1_AfterSelect);//usercontrol->itemcategory work ref 1
                treeView1 = (itemCategory1.Controls["treeView1"] as TreeView);
                metroTextBoxCategory.Focus();
                comboBox1.Hide();

                this.AutoScroll = true;
                metroRadioButton1.Checked = true;

                itemCategory1.Hide();
                metroTextBoxCategory.Hide();



                foreach (TreeNode node in treeView1.Nodes) collectChildren(node);


                metroTextBoxCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                metroTextBoxCategory.AutoCompleteCustomSource = source;
                metroTextBoxCategory.AutoCompleteSource = AutoCompleteSource.CustomSource;


                DataTable dt = gm.GetTable("select * from items where status !='-1' order by Name ASC");
                foreach (DataRow d in dt.Rows)
                {
                    sourceitemname.Add(d["name"].ToString() + " (" + d["id"].ToString() + ")");
                }

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("id");
                dt2.Columns.Add("name");
                foreach (DataRow d in dt.Rows)
                {
                    int exist = 0;
                    foreach (DataRow d2 in dt2.Rows)
                    {
                        if (d2[1].ToString() == d[2].ToString())
                        {
                            exist = 1;
                        }
                    }
                    if (exist == 0)
                    {
                        dt2.Rows.Add(d["id"].ToString(), d["name"].ToString());
                    }
                }
                
                comboBox1.DisplayMember = "name";
                comboBox1.ValueMember = "id";
                comboBox1.DataSource = dt2;
                comboBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
                comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
                comboBox1.SelectedIndex = -1;
            }
            catch { }
            dataGridView1.Columns[6].ReadOnly = false;
            getAllRecord();

            try
            {
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    c.DefaultCellStyle.Font = new Font("Jameel Noori Nastaleeq", 19.5F, GraphicsUnit.Pixel);
                }
            }
            catch { }
        }

        void collectChildren(TreeNode node)
        {
            if (node.Nodes.Count == 0) source.Add(node.Text + " ("+node.Tag.ToString()+")");
            else foreach (TreeNode n in node.Nodes) collectChildren(n);
        }

        GMDB gm = new GMDB();
        public void getAllRecord()
        {
            try
            {
                string query = "Select * from items where status != '-1' order by Name ASC";
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
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
                        //try
                        //{
                        //    qty = double.Parse(d["Qty"].ToString());
                        //}
                        //catch { }
                        
                        string itemid = d["id"].ToString();
                    //    string date = metroDateTimeFrom.Value.Year.ToString() + "-" + (metroDateTimeFrom.Value.Month.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Month.ToString()) : metroDateTimeFrom.Value.Month.ToString()) + "-" + (metroDateTimeFrom.Value.Day.ToString().Length == 1 ? ("0" + metroDateTimeFrom.Value.Day.ToString()) : metroDateTimeFrom.Value.Day.ToString());
                    //    query = "select sum(Bill_Details.Qty) from Bill_Details inner join Bill on Bill.id=Bill_Details.Bill_Id where Bill.Bill_Date<='"+date+"' and Bill.Bill_Type='purchase trading' and Bill_Details.Item_Id='"+itemid+"' and bill.status='1'";
                    //    DataTable dt2 = gm.GetTable(query);
                    //    //+purchase qty
                    //    try
                    //    {
                    //        qty += double.Parse(dt2.Rows[0][0].ToString());
                    //    }
                    //    catch { }

                    //    query = "select sum(Bill_Details.Qty) from Bill_Details inner join Bill on Bill.id=Bill_Details.Bill_Id where Bill.Bill_Date<='" + date + "' and Bill.Bill_Type='sale trading' and Bill_Details.Item_Id='" + itemid + "' and bill.status='1'";
                    //    dt2 = gm.GetTable(query);
                    //    //-sale qty
                    //    try
                    //    {
                    //        qty -= double.Parse(dt2.Rows[0][0].ToString());
                    //    }
                    //    catch { }

                    //    query = "select sum(Report_Details.Sale_Qty) from Report_Details inner join Report on Report.id=Report_Details.Report_Id where Report.report_date<='" + date + "' and Report_Details.Item_Id='" + itemid + "' and report.status='1'";
                    //    dt2 = gm.GetTable(query);
                    //    //-wholesale qty
                    //    try
                    //    {
                    //        qty -= double.Parse(dt2.Rows[0][0].ToString());
                    //    }
                    //    catch { }
                    }
                    catch { }
                    if (d["status"].ToString() == "1")
                    {
                        dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), brand, unit, d["Retail_Price"].ToString(), d["Mazdori"].ToString(), d["Purchase_Price"].ToString(), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Disable", "Delete",d["weight"].ToString());
                    }
                    else
                    {
                        dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), brand, unit, d["Retail_Price"].ToString(), d["Mazdori"].ToString(), d["Purchase_Price"].ToString(), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Enable", "Delete", d["weight"].ToString());
                    }
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
                string query = "Select * from items where category_id='" + category_id + "' and status != '-1' order by Name ASC";
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
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
                        dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), brand, unit, d["Retail_Price"].ToString(), d["Mazdori"].ToString(), d["Purchase_Price"].ToString(), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Disable", "Delete", d["weight"].ToString());
                    else
                        dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), brand, unit, d["Retail_Price"].ToString(), d["Mazdori"].ToString(), d["Purchase_Price"].ToString(), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Enable", "Delete", d["weight"].ToString());
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

        public void getItemNameRecord(string id)
        {
            try
            {
                string query = "Select * from items where id='" + id + "' and status != '-1' order by Name ASC";
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
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
                        dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), brand, unit, d["Retail_Price"].ToString(), d["Mazdori"].ToString(), d["Purchase_Price"].ToString(), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Disable", "Delete", d["weight"].ToString());
                    else
                        dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), brand, unit, d["Retail_Price"].ToString(), d["Mazdori"].ToString(), d["Purchase_Price"].ToString(), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Enable", "Delete", d["weight"].ToString());
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

        public void getItemNameRecord1(string name)
        {
            try
            {
                string query = "Select * from items where name = N'" + name + "' and status != '-1' order by Name ASC";
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
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
                        dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), brand, unit, d["Retail_Price"].ToString(), d["Mazdori"].ToString(), d["Purchase_Price"].ToString(), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Disable", "Delete", d["weight"].ToString());
                    else
                        dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), brand, unit, d["Retail_Price"].ToString(), d["Mazdori"].ToString(), d["Purchase_Price"].ToString(), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Enable", "Delete", d["weight"].ToString());
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
            //metroTile2.Show();
            //label1.Show();
            itemCategory1.Hide();
            metroTextBoxCategory.Hide();
            comboBox1.Hide();
            getAllRecord();
            //if (Pos_Sabzi_Mandi.Properties.Settings.Default.logintype == "User" || Pos_Sabzi_Mandi.Properties.Settings.Default.logintype == "user")
            //{
            //    metroTile1.Visible = false;
            //}
            //else
            //{
                metroTile1.Visible = true;
            //}
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                try
                {
                    //dataGridView1.Rows[e.RowIndex].Selected = true;
                    if (e.ColumnIndex == 12)
                    {
                        if (dataGridView1.Rows[e.RowIndex].Cells[12].Value.ToString() == "Enable")
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
                                    string id = comboBox1.SelectedValue.ToString();
                                    if (id.Trim() != "")
                                    {
                                        getItemNameRecord(id);
                                    }
                                    if (comboBox1.Text.Trim() == "" && comboBox1.SelectedValue==null)
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
                                    //string[] s = metroTextBox1.Text.Trim().Split('(');
                                    //string[] a = s[s.Length - 1].Trim().Split(')');
                                    //string id = a[0].ToString();
                                    string id = comboBox1.SelectedValue.ToString();
                                    if (id.Trim() != "")
                                    {
                                        getItemNameRecord(id);
                                    }
                                    if (comboBox1.Text.Trim() == "" && comboBox1.SelectedValue == null)
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
                    if (e.ColumnIndex == 13)
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

                        DataTable dt = gm.GetTable("select * from items where status !='-1' order by Name ASC");
                        foreach (DataRow d in dt.Rows)
                        {
                            sourceitemname.Add(d["name"].ToString() + " (" + d["id"].ToString() + ")");
                        }

                        comboBox1.DisplayMember = "name";
                        comboBox1.ValueMember = "id";
                        comboBox1.DataSource = dt;
                        comboBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
                        comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
                        comboBox1.SelectedIndex = -1;
                        if (metroRadioButton1.Checked == true)
                        {
                            getAllRecord();
                        }
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
                                //if (metroTextBoxCategory.Text.Trim() == "")
                                //    getAllRecord();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else if (metroRadioButton3.Checked == true)
                        {
                            comboBox1.Focus();
                            SendKeys.Send("{RIGHT}");
                        }
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
            comboBox1.Hide();
            metroTextBoxCategory.Focus();
            //if (Pos_Sabzi_Mandi.Properties.Settings.Default.logintype == "User" || Pos_Sabzi_Mandi.Properties.Settings.Default.logintype == "user")
            //{
            //    metroTile1.Visible = false;
            //}
            //else
            //{
                metroTile1.Visible = true;
            //}
        }

        private void itemCategory1_Load(object sender, EventArgs e)
        {

        }

        private void metroTextBoxCategory_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBoxCategory_TextChanged(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.Clear();
            }
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
            metroTile2.Hide();
            label1.Hide();
            itemCategory1.Hide();
            metroTextBoxCategory.Hide();
            comboBox1.Show();
            comboBox1.Text = string.Empty;
            comboBox1.SelectedIndex = -1;
            comboBox1.Focus();
            //if (Pos_Sabzi_Mandi.Properties.Settings.Default.logintype == "User" || Pos_Sabzi_Mandi.Properties.Settings.Default.logintype == "user")
            //{
            //    metroTile1.Visible = false;
            //}
            //else
            //{
                metroTile1.Visible = true;
            //}
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
        }

        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string id = comboBox1.SelectedValue.ToString();
                if (id.Trim() != "")
                {
                    getItemNameRecord(id);
                }
                //if (comboBox1.Text.Trim() == "" && comboBox1.SelectedValue == null)
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
                        comboBox1.Hide();
                        getAllRecord();
                    }
                    else if (metroRadioButton2.Checked == true)
                    {
                        treeView1.CollapseAll();
                        itemCategory1.Show();
                        metroTextBoxCategory.Text = string.Empty;
                        metroTextBoxCategory.Show();
                        comboBox1.Hide();
                        metroTextBoxCategory.Focus();
                    }
                    else if (metroRadioButton3.Checked == true)
                    {
                        itemCategory1.Hide();
                        metroTextBoxCategory.Hide();
                        comboBox1.Show();
                        comboBox1.Text = string.Empty;
                        comboBox1.SelectedIndex = -1;
                        comboBox1.Focus();
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
                if (e.Modifiers == Keys.Control && e.KeyCode == Keys.P)
                {
                    if(metroRadioButton3.Checked == false)
                        metroTile2.PerformClick();
                }
                if (e.Modifiers == Keys.Control && e.KeyCode == Keys.S)
                {
                    metroTile3.PerformClick();
                }
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
                    dt.Columns.Add("Item");
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
                            //, d.Cells[9].Value.ToString(), d.Cells[9].Value.ToString(), d.Cells[6].Value.ToString()
                            invoice = (double.Parse(d.Cells[9].Value.ToString().Trim().ToString())*double.Parse(d.Cells[8].Value.ToString().Trim().ToString())).ToString();
                        }
                        catch { }
                        try
                        {
                            trade = (double.Parse(d.Cells[9].Value.ToString().Trim().ToString())*double.Parse(d.Cells[6].Value.ToString().Trim().ToString())).ToString();
                        }
                        catch { }
                        try
                        {
                            totalinvoice = (double.Parse(totalinvoice) + double.Parse(invoice)).ToString();
                        }
                        catch { }
                        try
                        {
                            totaltrade = (double.Parse(totaltrade)+double.Parse(trade)).ToString();
                        }
                        catch { }
                        try
                        {
                            totalqty = (double.Parse(totalqty.ToString().Trim().ToString()) + double.Parse(d.Cells[9].Value.ToString().Trim().ToString())).ToString();
                        }
                        catch { }
                        dt.Rows.Add(d.Cells[5].Value.ToString(), d.Cells[2].Value.ToString(), d.Cells[3].Value.ToString());
                    }
                    string lowstockreport = "0";
                    string monthName = metroDateTimeFrom.Value.Date.ToString("MMM", CultureInfo.InvariantCulture);
                    //string date = metroDateTimeFrom.Value.Day.ToString() + " " + monthName + "," + metroDateTimeFrom.Value.Year.ToString();
                    string date = metroDateTimeFrom.Value.Day.ToString() + "-" + metroDateTimeFrom.Value.Month.ToString() + "-" + metroDateTimeFrom.Value.Year.ToString();
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
                        //string[] s = metroTextBox1.Text.Trim().Split('(');
                        //string[] a = s[s.Length - 1].Trim().Split(')');
                        //string id = a[0].ToString();
                        string id = comboBox1.SelectedValue.ToString();
                        if (id.Trim() != "")
                        {
                            getItemNameRecord(id);
                        }
                        if (comboBox1.Text.Trim() == "" && comboBox1.SelectedValue == null)
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

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
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
            catch { }
        }

        private void metroTile3_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "";
                foreach (DataGridViewRow d in dataGridView1.Rows)
                {
                    query = "update items set Retail_Price='" + d.Cells[6].Value.ToString() + "',Mazdori='" + (d.Cells[7].Value.ToString() == "" ? "0" : d.Cells[7].Value.ToString()) + "',Weight='" + (d.Cells[14].Value.ToString() == "" ? "0" : d.Cells[14].Value.ToString()) + "' where id = '" + d.Cells[0].Value.ToString() + "'";
                    gm.ExecuteNonQuery(query);
                }
                MessageBox.Show("Records Updated Successfully");
                if (metroRadioButton1.Checked == true)
                {
                    metroRadioButton1.Checked = false;
                    metroRadioButton1.PerformClick();

                }
                if (metroRadioButton2.Checked == true)
                {
                    metroRadioButton2.Checked = false;
                    metroRadioButton2.PerformClick();

                }
                if (metroRadioButton3.Checked == true)
                {
                    metroRadioButton3.Checked = false;
                    metroRadioButton3.PerformClick();

                }
            }
            catch { }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //comboBox1.Focus();
                //SendKeys.Send("{ENTER}");
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                if (comboBox1.Text.Trim() != "")
                {
                    //string id = comboBox1.SelectedValue.ToString();
                    //if (id.Trim() != "")
                    if (comboBox1.Text.Trim() != "")
                    {
                        //getItemNameRecord(id);
                        getItemNameRecord1(comboBox1.Text.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //comboBox1.Focus();
                //SendKeys.Send("{ENTER}");
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                if (comboBox1.Text.Trim() != "")
                {
                    //string id = comboBox1.SelectedValue.ToString();
                    //if (id.Trim() != "")
                    if (comboBox1.Text.Trim() != "")
                    {
                        //getItemNameRecord(id);
                        getItemNameRecord1(comboBox1.Text.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void comboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                SendKeys.Send("{RIGHT}");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
