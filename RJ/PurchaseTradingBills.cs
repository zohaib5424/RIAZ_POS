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

namespace POS_GM
{
    public partial class PurchaseTradingBills : Form
    {
        SqlConnection con = new SqlConnection(POS_GM.Properties.Settings.Default.Connectionstring);
        public PurchaseTradingBills()
        {
            InitializeComponent();
        }

        AutoCompleteStringCollection source = new AutoCompleteStringCollection();
        AutoCompleteStringCollection sourceusername = new AutoCompleteStringCollection();
        private void ItemsList_Load(object sender, EventArgs e)
        {
            try
            {
                source = new AutoCompleteStringCollection();
                sourceusername = new AutoCompleteStringCollection();
                metroTextBox1.Hide();

                this.AutoScroll = true;
                metroRadioButton1.Checked = true;


                string customervendortype = "Vendor";
                DataTable dt = gm.GetTable("select * from customer_or_vendor where customer_vendor_type='"+customervendortype+"' and status ='1'");
                foreach (DataRow d in dt.Rows)
                {
                    sourceusername.Add(d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")");
                }

                metroTextBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                metroTextBox1.AutoCompleteCustomSource = sourceusername;
                metroTextBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
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
                string query = "Select * from bill where bill_type='"+"Purchase Trading"+"' and status != '-1'";
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
                MessageBox.Show(ex.Message);
            }
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
                if (dataGridView2.Rows.Count > 0)
                {
                    dataGridView2.Rows.Clear();
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
                MessageBox.Show(ex.Message);
            }
        }

        public void getUserNameRecord(string id)
        {
            try
            {
                string query = "Select * from bill where bill_type='" + "Purchase Trading" + "' and customer_vendor_id='" + id + "' and status != '-1'";
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
                MessageBox.Show(ex.Message);
            }
        }

        public void getTransactionIdRecord(string id)
        {
            try
            {
                string query = "Select * from bill where bill_type='" + "Purchase Trading" + "' and bill_id='" + id + "' and status != '-1'";
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
                MessageBox.Show(ex.Message);
            }
        }

        private void metroRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            metroTextBox2.Hide();
            metroTextBox1.Hide();
            getAllRecord();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    if (dataGridView3.Rows.Count > 0)
                        dataGridView3.Rows.Clear();
                    string query = "select * from bill_details where bill_id = '"+dataGridView1.SelectedRows[0].Cells[0].Value.ToString().Trim()+"'";
                    DataTable dt = gm.GetTable(query);
                    foreach(DataRow d in dt.Rows)
                    {
                        string item = "";
                        query = "select * from items where id = '" + d["item_id"].ToString() + "'";
                        DataTable dt2 = gm.GetTable(query);
                        try
                        {
                            item = dt2.Rows[0]["Name"].ToString();
                        }
                        catch { }
                        dataGridView3.Rows.Add(d["item_id"].ToString(), item, d["unit_cost"].ToString(), d["qty"].ToString(), d["total_amount"].ToString(),d["manufacturing_date"].ToString(),d["expiry_date"].ToString(),d["batch_or_lot_number"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
            metroTextBox2.Hide();
            metroTextBox1.Show();
            metroTextBox1.Text = string.Empty;
            metroTextBox1.Focus();
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
                if (metroTextBox1.Text.Trim() == "")
                    getAllRecord();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroRadioButton4_CheckedChanged(object sender, EventArgs e)
        {
            metroTextBox1.Hide();
            metroTextBox2.Show();
            metroTextBox2.Text = string.Empty;
            metroTextBox2.Focus();
        }

        private void metroTextBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string[] s = metroTextBox2.Text.Trim().Split('(');
                string[] a = s[s.Length - 1].Trim().Split(')');
                string id = a[0].ToString();
                if (id.Trim() != "")
                {
                    getTransactionIdRecord(id);
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
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    gm.printBill(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(),"Purchase Invoice","Vendor","Duplicate Receipt");
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
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    if (dataGridView3.Rows.Count > 0)
                        dataGridView3.Rows.Clear();
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
                        dataGridView3.Rows.Add(d["item_id"].ToString(), item, d["unit_cost"].ToString(), d["qty"].ToString(), d["total_amount"].ToString(), d["manufacturing_date"].ToString(), d["expiry_date"].ToString(), d["batch_or_lot_number"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
