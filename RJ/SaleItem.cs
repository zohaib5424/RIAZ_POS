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
    public partial class SaleItem : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public SaleItem()
        {
            InitializeComponent();
        }

        AutoCompleteStringCollection source = new AutoCompleteStringCollection();
        AutoCompleteStringCollection sourceCustomer_Or_Vendor = new AutoCompleteStringCollection();
        AutoCompleteStringCollection main_sourceCustomer_Or_Vendor = new AutoCompleteStringCollection();
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
                    metroComboBoxTax.Items.Add(d["Tax_Name"].ToString() + " @ " + d["Tax_Percent"].ToString());
                }
                metroComboBoxTax.SelectedIndex = 0;

                metroComboBoxTax.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

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
                string billid = "S" + metroDateTime1.Value.Date.Day.ToString() + metroDateTime1.Value.Date.Month.ToString() + metroDateTime1.Value.Date.Year.ToString() + "XX";
                metroLabelTransactionId.Text = billid;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        DataTable dtitems = new DataTable();
        private void ItemsList_Load(object sender, EventArgs e)
        {
            comboBox_MainCustomer.Focus();
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            if (RJ.Properties.Settings.Default.datestatus == "1")
            {
                metroDateTime1.Enabled = true;
            }
            else
            {
                metroDateTime1.Enabled = false;
            }
            dataGridView1.Columns[10].DisplayIndex = 0;

            metroLabel2.Visible = false;
            labelPurchaseTotal.Visible = false;
            metroLabel3.Visible = false;
            labelPaymentDue.Visible = false;
            metroTextBox_main_CustomerContact.Enabled = false;
            metroTextBox_main_CustomerAddress.Enabled = false;
            metroTextBoxVendorContactNumber.Enabled = false;
            metroTextBoxVendorAddress.Enabled = false;
            try
            {
                metroRadioButton1.Checked = true;
                numericUpDown_Quantity.Minimum = 1;

                metroPanel2.Enabled = false;
                source = new AutoCompleteStringCollection();
                sourceCustomer_Or_Vendor = new AutoCompleteStringCollection();
                main_sourceCustomer_Or_Vendor = new AutoCompleteStringCollection();
                this.AutoScroll = true;

                getTaxes();
                comboBox_ItemName.Focus();
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


                //dataGridView1.Columns[0].ReadOnly = false;
                //dataGridView1.Columns[1].ReadOnly = true;
                //dataGridView1.Columns[2].ReadOnly = false;
                //dataGridView1.Columns[3].ReadOnly = false;
                //dataGridView1.Columns[4].ReadOnly = true;
                //dataGridView1.Columns[5].ReadOnly = false;

                dtitems = gm.GetTable("select * from items where status ='1'");
                //string query = "select name+\' (\'+id+\')\' from items where status ='1'";
                DataTable dt = new DataTable();
                dt.Columns.Add("id");
                dt.Columns.Add("name");
                foreach (DataRow d in dtitems.Rows)
                {
                    int exist = 0;
                    foreach (DataRow d2 in dt.Rows)
                    {
                        if (d2[1].ToString() == d[2].ToString())
                        {
                            exist = 1;
                        }
                    }
                    if (exist == 0)
                    {
                        dt.Rows.Add(d["id"].ToString(), d["name"].ToString());
                    }
                }

                comboBox_ItemName.DisplayMember = "name";
                comboBox_ItemName.ValueMember = "id";
                comboBox_ItemName.DataSource = dt;
                comboBox_ItemName.AutoCompleteMode = AutoCompleteMode.Suggest;
                comboBox_ItemName.AutoCompleteSource = AutoCompleteSource.ListItems;
                comboBox_ItemName.SelectedIndex = -1;

                dt = gm.GetTable("select * from customer_or_vendor where status ='1' and customer_vendor_type='Customer' and parent!=''");
                foreach (DataRow d in dt.Rows)
                {
                    sourceCustomer_Or_Vendor.Add(d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")");
                }

                //metroTextBoxVendorName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                //metroTextBoxVendorName.AutoCompleteCustomSource = sourceCustomer_Or_Vendor;
                //metroTextBoxVendorName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                comboBox_SubCustomer.DisplayMember = "customer_Or_Vendor_Name";
                comboBox_SubCustomer.ValueMember = "id";
                comboBox_SubCustomer.DataSource = dt;
                comboBox_SubCustomer.AutoCompleteMode = AutoCompleteMode.Suggest;
                comboBox_SubCustomer.AutoCompleteSource = AutoCompleteSource.ListItems;
                comboBox_SubCustomer.SelectedIndex = -1;

                dt = gm.GetTable("select * from customer_or_vendor where status ='1' and customer_vendor_type='Customer' and parent=''");
                foreach (DataRow d in dt.Rows)
                {
                    main_sourceCustomer_Or_Vendor.Add(d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")");
                }

                //metroTextBox_main_CustomerName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                //metroTextBox_main_CustomerName.AutoCompleteCustomSource = main_sourceCustomer_Or_Vendor;
                //metroTextBox_main_CustomerName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                comboBox_MainCustomer.DisplayMember = "customer_Or_Vendor_Name";
                comboBox_MainCustomer.ValueMember = "id";
                comboBox_MainCustomer.DataSource = dt;
                comboBox_MainCustomer.AutoCompleteMode = AutoCompleteMode.Suggest;
                comboBox_MainCustomer.AutoCompleteSource = AutoCompleteSource.ListItems;
                comboBox_MainCustomer.SelectedIndex = -1;

                comboBox_ItemName.Focus();
                getTransactionId();
                numericUpDown_Quantity.Value = 1;
                //numericUpDown_Quantity.Focus();
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
            metroTextBoxPaidAmount.Text = "0";
            metroTextBox_main_CustomerContact.Text = "";
            metroTextBox_main_CustomerAddress.Text = "";
            comboBox_MainCustomer.Focus();
            metroPanel3.Focus();
            try
            {
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    c.DefaultCellStyle.Font = new Font("Jameel Noori Nastaleeq", 19.5F, GraphicsUnit.Pixel);
                }
            }
            catch { }
        }

        GMDB gm = new GMDB();
        public void getItemNameRecord(string id)
        {
            dataGridView1.ClearSelection();
            try
            {
                double qty = 0;
                string query = "Select * from items where id='" + id + "' and status = '1'";
                DataTable dt = gm.GetTable(query);
                string unit = "Other";
                string brand = "Other";
                int item_rows = 0;
                foreach (DataRow d in dt.Rows)
                {
                    try
                    {
                        unit = d["Unit_Id"].ToString();
                        if (d["unit_id"].ToString() != "Other")
                        {
                            query = "Select * from units where id='" + d["unit_id"].ToString() + "'";
                            DataTable dtunit = gm.GetTable(query);
                            unit = dtunit.Rows[0][1].ToString();
                        }
                    }
                    catch { }
                    try
                    {
                        unit = d["brand_Id"].ToString();
                        if (d["brand_id"].ToString() != "Other")
                        {
                            query = "Select * from brands where id='" + d["brand_id"].ToString() + "'";
                            DataTable dtbrand = gm.GetTable(query);
                            brand = dtbrand.Rows[0][1].ToString();
                        }
                    }
                    catch { }
                    int exist = 0;
                    try
                    {
                        qty += double.Parse(numericUpDown_Quantity.Value.ToString());
                    }
                    catch { }
                    foreach (DataGridViewRow r in dataGridView1.Rows)
                    {
                        if (r.Cells[0].Value.ToString() == d["id"].ToString() && metroRadioButton_Kharab.Checked != true && r.Cells[2].Value.ToString() != metroRadioButton_Kharab.Text.ToString() && metroRadioButton_Sabqa.Checked != true && r.Cells[2].Value.ToString() != metroRadioButton_Sabqa.Text.ToString() && metroRadioButton_BillMein.Checked != true && r.Cells[2].Value.ToString() != metroRadioButton_BillMein.Text.ToString())
                        //if (r.Cells[0].Value.ToString() == d["id"].ToString() && metroRadioButton_Sabqa.Checked != true && r.Cells[2].Value.ToString() != metroRadioButton_Sabqa.Text.ToString() && metroRadioButton_BillMein.Checked != true && r.Cells[2].Value.ToString() != metroRadioButton_BillMein.Text.ToString())
                        {
                            //if (r.Cells[2].Value.ToString() == metroRadioButton_Kharab.Text.ToString())
                            {
                                try
                                {
                                    qty = double.Parse(r.Cells[5].Value.ToString());
                                }
                                catch { }
                                qty += int.Parse(numericUpDown_Quantity.Value.ToString());
                                r.Cells[5].Value = qty.ToString();
                                r.Cells[7].Value = d["mazdori"].ToString();
                                exist = 1;
                                comboBox_ItemName.Text = string.Empty;
                                comboBox_ItemName.SelectedIndex = -1;
                                comboBox_ItemName.Focus();
                            }
                        }
                        else
                        {
                            //if (metroRadioButton_Kharab.Checked == true)
                            //{
                            //    foreach (DataGridViewRow r2 in dataGridView1.Rows)
                            //    {
                            //        if (r2.Cells[0].Value.ToString() == d["id"].ToString() && r2.Cells[2].Value.ToString() == metroRadioButton_Kharab.Text.ToString())
                            //        {
                            //            item_rows++;
                            //        }
                            //    }
                            //}
                            //if (metroRadioButton_Sabqa.Checked == true)
                            //{
                            //    foreach (DataGridViewRow r2 in dataGridView1.Rows)
                            //    {
                            //        if (r2.Cells[0].Value.ToString() == d["id"].ToString() && r2.Cells[2].Value.ToString() == metroRadioButton_Sabqa.Text.ToString())
                            //        {
                            //            item_rows++;
                            //        }
                            //    }
                            //}
                        }
                    }
                    string type = "نارمل";
                    if (metroRadioButton_Sabqa.Checked == true)
                    {
                        type = metroRadioButton_Sabqa.Text.ToString();
                    }
                    if (metroRadioButton_Kharab.Checked == true)
                    {
                        type = metroRadioButton_Kharab.Text.ToString();
                    }
                    if (metroRadioButton_BillMein.Checked == true)
                    {
                        type = metroRadioButton_BillMein.Text.ToString();
                    }
                    if (exist == 0)
                    {
                        //if (metroRadioButton_Kharab.Checked == true)
                        //{
                        //    if (item_rows <= 0)
                        //    {
                        //        dataGridView1.Rows.Add(d["id"].ToString(), d["name"].ToString(), d["weight"].ToString(), unit, qty.ToString(), "-" + d["Retail_Price"].ToString(), "", "X", type);
                        //    }
                        //}
                        //if (metroRadioButton_Sabqa.Checked == true)
                        //{
                        //    if (item_rows <= 0)
                        //    {
                        //        dataGridView1.Rows.Add(d["id"].ToString(), d["name"].ToString(), d["weight"].ToString(), unit, qty.ToString(), "-" + "0", "", "X", type);
                        //    }
                        //}
                        if (metroRadioButton_Kharab.Checked == true)
                        {
                            item_rows = 0;
                            foreach (DataGridViewRow r2 in dataGridView1.Rows)
                            {
                                if (r2.Cells[0].Value.ToString() == d["id"].ToString() && r2.Cells[2].Value.ToString() == metroRadioButton_Kharab.Text.ToString())
                                {
                                    try
                                    {
                                        qty = int.Parse(r2.Cells[5].Value.ToString());
                                    }
                                    catch { }
                                    qty += int.Parse(numericUpDown_Quantity.Value.ToString());
                                    r2.Cells[5].Value = qty.ToString();
                                    item_rows++;
                                    //item_rows++;
                                }
                            }
                            if (item_rows <= 0)
                            {
                                dataGridView1.Rows.Add(d["id"].ToString(), d["name"].ToString(), type, brand, unit, gm.removePoints(qty.ToString()), "-" + gm.removePoints(d["Retail_Price"].ToString()), "0", "", "X", "");
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
                            }
                        }
                        else if (metroRadioButton_Sabqa.Checked == true)
                        {
                            item_rows = 0;
                            foreach (DataGridViewRow r2 in dataGridView1.Rows)
                            {
                                if (r2.Cells[0].Value.ToString() == d["id"].ToString() && r2.Cells[2].Value.ToString() == metroRadioButton_Sabqa.Text.ToString())
                                {
                                    try
                                    {
                                        qty = int.Parse(r2.Cells[5].Value.ToString());
                                    }
                                    catch { }
                                    qty += int.Parse(numericUpDown_Quantity.Value.ToString());
                                    r2.Cells[5].Value = qty.ToString();
                                    item_rows++;
                                }
                            }
                            if (item_rows <= 0)
                            {
                                dataGridView1.Rows.Add(d["id"].ToString(), d["name"].ToString(), type, brand, unit, gm.removePoints(qty.ToString()), "0", "0", "", "X", "");
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
                            }
                        }
                        else if (metroRadioButton_BillMein.Checked == true)
                        {
                            item_rows = 0;
                            foreach (DataGridViewRow r2 in dataGridView1.Rows)
                            {
                                if (r2.Cells[0].Value.ToString() == d["id"].ToString() && r2.Cells[2].Value.ToString() == metroRadioButton_BillMein.Text.ToString())
                                {
                                    try
                                    {
                                        qty = double.Parse(r2.Cells[5].Value.ToString());
                                    }
                                    catch { }
                                    qty += int.Parse(numericUpDown_Quantity.Value.ToString());
                                    r2.Cells[5].Value = qty.ToString();
                                    r2.Cells[7].Value = d["mazdori"].ToString();
                                    item_rows++;
                                }
                            }
                            if (item_rows <= 0)
                            {
                                dataGridView1.Rows.Add(d["id"].ToString(), d["name"].ToString(), type, brand, unit, gm.removePoints(qty.ToString()), gm.removePoints(d["Retail_Price"].ToString()), gm.removePoints(d["mazdori"].ToString()), "", "X", "");
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
                            }
                        }
                        else
                        {
                            if (item_rows <= 0)
                            {
                                dataGridView1.Rows.Add(d["id"].ToString(), d["name"].ToString(), type, brand, unit, gm.removePoints(qty.ToString()), gm.removePoints(d["Retail_Price"].ToString()), gm.removePoints(d["mazdori"].ToString()), "", "X", "");
                                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
                            }
                        }
                    }
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[9].Style.BackColor = Color.Red;
                    calculateTotalAmount();
                    calculateDiscountAmount();
                    calculatePurchaseTax();
                    calculateServiceCharges();
                    calculatePurchaseTotal();
                    calculatePaymentDue();
                }
                //dataGridView1.ClearSelection();
                numericUpDown_Quantity.Focus();
                numericUpDown_Quantity.Value = 1;
                metroRadioButton1.Checked = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            try
            {
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    c.DefaultCellStyle.Font = new Font("Jameel Noori Nastaleeq", 19.5F, GraphicsUnit.Pixel);
                }
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Height = 32;
                }
            }
            catch { }
            try
            {
                int i = 1;
                foreach (DataGridViewRow r2 in dataGridView1.Rows)
                {
                    r2.Cells[10].Value = i.ToString();
                    i++;
                }
            }
            catch { }
            comboBox_ItemName.Focus();
            SendKeys.Send("{BACKSPACE}");
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //dataGridView1.Rows[e.RowIndex].Selected = true;
                if (e.ColumnIndex == 9)
                {
                    dataGridView1.Rows.RemoveAt(e.RowIndex);
                    calculateTotalAmount();
                    calculateDiscountAmount();
                    calculatePurchaseTax();
                    calculateServiceCharges();
                    calculatePurchaseTotal();
                    calculatePaymentDue();
                    numericUpDown_Quantity.Value = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            try
            {
                int i = 1;
                foreach (DataGridViewRow r2 in dataGridView1.Rows)
                {
                    r2.Cells[10].Value = i.ToString();
                    i++;
                }
            }
            catch { }
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
                double totalqty = 0;
                double totalweight = 0;
                foreach (DataGridViewRow r in dataGridView1.Rows)
                {
                    double qty = 0;
                    double ratePerUnit = 0;
                    double weight = 0;
                    double mazdori = 0;
                    try
                    {
                        qty = double.Parse(r.Cells[5].Value.ToString());
                    }
                    catch { }
                    try
                    {
                        ratePerUnit = double.Parse(r.Cells[6].Value.ToString());
                    }
                    catch { }
                    try
                    {
                        weight = double.Parse(r.Cells[3].Value.ToString());
                    }
                    catch { }
                    try
                    {
                        mazdori = double.Parse(r.Cells[7].Value.ToString().Trim());
                    }
                    catch { }
                    double totalofitem = (Math.Round((qty * (ratePerUnit + mazdori)), 0, MidpointRounding.AwayFromZero));
                    r.Cells[8].Value = gm.removePoints(totalofitem.ToString());
                    try
                    {
                        total += double.Parse(r.Cells[8].Value.ToString());
                    } catch { }
                    try
                    {
                        totalqty += qty;
                    }
                    catch { }
                    try
                    {
                        if (r.Cells[2].Value.ToString() == "نارمل" || r.Cells[2].Value.ToString() == "بل میں")
                        {
                            totalweight += (qty * weight);
                        }
                    }
                    catch { }
                    labelNetTotal.Text = gm.removePoints(total.ToString());
                    label_Total_Qty.Text = gm.removePoints(totalqty.ToString());
                    label_Total_Weight.Text = gm.removePoints(totalweight.ToString());
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
                    if (metroComboBoxDiscountType.SelectedItem.ToString() == "Fixed")
                    {
                        discountAmount = discount;
                    }
                    else if (metroComboBoxDiscountType.SelectedItem.ToString() == "Percentage")
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
                    purchaseTax = (((netTotal - discountAmount) * tax) / 100);
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
                    serviceCharges = service;
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
                    purchaseTotal = (((netTotal - discountAmount) + purchaseTax) + serviceCharges);
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
                if (comboBox_ItemName.Text.Trim().ToString() != "" && comboBox_ItemName.SelectedValue != null)
                {
                    string query = "select * from items where (sku='" + comboBox_ItemName.Text.Trim().ToString() + "' or barcode='" + comboBox_ItemName.Text.Trim().ToString() + "') and status='1'";
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
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    if (e.ColumnIndex == 5)
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
                            if (a <= 0)
                                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "1";
                        }
                        catch
                        {
                            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "1";
                        }
                    }
                    if (e.ColumnIndex == 4)
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
                    try
                    {
                        if (dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() == metroRadioButton_Kharab.Text.ToString())
                        {
                            if (double.Parse(dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString()) > 0)
                            {
                                dataGridView1.Rows[e.RowIndex].Cells[6].Value = "-" + dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                            }
                            //if (double.Parse(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString()) > 1 || double.Parse(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString()) <=0)
                            //{
                            //    dataGridView1.Rows[e.RowIndex].Cells[5].Value = "1";
                            //}
                        }
                    }
                    catch { }
                    try
                    {
                        if (dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() == metroRadioButton_Sabqa.Text.ToString())
                        {
                            if (double.Parse(dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString()) > 0)
                            {
                                dataGridView1.Rows[e.RowIndex].Cells[6].Value = "0";
                            }
                        }
                    }
                    catch { }
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                }
                calculateTotalAmount();
                calculateDiscountAmount();
                calculatePurchaseTax();
                calculateServiceCharges();
                calculatePurchaseTotal();
                calculatePaymentDue();
            }
            catch { }
        }

        public string endproductitemlistid = "";
        public string endproductitemlistitem = "";
        private void metroTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Space)
            {
                metroRadioButton1.Checked = true;
                metroRadioButton1.Focus();
                return;
            }
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    try
                    {
                        if (comboBox_MainCustomer.Text.Trim().ToString() != "")
                        {
                            if (comboBox_MainCustomer.SelectedValue == null)
                            {
                                comboBox_ItemName.Focus();
                                comboBox_ItemName.SelectedIndex = -1;
                                comboBox_ItemName.Text = "";
                                MessageBox.Show("Select Customer");
                                comboBox_MainCustomer.Focus();
                                return;
                            }
                            string vendorid = comboBox_MainCustomer.SelectedValue.ToString();
                            string query = "select * from customer_or_vendor where id='" + vendorid + "'";
                            DataTable dt2 = gm.GetTable(query);
                            if (dt2.Rows.Count > 0)
                            {

                            }
                            else
                            {
                                comboBox_ItemName.Focus();
                                comboBox_ItemName.SelectedIndex = -1;
                                comboBox_ItemName.Text = "";
                                MessageBox.Show("Customer Not Registered");
                                comboBox_MainCustomer.Focus();
                                return;
                            }
                        }
                        else
                        {
                            comboBox_ItemName.Focus();
                            comboBox_ItemName.SelectedIndex = -1;
                            comboBox_ItemName.Text = "";
                            MessageBox.Show("Enter Customer Name");
                            comboBox_MainCustomer.Focus();
                            return;
                        }

                        if (comboBox_SubCustomer.Text.Trim().ToString() != "")
                        {
                            if (comboBox_SubCustomer.SelectedValue == null)
                            {
                                comboBox_ItemName.Focus();
                                comboBox_ItemName.SelectedIndex = -1;
                                comboBox_ItemName.Text = "";
                                MessageBox.Show("Select Sub Customer");
                                comboBox_SubCustomer.Focus();
                                return;
                            }
                            string vendorid = comboBox_SubCustomer.SelectedValue.ToString();
                            string query = "select * from customer_or_vendor where id='" + vendorid + "'";
                            DataTable dt2 = gm.GetTable(query);
                            if (dt2.Rows.Count > 0)
                            {

                            }
                            else
                            {
                                comboBox_ItemName.Focus();
                                comboBox_ItemName.SelectedIndex = -1;
                                comboBox_ItemName.Text = "";
                                MessageBox.Show("Sub Customer Not Registered");
                                comboBox_SubCustomer.Focus();
                                return;
                            }
                        }
                        else
                        {
                            comboBox_ItemName.Focus();
                            comboBox_ItemName.SelectedIndex = -1;
                            comboBox_ItemName.Text = "";
                            MessageBox.Show("Enter Sub Customer");
                            comboBox_SubCustomer.Focus();
                            return;
                        }
                    }
                    catch { }

                    try
                    {
                        //string[] s = metroTextBoxItemName.Text.Trim().Split('(');
                        //string[] a = s[s.Length - 1].Trim().Split(')');
                        //string id = a[0].ToString();
                        string id = comboBox_ItemName.SelectedValue.ToString();
                        if (id.Trim() != "")
                        {
                            if (e.KeyCode == Keys.Enter)
                            {
                                endproductitemlistid = "";
                                endproductitemlistitem = "";
                                ItemSelection epl = new ItemSelection();
                                epl.itemname = comboBox_ItemName.SelectedItem.ToString();
                                epl.ShowDialog();
                                endproductitemlistid = epl.selecteditemid;
                                endproductitemlistitem = epl.selecteditemname;
                                int index = 0;
                                foreach (object item in comboBox_ItemName.ValueMember)
                                {
                                    DataRowView row = item as DataRowView;
                                    if (row["id"].ToString() == endproductitemlistid)
                                    {
                                        comboBox_ItemName.SelectedIndex = index;
                                        //comboBox_ItemName.Text = endproductitemlistitem + " (" + endproductitemlistid + ")";
                                    }
                                    index++;
                                }

                                id = endproductitemlistid;
                            }

                            getItemNameRecord(id);
                            //comboBox_ItemName.Focus();
                            //SendKeys.Send("{BACKSPACE}");
                        }
                        else
                        {
                            comboBox_ItemName.Text = string.Empty;
                            comboBox_ItemName.SelectedIndex = -1;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        comboBox_ItemName.Text = string.Empty;
                        comboBox_ItemName.SelectedIndex = -1;
                    }
                    comboBox_ItemName.Text = string.Empty;
                    comboBox_ItemName.SelectedIndex = -1;
                    metroRadioButton_Sabqa.Checked = false;
                    metroRadioButton_BillMein.Checked = false;
                    metroRadioButton_Kharab.Checked = false;
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
                escape = 0;
                //qtyenter = 0;
                int maincustomerindex = comboBox_MainCustomer.SelectedIndex;
                try
                {
                    metroRadioButton1.Checked = true;
                    if (comboBox_MainCustomer.Text.Length <= 0 && comboBox_MainCustomer.SelectedValue == null)
                    {
                        metroPanel2.Enabled = false;
                    }
                    source = new AutoCompleteStringCollection();

                    getTaxes();
                    int i = metroComboBoxDiscountType.Items.IndexOf("Fixed");
                    metroComboBoxDiscountType.SelectedIndex = i;

                    metroComboBoxPaymentMethod.SelectedIndex = 0;

                    metroComboBoxCardType.SelectedIndex = -1;

                    metroComboBoxExpiryMonth.SelectedIndex = -1;

                    metroComboBoxExpiryYear.SelectedIndex = -1;

                    sourceitemname = new AutoCompleteStringCollection();
                    dtitems = gm.GetTable("select * from items where status ='1'");
                    //foreach (DataRow d in dt.Rows)
                    //{
                    //    sourceitemname.Add(d["name"].ToString() + " (" + d["id"].ToString() + ")");
                    //}

                    comboBox_ItemName.DisplayMember = "name";
                    comboBox_ItemName.ValueMember = "id";
                    comboBox_ItemName.DataSource = dtitems;
                    comboBox_ItemName.AutoCompleteMode = AutoCompleteMode.Suggest;
                    comboBox_ItemName.AutoCompleteSource = AutoCompleteSource.ListItems;
                    comboBox_ItemName.SelectedIndex = -1;

                    sourceCustomer_Or_Vendor = new AutoCompleteStringCollection();
                    //string[] s = metroTextBox_main_CustomerName.Text.Trim().ToString().Split('(');
                    //string customerid = s[s.Length - 1].ToString().Trim().Split(')')[0].ToString();
                    string customerid = comboBox_MainCustomer.SelectedValue.ToString();
                    metroPanel2.Enabled = true;
                    sourceCustomer_Or_Vendor = new AutoCompleteStringCollection();
                    DataTable dt = gm.GetTable("select * from customer_or_vendor where status ='1' and customer_vendor_type='Customer' and parent!='' and parent='" + customerid + "'");
                    foreach (DataRow d in dt.Rows)
                    {
                        sourceCustomer_Or_Vendor.Add(d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")");
                    }

                    //metroTextBoxVendorName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    //metroTextBoxVendorName.AutoCompleteCustomSource = sourceCustomer_Or_Vendor;
                    //metroTextBoxVendorName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    comboBox_SubCustomer.DisplayMember = "customer_Or_Vendor_Name";
                    comboBox_SubCustomer.ValueMember = "id";
                    comboBox_SubCustomer.DataSource = dt;
                    comboBox_SubCustomer.AutoCompleteMode = AutoCompleteMode.Suggest;
                    comboBox_SubCustomer.AutoCompleteSource = AutoCompleteSource.ListItems;
                    comboBox_SubCustomer.SelectedIndex = -1;


                    main_sourceCustomer_Or_Vendor = new AutoCompleteStringCollection();
                    dt = gm.GetTable("select * from customer_or_vendor where status ='1' and customer_vendor_type='Customer' and parent=''");
                    foreach (DataRow d in dt.Rows)
                    {
                        main_sourceCustomer_Or_Vendor.Add(d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")");
                    }

                    //metroTextBox_main_CustomerName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    //metroTextBox_main_CustomerName.AutoCompleteCustomSource = main_sourceCustomer_Or_Vendor;
                    //metroTextBox_main_CustomerName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    comboBox_MainCustomer.DisplayMember = "customer_Or_Vendor_Name";
                    comboBox_MainCustomer.ValueMember = "id";
                    comboBox_MainCustomer.DataSource = dt;
                    comboBox_MainCustomer.AutoCompleteMode = AutoCompleteMode.Suggest;
                    comboBox_MainCustomer.AutoCompleteSource = AutoCompleteSource.ListItems;
                    //comboBox_MainCustomer.SelectedIndex = -1;

                    comboBox_ItemName.Focus();

                    comboBox_ItemName.Text = string.Empty;
                    comboBox_ItemName.SelectedIndex = -1;
                    if (dataGridView1.Rows.Count > 0)
                        dataGridView1.Rows.Clear();
                    metroTextBoxDiscount.Text = string.Empty;
                    metroTextBoxServiceCharges.Text = string.Empty;
                    metroTextBoxPaidAmount.Text = string.Empty;
                    //metroTextBoxVendorName.Text = string.Empty;
                    comboBox_SubCustomer.Text = "";
                    comboBox_SubCustomer.SelectedIndex = -1;
                    metroTextBoxVendorContactNumber.Text = string.Empty;
                    metroTextBoxVendorAddress.Text = string.Empty;

                    //metroTextBox_main_CustomerName.Text = string.Empty;
                    //metroTextBox_main_CustomerContact.Text = string.Empty;
                    //metroTextBox_main_CustomerAddress.Text = string.Empty;

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
                    label_Total_Qty.Text = "0";
                    label_Total_Weight.Text = "0";
                    labelPurchaseTotal.Text = "0";
                    labelPaymentDue.Text = "0";
                    calculateTotalAmount();
                    calculateDiscountAmount();
                    calculatePurchaseTax();
                    calculateServiceCharges();
                    calculatePurchaseTotal();
                    calculatePaymentDue();
                    getTransactionId();

                    try
                    {
                        metroComboBoxDiscountType.SelectedIndex = 0;
                        metroComboBoxTax.SelectedIndex = 0;
                        metroComboBoxPaymentMethod.SelectedIndex = 0;
                    }
                    catch { }
                    numericUpDown_Quantity.Value = 1;
                    //numericUpDown_Quantity.Focus();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message); 
                }
                metroTextBoxPaidAmount.Text = "0";
                comboBox_MainCustomer.SelectedIndex = maincustomerindex;
                comboBox_SubCustomer.Focus();
            }
            catch { }
        }

        private void PurchaseItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    if (MessageBox.Show("Confirmation.\nAre You Sure To Close Form?\n Items List Will Not Be Saved. (yes / no)", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.Dispose(true);
                    }
                }
            }
            if (e.KeyCode == Keys.F1)
                comboBox_ItemName.Focus();
            if (e.KeyCode == Keys.F3)
                comboBox_MainCustomer.Focus();
            if (e.KeyCode == Keys.F4)
                comboBox_SubCustomer.Focus();
            if (e.KeyCode == Keys.F5)
                reset_fields();
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Up)
                numericUpDown_Quantity.Value++;
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Down)
            {
                if (numericUpDown_Quantity.Value > 1)
                {
                    numericUpDown_Quantity.Value--;
                }
            }
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.S)
                metroTile5.PerformClick();
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Space)
            {
                metroRadioButton1.Checked = true;
            }
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
                //if (metroTextBoxPaidAmount.Text.Trim().ToString () == "")
                //{
                //    MessageBox.Show("Enter Paid Amount To Complete This Transaction");
                //    return;
                //}

                //if (metroComboBoxPaymentMethod.SelectedIndex<0)
                //{
                //    MessageBox.Show("Select Payment Method");
                //    return;
                //}

                //if (metroComboBoxPaymentMethod.SelectedItem.ToString() == "Card")
                //{
                //    if (metroTextBoxCardHolderName.Text.Trim().ToString() == "")
                //    {
                //        MessageBox.Show("Enter Card Holder Name");
                //        return;
                //    }
                //    if (metroTextBoxCardNumber.Text.Trim().ToString() == "")
                //    {
                //        MessageBox.Show("Enter Card Number");
                //        return;
                //    }
                //    if (metroTextBoxCardTransactionNo.Text.Trim().ToString() == "")
                //    {
                //        MessageBox.Show("Enter Card Transaction No");
                //        return;
                //    }
                //    if (metroComboBoxCardType.SelectedIndex< 0)
                //    {
                //        MessageBox.Show("Select Card Type");
                //        return;
                //    }
                //    if (metroComboBoxExpiryMonth.SelectedIndex < 0)
                //    {
                //        MessageBox.Show("Select Card Expiry Month");
                //        return;
                //    }
                //    if (metroComboBoxExpiryYear.SelectedIndex < 0)
                //    {
                //        MessageBox.Show("Select Card Expiry Year");
                //        return;
                //    }
                //    if (metroTextBoxSecurityCode.Text.Trim().ToString() == "")
                //    {
                //        MessageBox.Show("Enter Security Code");
                //        return;
                //    }
                //}
                //else if (metroComboBoxPaymentMethod.SelectedItem.ToString() == "Cheque")
                //{
                //    if (metroTextBoxChequeNumber.Text.Trim().ToString() == "")
                //    {
                //        MessageBox.Show("Enter Cheque Number");
                //        return;
                //    }
                //}
                //else if (metroComboBoxPaymentMethod.SelectedItem.ToString() == "Bank Transfer")
                //{
                //    if (metroTextBoxBankName.Text.Trim().ToString() == "")
                //    {
                //        MessageBox.Show("Enter Bank Name");
                //        return;
                //    }
                //    if (metroTextBoxBankAccountNumber.Text.Trim().ToString() == "")
                //    {
                //        MessageBox.Show("Enter Bank Account Number");
                //        return;
                //    }
                //}

                metroTextBoxPaidAmount.Text = "0";
                string query = "";
                string customer_or_vendor_id = "";
                if (comboBox_SubCustomer.Text.Trim().ToString() != "")
                {
                    if (comboBox_SubCustomer.SelectedValue == null)
                    {
                        MessageBox.Show("Sub Customer Not Registered");
                        return;
                    }
                    string vendorid = comboBox_SubCustomer.SelectedValue.ToString();
                    query = "select * from customer_or_vendor where id='" + vendorid + "'";
                    DataTable dt2 = gm.GetTable(query);
                    if (dt2.Rows.Count > 0)
                        customer_or_vendor_id = vendorid;
                    else
                    {
                        MessageBox.Show("Sub Customer Not Registered");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Enter Sub Customer");
                    return;
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

                //string today_date = DateTime.Now.Date.Year.ToString() +"-"+DateTime.Now.Date.Month.ToString() +"-"+DateTime.Now.Date.Day.ToString();
                string today_date = metroDateTime1.Value.Date.Year.ToString() + "-" + metroDateTime1.Value.Date.Month.ToString() + "-" + metroDateTime1.Value.Date.Day.ToString();
                query = "select * from bill where Customer_Vendor_Id='" + customer_or_vendor_id + "' and bill_date='" + today_date + "' and status='1'";
                DataTable dt_temp1 = gm.GetTable(query);
                if (dt_temp1.Rows.Count <= 0)
                {

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
                    string billid = "S" + metroDateTime1.Value.Date.Day.ToString() + metroDateTime1.Value.Date.Month.ToString() + metroDateTime1.Value.Date.Year.ToString() + countstring;

                    query = "Select max(cast(id as int)) from bill";
                    string id = gm.MaxId(query);
                    query = @"insert into bill values('" + id + "','" + "Sale Trading" + "','" + billid + "','" + metroDateTime1.Value.Date + "','" + metroDateTime1.Value.ToShortTimeString() + "','" + ((labelNetTotal.Text.Trim() == "") ? "0" : labelNetTotal.Text.Trim()) + "','" + metroComboBoxDiscountType.SelectedItem.ToString() + "','" + ((metroTextBoxDiscount.Text.Trim().ToString() == "") ? "0" : metroTextBoxDiscount.Text.Trim().ToString()) + "','" + ((metroLabDiscountAmount.Text.Trim().ToString() == "") ? "0" : metroLabDiscountAmount.Text.Trim().ToString()) + "',N'" + metroComboBoxTax.SelectedItem.ToString() + "','" + ((metroLabelPurchaseTax.Text.Trim().ToString() == "") ? "0" : metroLabelPurchaseTax.Text.Trim().ToString()) + "','" + ((metroLabelServiceCharges.Text.Trim().ToString() == "") ? "0" : metroLabelServiceCharges.Text.Trim().ToString()) + "','" + "" + "','" + "0" + "','" + ((labelPurchaseTotal.Text.Trim().ToString() == "") ? "0" : labelPurchaseTotal.Text.Trim().ToString()) + "','" + ((metroTextBoxPaidAmount.Text.Trim().ToString() == "") ? "0" : metroTextBoxPaidAmount.Text.Trim().ToString()) + "','" + ((labelPaymentDue.Text.Trim().ToString() == "") ? "0" : labelPaymentDue.Text.Trim().ToString()) + "','" + "" + "','" + customer_or_vendor_id + "',N'" + metroComboBoxPaymentMethod.SelectedItem.ToString() + "',N'" + cardnumber + "',N'" + cardholdername + "','" + cardtransactionid + "',N'" + cardtype + "','" + cardexpirymonth + "','" + cardexpiryyearh + "','" + cardsecuritycode + "',N'" + chequenumber + "',N'" + bankname + "',N'" + bankaccountnumber + "',N'" + paymentnote + "','" + RJ.Properties.Settings.Default.loginid + "','" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.ToShortTimeString() + "','1')";
                    int a = gm.ExecuteNonQuery(query);
                    if (a > 0)
                    {
                        int ok = 0;
                        foreach (DataGridViewRow d in dataGridView1.Rows)
                        {
                            string itemid = d.Cells[0].Value.ToString();
                            string qty = d.Cells[5].Value.ToString();
                            string mazdori = d.Cells[7].Value.ToString();
                            string unitcost = d.Cells[6].Value.ToString();
                            string totalamount = d.Cells[8].Value.ToString();
                            string mfgdate = "";
                            string expdate = "";
                            string type = d.Cells[2].Value.ToString();
                            string batchorlotnum = "";
                            query = "select max(cast(id as int)) from bill_details";
                            string bid = gm.MaxId(query);
                            query = "insert into bill_details values('" + bid + "','" + id + "','" + itemid + "','" + qty + "','" + unitcost + "','" + totalamount + "','" + mfgdate + "','" + expdate + "','" + batchorlotnum + "','1',N'" + type + "','','" + metroDateTime1.Value.Date.ToShortDateString() + "','" + mazdori + "')";
                            int b = gm.ExecuteNonQuery(query);
                            if (b > 0)
                            {
                                query = "select qty from items where id = '" + itemid + "'";
                                DataTable dt3 = gm.GetTable(query);
                                string itemqty = dt3.Rows[0][0].ToString();
                                if (itemqty.Trim() == "")
                                {
                                    itemqty = "0";
                                }
                                try
                                {
                                    itemqty = (double.Parse(itemqty) - double.Parse(qty)).ToString();
                                }
                                catch { }
                                query = "update items set qty='" + itemqty + "' where id = '" + itemid + "'";
                                gm.ExecuteNonQuery(query);
                                ok = 1;
                            }
                        }
                        if (ok == 1)
                        {
                            MessageBox.Show("Transaction Successfully Completed");
                            //if (MessageBox.Show("Transaction Successfully Completed.\nDo you want to print reciept? (yes / no)", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            //{
                            //    string monthName = metroDateTime1.Value.Date.ToString("MMM", CultureInfo.InvariantCulture);
                            //    string date = metroDateTime1.Value.Day.ToString() + " " + monthName + "," + metroDateTime1.Value.Year.ToString();
                            //    gm.printBill(id.Trim(), "Sale Invoice", "Customer", "Original Receipt", date);
                            //}
                            //else
                            //{

                            //}
                            reset_fields();
                        }
                    }
                }
                else//if customer bill already exist of today
                {
                    string bill_id = dt_temp1.Rows[0][0].ToString();
                    query = "delete from bill_details where bill_id='" + bill_id + "'";
                    gm.ExecuteNonQuery(query);

                    query = @"update bill set Bill_Date='" + metroDateTime1.Value.Date.ToShortDateString() + "',Bill_Time='" + DateTime.Now.ToShortTimeString() + "',Net_Total_Amount='" + ((labelNetTotal.Text.Trim() == "") ? "0" : labelNetTotal.Text.Trim()) + "',Discount_Type='" + metroComboBoxDiscountType.SelectedItem.ToString() + "',Discount='" + ((metroTextBoxDiscount.Text.Trim().ToString() == "") ? "0" : metroTextBoxDiscount.Text.Trim().ToString()) + "',Discount_Amount='" + ((metroLabDiscountAmount.Text.Trim().ToString() == "") ? "0" : metroLabDiscountAmount.Text.Trim().ToString()) + "',Tax_Type=N'" + metroComboBoxTax.SelectedItem.ToString() + "',Tax_Amount='" + ((metroLabelPurchaseTax.Text.Trim().ToString() == "") ? "0" : metroLabelPurchaseTax.Text.Trim().ToString()) + "',Service_Charges='" + ((metroLabelServiceCharges.Text.Trim().ToString() == "") ? "0" : metroLabelServiceCharges.Text.Trim().ToString()) + "',Voucher_Or_Coupon_Id='" + "" + "',Vouchar_Coupon_Discount_Amount='" + "0" + "',Total_Amount='" + ((labelPurchaseTotal.Text.Trim().ToString() == "") ? "0" : labelPurchaseTotal.Text.Trim().ToString()) + "',Paid_Amount='" + ((metroTextBoxPaidAmount.Text.Trim().ToString() == "") ? "0" : metroTextBoxPaidAmount.Text.Trim().ToString()) + "',Balance='" + ((labelPaymentDue.Text.Trim().ToString() == "") ? "0" : labelPaymentDue.Text.Trim().ToString()) + "',_Description='" + "" + "',Customer_Vendor_Id='" + customer_or_vendor_id + "',Payment_Method=N'" + metroComboBoxPaymentMethod.SelectedItem.ToString() + "',Card_Number=N'" + cardnumber + "',Card_Holder_Name=N'" + cardholdername + "',Card_Transaction_No='" + cardtransactionid + "',Card_Type=N'" + cardtype + "',Month='" + cardexpirymonth + "',Year='" + cardexpiryyearh + "',Security_Code='" + cardsecuritycode + "',Cheque_No=N'" + chequenumber + "',Bank_Name=N'" + bankname + "',Bank_Account_No=N'" + bankaccountnumber + "',Payment_Note=N'" + paymentnote + "',AddedBy_UserId='" + RJ.Properties.Settings.Default.loginid + "',_Date='" + DateTime.Now.ToShortDateString() + "',_Time='" + DateTime.Now.ToShortTimeString() + "',Status='1' where id='" + bill_id + "'";
                    gm.ExecuteNonQuery(query);
                    //////////change
                    int ok = 0;
                    foreach (DataGridViewRow d in dataGridView1.Rows)
                    {
                        string itemid = d.Cells[0].Value.ToString();
                        string qty = d.Cells[5].Value.ToString();
                        string mazdori = d.Cells[7].Value.ToString();
                        string unitcost = d.Cells[6].Value.ToString();
                        string type = d.Cells[2].Value.ToString();
                        string totalamount = d.Cells[8].Value.ToString();
                        string mfgdate = "";
                        string expdate = "";
                        string batchorlotnum = "";
                        query = "select max(cast(id as int)) from bill_details";
                        string bid = gm.MaxId(query);
                        query = "insert into bill_details values('" + bid + "','" + bill_id + "','" + itemid + "','" + qty + "','" + unitcost + "','" + totalamount + "','" + mfgdate + "','" + expdate + "','" + batchorlotnum + "','1',N'" + type + "','','" + DateTime.Now.ToShortDateString() + "','" + mazdori + "')";
                        int b = gm.ExecuteNonQuery(query);
                        if (b > 0)
                        {
                            query = "select qty from items where id = '" + itemid + "'";
                            DataTable dt3 = gm.GetTable(query);
                            string itemqty = dt3.Rows[0][0].ToString();
                            if (itemqty.Trim() == "")
                            {
                                itemqty = "0";
                            }
                            try
                            {
                                itemqty = (double.Parse(itemqty) - double.Parse(qty)).ToString();
                            }
                            catch { }
                            query = "update items set qty='" + itemqty + "' where id = '" + itemid + "'";
                            gm.ExecuteNonQuery(query);
                            ok = 1;
                        }
                    }
                    if (ok == 1)
                    {
                        MessageBox.Show("Transaction Successfully Completed");
                        //if (MessageBox.Show("Transaction Successfully Completed.\nDo you want to print reciept? (yes / no)", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //{
                        //    string monthName = metroDateTime1.Value.Date.ToString("MMM", CultureInfo.InvariantCulture);
                        //    string date = metroDateTime1.Value.Day.ToString() + " " + monthName + "," + metroDateTime1.Value.Year.ToString();
                        //    gm.printBill(bill_id, "Sale Invoice", "Customer", "Original Receipt", date);
                        //}
                        //else
                        //{

                        //}
                        reset_fields();
                    }
                    //////////change                


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
            if (comboBox_MainCustomer.Text.Trim().ToString() != "")
            {
                if (comboBox_MainCustomer.SelectedValue == null)
                {
                    comboBox_ItemName.Focus();
                    comboBox_ItemName.SelectedIndex = -1;
                    comboBox_ItemName.Text = string.Empty;
                    MessageBox.Show("Select Customer");
                    comboBox_MainCustomer.Focus();
                    return;
                }
                string vendorid = comboBox_MainCustomer.SelectedValue.ToString();
                string query = "select * from customer_or_vendor where id='" + vendorid + "'";
                DataTable dt2 = gm.GetTable(query);
                if (dt2.Rows.Count > 0)
                {

                }
                else
                {
                    MessageBox.Show("Customer Not Registered");
                    comboBox_ItemName.Text = string.Empty;
                    comboBox_ItemName.SelectedIndex = -1;
                    comboBox_MainCustomer.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Enter Customer Name");
                comboBox_ItemName.Text = string.Empty;
                comboBox_ItemName.SelectedIndex = -1;
                comboBox_MainCustomer.Focus();
                return;
            }
            if (comboBox_SubCustomer.Text.Trim().ToString() != "")
            {
                if (comboBox_SubCustomer.SelectedValue == null)
                {
                    MessageBox.Show("Select Sub Customer");
                    comboBox_ItemName.Text = string.Empty;
                    comboBox_ItemName.SelectedIndex = -1;
                    comboBox_SubCustomer.Focus();
                    return;
                }
                string vendorid = comboBox_SubCustomer.SelectedValue.ToString();
                string query = "select * from customer_or_vendor where id='" + vendorid + "'";
                DataTable dt2 = gm.GetTable(query);
                if (dt2.Rows.Count > 0)
                {

                }
                else
                {
                    MessageBox.Show("Sub Customer Not Registered");
                    comboBox_ItemName.Text = string.Empty;
                    comboBox_ItemName.SelectedIndex = -1;
                    comboBox_SubCustomer.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Enter Sub Customer");
                comboBox_ItemName.Text = string.Empty;
                comboBox_ItemName.SelectedIndex = -1;
                comboBox_SubCustomer.Focus();
                return;
            }
            //if (e.KeyCode == Keys.Tab)
            //{
            //    numericUpDown_Quantity.Focus();
            //}
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (comboBox_SubCustomer.Text.Trim().ToString() != "" && comboBox_SubCustomer.SelectedValue != null)
                    {
                        //string[] s = metroTextBoxVendorName.Text.Trim().ToString().Split('(');
                        //string vendorid = s[s.Length - 1].ToString().Trim().Split(')')[0].ToString();
                        string vendorid = comboBox_SubCustomer.SelectedValue.ToString();
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

                                getCustomerTodayBill(comboBox_SubCustomer.SelectedValue.ToString());
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

        public void getCustomerTodayBill(string id)
        {
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                //double qty = 0;
                //string today_date = DateTime.Now.Date.Year.ToString() +"-"+DateTime.Now.Date.Month.ToString() +"-"+DateTime.Now.Date.Day.ToString();
                string today_date = metroDateTime1.Value.Date.Year.ToString() + "-" + metroDateTime1.Value.Date.Month.ToString() + "-" + metroDateTime1.Value.Date.Day.ToString();
                //string to = metroDateTimeTo.Value.Date.Year.ToString() +"-"+metroDateTimeTo.Value.Date.Month.ToString() +"-"+metroDateTimeTo.Value.Date.Day.ToString();
                string query = "Select * from bill where Customer_Vendor_Id='" + id + "' and Bill_Date>='" + today_date + "' and Bill_Date<='" + today_date + "' and status = '1'";
                DataTable dt = gm.GetTable(query);
                //string unit = "Other";
                foreach (DataRow d in dt.Rows)
                {
                    query = "Select * from bill_details where bill_id='" + d["id"].ToString() + "' and status = '1'";
                    DataTable dt2 = gm.GetTable(query);
                    //string unit = "Other";
                    foreach (DataRow d2 in dt2.Rows)
                    {
                        string item_id = d2["item_id"].ToString();
                        query = "Select * from items where id='" + item_id + "' and status = '1'";
                        DataTable dt3 = gm.GetTable(query);
                        string item_name = dt3.Rows[0]["Name"].ToString();
                        string weight = dt3.Rows[0]["weight"].ToString();
                        string unit = dt3.Rows[0]["Unit_Id"].ToString();
                        if (unit != "Other")
                        {
                            query = "Select * from units where id='" + unit + "'";
                            DataTable dtunit = gm.GetTable(query);
                            unit = dtunit.Rows[0][2].ToString();
                        }

                        string qty = d2["Qty"].ToString();
                        string type = d2["item_type"].ToString();
                        string retail_price = d2["Unit_Cost"].ToString();
                        dataGridView1.Rows.Add(item_id, item_name, type, gm.removePoints(weight), unit, gm.removePoints(qty), gm.removePoints(retail_price), gm.removePoints(d2["mazdori"].ToString()), "", "X", "");
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
                        calculateTotalAmount();
                        calculateDiscountAmount();
                        calculatePurchaseTax();
                        calculateServiceCharges();
                        calculatePurchaseTotal();
                        calculatePaymentDue();
                    }

                }
                //    int exist = 0;
                //    qty += int.Parse(numericUpDown_Quantity.Value.ToString());
                //    foreach (DataGridViewRow r in dataGridView1.Rows)
                //    {
                //        try
                //        {
                //            unit = d["Unit_Id"].ToString();
                //            if (d["unit_id"].ToString() != "Other")
                //            {
                //                query = "Select * from units where id='" + d["unit_id"].ToString() + "'";
                //                DataTable dtunit = gm.GetTable(query);
                //                unit = dtunit.Rows[0][1].ToString();
                //            }
                //        }
                //        catch { }
                //        if (r.Cells[0].Value.ToString() == d["id"].ToString())
                //        {
                //            try
                //            {
                //                qty = double.Parse(r.Cells[4].Value.ToString());
                //            }
                //            catch { }
                //            qty += int.Parse(numericUpDown_Quantity.Value.ToString());
                //            r.Cells[4].Value = qty.ToString();
                //            exist = 1;
                //            metroTextBoxItemName.Text = string.Empty;
                //            metroTextBoxItemName.Focus();
                //        }
                //    }
                //    if (exist == 0)
                //        dataGridView1.Rows.Add(d["id"].ToString(), d["name"].ToString(), d["weight"].ToString(), unit, qty.ToString(), d["Retail_Price"].ToString(), "", "X");
                //    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[8].Style.BackColor = Color.Red;
                //    calculateTotalAmount();
                //    calculateDiscountAmount();
                //    calculatePurchaseTax();
                //    calculateServiceCharges();
                //    calculatePurchaseTotal();
                //    calculatePaymentDue();
                //}
                //dataGridView1.ClearSelection();
                //numericUpDown_Quantity.Focus();
                //numericUpDown_Quantity.Value = 1;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            try
            {
                foreach (DataGridViewColumn c in dataGridView1.Columns)
                {
                    c.DefaultCellStyle.Font = new Font("Jameel Noori Nastaleeq", 19.5F, GraphicsUnit.Pixel);
                }
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Height = 32;
                }
            }
            catch { }
            try
            {
                int i = 1;
                foreach (DataGridViewRow r2 in dataGridView1.Rows)
                {
                    r2.Cells[10].Value = i.ToString();
                    i++;
                }
            }
            catch { }
        }

        private void metroDateTime1_ValueChanged(object sender, EventArgs e)
        {
            getTransactionId();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void metroTextBoxVendorName_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (comboBox_MainCustomer.Text.Trim().ToString() != "" && comboBox_MainCustomer.SelectedValue != null)
                    {
                        //string[] s = metroTextBox_main_CustomerName.Text.Trim().ToString().Split('(');
                        //string vendorid = s[s.Length - 1].ToString().Trim().Split(')')[0].ToString();
                        string vendorid = comboBox_MainCustomer.SelectedValue.ToString();
                        string query = "select * from customer_or_vendor where id='" + vendorid + "'";
                        DataTable dt2 = gm.GetTable(query);
                        if (dt2.Rows.Count > 0)
                        {
                            foreach (DataRow d in dt2.Rows)
                            {
                                //metroTextBox_main_CustomerName.Text = d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")";
                                //metroTextBox_main_CustomerName.Tag = d["id"].ToString();
                                metroTextBox_main_CustomerContact.Text = d["contact_number"].ToString();
                                metroTextBox_main_CustomerAddress.Text = d["_address"].ToString();
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

        private void metroTextBox_main_CustomerName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                metroPanel2.Enabled = false;
                //metroTextBoxVendorName.Text = string.Empty;
                comboBox_SubCustomer.Text = "";
                comboBox_SubCustomer.SelectedIndex = -1;
                metroTextBoxVendorContactNumber.Text = string.Empty;
                metroTextBoxVendorAddress.Text = string.Empty;
                if (comboBox_MainCustomer.Text.Trim().Length > 0 && comboBox_MainCustomer.SelectedValue != null)
                {
                    //string[] s = metroTextBox_main_CustomerName.Text.Trim().ToString().Split('(');
                    //string customerid = s[s.Length - 1].ToString().Trim().Split(')')[0].ToString();
                    string customerid = comboBox_MainCustomer.SelectedValue.ToString();
                    metroPanel2.Enabled = true;
                    sourceCustomer_Or_Vendor = new AutoCompleteStringCollection();
                    DataTable dt = gm.GetTable("select * from customer_or_vendor where status ='1' and customer_vendor_type='Customer' and parent!='' and parent='" + customerid + "'");
                    foreach (DataRow d in dt.Rows)
                    {
                        sourceCustomer_Or_Vendor.Add(d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")");
                    }

                    //metroTextBoxVendorName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    //metroTextBoxVendorName.AutoCompleteCustomSource = sourceCustomer_Or_Vendor;
                    //metroTextBoxVendorName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    comboBox_SubCustomer.DisplayMember = "customer_Or_Vendor_Name";
                    comboBox_SubCustomer.ValueMember = "id";
                    comboBox_SubCustomer.DataSource = dt;
                    comboBox_SubCustomer.AutoCompleteMode = AutoCompleteMode.Suggest;
                    comboBox_SubCustomer.AutoCompleteSource = AutoCompleteSource.ListItems;
                    comboBox_SubCustomer.SelectedIndex = -1;
                }
            }
            catch { }
        }

        private void metroRadioButton1_KeyDown(object sender, KeyEventArgs e)
        {
            //try
            //{
            //    metroRadioButton_Sabqa.Focus();
            //}
            //catch { }
        }

        private void metroLabel35_Click(object sender, EventArgs e)
        {

        }

        private void metroLabelTransactionId_Click(object sender, EventArgs e)
        {

        }

        private void metroLabel34_Click(object sender, EventArgs e)
        {

        }

        private void comboBox_MainCustomer_TextChanged(object sender, EventArgs e)
        {
            try
            {
                metroPanel2.Enabled = false;
                //metroTextBoxVendorName.Text = string.Empty;
                comboBox_SubCustomer.Text = "";
                comboBox_SubCustomer.SelectedIndex = -1;
                metroTextBoxVendorContactNumber.Text = string.Empty;
                metroTextBoxVendorAddress.Text = string.Empty;
                if (comboBox_MainCustomer.Text.Trim().Length > 0)
                {
                    string query = "";
                    string id = "";
                    try//only word if main customer or sub sub customer name is unique
                    {
                        query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + comboBox_MainCustomer.Text.Trim() + "' and status='1'";
                        id = gm.GetTable(query).Rows[0][0].ToString();
                    }
                    catch { }
                    if (id == "")
                    {
                        return;
                    }
                    string customerid = id;
                    metroPanel2.Enabled = true;
                    sourceCustomer_Or_Vendor = new AutoCompleteStringCollection();
                    DataTable dt = gm.GetTable("select * from customer_or_vendor where status ='1' and customer_vendor_type='Customer' and parent!='' and parent='" + customerid + "'");
                    foreach (DataRow d in dt.Rows)
                    {
                        sourceCustomer_Or_Vendor.Add(d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")");
                    }

                    //metroTextBoxVendorName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    //metroTextBoxVendorName.AutoCompleteCustomSource = sourceCustomer_Or_Vendor;
                    //metroTextBoxVendorName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    comboBox_SubCustomer.DisplayMember = "customer_Or_Vendor_Name";
                    comboBox_SubCustomer.ValueMember = "id";
                    comboBox_SubCustomer.DataSource = dt;
                    comboBox_SubCustomer.AutoCompleteMode = AutoCompleteMode.Suggest;
                    comboBox_SubCustomer.AutoCompleteSource = AutoCompleteSource.ListItems;
                    comboBox_SubCustomer.SelectedIndex = -1;
                    comboBox_SubCustomer.Focus();
                }
            }
            catch { }
            try
            {
                if (comboBox_MainCustomer.Text.Trim().ToString() != "" && comboBox_MainCustomer.SelectedValue != null)
                {
                    //string[] s = metroTextBox_main_CustomerName.Text.Trim().ToString().Split('(');
                    //string vendorid = s[s.Length - 1].ToString().Trim().Split(')')[0].ToString();
                    string vendorid = comboBox_MainCustomer.SelectedValue.ToString();
                    string query = "select * from customer_or_vendor where id='" + vendorid + "'";
                    DataTable dt2 = gm.GetTable(query);
                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataRow d in dt2.Rows)
                        {
                            //metroTextBox_main_CustomerName.Text = d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")";
                            //metroTextBox_main_CustomerName.Tag = d["id"].ToString();
                            metroTextBox_main_CustomerContact.Text = d["contact_number"].ToString();
                            metroTextBox_main_CustomerAddress.Text = d["_address"].ToString();
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

        int escape = 0;
        private void comboBox_MainCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
                key = "F1";
            else if (e.KeyCode == Keys.F3)
                key = "F3";
            else if (e.KeyCode == Keys.F4)
                key = "F4";
            if (e.Modifiers == Keys.Control && (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down))
            {
                return;
            }
            escape = 0;
            if (e.KeyCode == Keys.Escape)
            {
                escape = 1;
                this.Dispose();
            }
            if (e.KeyCode == Keys.Tab)
            {
                comboBox_SubCustomer.Focus();
            }
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (comboBox_MainCustomer.Text.Trim().ToString() != "")
                    {
                        string query = "";
                        string id = "";
                        try//only word if main customer or sub sub customer name is unique
                        {
                            query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + comboBox_MainCustomer.Text.Trim() + "' and status='1'";
                            id = gm.GetTable(query).Rows[0][0].ToString();
                        }
                        catch { }
                        if (id == "")
                        {
                            return;
                        }
                        //string[] s = metroTextBox_main_CustomerName.Text.Trim().ToString().Split('(');
                        //string vendorid = s[s.Length - 1].ToString().Trim().Split(')')[0].ToString();
                        string vendorid = id;
                        query = "select * from customer_or_vendor where id='" + vendorid + "'";
                        DataTable dt2 = gm.GetTable(query);
                        if (dt2.Rows.Count > 0)
                        {
                            foreach (DataRow d in dt2.Rows)
                            {
                                //metroTextBox_main_CustomerName.Text = d["customer_or_vendor_name"].ToString() + " (" + d["id"].ToString() + ")";
                                //metroTextBox_main_CustomerName.Tag = d["id"].ToString();
                                metroTextBox_main_CustomerContact.Text = d["contact_number"].ToString();
                                metroTextBox_main_CustomerAddress.Text = d["_address"].ToString();
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

        private void comboBox_MainCustomer_Leave(object sender, EventArgs e)
        {
            if (key != "")
                return;
            if (escape == 1)
            {
                return;
            }
            comboBox_SubCustomer.Focus();
        }

        private void metroTextBoxVendorName_Leave(object sender, EventArgs e)
        {
            //numericUpDown_Quantity.Focus();
        }
        string key = "";
        private void comboBox_SubCustomer_Leave(object sender, EventArgs e)
        {
            if (key != "")
                return;
            try
            {
                if (escape == 1)
                {
                    return;
                }
                //SendKeys.Send("{ENTER}");
                comboBox_ItemName.Focus();
            }
            catch { }
        }

        private void comboBox_SubCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
                key = "F1";
            else if (e.KeyCode == Keys.F3)
                key = "F3";
            else if (e.KeyCode == Keys.F4)
                key = "F4";
            if (e.Modifiers == Keys.Control && (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down))
            {
                return;
            }
            //try
            //{
            //    escape = 0;
            //    if (e.KeyCode == Keys.Escape)
            //    {
            //        escape = 1;
            //        this.Dispose();
            //    }
            //    //if (e.KeyCode == Keys.Enter)
            //    if (comboBox_SubCustomer.Text.Trim().Length > 0)// && comboBox_SubCustomer.SelectedValue!=null
            //    {
            //        try
            //        {
            //            if (comboBox_SubCustomer.Text.Trim().ToString() != "")// && comboBox_SubCustomer.SelectedValue != null
            //            {
            //                string query = "";
            //                string id = "";
            //                try//only word if main customer or sub sub customer name is unique
            //                {
            //                    query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + comboBox_SubCustomer.Text.Trim() + "' and status='1'";
            //                    id = gm.GetTable(query).Rows[0][0].ToString();
            //                }
            //                catch { }
            //                string vendorid = id;
            //                //string vendorid = comboBox_SubCustomer.SelectedValue.ToString();
            //                query = "select * from customer_or_vendor where id='" + vendorid + "'";
            //                DataTable dt2 = gm.GetTable(query);
            //                if (dt2.Rows.Count > 0)
            //                {
            //                    foreach (DataRow d in dt2.Rows)
            //                    {
            //                        metroTextBoxVendorContactNumber.Text = d["contact_number"].ToString();
            //                        metroTextBoxVendorAddress.Text = d["_address"].ToString();
            //                        getCustomerTodayBill(comboBox_SubCustomer.SelectedValue.ToString());
            //                        //numericUpDown_Quantity.Focus();
            //                        if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter)
            //                        {
            //                            numericUpDown_Quantity.Focus();
            //                        }
            //                    }
            //                }
            //                else
            //                {

            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            //MessageBox.Show(ex.Message);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //MessageBox.Show(ex.Message);
            //}
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                string customer_or_vendor_id = "";
                if (comboBox_SubCustomer.Text.Trim().ToString() != "" && comboBox_SubCustomer.SelectedValue != null)
                {
                    customer_or_vendor_id = comboBox_SubCustomer.SelectedValue.ToString();
                }
                string today_date = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
                string query = "select * from bill where Customer_Vendor_Id='" + customer_or_vendor_id + "' and bill_date='" + today_date + "' and status='1'";
                DataTable dt_temp1 = gm.GetTable(query);
                if (dt_temp1.Rows.Count > 0)
                {
                    query = "select * from bill_details where bill_id='" + dt_temp1.Rows[0]["id"].ToString() + "' and item_id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "' and status='1'";
                    DataTable dt_temp5 = gm.GetTable(query);
                    if (dt_temp5.Rows.Count > 0)
                    {
                        query = "delete from bill_details where bill_id='" + dt_temp1.Rows[0]["id"].ToString() + "' and item_id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "' and status='1'";
                        gm.ExecuteNonQuery(query);

                        try
                        {
                            query = "select qty from items where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
                            DataTable dt3 = gm.GetTable(query);
                            string itemqty = dt3.Rows[0][0].ToString();
                            if (itemqty.Trim() == "")
                            {
                                itemqty = "0";
                            }
                            try
                            {
                                itemqty = (double.Parse(itemqty) + double.Parse(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString())).ToString();
                            }
                            catch { }
                            query = "update items set qty='" + itemqty + "' where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
                            gm.ExecuteNonQuery(query);
                        }
                        catch { }



                        dataGridView1.Rows.RemoveAt(e.RowIndex);
                        calculateTotalAmount();
                        calculateDiscountAmount();
                        calculatePurchaseTax();
                        calculateServiceCharges();
                        calculatePurchaseTotal();
                        calculatePaymentDue();

                        query = @"update bill set Net_Total_Amount='" + ((labelNetTotal.Text.Trim() == "") ? "0" : labelNetTotal.Text.Trim()) + "',Total_Amount='" + ((labelPurchaseTotal.Text.Trim().ToString() == "") ? "0" : labelPurchaseTotal.Text.Trim().ToString()) + "',Balance='" + ((labelPaymentDue.Text.Trim().ToString() == "") ? "0" : labelPaymentDue.Text.Trim().ToString()) + "' where id='" + dt_temp1.Rows[0]["id"].ToString() + "'";
                        gm.ExecuteNonQuery(query);
                        numericUpDown_Quantity.Value = 1;
                        MessageBox.Show("Successfully Deleted");
                    }
                    else
                    {
                        MessageBox.Show("Record Not Found");
                    }
                }
            }
            catch { }
            try
            {
                //dataGridView1.Rows.RemoveAt(e.RowIndex);
                //calculateTotalAmount();
                //calculateDiscountAmount();
                //calculatePurchaseTax();
                //calculateServiceCharges();
                //calculatePurchaseTotal();
                //calculatePaymentDue();
                //removeAndSave();
            }
            catch { }
            try
            {
                int i = 1;
                foreach (DataGridViewRow r2 in dataGridView1.Rows)
                {
                    r2.Cells[10].Value = i.ToString();
                    i++;
                }
            }
            catch { }
            dataGridView1.ClearSelection();
        }

        public void removeAndSave()
        {
            try
            {
                if (dataGridView1.Rows.Count <= 0)
                {
                    MessageBox.Show("No Item Found For Complete This Transaction");
                    return;
                }
                metroTextBoxPaidAmount.Text = "0";
                string query = "";
                string customer_or_vendor_id = "";
                if (comboBox_SubCustomer.Text.Trim().ToString() != "" && comboBox_SubCustomer.SelectedValue != null)
                {
                    string vendorid = comboBox_SubCustomer.SelectedValue.ToString();
                    query = "select * from customer_or_vendor where id='" + vendorid + "'";
                    DataTable dt2 = gm.GetTable(query);
                    if (dt2.Rows.Count > 0)
                        customer_or_vendor_id = vendorid;
                    else
                    {
                        MessageBox.Show("Sub Customer Not Registered");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Enter Sub Customer");
                    return;
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

                string today_date = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
                query = "select * from bill where Customer_Vendor_Id='" + customer_or_vendor_id + "' and bill_date='" + today_date + "' and status='1'";
                DataTable dt_temp1 = gm.GetTable(query);
                if (dt_temp1.Rows.Count <= 0)
                {
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
                    string billid = "S" + metroDateTime1.Value.Date.Day.ToString() + metroDateTime1.Value.Date.Month.ToString() + metroDateTime1.Value.Date.Year.ToString() + countstring;

                    query = "Select max(cast(id as int)) from bill";
                    string id = gm.MaxId(query);
                    query = @"insert into bill values('" + id + "','" + "Sale Trading" + "','" + billid + "','" + metroDateTime1.Value.Date + "','" + DateTime.Now.ToShortTimeString() + "','" + ((labelNetTotal.Text.Trim() == "") ? "0" : labelNetTotal.Text.Trim()) + "','" + metroComboBoxDiscountType.SelectedItem.ToString() + "','" + ((metroTextBoxDiscount.Text.Trim().ToString() == "") ? "0" : metroTextBoxDiscount.Text.Trim().ToString()) + "','" + ((metroLabDiscountAmount.Text.Trim().ToString() == "") ? "0" : metroLabDiscountAmount.Text.Trim().ToString()) + "',N'" + metroComboBoxTax.SelectedItem.ToString() + "','" + ((metroLabelPurchaseTax.Text.Trim().ToString() == "") ? "0" : metroLabelPurchaseTax.Text.Trim().ToString()) + "','" + ((metroLabelServiceCharges.Text.Trim().ToString() == "") ? "0" : metroLabelServiceCharges.Text.Trim().ToString()) + "','" + "" + "','" + "0" + "','" + ((labelPurchaseTotal.Text.Trim().ToString() == "") ? "0" : labelPurchaseTotal.Text.Trim().ToString()) + "','" + ((metroTextBoxPaidAmount.Text.Trim().ToString() == "") ? "0" : metroTextBoxPaidAmount.Text.Trim().ToString()) + "','" + ((labelPaymentDue.Text.Trim().ToString() == "") ? "0" : labelPaymentDue.Text.Trim().ToString()) + "','" + "" + "','" + customer_or_vendor_id + "',N'" + metroComboBoxPaymentMethod.SelectedItem.ToString() + "',N'" + cardnumber + "',N'" + cardholdername + "','" + cardtransactionid + "',N'" + cardtype + "','" + cardexpirymonth + "','" + cardexpiryyearh + "','" + cardsecuritycode + "',N'" + chequenumber + "',N'" + bankname + "',N'" + bankaccountnumber + "',N'" + paymentnote + "','" + RJ.Properties.Settings.Default.loginid + "','" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.ToShortTimeString() + "','1')";
                    int a = gm.ExecuteNonQuery(query);
                    if (a > 0)
                    {
                        int ok = 0;
                        foreach (DataGridViewRow d in dataGridView1.Rows)
                        {
                            string itemid = d.Cells[0].Value.ToString();
                            string qty = d.Cells[5].Value.ToString();
                            string mazdori = d.Cells[7].Value.ToString();
                            string unitcost = d.Cells[6].Value.ToString();
                            string totalamount = d.Cells[8].Value.ToString();
                            string mfgdate = "";
                            string expdate = "";
                            string type = d.Cells[2].Value.ToString();
                            string batchorlotnum = "";
                            query = "select max(cast(id as int)) from bill_details";
                            string bid = gm.MaxId(query);
                            query = "insert into bill_details values('" + bid + "','" + id + "','" + itemid + "','" + qty + "','" + unitcost + "','" + totalamount + "','" + mfgdate + "','" + expdate + "','" + batchorlotnum + "','1',N'" + type + "','','" + DateTime.Now.ToShortDateString() + "','"+mazdori+"')";
                            int b = gm.ExecuteNonQuery(query);
                            if (b > 0)
                            {
                                query = "select qty from items where id = '" + itemid + "'";
                                DataTable dt3 = gm.GetTable(query);
                                string itemqty = dt3.Rows[0][0].ToString();
                                if (itemqty.Trim() == "")
                                {
                                    itemqty = "0";
                                }
                                try
                                {
                                    itemqty = (double.Parse(itemqty) - double.Parse(qty)).ToString();
                                }
                                catch { }
                                query = "update items set qty='" + itemqty + "' where id = '" + itemid + "'";
                                gm.ExecuteNonQuery(query);
                                ok = 1;
                            }
                        }
                        if (ok == 1)
                        {
                            MessageBox.Show("Successfully Saved");
                        }
                    }
                }
                else//if customer bill already exist of today
                {
                    string bill_id = dt_temp1.Rows[0][0].ToString();
                    query = "delete from bill_details where bill_id='" + bill_id + "'";
                    gm.ExecuteNonQuery(query);

                    query = @"update bill set Bill_Date='" + metroDateTime1.Value.Date + "',Bill_Time='" + DateTime.Now.ToShortTimeString() + "',Net_Total_Amount='" + ((labelNetTotal.Text.Trim() == "") ? "0" : labelNetTotal.Text.Trim()) + "',Discount_Type='" + metroComboBoxDiscountType.SelectedItem.ToString() + "',Discount='" + ((metroTextBoxDiscount.Text.Trim().ToString() == "") ? "0" : metroTextBoxDiscount.Text.Trim().ToString()) + "',Discount_Amount='" + ((metroLabDiscountAmount.Text.Trim().ToString() == "") ? "0" : metroLabDiscountAmount.Text.Trim().ToString()) + "',Tax_Type=N'" + metroComboBoxTax.SelectedItem.ToString() + "',Tax_Amount='" + ((metroLabelPurchaseTax.Text.Trim().ToString() == "") ? "0" : metroLabelPurchaseTax.Text.Trim().ToString()) + "',Service_Charges='" + ((metroLabelServiceCharges.Text.Trim().ToString() == "") ? "0" : metroLabelServiceCharges.Text.Trim().ToString()) + "',Voucher_Or_Coupon_Id='" + "" + "',Vouchar_Coupon_Discount_Amount='" + "0" + "',Total_Amount='" + ((labelPurchaseTotal.Text.Trim().ToString() == "") ? "0" : labelPurchaseTotal.Text.Trim().ToString()) + "',Paid_Amount='" + ((metroTextBoxPaidAmount.Text.Trim().ToString() == "") ? "0" : metroTextBoxPaidAmount.Text.Trim().ToString()) + "',Balance='" + ((labelPaymentDue.Text.Trim().ToString() == "") ? "0" : labelPaymentDue.Text.Trim().ToString()) + "',_Description='" + "" + "',Customer_Vendor_Id='" + customer_or_vendor_id + "',Payment_Method=N'" + metroComboBoxPaymentMethod.SelectedItem.ToString() + "',Card_Number=N'" + cardnumber + "',Card_Holder_Name=N'" + cardholdername + "',Card_Transaction_No='" + cardtransactionid + "',Card_Type=N'" + cardtype + "',Month='" + cardexpirymonth + "',Year='" + cardexpiryyearh + "',Security_Code='" + cardsecuritycode + "',Cheque_No=N'" + chequenumber + "',Bank_Name=N'" + bankname + "',Bank_Account_No=N'" + bankaccountnumber + "',Payment_Note=N'" + paymentnote + "',AddedBy_UserId='" + RJ.Properties.Settings.Default.loginid + "',_Date='" + DateTime.Now.ToShortDateString() + "',_Time='" + DateTime.Now.ToShortTimeString() + "',Status='1' where id='" + bill_id + "'";
                    gm.ExecuteNonQuery(query);
                    //////////change
                    int ok = 0;
                    foreach (DataGridViewRow d in dataGridView1.Rows)
                    {
                        string itemid = d.Cells[0].Value.ToString();
                        string qty = d.Cells[5].Value.ToString();
                        string mazdori = d.Cells[7].Value.ToString();
                        string unitcost = d.Cells[6].Value.ToString();
                        string type = d.Cells[2].Value.ToString();
                        string totalamount = d.Cells[8].Value.ToString();
                        string mfgdate = "";
                        string expdate = "";
                        string batchorlotnum = "";
                        query = "select max(cast(id as int)) from bill_details";
                        string bid = gm.MaxId(query);
                        query = "insert into bill_details values('" + bid + "','" + bill_id + "','" + itemid + "','" + qty + "','" + unitcost + "','" + totalamount + "','" + mfgdate + "','" + expdate + "','" + batchorlotnum + "','1',N'" + type + "','','" + DateTime.Now.ToShortDateString() + "','"+mazdori+"')";
                        int b = gm.ExecuteNonQuery(query);
                        if (b > 0)
                        {
                            query = "select qty from items where id = '" + itemid + "'";
                            DataTable dt3 = gm.GetTable(query);
                            string itemqty = dt3.Rows[0][0].ToString();
                            if (itemqty.Trim() == "")
                            {
                                itemqty = "0";
                            }
                            try
                            {
                                itemqty = (double.Parse(itemqty) - double.Parse(qty)).ToString();
                            }
                            catch { }
                            query = "update items set qty='" + itemqty + "' where id = '" + itemid + "'";
                            gm.ExecuteNonQuery(query);
                            ok = 1;
                        }
                    }
                    if (ok == 1)
                    {
                        MessageBox.Show("Successfully Saved");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void groupBox2_Leave(object sender, EventArgs e)
        {
            comboBox_ItemName.Focus();
        }

        private void comboBox_SubCustomer_TextChanged(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.Clear();
            }
            if (comboBox_SubCustomer.Text.Trim().Length > 0)//&& comboBox_SubCustomer.SelectedValue != null
            {
                try
                {
                    if (comboBox_SubCustomer.Text.Trim().ToString() != "")// && comboBox_SubCustomer.SelectedValue != null
                    {
                        string query = "";
                        string id = "";
                        try//only work if main customer or sub sub customer name is unique
                        {
                            query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + comboBox_MainCustomer.Text.Trim() + "' and status='1'";
                            string parentid = gm.GetTable(query).Rows[0][0].ToString();
                            query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + comboBox_SubCustomer.Text.Trim() + "' and status='1' and parent='"+parentid+"'";
                            id = gm.GetTable(query).Rows[0][0].ToString();
                        }
                        catch { }
                        if (id == "")
                        {
                            return;
                        }
                        string vendorid = id;
                        //string vendorid = comboBox_SubCustomer.SelectedValue.ToString();
                        query = "select * from customer_or_vendor where id='" + vendorid + "'";
                        //MessageBox.Show(query);
                        DataTable dt2 = gm.GetTable(query);
                        if (dt2.Rows.Count > 0)
                        {
                            foreach (DataRow d in dt2.Rows)
                            {
                                //metroTextBoxVendorContactNumber.Text = d["contact_number"].ToString();
                                //metroTextBoxVendorAddress.Text = d["_address"].ToString();
                                getCustomerTodayBill(vendorid);
                                //if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter)
                                //{
                                //    numericUpDown_Quantity.Focus();
                                //}
                            }
                        }
                        else
                        {

                        }
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
        }

        private void comboBox_SubCustomer_KeyUp(object sender, KeyEventArgs e)
        {
            if (comboBox_SubCustomer.Text.Trim().Length > 0)//&& comboBox_SubCustomer.SelectedValue != null
            {
                try
                {
                    if (comboBox_SubCustomer.Text.Trim().ToString() != "")// && comboBox_SubCustomer.SelectedValue != null
                    {
                        string query = "";
                        string id = "";
                        try//only word if main customer or sub sub customer name is unique
                        {
                            query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + comboBox_MainCustomer.Text.Trim() + "' and status='1'";
                            string parentid = gm.GetTable(query).Rows[0][0].ToString();
                            query = "select * from Customer_Or_Vendor where customer_Or_Vendor_Name=N'" + comboBox_SubCustomer.Text.Trim() + "' and status='1' and parent='"+parentid+"'";
                            id = gm.GetTable(query).Rows[0][0].ToString();
                        }
                        catch { }
                        if (id == "")
                        {
                            return;
                        }
                        string vendorid = id;
                        //string vendorid = comboBox_SubCustomer.SelectedValue.ToString();
                        query = "select * from customer_or_vendor where id='" + vendorid + "'";
                        //MessageBox.Show(query);
                        DataTable dt2 = gm.GetTable(query);
                        if (dt2.Rows.Count > 0)
                        {
                            foreach (DataRow d in dt2.Rows)
                            {
                                //metroTextBoxVendorContactNumber.Text = d["contact_number"].ToString();
                                //metroTextBoxVendorAddress.Text = d["_address"].ToString();
                                getCustomerTodayBill(vendorid);
                                if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter)
                                {
                                    numericUpDown_Quantity.Focus();
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
        }

        private void comboBox_MainCustomer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                comboBox_SubCustomer.Focus();
            }
        }

        int qtyenter = 0;
        private void numericUpDown_Quantity_Enter(object sender, EventArgs e)
        {
            //SendKeys.Send("{ENTER}");

            //qtyenter = 0;
            //try
            //{
            //    if (comboBox_MainCustomer.Text.Trim().ToString() != "")
            //    {
            //        if (comboBox_MainCustomer.SelectedValue == null)
            //        {
            //            MessageBox.Show("Select Customer");
            //            comboBox_MainCustomer.Focus();
            //            return;
            //        }
            //        string vendorid = comboBox_MainCustomer.SelectedValue.ToString();
            //        string query = "select * from customer_or_vendor where id='" + vendorid + "'";
            //        DataTable dt2 = gm.GetTable(query);
            //        if (dt2.Rows.Count > 0)
            //        {

            //        }
            //        else
            //        {
            //            MessageBox.Show("Customer Not Registered");
            //            comboBox_MainCustomer.Focus();
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Enter Customer Name");
            //        comboBox_MainCustomer.Focus();
            //        return;
            //    }

            //    if (comboBox_SubCustomer.Text.Trim().ToString() != "")
            //    {
            //        if (comboBox_SubCustomer.SelectedValue == null)
            //        {
            //            MessageBox.Show("Select Sub Customer");
            //            qtyenter = 1;
            //            comboBox_SubCustomer.Focus();
            //            return;
            //        }
            //        string vendorid = comboBox_SubCustomer.SelectedValue.ToString();
            //        string query = "select * from customer_or_vendor where id='" + vendorid + "'";
            //        DataTable dt2 = gm.GetTable(query);
            //        if (dt2.Rows.Count > 0)
            //        {

            //        }
            //        else
            //        {
            //            MessageBox.Show("Sub Customer Not Registered");
            //            qtyenter = 1;
            //            comboBox_SubCustomer.Focus();
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        //MessageBox.Show("Enter Sub Customer");
            //        qtyenter = 1;
            //        comboBox_SubCustomer.Focus();
            //        return;
            //    }
            //}
            //catch { }
        }

        private void comboBox_MainCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox_ItemName_TextChanged(object sender, EventArgs e)
        {
            try
            {//for sku/barcode
                if (comboBox_ItemName.Text.Trim().ToString() != "" && comboBox_ItemName.SelectedValue != null)
                {
                    string query = "select * from items where (sku='" + comboBox_ItemName.Text.Trim().ToString() + "' or barcode='" + comboBox_ItemName.Text.Trim().ToString() + "') and status='1'";
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
                //MessageBox.Show(ex.Message);
            }
        }

        string endproductid = "";
        private void comboBox_ItemName_KeyDown(object sender, KeyEventArgs e)
        {
            endproductid = "";
            if (e.Modifiers == Keys.Control && (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down))
            {
                SendKeys.Send("{RIGHT}");
                SendKeys.Send("{BACKSPACE}");
                return;
            }
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Space)
            {
                metroRadioButton1.Checked = true;
                metroRadioButton1.Focus();
                return;
            }
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    try
                    {
                        if (comboBox_MainCustomer.Text.Trim().ToString() != "")
                        {
                            if (comboBox_MainCustomer.SelectedValue == null)
                            {
                                comboBox_ItemName.Focus();
                                comboBox_ItemName.SelectedIndex = -1;
                                comboBox_ItemName.Text = "";
                                MessageBox.Show("Select Customer");
                                comboBox_MainCustomer.Focus();
                                return;
                            }
                            string vendorid = comboBox_MainCustomer.SelectedValue.ToString();
                            string query = "select * from customer_or_vendor where id='" + vendorid + "'";
                            DataTable dt2 = gm.GetTable(query);
                            if (dt2.Rows.Count > 0)
                            {

                            }
                            else
                            {
                                comboBox_ItemName.Focus();
                                comboBox_ItemName.SelectedIndex = -1;
                                comboBox_ItemName.Text = "";
                                MessageBox.Show("Customer Not Registered");
                                comboBox_MainCustomer.Focus();
                                return;
                            }
                        }
                        else
                        {
                            comboBox_ItemName.Focus();
                            comboBox_ItemName.SelectedIndex = -1;
                            comboBox_ItemName.Text = "";
                            MessageBox.Show("Enter Customer Name");
                            comboBox_MainCustomer.Focus();
                            return;
                        }

                        if (comboBox_SubCustomer.Text.Trim().ToString() != "")
                        {
                            if (comboBox_SubCustomer.SelectedValue == null)
                            {
                                comboBox_ItemName.Focus();
                                comboBox_ItemName.SelectedIndex = -1;
                                comboBox_ItemName.Text = "";
                                MessageBox.Show("Select Sub Customer");
                                comboBox_SubCustomer.Focus();
                                return;
                            }
                            string vendorid = comboBox_SubCustomer.SelectedValue.ToString();
                            string query = "select * from customer_or_vendor where id='" + vendorid + "'";
                            DataTable dt2 = gm.GetTable(query);
                            if (dt2.Rows.Count > 0)
                            {

                            }
                            else
                            {
                                comboBox_ItemName.Focus();
                                comboBox_ItemName.SelectedIndex = -1;
                                comboBox_ItemName.Text = "";
                                MessageBox.Show("Sub Customer Not Registered");
                                comboBox_SubCustomer.Focus();
                                return;
                            }
                        }
                        else
                        {
                            comboBox_ItemName.Focus();
                            comboBox_ItemName.SelectedIndex = -1;
                            comboBox_ItemName.Text = "";
                            MessageBox.Show("Enter Sub Customer");
                            comboBox_SubCustomer.Focus();
                            return;
                        }
                    }
                    catch { }

                    try
                    {
                        //string[] s = metroTextBoxItemName.Text.Trim().Split('(');
                        //string[] a = s[s.Length - 1].Trim().Split(')');
                        //string id = a[0].ToString();
                        string id = comboBox_ItemName.SelectedValue.ToString();
                        if (id.Trim() != "")
                        {
                            if (e.KeyCode == Keys.Enter)
                            {
                                endproductitemlistid = "";
                                endproductitemlistitem = "";
                                ItemSelection epl = new ItemSelection();
                                //string value = comboBox_ItemName.SelectedValue.ToString();
                                //string item1 = comboBox_ItemName.SelectedItem.ToString();
                                //string text = comboBox_ItemName.SelectedText.ToString();
                                //MessageBox.Show(comboBox_ItemName.SelectedValue.ToString());
                                epl.itemname = comboBox_ItemName.SelectedText.ToString();
                                epl.ShowDialog();
                                endproductitemlistid = epl.selecteditemid;
                                endproductitemlistitem = epl.selecteditemname;
                                //int index = comboBox_ItemName.Items.IndexOf(endproductitemlistid);
                                //comboBox_ItemName.SelectedIndex = index;

                                //MessageBox.Show(comboBox_ItemName.SelectedValue.ToString());

                                id = endproductitemlistid;
                            }

                            endproductid = id;
                            numericUpDown_Quantity.Focus();
                            return;
                            //getItemNameRecord(id);
                            //comboBox_ItemName.Focus();
                            //SendKeys.Send("{BACKSPACE}");
                        }
                        else
                        {
                            comboBox_ItemName.Text = string.Empty;
                            comboBox_ItemName.SelectedIndex = -1;
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                        comboBox_ItemName.Text = string.Empty;
                        comboBox_ItemName.SelectedIndex = -1;
                    }
                    comboBox_ItemName.Text = string.Empty;
                    comboBox_ItemName.SelectedIndex = -1;
                    metroRadioButton_Sabqa.Checked = false;
                    metroRadioButton_BillMein.Checked = false;
                    metroRadioButton_Kharab.Checked = false;
                }
            }
            catch (Exception ex) 
            { 
                //MessageBox.Show(ex.Message); 
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string customer_or_vendor_id = "";
                if (comboBox_SubCustomer.Text.Trim().ToString() != "" && comboBox_SubCustomer.SelectedValue != null)
                {
                    customer_or_vendor_id = comboBox_SubCustomer.SelectedValue.ToString();
                }
                string today_date = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
                string query = "select * from bill where Customer_Vendor_Id='" + customer_or_vendor_id + "' and bill_date='" + today_date + "' and status='1'";
                DataTable dt_temp1 = gm.GetTable(query);
                if (dt_temp1.Rows.Count > 0)
                {
                    query = "select * from bill_details where bill_id='" + dt_temp1.Rows[0]["id"].ToString() + "' and item_id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "' and status='1'";
                    DataTable dt_temp5 = gm.GetTable(query);
                    if (dt_temp5.Rows.Count > 0)
                    {
                        query = "delete from bill_details where bill_id='" + dt_temp1.Rows[0]["id"].ToString() + "' and item_id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "' and status='1'";
                        gm.ExecuteNonQuery(query);

                        try
                        {
                            query = "select qty from items where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
                            DataTable dt3 = gm.GetTable(query);
                            string itemqty = dt3.Rows[0][0].ToString();
                            if (itemqty.Trim() == "")
                            {
                                itemqty = "0";
                            }
                            try
                            {
                                itemqty = (double.Parse(itemqty) + double.Parse(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString())).ToString();
                            }
                            catch { }
                            query = "update items set qty='" + itemqty + "' where id = '" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
                            gm.ExecuteNonQuery(query);
                        }
                        catch { }
                        
                        
                        
                        dataGridView1.Rows.RemoveAt(e.RowIndex);
                        calculateTotalAmount();
                        calculateDiscountAmount();
                        calculatePurchaseTax();
                        calculateServiceCharges();
                        calculatePurchaseTotal();
                        calculatePaymentDue();

                        query = @"update bill set Net_Total_Amount='" + ((labelNetTotal.Text.Trim() == "") ? "0" : labelNetTotal.Text.Trim()) + "',Total_Amount='" + ((labelPurchaseTotal.Text.Trim().ToString() == "") ? "0" : labelPurchaseTotal.Text.Trim().ToString()) + "',Balance='" + ((labelPaymentDue.Text.Trim().ToString() == "") ? "0" : labelPaymentDue.Text.Trim().ToString()) + "' where id='" + dt_temp1.Rows[0]["id"].ToString()  + "'";
                        gm.ExecuteNonQuery(query);
                        numericUpDown_Quantity.Value = 1;
                        MessageBox.Show("Successfully Deleted");
                    }
                    else
                    {
                        MessageBox.Show("Record Not Found");
                    }
                }
            }
            catch { }
            try
            {
                //dataGridView1.Rows.RemoveAt(e.RowIndex);
                //calculateTotalAmount();
                //calculateDiscountAmount();
                //calculatePurchaseTax();
                //calculateServiceCharges();
                //calculatePurchaseTotal();
                //calculatePaymentDue();
                //removeAndSave();
            }
            catch { }
            try
            {
                int i = 1;
                foreach (DataGridViewRow r2 in dataGridView1.Rows)
                {
                    r2.Cells[10].Value = i.ToString();
                    i++;
                }
            }
            catch { }
            dataGridView1.ClearSelection();
        }

        private void numericUpDown_Quantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                getItemNameRecord(endproductid);
                comboBox_ItemName.Focus();
                SendKeys.Send("{RIGHT}");
                SendKeys.Send("{BACKSPACE}");
                //return;
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void comboBox_SubCustomer_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown_Quantity_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                //if (e.KeyCode == Keys.Enter)
                //{
                //    comboBox_ItemName.Focus();
                //}
            }
            catch { }
        }

        private void metroRadioButton1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    comboBox_ItemName.Focus();
                }
            }
            catch { }
        }

        private void metroRadioButton_Sabqa_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    comboBox_ItemName.Focus();
                }
            }
            catch { }
        }

        private void metroRadioButton_BillMein_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    comboBox_ItemName.Focus();
                }
            }
            catch { }
        }

        private void metroRadioButton_Kharab_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    comboBox_ItemName.Focus();
                }
            }
            catch { }
        }

        private void comboBox_ItemName_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void comboBox_ItemName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
