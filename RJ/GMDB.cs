using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace RJ
{
    class GMDB
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public int ExecuteNonQuery(string query)
        {
            try
            {
                if (con.State.ToString() == "Closed")
                    con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }

        public string removePoints(string value)
        {
            try
            {
                if (value.ToString().Trim().Contains('.'))
                {
                    try
                    {
                        int points = value.ToString().Trim().Split('.')[1].ToString().Length;
                        if ((points==3&&value.ToString().Trim().Split('.')[1].ToString().Substring(0, 3) == "000") || (points==2 && value.ToString().Trim().Split('.')[1].ToString().Substring(0, 2) == "00") || (points==1 && value.ToString().Trim().Split('.')[1].ToString().Substring(0, 1) == "0"))
                        {
                            value = value.ToString().Trim().Split('.')[0].ToString();
                            return value;
                        }
                        return value;
                    }
                    catch
                    {
                        return value;
                    }
                }
                return value;
            }
            catch
            {
                return value;
            }
        }

        public string removePointsZero(string value)
        {
            try
            {
                if (value.ToString().Trim().Contains('.'))
                {
                    try
                    {
                        if (value.ToString().Trim().Split('.')[1].ToString().Contains("00"))
                        {
                            value = value.ToString().Trim().Split('.')[0].ToString() +"." + value.ToString().Trim().Split('.')[1].ToString().Replace("00","");
                            return value;
                        }
                        else if (value.ToString().Trim().Split('.')[1].ToString().Length == 3)
                        {
                            value = value.ToString().Trim().Split('.')[0].ToString() + "." + value.ToString().Trim().Split('.')[1].ToString().Substring(0, 2);
                            return value;
                        }
                        //int points = value.ToString().Trim().Split('.')[1].ToString().Length;
                        //if ((points == 3 && value.ToString().Trim().Split('.')[1].ToString().Substring(0, 3) == "000") || (points == 2 && value.ToString().Trim().Split('.')[1].ToString().Substring(0, 2) == "00") || (points == 1 && value.ToString().Trim().Split('.')[1].ToString().Substring(0, 1) == "0"))
                        //{
                        //    value = value.ToString().Trim().Split('.')[0].ToString();
                        //    return value;
                        //}
                        return value;
                    }
                    catch
                    {
                        return value;
                    }
                }
                return value;
            }
            catch
            {
                return value;
            }
        }

        public DataTable GetTable(string query)
        {
            try
            {
                if (con.State.ToString() == "Closed")
                    con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet dd = new DataSet();
                DataTable dt = new DataTable();
                sda.Fill(dd);
                return dd.Tables[0];
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                MessageBox.Show("Please Wait\nData Is Processing");
                return null;
            }
        }

        public void AcceptDouble(object sender, EventArgs e)
        {
            MetroFramework.Controls.MetroTextBox txt = (MetroFramework.Controls.MetroTextBox)sender;
            try
            {
                bool dot = false;
                bool minus = false;
                for (int i = 0; i < txt.Text.Length; i++)
                    if (!char.IsDigit(txt.Text[i]))
                        if (txt.Text[i] == '.' && !dot)
                            dot = true;
                        else if (txt.Text[i] == '-' && !minus)
                            minus = true;
                        else
                        {
                            txt.Text = txt.Text.Remove(i, 1);
                            txt.SelectionStart = txt.Text.Length;
                            txt.SelectionLength = txt.Text.Length;
                        }
                    else
                        try
                        {
                            double.Parse(txt.Text);
                        }
                        catch
                        {
                            txt.Text = txt.Text.Remove(i, 1);
                            txt.SelectionStart = txt.Text.Length;
                            txt.SelectionLength = txt.Text.Length;
                        }
            }
            catch
            { txt.Text = ""; }
        }

        public void PrintItemList(DataTable dt, string report ,string category,string totalqty,string totalinvoice,string totaltrade,string lowstockreport,string till_date)
        {
            ItemsListReportViewer ws = new ItemsListReportViewer();

            string Cashier = RJ.Properties.Settings.Default.loginuser + " (" + RJ.Properties.Settings.Default.loginid + ")";
            ws.Cashier = Cashier;
            ws.Report = report;
            ws.Category = category;
            ws.totalqty = totalqty;
            //ws.totalinvoice= totalinvoice;
            //ws.totaltrade = totaltrade;
            ws.lowstockreport = lowstockreport;
            ws.till_date = till_date;
            ws.dt = dt;
            ws.Show();
        }

        //public void PrintBillItems(DataTable dt, string report, string category, string totalqty, string totalinvoice, string totaltrade, string lowstockreport, string till_date)
        public void PrintBillItems(DataTable dt, string date, string customer)
        {
            BillItemsListReportViewer ws = new BillItemsListReportViewer();

            //string Cashier = RJ.Properties.Settings.Default.loginuser + " (" + RJ.Properties.Settings.Default.loginid + ")";
            //ws.Cashier = Cashier;
            //ws.Report = report;
            //ws.Category = category;
            //ws.totalqty = totalqty;
            ////ws.totalinvoice= totalinvoice;
            ////ws.totaltrade = totaltrade;
            //ws.lowstockreport = lowstockreport;
            //ws.till_date = till_date;
            ws.date = date;
            ws.Customer = customer;
            ws.dt = dt;
            ws.Show();
        }

        public void PrintLedger(string id, string date, DataTable dt, string ledgerof)
        {
            LedgerReportViewer ws = new LedgerReportViewer();

            string Report_Date = date;
            string Cashier = RJ.Properties.Settings.Default.loginuser + " ("+ RJ.Properties.Settings.Default.loginid+")";
            ws.Bill_Date = Report_Date;
            ws.Cashier = Cashier;
            ws.LedgerOf = ledgerof;
            ws.dt = dt;
            double total_dr = 0;
            double total_cr = 0;
            double balance = 0;
            try
            {
                foreach (DataRow d in dt.Rows)
                {
                    try
                    {
                        total_dr += double.Parse(d[2].ToString());
                    }
                    catch { }
                    try
                    {
                        total_cr += double.Parse(d[3].ToString());
                    }
                    catch { }
                    //try
                    //{
                    //    balance += double.Parse(d[4].ToString());
                    //}
                    //catch { }
                }
            }
            catch { }
            ws.Total_Dr = removePoints(total_dr.ToString());
            ws.Total_Cr = removePoints(total_cr.ToString());
            ws.Balance = removePoints(dt.Rows[dt.Rows.Count - 1][4].ToString());// gm.removePoints(balance.ToString());
            //ws.Balance = removePoints(balance.ToString());
            ws.Show();
        }

        public void PrintWholesaleItemsReport(string id)
        {
        }

        public void PrintWholesaleSummaryReport(string id)
        {
        }

        public void printPaymentReceivingList(DataTable dt)
        {
            //Payment_ReceivingListReportViewer ws = new Payment_ReceivingListReportViewer();
            //int rows = dt.Rows.Count;
            //ws.Show();
            //ws.BringToFront();
        }
        public void printPaymentReceivingReceipt(string monthname1, string date1, string report1, string TransactionNumber1, string TransactionDate1, string PaymentToOrReceivingFrom1, string PaymentToOrReceivingFromValue1, string Amount1, string PaymentDueBefore1, string PaymentDueBeforeValue1, string PaymentDueAfter1, string PaymentDueAfterValue1, string CustomerOrVendorId,string CustomerTodayBill)
        {
            string report = report1;
            string monthName = monthname1;
            string date = date1;


            PaymentReceivingReportViewer ws = new PaymentReceivingReportViewer();

            string Cashier = RJ.Properties.Settings.Default.loginuser + " (" + RJ.Properties.Settings.Default.loginid + ")";
            ws.Cashier = Cashier;
            ws.Today_Bill_Amount = CustomerTodayBill;
            ws.Report = report;
            ws.TransactionNumber = TransactionNumber1;
            ws.TransactionDate= TransactionDate1;

            ws.PaymentToOrReceivingFrom = PaymentToOrReceivingFrom1;
            ws.PaymentToOrReceivingFromValue = PaymentToOrReceivingFromValue1;
            ws.Amount = Amount1;
            ws.PaymentDueBefore = PaymentDueBefore1;
            ws.PaymentDueBeforeValue = PaymentDueBeforeValue1;
            ws.PaymentDueAfter = PaymentDueAfter1;
            ws.PaymentDueAfterValue = PaymentDueAfterValue1;
            ws.UniqueId = CustomerOrVendorId;
            ws.Show();
            ws.BringToFront();
        
        }

        public void printBill(string bill_id, string Bill_Type, string Customer_Or_Vendor, string Receipt_Type,string transaction_date)
        {
            try
            {
                BillReportViewer sc = new BillReportViewer();
                sc.Receipt_Type = Receipt_Type;//original,duplicate

                DataTable BillDetails = new DataTable();
//                BillDetails.Columns.Add("Sr_No");
                BillDetails.Columns.Add("Item");
                BillDetails.Columns.Add("Unit_Cost");
                BillDetails.Columns.Add("Qty");
                BillDetails.Columns.Add("Total");
                BillDetails.Columns.Add("Unit");
                string query = "select * from bill_details where bill_id = '" + bill_id + "' and unit_cost!=0";
                DataTable d = GetTable(query);
                int i = 1;
                double qty = 0;
                sc.Bill_Type = Bill_Type;//sale invoice
                sc.Customer_Or_Vendor = Customer_Or_Vendor;
                sc.Transaction_Date = transaction_date;
                double totalweight = 0;
                string total_mazdori1 = "0";
                foreach (DataRow d1 in d.Rows)
                {
                    try
                    {
                        qty += double.Parse(d1["Qty"].ToString());
                    }
                    catch { }
                    string item = "";
                    query = "select * from items where id = '" + d1["item_id"].ToString() + "'";
                    string mazdori1 = "0";
                    DataTable dt2 = GetTable(query);
                    try
                    {
                            string item_type1 = d1["item_type"].ToString();
                            item = dt2.Rows[0]["Name"].ToString() + " - (" + item_type1 + ")";
                            if (item_type1 == "نارمل")
                            {
                                item = dt2.Rows[0]["Name"].ToString();
                                mazdori1 = d1["mazdori"].ToString();
                            }
                    }
                    catch { }
                    string unit = "";
                    query = "select * from items where id = '" + d1["item_id"].ToString() + "'";
                    dt2 = GetTable(query);
                    try
                    {
                        unit = dt2.Rows[0]["Unit_Id"].ToString();
                    }
                    catch { }
                    {
                        query = "select * from items where id = '" + d1["item_id"].ToString() + "'";
                        dt2 = GetTable(query);
                        try
                        {
                            totalweight += (double.Parse(dt2.Rows[0]["weight"].ToString())*(double.Parse(d1["Qty"].ToString())));
                        }
                        catch { }
                    }
                    query = "select * from Units where id = '" + unit + "'";
                    dt2 = GetTable(query);
                    try
                    {
                        unit = dt2.Rows[0]["Unit_Name"].ToString();
                    }
                    catch { }
                    string unit_cost2 = d1["unit_cost"].ToString();
                    if (unit_cost2.Contains('.'))
                    {
                        unit_cost2 = (unit_cost2.Split('.')[1].ToString() != "000" ? unit_cost2 : unit_cost2.Split('.')[0].ToString());
                    }
                    string qty2 = d1["qty"].ToString();
                    if (qty2.Contains('.'))
                    {
                        qty2 = (qty2.Split('.')[1].ToString() != "000" ? qty2 : qty2.Split('.')[0].ToString());
                    }
                    string total_amount2 = d1["Total_Amount"].ToString();
                    try
                    {
                        total_amount2 = ((Math.Round((double.Parse(total_amount2)), 0, MidpointRounding.AwayFromZero))).ToString();
                    }
                    catch {}
                    if (total_amount2.Contains('.'))
                    {
                        total_amount2 = (total_amount2.Split('.')[1].ToString() != "000" ? total_amount2 : total_amount2.Split('.')[0].ToString());
                    }
                    try
                    {
                        total_mazdori1 = (Math.Round((double.Parse(total_mazdori1) + (double.Parse(mazdori1) * double.Parse(qty2))), 0, MidpointRounding.AwayFromZero)).ToString();
                    }
                    catch { }
                    try
                    {
                        total_amount2 = (Math.Round((double.Parse(total_amount2) - (double.Parse(mazdori1) * double.Parse(qty2))), 0, MidpointRounding.AwayFromZero)).ToString();
                    }
                    catch { }
                    BillDetails.Rows.Add(item.Trim(), removePointsZero(removePoints(unit_cost2)), removePointsZero(removePoints(qty2)), removePointsZero(removePoints(total_amount2)), unit);
                    i++;
                }
                if (total_mazdori1 != "" && total_mazdori1 != "0")
                {
                    BillDetails.Rows.Add("مزدوری", "", "", total_mazdori1, "");
                }
                sc.dt = BillDetails;
                sc.totalqty = qty.ToString();
                sc.TotalWeight = totalweight.ToString();

                query = "select * from bill where status!='-1' and id = '" + bill_id + "'";
                d = GetTable(query);
                foreach (DataRow d1 in d.Rows)
                {
                    try
                    {
                        sc.transactionid = d1["bill_id"].ToString();
                        if (d1["Net_Total_Amount"].ToString().Contains('.'))
                        {
                            sc.Net_Total = (d1["Net_Total_Amount"].ToString().Split('.')[1].ToString() != "000" ? d1["Net_Total_Amount"].ToString() : d1["Net_Total_Amount"].ToString().Split('.')[0].ToString());
                        }
                        else
                        {
                            sc.Net_Total = d1["Net_Total_Amount"].ToString();
                        }
                        sc.Discount_Amount = d1["Discount_Amount"].ToString();
                        sc.Tax_Amount = d1["Tax_Amount"].ToString() + "\n(" + d1["Tax_Type"].ToString() + ")";//tax amount (gst@10) 280 (gst@10)
                        sc.Service_Charges = d1["Service_Charges"].ToString();
                        if (d1["Total_Amount"].ToString().Contains('.'))
                        {
                            sc.Total_Amount = (d1["Total_Amount"].ToString().Split('.')[1].ToString() != "000" ? d1["Total_Amount"].ToString() : d1["Total_Amount"].ToString().Split('.')[0].ToString());
                        }
                        else
                        {
                            sc.Total_Amount = d1["Total_Amount"].ToString();
                        }
                        if (d1["Customer_Vendor_Id"].ToString().Trim() == "")
                        {
                            if(Customer_Or_Vendor=="Vendor")
                                sc.Customer = "Unregistered Vendor";
                            else if (Customer_Or_Vendor == "Customer")
                                sc.Customer = "walk-in customer";
                            else
                                sc.Customer = "---";
                        }
                        else
                        {
                            string customer = "";
                            query = "select * from Customer_Or_Vendor where id = '" + d1["Customer_Vendor_Id"].ToString() + "'";
                            DataTable dt2 = GetTable(query);
                            try
                            {
                                customer = dt2.Rows[0][2].ToString() + " (" + dt2.Rows[0][0].ToString() + ")";
                            }
                            catch { }
                            //sc.Customer = customer;
                            string main_customer = "";
                            try
                            {
                                string[] s = customer.Trim().ToString().Split('(');
                                string customerid = s[s.Length - 1].ToString().Trim().Split(')')[0].ToString();
                                query = "select * from Customer_Or_Vendor where id in ( select parent from Customer_Or_Vendor where id='" + customerid + "')";
                                DataTable d2 = GetTable(query);
                                //main_customer = d2.Rows[0]["customer_Or_Vendor_Name"].ToString().Trim() + " (" + d2.Rows[0]["id"].ToString().Trim() + ")";
                                main_customer = d2.Rows[0]["customer_Or_Vendor_Name"].ToString().Trim();
                                customer = customer.Trim().ToString().Split('(')[0].ToString().Trim();
                            }
                            catch { }
                            sc.Customer = customer;
                            sc.Main_Customer_Or_Vendor = main_customer;
                        }
                        sc.Paid_Amount = d1["Paid_Amount"].ToString();
                        double change = 0;
                        try
                        {
                            change = (double.Parse(d1["Paid_Amount"].ToString().Trim()) - double.Parse(d1["Total_Amount"].ToString().Trim()));
                        }
                        catch { }
                        sc.Change = change.ToString();

                        string cashier = "";
                        query = "select * from login where id = '" + d1["AddedBy_UserId"].ToString() + "'";
                        DataTable d3 = GetTable(query);
                        try
                        {
                            cashier = d3.Rows[0][1].ToString() + " (" + d3.Rows[0][0].ToString() + ")";
                        }
                        catch { }
                        sc.Cashier = cashier;
                    }
                    catch { }
                }

                sc.Show();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        public void print_Customer_All_Bills(string Bill_Type, string Customer_Or_Vendor, string Receipt_Type, string transaction_date,string main_Customer_Id)
        {
            try
            {
                Customer_All_Bills_ReportViewer sc = new Customer_All_Bills_ReportViewer();
                sc.Receipt_Type = "All Bills";//original,duplicate

                string query = "";
                DataTable dt_Clients_Info = new DataTable();
                dt_Clients_Info.Columns.Add("Transaction_Id");
                dt_Clients_Info.Columns.Add("Transaction_Date");
                dt_Clients_Info.Columns.Add("Cashier");
                dt_Clients_Info.Columns.Add("Main_Customer");
                dt_Clients_Info.Columns.Add("Customer_Or_Vendor");
                dt_Clients_Info.Columns.Add("Customer");
                dt_Clients_Info.Columns.Add("Id");

                string todaydate = DateTime.Now.Date.Year.ToString() +"-"+DateTime.Now.Date.Month.ToString() +"-"+DateTime.Now.Date.Day.ToString();
                query = "select * from bill where bill_date='" + todaydate + "' and status='1' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='" + main_Customer_Id + "' and status!='-1') order by Bill_Id";
                //query = "select * from Customer_Or_Vendor where parent='" + main_Customer_Id + "'";
                //dt_Clients_Info = GetTable(query);
                DataTable dttemp = GetTable(query);
                if (dttemp.Rows.Count <= 0)
                {
                    MessageBox.Show("Bill Not Found For Current Date");
                    return;
                }

                DataTable d2 = new DataTable();
                string main_customer = "";
                query = "select * from Customer_Or_Vendor where id = '" + main_Customer_Id + "'";
                d2 = GetTable(query);
                try
                {
                    //main_customer = d2.Rows[0][2].ToString() + " (" + d2.Rows[0][0].ToString() + ")";
                    main_customer = d2.Rows[0][2].ToString();
                }
                catch { }

                foreach (DataRow d_temp in dttemp.Rows)
                {
                    string cashier = "";
                    query = "select * from login where id = '" + d_temp["AddedBy_UserId"].ToString() + "'";
                    d2 = GetTable(query);
                    try
                    {
                        cashier = d2.Rows[0][1].ToString() + " (" + d2.Rows[0][0].ToString() + ")";
                    }
                    catch { }

                    string customer_or_vendor = "Customer";
                    string customer = "";
                    query = "select * from Customer_Or_Vendor where id = '" + d_temp["Customer_Vendor_Id"].ToString() + "'";
                    d2 = GetTable(query);
                    try
                    {
                        //customer = d2.Rows[0][2].ToString() + " (" + d2.Rows[0][0].ToString() + ")";
                        customer = d2.Rows[0][2].ToString();
                    }
                    catch { }

                    dt_Clients_Info.Rows.Add(d_temp["Bill_Id"].ToString(), transaction_date, cashier, main_customer, customer_or_vendor, customer, d_temp["Id"].ToString());
                }

                //dt_Clients_Info.Rows.Add("1", "1", "1", "1", "1", "1");
                //dt_Clients_Info.Rows.Add("2", "2", "2", "2", "2", "2");
                sc.dt_Clients_Info = dt_Clients_Info;

                DataTable BillDetails = new DataTable();
                BillDetails.Columns.Add("Sr_No");
                BillDetails.Columns.Add("Item");
                BillDetails.Columns.Add("Unit_Cost");
                BillDetails.Columns.Add("Qty");
                BillDetails.Columns.Add("Total");
                BillDetails.Columns.Add("Unit");
                query = "select * from bill_details where Bill_Id = '" + "1" + "' and unit_cost!=0";
                DataTable d = GetTable(query);
                int i = 1;
                double qty = 0;
                sc.Bill_Type = Bill_Type;//sale invoice
                sc.Customer_Or_Vendor = Customer_Or_Vendor;
                sc.Transaction_Date = transaction_date;
                double totalweight = 0;
                foreach (DataRow d1 in d.Rows)
                {
                    string item = "";
                    query = "select * from items where id = '" + d1["item_id"].ToString() + "'";
                    DataTable dt2 = GetTable(query);
                    try
                    {
                        item = dt2.Rows[0]["Name"].ToString();
                    }
                    catch { }
                    string unit = "";
                    query = "select * from items where id = '" + d1["item_id"].ToString() + "'";
                    dt2 = GetTable(query);
                    try
                    {
                        unit = dt2.Rows[0]["Unit_Id"].ToString();
                    }
                    catch { }
                    if (d1["item_type"].ToString() == "")
                    {
                        query = "select * from items where id = '" + d1["item_id"].ToString() + "'";
                        dt2 = GetTable(query);
                        try
                        {
                            totalweight += (double.Parse(dt2.Rows[0]["weight"].ToString())*qty);
                        }
                        catch { }
                    }
                    query = "select * from Units where id = '" + unit + "'";
                    dt2 = GetTable(query);
                    try
                    {
                        unit = dt2.Rows[0]["Unit_Name"].ToString();
                    }
                    catch { }
                    try
                    {
                        qty += double.Parse(d1["qty"].ToString().Trim().ToString());
                    }
                    catch { }
                    string total_amount = "0";
                    try
                    {
                        total_amount = ((Math.Round(double.Parse(d1["Total_Amount"].ToString().Trim().ToString()), 0, MidpointRounding.AwayFromZero))).ToString();
                        //double totalofitem = (Math.Round((qty * (ratePerUnit + mazdori)), 0, MidpointRounding.AwayFromZero));
                    }
                    catch { }
                    BillDetails.Rows.Add(i.ToString(), item.Trim(), d1["unit_cost"].ToString(), d1["qty"].ToString(), total_amount, unit);
                    i++;
                }
                sc.dt = BillDetails;
                sc.totalqty = qty.ToString();
                sc.TotalWeight = totalweight.ToString();

                query = "select * from bill where status!='-1' and id = '" + "1" + "'";
                d = GetTable(query);
                foreach (DataRow d1 in d.Rows)
                {
                    try
                    {
                        sc.transactionid = d1["bill_id"].ToString();
                        if (d1["Net_Total_Amount"].ToString().Contains('.'))
                        {
                            sc.Net_Total = (d1["Net_Total_Amount"].ToString().Split('.')[1].ToString() != "000" ? d1["Net_Total_Amount"].ToString() : d1["Net_Total_Amount"].ToString().Split('.')[0].ToString());
                        }
                        else
                        {
                            sc.Net_Total = d1["Net_Total_Amount"].ToString();
                        }
                        sc.Discount_Amount = d1["Discount_Amount"].ToString();
                        sc.Tax_Amount = d1["Tax_Amount"].ToString() + "\n(" + d1["Tax_Type"].ToString() + ")";//tax amount (gst@10) 280 (gst@10)
                        sc.Service_Charges = d1["Service_Charges"].ToString();
                        if (d1["Total_Amount"].ToString().Contains('.'))
                        {
                            sc.Total_Amount = (d1["Total_Amount"].ToString().Split('.')[1].ToString() != "000" ? d1["Total_Amount"].ToString() : d1["Total_Amount"].ToString().Split('.')[0].ToString());
                        }
                        else
                        {
                            sc.Total_Amount = d1["Total_Amount"].ToString();
                        }
                        if (d1["Customer_Vendor_Id"].ToString().Trim() == "")
                        {
                            if(Customer_Or_Vendor=="Vendor")
                                sc.Customer = "Unregistered Vendor";
                            else if (Customer_Or_Vendor == "Customer")
                                sc.Customer = "walk-in customer";
                            else
                                sc.Customer = "---";
                        }
                        else
                        {
                            string customer = "";
                            query = "select * from Customer_Or_Vendor where id = '" + d1["Customer_Vendor_Id"].ToString() + "'";
                            DataTable dt2 = GetTable(query);
                            try
                            {
                                customer = dt2.Rows[0][2].ToString() + " (" + dt2.Rows[0][0].ToString() + ")";
                            }
                            catch { }
                            sc.Customer = customer;
                            main_customer = "";
                            try
                            {
                                string[] s = customer.Trim().ToString().Split('(');
                                string customerid = s[s.Length - 1].ToString().Trim().Split(')')[0].ToString();
                                query = "select * from Customer_Or_Vendor where id in ( select parent from Customer_Or_Vendor where id='" + customerid + "')";
                                d2 = GetTable(query);
                                main_customer = d2.Rows[0]["customer_Or_Vendor_Name"].ToString().Trim() + " (" + d2.Rows[0]["id"].ToString().Trim() + ")";
                            }
                            catch { }
                            sc.Main_Customer_Or_Vendor = main_customer;
                        }
                        sc.Paid_Amount = d1["Paid_Amount"].ToString();
                        double change = 0;
                        try
                        {
                            change = (double.Parse(d1["Paid_Amount"].ToString().Trim()) - double.Parse(d1["Total_Amount"].ToString().Trim()));
                        }
                        catch { }
                        sc.Change = change.ToString();

                        string cashier = "";
                        query = "select * from login where id = '" + d1["AddedBy_UserId"].ToString() + "'";
                        DataTable d3 = GetTable(query);
                        try
                        {
                            cashier = d3.Rows[0][1].ToString() + " (" + d3.Rows[0][0].ToString() + ")";
                        }
                        catch { }
                        sc.Cashier = cashier;
                    }
                    catch { }
                }

                sc.Show();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        public void print_1Customer_All_Bills(string Bill_Type, string Customer_Or_Vendor, string Receipt_Type, string transaction_date, string main_Customer_Id,string id)
        {
            try
            {
                Customer_All_Bills_ReportViewer sc = new Customer_All_Bills_ReportViewer();
                sc.Receipt_Type = "All Bills";//original,duplicate

                string query = "";
                DataTable dt_Clients_Info = new DataTable();
                dt_Clients_Info.Columns.Add("Transaction_Id");
                dt_Clients_Info.Columns.Add("Transaction_Date");
                dt_Clients_Info.Columns.Add("Cashier");
                dt_Clients_Info.Columns.Add("Main_Customer");
                dt_Clients_Info.Columns.Add("Customer_Or_Vendor");
                dt_Clients_Info.Columns.Add("Customer");
                dt_Clients_Info.Columns.Add("Id");

                string todaydate = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
                query = "select * from bill where bill_date='" + todaydate + "' and status='1' and id='"+id+"' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='" + main_Customer_Id + "' and status!='-1') order by Bill_Id";
                //query = "select * from Customer_Or_Vendor where parent='" + main_Customer_Id + "'";
                //dt_Clients_Info = GetTable(query);
                DataTable dttemp = GetTable(query);
                if (dttemp.Rows.Count <= 0)
                {
                    MessageBox.Show("Bill Not Found For Current Date");
                    return;
                }

                DataTable d2 = new DataTable();
                string main_customer = "";
                query = "select * from Customer_Or_Vendor where id = '" + main_Customer_Id + "'";
                d2 = GetTable(query);
                try
                {
                    //main_customer = d2.Rows[0][2].ToString() + " (" + d2.Rows[0][0].ToString() + ")";
                    main_customer = d2.Rows[0][2].ToString();
                }
                catch { }

                foreach (DataRow d_temp in dttemp.Rows)
                {
                    string cashier = "";
                    query = "select * from login where id = '" + d_temp["AddedBy_UserId"].ToString() + "'";
                    d2 = GetTable(query);
                    try
                    {
                        cashier = d2.Rows[0][1].ToString() + " (" + d2.Rows[0][0].ToString() + ")";
                    }
                    catch { }

                    string customer_or_vendor = "Customer";
                    string customer = "";
                    query = "select * from Customer_Or_Vendor where id = '" + d_temp["Customer_Vendor_Id"].ToString() + "'";
                    d2 = GetTable(query);
                    try
                    {
                        //customer = d2.Rows[0][2].ToString() + " (" + d2.Rows[0][0].ToString() + ")";
                        customer = d2.Rows[0][2].ToString();
                    }
                    catch { }

                    dt_Clients_Info.Rows.Add(d_temp["Bill_Id"].ToString(), transaction_date, cashier, main_customer, customer_or_vendor, customer, d_temp["Id"].ToString());
                }

                //dt_Clients_Info.Rows.Add("1", "1", "1", "1", "1", "1");
                //dt_Clients_Info.Rows.Add("2", "2", "2", "2", "2", "2");
                sc.dt_Clients_Info = dt_Clients_Info;

                DataTable BillDetails = new DataTable();
                BillDetails.Columns.Add("Sr_No");
                BillDetails.Columns.Add("Item");
                BillDetails.Columns.Add("Unit_Cost");
                BillDetails.Columns.Add("Qty");
                BillDetails.Columns.Add("Total");
                BillDetails.Columns.Add("Unit");
                query = "select * from bill_details where Bill_Id = '" + "1" + "' and unit_cost!=0";
                DataTable d = GetTable(query);
                int i = 1;
                double qty = 0;
                sc.Bill_Type = Bill_Type;//sale invoice
                sc.Customer_Or_Vendor = Customer_Or_Vendor;
                sc.Transaction_Date = transaction_date;
                double totalweight = 0;
                foreach (DataRow d1 in d.Rows)
                {
                    string item = "";
                    query = "select * from items where id = '" + d1["item_id"].ToString() + "'";
                    DataTable dt2 = GetTable(query);
                    try
                    {
                        item = dt2.Rows[0]["Name"].ToString();
                    }
                    catch { }
                    string unit = "";
                    query = "select * from items where id = '" + d1["item_id"].ToString() + "'";
                    dt2 = GetTable(query);
                    try
                    {
                        unit = dt2.Rows[0]["Unit_Id"].ToString();
                    }
                    catch { }
                    if (d1["item_type"].ToString() == "")
                    {
                        query = "select * from items where id = '" + d1["item_id"].ToString() + "'";
                        dt2 = GetTable(query);
                        try
                        {
                            totalweight += (double.Parse(dt2.Rows[0]["weight"].ToString()) * qty);
                        }
                        catch { }
                    }
                    query = "select * from Units where id = '" + unit + "'";
                    dt2 = GetTable(query);
                    try
                    {
                        unit = dt2.Rows[0]["Unit_Name"].ToString();
                    }
                    catch { }
                    try
                    {
                        qty += double.Parse(d1["qty"].ToString().Trim().ToString());
                    }
                    catch { }
                    string total_amount = "0";
                    try
                    {
                        total_amount = ((Math.Round(double.Parse(d1["Total_Amount"].ToString().Trim().ToString()), 0, MidpointRounding.AwayFromZero))).ToString();
                        //double totalofitem = (Math.Round((qty * (ratePerUnit + mazdori)), 0, MidpointRounding.AwayFromZero));
                    }
                    catch { }
                    BillDetails.Rows.Add(i.ToString(), item.Trim(), d1["unit_cost"].ToString(), d1["qty"].ToString(), total_amount, unit);
                    i++;
                }
                sc.dt = BillDetails;
                sc.totalqty = qty.ToString();
                sc.TotalWeight = totalweight.ToString();

                query = "select * from bill where status!='-1' and id = '" + "1" + "'";
                d = GetTable(query);
                foreach (DataRow d1 in d.Rows)
                {
                    try
                    {
                        sc.transactionid = d1["bill_id"].ToString();
                        if (d1["Net_Total_Amount"].ToString().Contains('.'))
                        {
                            sc.Net_Total = (d1["Net_Total_Amount"].ToString().Split('.')[1].ToString() != "000" ? d1["Net_Total_Amount"].ToString() : d1["Net_Total_Amount"].ToString().Split('.')[0].ToString());
                        }
                        else
                        {
                            sc.Net_Total = d1["Net_Total_Amount"].ToString();
                        }
                        sc.Discount_Amount = d1["Discount_Amount"].ToString();
                        sc.Tax_Amount = d1["Tax_Amount"].ToString() + "\n(" + d1["Tax_Type"].ToString() + ")";//tax amount (gst@10) 280 (gst@10)
                        sc.Service_Charges = d1["Service_Charges"].ToString();
                        if (d1["Total_Amount"].ToString().Contains('.'))
                        {
                            sc.Total_Amount = (d1["Total_Amount"].ToString().Split('.')[1].ToString() != "000" ? d1["Total_Amount"].ToString() : d1["Total_Amount"].ToString().Split('.')[0].ToString());
                        }
                        else
                        {
                            sc.Total_Amount = d1["Total_Amount"].ToString();
                        }
                        if (d1["Customer_Vendor_Id"].ToString().Trim() == "")
                        {
                            if (Customer_Or_Vendor == "Vendor")
                                sc.Customer = "Unregistered Vendor";
                            else if (Customer_Or_Vendor == "Customer")
                                sc.Customer = "walk-in customer";
                            else
                                sc.Customer = "---";
                        }
                        else
                        {
                            string customer = "";
                            query = "select * from Customer_Or_Vendor where id = '" + d1["Customer_Vendor_Id"].ToString() + "'";
                            DataTable dt2 = GetTable(query);
                            try
                            {
                                customer = dt2.Rows[0][2].ToString() + " (" + dt2.Rows[0][0].ToString() + ")";
                            }
                            catch { }
                            sc.Customer = customer;
                            main_customer = "";
                            try
                            {
                                string[] s = customer.Trim().ToString().Split('(');
                                string customerid = s[s.Length - 1].ToString().Trim().Split(')')[0].ToString();
                                query = "select * from Customer_Or_Vendor where id in ( select parent from Customer_Or_Vendor where id='" + customerid + "')";
                                d2 = GetTable(query);
                                main_customer = d2.Rows[0]["customer_Or_Vendor_Name"].ToString().Trim() + " (" + d2.Rows[0]["id"].ToString().Trim() + ")";
                            }
                            catch { }
                            sc.Main_Customer_Or_Vendor = main_customer;
                        }
                        sc.Paid_Amount = d1["Paid_Amount"].ToString();
                        double change = 0;
                        try
                        {
                            change = (double.Parse(d1["Paid_Amount"].ToString().Trim()) - double.Parse(d1["Total_Amount"].ToString().Trim()));
                        }
                        catch { }
                        sc.Change = change.ToString();

                        string cashier = "";
                        query = "select * from login where id = '" + d1["AddedBy_UserId"].ToString() + "'";
                        DataTable d3 = GetTable(query);
                        try
                        {
                            cashier = d3.Rows[0][1].ToString() + " (" + d3.Rows[0][0].ToString() + ")";
                        }
                        catch { }
                        sc.Cashier = cashier;
                    }
                    catch { }
                }

                sc.Show();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        public string MaxId(string query)
        {
            try
            {
                if (con.State.ToString() == "Closed")
                    con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                string id = "";
                foreach (DataRow d in dt.Rows)
                {
                    id = d[0].ToString();
                }
                if (id.Trim() == "")
                    id = "0";
                id = (int.Parse(id) + 1).ToString();
                return id;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public int ExecuteSQLWithImages(string SQLString, Dictionary<string, byte[]> imageBytes)
        {
            try
            {
                if (con.State.ToString() == "Closed")
                    con.Open();

                SqlCommand cmd = new SqlCommand();
                foreach (string imageName in imageBytes.Keys)
                {
                    System.Data.SqlClient.SqlParameter sqlParameter = new System.Data.SqlClient.SqlParameter(imageName, SqlDbType.VarBinary);
                    sqlParameter.Value = (object)imageBytes[imageName] ?? DBNull.Value;
                    cmd.Parameters.Add(sqlParameter);
                }
                cmd.Connection = con;
                cmd.CommandText = SQLString;
                return cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }
    }
}
