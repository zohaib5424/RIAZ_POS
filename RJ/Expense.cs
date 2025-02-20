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
    public partial class Expense : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public Expense()
        {
            InitializeComponent();
        }

        private void ClassType_Load(object sender, EventArgs e)
        {
            metroLabel6.Hide();
            metroTextBoxPaidAmount.Hide();
            getExpenses();
            metroDateTime1.Value = DateTime.Now;
            textBoxExpneseName.Focus();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (panel2.Location.X >= (this.Width - panel2.Width))
            //{
            //    panel2.Location = new Point(panel2.Location.X - 10, panel2.Location.Y);
            //}
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                textBoxExpneseName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString().Trim();
                metroTextBoxAmount.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString().Trim();
                metroTextBoxPaidAmount.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString().Trim();
                metroTextBoxDescription.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString().Trim();
                try
                {
                    string iDate = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                    DateTime oDate = Convert.ToDateTime(iDate);
                    metroDateTime1.Value = oDate;
                }
                catch (Exception ex1)
                {
                    MessageBox.Show(ex1.Message);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                    //dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Selected = true;
                    //textBoxExpneseName.Text = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[1].Value.ToString().Trim();
                    //metroTextBoxAmount.Text = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[2].Value.ToString().Trim();
                    //metroTextBoxPaidAmount.Text = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[3].Value.ToString().Trim();
                    //metroTextBoxDescription.Text = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[5].Value.ToString().Trim();
                    //try
                    //{
                    //    string iDate = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[4].Value.ToString();
                    //    DateTime oDate = Convert.ToDateTime(iDate);
                    //    metroDateTime1.Value = oDate;
                    //}
                    //catch (Exception ex1)
                    //{
                    //}
            }
            catch (Exception ex)
            {
            }
        }

        private void Brands_KeyDown(object sender, KeyEventArgs e)
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

        GMDB gm = new GMDB();
        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            gm.AcceptDouble(sender, e);
        }

        public void reset()
        {
            try {
                metroDateTime1.Value = DateTime.Now;
                textBoxExpneseName.Text = "";
                metroTextBoxAmount.Text = "";
                metroTextBoxPaidAmount.Text = "";
                metroTextBoxDescription.Text = "";
                textBoxExpneseName.Focus();
            }
            catch { }
        }

        public void getExpenses()
        {
            try
            {
                if(dataGridView1.Rows.Count>0)
                    dataGridView1.Rows.Clear();
                string query = "select * from bill where bill_type='Expense' and status='1'";
                DataTable dt = gm.GetTable(query);
                foreach(DataRow d in dt.Rows)
                {
                    dataGridView1.Rows.Add(d["id"].ToString(),d["bill_id"].ToString(),d["total_amount"].ToString(),d["paid_amount"].ToString(),d["bill_date"].ToString(),d["_description"].ToString());
                }
                dataGridView1.ClearSelection();
            }
            catch { }
        }

        private void metroTile5_Click(object sender, EventArgs e)
        {
            try
            {
                double balance = 0;
                double total = 0;
                double paid = 0;
                try
                {
                    total = double.Parse(((metroTextBoxAmount.Text.Trim() == "") ? "0" : metroTextBoxAmount.Text.Trim()));
                }
                catch { }
                try
                {
                    paid = double.Parse(((metroTextBoxPaidAmount.Text.Trim() == "") ? "0" : metroTextBoxPaidAmount.Text.Trim()));
                }
                catch { }
                balance = (total - paid);
                string query = "Select max(cast(id as int)) from bill";
                string id = gm.MaxId(query);
                query = @"insert into bill values('" + id + "','" + "Expense" + "','"+textBoxExpneseName.Text.Trim()+"','" + metroDateTime1.Value.Date + "','" + DateTime.Now.ToShortTimeString() + "','" + ((metroTextBoxAmount.Text.Trim() == "") ? "0" : metroTextBoxAmount.Text.Trim()) + "','','0','0','','0','0','','0','" + ((metroTextBoxAmount.Text.Trim().ToString() == "") ? "0" : metroTextBoxAmount.Text.Trim().ToString()) + "','" + ((metroTextBoxPaidAmount.Text.Trim().ToString() == "") ? "0" : metroTextBoxPaidAmount.Text.Trim().ToString()) + "','" + balance.ToString()+"','" + metroTextBoxDescription.Text.Trim() + "','','','','','','','','','','','','','','" + RJ.Properties.Settings.Default.loginid + "','" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.ToShortTimeString() + "','1')";
                int ra = gm.ExecuteNonQuery(query);
                if (ra > 0)
                {
                    MessageBox.Show("Expense Successfully Generated");
                    reset();
                    getExpenses();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void metroTextBox1_TextChanged_1(object sender, EventArgs e)
        {
            gm.AcceptDouble(sender, e);
        }

        private void metroPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
