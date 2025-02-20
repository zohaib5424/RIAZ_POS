namespace RJ
{
    partial class LowStockItemsList
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
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.metroTile1 = new MetroFramework.Controls.MetroTile();
            this.itemCategory1 = new RJ.ItemCategory();
            this.metroTextBoxCategory = new MetroFramework.Controls.MetroTextBox();
            this.metroRadioButton2 = new MetroFramework.Controls.MetroRadioButton();
            this.metroRadioButton1 = new MetroFramework.Controls.MetroRadioButton();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.metroPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column13,
            this.Column10,
            this.Column11,
            this.Column12});
            this.dataGridView1.Location = new System.Drawing.Point(3, 124);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(859, 260);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
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
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Unit";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Retail Price";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Purchase Price";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Quantity";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            // 
            // Column13
            // 
            this.Column13.HeaderText = "Alert Qty";
            this.Column13.Name = "Column13";
            this.Column13.ReadOnly = true;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "Barcode";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
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
            // metroPanel1
            // 
            this.metroPanel1.Controls.Add(this.label1);
            this.metroPanel1.Controls.Add(this.metroTile1);
            this.metroPanel1.Controls.Add(this.itemCategory1);
            this.metroPanel1.Controls.Add(this.metroTextBoxCategory);
            this.metroPanel1.Controls.Add(this.metroRadioButton2);
            this.metroPanel1.Controls.Add(this.metroRadioButton1);
            this.metroPanel1.Controls.Add(this.dataGridView1);
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(15, 36);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(862, 385);
            this.metroPanel1.TabIndex = 1;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            this.metroPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.metroPanel1_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(794, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 15);
            this.label1.TabIndex = 98;
            this.label1.Text = "Ctrl + P";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // metroTile1
            // 
            this.metroTile1.ActiveControl = null;
            this.metroTile1.Location = new System.Drawing.Point(780, 9);
            this.metroTile1.Name = "metroTile1";
            this.metroTile1.Size = new System.Drawing.Size(82, 97);
            this.metroTile1.TabIndex = 97;
            this.metroTile1.Text = "Print";
            this.metroTile1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.metroTile1.TileImage = global::RJ.Properties.Resources.if_office_19_809604;
            this.metroTile1.TileImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.metroTile1.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Regular;
            this.metroTile1.UseSelectable = true;
            this.metroTile1.UseTileImage = true;
            this.metroTile1.Click += new System.EventHandler(this.metroTile1_Click_1);
            // 
            // itemCategory1
            // 
            this.itemCategory1.AutoScroll = true;
            this.itemCategory1.BackColor = System.Drawing.Color.White;
            this.itemCategory1.Location = new System.Drawing.Point(257, 35);
            this.itemCategory1.Name = "itemCategory1";
            this.itemCategory1.Size = new System.Drawing.Size(226, 80);
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
            this.metroTextBoxCategory.Location = new System.Drawing.Point(257, 2);
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
            this.metroRadioButton2.Location = new System.Drawing.Point(13, 40);
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
            this.metroRadioButton1.Location = new System.Drawing.Point(13, 9);
            this.metroRadioButton1.Name = "metroRadioButton1";
            this.metroRadioButton1.Size = new System.Drawing.Size(90, 25);
            this.metroRadioButton1.TabIndex = 9;
            this.metroRadioButton1.Text = "View All";
            this.metroRadioButton1.UseSelectable = true;
            this.metroRadioButton1.CheckedChanged += new System.EventHandler(this.metroRadioButton1_CheckedChanged);
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel4.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel4.Location = new System.Drawing.Point(12, 9);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(87, 25);
            this.metroLabel4.TabIndex = 8;
            this.metroLabel4.Text = "Items List";
            this.metroLabel4.UseCustomBackColor = true;
            // 
            // LowStockItemsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(888, 439);
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.metroPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "LowStockItemsList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ItemsList";
            this.Load += new System.EventHandler(this.ItemsList_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LowStockItemsList_KeyDown);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewButtonColumn Column11;
        private System.Windows.Forms.DataGridViewButtonColumn Column12;
        private System.Windows.Forms.Label label1;
        private MetroFramework.Controls.MetroTile metroTile1;
    }
}