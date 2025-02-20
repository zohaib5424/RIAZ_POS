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
    public partial class Profit_Loss : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public Profit_Loss()
        {
            InitializeComponent();
        }

        private void ClassType_Load(object sender, EventArgs e)
        {
            getData();
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

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
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

           }
            catch { }
        }

        public void getData()
        {
            try
            {
                dataGridView1.Rows.Clear();
                metroLabel8.Text = "Total";
                labelNetTotal.Text = (0).ToString();
                labelNetTotal.ForeColor = Color.Black;

                double purchase_production_material = 0;//(-)(dr)(total amount)
                try
                {
                    string query = "select sum(Total_Amount) from bill where bill_type='Purchase Trading' and bill_id like 'p%' and status!='-1'";
                    DataTable dt = gm.GetTable(query);
                    foreach (DataRow d in dt.Rows)
                    {
                        try
                        {
                            purchase_production_material = double.Parse(d[0].ToString());
                        }
                        catch { }
                    }
                }
                catch { }

                double sale_production_material = 0;//(+)(cr)
                try
                {
                    string query = "select sum(Total_Amount) from bill where bill_type='Sale Trading' and bill_id like 's%' and status!='-1'";
                    DataTable dt = gm.GetTable(query);
                    foreach (DataRow d in dt.Rows)
                    {
                        try
                        {
                            sale_production_material = double.Parse(d[0].ToString());
                        }
                        catch { }
                    }
                }
                catch { }

                double sale_end_products = 0;//(+)(cr)
                try
                {
                    string query = "select sum(Total_Amount) from bill where bill_type='Sale Trading' and bill_id like 'sep%' and status!='-1'";
                    DataTable dt = gm.GetTable(query);
                    foreach (DataRow d in dt.Rows)
                    {
                        try
                        {
                            sale_end_products = double.Parse(d[0].ToString());
                        }
                        catch { }
                    }
                }
                catch { }

                double total_expenses = 0;//(-)(dr)
                try
                {
                    string query = "select sum(Total_Amount) from bill where bill_type='Expense' and status!='-1'";
                    DataTable dt = gm.GetTable(query);
                    foreach (DataRow d in dt.Rows)
                    {
                        try
                        {
                            total_expenses = double.Parse(d[0].ToString());
                        }
                        catch { }
                    }
                }
                catch { }

                //double total_amount_payable = 0;//(-)(dr)
                //try
                //{
                //    //string query = "select sum(balance) from bill where (bill_type='Purchase Trading' or bill_type='Expense') and status!='-1'";
                //    string query = "select sum(balance) from bill where (bill_type='Purchase Trading') and status!='-1'";
                //    DataTable dt = gm.GetTable(query);
                //    foreach (DataRow d in dt.Rows)
                //    {
                //        try
                //        {
                //            total_amount_payable = double.Parse(d[0].ToString());
                //        }
                //        catch { }
                //    }
                //}
                //catch { }

                //double total_amount_receivable = 0;//(+)(cr)
                //try
                //{
                //    string query = "select sum(balance) from bill where bill_type='Sale Trading' and status!='-1'";
                //    DataTable dt = gm.GetTable(query);
                //    foreach (DataRow d in dt.Rows)
                //    {
                //        try
                //        {
                //            total_amount_receivable = double.Parse(d[0].ToString());
                //        }
                //        catch { }
                //    }
                //}
                //catch { }




                dataGridView1.Rows.Add("purchase_production_material", purchase_production_material.ToString(),"");
                dataGridView1.Rows.Add("sale_production_material","",sale_production_material.ToString());
                dataGridView1.Rows.Add("sale_end_products","",sale_end_products.ToString());
                dataGridView1.Rows.Add("total_expenses", total_expenses.ToString(), "");
                //dataGridView1.Rows.Add("total_amount_payable",total_amount_payable.ToString(),"");
                //dataGridView1.Rows.Add("total_amount_receivable", "", total_amount_receivable.ToString());

                double total_dr = (purchase_production_material + total_expenses);
                double total_cr = (sale_production_material + sale_end_products );
                dataGridView1.Rows.Add("", "", "");
                dataGridView1.Rows.Add("", "", "");
                dataGridView1.Rows.Add("Total", total_dr.ToString(), total_cr.ToString());
                

                double total = (total_cr - total_dr);
                if(total>0)
                {
                    metroLabel8.Text = "Net Profit";
                    labelNetTotal.Text = (total).ToString();
                    labelNetTotal.ForeColor = Color.Green;
                }
                else if(total<0)
                {
                    metroLabel8.Text = "Net Loss";
                    labelNetTotal.Text = (-total).ToString();
                    labelNetTotal.ForeColor = Color.Red;
                }
                else if(total==0)
                {
                    metroLabel8.Text = "No Profit No Loss";
                    labelNetTotal.Text = (total).ToString();
                    labelNetTotal.ForeColor = Color.Black;
                }
                dataGridView1.ClearSelection();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void metroTile5_Click(object sender, EventArgs e)
        {
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
