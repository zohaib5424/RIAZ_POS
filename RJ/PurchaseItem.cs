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
    public partial class PurchaseItem : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public PurchaseItem()
        {
            InitializeComponent();
        }

        AutoCompleteStringCollection source = new AutoCompleteStringCollection();
        AutoCompleteStringCollection sourceCustomer_Or_Vendor = new AutoCompleteStringCollection();
        AutoCompleteStringCollection sourceitemname = new AutoCompleteStringCollection();


        public void getTaxes()
        {
            try
            {
                metroComboBoxTax.Items.Clear();
                metroComboBoxTax.Items.Add("None");
                string query = "select * from taxes where status='1'";
                DataTable dt = gm.GetTable(query);
                foreach (DataRow d in dt.Rows)
                {
                    metroComboBoxTax.Items.Add(d["Tax_Name"].ToString()+" @ "+ d["Tax_Percent"].ToString());
                }
                metroComboBoxTax.SelectedIndex = 0;
                metroComboBoxTax.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void getTransactionId()
        {
            try
            {
                int count = 0;
                string billdate = metroDateTime1.Value.Date.Year.ToString() + "-" + metroDateTime1.Value.Date.Month.ToString() + "-" + metroDateTime1.Value.Date.Day.ToString();
                string query = "select count(*) from bill where bill_date='" + billdate + "'";
                DataTable dt1 = gm.GetTable(query);
                try
                {
                    string c = dt1.Rows[0][0].ToString();
                    count = int.Parse(c.Trim());
                }
                catch { }
                count++;
                string billid = "P" + metroDateTime1.Value.Date.Day.ToString() + metroDateTime1.Value.Date.Month.ToString() + metroDateTime1.Value.Date.Year.ToString() + "XX";
                metroLabelTransactionId.Text = billid;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ItemsList_Load(object sender, EventArgs e)
        {
            try
            {
                hideMfgExpDate();
                hideBatchOrLotNumber();

                source = new AutoCompleteStringCollection();
                sourceCustomer_Or_Vendor = new AutoCompleteStringCollection();
                this.AutoScroll = true;

                getTaxes();
                metroTextBoxItemName.Focus();
                metroComboBoxDiscountType.Items.Add("Fixed");
                metroComboBoxDiscountType.Items.Add("Percentage");
                int i = metroComboBoxDiscountType.Items.IndexOf("Fixed");
                metroComboBoxDiscountType.SelectedIndex = i;

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


                dataGridView1.Columns[0].ReadOnly = false;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].ReadOnly = false;
                dataGridView1.Columns[3].ReadOnly = false;
                dataGridView1.Columns[4].ReadOnly = true;
                dataGridView1.Columns[5].ReadOnly = true;
                dataGridView1.Columns[6].ReadOnly = true;
                dataGridView1.Columns[7].ReadOnly = true;
                dataGridView1.Columns[8].ReadOnly = false;

                DataTable dt = gm.GetTable("select * from items where status ='1'");
                foreach (DataRow d in dt.Rows)
                {
                    sourceitemname.Add(d["name"].ToString() + " (" + d["id"].ToString() + ")");
                }

                metroTextBoxItemName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                metroTextBoxItemName.AutoCompleteCustomSource = sourceitemname;
                metroTextBoxItemName.AutoCompleteSource = AutoCompleteSource.CustomSource;

                dt = gm.GetTable("select * from customer_or_vendor where status ='1' and customer_vendor_type='Vendor'");
                foreach (DataRow d in dt.Rows)
                {
                    sourceCustomer_Or_Vendor.Add(d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")");
                }

                metroTextBoxVendorName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                metroTextBoxVendorName.AutoCompleteCustomSource = sourceCustomer_Or_Vendor;
                metroTextBoxVendorName.AutoCompleteSource = AutoCompleteSource.CustomSource;

                metroTextBoxItemName.Focus();
                getTransactionId();

                metroDateTimeManufacturingDate.Value = DateTime.Now;
                metroDateTimeExpiryDate.Value = DateTime.Now.AddYears(1);
                metroDateTimeExpiryDate.MinDate = metroDateTimeManufacturingDate.Value;
                metroDateTimeManufacturingDate.MaxDate = metroDateTimeExpiryDate.Value;

                try
                {
                    metroComboBoxDiscountType.SelectedIndex = 0;
                    metroComboBoxTax.SelectedIndex = 0;
                    metroComboBoxPaymentMethod.SelectedIndex = 0;
                }
                catch { }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        GMDB gm = new GMDB();

        public void showMfgExpDate()
        {
            lblManufacturingDate.Show();
            metroDateTimeManufacturingDate.Show();
            lblExpiryDate.Show();
            metroDateTimeExpiryDate.Show();
        }

        public void hideMfgExpDate()
        {
            metroDateTimeManufacturingDate.Value = DateTime.Now;
            metroDateTimeExpiryDate.Value = DateTime.Now.AddYears(1);

            lblManufacturingDate.Hide();
            metroDateTimeManufacturingDate.Hide();
            lblExpiryDate.Hide();
            metroDateTimeExpiryDate.Hide();
        }

        public void showBatchOrLotNumber()
        {
            lblBatchOrLotNumber.Show();
            metroTextBoxBatchOrLotNumber.Show();
        }

        public void hideBatchOrLotNumber()
        {
            metroTextBoxBatchOrLotNumber.Text = string.Empty;

            lblBatchOrLotNumber.Hide();
            metroTextBoxBatchOrLotNumber.Hide();
        }

        public void getItemNameRecord(string id)
        {
            try
            {
                string query = "Select * from items where id='" + id + "' and status = '1'";
                DataTable dt = gm.GetTable(query);
                foreach (DataRow d in dt.Rows)
                {
                    int exist = 0;
                    foreach (DataGridViewRow r in dataGridView1.Rows)
                    {
                        if (r.Cells[1].Value.ToString() == d["name"].ToString())
                        {
                            double qty = 0;
                            try
                            {
                                qty = double.Parse(r.Cells[2].Value.ToString());
                            }
                            catch { }
                            qty++;
                            r.Cells[2].Value = qty.ToString();
                            exist = 1;
                            metroTextBoxItemName.Text = string.Empty;
                            metroTextBoxItemName.Focus();
                            hideMfgExpDate();
                            hideBatchOrLotNumber();
                        }
                    }

                    if (d["have_mfg_or_exp_date"].ToString() == "True")
                    {
                        showMfgExpDate();
                    }
                    else
                    {
                        hideMfgExpDate();
                    }
                    if (d["have_batch_or_lot_no"].ToString() == "True")
                    {
                        showBatchOrLotNumber();
                    }
                    else
                    {
                        hideBatchOrLotNumber();
                    }

                    if (exist == 0)
                    {
                        if (metroDateTimeManufacturingDate.Visible.ToString() == "False" && metroTextBoxBatchOrLotNumber.Visible.ToString() == "False")
                        {
                            dataGridView1.Rows.Add(d["id"].ToString(), d["name"].ToString(), "1", d["Purchase_Price"].ToString(), "", "", "","", "X");
                            hideMfgExpDate();
                            hideBatchOrLotNumber();
                            calculateTotalAmount();
                            calculateDiscountAmount();
                            calculatePurchaseTax();
                            calculateServiceCharges();
                            calculatePurchaseTotal();
                            calculatePaymentDue();
                            metroTextBoxItemName.Text = string.Empty;
                            metroTextBoxItemName.Focus();
                        }
                        else
                        {
                            if (metroDateTimeManufacturingDate.Visible.ToString() == "True")
                                metroDateTimeManufacturingDate.Focus();
                            else
                                metroTextBoxBatchOrLotNumber.Focus();
                        }
                    }
                }
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void metroRadioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //dataGridView1.Rows[e.RowIndex].Selected = true;
                if (e.ColumnIndex == 8)
                {
                    dataGridView1.Rows.RemoveAt(e.RowIndex);
                    calculateTotalAmount();
                    calculateDiscountAmount();
                    calculatePurchaseTax();
                    calculateServiceCharges();
                    calculatePurchaseTotal();
                    calculatePaymentDue();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        public void calculateTotalAmount()
        {
            try
            {
                double total = 0;
                foreach (DataGridViewRow r in dataGridView1.Rows)
                {
                        double qty = 0;
                        double ratePerUnit = 0;
                    try
                    {
                        qty = double.Parse(r.Cells[2].Value.ToString());
                    }
                    catch { }
                    try
                    {
                        ratePerUnit = double.Parse(r.Cells[3].Value.ToString());
                    }
                    catch { }
                    double totalofitem = qty * ratePerUnit;
                    r.Cells[4].Value = totalofitem.ToString();
                    try
                    {
                        total += double.Parse(r.Cells[4].Value.ToString());
                    }catch{ }
                    labelNetTotal.Text = total.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void calculateDiscountAmount()
        {
            try
            {
                double netTotal = 0;
                double discount = 0;
                double discountAmount = 0;
                try
                {
                    netTotal = double.Parse(labelNetTotal.Text.ToString());
                }
                catch { }
                try
                {
                    discount = double.Parse(metroTextBoxDiscount.Text.Trim().ToString());
                }
                catch { }
                try
                {
                    if(metroComboBoxDiscountType.SelectedItem.ToString() == "Fixed")
                    {
                        discountAmount = discount;
                    }
                    else if(metroComboBoxDiscountType.SelectedItem.ToString() == "Percentage")
                    {
                        discountAmount = ((netTotal * discount) / 100);
                    }
                }
                catch { }
                metroLabDiscountAmount.Text = discountAmount.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void calculatePurchaseTax()
        {
            try
            {
                double tax = 0;
                double purchaseTax = 0;

                double netTotal = 0;
                double discountAmount = 0;
                try
                {
                    netTotal = double.Parse(labelNetTotal.Text.ToString());
                }
                catch { }
                try
                {
                    discountAmount = double.Parse(metroLabDiscountAmount.Text.Trim());
                }
                catch { }
                try
                {
                    if (metroComboBoxTax.SelectedItem.ToString().Contains("@"))
                    {
                        string[] a = metroComboBoxTax.SelectedItem.ToString().Split('@');
                        string percent = a[a.Length - 1].ToString().Trim();
                        tax = double.Parse(percent.ToString());
                    }
                    else
                    {
                        tax = 0;
                    }
                }
                catch { }
                try
                {
                    purchaseTax = (((netTotal - discountAmount)*tax)/100);
                }
                catch { }
                metroLabelPurchaseTax.Text = purchaseTax.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void calculateServiceCharges()
        {
            try
            {
                double service = 0;
                double serviceCharges = 0;
                try
                {
                    service = double.Parse(metroTextBoxServiceCharges.Text.ToString());
                }
                catch { }
                try
                {
                    serviceCharges= service;
                }
                catch { }
                metroLabelServiceCharges.Text = serviceCharges.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void calculatePurchaseTotal()
        {
            try
            {
                double purchaseTotal = 0;
                double netTotal = 0;
                double discountAmount = 0;
                double purchaseTax = 0;
                double serviceCharges = 0;
                try
                {
                    netTotal = double.Parse(labelNetTotal.Text.ToString());
                }
                catch { }
                try
                {
                    discountAmount = double.Parse(metroLabDiscountAmount.Text.Trim());
                }
                catch { }
                try
                {
                    purchaseTax = double.Parse(metroLabelPurchaseTax.Text.ToString());
                }
                catch { }
                try
                {
                    serviceCharges = double.Parse(metroLabelServiceCharges.Text.ToString());
                }
                catch { }
                try
                {
                    purchaseTotal = (((netTotal - discountAmount)+purchaseTax)+serviceCharges);
                }
                catch { }
                labelPurchaseTotal.Text = purchaseTotal.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void calculatePaymentDue()
        {
            try
            {
                double purchaseTotal = 0;
                double paidAmount = 0;
                double paymentDue = 0;
                try
                {
                    purchaseTotal = double.Parse(labelPurchaseTotal.Text.ToString());
                }
                catch { }
                try
                {
                    paidAmount = double.Parse(metroTextBoxPaidAmount.Text.ToString());
                }
                catch { }
                try
                {
                    paymentDue = (purchaseTotal - paidAmount);
                }
                catch { }
                labelPaymentDue.Text = paymentDue.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {//for sku/barcode
                if (metroTextBoxItemName.Text.Trim().ToString() != "")
                {
                    string query = "select * from items where (sku='" + metroTextBoxItemName.Text.Trim().ToString() + "' or barcode='" + metroTextBoxItemName.Text.Trim().ToString() + "') and status='1'";
                    DataTable dt2 = gm.GetTable(query);
                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataRow d in dt2.Rows)
                        {
                            getItemNameRecord(d["id"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                if (e.ColumnIndex == 2)
                {
                    try
                    {
                        double a = double.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                        if (a <= 0)
                            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "1";
                    }
                    catch
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "1";
                    }
                }
                if (e.ColumnIndex == 3)
                {
                    try
                    {
                        double a = double.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                        if (a < 0)
                            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "0";
                    }
                    catch
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "0";
                    }
                }
            }
            calculateTotalAmount();
            calculateDiscountAmount();
            calculatePurchaseTax();
            calculateServiceCharges();
            calculatePurchaseTotal();
            calculatePaymentDue();
        }

        private void metroTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    try
                    {
                        string[] s = metroTextBoxItemName.Text.Trim().Split('(');
                        string[] a = s[s.Length - 1].Trim().Split(')');
                        string id = a[0].ToString();
                        if (id.Trim() != "")
                        {
                            getItemNameRecord(id);
                        }
                        else
                        {
                            metroTextBoxItemName.Text = string.Empty;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        metroTextBoxItemName.Text = string.Empty;
                    }
                    //metroTextBoxItemName.Text = string.Empty;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void metroComboBoxDiscountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (metroComboBoxDiscountType.Text == "Fixed")
                {
                    calculateTotalAmount();
                    calculateDiscountAmount();
                    calculatePurchaseTax();
                    calculateServiceCharges();
                    calculatePurchaseTotal();
                    calculatePaymentDue();
                }
                else if (metroComboBoxDiscountType.Text == "Percentage")
                {
                    calculateTotalAmount();
                    calculateDiscountAmount();
                    calculatePurchaseTax();
                    calculateServiceCharges();
                    calculatePurchaseTotal();
                    calculatePaymentDue();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroTextBoxDiscount_TextChanged(object sender, EventArgs e)
        {
            gm.AcceptDouble(sender, e);
            calculateTotalAmount();
            calculateDiscountAmount();
            calculatePurchaseTax();
            calculateServiceCharges();
            calculatePurchaseTotal();
            calculatePaymentDue();
        }

        private void metroComboBoxTax_SelectedIndexChanged(object sender, EventArgs e)
        {
            calculateTotalAmount();
            calculateDiscountAmount();
            calculatePurchaseTax();
            calculateServiceCharges();
            calculatePurchaseTotal();
            calculatePaymentDue();
        }

        private void metroTextBoxPurchasePrice_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBoxDiscount_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBoxPurchasePrice_TextChanged(object sender, EventArgs e)
        {
            gm.AcceptDouble(sender, e);
            calculateTotalAmount();
            calculateDiscountAmount();
            calculatePurchaseTax();
            calculateServiceCharges();
            calculatePurchaseTotal();
            calculatePaymentDue();
        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            Taxes b = new Taxes();
            b.FormBorderStyle = FormBorderStyle.None;
            b.ShowDialog();
            getTaxes();
        }

        private void metroTextBox3_TextChanged(object sender, EventArgs e)
        {
            gm.AcceptDouble(sender, e);
            calculateTotalAmount();
            calculateDiscountAmount();
            calculatePurchaseTax();
            calculateServiceCharges();
            calculatePurchaseTotal();
            calculatePaymentDue();
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
                source = new AutoCompleteStringCollection();

                getTaxes();
                int i = metroComboBoxDiscountType.Items.IndexOf("Fixed");
                metroComboBoxDiscountType.SelectedIndex = i;

                metroComboBoxPaymentMethod.SelectedIndex = 0;

                metroComboBoxCardType.SelectedIndex = -1;

                metroComboBoxExpiryMonth.SelectedIndex = -1;

                metroComboBoxExpiryYear.SelectedIndex = -1;

                DataTable dt = gm.GetTable("select * from items where status ='1'");
                foreach (DataRow d in dt.Rows)
                {
                    sourceitemname.Add(d["name"].ToString() + " (" + d["id"].ToString() + ")");
                }

                metroTextBoxItemName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                metroTextBoxItemName.AutoCompleteCustomSource = sourceitemname;
                metroTextBoxItemName.AutoCompleteSource = AutoCompleteSource.CustomSource;

                dt = gm.GetTable("select * from customer_or_vendor where status ='1' and customer_vendor_type='Vendor'");
                foreach (DataRow d in dt.Rows)
                {
                    sourceCustomer_Or_Vendor.Add(d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")");
                }

                metroTextBoxVendorName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                metroTextBoxVendorName.AutoCompleteCustomSource = sourceCustomer_Or_Vendor;
                metroTextBoxVendorName.AutoCompleteSource = AutoCompleteSource.CustomSource;

                metroTextBoxItemName.Focus();

                metroTextBoxItemName.Text = string.Empty;
                if (dataGridView1.Rows.Count > 0)
                    dataGridView1.Rows.Clear();
                metroTextBoxDiscount.Text = string.Empty;
                metroTextBoxServiceCharges.Text = string.Empty;
                metroTextBoxPaidAmount.Text = string.Empty;
                metroTextBoxVendorName.Text = string.Empty;
                metroTextBoxVendorContactNumber.Text = string.Empty;
                metroTextBoxVendorAddress.Text = string.Empty;

                metroTextBoxPaymentNote.Text = string.Empty;

                metroTextBoxCardNumber.Text = string.Empty;
                metroTextBoxCardHolderName.Text = string.Empty;
                metroTextBoxCardTransactionNo.Text = string.Empty;
                metroComboBoxCardType.SelectedIndex = -1;
                metroComboBoxExpiryMonth.SelectedIndex = -1;
                metroComboBoxExpiryYear.SelectedIndex = -1;
                metroTextBoxSecurityCode.Text = string.Empty;

                metroTextBoxChequeNumber.Text = string.Empty;

                metroTextBoxBankName.Text = string.Empty;
                metroTextBoxBankAccountNumber.Text = string.Empty;

                metroDateTime1.Value = DateTime.Now;
                labelNetTotal.Text = "0";
                labelPurchaseTotal.Text = "0";
                labelPaymentDue.Text = "0";
                calculateTotalAmount();
                calculateDiscountAmount();
                calculatePurchaseTax();
                calculateServiceCharges();
                calculatePurchaseTotal();
                calculatePaymentDue();
                getTransactionId();

                metroDateTimeManufacturingDate.Value = DateTime.Now;
                metroDateTimeExpiryDate.Value = DateTime.Now.AddYears(1);

                try
                {
                    metroComboBoxDiscountType.SelectedIndex = 0;
                    metroComboBoxTax.SelectedIndex = 0;
                    metroComboBoxPaymentMethod.SelectedIndex = 0;
                }
                catch { }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void PurchaseItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Dispose(true);
            if (e.KeyCode == Keys.F1)
                metroTextBoxItemName.Focus();
            if (e.KeyCode == Keys.F2)
                metroTextBoxPaidAmount.Focus();
            if (e.KeyCode == Keys.F3)
                metroComboBoxPaymentMethod.Focus();
            if (e.KeyCode == Keys.F4)
                metroTextBoxVendorName.Focus();
            if (e.KeyCode == Keys.F5)
                reset_fields();
        }

        private void metroTile5_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count <= 0)
                {
                    MessageBox.Show("No Item Found For Complete This Transaction");
                    return;
                }
                if (metroTextBoxPaidAmount.Text.Trim().ToString () == "")
                {
                    MessageBox.Show("Enter Paid Amount To Complete This Transaction");
                    return;
                }

                if (metroComboBoxPaymentMethod.SelectedIndex<0)
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
                    if (metroComboBoxCardType.SelectedIndex< 0)
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

                string vendorName = "";
                string vendorContactNumber = "";
                string vendorAddress = "";
                string query = "";
                string customer_or_vendor_id = "";
                if(metroTextBoxVendorName.Text.Trim().ToString() != "")
                {
                    string[] s = metroTextBoxVendorName.Text.Trim().ToString().Split('(');
                    string vendorid = s[s.Length - 1].ToString().Trim().Split(')')[0].ToString();
                    query = "select * from customer_or_vendor where id='"+vendorid+"'";
                    DataTable dt2 = gm.GetTable(query);
                    if (dt2.Rows.Count > 0)
                        customer_or_vendor_id = vendorid;
                    else
                    {
                        MessageBox.Show("Sub Customer Not Registered");
                        return;
                    //    query = "select max(cast(id as int)) from customer_or_vendor";
                    //    string vid = gm.MaxId(query);
                    //    query = "insert into customer_or_vendor values('" + vid + "','Vendor',N'" + metroTextBoxVendorName.Text.Trim().ToString() + "',N'" + metroTextBoxVendorContactNumber.Text.Trim().ToString() + "',N'" + metroTextBoxVendorAddress.Text.Trim().ToString() + "','" + Pos_Sabzi_Mandi.Properties.Settings.Default.loginid + "','" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.ToShortTimeString() + "','1','0')";
                    //    gm.ExecuteNonQuery(query);
                    //    customer_or_vendor_id = vid;
                    }
                }
                else
                {
                    MessageBox.Show("Enter Sub Customer");
                }
                string paymentnote = "";

                string cardholdername = "";
                string cardnumber = "";
                string cardtransactionid = "";
                string cardtype = "";
                string cardexpirymonth= "";
                string cardexpiryyearh= "";
                string cardsecuritycode= "";
                
                string chequenumber = "";

                string bankname = "";
                string bankaccountnumber = "";
                
                if(metroComboBoxPaymentMethod.SelectedItem.ToString() == "Cash")
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
                query = "select count(*) from bill where bill_date='" + billdate + "'";
                DataTable dt1 = gm.GetTable(query);
                try
                {
                    string c = dt1.Rows[0][0].ToString();
                    count = int.Parse(c.Trim());
                }
                catch { }
                count++;
                string billid = "P" + metroDateTime1.Value.Date.Day.ToString() + metroDateTime1.Value.Date.Month.ToString() + metroDateTime1.Value.Date.Year.ToString() + count.ToString();
                query = "Select max(cast(id as int)) from bill";
                string id = gm.MaxId(query);

                query = @"insert into bill values('" + id + "','" + "Purchase Trading" + "','" + billid + "','" + metroDateTime1.Value.Date + "','" + DateTime.Now.ToShortTimeString() + "','" + ((labelNetTotal.Text.Trim() == "") ? "0" : labelNetTotal.Text.Trim()) + "','" + metroComboBoxDiscountType.SelectedItem.ToString() + "','" + ((metroTextBoxDiscount.Text.Trim().ToString() == "") ? "0" : metroTextBoxDiscount.Text.Trim().ToString()) + "','" + ((metroLabDiscountAmount.Text.Trim().ToString() == "") ? "0" : metroLabDiscountAmount.Text.Trim().ToString()) + "',N'" + metroComboBoxTax.SelectedItem.ToString() + "','" + ((metroLabelPurchaseTax.Text.Trim().ToString() == "") ? "0" : metroLabelPurchaseTax.Text.Trim().ToString()) + "','" + ((metroLabelServiceCharges.Text.Trim().ToString() == "") ? "0" : metroLabelServiceCharges.Text.Trim().ToString()) + "','" + "" + "','" + "0" + "','" + ((labelPurchaseTotal.Text.Trim().ToString() == "") ? "0" : labelPurchaseTotal.Text.Trim().ToString()) + "','" + ((metroTextBoxPaidAmount.Text.Trim().ToString() == "") ? "0" : metroTextBoxPaidAmount.Text.Trim().ToString()) + "','" + ((labelPaymentDue.Text.Trim().ToString() == "") ? "0" : labelPaymentDue.Text.Trim().ToString()) + "','" + "" + "',N'" + customer_or_vendor_id + "',N'" + metroComboBoxPaymentMethod.SelectedItem.ToString() + "',N'" + cardnumber + "',N'" + cardholdername + "',N'" + cardtransactionid + "',N'" + cardtype + "','" + cardexpirymonth + "','" + cardexpiryyearh + "',N'" + cardsecuritycode + "',N'" + chequenumber + "',N'" + bankname + "',N'" + bankaccountnumber + "',N'" + paymentnote + "','" + RJ.Properties.Settings.Default.loginid + "','" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.ToShortTimeString() + "','1')";
                int a = gm.ExecuteNonQuery(query);
                if (a > 0)
                {
                    int ok = 0;
                    foreach (DataGridViewRow d in dataGridView1.Rows)
                    {
                        string itemid = d.Cells[0].Value.ToString();
                        string qty = d.Cells[2].Value.ToString();
                        string mazdori = d.Cells[7].Value.ToString();
                        string unitcost = d.Cells[3].Value.ToString();
                        string totalamount = d.Cells[4].Value.ToString();
                        string mfgdate = d.Cells[5].Value.ToString();
                        string expdate = d.Cells[6].Value.ToString();
                        string batchorlotnum = d.Cells[7].Value.ToString();
                        query = "select max(cast(id as int)) from bill_details";
                        string bid = gm.MaxId(query);
                        try
                        {
                            string[] mfgdate1 = mfgdate.Split('/');
                            mfgdate = mfgdate1[2] + "/" + mfgdate1[1] + "/" + mfgdate1[0];//yyyy/MM/dd
                        }catch{}
                        try
                        {
                            string[] expdate1 = expdate.Split('/');
                            expdate = expdate1[2] + "/" + expdate1[1] + "/" + expdate1[0];//yyyy/MM/dd
                        }
                        catch { }
                        query = "insert into bill_details values('" + bid + "','" + id + "','" + itemid + "','" + qty + "','" + unitcost + "','" + totalamount + "','" + mfgdate + "','" + expdate + "','" + batchorlotnum + "','1','','" + DateTime.Now.ToShortDateString() + "','"+mazdori+"')";
                        int b = gm.ExecuteNonQuery(query);
                        if (b > 0)
                        {
                            query = "select qty from items where id = '"+itemid+"'";
                            DataTable dt3 = gm.GetTable(query); 
                            string itemqty = dt3.Rows[0][0].ToString();
                            if (itemqty.Trim() == "")
                            {
                                itemqty = "0";
                            }
                            try
                            {
                                itemqty = (double.Parse(itemqty) + double.Parse(qty)).ToString();
                            }
                            catch { }
                            query = "update items set qty='"+itemqty+"',purchase_price='"+unitcost+"' where id = '"+itemid+"'";
                            gm.ExecuteNonQuery(query);
                            ok = 1;
                        }
                    }
                    if (ok == 1)
                    {
                        if (MessageBox.Show("Transaction Successfully Completed.\nDo you want to print reciept? (yes / no)", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            string monthName = metroDateTime1.Value.Date.ToString("MMM", CultureInfo.InvariantCulture);
                            string date = metroDateTime1.Value.Day.ToString() + " " + monthName + "," + metroDateTime1.Value.Year.ToString();
                            gm.printBill(id, "Purchase Invoice", "Vendor", "Original Receipt",date);
                        }
                        else
                        {

                        }
                        reset_fields();
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

        private void metroTextBoxVendorName_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    if (metroTextBoxVendorName.Text.Trim().ToString() != "")
            //    {
            //        string[] s = metroTextBoxVendorName.Text.Trim().ToString().Split('(');
            //        string vendorid = s[s.Length - 1].ToString().Trim().Split(')')[0].ToString();
            //        string query = "select * from customer_or_vendor where id='" + vendorid + "'";
            //        DataTable dt2 = gm.GetTable(query);
            //        if (dt2.Rows.Count > 0)
            //        {
            //            foreach (DataRow d in dt2.Rows)
            //            {
            //                metroTextBoxVendorName.Text = d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")";
            //                metroTextBoxVendorName.Tag = d["id"].ToString();
            //                metroTextBoxVendorContactNumber.Text = d["contact_number"].ToString();
            //                metroTextBoxVendorAddress.Text = d["_address"].ToString();
            //            }
            //        }
            //        else
            //        {
            //            //if (metroTextBoxVendorName.Text.Trim() != "")
            //            //{
            //            //    metroTextBoxVendorName.Text = "";
            //            //    metroTextBoxVendorName.Tag = "";
            //            //    metroTextBoxVendorContactNumber.Text = "";
            //            //    metroTextBoxVendorAddress.Text = "";
            //            //}
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void metroTextBoxVendorName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (metroTextBoxVendorName.Text.Trim().ToString() != "")
                    {
                        string[] s = metroTextBoxVendorName.Text.Trim().ToString().Split('(');
                        string vendorid = s[s.Length - 1].ToString().Trim().Split(')')[0].ToString();
                        string query = "select * from customer_or_vendor where id='" + vendorid + "'";
                        DataTable dt2 = gm.GetTable(query);
                        if (dt2.Rows.Count > 0)
                        {
                            foreach (DataRow d in dt2.Rows)
                            {
                                metroTextBoxVendorName.Text = d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")";
                                metroTextBoxVendorName.Tag = d["id"].ToString();
                                metroTextBoxVendorContactNumber.Text = d["contact_number"].ToString();
                                metroTextBoxVendorAddress.Text = d["_address"].ToString();
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
        }

        private void metroTextBoxBatchOrLotNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                metroButton1.PerformClick();
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
                try
                {
                    string id = "";
                    try
                    {
                        string[] s = metroTextBoxItemName.Text.Trim().Split('(');
                        string[] a = s[s.Length - 1].Trim().Split(')');
                        id = a[0].ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        metroTextBoxItemName.Text = string.Empty;
                        metroTextBoxItemName.Focus();
                        return;
                    }
                    string query = "Select * from items where id='" + id + "' and status = '1'";
                    DataTable dt = gm.GetTable(query);
                    foreach (DataRow d in dt.Rows)
                    {
                        int exist = 0;
                        foreach (DataGridViewRow r in dataGridView1.Rows)
                        {
                            if (r.Cells[1].Value.ToString() == d["name"].ToString())
                            {
                                double qty = 0;
                                try
                                {
                                    qty = double.Parse(r.Cells[2].Value.ToString());
                                }
                                catch { }
                                qty++;
                                r.Cells[2].Value = qty.ToString();
                                exist = 1;
                                metroTextBoxItemName.Text = string.Empty;
                                metroTextBoxItemName.Focus();
                                hideMfgExpDate();
                                hideBatchOrLotNumber();
                            }
                        }

                        if (d["have_mfg_or_exp_date"].ToString() == "True")
                        {
                            showMfgExpDate();
                        }
                        else
                        {
                            hideMfgExpDate();
                        }
                        if (d["have_batch_or_lot_no"].ToString() == "True")
                        {
                            showBatchOrLotNumber();
                        }
                        else
                        {
                            hideBatchOrLotNumber();
                        }

                        if (exist == 0)
                        {
                            if (metroDateTimeManufacturingDate.Visible.ToString() == "False" && metroTextBoxBatchOrLotNumber.Visible.ToString() == "False")
                            {
                                dataGridView1.Rows.Add(d["id"].ToString(), d["name"].ToString(), "1", d["Purchase_Price"].ToString(), "", "", "", "", "X");
                                hideMfgExpDate();
                                hideBatchOrLotNumber();
                                calculateTotalAmount();
                                calculateDiscountAmount();
                                calculatePurchaseTax();
                                calculateServiceCharges();
                                calculatePurchaseTotal();
                                calculatePaymentDue();
                                metroTextBoxItemName.Text = string.Empty;
                                metroTextBoxItemName.Focus();
                            }
                            else if (metroDateTimeManufacturingDate.Visible.ToString() == "True" && metroTextBoxBatchOrLotNumber.Visible.ToString() == "True")
                            {
                                if (metroTextBoxBatchOrLotNumber.Text.Trim().ToString() == "")
                                {
                                    MessageBox.Show("Enter Batch/Lot Number");
                                    metroTextBoxBatchOrLotNumber.Focus();
                                    return;
                                }
                                string mfg = metroDateTimeManufacturingDate.Value.Date.Day.ToString() + "/" + metroDateTimeManufacturingDate.Value.Date.Month.ToString() + "/" + metroDateTimeManufacturingDate.Value.Date.Year.ToString();
                                string exp = metroDateTimeExpiryDate.Value.Date.Day.ToString() + "/" + metroDateTimeExpiryDate.Value.Date.Month.ToString() + "/" + metroDateTimeExpiryDate.Value.Date.Year.ToString();
                                dataGridView1.Rows.Add(d["id"].ToString(), d["name"].ToString(), "1", d["Purchase_Price"].ToString(), "", mfg, exp, metroTextBoxBatchOrLotNumber.Text.Trim().ToString(), "X");
                                hideMfgExpDate();
                                hideBatchOrLotNumber();
                                metroTextBoxItemName.Text = string.Empty;
                                metroTextBoxItemName.Focus();
                            }
                            else if (metroDateTimeManufacturingDate.Visible.ToString() == "True" && metroTextBoxBatchOrLotNumber.Visible.ToString() == "False")
                            {
                                string mfg = metroDateTimeManufacturingDate.Value.Date.Day.ToString() + "/" + metroDateTimeManufacturingDate.Value.Date.Month.ToString() + "/" + metroDateTimeManufacturingDate.Value.Date.Year.ToString();
                                string exp = metroDateTimeExpiryDate.Value.Date.Day.ToString() + "/" + metroDateTimeExpiryDate.Value.Date.Month.ToString() + "/" + metroDateTimeExpiryDate.Value.Date.Year.ToString();
                                dataGridView1.Rows.Add(d["id"].ToString(), d["name"].ToString(), "1", d["Purchase_Price"].ToString(), "", mfg, exp, "", "X");
                                hideMfgExpDate();
                                hideBatchOrLotNumber();
                                metroTextBoxItemName.Text = string.Empty;
                                metroTextBoxItemName.Focus();
                            }
                            else
                            {
                                if (metroTextBoxBatchOrLotNumber.Text.Trim().ToString() == "")
                                {
                                    MessageBox.Show("Enter Batch/Lot Number");
                                    metroTextBoxBatchOrLotNumber.Focus();
                                    return;
                                }
                                dataGridView1.Rows.Add(d["id"].ToString(), d["name"].ToString(), "1", d["Purchase_Price"].ToString(), "", "", "", metroTextBoxBatchOrLotNumber.Text.Trim().ToString(), "X");
                                hideMfgExpDate();
                                hideBatchOrLotNumber();
                                metroTextBoxItemName.Text = string.Empty;
                                metroTextBoxItemName.Focus();
                            }
                        }
                        calculateTotalAmount();
                        calculateDiscountAmount();
                        calculatePurchaseTax();
                        calculateServiceCharges();
                        calculatePurchaseTotal();
                        calculatePaymentDue();
                    }
                    dataGridView1.ClearSelection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }
    }
}
