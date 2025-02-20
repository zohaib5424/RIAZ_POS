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
    public partial class LowStockItemsList : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public LowStockItemsList()
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
        private void ItemsList_Load(object sender, EventArgs e)
        {
            try
            {
                source = new AutoCompleteStringCollection();
                itemCategory1.treeView1AfterSelect += new EventHandler(treeView1_AfterSelect);//usercontrol->itemcategory work ref 1
                treeView1 = (itemCategory1.Controls["treeView1"] as TreeView);
                metroTextBoxCategory.Focus();

                this.AutoScroll = true;
                metroRadioButton1.Checked = true;

                itemCategory1.Hide();
                metroTextBoxCategory.Hide();



                foreach (TreeNode node in treeView1.Nodes) collectChildren(node);


                metroTextBoxCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                metroTextBoxCategory.AutoCompleteCustomSource = source;
                metroTextBoxCategory.AutoCompleteSource = AutoCompleteSource.CustomSource;
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
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                string query = "Select * from items where status = '1'";
                DataTable dt = gm.GetTable(query);
                foreach (DataRow d in dt.Rows)
                {
                    double qty = 0;
                    double alertqty = 0;
                    try
                    {
                        qty = double.Parse(d["qty"].ToString());
                    }
                    catch { }
                    try
                    {
                        alertqty = double.Parse(d["alertqty"].ToString());
                    }
                    catch { }
                    if (qty <= alertqty)
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
                        if (d["status"].ToString() == "1")
                            dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), brand, unit, d["Retail_Price"].ToString(), d["Purchase_Price"].ToString(), d["Qty"].ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Disable", "Delete");
                        else
                            dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), brand, unit, d["Retail_Price"].ToString(), d["Purchase_Price"].ToString(), d["Qty"].ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Enable", "Delete");
                        if (d["status"].ToString() == "0")
                        {
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                        }
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
                string query = "Select * from items where category_id='"+category_id+"' and status = '1'";
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                DataTable dt = gm.GetTable(query);
                foreach (DataRow d in dt.Rows)
                {
                    double qty = 0;
                    double alertqty = 0;
                    try
                    {
                        qty = double.Parse(d["qty"].ToString());
                    }
                    catch { }
                    try
                    {
                        alertqty = double.Parse(d["alertqty"].ToString());
                    }
                    catch { }
                    if (qty <= alertqty)
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
                        if (d["status"].ToString() == "1")
                            dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), brand, unit, d["Retail_Price"].ToString(), d["Purchase_Price"].ToString(), d["Qty"].ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Disable", "Delete");
                        else
                            dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), brand, unit, d["Retail_Price"].ToString(), d["Purchase_Price"].ToString(), d["Qty"].ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Enable", "Delete");
                        if (d["status"].ToString() == "0")
                        {
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                        }
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
            itemCategory1.Hide();
            metroTextBoxCategory.Hide();
            getAllRecord();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                if (e.ColumnIndex == 10)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[10].Value.ToString() == "Enable")
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void metroRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            treeView1.CollapseAll();
            itemCategory1.Show();
            metroTextBoxCategory.Text = string.Empty;
            metroTextBoxCategory.Show();
            metroTextBoxCategory.Focus();
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
                if (metroTextBoxCategory.Text.Trim() == "")
                    getAllRecord();
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

        }

        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {

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
                        getAllRecord();
                    }
                    else if (metroRadioButton2.Checked == true)
                    {
                        treeView1.CollapseAll();
                        itemCategory1.Show();
                        metroTextBoxCategory.Text = string.Empty;
                        metroTextBoxCategory.Show();
                        metroTextBoxCategory.Focus();
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

        private void metroTile1_Click_1(object sender, EventArgs e)
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
                    string report = "Low Stock Items List";
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
                    int i = 0;
                    foreach (DataGridViewRow d in dataGridView1.Rows)
                    {
                        i++;
                        dt.Rows.Add(i.ToString(), d.Cells[2].Value.ToString(), d.Cells[1].Value.ToString(), d.Cells[3].Value.ToString());
                    }
                    string lowstockreport = "1";
                    string monthName = DateTime.Now.Date.ToString("MMM", CultureInfo.InvariantCulture);
                    string date = DateTime.Now.Day.ToString() + " " + monthName + "," + DateTime.Now.Year.ToString();
                    gm.PrintItemList(dt, report, category, "0", "0", "0", lowstockreport,date);
                }
                else
                {
                    MessageBox.Show("No Record Found To Print");
                    return;
                }
            }
            catch { }
        }

        private void LowStockItemsList_KeyDown(object sender, KeyEventArgs e)
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

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
