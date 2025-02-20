using Calculator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Management;
using System.Collections;
using PrintWordFileInCsharp;

namespace RJ
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public void setsize()
        {
            //1050,600
            this.WindowState = FormWindowState.Maximized;
            int height = this.Height - (ribbonControl2.Height) - 18;
            int width = this.Width;
            int panelwdh = this.Width * 20 / 100;
            panelEx2.Size = new Size(panelwdh, height / 2);
            panelEx3.Size = new Size(panelwdh, (height / 2));
            panelEx1.Size = new Size(width - panelEx3.Width, height);
            //panelEx1.Size = new Size(width - 3, height);
            panelEx2.Location = new Point(0, ribbonPanel1.Location.Y + ribbonPanel1.Height);
            panelEx3.Location = new Point(0, panelEx2.Location.Y + panelEx2.Height);
            panelEx1.Location = new Point(panelEx3.Location.X + panelEx3.Width + 1, ribbonPanel1.Location.Y + ribbonPanel1.Height);
            //panelEx1.Location = new Point(3, ribbonPanel1.Location.Y + ribbonPanel1.Height);
            label1.Location = new Point(width / 2 - label3.Width, panelEx1.Location.Y + panelEx1.Height);
            label3.Location = new Point(width - label3.Width - 380, panelEx1.Location.Y + panelEx1.Height);
            label3.Text = "© 2017 GTechy RIGHTS RESERVED";
            linkLabel1.Location = new Point(label3.Location.X + label3.Width + 2, label3.Location.Y);
            linkLabel1.Text = "www.gtechy.com";
            label2.Location = new Point(2, panelEx1.Location.Y + panelEx1.Height);
            label2.Text = "User : " + RJ.Properties.Settings.Default.loginuser;
            ribbonBar3.Location = new Point(0, panelEx3.Location.Y + panelEx3.Height - ribbonBar3.Height);
            ribbonBar3.Width = panelEx3.Width - 1;
            panelEx1.BringToFront();
        }



        ArrayList hardDriveDetails = new ArrayList();
        public void SystemValidation()
        {
            string message = "";
            int ok = 1;
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String sMacAddress = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)// only return MAC Address from first card  
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }
                message += "\n*" + sMacAddress.Trim() + "*\n";
                if (sMacAddress.Trim() != "D85D4CA4A54C")
                    ok = 0;

                ManagementObjectSearcher moSearcher = new
  ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

                foreach (ManagementObject wmi_HD in moSearcher.Get())
                {
                    HardDrive hd = new HardDrive();  // User Defined Class
                    hd.Model = wmi_HD["Model"].ToString();  //Model Number
                    hd.Type = wmi_HD["InterfaceType"].ToString();  //Interface Type
                    hd.SerialNo = wmi_HD["SerialNumber"].ToString();
                    hardDriveDetails.Add(hd);
                    message += "\n*" + hd.Model + "*\n";
                    if (hd.Model.Trim() == "HDS728080PLAT20 ATA Device")
                    { }
                    else
                        ok = 0;
                    message += "\n*" + hd.Type + "*\n";
                    if (hd.Type.Trim() == "IDE")
                    { }
                    else
                        ok = 0;
                    message += "\n*" + hd.SerialNo.Trim() + "*\n";
                    if (hd.SerialNo.Trim() == "20202020202046503244393159535054575a4652")
                    { }
                    else
                        ok = 0;
                    if (ok == 1)
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (ok == 0)
            {
                MessageBox.Show("You Are Trying To Run Copy Of Software.\nReport Sent To GTechy." + message, "Warning!!!!!!!!!!\n",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
        }

        GMDB gm = new GMDB();
        private void Form1_Load(object sender, EventArgs e)
        {
            buttonItem6.Visible = false;
            buttonItem25.Visible = false;
            panelEx3.Controls.Clear();
            Calendar ch = new Calendar();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            panelEx3.Controls.Add(ch);
            buttonItem4.Visible = false;
            try
            {
                try
                {
                    string query = "update items set update_status='' where update_date<'" + (DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString()) + "'";
                    gm.ExecuteNonQuery(query);
                }
                catch { }
                buttonItem21.Visible = false;
                buttonItem13.Visible = false;
                buttonItem24.Visible = false;
                buttonItem28.Visible = false;
                buttonItem29.Visible = false;
                buttonItem36.Visible = false;

                buttonItem6.Enabled = false;
                buttonItem25.Enabled = false;
                buttonItem19.Visible = false;
                this.Text = "  ";// "الحاج راجہ رفیق اینڈ سنز";
                RJ.Properties.Settings.Default.SchoolName = "  ";// "الحاج راجہ رفیق اینڈ سنز";
                RJ.Properties.Settings.Default.address = "  ";// "دوکان نمبر 296 سبزی منڈی اسلام آباد";//" اسلام آباد I-11/4 " + "دوکان نمبر 296 سبزي منڈي سیکٹر";
                RJ.Properties.Settings.Default.contact = "  ";// "پروپریئٹر";
                ribbonTabItem9.Checked = true;
                timer1.Start();
                setsize();

                //SystemValidation();








                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }
                RJ.Properties.Settings.Default.loginid = "";

                //GMPatternLogin gm = new GMPatternLogin();
                //gm.ShowDialog();

                Login l = new Login();
                l.ShowDialog();

                label2.Text = "User : " + RJ.Properties.Settings.Default.loginuser;
                setusertypefeatures();
                if (RJ.Properties.Settings.Default.loginid.Trim() == "")
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //label1.Text = DateTime.Now.ToShortTimeString() + "  " + DateTime.Now.Date.Day.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Year.ToString();
            label1.Text = DateTime.Now.Date.Day.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Year.ToString();
        }

        private void panelEx1_Click(object sender, EventArgs e)
        {

        }


        private void buttonItem6_Click(object sender, EventArgs e)
        {
            panelEx3.Controls.Clear();
            Chat ch = new Chat();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            panelEx3.Controls.Add(ch);
        }

        private void buttonItem7_Click(object sender, EventArgs e)
        {
            panelEx3.Controls.Clear();
            Calendar ch = new Calendar();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            panelEx3.Controls.Add(ch);
        }

        private void buttonItem9_Click(object sender, EventArgs e)
        {

        }

        private void buttonItem25_Click(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            Settings ch = new Settings();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem27_Click(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            Settings ch = new Settings();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            ch.Show();
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem17_Click(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            Settings ch = new Settings();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        private void ribbonControl2_Click(object sender, EventArgs e)
        {

        }

        private void ribbonTabItem3_Click(object sender, EventArgs e)
        {

        }

        private void ribbonTabItem_Student_Click(object sender, EventArgs e)
        {

        }

        private void buttonItem1_Click(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
          //  Student_Reg ch = new Student_Reg();
            //ch.FormBorderStyle = FormBorderStyle.None;
            //ch.TopLevel = false;
            //ch.Show();
            //ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            //ch.Location = new Point(
            //    (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
            //    (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            //panelEx1.Controls.Add(ch);
        }

        private void buttonItem15_Click(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            SmartCardsReportViewer ch = new SmartCardsReportViewer();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem16_Click(object sender, EventArgs e)
        {
        }

        private void buttonItem16_Click_1(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            ClassAndSectionStrength ch = new ClassAndSectionStrength();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem18_Click(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            Settings ch = new Settings();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void ribbonBar7_ItemClick(object sender, EventArgs e)
        {

        }

        private void buttonItem8_Click(object sender, EventArgs e)
        {
            panelEx3.Controls.Clear();
            Cal ch = new Cal();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            panelEx3.Controls.Add(ch);
        }

        private void buttonItem2_Click(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            Employee_Reg ch = new Employee_Reg();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem26_Click(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
           // ViewStudents ch = new ViewStudents();
            //ch.FormBorderStyle = FormBorderStyle.None;
            //ch.TopLevel = false;
            //ch.Show();
            //ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            //ch.Location = new Point(
            //    (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
            //    (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            //panelEx1.Controls.Add(ch);
        }

        private void buttonItem28_Click(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            ClassType ch = new ClassType();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem29_Click(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            Class ch = new Class();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem30_Click(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            Section ch = new Section();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem31_Click(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            ClassSection ch = new ClassSection();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem32_Click(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            Session ch = new Session();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem33_Click(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
           // StudentDocuments ch = new StudentDocuments();
            //ch.FormBorderStyle = FormBorderStyle.None;
            //ch.TopLevel = false;
            //ch.Show();
            //ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            //ch.Location = new Point(
            //    (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
            //    (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            //panelEx1.Controls.Add(ch);
        }

        private void buttonItem34_Click(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            StaffDocuments ch = new StaffDocuments();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem35_Click(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            FeesType ch = new FeesType();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem36_Click(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            FeesManagement ch = new FeesManagement();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem37_Click(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            AddmissionCertificateCrystalReportViewer ch = new AddmissionCertificateCrystalReportViewer();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem9_Click_1(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
           // StudentAttendance ch = new StudentAttendance();
            //ch.FormBorderStyle = FormBorderStyle.None;
            //ch.TopLevel = false;
            //ch.Show();
            //ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            //ch.Location = new Point(
            //    (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
            //    (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            //panelEx1.Controls.Add(ch);
        }

        public void setusertypefeatures()
        {
            try
            {
                buttonItem17.Visible = false;
                if (RJ.Properties.Settings.Default.logintype == "User" || RJ.Properties.Settings.Default.logintype == "user")
                {
                    buttonItem22.Visible = false;
                    //buttonItem12.Visible = false;
                    buttonItem2.Visible = false;
                    //buttonItem12.Visible = false;
                    ribbonTabItem1.Visible = false;
                    ribbonTabItem9.Checked = true;
                }
                else
                {
                    buttonItem22.Visible = true;
                    //buttonItem12.Visible = true;
                    buttonItem2.Visible = true;
                    //buttonItem12.Visible = true;
                    ribbonTabItem1.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void buttonItem38_Click(object sender, EventArgs e)
        {
            RJ.Properties.Settings.Default.loginid = "";

            Login l = new Login();
            l.ShowDialog();


            label2.Text = "User : " + RJ.Properties.Settings.Default.loginuser;
            setusertypefeatures();
            if (RJ.Properties.Settings.Default.loginid == "")
                this.Close();
            panelEx1.Controls.Clear();
            treeView2.Nodes.Clear();
        }

        private void buttonItem39_Click(object sender, EventArgs e)
        {
            //panelEx1.Controls.Clear();
            //StudentRegistrationReportViewer ch = new StudentRegistrationReportViewer();
            //ch.FormBorderStyle = FormBorderStyle.None;
            //ch.TopLevel = false;
            //ch.Show();
            //ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            //ch.Location = new Point(
            //    (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
            //    (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            //panelEx1.Controls.Add(ch);
        }

        private void buttonItem40_Click(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            Course ch = new Course();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem41_Click(object sender, EventArgs e)
        {
            try
            {
                panelEx1.Controls.Clear();
                Session_Course ch = new Session_Course();
                //            ch.FormBorderStyle = FormBorderStyle.None;
                //            ch.TopLevel = false;
                ch.Show();
                ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
                ch.Location = new Point(
                    (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                    (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
                panelEx1.Controls.Add(ch);
            }
            catch { }
        }

        private void buttonItem42_Click(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
           // StudentCourseRegistratoin ch = new StudentCourseRegistratoin();
            //ch.FormBorderStyle = FormBorderStyle.None;
            //ch.TopLevel = false;
            //ch.Show();
            //ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            //ch.Location = new Point(
            //    (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
            //    (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            //panelEx1.Controls.Add(ch);
        }

        private void buttonItem1_Click_1(object sender, EventArgs e)
        {
            try
            {
                treeView2.Show();
                treeView2.Nodes.Clear();
            }
            catch { }
            panelEx1.Controls.Clear();
            //Dashboard ch = new Dashboard();
            //ch.FormBorderStyle = FormBorderStyle.None;
            //ch.TopLevel = false;
            //ch.Show();
            //ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            //ch.Location = new Point(
            //    (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
            //    (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            //panelEx1.Controls.Add(ch);
        }

        private void panelEx1_DockChanged(object sender, EventArgs e)
        {

        }

        private void buttonItem2_Click_1(object sender, EventArgs e)
        {
            //panelEx1.Controls.Clear();
            AddItem ch = new AddItem();
            //ch.FormBorderStyle = FormBorderStyle.None;
            //ch.TopLevel = false;
            ch.Show();
            //ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            //ch.Location = new Point(
            //    (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
            //    (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            //panelEx1.Controls.Add(ch);
        }

        private void buttonItem2_Click_2(object sender, EventArgs e)
        {
            try
            {
                treeView2.Show();
                treeView2.Nodes.Clear();
            }
            catch { }
            panelEx1.Controls.Clear();
            AddItem ch = new AddItem();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem3_Click(object sender, EventArgs e)
        {

        }

        private void buttonItem4_Click(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            Brands ch = new Brands();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem5_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter Password", "Authentication", "", -1, -1);
            if (input == "ZohaibGtechy@123")
            {
                panelEx1.Controls.Clear();
                Units ch = new Units();
                ch.FormBorderStyle = FormBorderStyle.None;
                ch.TopLevel = false;
                ch.Show();
                ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
                ch.Location = new Point(
                    (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                    (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
                panelEx1.Controls.Add(ch);
            }
            else
            {
                if (input.Trim() != "")
                {
                    MessageBox.Show("Incorrect Password");
                }
            }
        }

        private void buttonItem3_Click_1(object sender, EventArgs e)
        {

            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter Password", "Authentication", "", -1, -1);
            if (input == "ZohaibGtechy@123")
            {
                panelEx1.Controls.Clear();
                CreateItemCategory ch = new CreateItemCategory();
                ch.FormBorderStyle = FormBorderStyle.None;
                ch.TopLevel = false;
                ch.Show();
                ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
                ch.Location = new Point(
                    (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                    (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
                panelEx1.Controls.Add(ch);
            }
            else
            {
                if (input.Trim() != "")
                {
                    MessageBox.Show("Incorrect Password");
                }
            }
        }

        private void buttonItem10_Click(object sender, EventArgs e)
        {
        }

        private void buttonItem9_Click_2(object sender, EventArgs e)
        {
        }

        private void buttonItem11_Click(object sender, EventArgs e)
        {
            try
            {
                treeView2.Show();
                treeView2.Nodes.Clear();
            }
            catch { }
            panelEx1.Controls.Clear();
            ItemsList ch = new ItemsList();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem12_Click(object sender, EventArgs e)
        {
            try
            {
                treeView2.Show();
                treeView2.Nodes.Clear();
            }
            catch { }
            panelEx1.Controls.Clear();
            UsersList ch = new UsersList();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem13_Click(object sender, EventArgs e)
        {
            try
            {
                treeView2.Show();
                treeView2.Nodes.Clear();
            }
            catch { }
            panelEx1.Controls.Clear();
            PurchaseItem ch = new PurchaseItem();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem15_Click_1(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter Password", "Authentication", "", -1, -1);
            if (input == "ZohaibGtechy@123")
            {
                panelEx1.Controls.Clear();
                Taxes ch = new Taxes();
                ch.FormBorderStyle = FormBorderStyle.None;
                ch.TopLevel = false;
                ch.Show();
                ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
                ch.Location = new Point(
                    (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                    (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
                panelEx1.Controls.Add(ch);
            }
            else
            {
                if (input.Trim() != "")
                {
                    MessageBox.Show("Incorrect Password");
                }
            }
        }

        private void buttonItem16_Click_2(object sender, EventArgs e)
        {
            try
            {
                treeView2.Show();
                treeView2.Nodes.Clear();
            }
            catch { }
            panelEx1.Controls.Clear();
            SaleItem ch = new SaleItem();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem19_Click(object sender, EventArgs e)
        {
        }

        private void buttonItem20_Click(object sender, EventArgs e)
        {

        }

        private void buttonItem21_Click(object sender, EventArgs e)
        {
        }

        private void buttonItem22_Click(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            RegisterUser ch = new RegisterUser();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem9_Click_3(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            ChangePassword ch = new ChangePassword();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem10_Click_1(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            ViewBills ch = new ViewBills("sale trading");
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
            //treeView2.Show();
            //treeView2.Nodes.Clear();
            //treeView2.Nodes.Add("Purchase Items Report");
            //treeView2.Nodes.Add("Sale Items Report");
            //treeView2.Nodes.Add("Expenses Report");
            //treeView2.Nodes.Add("Payment Report");
            //treeView2.Nodes.Add("Receiving Reports");
            //treeView2.Nodes.Add("Profit/Loss Report");
        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Text == "Purchase Items Report")//gmg
            {
                panelEx1.Controls.Clear();
                ViewBills ch = new ViewBills("purchase trading");
                ch.FormBorderStyle = FormBorderStyle.None;
                ch.TopLevel = false;
                ch.Show();
                ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
                ch.Location = new Point(
                    (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                    (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
                panelEx1.Controls.Add(ch);
            }
            if (e.Node.Text == "Sale Items Report")//gmg
            {
                panelEx1.Controls.Clear();
                ViewBills ch = new ViewBills("sale trading");
                ch.FormBorderStyle = FormBorderStyle.None;
                ch.TopLevel = false;
                ch.Show();
                ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
                ch.Location = new Point(
                    (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                    (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
                panelEx1.Controls.Add(ch);
            }
            if (e.Node.Text == "Expenses Report")//gmg
            {
                panelEx1.Controls.Clear();
                ViewBills ch = new ViewBills("expense");
                ch.FormBorderStyle = FormBorderStyle.None;
                ch.TopLevel = false;
                ch.Show();
                ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
                ch.Location = new Point(
                    (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                    (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
                panelEx1.Controls.Add(ch);
            }
            if (e.Node.Text == "Payment Report")//gmg
            {
                panelEx1.Controls.Clear();
                ViewPaymentsAndReceivings ch = new ViewPaymentsAndReceivings("payment");
                ch.FormBorderStyle = FormBorderStyle.None;
                ch.TopLevel = false;
                ch.Show();
                ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
                ch.Location = new Point(
                    (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                    (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
                panelEx1.Controls.Add(ch);
            }
            if (e.Node.Text == "Receiving Reports")//gmg
            {
                panelEx1.Controls.Clear();
                ViewPaymentsAndReceivings ch = new ViewPaymentsAndReceivings("receiving");
                ch.FormBorderStyle = FormBorderStyle.None;
                ch.TopLevel = false;
                ch.Show();
                ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
                ch.Location = new Point(
                    (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                    (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
                panelEx1.Controls.Add(ch);
            }
            if (e.Node.Text == "Profit/Loss Report")//gmg
            {
                panelEx1.Controls.Clear();
                Profit_Loss ch = new Profit_Loss();
                ch.FormBorderStyle = FormBorderStyle.None;
                ch.TopLevel = false;
                ch.Show();
                ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
                ch.Location = new Point(
                    (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                    (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
                panelEx1.Controls.Add(ch);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }

        private void buttonItem23_Click(object sender, EventArgs e)
        {
            try
            {
                treeView2.Show();
                treeView2.Nodes.Clear();
            }
            catch { }
            panelEx1.Controls.Clear();
            Ledger ch = new Ledger();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem24_Click(object sender, EventArgs e)
        {
            try
            {
                treeView2.Show();
                treeView2.Nodes.Clear();
            }
            catch { }
            panelEx1.Controls.Clear();
            LowStockItemsList ch = new LowStockItemsList();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem26_Click_1(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            ChangeLockSetting ch = new ChangeLockSetting();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem14_Click(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            AboutBox1 ch = new AboutBox1();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("www.gtechy.com");
            }
            catch { }
        }

        private void ribbonControl2_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Modifiers == Keys.Control && e.KeyCode == Keys.W)
                {

                }
                //buttonItem19.Checked=true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonItem18_Click_1(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            Backup ch = new Backup();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem18_Click_2(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            Backup ch = new Backup();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem20_Click_1(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            RestoreBackup ch = new RestoreBackup();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void ribbonBar8_ItemClick(object sender, EventArgs e)
        {

        }

        private void buttonItem21_Click_1(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            Expense ch = new Expense();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem28_Click_1(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            Profit_Loss ch = new Profit_Loss();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem29_Click_1(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            Payment_Receiving ch = new Payment_Receiving("Payment");
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem30_Click_1(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            Payment_Receiving ch = new Payment_Receiving("Receiving");
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem32_Click_1(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            RegisterCustomerOrVendor ch = new RegisterCustomerOrVendor();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem33_Click_1(object sender, EventArgs e)
        {

        }

        private void buttonItem34_Click_1(object sender, EventArgs e)
        {

        }

        private void buttonItem35_Click_1(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            RegisterCustomerOrVendor ch = new RegisterCustomerOrVendor();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem36_Click_1(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            CustomersOrVendorsList ch = new CustomersOrVendorsList(0);
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem37_Click_1(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            CustomersOrVendorsList ch = new CustomersOrVendorsList(1);
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem32_Click_2(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            Customers_Bill_Rate_Update ch = new Customers_Bill_Rate_Update();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem33_Click_2(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            ViewPaymentsAndReceivings ch = new ViewPaymentsAndReceivings("receiving");
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void applicationButton2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Leave(object sender, EventArgs e)
        {

        }

        private void buttonItem34_Click_2(object sender, EventArgs e)
        {
            panelEx1.Controls.Clear();
            PrintFileForm ch = new PrintFileForm();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        private void buttonItem39_Click_1(object sender, EventArgs e)
        {
            try
            {
                treeView2.Show();
                treeView2.Nodes.Clear();
            }
            catch { }
            panelEx1.Controls.Clear();
            UsersList ch = new UsersList();
            ch.FormBorderStyle = FormBorderStyle.None;
            ch.TopLevel = false;
            ch.Show();
            ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
            ch.Location = new Point(
                (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
            panelEx1.Controls.Add(ch);
        }

        int buttonclick = 0;
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            this.Location = new Point(0, 0);
            if (buttonclick == 0)
            {
                if (mm == 0 && this.WindowState == FormWindowState.Maximized)
                {
                    this.WindowState = FormWindowState.Maximized;
                    this.Height = this.Height - 2;
                    mm = 1;
                    buttonclick = 1;
                }
            }
            else
            {
                if (this.WindowState == FormWindowState.Maximized && mm == 1)
                {
                    this.WindowState = FormWindowState.Maximized;
                    mm = 0;
                    buttonclick = 0;
                }
            }
        }

        private void buttonItem12_Click_1(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter Password", "Authentication", "", -1, -1);
            if (input == "ZohaibGtechy@123")
            {
                panelEx1.Controls.Clear();
                Admin_Settings ch = new Admin_Settings();
                ch.FormBorderStyle = FormBorderStyle.None;
                ch.TopLevel = false;
                ch.Show();
                ch.Location = new Point(panelEx1.Location.X - (ch.Width / 2), panelEx1.Location.Y - (ch.Height / 2));
                ch.Location = new Point(
                    (this.panelEx1.Width / 2) - (ch.Size.Width / 2),
                    (this.panelEx1.Height / 2) - (ch.Size.Height / 2));
                panelEx1.Controls.Add(ch);
            }
            else
            {
                if (input.Trim() != "")
                {
                    MessageBox.Show("Incorrect Password");
                }
            }
        }

        int mm = 0;
        private void Form1_MaximizedBoundsChanged(object sender, EventArgs e)
        {
            //if (mm == 0)
            //{
            //this.WindowState = FormWindowState.Maximized;
            //this.WindowState = FormWindowState.Normal;
            //this.Location = new Point(0, 0);
            //this.Size = new Size(this.Width,this.Size.Height - 2);
            //mm = 1;
            //}
            //else if(this.WindowState == FormWindowState.Maximized)
            //{
            //    this.WindowState = FormWindowState.Maximized;
            //    mm = 0;
            //}

        }

        private void Form1_MinimumSizeChanged(object sender, EventArgs e)
        {

        }
    }
    class HardDrive
    {
        private string model = null;
        private string type = null;
        private string serialNo = null;
        public string Model
        {
            get { return model; }
            set { model = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public string SerialNo
        {
            get { return serialNo; }
            set { serialNo = value; }
        }
    }
}
