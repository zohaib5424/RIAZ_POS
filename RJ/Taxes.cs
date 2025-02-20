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
    public partial class Taxes : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public Taxes()
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
                string query = @"select * from taxes where status!='-1'";
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

                    if (d["status"].ToString() == "1")
                        dataGridView1.Rows.Add(d[0].ToString(), d[1].ToString(), d[2].ToString(), "Disable");
                    else
                        dataGridView1.Rows.Add(d[0].ToString(), d[1].ToString(), d[2].ToString(), "Enable");
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
                if (metroTextBox1.Text.Trim().Length <= 0)
                {
                    MessageBox.Show("Enter Tax Percentage");
                    return;
                }
                if (textBox1.Text.Trim() != "" && textBox1.Text.Trim().Length > 0)
                {
                    if (con.State.ToString() == "Closed")
                    {
                        con.Open();
                    }
                    string query = @"select max(cast(id as int)) from  taxes";
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

                    query = @"select * from taxes where tax_name='" + data2 + "' and status !='-1'";
                    cmd = new SqlCommand(query, con);
                    sda = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    sda.Fill(dt);

                    if (dt.Rows.Count <= 0)
                    {
                        query = @"insert into taxes values('" + id + "',N'" + data2 + "',N'"+metroTextBox1.Text.Trim()+"','"+RJ.Properties.Settings.Default.loginid+"','"+DateTime.Now.ToShortDateString()+"','"+DateTime.Now.ToShortTimeString()+"','1')";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                        textBox1.Text = "";
                        metroTextBox1.Text = "";
                        MessageBox.Show("Successfully Saved");
                        filldata();
                    }
                    else
                    {
                        MessageBox.Show("Tax Name Already Exist.");
                    }
                }
                else
                {
                    MessageBox.Show("Enter Tax Name");
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
                textBox1.Tag = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();
                metroTextBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString().Trim();
                dataGridView1.Rows[e.RowIndex].Selected = true;
                if (e.ColumnIndex == 3)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString() == "Enable")
                    {
                        if (con.State.ToString() == "Closed")
                        {
                            con.Open();
                        }
                        string query = @"update taxes set status='1' where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
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
                        string query = @"update taxes set status='0' where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() + "'";
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
                    if (dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[3].Value.ToString().Trim() == "Enable")
                    {
                        MessageBox.Show("First Enable The Record To Update");
                        return;
                    }
                    if (metroTextBox1.Text.Trim().Length <= 0)
                    {
                        MessageBox.Show("Enter Tax Percentage");
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
                        string query = @"select * from taxes where  tax_name='" + data2 + "' and status !='-1' and id!='"+metroTextBox1.Tag+"'";
                        MessageBox.Show(metroTextBox1.Tag.ToString());
                        SqlCommand cmd = new SqlCommand(query, con);
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        if (dt.Rows.Count <= 0)
                        {
                            query = @"update taxes set  tax_name='" + data2 + "',Tax_Percent='"+metroTextBox1.Text.Trim().ToString()+"' where id = '" + dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString().Trim() + "'";
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Successfully Updated");
                            reset();
                            filldata();
                        }
                        else
                        {
                            MessageBox.Show("Tax Name Already Exist.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Enter Tax Name For Update");
                    }
                }
                else
                {
                    MessageBox.Show("Select Record For Update");
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
            metroTextBox1.Text = "";
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
                    string query = @"update taxes set status='-1' where id = '" + dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString().Trim() + "'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully Deleted");
                    reset();
                    filldata();
                }
                else
                {
                    MessageBox.Show("Select Record For Delete");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void  taxes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose(true);
            }
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose(true);
        }

        private void metroPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        GMDB gm = new GMDB();
        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                gm.AcceptDouble(sender, e);
            }catch{}
        }

        private void metroTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2.PerformClick();
            }
        }

        private void Taxes_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose(true);
            }
        }
    }
}
