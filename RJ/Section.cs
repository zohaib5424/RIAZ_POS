﻿using System;
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
    public partial class Section : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public Section()
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
                string query = @"select * from sections where status!='-1'";
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
                    
                    if (d[2].ToString() == "1")
                        dataGridView1.Rows.Add(d[0].ToString(), d[1].ToString(), "Disable");
                    else
                        dataGridView1.Rows.Add(d[0].ToString(), d[1].ToString(), "Enable");
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
                if (textBox1.Text.Trim() != "" && textBox1.Text.Trim().Length > 0)
                {
                    if (con.State.ToString() == "Closed")
                    {
                        con.Open();
                    }
                    string query = @"select max(cast(id as int)) from Sections";
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
                    string data2 = "";
                    string[] abc = data.Split(' ');
                    foreach (string a in abc)
                    {
                        data2 += a.Substring(0, 1).ToUpper() + a.Substring(1, a.Length - 1);
                        data2 += " ";
                    }

                    query = @"select * from Sections where Section='" + data2 + "' and status !='-1'";
                    cmd = new SqlCommand(query, con);
                    sda = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    sda.Fill(dt);

                    if (dt.Rows.Count <= 0)
                    {
                        query = @"insert into Sections values('" + id + "','" + data2 + "','1')";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                        textBox1.Text = "";
                        MessageBox.Show("Successfully Saved");
                        filldata();
                    }
                    else
                    {
                        MessageBox.Show("Section Already Exist.");
                    }
                }
                else
                {
                    MessageBox.Show("Enter Section");
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString().Trim();
                dataGridView1.Rows[e.RowIndex].Selected = true;
                if (e.ColumnIndex == 2)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() == "Enable")
                    {
                        if (con.State.ToString() == "Closed")
                        {
                            con.Open();
                        }
                        string query = @"update sections set status='1' where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
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
                        string query = @"update sections set status='0' where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    if (dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[2].Value.ToString().Trim() == "Enable")
                    {
                        MessageBox.Show("First Enable The Record To Update");
                        return;
                    }
                    if (textBox1.Text.Trim().Length > 0)
                    {
                        if (con.State.ToString() == "Closed")
                        {
                            con.Open();
                        }
                        string data = textBox1.Text.Trim().ToLower().ToString();
                        string data2 = "";
                        string[] abc = data.Split(' ');
                        foreach (string a in abc)
                        {
                            data2 += a.Substring(0, 1).ToUpper() + a.Substring(1, a.Length - 1);
                            data2 += " ";
                        }
                        string query = @"select * from Sections where Section='" + data2 + "' and status !='-1'";
                        SqlCommand cmd = new SqlCommand(query, con);
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        if (dt.Rows.Count <= 0)
                        {
                            query = @"update Sections set Section='" + data2 + "' where id = '" + dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString().Trim() + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Successfully Updated");
                            reset();
                            filldata();
                        }
                        else
                        {
                            MessageBox.Show("Section Already Exist.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Enter Section For Update");
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
            textBox1.Text = "";
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
                    string query = @"update Sections set status='-1' where id = '" + dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString().Trim() + "'";
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
