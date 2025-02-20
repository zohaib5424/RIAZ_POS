using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;

namespace RJ
{
    public partial class LicenseKey : MetroFramework.Forms.MetroForm
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public LicenseKey()
        {
            InitializeComponent();
        }

        private void metrometroTile1_Click(object sender, EventArgs e)
        {
        }
        public void getlicencekey()
        {
            try
            {
                if (con.State.ToString() == "Closed")
                    con.Open();
                string query = "select lk from mml";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                string key = "";
                foreach (DataRow d in dt.Rows)
                {
                    key = d[0].ToString();
                }
                metroTextBox1.Text = key;

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void LicenceKey_Load(object sender, EventArgs e)
        {
            metroTextBox1.MaxLength = 20;
            //metroTextBox1.Text = "Permanently Active";
            //metroTextBox1.ReadOnly = true;
            //metroTextBox1.Enabled = false;
            //metroTile1.Enabled = false;

            //getlicencekey();
        }

        private void metroTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                metroTile1.PerformClick();
        }

        private void LicenceKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose(true);
            }
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            bool ok = false;
            try
            {
                string dfrom = "";
                string dto = "";
                CultureInfo ci = CultureInfo.InvariantCulture;
                if ((DateTime.Now.Date >= DateTime.ParseExact("01/01/2020", "MM/dd/yyyy", ci).Date) && (DateTime.Now.Date <= DateTime.ParseExact("01/01/2021", "MM/dd/yyyy", ci).Date) && (metroTextBox1.Text.Trim().ToString() == "mm051012gpraa0192018"))
                {
                    dfrom = "2020-01-01";
                    dto = "2020-01-01";
                    ok = true;
                }
                else if (ok == false && (DateTime.Now.Date >= DateTime.ParseExact("01/01/2021", "MM/dd/yyyy", ci).Date) && (DateTime.Now.Date <= DateTime.ParseExact("01/01/3000", "MM/dd/yyyy", ci).Date) && (metroTextBox1.Text.Trim().ToString() == "mm051gp01292019raa02"))
                {
                    dfrom = "2020-01-01";
                    dto = "2020-01-01";
                    ok = true;
                }
                else if (ok == false && (DateTime.Now.Date >= DateTime.ParseExact("09/26/2020", "MM/dd/yyyy", ci).Date) && (DateTime.Now.Date <= DateTime.ParseExact("09/26/2021", "MM/dd/yyyy", ci).Date) && (metroTextBox1.Text.Trim().ToString() == "mm051012gpraa03920200"))
                {
                    dfrom = "2020-09-26";
                    dto = "2021-09-26";
                    ok = true;
                }
                else if (ok == false && (DateTime.Now.Date >= DateTime.ParseExact("09/26/2021", "MM/dd/yyyy", ci).Date) && (DateTime.Now.Date <= DateTime.ParseExact("09/26/2022", "MM/dd/yyyy", ci).Date) && (metroTextBox1.Text.Trim().ToString() == "mm051gp01292021raa04"))
                {
                    dfrom = "2021-09-26";
                    dto = "2022-09-26";
                    ok = true;
                }
                else if (ok == false && (DateTime.Now.Date >= DateTime.ParseExact("09/26/2022", "MM/dd/yyyy", ci).Date) && (DateTime.Now.Date <= DateTime.ParseExact("09/26/2023", "MM/dd/yyyy", ci).Date) && (metroTextBox1.Text.Trim().ToString() == "mm051012gpraa0592022"))
                {
                    dfrom = "2022-09-26";
                    dto = "2023-09-26";
                    ok = true;
                }
                else if (ok == false && (DateTime.Now.Date >= DateTime.ParseExact("09/26/2023", "MM/dd/yyyy", ci).Date) && (DateTime.Now.Date <= DateTime.ParseExact("09/26/2024", "MM/dd/yyyy", ci).Date) && (metroTextBox1.Text.Trim().ToString() == "mm051gp01292023raa06"))
                {
                    dfrom = "2023-09-26";
                    dto = "2024-09-26";
                    ok = true;
                }
                else if (ok == false && (DateTime.Now.Date >= DateTime.ParseExact("09/26/2024", "MM/dd/yyyy", ci).Date) && (DateTime.Now.Date <= DateTime.ParseExact("09/26/2025", "MM/dd/yyyy", ci).Date) && (metroTextBox1.Text.Trim().ToString() == "mm051012gpraa0792024"))
                {
                    dfrom = "2024-09-26";
                    dto = "2025-09-26";
                    ok = true;
                }
                else if (ok == false && (DateTime.Now.Date >= DateTime.ParseExact("09/26/2025", "MM/dd/yyyy", ci).Date) && (DateTime.Now.Date <= DateTime.ParseExact("09/26/2026", "MM/dd/yyyy", ci).Date) && (metroTextBox1.Text.Trim().ToString() == "mm051gp01292025raa08"))
                {
                    dfrom = "2025-09-26";
                    dto = "2026-09-26";
                    ok = true;
                }
                else if (ok == false && (DateTime.Now.Date >= DateTime.ParseExact("09/26/2026", "MM/dd/yyyy", ci).Date) && (DateTime.Now.Date <= DateTime.ParseExact("09/26/2027", "MM/dd/yyyy", ci).Date) && (metroTextBox1.Text.Trim().ToString() == "mm051012gpraa0992026"))
                {
                    dfrom = "2026-09-26";
                    dto = "2027-09-26";
                    ok = true;
                }
                else if (ok == false && (DateTime.Now.Date >= DateTime.ParseExact("09/26/2027", "MM/dd/yyyy", ci).Date) && (DateTime.Now.Date <= DateTime.ParseExact("09/26/2028", "MM/dd/yyyy", ci).Date) && (metroTextBox1.Text.Trim().ToString() == "mm051gp01292027raa10"))
                {
                    dfrom = "2027-09-26";
                    dto = "2028-09-26";
                    ok = true;
                }
                if (ok == false)
                {
                    MessageBox.Show("Invalid Key. Re-Enter Key");
                }
                else
                {
                    if (con.State.ToString() == "Closed")
                        con.Open();
                    string query = "delete from mml";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    query = "insert into mml values('1','" + metroTextBox1.Text.Trim().ToString() + "','" + dfrom + "','" + dto + "','1')";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Key Successfully Updated");
                    metroTextBox1.Text = "";
                    getlicencekey();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
