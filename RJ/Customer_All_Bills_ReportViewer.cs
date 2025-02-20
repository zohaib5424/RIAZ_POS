//using Microsoft.Reporting.WebForms;
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

namespace RJ
{
    public partial class Customer_All_Bills_ReportViewer : Form
    {
        public Customer_All_Bills_ReportViewer()
        {
            InitializeComponent();
        }

        public Customer_All_Bills_ReportViewer(string transactionid)
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
//                rpt.SetParameterValue("Company_Name", I_Bee_Mini_Mart.Properties.Settings.Default.SchoolName);
//                rpt.SetParameterValue("Address", I_Bee_Mini_Mart.Properties.Settings.Default.address);
//                rpt.SetParameterValue("Contact_Number", I_Bee_Mini_Mart.Properties.Settings.Default.contact);
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
        DataTable dtBill_Info_Copy = new DataTable();
        DataTable dtBill_Info_Total = new DataTable();
        DataTable dtSpaces = new DataTable();
        public DataTable dt_Clients_Info = new DataTable();
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
        public string Customer_Or_Vendor;
        public string Main_Customer_Or_Vendor;
        public string Bill_Type;
        public string Transaction_Date;
        public string TotalWeight;
        private void BillCrystalReportViewer_Load(object sender, EventArgs e)
        {
            dtBill_Info_Copy.Columns.Add("Sr_No");
            dtBill_Info_Copy.Columns.Add("Item");
            dtBill_Info_Copy.Columns.Add("Unit_Cost");
            dtBill_Info_Copy.Columns.Add("Qty");
            dtBill_Info_Copy.Columns.Add("Total");
            dtBill_Info_Copy.Columns.Add("Unit");

            dtBill_Info_Total.Columns.Add("total_amount");
            dtBill_Info_Total.Columns.Add("total_weight");
            dtBill_Info_Total.Columns.Add("weight_in_mun");

            dtSpaces.Columns.Add("value");

            try
            {
                Microsoft.Reporting.WinForms.ReportParameter[] p = new Microsoft.Reporting.WinForms.ReportParameter[19];
                p[0] = new Microsoft.Reporting.WinForms.ReportParameter("Company_Name", RJ.Properties.Settings.Default.SchoolName.ToString(), false);
                p[1] = new Microsoft.Reporting.WinForms.ReportParameter("Address", RJ.Properties.Settings.Default.address.ToString(), false);
                p[2] = new Microsoft.Reporting.WinForms.ReportParameter("Contact_Number", RJ.Properties.Settings.Default.contact.ToString(), false);
                string monthName = DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture);
                p[3] = new Microsoft.Reporting.WinForms.ReportParameter("Date", (DateTime.Now.Date.Day.ToString() + " " + monthName + "," + DateTime.Now.Date.Year.ToString()), false);
                p[4] = new Microsoft.Reporting.WinForms.ReportParameter("Time", DateTime.Now.ToShortTimeString(), false);
                p[5] = new Microsoft.Reporting.WinForms.ReportParameter("totalqty", totalqty, false);
                p[6] = new Microsoft.Reporting.WinForms.ReportParameter("nettotal", Net_Total, false);

                p[7] = new Microsoft.Reporting.WinForms.ReportParameter("Discount_Amount", Discount_Amount, false);
                p[8] = new Microsoft.Reporting.WinForms.ReportParameter("Tax_Amount", Tax_Amount, false);//tax amount (gst@10) 280 (gst@10)
                p[9] = new Microsoft.Reporting.WinForms.ReportParameter("Service_Charges", Service_Charges, false);
                p[10] = new Microsoft.Reporting.WinForms.ReportParameter("Total_Amount", Total_Amount, false);
                p[11] = new Microsoft.Reporting.WinForms.ReportParameter("Cashier", Cashier, false);
                p[12] = new Microsoft.Reporting.WinForms.ReportParameter("Customer", Customer, false);
                //p[13] = new Microsoft.Reporting.WinForms.ReportParameter("ReceiptType", Receipt_Type, false);//original or duplicate
                p[13] = new Microsoft.Reporting.WinForms.ReportParameter("Paid_Amount", Paid_Amount, false);
                p[14] = new Microsoft.Reporting.WinForms.ReportParameter("Change", Change, false);
                p[15] = new Microsoft.Reporting.WinForms.ReportParameter("Bill_Type", Bill_Type, false);//sale invoice,purchase invoice
                p[16] = new Microsoft.Reporting.WinForms.ReportParameter("Customer_Or_Vendor", Customer_Or_Vendor, false);//sale invoice,purchase invoice
                p[17] = new Microsoft.Reporting.WinForms.ReportParameter("TotalWeight", TotalWeight, false);
                p[18] = new Microsoft.Reporting.WinForms.ReportParameter("Main_Customer", Main_Customer_Or_Vendor, false);
                reportViewer1.LocalReport.SetParameters(p);

                reportViewer1.LocalReport.DataSources.Clear();
                //reportViewer1.LocalReport.DataSources.Clear();
                Microsoft.Reporting.WinForms.ReportDataSource RDS1 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt_Clients_Info);
                reportViewer1.LocalReport.EnableExternalImages = true;
                reportViewer1.LocalReport.DataSources.Add(RDS1);

