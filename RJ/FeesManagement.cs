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
    public partial class FeesManagement : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public FeesManagement()
        {
            InitializeComponent();
        }

        private void ClassType_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
            label2.Hide();
            label3.Hide();
            textBox1.Hide();
            textBox2.Hide();
        }

        public void filldata()
        {
            try
            {
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }

                string query = @"select * from Fees_Types where status='1'";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }

                foreach (DataRow d in dt.Rows)
                {
                    dataGridView1.Rows.Add(d[0].ToString(), d[1].ToString() , d[3].ToString());
                }
                dataGridView1.ClearSelection();
                //if (comboBox1.Items.Count > 0)
                //{
                //    comboBox1.Items.Clear();
                //}
                //comboBox1.Items.Add("Class Wise");
                //comboBox1.Items.Add("Other");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (panel2.Location.X >= (this.Width - panel2.Width))
            //{
            //    panel2.Location = new Point(panel2.Location.X - 10, panel2.Location.Y);
            //}
        }


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State.ToString().Trim() == "Closed")
                {
                    con.Open();
                }
                string query = @"";
                SqlCommand cmd = new SqlCommand();
                if (radioButton1.Checked == true)
                {
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        int ok = 1;
                        foreach (DataGridViewRow g in dataGridView2.Rows)
                        {
                            if (g.Cells[1].Value.ToString() == "True")
                            {
                                if (g.Cells[3].Value.ToString().Trim() == "")
                                {
                                    ok = 0;
                                }
                            }
                        }
                        if (ok == 1)
                        {
                            query = @"Update fees_types_classwisedetails set status='0'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            foreach (DataGridViewRow d in dataGridView2.Rows)
                            {
                                if (d.Cells[1].Value.ToString() == "True")
                                {
                                    string id = "";
                                    query = @"select max(cast(id as int)) from fees_types_classwisedetails";
                                    cmd = new SqlCommand(query, con);
                                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                                    DataTable dt = new DataTable();
                                    sda.Fill(dt);
                                    foreach (DataRow d2 in dt.Rows)
                                    {
                                        id = d2[0].ToString();
                                    }
                                    if (id == "")
                                    {
                                        id = "0";
                                    }
                                    id = (int.Parse(id) + 1).ToString();
                                    query = @"insert into fees_types_classwisedetails values('" + id + "','" + dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString() + "' , '" + d.Cells[0].Value.ToString() + "','" + d.Cells[3].Value.ToString() + "' , '1')";
                                    cmd = new SqlCommand(query, con);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            MessageBox.Show("Successfully Saved");
                            dataGridView1.ClearSelection();
                            getnewclass();
                        }
                        else
                        {
                            MessageBox.Show("Enter Fees Value Of Selected Class");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Select Fees");
                    }
                }
                else if (radioButton2.Checked == true)
                {
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        if (textBox2.Text.Trim() != "")
                        {
                            query = @"update Fees_Types set fees_amount='"+textBox2.Text+"' where id='"+dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString() +"'";
                            cmd = new SqlCommand(query , con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Successfully Saved");
                            dataGridView1.ClearSelection();
                            textBox1.Text = textBox2.Text = "";
                        }
                        else
                        {
                            MessageBox.Show("Enter Amount");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Select Fees");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void reset()
        {
            //comboBox1.SelectedIndex = -1;
            //textBox2.Text = comboBox1.Text = "";
        }


        public void getnewclass()
        {
            try
            {
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }

                string query = @"select * from Class where status='1'";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dataGridView2.Rows.Count > 0)
                {
                    dataGridView2.Rows.Clear();
                }

                foreach (DataRow d in dt.Rows)
                {
                    dataGridView2.Rows.Add(d[0].ToString(),false, d[1].ToString(), "");
                }
                dataGridView2.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        public void getclass()
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    if (con.State.ToString() == "Closed")
                    {
                        con.Open();
                    }

                    string query = @"select * from Class where status='1'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dataGridView2.Rows.Count > 0)
                    {
                        dataGridView2.Rows.Clear();
                    }

                    foreach (DataRow d in dt.Rows)
                    {
                        //dataGridView2.Rows.Add(d[0].ToString(),false, d[1].ToString(), "");
                        query = @"select fees_amount from fees_types_classwisedetails where fees_id='" + dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString() + "' and class_id='" + d[0].ToString() + "' and status='1'";
                        cmd = new SqlCommand(query, con);
                        sda = new SqlDataAdapter(cmd);
                        DataTable d2 = new DataTable();
                        sda.Fill(d2);
                        string amount = "";
                        foreach (DataRow d1 in d2.Rows)
                        {
                            amount = d1[0].ToString();
                        }
                        if (amount.Trim().Length > 0 && amount.Trim() != "")
                        {
                            dataGridView2.Rows.Add(d[0].ToString(), true, d[1].ToString(), amount);
                        }
                        else
                        {
                            dataGridView2.Rows.Add(d[0].ToString(), false, d[1].ToString(), amount);
                        }
                    }
                    dataGridView2.ClearSelection();
                }
                else
                {
                    MessageBox.Show("Select Fees");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString().Trim().ToString();
                textBox2.Text = "";
                if (radioButton1.Checked == true)
                    getclass();
                else if(radioButton2.Checked == true)
                {
                    try
                    {
                            if (con.State.ToString() == "Closed")
                            {
                                con.Open();
                            }
                            string query = @"select * from fees_types where status = '1' and id = '" + dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString().Trim() + "'";
                            SqlCommand cmd = new SqlCommand(query, con);
                            DataTable dt = new DataTable();
                            SqlDataAdapter sda = new SqlDataAdapter(cmd);
                            sda.Fill(dt);
                            foreach (DataRow d in dt.Rows)
                            {
                                textBox2.Text = d[2].ToString();
                            }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                //comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString().Trim().ToString();
                //comboBox1.SelectedIndex = comboBox1.Items.IndexOf(comboBox1.Text.Trim());
            }
            catch (Exception ex)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString().Trim().ToString();
                textBox2.Text = "";
                if (radioButton1.Checked == true)
                    getclass();
                else if (radioButton2.Checked == true)
                {
                    try
                    {
                        if (con.State.ToString() == "Closed")
                        {
                            con.Open();
                        }
                        string query = @"select * from fees_types where status = '1' and id = '" + dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString().Trim() + "'";
                        SqlCommand cmd = new SqlCommand(query, con);
                        DataTable dt = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        sda.Fill(dt);
                        foreach (DataRow d in dt.Rows)
                        {
                            textBox2.Text = d[2].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                //comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString().Trim().ToString();
                //comboBox1.SelectedIndex = comboBox1.Items.IndexOf(comboBox1.Text.Trim());
            }
            catch (Exception ex)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Selected = true;
                textBox1.Text = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[1].Value.ToString().Trim().ToString();
                textBox2.Text = "";
                if (radioButton1.Checked == true)
                    getclass();
                else if (radioButton2.Checked == true)
                {
                    try
                    {
                        if (con.State.ToString() == "Closed")
                        {
                            con.Open();
                        }
                        string query = @"select * from fees_types where status = '1' and id = '" + dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString().Trim() + "'";
                        SqlCommand cmd = new SqlCommand(query, con);
                        DataTable dt = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        sda.Fill(dt);
                        foreach (DataRow d in dt.Rows)
                        {
                            textBox2.Text = d[2].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                //comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString().Trim().ToString();
                //comboBox1.SelectedIndex = comboBox1.Items.IndexOf(comboBox1.Text.Trim());
            }
            catch (Exception ex)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                //MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (dataGridView1.SelectedRows.Count > 0)
            //    {
            //        if (textBox2.Text.Trim().Length > 0)
            //        {
            //            if (comboBox1.SelectedIndex >= 0)
            //            {
            //                if (con.State.ToString().Trim() == "Closed")
            //                {
            //                    con.Open();
            //                }
            //                string data = textBox2.Text.Trim().ToLower().ToString().Trim();
            //                string data2 = "";
            //                string[] abc = data.Split(' ');
            //                foreach (string a in abc)
            //                {
            //                    data2 += a.Substring(0, 1).ToUpper() + a.Substring(1, a.Length - 1);
            //                    data2 += " ";
            //                }
            //                string query = @"select * from fees_types where fees_name='" + data2 + "' and category='" + comboBox1.Text.ToString().Trim().ToString() + "' and status='1'";
            //                SqlCommand cmd = new SqlCommand(query, con);
            //                SqlDataAdapter sda = new SqlDataAdapter(cmd);
            //                DataTable dt = new DataTable();
            //                sda.Fill(dt);

            //                if (dt.Rows.Count <= 0)
            //                {
            //                    query = @"update fees_types set fees_name='" + data2 + "' , category='" + comboBox1.Text.ToString().Trim()+ "' where id = '" + dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString().Trim() + "'";
            //                    cmd = new SqlCommand(query, con);
            //                    cmd.ExecuteNonQuery();
            //                    MessageBox.Show("Successfully Updated");
            //                    reset();
            //                    filldata();
            //                }
            //                else
            //                {
            //                    MessageBox.Show("Record Already Exist.");
            //                }
            //            }
            //            else
            //            {
            //                MessageBox.Show("Select Category Type");
            //            }
            //        }
            //        else
            //        {
            //            MessageBox.Show("Enter Fees Name For Update");
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Select Row For Update");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                            if (con.State.ToString() == "Closed")
                            {
                                con.Open();
                            }
                            string query = @"Update fees_types set status = '0' where id = '" + dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString().Trim() + "'";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Successfully Deleted");
                            reset();
                            filldata();
                }
                else
                {
                    MessageBox.Show("Select Row For Delete");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                dataGridView2.Show();
                label2.Hide();
                label3.Hide();
                textBox1.Hide();
                textBox2.Hide();
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }

                string query = @"select * from Fees_Types where category='Class Wise' and status='1'";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }

                foreach (DataRow d in dt.Rows)
                {
                    dataGridView1.Rows.Add(d[0].ToString(), d[1].ToString(), d[3].ToString());
                }
                dataGridView1.ClearSelection();
                //if (comboBox1.Items.Count > 0)
                //{
                //    comboBox1.Items.Clear();
                //}
                //comboBox1.Items.Add("Class Wise");
                //comboBox1.Items.Add("Other");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                dataGridView2.Hide();
                label2.Show();
                label3.Show();
                textBox1.Show();
                textBox2.Show();
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }

                string query = @"select * from Fees_Types where category='Other' and status='1'";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }

                foreach (DataRow d in dt.Rows)
                {
                    dataGridView1.Rows.Add(d[0].ToString(), d[1].ToString(), d[3].ToString());
                }
                dataGridView1.ClearSelection();
                //if (comboBox1.Items.Count > 0)
                //{
                //    comboBox1.Items.Clear();
                //}
                //comboBox1.Items.Add("Class Wise");
                //comboBox1.Items.Add("Other");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 3)
                {
                    if (e.RowIndex >= 0)
                    {
                        if (dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString().Trim() != "")
                        {
                            double.Parse(dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString().Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dataGridView2.Rows[e.RowIndex].Cells[3].Value = "";
            }
        }

        private void dataGridView2_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

