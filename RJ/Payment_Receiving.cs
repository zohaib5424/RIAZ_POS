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
using System.Globalization;

namespace RJ
{
    public partial class Payment_Receiving : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public Payment_Receiving(string PaymentOrReceiving1)
        {
            PaymentOrReceiving = PaymentOrReceiving1;
            InitializeComponent();
        }

        AutoCompleteStringCollection source = new AutoCompleteStringCollection();
        AutoCompleteStringCollection sourceCustomer_Or_Vendor = new AutoCompleteStringCollection();
        AutoCompleteStringCollection sourceitemname = new AutoCompleteStringCollection();


        private void CellAcceptDouble(object sender, EventArgs e)
        {
            DataGridViewTextBoxCell txt = (DataGridViewTextBoxCell)sender;
            try
            {
                bool dot = false;
                bool minus = false;
                for (int i = 0; i < txt.Value.ToString().Length; i++)
                    if (!char.IsDigit(txt.Value.ToString()[i]))
                        if (txt.Value.ToString()[i] == '.' && !dot)
                            dot = true;
                        else if (txt.Value.ToString()[i] == '-' && !minus)
                            minus = true;
                        else
                        {
                            txt.Value = "0";
                        }
                    else
                        try
                        {
                            double.Parse(txt.Value.ToString());
                        }
                        catch
                        {
                            txt.Value = "0";
                        }

            }
            catch
            { txt.Value = "0"; }
        }

