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
using System.Globalization;

namespace RJ
{
    public partial class Login : MetroFramework.Forms.MetroForm
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (metroTextBox1.Text.Trim().ToString() == "abc" && metroTextBox2.Text.Trim().ToString() == "123")
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
                if (metroTextBox1.Text.Trim().Contains(" ") || metroTextBox2.Text.Trim().Contains(" "))
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
                    string query = "select * from login where user_id=N'" + metroTextBox1.Text.Trim() + "' and password=N'" + metroTextBox2.Text.Trim() + "' and status='1'";
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
            showpassword();
            hidepattern();
            pictureBox1.Image = (Image)Resize(pictureBox1.Image, pictureBox1.Width, pictureBox1.Height, true);
            pictureBox2.Image = (Image)Resize(pictureBox2.Image, pictureBox2.Width, pictureBox2.Height, true);
            pictureBox3.Image = (Image)Resize(pictureBox3.Image, pictureBox3.Width, pictureBox3.Height, true);
            pictureBox4.Image = (Image)Resize(pictureBox4.Image, pictureBox4.Width, pictureBox4.Height, true);
        }

        public void GMSetPasscode()
        {
            try
            {
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }
                string query = "select * from login where user_id=N'" + metroTextBox1.Text.Trim() + "' and status='1'";
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

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Application.Exit();
            if (e.KeyCode == Keys.Enter)
                pictureBox4_Click(this.pictureBox4, null);
        }

        CultureInfo ci = CultureInfo.InvariantCulture;
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            try
            {
                if (metroTextBox1.Text.Trim().Contains(" ") || metroTextBox2.Text.Trim().Contains(" "))
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
                    string query = "select * from login where user_id=N'" + metroTextBox1.Text.Trim() + "' and password=N'" + metroTextBox2.Text.Trim() + "'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    foreach (DataRow d in dt.Rows)
                    {
                        if (d["status"].ToString() == "1")
                        {
                            string loginok = "";
                            try
                            {
                                try//remove loginlogs entries if entries >500 record
                                {
                                    query = "select count(*) from loginlogs";
                                    if (double.Parse(gm.GetTable(query).Rows[0][0].ToString()) > 500)
                                    {
                                        query = "Select max(cast(id as int)) from loginlogs";
                                        string maxid = gm.GetTable(query).Rows[0][0].ToString();
                                        query = "delete from loginlogs where id!='" + maxid + "'";
                                        gm.ExecuteNonQuery(query);
                                        query = "update loginlogs set id='1' where id='" + maxid + "'";
                                        gm.ExecuteNonQuery(query);
                                    }
                                }
                                catch
                                { }
                                try//check login date and time
                                {
                                    //string todaydate = DateTime.Now.Year.ToString()+"-"+DateTime.Now.Month.ToString()+"-"+DateTime.Now.Day.ToString();
                                    query = "select * from loginlogs where login_userid='" + d["user_id"].ToString() + "'";
                                    if (gm.GetTable(query).Rows.Count <= 0)
                                    {
                                        loginok = "1";
                                        query = "Select max(cast(id as int)) from loginlogs";
                                        string id = gm.MaxId(query);
                                        query = "insert into loginlogs values('" + id + "','" + d["user_id"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','1')";
                                        gm.ExecuteNonQuery(query);
                                    }
                                    else
                                    {
                                        //query = "select * from loginlogs where login_date>='" + todaydate + "' and login_time>'" + DateTime.Now.ToString("HH:mm:ss") + "' and login_userid='" + d["user_id"].ToString() + "'";
                                        query = "select * from loginlogs where login_date>'" + DateTime.Now.ToString("yyyy-MM-dd") + "' and login_userid='" + d["user_id"].ToString() + "'";
                                        if (gm.GetTable(query).Rows.Count <= 0)
                                        {
                                            query = "select * from loginlogs where login_date>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' and login_time>'" + DateTime.Now.ToString("HH:mm:ss") + "' and login_userid='" + d["user_id"].ToString() + "'";
                                            if (gm.GetTable(query).Rows.Count <= 0)
                                            {
                                                loginok = "1";
                                                query = "Select max(cast(id as int)) from loginlogs";
                                                string id = gm.MaxId(query);
                                                query = "insert into loginlogs values('" + id + "','" + d["user_id"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("HH:mm:ss") + "','1')";
                                                gm.ExecuteNonQuery(query);
                                            }
                                        }
                                    }
                                }
                                catch { }

                                //try
                                //{
                                //    DataTable dt3 = gm.GetTable("select * from customer_or_vendor where status ='1' and (parent='' or parent='NULL')");
                                //    DataTable dt2;
                                //    foreach (DataRow dd in dt3.Rows)
                                //    {
                                //        string todaydate = DateTime.Now.Year.ToString() + "-" + (DateTime.Now.Month.ToString().Length == 1 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString()) + "-" + (DateTime.Now.Day.ToString().Length == 1 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString());
                                //        query = "Select * from bill where bill_type='" + "Receiving" + "' and status != '-1' and bill_date='" + todaydate + "' and Customer_Vendor_Id='"+dd[0].ToString()+"' order by Bill_Date DESC";
                                //        dt2 = gm.GetTable(query);
                                //        if (dt2.Rows.Count <= 0)
                                //        {
                                //            int count = 0;
                                //            string billdate = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
                                //            query = "select count(*) from bill where status!='-1' and bill_date='" + billdate + "'";
                                //            DataTable dt1 = gm.GetTable(query);
                                //            try
                                //            {
                                //                string c = dt1.Rows[0][0].ToString();
                                //                count = int.Parse(c.Trim());
                                //            }
                                //            catch { }
                                //            count++;
                                //            string countstring = count.ToString();
                                //            if (count.ToString().Trim().Length == 1)
                                //            {
                                //                countstring = "00" + count.ToString();
                                //            }
                                //            else if (count.ToString().Trim().Length == 2)
                                //            {
                                //                countstring = "0" + count.ToString();
                                //            }
                                //            string billid = "";
                                //            {
                                //                billid = "REC" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + countstring.ToString();
                                //            }

                                //            string paymentDue = "0";
                                //            query = "Select sum(balance) from bill where status!='-1' and customer_vendor_id in ( select id from customer_or_vendor where parent='" + dd[0].ToString() + "') or customer_vendor_id='" + dd[0].ToString() + "'";
                                //            DataTable dttt2 = gm.GetTable(query);
                                //            if (dttt2.Rows.Count > 0)
                                //            {
                                //                try
                                //                {
                                //                    double.Parse(dt2.Rows[0][0].ToString().Trim());
                                //                    paymentDue = dt2.Rows[0][0].ToString().Trim();
                                //                }
                                //                catch
                                //                {
                                //                    paymentDue = "0";
                                //                }
                                //            }
                                //            else
                                //            {
                                //                paymentDue = "0";
                                //            }

                                //            double percentage = 0;
                                //            int percentageamount = 0;
                                //            if (RJ.Properties.Settings.Default.ReceivingInPoints == "0")
                                //            {
                                //                try
                                //                {
                                //                    query = "select percentage from customer_or_vendor where id='" + dd[0].ToString() + "'";
                                //                    DataTable td = gm.GetTable(query);
                                //                    if (td.Rows.Count > 0)
                                //                    {
                                //                        percentage = double.Parse(td.Rows[0][0].ToString().Trim() == "" ? "0" : td.Rows[0][0].ToString().Trim());
                                //                    }
                                //                }
                                //                catch { }
                                //                percentageamount = int.Parse(Math.Round(((0 * percentage) / 100), 0, MidpointRounding.AwayFromZero).ToString());
                                //            }
                                //            query = "Select max(cast(id as int)) from bill";
                                //            string id = gm.MaxId(query);
                                //            query = @"insert into bill values('" + id + "','" + "Receiving" + "','" + billid + "','" + DateTime.Now.Date + "','" + DateTime.Now.ToShortTimeString() + "','" + ((paymentDue == "") ? "0" : paymentDue) + "','','0','0','','0','" + percentageamount + "','','0','" + ((paymentDue.ToString() == "") ? "0" : paymentDue.ToString()) + "','" + (("0".ToString() == "") ? "0" : "0".Trim()) + "','" + ("Receiving" == "Payment" ? 0 : -(0 + percentageamount)) + "','','" + dd[0].ToString() + "','" + "Cash" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + RJ.Properties.Settings.Default.loginid + "','" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.ToShortTimeString() + "','1')";
                                //            gm.ExecuteNonQuery(query);
                                //        }
                                //    }
                                //}
                                //catch { }

                                if (loginok == "1")
                                {

                                    pay0Rec();

                                    try//check licence
                                    {
                                        string licence = "";
                                        //try
                                        //{
                                        //    query = "select * from mml where status='1'";
                                        //    DataTable dtt = gm.GetTable(query);
                                        //    if (dtt.Rows.Count <= 0)
                                        //    {
                                        //        MessageBox.Show("Licence Not Found.\n Enter your software licence and login again", "Licence Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        //        return;
                                        //    }
                                        //    else//check licence if ok or expire if ok then licence = "1";
                                        //    {
                                        //        bool ok = false;
                                        //        try
                                        //        {
                                        //            string dfrom = "";
                                        //            string dto = "";
                                        //            CultureInfo ci = CultureInfo.InvariantCulture;
                                        //            if ((DateTime.Now.Date >= DateTime.ParseExact("01/01/2020", "MM/dd/yyyy", ci).Date) && (DateTime.Now.Date <= DateTime.ParseExact("01/01/2021", "MM/dd/yyyy", ci).Date) && (dtt.Rows[0]["lk"].ToString() == "mm051012gpraa0192018"))
                                        //            {
                                        //                licence = "1";
                                        //            }
                                        //            else if ((DateTime.Now.Date >= DateTime.ParseExact("01/01/2021", "MM/dd/yyyy", ci).Date) && (DateTime.Now.Date <= DateTime.ParseExact("01/01/3000", "MM/dd/yyyy", ci).Date) && (dtt.Rows[0]["lk"].ToString() == "mm051gp01292019raa02"))
                                        //            {
                                        //                licence = "1";
                                        //            }
                                        //            else
                                        //            {
                                        //                licence = "0";
                                        //            }
                                        //        }
                                        //        catch (Exception ex)
                                        //        {
                                        //            //MessageBox.Show(ex.Message);
                                        //        }
                                        //    }
                                        //}
                                        //catch
                                        //{
                                        //    MessageBox.Show("Licence Not Found.\n Enter your software licence and login again", "Licence Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        //    return;
                                        //}
                                        licence = "1";
                                        //if (licence == "1" || (DateTime.Now.Date <= DateTime.ParseExact("02/25/2020", "MM/dd/yyyy", ci).Date))
                                        if (licence == "1")
                                        {
                                            RJ.Properties.Settings.Default.loginid = d["id"].ToString();
                                            RJ.Properties.Settings.Default.loginuserid = d["user_id"].ToString();
                                            RJ.Properties.Settings.Default.loginuser = d["username"].ToString();
                                            RJ.Properties.Settings.Default.logintype = d["usertype"].ToString();
                                        }
                                        else
                                        {
                                            MessageBox.Show("Your software licence is expired.\n Contact your service provider", "Licence Expired", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                    catch
                                    {
                                        MessageBox.Show("Your software licence is expired.\n Contact your service provider", "Licence Expired", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Your computer time is not correct.\n Set your computer time and login again", "Computer Time Incorrect", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            catch
                            {
                                //MessageBox.Show("Your software licence is expired.\n Contact your service provider", "Licence Expired", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (d["status"].ToString() == "0")
                        {
                            MessageBox.Show("User Deactivated");
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Unknown");
                            return;
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        this.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("Invalid User or Password");
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

        public void pay0Rec()
        {
            try
            {
                //string billdate = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
                string date1 = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
                string q2 = @"select id,customer_Or_Vendor_Name from Customer_Or_Vendor where id in
                                                    (
                                                    select distinct Customer_Or_Vendor.parent from customer_or_vendor
                                                    inner join
                                                    bill on customer_Or_Vendor.id = Bill.Customer_Vendor_Id
                                                     where customer_or_vendor.status ='1' and customer_or_vendor.customer_vendor_type='Customer' and
                                                     bill.status ='1' and bill.Bill_Type='Sale Trading' and bill.Bill_Date='" + date1 + "')";
                SqlCommand cmd = new SqlCommand(q2, con);
                SqlDataAdapter ag = new SqlDataAdapter(cmd);
                DataSet datag = new DataSet();
                ag.Fill(datag);
                DataTable d2 = datag.Tables[0];
                string PaymentOrReceiving = "Receiving";
                if (d2.Rows.Count > 0)
                {
                    for (int i = 0; i < d2.Rows.Count; i++)
                    {
                        string labelPaymentDue = "0";
                        q2 = "Select sum(balance) from bill where status!='-1' and Bill_Date<='" + date1 + "' and customer_vendor_id in ( select id from customer_or_vendor where parent='" + d2.Rows[i][0].ToString() + "') or customer_vendor_id='" + d2.Rows[i][0].ToString() + "'";
                        cmd = new SqlCommand(q2, con);
                        ag = new SqlDataAdapter(cmd);
                        datag = new DataSet();
                        ag.Fill(datag);
                        DataTable dt2 = datag.Tables[0];
                        if (dt2.Rows.Count > 0)
                        {
                            try
                            {
                                double.Parse(dt2.Rows[0][0].ToString().Trim());
                                labelPaymentDue = dt2.Rows[0][0].ToString().Trim();
                            }
                            catch
                            {
                                labelPaymentDue = "0";
                            }
                        }
                        else
                        {
                            labelPaymentDue = "0";
                        }




                        int count = 0;
                        q2 = "select count(*) from bill where status!='-1' and bill_date='" + date1 + "'";
                        cmd = new SqlCommand(q2, con);
                        ag = new SqlDataAdapter(cmd);
                        datag = new DataSet();
                        ag.Fill(datag);
                        DataTable dt1 = datag.Tables[0];
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

                        try//check if today receiving already exist of that main customer than dont insert 0 receiving
                        {
                            q2 = "select * from bill where status!='-1' and bill_date='" + date1 + "' and bill_type='receiving' and customer_vendor_id='" + d2.Rows[i][0].ToString() + "'";
                            cmd = new SqlCommand(q2, con);
                            ag = new SqlDataAdapter(cmd);
                            datag = new DataSet();
                            ag.Fill(datag);
                            dt1 = datag.Tables[0];
                            try
                            {
                                if ((dt1.Rows.Count > 0) && (double.Parse("0") == 0))
                                {
                                    return;
                                }
                            }
                            catch { }
                        }
                        catch { }



                        string billid = "";
                        if (PaymentOrReceiving == "Payment")
                        {
                            billid = "PAY" + DateTime.Now.Date.Day.ToString() + DateTime.Now.Date.Month.ToString() + DateTime.Now.Date.Year.ToString() + countstring.ToString();
                        }
                        else if (PaymentOrReceiving == "Receiving")
                        {
                            billid = "REC" + DateTime.Now.Date.Day.ToString() + DateTime.Now.Date.Month.ToString() + DateTime.Now.Date.Year.ToString() + countstring.ToString();
                        }


                        q2 = "Select max(cast(id as int)) from bill";
                        string id = gm.MaxId(q2);
                        q2 = @"insert into bill values('" + id + "','" + PaymentOrReceiving + "','" + billid + "','" + DateTime.Now.Date + "','" + DateTime.Now.ToShortTimeString() + "','" + ((labelPaymentDue.Trim() == "") ? "0" : labelPaymentDue.Trim()) + "','','0','0','','0','" + "0" + "','','0','" + ((labelPaymentDue.Trim().ToString() == "") ? "0" : labelPaymentDue.Trim().ToString()) + "','" + ("0") + "','" + (PaymentOrReceiving == "Payment" ? 0 : -(0)) + "','','" + d2.Rows[i][0].ToString() + "','" + "payment method" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + RJ.Properties.Settings.Default.loginid + "','" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.ToShortTimeString() + "','1')";
                        int ra = gm.ExecuteNonQuery(q2);
                    }
                }
            }
            catch { }
        }

        GMDB gm = new GMDB();
        public void showpassword()
        {
            metroLabel2.Show();
            metroTextBox2.Show();
            pictureBox3.Show();
            pictureBox4.Show();
        }
        public void hidepassword()
        {
            metroLabel2.Hide();
            metroTextBox2.Hide();
            pictureBox3.Hide();
            pictureBox4.Hide();
        }
        public void showpattern()
        {
            metroLabel3.Show();
            lockScreenControl1.Show();
        }
        public void hidepattern()
        {
            metroLabel3.Hide();
            lockScreenControl1.Hide();
        }

        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string query = "select * from login where user_id=N'" + metroTextBox1.Text.Trim() + "'";
                DataTable dt = gm.GetTable(query);
                string password_type = "";
                try
                {
                    password_type = dt.Rows[0]["active_password_type"].ToString();
                }
                catch { }
                if (password_type == "Password")
                {
                    showpassword();
                    hidepattern();
                }
                else if (password_type == "Pattern")
                {
                    hidepassword();
                    showpattern();
                }
                else
                {
                    showpassword();
                    hidepattern();
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
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
                    string query = "select * from login where user_id=N'" + metroTextBox1.Text.Trim() + "' and status='1'";
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

        private void lockScreenControl1_MouseClick(object sender, MouseEventArgs e)
        {
            GMSetPasscode();
        }

        private void metroTextBox2_Leave(object sender, EventArgs e)
        {
            pictureBox3.Focus();
        }
    }
}
