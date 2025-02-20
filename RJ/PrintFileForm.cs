using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace PrintWordFileInCsharp
{
    public partial class PrintFileForm : Form
    {
        public PrintFileForm()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            string[] s = openFileDialog1.FileName.Split('.');
            if (dr.ToString() == "OK")
            {
                if (s.Length > 1)
                    if (s[1] == "doc" || s[1] == "docx" || s[1] == "jpg")
                        txtFileName.Text = openFileDialog1.FileName;
                    else
                        MessageBox.Show("Please select doc,docx,jpeg file !!");
            }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFileName.Text.Trim()))
            {
                txtFileName.BackColor = Color.Yellow;
                MessageBox.Show("Please Select file.");
                return;
            }
            ProcessStartInfo info = new ProcessStartInfo(txtFileName.Text.Trim());
            info.Verb = "Print";
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(info);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtFileName.ReadOnly = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFileName.Text.Trim()))
            {
                txtFileName.BackColor = Color.Yellow;
                MessageBox.Show("Please Select file.");
                return;
            }
            ProcessStartInfo info = new ProcessStartInfo(txtFileName.Text.Trim());
            info.Verb = "Print";
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(info);
        }
    }
}
