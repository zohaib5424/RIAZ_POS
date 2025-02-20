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
    public partial class Session : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public Session()
        {
            InitializeComponent();
        }

        private void ClassType_Load(object sender, EventArgs e)
        {
            try
            {
                dateTimePicker2.Value = DateTime.Parse("1-1-" + (dateTimePicker1.Value.Year + 1).ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                string query = @"select * from session where status !='-1'";
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
                    if (d[02].ToString() == "1")
                        dataGridView1.Rows.Add(d[0].ToString(), d[1].ToString(),"Disable");
                    else
                        dataGridView1.Rows.Add(d[0].ToString(), d[1].ToString(),"Enable");
                    if (d[2].ToString() == "0")
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
                {
                    if (con.State.ToString() == "Closed")
                    {
                        con.Open();
                    }
                    string query = @"select max(cast(id as int)) from session";
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
                    string data2 = dateTimePicker1.Value.Year.ToString()+" - "+dateTimePicker2.Value.Year.ToString();
                    query = @"select * from session where session='" + data2 + "' and status !='-1'";
                    cmd = new SqlCommand(query, con);
                    sda = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    sda.Fill(dt);

                    if (dt.Rows.Count <= 0)
                    {
                        query = @"insert into session values('" + id + "','" + data2 + "','1')";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Successfully Saved");
                        filldata();
                    }
                    else
                    {
                        MessageBox.Show("Session Already Exist.");
                    }
                }
                {

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
                string[] dat1 = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString().Trim().ToString().Split('-');
                dateTimePicker1.Value = DateTime.Parse("1-1-" + dat1[0].ToString().Trim());
                dateTimePicker2.Value = DateTime.Parse("1-1-"+dat1[1].ToString().Trim());
                if (e.ColumnIndex == 2)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() == "Enable")
                    {
                        if (con.State.ToString() == "Closed")
                        {
                            con.Open();
                        }
                        string query = @"update session set status='1' where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
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
                        string query = @"update session set status='0' where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
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
                dataGridView1.Rows[e.RowIndex].Selected = true;
                string[] dat1 = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString().Trim().ToString().Split('-');
                dateTimePicker1.Value = DateTime.Parse("1-1-" + dat1[0].ToString().Trim());
                dateTimePicker2.Value = DateTime.Parse("1-1-" + dat1[1].ToString().Trim());
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
                if(dataGridView1.SelectedRows.Count > 0)
                {
                    if (dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[2].Value.ToString().Trim() == "Enable")
                    {
                        MessageBox.Show("First Enable The Record To Update");
                        return;
                    }
                    if (con.State.ToString() == "Closed")
                        {
                            con.Open();
                        }
                        string data2 = dateTimePicker1.Value.Year.ToString() +" - "+ dateTimePicker2.Value.Year.ToString();
                        string query = @"select * from session where session='" + data2 + "' and status !='-1'";
                        SqlCommand cmd = new SqlCommand(query, con);
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        if (dt.Rows.Count <= 0)
                        {
                            query = @"update session set session='" + data2 + "' where id = '" + dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString().Trim() + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Successfully Updated");
                            reset();
                            filldata();
                        }
                        else
                        {
                            MessageBox.Show("Session Already Exist.");
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

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Selected = true;
                    string[] dat1 = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[1].Value.ToString().Trim().ToString().Split('-');
                    dateTimePicker1.Value = DateTime.Parse("1-1-"+dat1[0].Trim());
                    dateTimePicker2.Value = DateTime.Parse("1-1-"+dat1[1].Trim());
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
                    string query = @"update session set status='-1' where id = '" + dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString().Trim() + "'";
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

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                dateTimePicker1.Value = DateTime.Parse("1-1-"+(dateTimePicker2.Value.Year -1).ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                dateTimePicker2.Value = DateTime.Parse("1-1-" + (dateTimePicker1.Value.Year + 1).ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
