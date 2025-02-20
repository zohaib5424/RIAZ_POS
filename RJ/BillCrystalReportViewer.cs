using Microsoft.Reporting.WebForms;
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
using Microsoft.Reporting.WinForms;

namespace POS_GM
{
    public partial class BillCrystalReportViewer : Form
    {
        public BillCrystalReportViewer()
        {
            InitializeComponent();
        }

        public BillCrystalReportViewer(string transactionid)
            : this()
        {
            try
            {
//                BillCrystalReport rpt = new BillCrystalReport();
//                DataTable BillDetails = new DataTable();
//                BillDetails.Columns.Add("Sr_No");
//                BillDetails.Columns.Add("Item");
//                BillDetails.Columns.Add("Unit_Cost");
//                BillDetails.Columns.Add("Qty");
//                BillDetails.Columns.Add("Total");
//                BillDetails.Rows.Add("1", "abc", "def", "ghi", "jkl");
//                BillDetails.Rows.Add("1", "abc", "def", "ghi", "jkl");
//                BillDetails.Rows.Add("1", "abc", "def", "ghi", "jkl");
//                BillDetails.Rows.Add("1", "abc", "def", "ghi", "jkl");
//                BillDetails.Rows.Add("1", "abc", "def", "ghi", "jkl");
//                BillDetails.Rows.Add("1", "abc", "def", "ghi", "jkl");
////                rpt.OpenSubreport("subReport1").SetDataSource(BillDetails);
//                rpt.SetParameterValue("Company_Name", POS_GM.Properties.Settings.Default.SchoolName);
//                rpt.SetParameterValue("Address", POS_GM.Properties.Settings.Default.address);
//                rpt.SetParameterValue("Contact_Number", POS_GM.Properties.Settings.Default.contact);
//                //string monthName = DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture);
//                //rpt.SetParameterValue("Date", DateTime.Now.Date.Day.ToString()+" "+monthName+","+DateTime.Now.Date.Year.ToString());
//                //rpt.SetParameterValue("Time", DateTime.Now.ToShortTimeString());
//                //rpt.SetParameterValue("Transaction_Id", transactionid);
//                crystalReportViewer1.ReportSource = rpt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public DataTable dt = new DataTable();
        public string transactionid;
        public string totalqty;
        public string Net_Total;
        public string Discount_Amount;
        public string Tax_Amount;
        public string Service_Charges;
        public string Total_Amount;
        public string Cashier;
        public string Customer;
        public string Receipt_Type;
        public string Paid_Amount;
        public string Change;
        public string Bill_Type;
        private void BillCrystalReportViewer_Load(object sender, EventArgs e)
        {
            Microsoft.Reporting.WinForms.ReportParameter[] p = new Microsoft.Reporting.WinForms.ReportParameter[18];
            p[0] = new Microsoft.Reporting.WinForms.ReportParameter("Company_Name", POS_GM.Properties.Settings.Default.SchoolName.ToString(), false);
            p[1] = new Microsoft.Reporting.WinForms.ReportParameter("Address", POS_GM.Properties.Settings.Default.address.ToString(), false);
            p[2] = new Microsoft.Reporting.WinForms.ReportParameter("Contact_Number", POS_GM.Properties.Settings.Default.contact.ToString(), false);
            string monthName = DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture);
            p[3] = new Microsoft.Reporting.WinForms.ReportParameter("Date", (DateTime.Now.Date.Day.ToString() + " " + monthName + "," + DateTime.Now.Date.Year.ToString()), false);
            p[4] = new Microsoft.Reporting.WinForms.ReportParameter("Time", DateTime.Now.ToShortTimeString(), false);
            p[5] = new Microsoft.Reporting.WinForms.ReportParameter("Transaction_Id", transactionid, false);
            p[6] = new Microsoft.Reporting.WinForms.ReportParameter("totalqty", totalqty, false);
            p[7] = new Microsoft.Reporting.WinForms.ReportParameter("nettotal", Net_Total, false);

            p[8] = new Microsoft.Reporting.WinForms.ReportParameter("Discount_Amount", Discount_Amount, false);
            p[9] = new Microsoft.Reporting.WinForms.ReportParameter("Tax_Amount", Tax_Amount, false);//tax amount (gst@10) 280 (gst@10)
            p[10] = new Microsoft.Reporting.WinForms.ReportParameter("Service_Charges", Service_Charges, false);
            p[11] = new Microsoft.Reporting.WinForms.ReportParameter("Total_Amount", Total_Amount, false);
            p[12] = new Microsoft.Reporting.WinForms.ReportParameter("Cashier", Cashier, false);
            p[13] = new Microsoft.Reporting.WinForms.ReportParameter("Customer", Customer, false);
            p[14] = new Microsoft.Reporting.WinForms.ReportParameter("ReceiptType", Receipt_Type, false);//original or duplicate
            p[15] = new Microsoft.Reporting.WinForms.ReportParameter("Paid_Amount", Paid_Amount, false);
            p[16] = new Microsoft.Reporting.WinForms.ReportParameter("Change", Change, false);
            p[17] = new Microsoft.Reporting.WinForms.ReportParameter("Bill_Type", Bill_Type, false);//sale invoice,purchase invoice
            reportViewer1.LocalReport.SetParameters(p);

            reportViewer1.LocalReport.DataSources.Clear();
            Microsoft.Reporting.WinForms.ReportDataSource RDS1 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt);
            reportViewer1.LocalReport.EnableExternalImages = true;
            reportViewer1.LocalReport.DataSources.Add(RDS1);

            this.reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
