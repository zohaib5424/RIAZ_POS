using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RJ
{
    public partial class Admin_Settings : Form
    {
        public Admin_Settings()
        {
            InitializeComponent();
        }

        private void switchButton1_ValueChanged(object sender, EventArgs e)
        {
            if (switchButton1.Value == true)
            {
                RJ.Properties.Settings.Default.datestatus = "1";
            }
            else
            {
                RJ.Properties.Settings.Default.datestatus = "0";
            }
        }

        private void Admin_Settings_Load(object sender, EventArgs e)
        {
            if (RJ.Properties.Settings.Default.datestatus == "1")
            {
                switchButton1.Value = true;
            }
        }

        private void metroLabel5_Click(object sender, EventArgs e)
        {

        }

        private void switchButton2_ValueChanged(object sender, EventArgs e)
        {
            if (switchButton2.Value == true)
            {
                RJ.Properties.Settings.Default.EnableDeleteButton = "1";
            }
            else
            {
                RJ.Properties.Settings.Default.EnableDeleteButton = "0";
            }
        }

        private void switchButton3_ValueChanged(object sender, EventArgs e)
        {
            if (switchButton3.Value == true)
            {
                RJ.Properties.Settings.Default.ReceivingInPoints = "1";
            }
            else
            {
                RJ.Properties.Settings.Default.ReceivingInPoints = "0";
            }
        }
    }
}
