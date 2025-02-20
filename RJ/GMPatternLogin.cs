using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;

namespace RJ
{
    public partial class GMPatternLogin : MetroFramework.Forms.MetroForm
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public GMPatternLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (metroTextBox1.Text.Trim().ToString() == "abc")
            {
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid User Name Or Password");
            }
        }

        public string ok = "0";
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (metroTextBox1.Text.Trim().Contains(" "))
                {
                    MessageBox.Show("Don't use space");
                    return;
                }
                else
                {
                    if (con.State.ToString() == "Closed")
                    {
                        con.Open();
                    }
                    string query = "select * from login where user_id='" + metroTextBox1.Text.Trim() + "' and status='1'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    foreach (DataRow d in dt.Rows)
                    {

                        string[] pattern = d["pattern"].ToString().Split(',');
                        int[] patternkey = new int[pattern.Length];
                        int i = 0;
                        foreach (String s in pattern)
                        {
                            patternkey[i] = int.Parse(s.Trim());
                            i++;
                        }
                        lockScreenControl1.SetPassCode(patternkey);
                        RJ.Properties.Settings.Default.loginid = d["id"].ToString();
                        RJ.Properties.Settings.Default.loginuserid = d["user_id"].ToString();
                        RJ.Properties.Settings.Default.loginuser = d["username"].ToString();
                        RJ.Properties.Settings.Default.logintype = d["usertype"].ToString();
                    }
                    if (dt.Rows.Count > 0)
                    {
                        this.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("Invalid User");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static Image Resize(Image image, int newWidth, int maxHeight, bool onlyResizeIfWider)
        {
            if (onlyResizeIfWider && image.Width <= newWidth) newWidth = image.Width;

            var newHeight = image.Height * newWidth / image.Width;
            if (newHeight > maxHeight)
            {
                // Resize with height instead  
                newWidth = image.Width * maxHeight / image.Height;
                newHeight = maxHeight;
            }

            var res = new Bitmap(newWidth, newHeight);

            using (var graphic = Graphics.FromImage(res))
            {
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return res;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = (Image)Resize(pictureBox1.Image, pictureBox1.Width, pictureBox1.Height, true);
            pictureBox2.Image = (Image)Resize(pictureBox2.Image, pictureBox2.Width, pictureBox2.Height, true);
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Dispose(true);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            try
            {
                if (metroTextBox1.Text.Trim().Contains(" "))
                {
                    MessageBox.Show("Don't use space");
                    return;
                }
                else
                {
                    if (con.State.ToString() == "Closed")
                    {
                        con.Open();
                    }
                    string query = "select * from login where user_id='" + metroTextBox1.Text.Trim() + "' and status='1'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    foreach (DataRow d in dt.Rows)
                    {
                        RJ.Properties.Settings.Default.loginid = d["id"].ToString();
                        RJ.Properties.Settings.Default.loginuserid = d["user_id"].ToString();
                        RJ.Properties.Settings.Default.loginuser = d["username"].ToString();
                        RJ.Properties.Settings.Default.logintype = d["usertype"].ToString();
                    }
                    if (dt.Rows.Count > 0)
                    {
                        this.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("Invalid User");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GMSetPasscode()
        {
            try
            {
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }
                string query = "select * from login where user_id='" + metroTextBox1.Text.Trim() + "' and status='1'";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                int ok = 0;
                foreach (DataRow d in dt.Rows)
                {
                    string[] pattern = d["pattern"].ToString().Split(',');
                    if (pattern.Length > 1)
                    {
                        int[] patternkey = new int[pattern.Length];
                        int i = 0;
                        foreach (String s in pattern)
                        {
                            patternkey[i] = int.Parse(s.Trim());
                            i++;
                        }
                        lockScreenControl1.SetPassCode(patternkey);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void metroTile1_Click(object sender, EventArgs e)
        {
            LicenseKey lk = new LicenseKey();
            lk.ShowDialog();
        }

        private void lockScreenControl1_PassCodeSubmitted(object sender, GestureLockApp.GestureLockControl.PassCodeSubmittedEventArgs e)
        {
            try
            {
                if (metroTextBox1.Text.Trim().Contains(" "))
                {
                    MessageBox.Show("Don't use space");
                    return;
                }
                if (metroTextBox1.Text.Trim() == "")
                {
                    MessageBox.Show("Enter User_Id");
                    return;
                }
                else
                {
                    if (con.State.ToString() == "Closed")
                    {
                        con.Open();
                    }
                    string query = "select * from login where user_id='" + metroTextBox1.Text.Trim() + "' and status='1'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    int ok = 0;
                    foreach (DataRow d in dt.Rows)
                    {
                        string[] pattern = d["pattern"].ToString().Split(',');
                        int[] patternkey = new int[pattern.Length];
                        if (pattern.Length > 1)
                        {
                            if (e.Valid)
                            {
                                RJ.Properties.Settings.Default.loginid = d["id"].ToString();
                                RJ.Properties.Settings.Default.loginuserid = d["user_id"].ToString();
                                RJ.Properties.Settings.Default.loginuser = d["username"].ToString();
                                RJ.Properties.Settings.Default.logintype = d["usertype"].ToString();
                                ok = 1;
                            }
                            else
                            {
                                MessageBox.Show("Wrong Pattern");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Pattern Not Set");
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        if (ok == 1)
                            this.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("Invalid User");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void metroLabel1_Click(object sender, EventArgs e)
        {

        }

        private void lockScreenControl1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void lockScreenControl1_MouseClick_1(object sender, MouseEventArgs e)
        {
            GMSetPasscode();
        }
    }
}
