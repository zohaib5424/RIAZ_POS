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
    public partial class Class : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public Class()
        {
            InitializeComponent();
        }

        private void ClassType_Load(object sender, EventArgs e)
        {
            filldata();
        }

        public void filldata()
        {
            try
            {
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }

                string query = @"select * from class where status !='-1'";
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
                    query = @"select * from class_type where id = '"+d[2].ToString()+"' and status !='-1'";
                    cmd = new SqlCommand(query, con);
                    sda = new SqlDataAdapter(cmd);
                    DataTable dt2 = new DataTable();
                    sda.Fill(dt2);

                    string class_type="";

                    foreach (DataRow d2 in dt2.Rows)
                    {
                        class_type = d2[1].ToString().Trim() + " (" + d[2].ToString().Trim() + ")";
                    }

                    if (d[3].ToString() == "1")
                        dataGridView1.Rows.Add(d[0].ToString(), d[1].ToString(), class_type ,"Disable");
                    else
                        dataGridView1.Rows.Add(d[0].ToString(), d[1].ToString(), class_type , "Enable");
                    if (d[3].ToString() == "0")
                    {
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
                dataGridView1.ClearSelection();
                if (comboBox1.Items.Count > 0)
                {
                    comboBox1.Items.Clear();
                }
                query = @"select * from class_type where status ='1'";
                cmd = new SqlCommand(query, con);
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow d in dt.Rows)
                {
                    comboBox1.Items.Add(d[1].ToString().Trim() + " (" + d[0].ToString().Trim() + ")");
                }
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
                if (textBox2.Text.Trim() != "" && textBox2.Text.Trim().Length > 0)
                {
                    if (comboBox1.SelectedIndex >= 0)
                    {
                        if (con.State.ToString().Trim() == "Closed")
                        {
                            con.Open();
                        }
                        string query = @"select max(cast(id as int)) from class";
                        SqlCommand cmd = new SqlCommand(query, con);
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        string id = "";
                        foreach (DataRow d in dt.Rows)
                        {
                            id = d[0].ToString().Trim();
                        }
                        if (id == "")
                        {
                            id = "0";
                        }
                        id = (int.Parse(id) + 1).ToString().Trim();
                        string data = textBox2.Text.Trim().ToLower().ToString().Trim();
                        string data2 = "";
                        string[] abc = data.Split(' ');
                        foreach (string a in abc)
                        {
                            data2 += a.Substring(0, 1).ToUpper() + a.Substring(1, a.Length - 1);
                            data2 += " ";
                        }

                        string classtypeid = comboBox1.Text.ToString().Split('(')[1].ToString().Split(')')[0].Trim().ToString();
                        query = @"select * from class where class='" + data2 + "' and class_type_id='"+classtypeid+"' and status !='-1'";
                        cmd = new SqlCommand(query, con);
                        sda = new SqlDataAdapter(cmd);
                        dt = new DataTable();
                        sda.Fill(dt);

                        if (dt.Rows.Count <= 0)
                        {
                            query = @"insert into Class values('" + id + "','" + data2 + "','" + comboBox1.Text.ToString().Trim().ToString().Split('(')[1].Trim().Split(')')[0].Trim() + "','1')";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            textBox2.Text = "";
                            MessageBox.Show("Successfully Saved");
                            filldata();
                            reset();
                        }
                        else
                        {
                            MessageBox.Show("Class Type Already Exist.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Select Class Type");
                    }
                }
                else
                {
                    MessageBox.Show("Enter Class Name");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void reset()
        {
            comboBox1.SelectedIndex = -1;
            textBox2.Text = comboBox1.Text = "";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    dataGridView1.Rows[e.RowIndex].Selected = true;
            //    textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString().Trim().ToString();
            //    comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString().Trim().ToString();
            //    comboBox1.SelectedIndex = comboBox1.Items.IndexOf(comboBox1.Text.Trim());
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    dataGridView1.Rows[e.RowIndex].Selected = true;
            //    if (e.ColumnIndex == 2)
            //    {
            //        string id = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
            //        string newval = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[1].Value.ToString();
            //        if (newval != "" && newval.Length > 0)
            //        {
            //            string data = newval.Trim().ToLower().ToString();
            //            string data2 = "";
            //            string[] abc = data.Split(' ');
            //            foreach (string a in abc)
            //            {
            //                data2 += a.Substring(0, 1).ToUpper() + a.Substring(1, a.Length - 1);
            //            }

            //            string query = @"select * from classtype where classtype='" + data2 + "' and status='1'";
            //            SqlCommand cmd = new SqlCommand(query, con);
            //            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            //            DataTable dt = new DataTable();
            //            sda.Fill(dt);

            //            if (dt.Rows.Count <= 0)
            //            {

            //                query = @"update ClassType set classtype='" + data2 + "' where id='" + id + "'";
            //                cmd = new SqlCommand(query, con);
            //                cmd.ExecuteNonQuery();
            //                MessageBox.Show("Successfully  Updated");
            //                filldata();
            //            }
            //            else
            //            {
            //                MessageBox.Show("Class Type Already Exist");
            //            }
            //        }
            //        else
            //        {
            //            MessageBox.Show("Enter Class Type");
            //        }
            //    }
            //    if (e.ColumnIndex == 3)
            //    {
            //        string id = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
            //        string query = @"update ClassType set status='0' where id='" + id + "'";
            //        SqlCommand cmd = new SqlCommand(query, con);
            //        cmd.ExecuteNonQuery();
            //        MessageBox.Show("Delete Successfully");
            //        filldata();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            try
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString().Trim().ToString();
                comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString().Trim().ToString();
                comboBox1.SelectedIndex = comboBox1.Items.IndexOf(comboBox1.Text);

                if (e.ColumnIndex == 3)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString() == "Enable")
                    {
                        if (con.State.ToString() == "Closed")
                        {
                            con.Open();
                        }
                        string query = @"update Class set status='1' where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Successfully Enabled");
                        filldata();
                    }
                    else
                    {
                        if (con.State.ToString() == "Closed")
                        {
                            con.Open();
                        }
                        string query = @"update Class set status='0' where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Successfully Disabled");
                        filldata();
                    }
                    dataGridView1.ClearSelection();
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
                    textBox2.Text = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[1].Value.ToString().Trim().ToString();
                    comboBox1.Text = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[2].Value.ToString().Trim().ToString();
                    comboBox1.SelectedIndex = comboBox1.Items.IndexOf(comboBox1.Text.Trim());
                }
                else
                {
                    textBox2.Text = "";
                    comboBox1.Text = "";
                    comboBox1.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {

                    if (dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[3].Value.ToString().Trim() == "Enable")
                    {
                        MessageBox.Show("First Enable The Record To Update");
                        return;
                    }
                    if (dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[3].Value.ToString().Trim() == "Enable")
                    {
                        MessageBox.Show("First Enable The Record To Update");
                        return;
                    }
                    if (textBox2.Text.Trim().Length > 0)
                    {
                        if (comboBox1.SelectedIndex >= 0)
                        {
                            if (con.State.ToString().Trim() == "Closed")
                            {
                                con.Open();
                            }
                            string data = textBox2.Text.Trim().ToLower().ToString().Trim();
                            string data2 = "";
                            string[] abc = data.Split(' ');
                            foreach (string a in abc)
                            {
                                data2 += a.Substring(0, 1).ToUpper() + a.Substring(1, a.Length - 1);
                                data2 += " ";
                            }
                            string query = @"select * from class where class='" + data2 + "' and class_type_id='" + comboBox1.Text.ToString().Trim().ToString().Split('(')[1].Trim().Split(')')[0].Trim() + "' and status !='-1'";
                            SqlCommand cmd = new SqlCommand(query, con);
                            SqlDataAdapter sda = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            if (dt.Rows.Count <= 0)
                            {
                                query = @"update class set class='" + data2 + "' , class_type_id='" + comboBox1.Text.ToString().Trim().ToString().Split('(')[1].Trim().Split(')')[0].Trim() + "' where id = '" + dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString().Trim() + "'";
                                cmd = new SqlCommand(query, con);
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Successfully Updated");
                                reset();
                                filldata();
                            }
                            else
                            {
                                MessageBox.Show("Record Already Exist.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Select Class Type");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Enter Type For Update");
                    }
                }
                else
                {
                    MessageBox.Show("Select Row For Update");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                            string query = @"Update class set status = '-1' where id = '" + dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString().Trim() + "'";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Successfully deleted");
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
    }
}

