namespace RJ
{
    partial class GMPatternLock
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
            this.lockScreenControl1 = new GestureLockApp.GestureLockControl.LockScreenControl();
            this.SuspendLayout();
            // 
            // lockScreenControl1
            // 
            this.lockScreenControl1.Location = new System.Drawing.Point(0, 55);
            this.lockScreenControl1.Name = "lockScreenControl1";
            this.lockScreenControl1.Size = new System.Drawing.Size(300, 300);
            this.lockScreenControl1.TabIndex = 0;
            this.lockScreenControl1.Text = "lockScreenControl1";
            this.lockScreenControl1.PassCodeSubmitted += new System.EventHandler<GestureLockApp.GestureLockControl.PassCodeSubmittedEventArgs>(this.lockScreenControl1_PassCodeSubmitted);
            // 
            // GMPatternLock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 358);
            this.Controls.Add(this.lockScreenControl1);
            this.KeyPreview = true;
            this.Name = "GMPatternLock";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.DropShadow;
            this.Text = "Draw Your Pattern";
            this.TextAlign = MetroFramework.Forms.MetroFormTextAlign.Center;
            this.Load += new System.EventHandler(this.GMPatternLock_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GMPatternLock_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private GestureLockApp.GestureLockControl.LockScreenControl lockScreenControl1;
    }
}