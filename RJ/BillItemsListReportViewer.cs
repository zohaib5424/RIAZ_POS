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
    public partial class BillItemsListReportViewer : Form
    {
        public BillItemsListReportViewer()
        {
            InitializeComponent();
        }

        public DataTable dt;
        public string Cashier;
        public string Report;
        public string Category;
        public string totalqty;
        public string totalinvoice;
        public string totaltrade;
        public string lowstockreport;
        public string till_date;
        public string date;
        public string Customer;
        private void BillItemsListReportViewer_Load(object sender, EventArgs e)
        {
            try
            {
                Microsoft.Reporting.WinForms.ReportParameter[] p = new Microsoft.Reporting.WinForms.ReportParameter[2];
                //p[0] = new Microsoft.Reporting.WinForms.ReportParameter("Company_Name", RJ.Properties.Settings.Default.SchoolName, false);
                //p[1] = new Microsoft.Reporting.WinForms.ReportParameter("Address", RJ.Properties.Settings.Default.address, false);
                //p[2] = new Microsoft.Reporting.WinForms.ReportParameter("Contact_Number", RJ.Properties.Settings.Default.contact, false);
                //string monthName = DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture);
                p[0] = new Microsoft.Reporting.WinForms.ReportParameter("Date", date, false);
                p[1] = new Microsoft.Reporting.WinForms.ReportParameter("Customer", Customer, false);
                //p[4] = new Microsoft.Reporting.WinForms.ReportParameter("Time", DateTime.Now.ToShortTimeString(), false);
                //p[5] = new Microsoft.Reporting.WinForms.ReportParameter("Report", Report, false);
                //p[6] = new Microsoft.Reporting.WinForms.ReportParameter("Cashier", Cashier, false);
                //p[7] = new Microsoft.Reporting.WinForms.ReportParameter("Category", Category, false);
                //p[8] = new Microsoft.Reporting.WinForms.ReportParameter("Total_Qty", totalqty, false);
                //if (lowstockreport == "1")
                //{
                //    p[9] = new Microsoft.Reporting.WinForms.ReportParameter("Column_Visible", "False");
                //}
                //else
                //{
                //    p[9] = new Microsoft.Reporting.WinForms.ReportParameter("Column_Visible", "True");
                //}
                //p[10] = new Microsoft.Reporting.WinForms.ReportParameter("Till_Date", till_date, false);
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