                Microsoft.Reporting.WinForms.ReportDataSource RDS2 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt);
                reportViewer1.LocalReport.EnableExternalImages = true;
                reportViewer1.LocalReport.DataSources.Add(RDS2);
                this.reportViewer1.LocalReport.SubreportProcessing += new Microsoft.Reporting.WinForms.SubreportProcessingEventHandler(localReport_SubreportProcessing);

                Microsoft.Reporting.WinForms.ReportDataSource RDS3 = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet3", dtBill_Info_Total);
                reportViewer1.LocalReport.EnableExternalImages = true;
                reportViewer1.LocalReport.DataSources.Add(RDS3);
                this.reportViewer1.LocalReport.SubreportProcessing += new Microsoft.Reporting.WinForms.SubreportProcessingEventHandler(localReport_SubreportProcessing3);

                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        GMDB gm = new GMDB();
        void localReport_SubreportProcessing(object sender, Microsoft.Reporting.WinForms.SubreportProcessingEventArgs e)
        {
            try
            {
                if (dtBill_Info_Copy.Rows.Count > 0)
                    dtBill_Info_Copy.Rows.Clear();
                var ID = e.Parameters[0].Values[0].ToString();
                string query = "select * from bill_details where bill_id='"+ID+ "' and unit_cost!=0";// in (select id from bill where status!='-1' and id='" + ID + "')";
                DataTable dt = gm.GetTable(query);
                if (dt.Rows.Count > 0)
                {
                    int i = 1;
                    double qty = 0;
                    double totalweight = 0;
                    double totalamount = 0;
                    double nettotal = 0;
                    string total_mazdori1 = "0";
                    foreach (DataRow d in dt.Rows)
                    {
                        string item = "";
                        query = "select * from items where id = '" + d["item_id"].ToString() + "'";
                        DataTable dt2 = gm.GetTable(query);
                        string mazdori1 = "0";
                        string qty1 = "0";
                        try
                        {
                                string item_type1 = d["item_type"].ToString();
                                item = dt2.Rows[0]["Name"].ToString() + " - (" + item_type1 + ")";
                                if (item_type1 == "نارمل")
                                {
                                    item = dt2.Rows[0]["Name"].ToString();
                                    mazdori1 = d["mazdori"].ToString();
                                    qty1 = d["Qty"].ToString();
                                }
                        }
                        catch { }
                        string unit = "";
                        query = "select * from items where id = '" + d["item_id"].ToString() + "'";
                        dt2 = gm.GetTable(query);
                        try
                        {
                            unit = dt2.Rows[0]["Unit_Id"].ToString();
                        }
                        catch { }
                        if (d["item_type"].ToString() == "")
                        {
                            query = "select * from items where id = '" + d["item_id"].ToString() + "'";
                            dt2 = gm.GetTable(query);
                            try
                            {
                                if (d["item_type"].ToString() == "نارمل" || d["item_type"].ToString() == "بل میں")
                                {
                                    totalweight += (double.Parse(dt2.Rows[0]["weight"].ToString()) * qty);
                                }
                            }
                            catch { }
                        }
                        query = "select * from Units where id = '" + unit + "'";
                        dt2 = gm.GetTable(query);
                        try
                        {
                            unit = dt2.Rows[0]["Unit_Name"].ToString();
                        }
                        catch { }
                        try
                        {
                            qty += double.Parse(d["qty"].ToString().Trim().ToString());
                        }
                        catch { }
                        try
                        {
                            total_mazdori1 = (Math.Round((double.Parse(total_mazdori1) + (double.Parse(mazdori1) * double.Parse(qty1))), 0, MidpointRounding.AwayFromZero)).ToString();
                        }
                        catch { }
                        string total_amount2 = d["Total_Amount"].ToString();
                        try
                        {
                            total_amount2 = ((Math.Round((double.Parse(total_amount2) - (double.Parse(mazdori1) * double.Parse(qty1))), 0, MidpointRounding.AwayFromZero))).ToString();
                        }
                        catch { }
                        if (total_amount2.Contains('.'))
                        {
                            total_amount2 = (total_amount2.Split('.')[1].ToString() != "000" ? total_amount2 : total_amount2.Split('.')[0].ToString());
                        }
                        dtBill_Info_Copy.Rows.Add(i.ToString(), item.Trim(), gm.removePointsZero(gm.removePoints(d["unit_cost"].ToString().Split('.')[1].ToString() != "000" ? d["unit_cost"].ToString() : d["unit_cost"].ToString().Split('.')[0].ToString())), gm.removePointsZero(gm.removePoints(d["qty"].ToString().Split('.')[1].ToString() != "000" ? d["qty"].ToString() : d["qty"].ToString().Split('.')[0].ToString())), gm.removePointsZero(gm.removePoints(total_amount2)), unit);
                        i++;
                    }
                    if (total_mazdori1.Contains('.'))
                    {
                        total_mazdori1 = (total_mazdori1.Split('.')[1].ToString() != "000" ? total_mazdori1 : total_mazdori1.Split('.')[0].ToString());
                    }
                    if (total_mazdori1 != "" && total_mazdori1 != "0")
                    {
                        dtBill_Info_Copy.Rows.Add("", "مزدوری", "", "", total_mazdori1, "");
                    }

            //dtBill_Info_Copy.Columns.Add("Sr_No");
            //dtBill_Info_Copy.Columns.Add("Item");
            //dtBill_Info_Copy.Columns.Add("Unit_Cost");
            //dtBill_Info_Copy.Columns.Add("Qty");
            //dtBill_Info_Copy.Columns.Add("Total");
            //dtBill_Info_Copy.Columns.Add("Unit");

                }
                e.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dtBill_Info_Copy));
            }
            catch { }
        }

        void localReport_SubreportProcessing2(object sender, Microsoft.Reporting.WinForms.SubreportProcessingEventArgs e)
        {
            var ID = e.Parameters[0].Values[0].ToString();
            if (dtSpaces.Rows.Count > 0)
                dtSpaces.Rows.Clear();
            int remaining = 0;
            int rows = 18;
            if (dtBill_Info_Copy.Rows.Count < rows)
            {
                remaining = rows - dtBill_Info_Copy.Rows.Count;
            }
            else if (dtBill_Info_Copy.Rows.Count > rows)
            {
                remaining = dtBill_Info_Copy.Rows.Count % rows;
            }
            for (int i = 0; i < remaining; i++)
            {
                dtSpaces.Rows.Add("");
            }
            e.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet2", dtSpaces));
        }

        void localReport_SubreportProcessing3(object sender, Microsoft.Reporting.WinForms.SubreportProcessingEventArgs e)
        {
            string total_weight = "0";
            string total_amount = "0";
            string weight_in_mun = "0";
            var ID = e.Parameters[0].Values[0].ToString();
            string query = "select total_amount from bill where status!='-1' and id='" + ID + "'";
            DataTable dt = gm.GetTable(query);
            total_amount = dt.Rows[0][0].ToString();

            query = "select * from bill_details where bill_id='"+ID+"' and unit_cost!=0";// in (select id from bill where status!='-1' and bill_id='" + ID + "')";
            dt = gm.GetTable(query);
            foreach (DataRow d in dt.Rows)
            {
                query = "select weight from items where id ='" + d["item_id"].ToString() + "'";
                DataTable dt2 = gm.GetTable(query);
                try
                {
                    if (d["item_type"].ToString() == "نارمل" || d["item_type"].ToString() == "بل میں")
                    {
                        total_weight = (double.Parse(total_weight) + (double.Parse(dt2.Rows[0][0].ToString()) * double.Parse(d["Qty"].ToString()))).ToString();
                    }
                }
                catch { }
            }


            try
            {
                weight_in_mun = ((double.Parse(total_weight)) / 40).ToString();
            }
            catch { }
            if (dtBill_Info_Total.Rows.Count > 0)
                dtBill_Info_Total.Rows.Clear();
            if (total_amount.Contains('.'))
            {
                total_amount = (total_amount.Split('.')[1].ToString() != "000" ? total_amount : total_amount.Split('.')[0].ToString());
            }
            if (weight_in_mun.Contains('.'))
            {
                weight_in_mun = (weight_in_mun.Split('.')[1].ToString() != "000" ? weight_in_mun : weight_in_mun.Split('.')[0].ToString());
            }
            if (total_weight.Contains('.'))
            {
                total_weight = (total_weight.ToString().Split('.')[1].ToString() != "000" ? total_weight.ToString() : total_weight.ToString().Split('.')[0].ToString());
            }
            dtBill_Info_Total.Rows.Add(total_amount, total_weight,weight_in_mun);
            e.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet3", dtBill_Info_Total));
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
