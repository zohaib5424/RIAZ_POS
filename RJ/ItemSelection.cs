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
    public partial class ItemSelection : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public ItemSelection()
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
        public string itemname = "";
        private void ItemsList_Load(object sender, EventArgs e)
        {
            dataGridView1.Columns[4].DefaultCellStyle.BackColor = Color.Yellow;
            try
            {
                getUnits();
                getWeight();
                getAllRecord();

                metroComboBoxWeight.SelectedIndex = 0;
                metroComboBoxUnit.SelectedIndex = 0;

                if (RJ.Properties.Settings.Default.logintype == "User" || RJ.Properties.Settings.Default.logintype == "user")
                {
                    //dataGridView1.Columns[7].Visible = false;
                    //dataGridView1.Columns[15].Visible = false;
                    //dataGridView1.Columns[16].Visible = false;
                }
                else
                {
                    //dataGridView1.Columns[7].Visible = true;
                    //dataGridView1.Columns[15].Visible = true;
                    //dataGridView1.Columns[16].Visible = true;
                }
                source = new AutoCompleteStringCollection();

                this.AutoScroll = true;


                try
                {
                    foreach (TreeNode node in treeView1.Nodes) collectChildren(node);
                }
                catch { }



                DataTable dt = gm.GetTable("select * from items where status !='-1' and name='"+itemname+"'");
                foreach (DataRow d in dt.Rows)
                {
                    sourceitemname.Add(d["name"].ToString() + " (" + d["id"].ToString() + ")");
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
                string weight = "";
                if (metroComboBoxWeight.SelectedIndex > 0)
                {
                    weight = metroComboBoxWeight.SelectedItem.ToString();
                }
                else
                {
                    weight = "%";
                }
                string unit_id = "";
                if (metroComboBoxUnit.SelectedIndex > 0)
                {
                    if (metroComboBoxUnit.SelectedIndex == 1)
                    {
                        unit_id = "Other";
                    }
                    else
                    {
                        unit_id = metroComboBoxUnit.SelectedValue.ToString();
                    }
                }
                else
                {
                    unit_id = "%";
                }

                string query = "Select * from items where status != '-1' and name=N'" + itemname + "' and weight like '"+weight+"' and unit_id like '"+unit_id+"'";

                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                DataTable dt = gm.GetTable(query);
                DataTable dtitems = new DataTable();
                dtitems.Columns.Add("id");
                dtitems.Columns.Add("Category");
                dtitems.Columns.Add("Item_Name");
                dtitems.Columns.Add("Weight");
                dtitems.Columns.Add("Unit");
                dtitems.Columns.Add("Retail_Price");
                dtitems.Columns.Add("Purchase_Price");
                dtitems.Columns.Add("Quantity");
                dtitems.Columns.Add("Barcode");
                dtitems.Columns.Add("Enable_Disable");
                dtitems.Columns.Add("Delete");
                dtitems.Columns.Add("Status");
                foreach (DataRow d in dt.Rows)
                {
                    string item = d["id"].ToString();
                    query = "Select * from categories where id='" + d["category_id"].ToString() + "'";
                    DataTable dtcategory = gm.GetTable(query);
                    string category = dtcategory.Rows[0][2].ToString();
                    string unit = d["Unit_Id"].ToString();
                    if (d["unit_id"].ToString() != "Other")
                    {
                        query = "Select * from units where id='" + d["unit_id"].ToString() + "'";
                        DataTable dtunit = gm.GetTable(query);
                        unit = dtunit.Rows[0][2].ToString();
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
                        DataTable dt2 = gm.GetTable(query);
                        //+purchase qty
                        try
                        {
                            qty += double.Parse(dt2.Rows[0][0].ToString());
                        }
                        catch { }

                        //-sale qty
                        try
                        {
                            qty -= double.Parse(dt2.Rows[0][0].ToString());
                        }
                        catch { }

                        //-wholesale qty
                        try
                        {
                            qty -= double.Parse(dt2.Rows[0][0].ToString());
                        }
                        catch { }
                    }
                    catch { }
                    if (d["status"].ToString() == "1")
                        dtitems.Rows.Add(d["id"].ToString(), category, d["name"].ToString(), d["weight"].ToString(), unit, d["Retail_Price"].ToString(), d["Purchase_Price"].ToString(), qty.ToString(), d["Barcode"].ToString(), "Disable", "Delete", d["status"].ToString());
                    else
                        dtitems.Rows.Add(d["id"].ToString(), category, d["name"].ToString(), d["weight"].ToString(), unit, d["Retail_Price"].ToString(), d["Purchase_Price"].ToString(), qty.ToString(), d["Barcode"].ToString(), "Enable", "Delete", d["status"].ToString());
                    //if (d["status"].ToString() == "0")
                    //{
                    //    dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                    //}
                }
                try
                {
                    List<int> deleteindexes = new List<int>();
                    for (int i = 0; i < dtitems.Rows.Count;i++ )
                    {
                        if (dtitems.Rows[i][3].ToString().Trim() == "5")
                        {
                            dataGridView1.Rows.Add(dtitems.Rows[i][0].ToString().Trim(), dtitems.Rows[i][1].ToString().Trim(), dtitems.Rows[i][2].ToString().Trim(), dtitems.Rows[i][3].ToString().Trim(), dtitems.Rows[i][4].ToString().Trim(), dtitems.Rows[i][5].ToString().Trim(), dtitems.Rows[i][6].ToString().Trim(), dtitems.Rows[i][7].ToString().Trim(), dtitems.Rows[i][8].ToString().Trim(), dtitems.Rows[i][9].ToString().Trim(), dtitems.Rows[i][10].ToString().Trim());
                            if (dtitems.Rows[i][11].ToString().Trim() == "0")
                            {
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                            }
                            deleteindexes.Add(i);
                        }
                    }
                    foreach (int j in deleteindexes)
                    {
                        dtitems.Rows[j].Delete();
                    }
                    deleteindexes = new List<int>();
                    for (int i = 0; i < dtitems.Rows.Count; i++)
                    {
                        if (dtitems.Rows[i][3].ToString().Trim() == "2.5")
                        {
                            dataGridView1.Rows.Add(dtitems.Rows[i][0].ToString().Trim(), dtitems.Rows[i][1].ToString().Trim(), dtitems.Rows[i][2].ToString().Trim(), dtitems.Rows[i][3].ToString().Trim(), dtitems.Rows[i][4].ToString().Trim(), dtitems.Rows[i][5].ToString().Trim(), dtitems.Rows[i][6].ToString().Trim(), dtitems.Rows[i][7].ToString().Trim(), dtitems.Rows[i][8].ToString().Trim(), dtitems.Rows[i][9].ToString().Trim(), dtitems.Rows[i][10].ToString().Trim());
                            if (dtitems.Rows[i][11].ToString().Trim() == "0")
                            {
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                            }
                            deleteindexes.Add(i);
                        }
                    }
                    foreach (int j in deleteindexes)
                    {
                        dtitems.Rows[j].Delete();
                    }
                    for (int i = 0; i < dtitems.Rows.Count; i++)
                    {
                        dataGridView1.Rows.Add(dtitems.Rows[i][0].ToString().Trim(), dtitems.Rows[i][1].ToString().Trim(), dtitems.Rows[i][2].ToString().Trim(), dtitems.Rows[i][3].ToString().Trim(), dtitems.Rows[i][4].ToString().Trim(), dtitems.Rows[i][5].ToString().Trim(), dtitems.Rows[i][6].ToString().Trim(), dtitems.Rows[i][7].ToString().Trim(), dtitems.Rows[i][8].ToString().Trim(), dtitems.Rows[i][9].ToString().Trim(), dtitems.Rows[i][10].ToString().Trim());
                        if (dtitems.Rows[i][11].ToString().Trim() == "0")
                        {
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                        }
                    }
                }
            catch { }
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.ClearSelection();
                }
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

                    }
                    catch { }
                    if (d["status"].ToString() == "1")
                        dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), brand, unit, d["Retail_Price"].ToString(), d["Purchase_Price"].ToString(), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Disable", "Delete");
                    else
                        dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), brand, unit, d["Retail_Price"].ToString(), d["Purchase_Price"].ToString(), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Enable", "Delete");
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

        public void getItemNameRecord(string id)
        {
            try
            {
                string query = "Select * from items where id='" + id + "' and status != '-1'";
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
                        DataTable dt2 = gm.GetTable(query);
                        //+purchase qty
                        try
                        {
                            qty += double.Parse(dt2.Rows[0][0].ToString());
                        }
                        catch { }

                        dt2 = gm.GetTable(query);
                        //-sale qty
                        try
                        {
                            qty -= double.Parse(dt2.Rows[0][0].ToString());
                        }
                        catch { }

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
                        dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), brand, unit, d["Retail_Price"].ToString(), d["Purchase_Price"].ToString(), qty.ToString(), d["AlertQty"].ToString(), d["Barcode"].ToString(), "Disable", "Delete");
                    else
                        dataGridView1.Rows.Add(d["id"].ToString(), d["sku"].ToString(), category, d["name"].ToString(), brand, unit, d["Retail_Price"].ToString(), d["Purchase_Price"].ToString(), qty.ToString() , d["AlertQty"].ToString(), d["Barcode"].ToString(), "Enable", "Delete");
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
            getAllRecord();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
            }
            catch { }
            try
            {
                try
                {
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                    if (e.ColumnIndex == 11)
                    {
                        if (dataGridView1.Rows[e.RowIndex].Cells[11].Value.ToString() == "Enable")
                        {
                            if (con.State.ToString() == "Closed")
                            {
                                con.Open();
                            }
                            string query = @"update items set status='1' where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Successfully Enabled");
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
                        }
                        dataGridView1.ClearSelection();
                    }
                    if (e.ColumnIndex == 12)
                    {
                        DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this item?(yes/no)", "Confirmation", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.No)
                        {
                            return;
                        }
                        string query = @"update items set status='-1' where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
                        gm.ExecuteNonQuery(query);
                        MessageBox.Show("Delete Successfully");
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void metroRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            treeView1.CollapseAll();
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
            try
            {
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
            //try
            //{
            //    if (dataGridView1.SelectedRows.Count > 0)
            //    {
            //        AddProductionMaterial ai = new AddProductionMaterial(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            //        ai.ShowDialog();
            //    }
            //    else
            //    {
            //        MessageBox.Show("Select Item To Edit");
            //        return;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void ItemsList_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                    this.Dispose(true);
                if (e.Modifiers == Keys.Control && e.KeyCode == Keys.P)
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroTile2_Click(object sender, EventArgs e)
        {

        }

        public void getUnits()
        {
            try
            {
                List<Unit1> units = new List<Unit1>();
                metroComboBoxUnit.DataSource = null;
                string query = "select * from units where status='1'";
                DataTable dt = gm.GetTable(query);
                units.Add(new Unit1("Any", ""));
                units.Add(new Unit1("Other", ""));
                foreach (DataRow d in dt.Rows)
                {
                    units.Add(new Unit1(d["Unit_Name"].ToString(), d["Id"].ToString()));
                }
                metroComboBoxUnit.DataSource = units;
                metroComboBoxUnit.DisplayMember = "Name";
                metroComboBoxUnit.ValueMember = "Id";
                metroComboBoxUnit.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void getWeight()
        {
            try
            {
                metroComboBoxWeight.DataSource = null;
                string query = "select distinct(weight) from items";
                DataTable dt = gm.GetTable(query);
                metroComboBoxWeight.Items.Add("Any");
                foreach (DataRow d in dt.Rows)
                {
                    metroComboBoxWeight.Items.Add(d[0].ToString());
                }
                metroComboBoxWeight.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroDateTimeFrom_ValueChanged(object sender, EventArgs e)
        {

        }

        private void metroComboBoxBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            getAllRecord();
        }

        private void metroComboBoxQuality_SelectedIndexChanged(object sender, EventArgs e)
        {
            getAllRecord();
        }

        private void metroComboBoxShirtType_SelectedIndexChanged(object sender, EventArgs e)
        {
            getAllRecord();
        }

        private void metroComboBoxSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            getAllRecord();
        }

        private void metroComboBoxColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            getAllRecord();
        }

        public string selecteditemid = "";
        public string selecteditemname = "";
        private void metroTile2_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    selecteditemid = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();//
                    selecteditemname = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();//
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Select Any Item");
                }
            }
            catch { }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    metroTile2.PerformClick();
                }
            }
            catch { }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
            }
            catch { }
        }

        private void metroComboBoxUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            getAllRecord();
        }
    }
}
