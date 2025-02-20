using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RJ
{
    public partial class LedgerReportViewer : Form
    {
        public LedgerReportViewer()
        {
            InitializeComponent();
        }

        public DataTable dt;
        public string Cashier;
        public string Bill_Date;
        public string Total_Dr;
        public string Total_Cr;
        public string Balance;
        public string LedgerOf;
        private void LedgerReportViewer_Load(object sender, EventArgs e)
        {
            try
            {
                Microsoft.Reporting.WinForms.ReportParameter[] p = new Microsoft.Reporting.WinForms.ReportParameter[11];
                p[0] = new Microsoft.Reporting.WinForms.ReportParameter("Company_Name", RJ.Properties.Settings.Default.SchoolName, false);
                p[1] = new Microsoft.Reporting.WinForms.ReportParameter("Address", RJ.Properties.Settings.Default.address, false);
                p[2] = new Microsoft.Reporting.WinForms.ReportParameter("Contact_Number", RJ.Properties.Settings.Default.contact, false);
                string monthName = DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture);
                //p[3] = new Microsoft.Reporting.WinForms.ReportParameter("Date", (DateTime.Now.Date.Day.ToString() + " " + monthName + "," + DateTime.Now.Date.Year.ToString()), false);
                p[3] = new Microsoft.Reporting.WinForms.ReportParameter("Date", (DateTime.Now.Date.Day.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Year.ToString()), false);
                p[4] = new Microsoft.Reporting.WinForms.ReportParameter("Time", DateTime.Now.ToShortTimeString(), false);
                p[5] = new Microsoft.Reporting.WinForms.ReportParameter("Bill_Date", Bill_Date, false);
                p[6] = new Microsoft.Reporting.WinForms.ReportParameter("Cashier", Cashier, false);
                p[7] = new Microsoft.Reporting.WinForms.ReportParameter("LedgerOf", LedgerOf, false);
                p[8] = new Microsoft.Reporting.WinForms.ReportParameter("Total_Dr", Total_Dr, false);
                p[9] = new Microsoft.Reporting.WinForms.ReportParameter("Total_Cr", Total_Cr, false);
                p[10] = new Microsoft.Reporting.WinForms.ReportParameter("Balance", Balance, false);
                reportViewer1.LocalReport.SetParameters(p);

                reportViewer1.LocalReport.DataSources.Clear();
                Microsoft.Reporting.WinForms.ReportDataSource RDS1 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt);
                reportViewer1.LocalReport.EnableExternalImages = true;
                reportViewer1.LocalReport.DataSources.Add(RDS1);
                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
