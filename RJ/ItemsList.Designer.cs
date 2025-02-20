namespace RJ
{
    partial class ItemsList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.Column14 = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Column15 = new DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.metroTile3 = new MetroFramework.Controls.MetroTile();
            this.metroLabel8 = new MetroFramework.Controls.MetroLabel();
            this.metroDateTimeFrom = new MetroFramework.Controls.MetroDateTime();
            this.label1 = new System.Windows.Forms.Label();
            this.metroTile2 = new MetroFramework.Controls.MetroTile();
            this.metroRadioButton3 = new MetroFramework.Controls.MetroRadioButton();
            this.itemCategory1 = new RJ.ItemCategory();
            this.metroTextBoxCategory = new MetroFramework.Controls.MetroTextBox();
            this.metroRadioButton2 = new MetroFramework.Controls.MetroRadioButton();
            this.metroRadioButton1 = new MetroFramework.Controls.MetroRadioButton();
            this.metroTile1 = new MetroFramework.Controls.MetroTile();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.metroPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column14,
            this.Column8,
            this.Column9,
            this.Column13,
            this.Column10,
            this.Column11,
            this.Column12,
            this.Column15});
            this.dataGridView1.Location = new System.Drawing.Point(3, 119);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridView1.Size = new System.Drawing.Size(860, 419);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.a);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "id";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Visible = false;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "SKU";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Visible = false;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Category";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Item Name";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Brand";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Visible = false;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Unit";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // Column7
            // 
            // 
            // 
            // 
            this.Column7.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.Column7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Column7.HeaderText = "Retail Price";
            this.Column7.Increment = 1D;
            this.Column7.Name = "Column7";
            this.Column7.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Column14
            // 
            // 
            // 
            // 
            this.Column14.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.Column14.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Column14.HeaderText = "Mazdori";
            this.Column14.Increment = 1D;
            this.Column14.Name = "Column14";
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Purchase Price";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Visible = false;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Quantity";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.Visible = false;
            // 
            // Column13
            // 
            this.Column13.HeaderText = "Alert Qty";
            this.Column13.Name = "Column13";
            this.Column13.ReadOnly = true;
            this.Column13.Visible = false;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "Barcode";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            this.Column10.Visible = false;
            // 
            // Column11
            // 
            this.Column11.HeaderText = "Enable / Disable";
            this.Column11.Name = "Column11";
            this.Column11.ReadOnly = true;
            // 
            // Column12
            // 
            this.Column12.HeaderText = "Delete";
            this.Column12.Name = "Column12";
            this.Column12.ReadOnly = true;
            this.Column12.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column12.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Column15
            // 
            // 
            // 
            // 
            this.Column15.BackgroundStyle.Class = "DataGridViewNumericBorder";
            this.Column15.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Column15.HeaderText = "Weight";
            this.Column15.Increment = 1D;
            this.Column15.Name = "Column15";
            this.Column15.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // metroPanel1
            // 
            this.metroPanel1.Controls.Add(this.comboBox1);
            this.metroPanel1.Controls.Add(this.label2);
            this.metroPanel1.Controls.Add(this.metroTile3);
            this.metroPanel1.Controls.Add(this.metroLabel8);
            this.metroPanel1.Controls.Add(this.metroDateTimeFrom);
            this.metroPanel1.Controls.Add(this.label1);
            this.metroPanel1.Controls.Add(this.metroTile2);
            this.metroPanel1.Controls.Add(this.metroRadioButton3);
            this.metroPanel1.Controls.Add(this.itemCategory1);
            this.metroPanel1.Controls.Add(this.metroTextBoxCategory);
            this.metroPanel1.Controls.Add(this.metroRadioButton2);
            this.metroPanel1.Controls.Add(this.metroRadioButton1);
            this.metroPanel1.Controls.Add(this.dataGridView1);
            this.metroPanel1.Controls.Add(this.metroTile1);
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(15, 31);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(863, 541);
            this.metroPanel1.TabIndex = 1;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            this.metroPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.metroPanel1_Paint);
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(257, 66);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.comboBox1.Size = new System.Drawing.Size(226, 26);
            this.comboBox1.TabIndex = 11112;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.comboBox1.TextChanged += new System.EventHandler(this.comboBox1_TextChanged);
            this.comboBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox1_KeyDown);
            this.comboBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.comboBox1_KeyUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(791, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 15);
            this.label2.TabIndex = 105;
            this.label2.Text = "Ctrl + S";
            // 
            // metroTile3
            // 
            this.metroTile3.ActiveControl = null;
            this.metroTile3.Location = new System.Drawing.Point(778, 5);
            this.metroTile3.Name = "metroTile3";
            this.metroTile3.Size = new System.Drawing.Size(82, 97);
            this.metroTile3.TabIndex = 104;
            this.metroTile3.Text = "UPDATE";
            this.metroTile3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.metroTile3.TileImage = global::RJ.Properties.Resources.if_pen_checkbox_353430;
            this.metroTile3.TileImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.metroTile3.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Regular;
            this.metroTile3.UseSelectable = true;
            this.metroTile3.UseTileImage = true;
            this.metroTile3.Click += new System.EventHandler(this.metroTile3_Click);
            // 
            // metroLabel8
            // 
            this.metroLabel8.AutoSize = true;
            this.metroLabel8.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel8.Location = new System.Drawing.Point(257, 1);
            this.metroLabel8.Name = "metroLabel8";
            this.metroLabel8.Size = new System.Drawing.Size(40, 19);
            this.metroLabel8.TabIndex = 103;
            this.metroLabel8.Text = "Date";
            // 
            // metroDateTimeFrom
            // 
            this.metroDateTimeFrom.FontSize = MetroFramework.MetroDateTimeSize.Small;
            this.metroDateTimeFrom.Location = new System.Drawing.Point(257, 23);
            this.metroDateTimeFrom.MinimumSize = new System.Drawing.Size(0, 25);
            this.metroDateTimeFrom.Name = "metroDateTimeFrom";
            this.metroDateTimeFrom.Size = new System.Drawing.Size(227, 25);
            this.metroDateTimeFrom.TabIndex = 102;
            this.metroDateTimeFrom.ValueChanged += new System.EventHandler(this.metroDateTimeFrom_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(702, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 15);
            this.label1.TabIndex = 100;
            this.label1.Text = "Ctrl + P";
            // 
            // metroTile2
            // 
            this.metroTile2.ActiveControl = null;
            this.metroTile2.Location = new System.Drawing.Point(688, 5);
            this.metroTile2.Name = "metroTile2";
            this.metroTile2.Size = new System.Drawing.Size(82, 97);
            this.metroTile2.TabIndex = 99;
            this.metroTile2.Text = "Print";
            this.metroTile2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.metroTile2.TileImage = global::RJ.Properties.Resources.if_office_19_809604;
            this.metroTile2.TileImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.metroTile2.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Regular;
            this.metroTile2.UseSelectable = true;
            this.metroTile2.UseTileImage = true;
            this.metroTile2.Click += new System.EventHandler(this.metroTile2_Click);
            // 
            // metroRadioButton3
            // 
            this.metroRadioButton3.AutoSize = true;
            this.metroRadioButton3.FontSize = MetroFramework.MetroCheckBoxSize.Tall;
            this.metroRadioButton3.Location = new System.Drawing.Point(13, 67);
            this.metroRadioButton3.Name = "metroRadioButton3";
            this.metroRadioButton3.Size = new System.Drawing.Size(140, 25);
            this.metroRadioButton3.TabIndex = 14;
            this.metroRadioButton3.Text = "By Item Name";
            this.metroRadioButton3.UseSelectable = true;
            this.metroRadioButton3.CheckedChanged += new System.EventHandler(this.metroRadioButton3_CheckedChanged);
            // 
            // itemCategory1
            // 
            this.itemCategory1.AutoScroll = true;
            this.itemCategory1.BackColor = System.Drawing.Color.White;
            this.itemCategory1.Location = new System.Drawing.Point(257, 40);
            this.itemCategory1.Name = "itemCategory1";
            this.itemCategory1.Size = new System.Drawing.Size(226, 75);
            this.itemCategory1.TabIndex = 13;
            this.itemCategory1.Load += new System.EventHandler(this.itemCategory1_Load);
            // 
            // metroTextBoxCategory
            // 
            // 
            // 
            // 
            this.metroTextBoxCategory.CustomButton.Image = null;
            this.metroTextBoxCategory.CustomButton.Location = new System.Drawing.Point(198, 1);
            this.metroTextBoxCategory.CustomButton.Name = "";
            this.metroTextBoxCategory.CustomButton.Size = new System.Drawing.Size(27, 27);
            this.metroTextBoxCategory.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBoxCategory.CustomButton.TabIndex = 1;
            this.metroTextBoxCategory.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBoxCategory.CustomButton.UseSelectable = true;
            this.metroTextBoxCategory.CustomButton.Visible = false;
            this.metroTextBoxCategory.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.metroTextBoxCategory.Lines = new string[0];
            this.metroTextBoxCategory.Location = new System.Drawing.Point(257, 5);
            this.metroTextBoxCategory.MaxLength = 32767;
            this.metroTextBoxCategory.Name = "metroTextBoxCategory";
            this.metroTextBoxCategory.PasswordChar = '\0';
            this.metroTextBoxCategory.PromptText = "Enter Item Category";
            this.metroTextBoxCategory.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBoxCategory.SelectedText = "";
            this.metroTextBoxCategory.SelectionLength = 0;
            this.metroTextBoxCategory.SelectionStart = 0;
            this.metroTextBoxCategory.ShortcutsEnabled = true;
            this.metroTextBoxCategory.Size = new System.Drawing.Size(226, 29);
            this.metroTextBoxCategory.TabIndex = 12;
            this.metroTextBoxCategory.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.metroTextBoxCategory.UseSelectable = true;
            this.metroTextBoxCategory.WaterMark = "Enter Item Category";
            this.metroTextBoxCategory.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBoxCategory.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.metroTextBoxCategory.TextChanged += new System.EventHandler(this.metroTextBoxCategory_TextChanged);
            this.metroTextBoxCategory.Click += new System.EventHandler(this.metroTextBoxCategory_Click);
            // 
            // metroRadioButton2
            // 
            this.metroRadioButton2.AutoSize = true;
            this.metroRadioButton2.FontSize = MetroFramework.MetroCheckBoxSize.Tall;
            this.metroRadioButton2.Location = new System.Drawing.Point(13, 36);
            this.metroRadioButton2.Name = "metroRadioButton2";
            this.metroRadioButton2.Size = new System.Drawing.Size(124, 25);
            this.metroRadioButton2.TabIndex = 10;
            this.metroRadioButton2.Text = "By Category";
            this.metroRadioButton2.UseSelectable = true;
            this.metroRadioButton2.CheckedChanged += new System.EventHandler(this.metroRadioButton2_CheckedChanged);
            // 
            // metroRadioButton1
            // 
            this.metroRadioButton1.AutoSize = true;
            this.metroRadioButton1.FontSize = MetroFramework.MetroCheckBoxSize.Tall;
            this.metroRadioButton1.Location = new System.Drawing.Point(13, 5);
            this.metroRadioButton1.Name = "metroRadioButton1";
            this.metroRadioButton1.Size = new System.Drawing.Size(90, 25);
            this.metroRadioButton1.TabIndex = 9;
            this.metroRadioButton1.Text = "View All";
            this.metroRadioButton1.UseSelectable = true;
            this.metroRadioButton1.CheckedChanged += new System.EventHandler(this.metroRadioButton1_CheckedChanged);
            // 
            // metroTile1
            // 
            this.metroTile1.ActiveControl = null;
            this.metroTile1.Location = new System.Drawing.Point(601, 422);
            this.metroTile1.Name = "metroTile1";
            this.metroTile1.Size = new System.Drawing.Size(82, 97);
            this.metroTile1.TabIndex = 84;
            this.metroTile1.Text = "EDIT";
            this.metroTile1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.metroTile1.TileImage = global::RJ.Properties.Resources.if_pen_checkbox_353430;
            this.metroTile1.TileImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.metroTile1.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Regular;
            this.metroTile1.UseSelectable = true;
            this.metroTile1.UseTileImage = true;
            this.metroTile1.Click += new System.EventHandler(this.metroTile1_Click);
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel4.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel4.Location = new System.Drawing.Point(12, 4);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(87, 25);
            this.metroLabel4.TabIndex = 8;
            this.metroLabel4.Text = "Items List";
            this.metroLabel4.UseCustomBackColor = true;
            // 
            // ItemsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(889, 584);
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.metroPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "ItemsList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ItemsList";
            this.Load += new System.EventHandler(this.ItemsList_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ItemsList_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.metroPanel1.ResumeLayout(false);
            this.metroPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private MetroFramework.Controls.MetroPanel metroPanel1;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroRadioButton metroRadioButton1;
        private MetroFramework.Controls.MetroRadioButton metroRadioButton2;
        private MetroFramework.Controls.MetroTextBox metroTextBoxCategory;
        private ItemCategory itemCategory1;
        private MetroFramework.Controls.MetroRadioButton metroRadioButton3;
        private MetroFramework.Controls.MetroTile metroTile1;
        private System.Windows.Forms.Label label1;
        private MetroFramework.Controls.MetroTile metroTile2;
        private MetroFramework.Controls.MetroLabel metroLabel8;
        private MetroFramework.Controls.MetroDateTime metroDateTimeFrom;
        private MetroFramework.Controls.MetroTile metroTile3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn Column7;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn Column14;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewButtonColumn Column11;
        private System.Windows.Forms.DataGridViewButtonColumn Column12;
        private DevComponents.DotNetBar.Controls.DataGridViewDoubleInputColumn Column15;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}