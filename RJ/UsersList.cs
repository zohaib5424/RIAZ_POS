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
    public partial class UsersList : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public UsersList()
        {
            InitializeComponent();
        }

        AutoCompleteStringCollection source = new AutoCompleteStringCollection();
        AutoCompleteStringCollection sourceusername = new AutoCompleteStringCollection();
        AutoCompleteStringCollection sourceuserid = new AutoCompleteStringCollection();
        private void ItemsList_Load(object sender, EventArgs e)
        {
            try
            {
                metroComboBoxUserType.Items.Add("User");
                metroComboBoxUserType.Items.Add("Admin");
                source = new AutoCompleteStringCollection();
                sourceusername = new AutoCompleteStringCollection();
                sourceuserid = new AutoCompleteStringCollection();
                metroTextBox1.Hide();

                this.AutoScroll = true;
                metroRadioButton1.Checked = true;

                metroComboBoxUserType.Hide();



                DataTable dt = gm.GetTable("select * from login where status !='-1'");
                foreach (DataRow d in dt.Rows)
                {
                    sourceusername.Add(d["username"].ToString() + " (" + d["id"].ToString() + ")");
                    sourceuserid.Add(d["user_id"].ToString() + " (" + d["id"].ToString() + ")");
                }

                metroTextBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                metroTextBox1.AutoCompleteCustomSource = sourceusername;
                metroTextBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

                metroTextBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                metroTextBox2.AutoCompleteCustomSource = sourceuserid;
                metroTextBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;
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
                string query = "Select * from login where status != '-1'";
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
                        dataGridView1.Rows.Add(d["id"].ToString(), d["username"].ToString(), d["User_Id"].ToString(), d["_date"].ToString().Split(' ')[0].Trim(), d["_time"].ToString(), addedby, "Deactive", "Delete");
                    else
                        dataGridView1.Rows.Add(d["id"].ToString(), d["username"].ToString(), d["User_Id"].ToString(), d["_date"].ToString().Split(' ')[0].Trim(), d["_time"].ToString(), addedby, "Active", "Delete");
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
                        dataGridView1.Rows.Add(d["id"].ToString(), d["username"].ToString(), d["User_Id"].ToString(), d["_date"].ToString().Split(' ')[0].Trim(), d["_time"].ToString(), addedby, "Deactive", "Delete");
                    else
                        dataGridView1.Rows.Add(d["id"].ToString(), d["username"].ToString(), d["User_Id"].ToString(), d["_date"].ToString().Split(' ')[0].Trim(), d["_time"].ToString(), addedby, "Active", "Delete");
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
                        dataGridView1.Rows.Add(d["id"].ToString(), d["username"].ToString(), d["User_Id"].ToString(), d["_date"].ToString().Split(' ')[0].Trim(), d["_time"].ToString(), addedby, "Deactive", "Delete");
                    else
                        dataGridView1.Rows.Add(d["id"].ToString(), d["username"].ToString(), d["User_Id"].ToString(), d["_date"].ToString().Split(' ')[0].Trim(), d["_time"].ToString(), addedby, "Active", "Delete");
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
                string query = "Select * from Login where usertype='" + type+ "' and status != '-1'";
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
                        dataGridView1.Rows.Add(d["id"].ToString(), d["username"].ToString(), d["User_Id"].ToString(), d["_date"].ToString().Split(' ')[0].Trim(), d["_time"].ToString(), addedby, "Deactive", "Delete");
                    else
                        dataGridView1.Rows.Add(d["id"].ToString(), d["username"].ToString(), d["User_Id"].ToString(), d["_date"].ToString().Split(' ')[0].Trim(), d["_time"].ToString(), addedby, "Active", "Delete");
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
            metroTextBox2.Hide();
            metroComboBoxUserType.Hide();
            metroTextBox1.Hide();
            getAllRecord();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                if (e.ColumnIndex == 6)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString() == "Active")
                    {
                        if (con.State.ToString() == "Closed")
                        {
                            con.Open();
                        }
                        string query = @"update login set status='1' where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Successfully Actived");
                        if (metroRadioButton1.Checked == true)
                            getAllRecord();
                        else if (metroRadioButton2.Checked == true)
                            getUserTypeRecord(metroComboBoxUserType.SelectedItem.ToString());
                        else if (metroRadioButton3.Checked == true)
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
                        else if (metroRadioButton4.Checked == true)
                        {
                            try
                            {
                                string[] s = metroTextBox2.Text.Trim().Split('(');
                                string[] a = s[s.Length - 1].Trim().Split(')');
                                string id = a[0].ToString();
                                if (id.Trim() != "")
                                {
                                    getUserIdRecord(id);
                                }
                                if (metroTextBox2.Text.Trim() == "")
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
                        if (dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() == RJ.Properties.Settings.Default.loginid)
                        {
                            MessageBox.Show("You Can't Deactive yourself");
                            return;
                        }
                        if (con.State.ToString() == "Closed")
                        {
                            con.Open();
                        }
                        string query = @"update login set status='0' where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Successfully Deactived");
                        if (metroRadioButton1.Checked == true)
                            getAllRecord();
                        else if (metroRadioButton2.Checked == true)
                            getUserTypeRecord(metroComboBoxUserType.SelectedItem.ToString());
                        else if (metroRadioButton3.Checked == true)
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
                        else if (metroRadioButton4.Checked == true)
                        {
                            try
                            {
                                string[] s = metroTextBox2.Text.Trim().Split('(');
                                string[] a = s[s.Length - 1].Trim().Split(')');
                                string id = a[0].ToString();
                                if (id.Trim() != "")
                                {
                                    getUserIdRecord(id);
                                }
                                if (metroTextBox2.Text.Trim() == "")
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
                if (e.ColumnIndex == 7)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() == RJ.Properties.Settings.Default.loginid)
                    {
                        MessageBox.Show("You Can't Delete yourself");
                        return;
                    }
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this user?(yes/no)", "Confirmation", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                    string query = @"update login set status='-1' where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
                    gm.ExecuteNonQuery(query);
                    MessageBox.Show("Delete Successfully");
                        if (metroRadioButton1.Checked == true)
                            getAllRecord();
                        else if (metroRadioButton2.Checked == true)
                            getUserTypeRecord(metroComboBoxUserType.SelectedItem.ToString());
                        else if (metroRadioButton3.Checked == true)
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
                        else if (metroRadioButton4.Checked == true)
                        {
                            try
                            {
                                string[] s = metroTextBox2.Text.Trim().Split('(');
                                string[] a = s[s.Length - 1].Trim().Split(')');
                                string id = a[0].ToString();
                                if (id.Trim() != "")
                                {
                                    getUserIdRecord(id);
                                }
                                if (metroTextBox2.Text.Trim() == "")
                                    getAllRecord();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
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
            metroTextBox2.Hide();
            metroComboBoxUserType.Show();
            metroTextBox1.Hide();
            metroComboBoxUserType.Focus();
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
            metroComboBoxUserType.Hide();
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
            metroComboBoxUserType.Hide();
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
                    getUserIdRecord(id);
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
            try
            {
                getUserTypeRecord(metroComboBoxUserType.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
