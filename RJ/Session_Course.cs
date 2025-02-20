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
    public partial class Session_Course : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public Session_Course()
        {
            InitializeComponent();
        }

        private void ClassType_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
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
                string query = @"select * from Courses where status='1'";
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
                    dataGridView1.Rows.Add(d[0].ToString(),false, d[1].ToString(), d[2].ToString(), d[3].ToString(), d[4].ToString());
                }
                dataGridView1.ClearSelection();

                query = @"select * from session where status='1'";
                cmd = new SqlCommand(query, con);
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);
                if (listBox1.Items.Count > 0)
                {
                    listBox1.Items.Clear();
                }
                foreach (DataRow d in dt.Rows)
                {
                    listBox1.Items.Add(d[1].ToString()+" ("+d[0].ToString() +")");
                }
                listBox1.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void showsessioncourses()
        {
            try
            {
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }
                string query = @"select * from Session_Courses where status='1'";
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
                    dataGridView1.Rows.Add(d[0].ToString(), false, d[1].ToString(), d[2].ToString(), d[3].ToString(), d[4].ToString());
                }
                dataGridView1.ClearSelection();

                query = @"select * from session where status='1'";
                cmd = new SqlCommand(query, con);
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);
                if (listBox1.Items.Count > 0)
                {
                    listBox1.Items.Clear();
                }
                foreach (DataRow d in dt.Rows)
                {
                    listBox1.Items.Add(d[1].ToString() + " (" + d[0].ToString() + ")");
                }
                listBox1.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void showallcourses()
        {
            try
            {
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }
                string query = @"select * from Courses where status='1'";
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
                    dataGridView1.Rows.Add(d[0].ToString(), false, d[1].ToString(), d[2].ToString(), d[3].ToString(), d[4].ToString());
                }
                dataGridView1.ClearSelection();

                query = @"select * from session where status='1'";
                cmd = new SqlCommand(query, con);
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);
                if (listBox1.Items.Count > 0)
                {
                    listBox1.Items.Clear();
                }
                foreach (DataRow d in dt.Rows)
                {
                    listBox1.Items.Add(d[1].ToString() + " (" + d[0].ToString() + ")");
                }
                listBox1.SelectedIndex = -1;
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
            if (listBox1.SelectedIndex >= 0 && listBox1.SelectedItem.ToString() != "")
            {

            }
            else
            {
                MessageBox.Show("Select Session First");
                return;
            }
            int ok = 0;
            foreach (DataGridViewRow g in dataGridView1.Rows)
            {
                if (g.Cells[1].Value.ToString() == "True")
                {
                    ok = 1;
                }
            }
            if (ok == 0)
            {
                MessageBox.Show("No Course Found To Be Registered For Session");
                return;
            }
            try
            {
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }
                string query = @"select max(cast(id as int)) from Session_Courses";
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

                string sessionid = listBox1.SelectedItem.ToString().Trim().Split('(')[1].ToString().Trim().Split(')')[0].ToString().Trim();
                query = @"update Session_Courses set status='0' where session_id ='" + sessionid + "'";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                foreach (DataGridViewRow g in dataGridView1.Rows)
                {
                    if (g.Cells[1].Value.ToString() == "True")
                    {
                        query = @"insert into Session_Courses values('" + id + "','" + sessionid + "','" + g.Cells[0].Value.ToString() + "','" + "1" + "')";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                        id = (int.Parse(id) + 1).ToString();
                    }
                }
                MessageBox.Show("Successfully Saved");
                filldata();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Course arc = new Course();
            arc.ShowDialog();
            filldata();
            //try
            //{
            //    if (textBox1.Text.Trim() == "" || textBox1.Text.Trim().Length < 1)
            //    {
            //        MessageBox.Show("Enter Course Code For Update");
            //        return;
            //    }
            //    if (textBox3.Text.Trim() == "" || textBox3.Text.Trim().Length < 1)
            //    {
            //        MessageBox.Show("Enter Course Credit Hours");
            //        return;
            //    }
            //    if (dataGridView1.SelectedRows.Count > 0)
            //    {
            //        if (textBox2.Text.Trim().Length > 0)
            //        {
            //            if (con.State.ToString() == "Closed")
            //            {
            //                con.Open();
            //            }
            //            string query = @"select * from Courses where Course_name='" + textBox2.Text.Trim() + "' and status='1'";
            //            SqlCommand cmd = new SqlCommand(query, con);
            //            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            //            DataTable dt = new DataTable();
            //            sda.Fill(dt);

            //            if (dt.Rows.Count <= 0)
            //            {
            //                query = @"select * from Courses where Course_code='" + textBox1.Text.Trim() + "' and status='1'";
            //                cmd = new SqlCommand(query, con);
            //                sda = new SqlDataAdapter(cmd);
            //                dt = new DataTable();
            //                sda.Fill(dt);

            //                if (dt.Rows.Count <= 0)
            //                {
            //                    query = @"update Courses set Course_code='" + textBox1.Text.Trim() + "',Course_name='" + textBox2.Text.Trim() + "',Credit_hours='" + textBox3.Text.Trim() + "',Course_description='" + richTextBox1.Text.Trim() + "' where id = '" + dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString().Trim() + "'";
            //                    cmd = new SqlCommand(query, con);
            //                    cmd.ExecuteNonQuery();
            //                    MessageBox.Show("Successfully Updated");
            //                    reset();
            //                    filldata();
            //                }
            //                else
            //                {
            //                    MessageBox.Show("Course Code Already Exist.");
            //                }
            //            }
            //            else
            //            {
            //                MessageBox.Show("Course Name Already Exist.");
            //            }
            //        }
            //        else
            //        {
            //            MessageBox.Show("Enter Course Name For Update");
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
            Session arc = new Session();
            arc.ShowDialog();
            filldata();
            //try
            //{
            //    if (dataGridView1.SelectedRows.Count > 0)
            //    {
            //        if (con.State.ToString() == "Closed")
            //        {
            //            con.Open();
            //        }
            //        string query = @"update Courses set status='0' where id = '" + dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString().Trim() + "'";
            //        SqlCommand cmd = new SqlCommand(query, con);
            //        cmd.ExecuteNonQuery();
            //        MessageBox.Show("Successfully Deleted");
            //        reset();
            //        filldata();
            //    }
            //    else
            //    {
            //        MessageBox.Show("Select Row For Delete");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                     dataGridView1.Rows[i].Visible = true;
            }
            for (int j = 0; j < dataGridView1.Rows.Count; j++)
            {
                dataGridView1.Rows[j].Cells[1].Value = false;
                dataGridView1.Rows[j].DefaultCellStyle.BackColor = Color.White;
            }
                try
                {
                    if (con.State.ToString() == "Closed")
                    {
                        con.Open();
                    }
                    string id = listBox1.SelectedItem.ToString().Split('(')[1].ToString().Split(')')[0].ToString().Trim();
                    string query = @"select * from Session_Courses where session_id = '" + id + "' and status='1'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    List<int> rows = new List<int>();
                    foreach (DataRow d in dt.Rows)
                    {
                        int i = 0;
                        foreach (DataGridViewRow g in dataGridView1.Rows)
                        {
                            if (g.Cells[0].Value.ToString() == d[2].ToString())
                            {
                                rows.Add(i);

                            }
                            i++;
                        }
                    }
                    foreach (int j in rows)
                    {
                        dataGridView1.Rows[j].Cells[1].Value = true;
                        dataGridView1.Rows[j].DefaultCellStyle.BackColor = Color.LightGreen;
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

            //try
            //{
            //    dataGridView1.Rows[e.RowIndex].Selected = true;
            //    textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString().Trim().ToString();
            //    textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString().Trim().ToString();
            //    textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString().Trim().ToString();
            //    richTextBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString().Trim().ToString();
            //    if (e.ColumnIndex == 1)
            //    {
            //        if (listBox1.SelectedIndex >= 0 && listBox1.SelectedItem.ToString().Trim() != "")
            //        {
            //            if (dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() == "True")
            //            {
            //                dataGridView1.Rows[e.RowIndex].Cells[1].Value = false;
            //            }
            //            else
            //            {
            //                dataGridView1.Rows[e.RowIndex].Cells[1].Value = true;
            //            }
            //        }
            //        else
            //        {
            //            MessageBox.Show("Select Session First");
            //        }
            //    }
            //}
            //catch { }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString().Trim().ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString().Trim().ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString().Trim().ToString();
                richTextBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString().Trim().ToString();
                if (e.ColumnIndex == 1)
                {
                    if (listBox1.SelectedIndex >= 0 && listBox1.SelectedItem.ToString().Trim() != "")
                    {
                        if (dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() == "True")
                        {
                            dataGridView1.Rows[e.RowIndex].Cells[1].Value = false;
                            dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                        }
                        else
                        {
                            dataGridView1.Rows[e.RowIndex].Cells[1].Value = true;
                            dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Select Session First");
                    }
                    dataGridView1.ClearSelection();
                }
            }
            catch { }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem.ToString() == "" || listBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Select Session First");
                return;
            }
            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if(dataGridView1.Rows[i].Cells[1].Value.ToString() == "False")
                        dataGridView1.Rows[i].Visible = false;
                }
            }
            catch { }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Visible = true;
            }
        }
    }
}