        string PaymentOrReceiving = "";
        public void getTransactionId()
        {
            try
            {
                int count = 0;
                string billdate = metroDateTime1.Value.Date.Year.ToString() + "-" + metroDateTime1.Value.Date.Month.ToString() + "-" + metroDateTime1.Value.Date.Day.ToString();
                string query = "select count(*) from bill where status!='-1' and bill_date='" + billdate + "'";
                DataTable dt1 = gm.GetTable(query);
                try
                {
                    string c = dt1.Rows[0][0].ToString();
                    count = int.Parse(c.Trim());
                }
                catch { }
                count++;
                string billid = "";
                if (PaymentOrReceiving == "Payment")
                {
                    billid += "PAY" + metroDateTime1.Value.Date.Day.ToString() + metroDateTime1.Value.Date.Month.ToString() + metroDateTime1.Value.Date.Year.ToString() + "XX";
                    metroLabel4.Text = "Payment";
                    metroLabel8.Text = "Select Payment To Vendor / Customer";
                    metroLabel21.Text = "Payment Information";
                    label1.Text = "F1 - Enter Vendor Name";
                }
                else if (PaymentOrReceiving == "Receiving")
                {
                    billid += "REC" + metroDateTime1.Value.Date.Day.ToString() + metroDateTime1.Value.Date.Month.ToString() + metroDateTime1.Value.Date.Year.ToString() + "XX";
                    metroLabel4.Text = "Receiving";
                    metroLabel8.Text = "Select Receiving From Vendor / Customer";
                    metroLabel21.Text = "Receiving Information";
                    label1.Text = "F1 - Enter Customer Name";
                }
                metroLabelTransactionId.Text = billid;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ItemsList_Load(object sender, EventArgs e)
        {
            //metroDateTime1.Enabled = false;
            try
            {
                metroLabel8.Visible = false;
                metroRadioButtonCustomer.Visible = false;
                metroRadioButtonVendor.Visible = false;

                metroTextBoxVendorContactNumber.ReadOnly = true;
                metroTextBoxVendorAddress.ReadOnly = true;

                metroTextBoxCustomerContactNumber.ReadOnly = true;
                metroTextBoxCustomerAddress.ReadOnly = true;

                metroTextBoxVendorContactNumber.ReadOnly = true;
                metroTextBoxVendorAddress.ReadOnly = true;
                metroTextBoxCustomerContactNumber.ReadOnly = true;
                metroTextBoxCustomerAddress.ReadOnly = true;

                source = new AutoCompleteStringCollection();
                sourceCustomer_Or_Vendor = new AutoCompleteStringCollection();
                this.AutoScroll = true;

                metroComboBoxPaymentMethod.Items.Add("Cash");
                metroComboBoxPaymentMethod.Items.Add("Card");
                metroComboBoxPaymentMethod.Items.Add("Cheque");
                metroComboBoxPaymentMethod.Items.Add("Bank Transfer");
                metroComboBoxPaymentMethod.Items.Add("Other");

                metroComboBoxPaymentMethod.SelectedIndex = 0;

                metroComboBoxCardType.Items.Add("Visa");
                metroComboBoxCardType.Items.Add("MasterCard");

                metroComboBoxExpiryMonth.Items.Add("01");
                metroComboBoxExpiryMonth.Items.Add("02");
                metroComboBoxExpiryMonth.Items.Add("03");
                metroComboBoxExpiryMonth.Items.Add("04");
                metroComboBoxExpiryMonth.Items.Add("05");
                metroComboBoxExpiryMonth.Items.Add("06");
                metroComboBoxExpiryMonth.Items.Add("07");
                metroComboBoxExpiryMonth.Items.Add("08");
                metroComboBoxExpiryMonth.Items.Add("09");
                metroComboBoxExpiryMonth.Items.Add("10");
                metroComboBoxExpiryMonth.Items.Add("11");
                metroComboBoxExpiryMonth.Items.Add("12");

                int year = DateTime.Now.Year;
                for (int i1 = 0; i1 < 20; i1++)
                {
                    metroComboBoxExpiryYear.Items.Add(year.ToString());
                    year += 1;
                }

                DataTable dt = gm.GetTable("select * from customer_or_vendor where status ='1' and customer_vendor_type='Vendor'");
                foreach (DataRow d in dt.Rows)
                {
                    sourceitemname.Add(d["customer_Or_Vendor_Name"].ToString() + " (" + d["id"].ToString() + ")");
                }

                comboBox1.DisplayMember = "customer_Or_Vendor_Name";
                comboBox1.ValueMember = "id";
                comboBox1.DataSource = dt;
                comboBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
                comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;

                dt = gm.GetTable("select * from customer_or_vendor where status ='1' and customer_vendor_type='Customer' and parent=''");
                foreach (DataRow d in dt.Rows)
                {
                    sourceCustomer_Or_Vendor.Add(d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")");
                    //metroComboBox1.Items.Add(d["customer_Or_Vendor_Name"].ToString() + " (" + d["id"].ToString() + ")");
                }

                //metroTextBoxCustomerName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                //metroTextBoxCustomerName.AutoCompleteCustomSource = sourceCustomer_Or_Vendor;
                //metroTextBoxCustomerName.AutoCompleteSource = AutoCompleteSource.CustomSource;

                comboBox1.DisplayMember = "customer_Or_Vendor_Name";
                comboBox1.ValueMember = "id";
                comboBox1.DataSource = dt;
                comboBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
                comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
                //comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                
                getTransactionId();

                try
                {
                    metroComboBoxPaymentMethod.SelectedIndex = 0;
                }
                catch { }
                metroRadioButtonCustomer.Checked = true;

            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        GMDB gm = new GMDB();

        public void calculatePaymentDue()
        {
            try 
            {
                //labelPaymentDue.Text = "0";
                //string[] s = (metroRadioButtonVendor.Checked == true ? comboBox1.Text.Trim().Split('(') : metroTextBoxCustomerName.Text.Trim().Split('('));
                //string[] a = s[s.Length - 1].Trim().Split(')');
                //string id = a[0].ToString();
                string id = comboBox1.SelectedValue.ToString();
                if (id.Trim() != "")
                {
                    string date1 = metroDateTime1.Value.Date.Year.ToString() + "-" + metroDateTime1.Value.Date.Month.ToString() + "-" + metroDateTime1.Value.Date.Day.ToString();
                    //string query = "Select sum(balance) from bill where status!='-1' and customer_vendor_id in ( select id from customer_or_vendor where parent='" + id + "') or customer_vendor_id='" + id + "'";
                    string query = "Select sum(balance) from bill where status!='-1' and Bill_Date<='"+date1+"' and customer_vendor_id in ( select id from customer_or_vendor where parent='" + id + "') or customer_vendor_id='" + id + "'";
                    DataTable dt2 = gm.GetTable(query);
                    if (dt2.Rows.Count > 0)
                    {
                        try
                        {
                            double.Parse(dt2.Rows[0][0].ToString().Trim());
                            labelPaymentDue.Text = dt2.Rows[0][0].ToString().Trim();
                        }
                        catch
                        {
                            labelPaymentDue.Text = "0";
                        }
                    }
                    else
                    {
                        labelPaymentDue.Text = "0";
                    }
                }
            }
            catch
            {
                labelPaymentDue.Text = "0";
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void metroRadioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void itemCategory1_Load(object sender, EventArgs e)
        {

        }

        private void metroTextBoxCategory_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBoxCategory_TextChanged(object sender, EventArgs e)
        {

        }

        private void metroPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void metroRadioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void metroTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBoxPurchasePrice_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBoxDiscount_Click(object sender, EventArgs e)
        {

        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            Taxes b = new Taxes();
            b.FormBorderStyle = FormBorderStyle.None;
            b.ShowDialog();
        }

        private void metroPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void metroComboBoxPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (metroComboBoxPaymentMethod.SelectedItem.ToString() == "Cash")
                {
                    groupBox1.Show();
                    groupBox1.BringToFront();
                    groupBoxCard.Hide();
                    groupBoxCheque.Hide();
                    groupBoxBankTransfer.Hide();
                }
                else if (metroComboBoxPaymentMethod.SelectedItem.ToString() == "Card")
                {
                    groupBox1.Hide();
                    groupBoxCard.Show();
                    groupBoxCard.BringToFront();
                    groupBoxCheque.Hide();
                    groupBoxBankTransfer.Hide();
                }
                else if (metroComboBoxPaymentMethod.SelectedItem.ToString() == "Cheque")
                {
                    groupBox1.Hide();
                    groupBoxCard.Hide();
                    groupBoxCheque.Show();
                    groupBoxCheque.BringToFront();
                    groupBoxBankTransfer.Hide();
                }
                else if (metroComboBoxPaymentMethod.SelectedItem.ToString() == "Bank Transfer")
                {
                    groupBox1.Hide();
                    groupBoxCard.Hide();
                    groupBoxCheque.Hide();
                    groupBoxBankTransfer.Show();
                    groupBoxBankTransfer.BringToFront();
                }
                else if (metroComboBoxPaymentMethod.SelectedItem.ToString() == "Other")
                {
                    groupBox1.Show();
                    groupBox1.BringToFront();
                    groupBoxCard.Hide();
                    groupBoxCheque.Hide();
                    groupBoxBankTransfer.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        public void reset_fields()
        {
            try
            {
                comboBox1.Text = string.Empty;
                metroTextBoxVendorContactNumber.Text = string.Empty;
                metroTextBoxVendorAddress.Text = string.Empty;

                //metroTextBoxCustomerName.Text = string.Empty;
                comboBox1.SelectedIndex = -1;
                comboBox1.Text = "";
                metroTextBoxCustomerContactNumber.Text = string.Empty;
                metroTextBoxCustomerAddress.Text = string.Empty;

                metroTextBoxPaidAmount.Text = string.Empty;

                labelPaymentDue.Text = "0";

                metroTextBoxPaymentNote.Text = string.Empty;

                metroComboBoxPaymentMethod.SelectedIndex = 0;

                metroTextBoxCardNumber.Text = string.Empty;
                metroTextBoxCardHolderName.Text = string.Empty;
                metroTextBoxCardTransactionNo.Text = string.Empty;
                try
                {
                    metroComboBoxCardType.SelectedIndex = -1;
                    metroComboBoxExpiryMonth.SelectedIndex = -1;
                    metroComboBoxExpiryYear.SelectedIndex = -1;
                }
                catch { }
                metroTextBoxSecurityCode.Text = string.Empty;

                metroTextBoxChequeNumber.Text = string.Empty;

                metroTextBoxBankName.Text = string.Empty;
                metroTextBoxBankAccountNumber.Text = string.Empty;

                metroDateTime1.Value = DateTime.Now;

                comboBox1.Focus();
            }
            catch (Exception ex)
            {

            }
        }

        public string todayReceiving(string customer_id, string id)
        {
            try
            {
                DateTime dtm = DateTime.Now;//Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells[3].Value.ToString());
                string date = dtm.Year.ToString() + "-" + dtm.Month.ToString() + "-" + dtm.Day.ToString();
                //string query = "select sum(-balance) from Bill where status!='-1' and Bill_Type='receiving' and Bill_Date>='" + date + "' and Bill_Date<='" + date + "' and Customer_Vendor_Id='" + customer_id + "' and id<'"+id+"'";
                string query = "select sum(-balance) from Bill where status!='-1' and Bill_Type='receiving' and Bill_Date>='" + date + "' and Bill_Date<='" + date + "' and Customer_Vendor_Id='" + customer_id + "' and id!='" + id + "'";
                DataTable dt = gm.GetTable(query);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString().Trim() != "")
                        return dt.Rows[0][0].ToString();
                    return "0";
                }
                return "0";
            }
            catch
            {
                return "0";
            }
        }

        public string getCustomerTodayBill(string customer_id)
        {
            try
            {
                string todaydate = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
                string query = "select sum(Total_Amount) from Bill where status!='-1' and Bill_Type='sale trading' and Bill_Date>='" + todaydate + "' and Bill_Date<='" + todaydate + "' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='" + customer_id + "')";
                DataTable dt = gm.GetTable(query);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString().Trim() != "")
                        return dt.Rows[0][0].ToString();
                    return "0";
                }
                return "0";
            }
            catch
            {
                return "0";
            }
        }

        public string billamountwithouttoday(string customer_id)
        {
            try
            {
                DateTime dtm = DateTime.Now;//Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells[3].Value.ToString());
                //string monthName = dtm.Date.ToString("MMM", CultureInfo.InvariantCulture);
                string date = dtm.Year.ToString() + "-" + dtm.Month.ToString() + "-" + dtm.Day.ToString();
                string query = "select sum(Total_Amount) from Bill where status!='-1' and Bill_Type='sale trading' and Bill_Date<'" + date + "' and Customer_Vendor_Id in (select id from Customer_Or_Vendor where parent='" + customer_id + "')";
                DataTable dt = gm.GetTable(query);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString().Trim() != "")
                        return dt.Rows[0][0].ToString();
                    return "0";
                }
                return "0";
            }
            catch
            {
                return "0";
            }
        }

        public string receivingamountwithouttoday(string customer_id)
        {
            try
            {
                DateTime dtm = DateTime.Now;//Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells[3].Value.ToString());
                //string monthName = dtm.Date.ToString("MMM", CultureInfo.InvariantCulture);
                string date = dtm.Year.ToString() + "-" + dtm.Month.ToString() + "-" + dtm.Day.ToString();
                string query = "select sum(-balance) from Bill where status!='-1' and Bill_Type='receiving' and Bill_Date<'" + date + "' and Customer_Vendor_Id='" + customer_id + "'";
                DataTable dt = gm.GetTable(query);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString().Trim() != "")
                        return dt.Rows[0][0].ToString();
                    return "0";
                }
                return "0";
            }
            catch
            {
                return "0";
            }
        }

        private void metroTile5_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "";
                string id = "";
                if (comboBox1.SelectedValue == null)
                {
                    MessageBox.Show((metroRadioButtonVendor.Checked == true ? "Vendor Name Not Found.\nInvalid Vendor" : "Customer Name Not Found.\nInvalid Customer"));
                    return;
                }
                string customer_or_vendor_id = "";
                {
                    //string[] s = (metroRadioButtonVendor.Checked == true ? comboBox1.Text.Trim().Split('(') : metroTextBoxCustomerName.Text.Trim().Split('('));
                    //string[] a = s[s.Length - 1].Trim().Split(')');
                    //id = a[0].ToString();
                    id = comboBox1.SelectedValue.ToString();
                    if (id.Trim() != "")
                    {
                        query = "Select * from customer_or_vendor where id='" + id + "' and status != '-1'";
                        DataTable dt2 = gm.GetTable(query);
                        if (dt2.Rows.Count <= 0)
                        {
                            MessageBox.Show((metroRadioButtonVendor.Checked == true ? "Vendor Name Not Found.\nInvalid Vendor" : "Customer Name Not Found.\nInvalid Customer"));
                            return;
                        }
                        else
                        {
                            customer_or_vendor_id = dt2.Rows[0]["id"].ToString();
                        }
                    }
                }

                if (metroTextBoxPaidAmount.Text.Trim().ToString() == "")
                {
                    MessageBox.Show("Enter Paid Amount To Complete This Transaction");
                    return;
                }

                if (metroComboBoxPaymentMethod.SelectedIndex < 0)
                {
                    MessageBox.Show("Select Payment Method");
                    return;
                }

                if (metroComboBoxPaymentMethod.SelectedItem.ToString() == "Card")
                {
                    if (metroTextBoxCardHolderName.Text.Trim().ToString() == "")
                    {
                        MessageBox.Show("Enter Card Holder Name");
                        return;
                    }
                    if (metroTextBoxCardNumber.Text.Trim().ToString() == "")
                    {
                        MessageBox.Show("Enter Card Number");
                        return;
                    }
                    if (metroTextBoxCardTransactionNo.Text.Trim().ToString() == "")
                    {
                        MessageBox.Show("Enter Card Transaction No");
                        return;
                    }
                    if (metroComboBoxCardType.SelectedIndex < 0)
                    {
                        MessageBox.Show("Select Card Type");
                        return;
                    }
                    if (metroComboBoxExpiryMonth.SelectedIndex < 0)
                    {
                        MessageBox.Show("Select Card Expiry Month");
                        return;
                    }
                    if (metroComboBoxExpiryYear.SelectedIndex < 0)
                    {
                        MessageBox.Show("Select Card Expiry Year");
                        return;
                    }
                    if (metroTextBoxSecurityCode.Text.Trim().ToString() == "")
                    {
                        MessageBox.Show("Enter Security Code");
                        return;
                    }
                }
                else if (metroComboBoxPaymentMethod.SelectedItem.ToString() == "Cheque")
                {
                    if (metroTextBoxChequeNumber.Text.Trim().ToString() == "")
                    {
                        MessageBox.Show("Enter Cheque Number");
                        return;
                    }
                }
                else if (metroComboBoxPaymentMethod.SelectedItem.ToString() == "Bank Transfer")
                {
                    if (metroTextBoxBankName.Text.Trim().ToString() == "")
                    {
                        MessageBox.Show("Enter Bank Name");
                        return;
                    }
                    if (metroTextBoxBankAccountNumber.Text.Trim().ToString() == "")
                    {
                        MessageBox.Show("Enter Bank Account Number");
                        return;
                    }
                }

                string paymentnote = "";

                string cardholdername = "";
                string cardnumber = "";
                string cardtransactionid = "";
                string cardtype = "";
                string cardexpirymonth = "";
                string cardexpiryyearh = "";
                string cardsecuritycode = "";

                string chequenumber = "";

                string bankname = "";
                string bankaccountnumber = "";

                if (metroComboBoxPaymentMethod.SelectedItem.ToString() == "Cash")
                {
                    paymentnote = metroTextBoxPaymentNote.Text.Trim();
                }
                else if (metroComboBoxPaymentMethod.SelectedItem.ToString() == "Card")
                {
                    cardholdername = metroTextBoxCardHolderName.Text.Trim();
                    cardnumber = metroTextBoxCardNumber.Text.Trim();
                    cardtransactionid = metroTextBoxCardTransactionNo.Text.Trim();
                    cardtype = metroComboBoxCardType.SelectedItem.ToString().Trim();
                    cardexpirymonth = metroComboBoxExpiryMonth.SelectedItem.ToString().Trim();
                    cardexpiryyearh = metroComboBoxExpiryYear.SelectedItem.ToString().Trim();
                    cardsecuritycode = metroTextBoxSecurityCode.Text.Trim();
                    paymentnote = metroTextBoxPaymentNoteCard.Text.Trim();
                }
                else if (metroComboBoxPaymentMethod.SelectedItem.ToString() == "Cheque")
                {
                    chequenumber = metroTextBoxChequeNumber.Text.Trim();

                    paymentnote = metroTextBoxPaymentNoteCheque.Text.Trim();
                }
                else if (metroComboBoxPaymentMethod.SelectedItem.ToString() == "Bank Transfer")
                {
                    bankname = metroTextBoxBankName.Text.Trim();
                    bankaccountnumber = metroTextBoxBankAccountNumber.Text.Trim();
                    paymentnote = metroTextBoxPaymentNoteBankTransfer.Text.Trim();
                }
                else if (metroComboBoxPaymentMethod.SelectedItem.ToString() == "Other")
                {
                    paymentnote = metroTextBoxPaymentNote.Text.Trim();
                }



                int count = 0;
                string billdate = metroDateTime1.Value.Date.Year.ToString() + "-" + metroDateTime1.Value.Date.Month.ToString() + "-" + metroDateTime1.Value.Date.Day.ToString();
                query = "select count(*) from bill where status!='-1' and bill_date='" + billdate + "'";
                DataTable dt1 = gm.GetTable(query);
                try
                {
                    string c = dt1.Rows[0][0].ToString();
                    count = int.Parse(c.Trim());
                }
                catch { }
                count++;
                string countstring = count.ToString();
                if (count.ToString().Trim().Length == 1)
                {
                    countstring = "00" + count.ToString();
                }
                else if (count.ToString().Trim().Length == 2)
                {
                    countstring = "0" + count.ToString();
                }
                //else if (count.ToString().Trim().Length == 3)
                //{
                //    countstring = "0" + count.ToString();
                //}



                try//check if today receiving already exist of that main customer than dont insert 0 receiving
                {
                    query = "select * from bill where status!='-1' and bill_date='" + billdate + "' and bill_type='receiving' and customer_vendor_id='" + customer_or_vendor_id + "'";
                    dt1 = gm.GetTable(query);
                    try
                    {
                        if ((dt1.Rows.Count > 0) && (double.Parse(metroTextBoxPaidAmount.Text.Trim()) == 0))
                        {
                            MessageBox.Show("Receiving with 0(zero) paid amount not be inserted. \n This customer already have receiving in record", "Receiving Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    catch { }
                }
                catch { }



                string billid = "";
                if (PaymentOrReceiving == "Payment")
                {
                    billid = "PAY" + metroDateTime1.Value.Date.Day.ToString() + metroDateTime1.Value.Date.Month.ToString() + metroDateTime1.Value.Date.Year.ToString() + countstring.ToString();
                }
                else if (PaymentOrReceiving == "Receiving")
                {
                    billid = "REC" + metroDateTime1.Value.Date.Day.ToString() + metroDateTime1.Value.Date.Month.ToString() + metroDateTime1.Value.Date.Year.ToString() + countstring.ToString();
                }

                double balance = 0;
                double paymentdue = 0;
                double paidamount = 0;
                double paidamount_with_percentage = 0;
                try
                {
                    paymentdue = double.Parse(labelPaymentDue.Text.Trim());
                }
                catch { }
                try
                {
                    paidamount = double.Parse(metroTextBoxPaidAmount.Text.Trim());
                }
                catch
                { }
                double percentage = 0;
                int percentageamount = 0;
                if(RJ.Properties.Settings.Default.ReceivingInPoints=="0")
                {
                    try
                    {
                        query = "select percentage from customer_or_vendor where id='" + customer_or_vendor_id + "'";
                        DataTable dt = gm.GetTable(query);
                        if (dt.Rows.Count > 0)
                        {
                            percentage = double.Parse(dt.Rows[0][0].ToString().Trim() == "" ? "0" : dt.Rows[0][0].ToString().Trim());
                        }
                    }
                    catch { }
                    percentageamount = int.Parse(Math.Round(((paidamount * percentage) / 100),0,MidpointRounding.AwayFromZero).ToString());
                }
                paidamount_with_percentage = paidamount + percentageamount;
                balance = (paymentdue - paidamount_with_percentage);
                query = "Select max(cast(id as int)) from bill";
                id = gm.MaxId(query);
                query = @"insert into bill values('" + id + "','" + PaymentOrReceiving + "','" + billid + "','" + metroDateTime1.Value.Date + "','" + DateTime.Now.ToShortTimeString() + "','" + ((labelPaymentDue.Text.Trim() == "") ? "0" : labelPaymentDue.Text.Trim()) + "','','0','0','','0','" + percentageamount + "','','0','" + ((labelPaymentDue.Text.Trim().ToString() == "") ? "0" : labelPaymentDue.Text.Trim().ToString()) + "','" + ((metroTextBoxPaidAmount.Text.Trim().ToString() == "") ? "0" : metroTextBoxPaidAmount.Text.Trim().ToString()) + "','" + (PaymentOrReceiving == "Payment" ? paidamount : -(paidamount + percentageamount)) + "','','" + customer_or_vendor_id + "','" + metroComboBoxPaymentMethod.SelectedItem.ToString() + "','" + cardnumber + "','" + cardholdername + "','" + cardtransactionid + "','" + cardtype + "','" + cardexpirymonth + "','" + cardexpiryyearh + "','" + cardsecuritycode + "','" + chequenumber + "','" + bankname + "','" + bankaccountnumber + "','" + paymentnote + "','" + RJ.Properties.Settings.Default.loginid + "','" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.ToShortTimeString() + "','1')";
                //query = @"insert into bill values('" + id + "','" + "Sale Trading" + "','" + billid + "','" + metroDateTime1.Value.Date + "','" + DateTime.Now.ToShortTimeString() + "','" + ((labelNetTotal.Text.Trim() == "") ? "0" : labelNetTotal.Text.Trim()) + "','" + metroComboBoxDiscountType.SelectedItem.ToString() + "','" + ((metroTextBoxDiscount.Text.Trim().ToString() == "") ? "0" : metroTextBoxDiscount.Text.Trim().ToString()) + "','" + ((metroLabDiscountAmount.Text.Trim().ToString() == "") ? "0" : metroLabDiscountAmount.Text.Trim().ToString()) + "','" + metroComboBoxTax.SelectedItem.ToString() + "','" + ((metroLabelPurchaseTax.Text.Trim().ToString() == "") ? "0" : metroLabelPurchaseTax.Text.Trim().ToString()) + "','" + ((metroLabelServiceCharges.Text.Trim().ToString() == "") ? "0" : metroLabelServiceCharges.Text.Trim().ToString()) + "','" + "" + "','" + "0" + "','" + ((labelPurchaseTotal.Text.Trim().ToString() == "") ? "0" : labelPurchaseTotal.Text.Trim().ToString()) + "','" + ((metroTextBoxPaidAmount.Text.Trim().ToString() == "") ? "0" : metroTextBoxPaidAmount.Text.Trim().ToString()) + "','" + ((labelPaymentDue.Text.Trim().ToString() == "") ? "0" : labelPaymentDue.Text.Trim().ToString()) + "','" + "" + "','" + customer_or_vendor_id + "','" + metroComboBoxPaymentMethod.SelectedItem.ToString() + "','" + cardnumber + "','" + cardholdername + "','" + cardtransactionid + "','" + cardtype + "','" + cardexpirymonth + "','" + cardexpiryyearh + "','" + cardsecuritycode + "','" + chequenumber + "','" + bankname + "','" + bankaccountnumber + "','" + paymentnote + "','" + I_Bee_Mini_Mart.Properties.Settings.Default.loginid + "','" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.ToShortTimeString() + "','1')";
                int ra = gm.ExecuteNonQuery(query);
                if (ra > 0)
                {
                    if (metroDateTime1.Value.Date< DateTime.Now.Date)
                    {
                        MessageBox.Show("Transaction Successfully Completed");
                        reset_fields();
                    }
                    else
                    {
                        if (MessageBox.Show("Transaction Successfully Completed.\nDo you want to print reciept? (yes / no)", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            string monthName = metroDateTime1.Value.Date.ToString("MMM", CultureInfo.InvariantCulture);
                            //string date = metroDateTime1.Value.Day.ToString() + " " + monthName + "," + metroDateTime1.Value.Year.ToString();
                            string date = metroDateTime1.Value.Day.ToString() + "-" + metroDateTime1.Value.Month.ToString() + "-" + metroDateTime1.Value.Year.ToString();
                            string report = (PaymentOrReceiving == "Payment" ? "Payment Receipt" : "Receiving Receipt");
                            //gm.printBill(id.Trim(), "Sale Invoice", "Customer", "Original Receipt", date);
                            string PaymentToOrReceivingFrom = (PaymentOrReceiving == "Payment" ? "Payment To : " : "Receiving From : ");
                            string customerOrVendorname = "";
                            try
                            {
                                query = "select * from customer_or_vendor where id='" + customer_or_vendor_id + "'";
                                DataTable dt = gm.GetTable(query);
                                if (dt.Rows.Count > 0)
                                {
                                    customerOrVendorname = dt.Rows[0]["customer_Or_Vendor_Name"].ToString();
                                }
                            }
                            catch { }
                            string PaymentToOrReceivingFromValue = customerOrVendorname;
                            string Amount = paidamount_with_percentage.ToString();
                            try
                            {
                                Amount = (double.Parse(Amount) + double.Parse(todayReceiving(customer_or_vendor_id, id))).ToString();
                            }
                            catch { }

                            string PaymentDueBefore = "Amount Due Before " + PaymentOrReceiving + " : ";
                            double PaymentDueBeforeValue1 = 0;
                            //try
                            //{
                            //    PaymentDueBeforeValue1 -= double.Parse(labelPaymentDue.Text.Trim());
                            //}
                            //catch { }
                            try
                            {
                                //PaymentDueBeforeValue1 = double.Parse(labelPaymentDue.Text.Trim()) - double.Parse(getCustomerTodayBill(customer_or_vendor_id));
                                double billamountwithouttoday2 = Math.Round(double.Parse(billamountwithouttoday(customer_or_vendor_id)), 3);
                                double receivingamountwithouttoday2 = Math.Round(double.Parse(receivingamountwithouttoday(customer_or_vendor_id)), 3);
                                PaymentDueBeforeValue1 = (billamountwithouttoday2 - receivingamountwithouttoday2);
                            }
                            catch { }
                            string PaymentDueBeforeValue = PaymentDueBeforeValue1.ToString();
                            calculatePaymentDue();
                            string PaymentDueAfter = "Amount Due After " + PaymentOrReceiving + " : ";
                            double PaymentDueAfterValue1 = 0;
                            //try
                            //{
                            //    PaymentDueAfterValue1 -= double.Parse(labelPaymentDue.Text.Trim());
                            //}
                            //catch { }
                            try
                            {
                                PaymentDueAfterValue1 = double.Parse(labelPaymentDue.Text.Trim());
                            }
                            catch { }
                            string PaymentDueAfterValue = PaymentDueAfterValue1.ToString();

                            reset_fields();
                            gm.printPaymentReceivingReceipt(monthName, date, report, billid, date, PaymentToOrReceivingFrom, customerOrVendorname, Amount, PaymentDueBefore, PaymentDueBeforeValue, PaymentDueAfter, PaymentDueAfterValue, customer_or_vendor_id, gm.removePoints(getCustomerTodayBill(customer_or_vendor_id)));
                        }
                        else
                        {
                            reset_fields();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroTextBoxPaidAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                metroTile5.PerformClick();
        }

        private void metroTextBoxVendorName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (comboBox1.SelectedItem.ToString().Trim() != "" && comboBox1.SelectedValue.ToString()!="")
                    {
                        //string[] s = metroTextBoxCustomerName.Text.Trim().ToString().Split('(');
                        //string vendorid = s[s.Length - 1].ToString().Trim().Split(')')[0].ToString();
                        string vendorid = comboBox1.SelectedValue.ToString();
                        string query = "select * from customer_or_vendor where id='" + vendorid + "'";
                        DataTable dt2 = gm.GetTable(query);
                        if (dt2.Rows.Count > 0)
                        {
                            foreach (DataRow d in dt2.Rows)
                            {
                                //metroTextBoxVendorName.Text = d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")";
                                //metroTextBoxVendorName.Tag = d["id"].ToString();
                                metroTextBoxVendorContactNumber.Text = d["contact_number"].ToString();
                                metroTextBoxVendorAddress.Text = d["_address"].ToString();
                                metroTextBoxPaidAmount.Focus();
                            }
                        }
                        else
                        {
                            //if (metroTextBoxVendorName.Text.Trim() != "")
                            //{
                            //    metroTextBoxVendorName.Text = "";
                            //    metroTextBoxVendorName.Tag = "";
                            //    metroTextBoxVendorContactNumber.Text = "";
                            //    metroTextBoxVendorAddress.Text = "";
                            //}
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void metroDateTime1_ValueChanged(object sender, EventArgs e)
        {
            getTransactionId();
            calculatePaymentDue();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void metroTextBoxVendorName_Click(object sender, EventArgs e)
        {

        }

        private void metroRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                comboBox1.Text = "";
                metroPanelVendor.Show();
                metroPanelCustomer.Hide();
                comboBox1.Focus();
            }
            catch { }
        }

        private void metroRadioButtonCustomer_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //metroTextBoxCustomerName.Text = "";
                comboBox1.Text = "";
                comboBox1.SelectedIndex = -1;
                metroPanelVendor.Hide();
                metroPanelCustomer.Show();
                //metroTextBoxCustomerName.Focus();
                comboBox1.Focus();
            }
            catch { }
        }

        private void metroTextBoxVendorName_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (comboBox1.Text.Trim().ToString() != "")
                    {
                        string[] s = comboBox1.Text.Trim().ToString().Split('(');
                        string vendorid = s[s.Length - 1].ToString().Trim().Split(')')[0].ToString();
                        string query = "select * from customer_or_vendor where id='" + vendorid + "'";
                        DataTable dt2 = gm.GetTable(query);
                        if (dt2.Rows.Count > 0)
                        {
                            foreach (DataRow d in dt2.Rows)
                            {
                                comboBox1.Text = d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")";
                                comboBox1.Tag = d["id"].ToString();
                                metroTextBoxVendorContactNumber.Text = d["contact_number"].ToString();
                                metroTextBoxVendorAddress.Text = d["_address"].ToString();
                                metroTextBoxPaidAmount.Focus();
                            }
                        }
                        else
                        {
                            //if (metroTextBoxVendorName.Text.Trim() != "")
                            //{
                            //    metroTextBoxVendorName.Text = "";
                            //    metroTextBoxVendorName.Tag = "";
                            //    metroTextBoxVendorContactNumber.Text = "";
                            //    metroTextBoxVendorAddress.Text = "";
                            //}
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void metroTextBoxPaidAmount_TextChanged(object sender, EventArgs e)
        {
            gm.AcceptDouble(sender, e);
            try
            {
                if (RJ.Properties.Settings.Default.ReceivingInPoints == "0")
                {
                    if (metroTextBoxPaidAmount.Text.Trim() != "")
                    {
                        try
                        {
                            metroTextBoxPaidAmount.Text = (Math.Round(double.Parse(metroTextBoxPaidAmount.Text.Trim()), 0, MidpointRounding.AwayFromZero)).ToString();
                            metroTextBoxPaidAmount.Select(metroTextBoxPaidAmount.Text.Length, 0);
                        }
                        catch { }
                    }
                }
                //if (RJ.Properties.Settings.Default.ReceivingInPoints == "0")
                //{
                //    if (metroTextBoxPaidAmount.Text.Trim() != "")
                //    {
                //        try
                //        {
                //            metroTextBoxPaidAmount.Text = (double.Parse(metroTextBoxPaidAmount.Text.Trim()) < 0 ? "" : metroTextBoxPaidAmount.Text);
                //            metroTextBoxPaidAmount.Select(metroTextBoxPaidAmount.Text.Length, 0);
                //        }
                //        catch { }
                //    }
                //}
            }
            catch { }
        }

        private void metroTextBoxVendorName_Click_1(object sender, EventArgs e)
        {

        }

        private void metroTextBoxVendorName_TextChanged_1(object sender, EventArgs e)
        {
            calculatePaymentDue();
        }

        private void metroTextBoxCustomerName_TextChanged(object sender, EventArgs e)
        {
            calculatePaymentDue();
        }

        private void Payment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (MessageBox.Show("Confirmation\n Are You Sure To Close This Form? (yes / no)", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.Dispose(true);
                }
            }
            if (e.KeyCode == Keys.F1)
            {
                if (metroRadioButtonVendor.Checked == true)
                    comboBox1.Focus();
                else if (metroRadioButtonCustomer.Checked == true)
                    comboBox1.Focus();
            }
            if (e.KeyCode == Keys.F2)
                metroTextBoxPaidAmount.Focus();
            if (e.KeyCode == Keys.F3)
                metroComboBoxPaymentMethod.Focus();
            if (e.KeyCode == Keys.F5)
                reset_fields();
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                metroTextBoxPaidAmount.Focus();
            }
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (comboBox1.Text.Trim().ToString() != "" && comboBox1.SelectedValue != null)
                    {
                        //string[] s = comboBox1.Text.Trim().ToString().Split('(');
                        //string vendorid = s[s.Length - 1].ToString().Trim().Split(')')[0].ToString();
                        string vendorid = comboBox1.SelectedValue.ToString();
                        string query = "select * from customer_or_vendor where id='" + vendorid + "'";
                        DataTable dt2 = gm.GetTable(query);
                        if (dt2.Rows.Count > 0)
                        {
                            foreach (DataRow d in dt2.Rows)
                            {
                                //comboBox1.Text = d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")";
                                //comboBox1.Tag = d["id"].ToString();
                                metroTextBoxCustomerContactNumber.Text = d["contact_number"].ToString();
                                metroTextBoxCustomerAddress.Text = d["_address"].ToString();
                                metroTextBoxPaidAmount.Focus();
                            }
                        }
                        else
                        {
                            //if (metroTextBoxVendorName.Text.Trim() != "")
                            //{
                            //    metroTextBoxVendorName.Text = "";
                            //    metroTextBoxVendorName.Tag = "";
                            //    metroTextBoxVendorContactNumber.Text = "";
                            //    metroTextBoxVendorAddress.Text = "";
                            //}
                        }
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            metroTextBoxCustomerContactNumber.Text = "";
            metroTextBoxCustomerAddress.Text = "";
            calculatePaymentDue();
            try
            {
                if (comboBox1.Text.Trim().ToString() != "" && comboBox1.SelectedValue != null)
                {
                    //string[] s = comboBox1.Text.Trim().ToString().Split('(');
                    //string vendorid = s[s.Length - 1].ToString().Trim().Split(')')[0].ToString();
                    string vendorid = comboBox1.SelectedValue.ToString();
                    string query = "select * from customer_or_vendor where id='" + vendorid + "'";
                    DataTable dt2 = gm.GetTable(query);
                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataRow d in dt2.Rows)
                        {
                            //comboBox1.Text = d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")";
                            //comboBox1.Tag = d["id"].ToString();
                            metroTextBoxCustomerContactNumber.Text = d["contact_number"].ToString();
                            metroTextBoxCustomerAddress.Text = d["_address"].ToString();
                            metroTextBoxPaidAmount.Focus();
                        }
                    }
                    else
                    {
                        //if (metroTextBoxVendorName.Text.Trim() != "")
                        //{
                        //    metroTextBoxVendorName.Text = "";
                        //    metroTextBoxVendorName.Tag = "";
                        //    metroTextBoxVendorContactNumber.Text = "";
                        //    metroTextBoxVendorAddress.Text = "";
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            calculatePaymentDue();
            try
            {
                if (comboBox1.Text.Trim().ToString() != "" && comboBox1.SelectedValue != null)
                {
                    //string[] s = comboBox1.Text.Trim().ToString().Split('(');
                    //string vendorid = s[s.Length - 1].ToString().Trim().Split(')')[0].ToString();
                    string vendorid = comboBox1.SelectedValue.ToString();
                    string query = "select * from customer_or_vendor where id='" + vendorid + "'";
                    DataTable dt2 = gm.GetTable(query);
                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataRow d in dt2.Rows)
                        {
                            //comboBox1.Text = d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")";
                            //comboBox1.Tag = d["id"].ToString();
                            metroTextBoxCustomerContactNumber.Text = d["contact_number"].ToString();
                            metroTextBoxCustomerAddress.Text = d["_address"].ToString();
                            metroTextBoxPaidAmount.Focus();
                        }
                    }
                    else
                    {
                        //if (metroTextBoxVendorName.Text.Trim() != "")
                        //{
                        //    metroTextBoxVendorName.Text = "";
                        //    metroTextBoxVendorName.Tag = "";
                        //    metroTextBoxVendorContactNumber.Text = "";
                        //    metroTextBoxVendorAddress.Text = "";
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            metroTextBoxPaidAmount.Focus();
        }

        private void metroTextBoxVendorContactNumber_Click(object sender, EventArgs e)
        {

        }

        private void metroLabel35_Click(object sender, EventArgs e)
        {

        }

        private void metroPanelVendor_Paint(object sender, PaintEventArgs e)
        {

        }

        private void metroTextBoxPaidAmount_Click(object sender, EventArgs e)
        {

        }
    }
}
