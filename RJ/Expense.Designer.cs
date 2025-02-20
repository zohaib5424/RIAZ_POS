namespace RJ
{
    partial class Expense
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
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBoxExpneseName = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.metroTextBoxPaidAmount = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
            this.metroTextBoxDescription = new MetroFramework.Controls.MetroTextBox();
            this.metroTile5 = new MetroFramework.Controls.MetroTile();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.metroDateTime1 = new MetroFramework.Controls.MetroDateTime();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroTextBoxAmount = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
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
            this.Column4,
            this.Column1,
            this.Column2,
            this.Column6,
            this.Column5,
            this.Column3});
            this.dataGridView1.Location = new System.Drawing.Point(18, 228);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(517, 241);
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // Column4
            // 
            this.Column4.HeaderText = "id";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Visible = false;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Expense Name";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Amount";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 70;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Paid Amount";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 70;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Date";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 70;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Description";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 160;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBoxExpneseName
            // 
            // 
            // 
            // 
            this.textBoxExpneseName.CustomButton.Image = null;
            this.textBoxExpneseName.CustomButton.Location = new System.Drawing.Point(192, 1);
            this.textBoxExpneseName.CustomButton.Name = "";
            this.textBoxExpneseName.CustomButton.Size = new System.Drawing.Size(27, 27);
            this.textBoxExpneseName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.textBoxExpneseName.CustomButton.TabIndex = 1;
            this.textBoxExpneseName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.textBoxExpneseName.CustomButton.UseSelectable = true;
            this.textBoxExpneseName.CustomButton.Visible = false;
            this.textBoxExpneseName.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.textBoxExpneseName.Lines = new string[0];
            this.textBoxExpneseName.Location = new System.Drawing.Point(124, 49);
            this.textBoxExpneseName.MaxLength = 32767;
            this.textBoxExpneseName.Name = "textBoxExpneseName";
            this.textBoxExpneseName.PasswordChar = '\0';
            this.textBoxExpneseName.PromptText = "Enter Expense Name";
            this.textBoxExpneseName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxExpneseName.SelectedText = "";
            this.textBoxExpneseName.SelectionLength = 0;
            this.textBoxExpneseName.SelectionStart = 0;
            this.textBoxExpneseName.ShortcutsEnabled = true;
            this.textBoxExpneseName.Size = new System.Drawing.Size(220, 29);
            this.textBoxExpneseName.TabIndex = 2;
            this.textBoxExpneseName.UseSelectable = true;
            this.textBoxExpneseName.WaterMark = "Enter Expense Name";
            this.textBoxExpneseName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.textBoxExpneseName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(18, 54);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(96, 19);
            this.metroLabel1.TabIndex = 15;
            this.metroLabel1.Text = "Expense Name";
            // 
            // metroPanel1
            // 
            this.metroPanel1.Controls.Add(this.metroTextBoxDescription);
            this.metroPanel1.Controls.Add(this.metroTile5);
            this.metroPanel1.Controls.Add(this.metroLabel5);
            this.metroPanel1.Controls.Add(this.metroDateTime1);
            this.metroPanel1.Controls.Add(this.metroLabel3);
            this.metroPanel1.Controls.Add(this.metroTextBoxAmount);
            this.metroPanel1.Controls.Add(this.metroLabel2);
            this.metroPanel1.Controls.Add(this.dataGridView1);
            this.metroPanel1.Controls.Add(this.textBoxExpneseName);
            this.metroPanel1.Controls.Add(this.metroLabel1);
            this.metroPanel1.Controls.Add(this.metroTextBoxPaidAmount);
            this.metroPanel1.Controls.Add(this.metroLabel6);
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(14, 34);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(541, 478);
            this.metroPanel1.TabIndex = 19;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            this.metroPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.metroPanel1_Paint);
            // 
            // metroTextBoxPaidAmount
            // 
            // 
            // 
            // 
            this.metroTextBoxPaidAmount.CustomButton.Image = null;
            this.metroTextBoxPaidAmount.CustomButton.Location = new System.Drawing.Point(192, 1);
            this.metroTextBoxPaidAmount.CustomButton.Name = "";
            this.metroTextBoxPaidAmount.CustomButton.Size = new System.Drawing.Size(27, 27);
            this.metroTextBoxPaidAmount.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBoxPaidAmount.CustomButton.TabIndex = 1;
            this.metroTextBoxPaidAmount.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBoxPaidAmount.CustomButton.UseSelectable = true;
            this.metroTextBoxPaidAmount.CustomButton.Visible = false;
            this.metroTextBoxPaidAmount.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.metroTextBoxPaidAmount.Lines = new string[0];
            this.metroTextBoxPaidAmount.Location = new System.Drawing.Point(124, 119);
            this.metroTextBoxPaidAmount.MaxLength = 32767;
            this.metroTextBoxPaidAmount.Name = "metroTextBoxPaidAmount";
            this.metroTextBoxPaidAmount.PasswordChar = '\0';
            this.metroTextBoxPaidAmount.PromptText = "Enter Paid Amount";
            this.metroTextBoxPaidAmount.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBoxPaidAmount.SelectedText = "";
            this.metroTextBoxPaidAmount.SelectionLength = 0;
            this.metroTextBoxPaidAmount.SelectionStart = 0;
            this.metroTextBoxPaidAmount.ShortcutsEnabled = true;
            this.metroTextBoxPaidAmount.Size = new System.Drawing.Size(220, 29);
            this.metroTextBoxPaidAmount.TabIndex = 4;
            this.metroTextBoxPaidAmount.UseSelectable = true;
            this.metroTextBoxPaidAmount.WaterMark = "Enter Paid Amount";
            this.metroTextBoxPaidAmount.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBoxPaidAmount.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.metroTextBoxPaidAmount.TextChanged += new System.EventHandler(this.metroTextBox1_TextChanged_1);
            // 
            // metroLabel6
            // 
            this.metroLabel6.AutoSize = true;
            this.metroLabel6.Location = new System.Drawing.Point(18, 124);
            this.metroLabel6.Name = "metroLabel6";
            this.metroLabel6.Size = new System.Drawing.Size(85, 19);
            this.metroLabel6.TabIndex = 25;
            this.metroLabel6.Text = "Paid Amount";
            // 
            // metroTextBoxDescription
            // 
            // 
            // 
            // 
            this.metroTextBoxDescription.CustomButton.Image = null;
            this.metroTextBoxDescription.CustomButton.Location = new System.Drawing.Point(160, 1);
            this.metroTextBoxDescription.CustomButton.Name = "";
            this.metroTextBoxDescription.CustomButton.Size = new System.Drawing.Size(59, 59);
            this.metroTextBoxDescription.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBoxDescription.CustomButton.TabIndex = 1;
            this.metroTextBoxDescription.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBoxDescription.CustomButton.UseSelectable = true;
            this.metroTextBoxDescription.CustomButton.Visible = false;
            this.metroTextBoxDescription.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.metroTextBoxDescription.Lines = new string[0];
            this.metroTextBoxDescription.Location = new System.Drawing.Point(124, 119);
            this.metroTextBoxDescription.MaxLength = 32767;
            this.metroTextBoxDescription.Multiline = true;
            this.metroTextBoxDescription.Name = "metroTextBoxDescription";
            this.metroTextBoxDescription.PasswordChar = '\0';
            this.metroTextBoxDescription.PromptText = "Expense Description";
            this.metroTextBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBoxDescription.SelectedText = "";
            this.metroTextBoxDescription.SelectionLength = 0;
            this.metroTextBoxDescription.SelectionStart = 0;
            this.metroTextBoxDescription.ShortcutsEnabled = true;
            this.metroTextBoxDescription.Size = new System.Drawing.Size(220, 61);
            this.metroTextBoxDescription.TabIndex = 5;
            this.metroTextBoxDescription.UseSelectable = true;
            this.metroTextBoxDescription.WaterMark = "Expense Description";
            this.metroTextBoxDescription.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBoxDescription.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroTile5
            // 
            this.metroTile5.ActiveControl = null;
            this.metroTile5.Location = new System.Drawing.Point(124, 186);
            this.metroTile5.Name = "metroTile5";
            this.metroTile5.Size = new System.Drawing.Size(161, 36);
            this.metroTile5.TabIndex = 6;
            this.metroTile5.Text = "SAVE";
            this.metroTile5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.metroTile5.TileImage = global::RJ.Properties.Resources.if_save_173091;
            this.metroTile5.TileImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.metroTile5.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Regular;
            this.metroTile5.UseSelectable = true;
            this.metroTile5.UseTileImage = true;
            this.metroTile5.Click += new System.EventHandler(this.metroTile5_Click);
            // 
            // metroLabel5
            // 
            this.metroLabel5.AutoSize = true;
            this.metroLabel5.Location = new System.Drawing.Point(19, 24);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new System.Drawing.Size(36, 19);
            this.metroLabel5.TabIndex = 21;
            this.metroLabel5.Text = "Date";
            // 
            // metroDateTime1
            // 
            this.metroDateTime1.FontSize = MetroFramework.MetroDateTimeSize.Small;
            this.metroDateTime1.Location = new System.Drawing.Point(124, 18);
            this.metroDateTime1.MinimumSize = new System.Drawing.Size(0, 25);
            this.metroDateTime1.Name = "metroDateTime1";
            this.metroDateTime1.Size = new System.Drawing.Size(220, 25);
            this.metroDateTime1.TabIndex = 1;
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(18, 124);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(74, 19);
            this.metroLabel3.TabIndex = 19;
            this.metroLabel3.Text = "Description";
            // 
            // metroTextBoxAmount
            // 
            // 
            // 
            // 
            this.metroTextBoxAmount.CustomButton.Image = null;
            this.metroTextBoxAmount.CustomButton.Location = new System.Drawing.Point(192, 1);
            this.metroTextBoxAmount.CustomButton.Name = "";
            this.metroTextBoxAmount.CustomButton.Size = new System.Drawing.Size(27, 27);
            this.metroTextBoxAmount.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBoxAmount.CustomButton.TabIndex = 1;
            this.metroTextBoxAmount.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBoxAmount.CustomButton.UseSelectable = true;
            this.metroTextBoxAmount.CustomButton.Visible = false;
            this.metroTextBoxAmount.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.metroTextBoxAmount.Lines = new string[0];
            this.metroTextBoxAmount.Location = new System.Drawing.Point(124, 84);
            this.metroTextBoxAmount.MaxLength = 32767;
            this.metroTextBoxAmount.Name = "metroTextBoxAmount";
            this.metroTextBoxAmount.PasswordChar = '\0';
            this.metroTextBoxAmount.PromptText = "Enter Expense Amount";
            this.metroTextBoxAmount.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBoxAmount.SelectedText = "";
            this.metroTextBoxAmount.SelectionLength = 0;
            this.metroTextBoxAmount.SelectionStart = 0;
            this.metroTextBoxAmount.ShortcutsEnabled = true;
            this.metroTextBoxAmount.Size = new System.Drawing.Size(220, 29);
            this.metroTextBoxAmount.TabIndex = 3;
            this.metroTextBoxAmount.UseSelectable = true;
            this.metroTextBoxAmount.WaterMark = "Enter Expense Amount";
            this.metroTextBoxAmount.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBoxAmount.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.metroTextBoxAmount.TextChanged += new System.EventHandler(this.metroTextBox1_TextChanged);
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(18, 89);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(56, 19);
            this.metroLabel2.TabIndex = 17;
            this.metroLabel2.Text = "Amount";
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel4.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel4.Location = new System.Drawing.Point(12, 6);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(76, 25);
            this.metroLabel4.TabIndex = 7;
            this.metroLabel4.Text = "Expense";
            this.metroLabel4.UseCustomBackColor = true;
            // 
            // Expense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(568, 523);
            this.ControlBox = false;
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.metroPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Expense";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Section";
            this.Load += new System.EventHandler(this.ClassType_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Brands_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.metroPanel1.ResumeLayout(false);
            this.metroPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Timer timer1;
        private MetroFramework.Controls.MetroTextBox textBoxExpneseName;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroPanel metroPanel1;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroTextBox metroTextBoxAmount;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel metroLabel5;
        private MetroFramework.Controls.MetroDateTime metroDateTime1;
        private MetroFramework.Controls.MetroTile metroTile5;
        private MetroFramework.Controls.MetroTextBox metroTextBoxDescription;
        private MetroFramework.Controls.MetroTextBox metroTextBoxPaidAmount;
        private MetroFramework.Controls.MetroLabel metroLabel6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
    }
}