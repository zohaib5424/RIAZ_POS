using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using GETPAK_AMS_SQL_CLASS;
using System.Web;
using System.Runtime.InteropServices;
using System.Web.Services;
using System.Diagnostics;
using GETPAK_AMS;

namespace gmCalenderNote
{
    public partial class View_Note : Form
    {
        public View_Note()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(GETPAK_AMS.Properties.Settings.Default.Connectionstring);


        public void nt()
        {
            if (i == 1)
            {
                dataGridView1.Rows.Clear();
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("select id,readd from notedetail where to_id='" + LoginUser.userLogin + "'", con);
                SqlDataAdapter d1 = new SqlDataAdapter(cmd);
                DataTable t = new DataTable();
                d1.Fill(t);
                foreach (DataRow w in t.Rows)
                {
                    cmd = new SqlCommand("select * from note where id='" + w[0].ToString() + "'", con);
                    SqlDataReader d = cmd.ExecuteReader();
                    while (d.Read())
                    {
                        dataGridView1.Rows.Add(d[1].ToString(), d[3].ToString(), d[2].ToString(), w[0].ToString());
                        if (w[1].ToString() == "0")
                        {
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Style.BackColor = Color.SkyBlue;
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Style.BackColor = Color.SkyBlue;
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Style.BackColor = Color.SkyBlue;
                        }
                    }
                    d.Close();
                    dataGridView1.Rows[0].Cells[0].Selected = false;
                }
            }
            else if (i == 2)
            {
                dataGridView1.Rows.Clear();
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }
                dataGridView1.Columns[0].HeaderText = "To";
                SqlCommand cmd = new SqlCommand("select id,to_id,readd from notedetail where userid='" + LoginUser.userLogin + "'", con);
                SqlDataAdapter d1 = new SqlDataAdapter(cmd);
                DataTable t = new DataTable();
                d1.Fill(t);
                foreach (DataRow w in t.Rows)
                {
                    cmd = new SqlCommand("select * from note where id='" + w[0].ToString() + "'", con);
                    SqlDataReader d = cmd.ExecuteReader();
                    while (d.Read())
                    {
                        dataGridView1.Rows.Add(w[1].ToString(), d[3].ToString(), d[2].ToString(), w[0].ToString());
                        if (w[2].ToString() == "0")
                        {
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Style.BackColor = Color.SkyBlue;
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Style.BackColor = Color.SkyBlue;
                            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Style.BackColor = Color.SkyBlue;
                        }
                    }
                    d.Close();
                    dataGridView1.Rows[0].Cells[0].Selected = false;
                }
            }
        }
        public int i = 0;
        private void View_Note_Load(object sender, EventArgs e)
        {
            nt();
            richTextBox1.ReadOnly = true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].Selected = true;
            try
            {
                to = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                td = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                richTextBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("update notedetail set readd='1' where id='" + dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString() + "' and to_id='" + LoginUser.userLogin + "'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("update notedetail set readd='1' where id='" + dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString() + "'", con);
                cmd.ExecuteNonQuery();
                nt();
                dataGridView1.Rows[e.RowIndex].Selected = false;
            }
            catch (Exception ex)
            {

            }
        }
        string to = "";
        string td = "";
        private void button2_Click(object sender, EventArgs e)
        {
            if (richTextBox2.Text != "")
            {
                    if (con.State.ToString() == "Closed")
                    {
                        con.Open();
                    }
                    string msg = richTextBox1.Text+"\n"+ LoginUser.userLogin + "   (" + DateTime.Now.ToString() + ")" + "\n" + richTextBox2.Text.Trim()+"\n";
                    //cmd = new SqlCommand("insert into notedetail values('" + id + "','" + userid + "','" + tou + "')", con);
                    //cmd.ExecuteNonQuery();
                    SqlCommand cmd = new SqlCommand("update note set note='" + msg + "' where id='"+td+"'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("update notedetail set readd='0' where id='" + td + "'", con);
                    int ra = cmd.ExecuteNonQuery();
                    if (ra > 0)
                    {
                        MessageBox.Show("Reply Succeed");
                        richTextBox2.Text = "";
                    }
            }
            else
            {
                MessageBox.Show("Reply box Should not be empty");
            }
        }
    }
}
