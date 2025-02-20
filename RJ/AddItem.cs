using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RJ
{
    public partial class AddItem : Form // MetroFramework.Forms.MetroForm
    {
        public AddItem()
        {
            InitializeComponent();
        }

        protected void treeView1_AfterSelect(object sender, EventArgs e)//usercontrol->itemcategory work ref 1
        {
            try
            {
                if (treeView1.SelectedNode != null)
                {
                    if (treeView1.SelectedNode.Level > 0)
                    {
                        if (treeView1.SelectedNode.Nodes.Count == 0)
                        {
                            metroTextBoxCategory.Text = treeView1.SelectedNode.Text.Trim().ToString();
                            metroTextBoxCategory.Tag = treeView1.SelectedNode.Tag;
                        }
                    }
                }
                else
                    MessageBox.Show("Select Category");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void reset_fields()
        {
            try
            {
                metroTextBox_Mazdori.Text = "0";
                metroTextBox_Weight.Text = "0";
                metroTextBoxItemName.Text = string.Empty;
                getBrands();
                getUnits();
                metroComboBoxBrand.SelectedIndex = 0;
                comboBox_Unit.SelectedIndex = 0;
                metroTextBoxCategory.Text = string.Empty;
                metroTextBoxSKU.Text = string.Empty;
                metroTextBoxPurchasePrice.Text = string.Empty;
                metroTextBoxSellingPrice.Text = string.Empty;
                metroTextBoxBarcode.Text = string.Empty;
                metroTextBoxAlertQty.Text = string.Empty;
                metroCheckBoxHavingMfgExpDate.Checked = false;
                metroCheckBoxHaveBatchLotNum.Checked = false;
                pictureBox1.Image = pictureBox1.InitialImage;
                metroTextBoxItemDescription.Text = string.Empty;
                treeView1.CollapseAll();
                //metroTextBox_Weight.Text = string.Empty;

                DataTable dt = new DataTable();
                try
                {
                    string query = "select * from categories where id='1'";
                    dt = gm.GetTable(query);
                    foreach (DataRow d in dt.Rows)
                    {
                        metroTextBoxCategory.Text = d[2].ToString();
                        metroTextBoxCategory.Tag = d[0].ToString();
                    }
                }
                catch { }

                dt = gm.GetTable("select * from items where status ='1'");
                foreach (DataRow d in dt.Rows)
                {
                    sourceitemname.Add(d["name"].ToString());
                }

                metroTextBoxItemName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                metroTextBoxItemName.AutoCompleteCustomSource = sourceitemname;
                metroTextBoxItemName.AutoCompleteSource = AutoCompleteSource.CustomSource;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void getBrands()
        {
            try
            {
                List<Brand1> brands = new List<Brand1>();
                metroComboBoxBrand.DataSource = null;
                string query = "select * from brands where status='1'";
                DataTable dt = gm.GetTable(query);
                brands.Add(new Brand1("Other", ""));
                foreach (DataRow d in dt.Rows)
                {
                    brands.Add(new Brand1(d["Brand_Name"].ToString(), d["Id"].ToString()));
                }
                metroComboBoxBrand.DataSource = brands;
                metroComboBoxBrand.DisplayMember = "Name";
                metroComboBoxBrand.ValueMember = "Id";
                metroComboBoxBrand.SelectedIndex=-1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void getUnits()
        {
            try
            {
                List<Unit1> units = new List<Unit1>();
                comboBox_Unit.DataSource = null;
                string query = "select * from units where status='1'";
                DataTable dt = gm.GetTable(query);
                //units.Add(new Unit("Other", ""));
                foreach (DataRow d in dt.Rows)
                {
                    units.Add(new Unit1(d["Short_Name"].ToString(), d["Id"].ToString()));
                }
                //MessageBox.Show(units[0].Name.ToString());
                comboBox_Unit.DataSource = units;
                comboBox_Unit.DisplayMember = "Name";
                comboBox_Unit.ValueMember = "Id";
                comboBox_Unit.AutoCompleteMode = AutoCompleteMode.Suggest;
                comboBox_Unit.AutoCompleteSource = AutoCompleteSource.ListItems;
                comboBox_Unit.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }



        public void messag()
        {
            MessageBox.Show("ac");
        }

        int edit = 0;
        public void LoadItem(string itemid)
        {
            try
            {
                string query = "select * from items where id = '"+itemid+"'";
                DataTable dt = gm.GetTable(query);
                foreach (DataRow d in dt.Rows)
                {
                    byte[] imageData = (byte[])d["picture"];
                    if (imageData != null)
                    {
                        MemoryStream ms = new MemoryStream(imageData);
                        pictureBox1.Image = Image.FromStream(ms);
                    }
                    metroTextBoxItemName.Text = d["Name"].ToString();
                    metroTextBoxItemName.Tag= d["Id"].ToString();

                    int index = -1;
                    int index1 = 0;
                    if (d["brand_id"].ToString().Trim() == "Other")
                        metroComboBoxBrand.SelectedIndex = 0;
                    else
                    {
                        foreach (Brand1 b in metroComboBoxBrand.Items)
                        {
                            if (b.Id == d["brand_id"].ToString())
                            {
                                index = index1;
                            }
                            index1++;
                        }
                        if (index == -1)
                        {
                            Brand1 b = new Brand1("", "");
                            b.Id = d["Brand_Id"].ToString();
                            b.Name = "";
                            try
                            {
                                query = "select * from brands where id = '" + d["brand_id"].ToString().Trim() + "'";
                                DataTable dt2 = gm.GetTable(query);
                                b.Name = dt2.Rows[0][1].ToString();
                            }
                            catch { }




                            List<Brand1> brands = new List<Brand1>();
                            metroComboBoxBrand.DataSource = null;
                            query = "select * from brands where status='1'";
                            DataTable dt3 = gm.GetTable(query);
                            brands.Add(new Brand1("Other", ""));
                            foreach (DataRow d3 in dt3.Rows)
                            {
                                brands.Add(new Brand1(d3["Brand_Name"].ToString(), d3["Id"].ToString()));
                            }
                            brands.Add(new Brand1(b.Name, b.Id));
                            metroComboBoxBrand.DataSource = brands;
                            metroComboBoxBrand.DisplayMember = "Name";
                            metroComboBoxBrand.ValueMember = "Id";


                            index = -1;
                            index1 = 0;
                            foreach (Brand1 b2 in metroComboBoxBrand.Items)
                            {
                                if (b2.Id == d["brand_id"].ToString())
                                {
                                    index = index1;
                                }
                                index1++;
                            }
                            metroComboBoxBrand.SelectedIndex = index;
                        }
                        else
                        {
                            metroComboBoxBrand.SelectedIndex = index;
                        }

                    }

                    if (d["unit_id"].ToString().Trim() == "Other")
                        comboBox_Unit.SelectedIndex = 0;
                    else
                    {
                        index = -1;
                        index1 = 0;
                        foreach (Unit1 b in comboBox_Unit.Items)
                        {
                            if (b.Id == d["unit_id"].ToString())
                            {
                                index = index1;
                            }
                            index1++;
                        }
                        if (index == -1)
                        {
                            Unit1 b = new Unit1("", "");
                            b.Id = d["Unit_Id"].ToString();
                            b.Name = "";
                            try
                            {
                                query = "select * from units where id = '" + d["unit_id"].ToString().Trim() + "'";
                                DataTable dt2 = gm.GetTable(query);
                                b.Name = dt2.Rows[0][1].ToString();
                            }
                            catch { }

                            List<Unit1> units = new List<Unit1>();
                            comboBox_Unit.DataSource = null;
                            query = "select * from units where status='1'";
                            DataTable dt3 = gm.GetTable(query);
                            units.Add(new Unit1("Other", ""));
                            foreach (DataRow d3 in dt3.Rows)
                            {
                                units.Add(new Unit1(d3["Short_Name"].ToString(), d3["Id"].ToString()));
                            }
                            units.Add(new Unit1(b.Name, b.Id));
                            comboBox_Unit.DataSource = units;
                            comboBox_Unit.DisplayMember = "Name";
                            comboBox_Unit.ValueMember = "Id";
                            comboBox_Unit.AutoCompleteMode = AutoCompleteMode.Suggest;
                            comboBox_Unit.AutoCompleteSource = AutoCompleteSource.ListItems;

                            index = -1;
                            index1 = 0;
                            foreach (Unit1 b2 in comboBox_Unit.Items)
                            {
                                if (b2.Id == d["unit_id"].ToString())
                                {
                                    index = index1;
                                }
                                index1++;
                            }
                            comboBox_Unit.SelectedIndex = index;
                        }
                        else
                        {
                            comboBox_Unit.SelectedIndex = index;
                        }
                    }
                    string category = "";
                    try
                    {
                        query = "select * from categories where id='"+d["category_id"].ToString()+"'";
                        DataTable dt2 = gm.GetTable(query);
                        category = dt2.Rows[0][2].ToString();
                    }catch{}
                    metroTextBoxCategory.Text = category;
                    metroTextBoxCategory.Tag = d["category_id"].ToString();

                    metroTextBoxSKU.Text = d["SKU"].ToString();
                    metroTextBoxPurchasePrice.Text = d["Purchase_Price"].ToString();
                    metroTextBoxSellingPrice.Text = d["Retail_Price"].ToString();
                    metroTextBoxBarcode.Text = d["barcode"].ToString();
                    metroTextBoxAlertQty.Text = d["AlertQty"].ToString();
                    metroTextBoxItemDescription.Text = d["Detail"].ToString();
                    if (d["Have_Mfg_Or_Exp_Date"].ToString() == "True")
                    {
                        metroCheckBoxHavingMfgExpDate.Checked = true;
                    }
                    else
                    {
                        metroCheckBoxHavingMfgExpDate.Checked = false;
                    }
                    if (d["Have_Batch_Or_Lot_No"].ToString() == "True")
                    {
                        metroCheckBoxHaveBatchLotNum.Checked = true;
                    }
                    else
                    {
                        metroCheckBoxHaveBatchLotNum.Checked = false;
                    }

                    //metroTextBoxItemName.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        string item_id = "";
        public AddItem(string itemid)
        {
            try
            {
                InitializeComponent();

                getBrands();
                getUnits();
                metroComboBoxBrand.SelectedIndex = 0;
                comboBox_Unit.SelectedIndex = 0;
                itemCategory1.treeView1AfterSelect += new EventHandler(treeView1_AfterSelect);//usercontrol->itemcategory work ref 1
                treeView1 = (itemCategory1.Controls["treeView1"] as TreeView);
                metroTextBoxItemName.Focus();

                edit = 1;
                item_id = itemid;
                LoadItem(itemid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        AutoCompleteStringCollection sourceitemname = new AutoCompleteStringCollection();
        AutoCompleteStringCollection sourceweight = new AutoCompleteStringCollection();
        private void AddItem_Load(object sender, EventArgs e)
        {
            metroLabel11.Visible = false;
            metroTextBoxPurchasePrice.Visible = false;
            metroTextBoxPurchasePrice.TabStop = false;
            //hidden fields
            {
                metroTile7.Visible = false;
                metroTextBox_Mazdori.Text = "0";
                metroTextBox_Weight.Text = "0";
                metroLabel2.Hide();
                metroComboBoxBrand.Hide();
                metroTile2.Hide();
                metroLabel10.Hide();
                metroLabel13.Hide();
                metroTextBoxBarcode.Hide();
                metroLabel9.Hide();
                metroTextBoxAlertQty.Hide();
                metroCheckBoxHavingMfgExpDate.Hide();
                metroCheckBoxHaveBatchLotNum.Hide();
                metroLabel7.Hide();
                metroTile1.Hide();
                metroTextBoxSKU.Hide();
                metroLabel16.Hide();
                pictureBox1.Hide();
                metroLabel15.Hide();
                metroLabel17.Hide();
                pictureBox2.Hide();
            }


            label4.Hide();
            label2.Hide();
            metroTile4.Hide();
            metroTile6.Hide();
            this.AutoScroll = true;
            metroTextBoxCategory.ReadOnly = true;
            metroTextBoxCategory.Enabled = false;


            if (edit != 1)
            {
                //getBrands();
                getUnits();
                //metroComboBoxBrand.SelectedIndex = 0;
                if (comboBox_Unit.Items.Count > 0)
                {
                    comboBox_Unit.SelectedIndex = 0;
                }
                itemCategory1.treeView1AfterSelect += new EventHandler(treeView1_AfterSelect);//usercontrol->itemcategory work ref 1
                treeView1 = (itemCategory1.Controls["treeView1"] as TreeView);
                metroTextBoxItemName.Focus();
            }

            try
            {
                string query = "select * from categories where id='1'";
                DataTable dt = gm.GetTable(query);
                foreach (DataRow d in dt.Rows)
                {
                    metroTextBoxCategory.Text = d[2].ToString();
                    metroTextBoxCategory.Tag = d[0].ToString();
                }

                dt = gm.GetTable("select * from items where status ='1'");
                foreach (DataRow d in dt.Rows)
                {
                    sourceitemname.Add(d["name"].ToString());
                }

                metroTextBoxItemName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                metroTextBoxItemName.AutoCompleteCustomSource = sourceitemname;
                metroTextBoxItemName.AutoCompleteSource = AutoCompleteSource.CustomSource;

                sourceweight.Add("1.5");
                sourceweight.Add("2.5");
                sourceweight.Add("3.5");
                sourceweight.Add("4.5");
                sourceweight.Add("5.5");
                sourceweight.Add("6.5");
                sourceweight.Add("7.5");
                metroTextBox_Weight.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                metroTextBox_Weight.AutoCompleteCustomSource = sourceweight;
                metroTextBox_Weight.AutoCompleteSource = AutoCompleteSource.CustomSource;

            }
            catch { }
        }

        private void metroLabel4_Click(object sender, EventArgs e)
        {

        }

        private void metroPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void metroTile7_Click(object sender, EventArgs e)
        {
            CreateItemCategory cit = new CreateItemCategory();
            cit.FormBorderStyle = FormBorderStyle.None;
            cit.ShowDialog();
            metroTextBoxCategory.Text = "";
            metroTextBoxCategory.Tag = "";
            itemCategory1.LoadItems();
            treeView1 = (itemCategory1.Controls["treeView1"] as TreeView);
        }

        private void itemCategory1_Click(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show(itemCategory1.Controls["treeView1"].Text);
                TreeView treeView1 = (ParentForm.Controls["treeView1"] as TreeView);
                MessageBox.Show(treeView1.SelectedNode.Text);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        TreeView treeView1;

        private void metroTile2_Click(object sender, EventArgs e)
        {
            Brands b = new Brands();
            b.FormBorderStyle = FormBorderStyle.None;
            b.ShowDialog();
            getBrands();
        }

        GMDB gm = new GMDB();
        int skucount = 0;
        public string getsku()
        {
            try
            {
                string sku = "";
                string[] item = metroTextBoxItemName.Text.Trim().Split(' ');
                foreach(String s in item)
                {
                    sku += s[0].ToString();
                }
                int ok = 0;
                string date = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
                sku += date;
                do
                {
                    skucount++;
                    sku += skucount.ToString();
                    DataTable dt = gm.GetTable("select * from items where sku='" + sku + "'");
                    if (dt.Rows.Count > 0)
                    {
                        getsku();
                    }
                    else
                    {
                        skucount = 0;
                        ok = 1;
                    }
                } while (ok != 1);
                return sku;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public byte[] convertImageToByteArray(System.Drawing.Image imageIn)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        private void metroTile5_Click(object sender, EventArgs e)
        {
            try
            {
                if (metroTextBoxPurchasePrice.Text.ToString().Trim() == "")
                {
                    metroTextBoxPurchasePrice.Text = "0";
                }
                if (metroTextBox_Mazdori.Text.ToString().Trim() == "")
                {
                    metroTextBox_Mazdori.Text = "0";
                }
                if (metroTextBox_Weight.Text.ToString().Trim() == "")
                {
                    metroTextBox_Weight.Text = "0";
                }
                if (metroTextBoxItemName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Enter Item Name");
                    metroTextBoxItemName.Focus();
                    return;
                }
                //if (metroComboBoxBrand.SelectedIndex<0)
                //{
                //    MessageBox.Show("Select Brand");
                //    metroComboBoxBrand.Focus();
                //    return;
                //}
                if (comboBox_Unit.SelectedIndex < 0)
                {
                    MessageBox.Show("Select Unit");
                    comboBox_Unit.Focus();
                    return;
                }
                if (metroTextBoxCategory.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Select Category");
                    try
                    {
                        itemCategory1.Focus();
                    }
                    catch { }
                    return;
                }
                if (metroTextBoxPurchasePrice.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Enter Purchase Price");
                    metroTextBoxPurchasePrice.Focus();
                    return;
                }
                if (metroTextBoxSellingPrice.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Enter Selling Price");
                    metroTextBoxSellingPrice.Focus();
                    return;
                }

                //if (metroTextBoxBarcode.Text.Trim() != string.Empty)
                //{
                //    if (metroTextBoxBarcode.Text.Trim().Length < 8)
                //    {
                //        MessageBox.Show("Barcode must greater or equal to 8 character length.");
                //        metroTextBoxBarcode.Focus();
                //        return;
                //    }
                //    else
                //    {
                //        DataTable dt = gm.GetTable("select * from items where barcode='" + metroTextBoxBarcode.Text.Trim() + "' and id!='"+item_id+"'");
                //        if (dt.Rows.Count > 0)
                //        {
                //            MessageBox.Show("Barcode Already Exist");
                //            metroTextBoxBarcode.Focus();
                //            return;
                //        }
                //        else
                //        {
                //            dt = gm.GetTable("select * from items where name='" + metroTextBoxBarcode.Text.Trim() + "' and id!='" + item_id + "'");
                //            if (dt.Rows.Count > 0)
                //            {
                //                MessageBox.Show("Barcode Already Exist in some items's Name");
                //                metroTextBoxBarcode.Focus();
                //                return;
                //            }
                //            dt = gm.GetTable("select * from items where SKU='" + metroTextBoxBarcode.Text.Trim() + "' and id!='" + item_id + "'");
                //            if (dt.Rows.Count > 0)
                //            {
                //                MessageBox.Show("Barcode Already Exist in some items's SKU");
                //                metroTextBoxBarcode.Focus();
                //                return;
                //            }
                //        }
                //    }
                //}
                //string sku = "";
                //if (metroTextBoxSKU.Text.Trim() != string.Empty)
                //{
                //    DataTable dt = gm.GetTable("select * from items where sku='" + metroTextBoxSKU.Text.Trim() + "' and id!='" + item_id + "'");
                //    if (dt.Rows.Count > 0)
                //    {
                //        MessageBox.Show("SKU Already Exist");
                //        metroTextBoxSKU.Focus();
                //        return;
                //    }
                //    else
                //    {
                //        dt = gm.GetTable("select * from items where name='" + metroTextBoxSKU.Text.Trim() + "' and id!='" + item_id + "'");
                //        if (dt.Rows.Count > 0)
                //        {
                //            MessageBox.Show("SKU Already Exist in some items's Name");
                //            metroTextBoxSKU.Focus();
                //            return;
                //        }
                //        dt = gm.GetTable("select * from items where barcode='" + metroTextBoxSKU.Text.Trim() + "' and id!='" + item_id + "'");
                //        if (dt.Rows.Count > 0)
                //        {
                //            MessageBox.Show("SKU Already Exist in some items's Barcode");
                //            metroTextBoxSKU.Focus();
                //            return;
                //        }
                //    }
                //    sku = metroTextBoxSKU.Text.Trim();
                //}
                //else
                //{
                //    sku = getsku();
                //}
                if (metroTextBoxItemName.Text.Trim() != string.Empty)
                {
                    string unittemp = "";
                    try
                    {
                        unittemp = (((Unit1)comboBox_Unit.SelectedItem).Id.ToString() == "") ? "Other" : ((Unit1)comboBox_Unit.SelectedItem).Id.ToString();
                    }
                    catch { }
                    DataTable dt = gm.GetTable("select * from items where name=N'" + metroTextBoxItemName.Text.Trim() + "' and weight=N'" + metroTextBox_Weight.Text.Trim().ToString() + "' and unit_id=N'" + unittemp + "' and status!='-1'");
                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("Item Already Exist");
                        metroTextBoxItemName.Focus();
                        return;
                    }
                    //else//item name also check in sku and barcode that sku and barcode also not like item name
                    //{
                    //    dt = gm.GetTable("select * from items where barcode='" + metroTextBoxItemName.Text.Trim() + "' and status!='-1' and id!='" + item_id + "'");
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        MessageBox.Show("Item Already Exist in some items's barcode");
                    //        metroTextBoxItemName.Focus();
                    //        return;
                    //    }
                    //    dt = gm.GetTable("select * from items where sku='" + metroTextBoxItemName.Text.Trim() + "' and status!='-1' and id!='" + item_id + "'");
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        MessageBox.Show("Item Already Exist in some items's sku");
                    //        metroTextBoxItemName.Focus();
                    //        return;
                    //    }
                    //}
                }
                else {
                    MessageBox.Show("Enter Item Name");
                    metroTextBoxItemName.Focus();
                }

                //if (((metroTextBoxItemName.Text.Trim() == metroTextBoxBarcode.Text.Trim()) || (metroTextBoxItemName.Text.Trim() == metroTextBoxSKU.Text.Trim())) && (metroTextBoxItemName.Text.Trim()!=""))
                //{
                //    MessageBox.Show("Item Name Must Be Different From Barcode and SKU");
                //    metroTextBoxItemName.Focus();
                //    return;
                //}

                //if (((metroTextBoxBarcode.Text.Trim() == metroTextBoxItemName.Text.Trim()) || (metroTextBoxBarcode.Text.Trim() == metroTextBoxSKU.Text.Trim())) && (metroTextBoxBarcode.Text.Trim() != ""))
                //{
                //    MessageBox.Show("Barcode Must Be Different From Item Name and Barcode");
                //    metroTextBoxBarcode.Focus();
                //    return;
                //}

                //if (((metroTextBoxSKU.Text.Trim() == metroTextBoxBarcode.Text.Trim()) || (metroTextBoxSKU.Text.Trim() == metroTextBoxItemName.Text.Trim())) && (metroTextBoxSKU.Text.Trim() != ""))
                //{
                //    MessageBox.Show("SKU Must Be Different From Item Name and Barcode");
                //    metroTextBoxSKU.Focus();
                //    return;
                //}

                string query = "";

                string alertqty = metroTextBoxAlertQty.Text.Trim();
                if (alertqty == "")
                    alertqty = "0";
                if (edit == 1)
                {
                    try
                    {
                        string brand = (((Brand1)metroComboBoxBrand.SelectedItem).Id.ToString() == "") ? "Other" : ((Brand1)metroComboBoxBrand.SelectedItem).Id.ToString();
                        string unit = (((Unit1)comboBox_Unit.SelectedItem).Id.ToString() == "") ? "Other" : ((Unit1)comboBox_Unit.SelectedItem).Id.ToString();
                        string havingMfgExpDate = (metroCheckBoxHavingMfgExpDate.Checked == true) ? "True" : "False";
                        string havingBatchLotNum = (metroCheckBoxHaveBatchLotNum.Checked == true) ? "True" : "False";
                        Dictionary<string, byte[]> imageBytes = new Dictionary<string, byte[]>(2);
                        imageBytes.Add("@Picture", null);
                        if (pictureBox1.Image != null)
                        {
                            imageBytes["@Picture"] = convertImageToByteArray(pictureBox1.Image);
                        }

                        try
                        {
                            query = "select * from items where name='" + metroTextBoxItemName.Text.Trim().ToString() + "' and weight='" + metroTextBox_Weight.Text.Trim().ToString() + "' and unit_id='" + unit + "' and category_id='" + metroTextBoxCategory.Tag.ToString().Trim().ToString() + "' and status!='-1'";
                            DataTable dt1 = gm.GetTable(query);
                            if (dt1.Rows.Count > 0)
                            {
                                MessageBox.Show("Item Already Exist");
                                return;
                            }
                        }
                        catch { }
                        string sku = "";
                        query = "update items set name=N'" + metroTextBoxItemName.Text.Trim() + "',sku='" + sku + "',brand_id='" + brand + "',unit_id='" + unit + "',category_id='" + metroTextBoxCategory.Tag.ToString().Trim().ToString() + "',barcode='" + metroTextBoxBarcode.Text.Trim() + "',Picture=@Picture,Mazdori='" + metroTextBox_Mazdori.Text.Trim() + "',weight='" + metroTextBox_Weight.Text.Trim() + "',purchase_price='" + metroTextBoxPurchasePrice.Text.Trim() + "',retail_price='" + metroTextBoxSellingPrice.Text.Trim() + "',have_mfg_or_exp_date='" + havingMfgExpDate + "',have_batch_or_lot_no='" + havingBatchLotNum + "',alertqty='" + alertqty + "',detail=N'" + metroTextBoxItemDescription.Text.Trim() + "',AddedBy_UserId='" + RJ.Properties.Settings.Default.loginid.ToString() + "',_Date='" + DateTime.Now.ToShortDateString() + "',_Time='" + DateTime.Now.ToShortTimeString() + "' where id = '" + item_id + "'";
                        if (gm.ExecuteSQLWithImages(query, imageBytes) > 0)
                        {
                            MessageBox.Show("Successfully Saved...");
                            this.Dispose(true);
                        }
                        else
                        {
                            MessageBox.Show("Error");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }                    
                }
                if (edit != 1)
                {
                    string id = gm.MaxId("select max(cast(id as int)) from items");
                    //string brand = (((Brand)metroComboBoxBrand.SelectedItem).Id.ToString() == "") ? "Other" : ((Brand)metroComboBoxBrand.SelectedItem).Id.ToString();
                    string brand = "Other";
                    string unit = (((Unit1)comboBox_Unit.SelectedItem).Id.ToString() == "") ? "Other" : ((Unit1)comboBox_Unit.SelectedItem).Id.ToString();
                    string havingMfgExpDate = (metroCheckBoxHavingMfgExpDate.Checked == true) ? "True" : "False";
                    string havingBatchLotNum = (metroCheckBoxHaveBatchLotNum.Checked == true) ? "True" : "False";
                    Dictionary<string, byte[]> imageBytes = new Dictionary<string, byte[]>(2);
                    imageBytes.Add("@Picture", null);
                    if (pictureBox1.Image != null)
                    {
                        imageBytes["@Picture"] = convertImageToByteArray(pictureBox1.Image);
                    }

                    try
                    {
                        query = "select * from items where name='" + metroTextBoxItemName.Text.Trim().ToString() + "' and weight='" + metroTextBox_Weight.Text.Trim().ToString() + "' and unit_id='" + unit + "' and category_id='" + metroTextBoxCategory.Tag.ToString().Trim().ToString() + "' and status!='-1'";
                        DataTable dt1 = gm.GetTable(query);
                        if (dt1.Rows.Count > 0)
                        {
                            MessageBox.Show("Item Already Exist");
                            return;
                        }
                    }
                    catch { }
                    string sku = "";
                    query = "insert into items values('" + id + "',N'" + sku + "',N'" + metroTextBoxItemName.Text.Trim() + "',N'" + brand + "',N'" + unit + "','" + metroTextBoxCategory.Tag.ToString().Trim().ToString() + "','" + metroTextBoxBarcode.Text.Trim() + "',@Picture,'" + metroTextBoxPurchasePrice.Text.Trim() + "','" + metroTextBoxSellingPrice.Text.Trim() + "','" + havingMfgExpDate + "','" + havingBatchLotNum + "','" + alertqty + "','0',N'" + metroTextBoxItemDescription.Text.Trim() + "','" + RJ.Properties.Settings.Default.loginid.ToString() + "','" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.ToShortTimeString() + "','1','" + metroTextBox_Weight.Text.ToString().Trim() + "','','" + DateTime.Now.ToShortDateString() + "','" + metroTextBox_Mazdori.Text.Trim() + "')";
                    if (gm.ExecuteSQLWithImages(query, imageBytes) > 0)
                        MessageBox.Show("Successfully Saved...");
                    reset_fields();
                    metroTextBoxItemName.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroTextBoxPurchasePrice_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBoxPurchasePrice_TextChanged(object sender, EventArgs e)
        {
            gm.AcceptDouble(sender, e);
        }

        private void metroTextBoxSellingPrice_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBoxAlertQty_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBoxAlertQty_TextChanged(object sender, EventArgs e)
        {
            gm.AcceptDouble(sender, e);
        }

        private void metroTextBoxSellingPrice_TextChanged(object sender, EventArgs e)
        {
            gm.AcceptDouble(sender, e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                string str = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
                openFileDialog.Filter = str;
                openFileDialog.Title = "Select an image";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = new Bitmap(openFileDialog.FileName);
                    pictureBox1.BringToFront();
                    Image i = pictureBox1.Image;
                    pictureBox1.Image = ScaleImage(i, pictureBox1.Width, pictureBox1.Height);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Select an image please");
            }
            if (pictureBox1.Image != null)
            {
                metroLabel15.Text = "Remove Image";
            }
            else
            {
                metroLabel15.Text = "Upload Image";
            }
        }

        private void metroTile4_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                string str = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
                openFileDialog.Filter = str;
                openFileDialog.Title = "Select an image";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = new Bitmap(openFileDialog.FileName);
                    pictureBox1.BringToFront();
                    Image i = pictureBox1.Image;
                    pictureBox1.Image = ScaleImage(i, pictureBox1.Width, pictureBox1.Height);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Select an image please");
            }
        }

        private void metroLabel15_Click(object sender, EventArgs e)
        {
            if (metroLabel15.Text == "Remove Image")
            {
                pictureBox1.Image = null;
            }
            else
            {
                try
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    string str = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
                    openFileDialog.Filter = str;
                    openFileDialog.Title = "Select an image";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        pictureBox1.Image = new Bitmap(openFileDialog.FileName);
                        pictureBox1.BringToFront();
                        Image i = pictureBox1.Image;
                        pictureBox1.Image = ScaleImage(i, pictureBox1.Width, pictureBox1.Height);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Select an image please");
                }
            }
            if (pictureBox1.Image != null)
            {
                metroLabel15.Text = "Remove Image";
            }
            else
            {
                metroLabel15.Text = "Upload Image";
            }
        }

        private void metroTextBoxBarcode_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBoxBarcode_TextChanged(object sender, EventArgs e)
        {
            if (metroTextBoxBarcode.Text.Trim().Length > 7)
            {
                BarcodeLib.Barcode barcode = new BarcodeLib.Barcode();
                barcode.IncludeLabel = true;
                barcode.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;
                pictureBox2.Image = barcode.Encode(BarcodeLib.TYPE.CODE128, metroTextBoxBarcode.Text.Trim(), 370, 70);
            }
            else if (pictureBox2.Image != null)
                pictureBox2.Image = null;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void metroTile3_Click(object sender, EventArgs e)
        {
            Units u = new Units();
            u.FormBorderStyle = FormBorderStyle.None;
            u.ShowDialog();
            getUnits();
        }

        private void AddItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Dispose(true);
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.S)
                metroTile5.PerformClick();
            if (e.KeyCode == Keys.F1)
                metroTextBoxItemName.Focus();
            if (e.KeyCode == Keys.F5)
                reset_fields();
        }

        private void metroLabel18_Click(object sender, EventArgs e)
        {
            reset_fields();
        }

        private void metroTextBoxItemDescription_Click(object sender, EventArgs e)
        {

        }

        private void metroTile4_Click_1(object sender, EventArgs e)
        {
            try
            {

            }
            catch { }
        }

        private void metroTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void metroTextBox1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            gm.AcceptDouble(sender, e);
        }

        private void metroTextBox_Mazdori_TextChanged(object sender, EventArgs e)
        {
            gm.AcceptDouble(sender, e);
        }

        private void itemCategory1_Load(object sender, EventArgs e)
        {

        }

        private void metroTextBox_Mazdori_Click(object sender, EventArgs e)
        {

        }

        private void metroTile6_Click(object sender, EventArgs e)
        {

        }
    }

    public class Brand1
    {
        public Brand1(string name, string id)
        {
            this.Name = name; this.Id = id;
        }
        public string Name
        { get; set; }
        public string Id
        { get; set; }
    }

    public class Unit1
    {
        public Unit1(string name, string id)
        {
            this.Name = name; this.Id = id;
        }
        public string Name
        { get; set; }
        public string Id
        { get; set; }
    }
}
