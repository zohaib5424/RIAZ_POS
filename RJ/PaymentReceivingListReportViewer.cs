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
    public partial class PaymentReceivingListReportViewer : Form
    {
        public PaymentReceivingListReportViewer()
        {
            InitializeComponent();
        }

        public string Cashier;
        public string Report;
        public string TransactionNumber;
        public string TransactionDate;

        public string PaymentToOrReceivingFrom;
        public string PaymentToOrReceivingFromValue;
        public string Amount;
        public string PaymentDueBefore;
        public string PaymentDueBeforeValue;
        public string PaymentDueAfter;
        public string PaymentDueAfterValue;
        public string UniqueId;
        public string Today_Bill_Amount;
        private void ItemsListReportViewer_Load(object sender, EventArgs e)
        {
            try
            {
                Microsoft.Reporting.WinForms.ReportParameter[] p = new Microsoft.Reporting.WinForms.ReportParameter[19];
                p[0] = new Microsoft.Reporting.WinForms.ReportParameter("Company_Name", RJ.Properties.Settings.Default.SchoolName, false);
                p[1] = new Microsoft.Reporting.WinForms.ReportParameter("Address", RJ.Properties.Settings.Default.address, false);
                p[2] = new Microsoft.Reporting.WinForms.ReportParameter("Contact_Number", RJ.Properties.Settings.Default.contact, false);
                string monthName = DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture);
                //p[3] = new Microsoft.Reporting.WinForms.ReportParameter("Date", (DateTime.Now.Date.Day.ToString() + " " + monthName + "," + DateTime.Now.Date.Year.ToString()), false);
                p[3] = new Microsoft.Reporting.WinForms.ReportParameter("Date", (DateTime.Now.Date.Day.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Year.ToString()), false);
                p[4] = new Microsoft.Reporting.WinForms.ReportParameter("Time", DateTime.Now.ToShortTimeString(), false);
                p[5] = new Microsoft.Reporting.WinForms.ReportParameter("Report", Report, false);
                p[6] = new Microsoft.Reporting.WinForms.ReportParameter("Cashier", Cashier, false);
                p[7] = new Microsoft.Reporting.WinForms.ReportParameter("TransactionNumber", TransactionNumber, false);
                p[8] = new Microsoft.Reporting.WinForms.ReportParameter("TransactionDate", TransactionDate, false);
                p[9] = new Microsoft.Reporting.WinForms.ReportParameter("PaymentToOrReceivingFrom", PaymentToOrReceivingFrom, false);
                p[10] = new Microsoft.Reporting.WinForms.ReportParameter("PaymentToOrReceivingFromValue", PaymentToOrReceivingFromValue, false);
                p[11] = new Microsoft.Reporting.WinForms.ReportParameter("Amount", Amount, false);

                p[12] = new Microsoft.Reporting.WinForms.ReportParameter("PaymentDueBefore", PaymentDueBefore, false);
                p[13] = new Microsoft.Reporting.WinForms.ReportParameter("PaymentDueBeforeValue", PaymentDueBeforeValue, false);
                p[14] = new Microsoft.Reporting.WinForms.ReportParameter("PaymentDueAfter", PaymentDueAfter, false);
                p[15] = new Microsoft.Reporting.WinForms.ReportParameter("PaymentDueAfterValue", PaymentDueAfterValue, false);
                p[16] = new Microsoft.Reporting.WinForms.ReportParameter("UniqueId", UniqueId, false);
                p[17] = new Microsoft.Reporting.WinForms.ReportParameter("Today_Bill_Amount", Today_Bill_Amount, false);
                double total_amount1 = 0;
                try
                {
                    total_amount1 = double.Parse(Today_Bill_Amount) + double.Parse(PaymentDueBeforeValue);
                }
                catch { }
                p[18] = new Microsoft.Reporting.WinForms.ReportParameter("Total_Amount1", total_amount1.ToString(), false);
                reportViewer1.LocalReport.SetParameters(p);

                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
