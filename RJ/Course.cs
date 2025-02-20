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
    public partial class Course : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public Course()
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
                string query = @"select * from Courses where status !='-1'";
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
                    if (d[5].ToString() == "1")
                        dataGridView1.Rows.Add(d[0].ToString(), d[1].ToString(), d[2].ToString(), d[3].ToString(), d[4].ToString(),"Disable");
                    else
                        dataGridView1.Rows.Add(d[0].ToString(), d[1].ToString(), d[2].ToString(), d[3].ToString(), d[4].ToString(),"Enable");
                    if (d[5].ToString() == "0")
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
                if (textBox1.Text.Trim() == "" || textBox1.Text.Trim().Length < 1)
                {
                    MessageBox.Show("Enter Course Code");
                    return;
                }
                if (textBox3.Text.Trim() == "" || textBox3.Text.Trim().Length < 1)
                {
                    MessageBox.Show("Enter Course Credit Hours");
                    return;
                }
                if (textBox2.Text.Trim() != "" && textBox2.Text.Trim().Length > 0)
                {
                    if (con.State.ToString() == "Closed")
                    {
                        con.Open();
                    }
                    string query = @"select max(cast(id as int)) from Courses";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    string id = "";
                    foreach (DataRow d in dt.Rows)
                    {
                        id = d[0].ToString();
                    }
                    if (id == "")
                    {
                        id = "0";
                    }
                    id = (int.Parse(id) + 1).ToString();

                    string data = textBox1.Text.Trim().ToLower().ToString();
                    string cousecode = textBox1.Text.Trim().ToString();
                    //string[] abc = data.Split(' ');
                    //foreach (string a in abc)
                    //{
                    //    cousecode += a.Substring(0, 1).ToUpper() + a.Substring(1, a.Length - 1);
                    //    cousecode += " ";
                    //}

                    //data = textBox2.Text.Trim().ToLower().ToString();
                    string cousename = textBox2.Text.Trim();
                    //string[] abc1 = data.Split(' ');
                    //foreach (string a in abc)
                    //{
                    //    cousecode += a.Substring(0, 1).ToUpper() + a.Substring(1, a.Length - 1);
                    //    cousecode += " ";
                    //}

                    string cousecredit = textBox3.Text.Trim().ToString();

                    data = richTextBox1.Text.Trim().ToString();
                    string cousedescription = data;
                    //string[] abc2 = data.Split(' ');
                    //foreach (string a in abc)
                    //{
                    //    cousedescription += a.Substring(0, 1).ToUpper() + a.Substring(1, a.Length - 1);
                    //    cousedescription += " ";
                    //}

                    query = @"select * from Courses where Course_name='" + cousename + "' and status='1'";
                    cmd = new SqlCommand(query, con);
                    sda = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    sda.Fill(dt);

                    if (dt.Rows.Count <= 0)
                    {
                        query = @"insert into Courses values('" + id + "','" + cousecode + "','" + cousename + "','" + cousecredit + "','" + cousedescription + "','1')";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                        textBox1.Text = textBox2.Text = "";
                        textBox3.Text = richTextBox1.Text = "";
                        MessageBox.Show("Successfully Saved");
                        filldata();
                    }
                    else
                    {
                        MessageBox.Show("Course Already Exist.");
                    }
                }
                else
                {
                    MessageBox.Show("Enter Course Name");
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
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString().Trim().ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString().Trim().ToString();
                richTextBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString().Trim().ToString();
                if (e.ColumnIndex == 5)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString() == "Enable")
                    {
                        if (con.State.ToString() == "Closed")
                        {
                            con.Open();
                        }
                        string query = @"update Courses set status='1' where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
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
                        string query = @"update Courses set status='0' where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString().Trim();
                dataGridView1.Rows[e.RowIndex].Selected = true;
                if (e.ColumnIndex == 2)
                {
                    string id = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
                    string newval = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[1].Value.ToString();
                    if (newval != "" && newval.Length > 0)
                    {
                        string data = newval.Trim().ToLower().ToString();
                        string data2 = "";
                        string[] abc = data.Split(' ');
                        foreach (string a in abc)
                        {
                            data2 += a.Substring(0, 1).ToUpper() + a.Substring(1, a.Length - 1);
                        }

                        string query = @"select * from sections where section='" + data2 + "' and status='1'";
                        SqlCommand cmd = new SqlCommand(query, con);
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        if (dt.Rows.Count <= 0)
                        {

                            query = @"update sections set section='" + data2 + "' where id='" + id + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Successfully  Updated");
                            filldata();
                        }
                        else
                        {
                            MessageBox.Show("Record Already Exist");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Enter Section");
                    }
                }
                if (e.ColumnIndex == 3)
                {
                    string id = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
                    string query = @"update sections set status='0' where id='" + id + "'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Delete Successfully");
                    filldata();
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
                if (textBox1.Text.Trim() == "" || textBox1.Text.Trim().Length < 1)
                {
                    MessageBox.Show("Enter Course Code For Update");
                    return;
                }
                if (textBox3.Text.Trim() == "" || textBox3.Text.Trim().Length < 1)
                {
                    MessageBox.Show("Enter Course Credit Hours");
                    return;
                }
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    if (textBox2.Text.Trim().Length > 0)
                    {
                        if (con.State.ToString() == "Closed")
                        {
                            con.Open();
                        }
                        string query = @"select * from Courses where Course_name='" + textBox2.Text.Trim() + "' and status='1'";
                        SqlCommand cmd = new SqlCommand(query, con);
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        if (dt.Rows.Count <= 0)
                        {
                            query = @"select * from Courses where Course_code='" + textBox1.Text.Trim() + "' and status='1'";
                            cmd = new SqlCommand(query, con);
                            sda = new SqlDataAdapter(cmd);
                            dt = new DataTable();
                            sda.Fill(dt);

                            if (dt.Rows.Count <= 0)
                            {
                                query = @"update Courses set Course_code='" + textBox1.Text.Trim() + "',Course_name='" + textBox2.Text.Trim() + "',Credit_hours='" + textBox3.Text.Trim() + "',Course_description='" + richTextBox1.Text.Trim() + "' where id = '" + dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString().Trim() + "'";
                                cmd = new SqlCommand(query, con);
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Successfully Updated");
                                reset();
                                filldata();
                            }
                            else
                            {
                                MessageBox.Show("Course Code Already Exist.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Course Name Already Exist.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Enter Course Name For Update");
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

        public void reset()
        {
            textBox1.Text = textBox2.Text = "";
            textBox3.Text = richTextBox1.Text = "";
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                    textBox1.Text = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[1].Value.ToString().Trim().ToString();
                else
                    textBox1.Text = "";
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
                    string query = @"update Courses set status='-1' where id = '" + dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString().Trim() + "'";
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
    }
}
