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
    public partial class AddmissionCertificateCrystalReportViewer : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public AddmissionCertificateCrystalReportViewer()
        {
            InitializeComponent();
        }

        private void AddmissionCertificateCrystalReportViewer_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            try
            {
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }
                string query = @"select reg,pic from student";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                dt.Columns[0].ColumnName = "reg";
                dt.Columns[1].ColumnName = "pic";

                MessageBox.Show(dt.Rows.Count.ToString());
                AddmissionCertificat rpt = new AddmissionCertificat();
                rpt.OpenSubreport("subReport1").SetDataSource(dt);
                rpt.SetParameterValue("heading", "GM REHMAN");
                crystalReportViewer1.ReportSource = rpt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public AddmissionCertificateCrystalReportViewer(DataTable dt, string heading, string fromDate, string toDate)
            : this()
        {
            dt.Columns[0].ColumnName = "reg";
            dt.Columns[1].ColumnName = "pic";

            AddmissionCertificat rpt = new AddmissionCertificat();
                rpt.OpenSubreport("subReport1").SetDataSource(dt);
                rpt.SetParameterValue("Heading", "GM REHMAN");
                crystalReportViewer1.ReportSource = rpt;
        }
}
}
