using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace RJ
{
    public partial class Employee_Reg: Form
    {
        public Employee_Reg()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        DataTable sections = new DataTable();
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
        Image load_img;
        private void Student_Reg_Load(object sender, EventArgs e)
        {
            try
            {
                load_img = pictureBox1.Image;
                setreg();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                //openFileDialog1.Filter = "bmp files (*.jpg)|*.jpeg|*.png";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                    Image i = pictureBox1.Image;
                    pictureBox1.Image = ScaleImage(i, pictureBox1.Width, pictureBox1.Height);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Un-Support Format");
            }
        }

        public void setreg()
        {
            try
            {
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("Select count(*) from employee", con);
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sd.Fill(dt);
                string iq = "";
                foreach (DataRow d in dt.Rows)
                {
                    iq = d[0].ToString();
                }
                if (iq == "")
                {
                    iq = "000000";
                }
                string id = "emp_";
                int l = (int.Parse(iq) + 1);
                //for complete six digit
                if (l.ToString().Length == 1)
                {
                    id += "00000" + l.ToString();
                }
                else if (l.ToString().Length == 2)
                {
                    id += "0000" + l.ToString();
                }
                else if (l.ToString().Length == 3)
                {
                    id += "000" + l.ToString();
                }
                else if (l.ToString().Length == 4)
                {
                    id += "00" + l.ToString();
                }
                else if (l.ToString().Length == 5)
                {
                    id += "0" + l.ToString();
                }
                else
                {
                    id += l.ToString();
                }
                //end for complete six digit

                label6.Text = id.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private byte[] ImageToStream(string fileName)
        {
            MemoryStream stream = new MemoryStream();
        tryagain:
            try
            {
                Bitmap image = new Bitmap(fileName);
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                goto tryagain;
            }

            return stream.ToArray();
        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtfirstname.Text.Trim() != ""  && txt_cnic.Text.Trim() != "" && comboBoxreligion.SelectedIndex >= 0 && (radiobuttonmale.Checked == true || radiobuttonfemale.Checked == true) && txtstate.Text.Trim() != "" && txt_city.Text.Trim() != "" && txtaddress.Text != "" && txtfathername.Text != "" && txtmothername.Text != "" && txtmiddlename.Text != "" && txtcontact.Text != "" && comboBoxDesignation.SelectedIndex >= 0 && txtSalary.Text != "" && txtCl.Text != "" && txtPl.Text != "" && txtAl.Text != "" && txtMl.Text != "" && txtOtherL.Text != "")
            {
                try
                {
                    if (con.State.ToString() == "Closed")
                    {
                        con.Open();
                    }
                    string gender = "";
                    if (radiobuttonmale.Checked == true)
                    {
                        gender = radiobuttonmale.Text;
                    }
                    else if (radiobuttonfemale.Checked == true)
                    {
                        gender = radiobuttonfemale.Text;
                    }


                    MemoryStream stream = new MemoryStream();
                    Bitmap image = (Bitmap)pictureBox1.Image;
                    image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] content = stream.ToArray();
                    SqlCommand cmd = new SqlCommand(@"insert into employee values('" + label6.Text + "','" + txtfirstname.Text.Trim() + "','" + txtmiddlename.Text.Trim() + "','" + txtlastname.Text.Trim() + "','" + txt_cnic.Text.Trim() + "',@img,'" + txtfathername.Text.Trim() + "','" + txtmothername.Text.Trim() + "','" + comboBoxreligion.SelectedItem.ToString() + "','" + datetimepickerdob.Value.ToShortDateString() + "','" + gender + "','" + txtstate.Text.Trim() + "','" + txt_city.Text.Trim() + "','" + txtaddress.Text.Trim() + "','" + txtcontact.Text.Trim() + "','" + txtemail.Text.Trim() + "','" + comboBoxDesignation.SelectedItem.ToString() + "','" + dateTimePicker2.Value.ToShortDateString() + "','" + txtSalary.Text.Trim() + "','" + txtCl.Text.Trim() + "','" + txtAl.Text.Trim() + "','" + txtMl.Text.Trim() + "','" + txtPl.Text.Trim() + "','" + txtOtherL.Text.Trim() + "','1')", con);
                    cmd.Parameters.AddWithValue("@img", content);
                    int a = cmd.ExecuteNonQuery();
                    if (a > 0)
                    {
                        //foreach (DataGridViewRow d in dataGridView1.Rows)
                        //{
                        //    if (d.Cells[0].Value.ToString() == "True")
                        //    {
                        //        cmd = new SqlCommand("Select max(cast(id as int)) from studentfeesmgm", con);
                        //        SqlDataAdapter sd = new SqlDataAdapter(cmd);
                        //        DataTable dt = new DataTable();
                        //        sd.Fill(dt);
                        //        string id = "";
                        //        foreach (DataRow g in dt.Rows)
                        //        {
                        //            id = g[0].ToString();
                        //        }
                        //        if (id == "")
                        //        {
                        //            id = "0";
                        //        }
                        //        id = (int.Parse(id) + 1).ToString();
                        //        cmd = new SqlCommand("insert into studentfeesmgm values('" + id + "','" + label6.Text + "','" + d.Cells[3].Value.ToString() + "','" + d.Cells[1].Value.ToString() + "','" + d.Cells[2].Value.ToString() + "','1')", con);
                        //        cmd.ExecuteNonQuery();
                        //    }
                        //}
                        MessageBox.Show("Registration Successfully Complete");
                        txtfirstname.Text = txtmiddlename.Text = txtlastname.Text = txtemail.Text = txt_cnic.Text = txtstate.Text = txt_city.Text = txtaddress.Text = txtfathername.Text = txtmothername.Text = txtmiddlename.Text = txtcontact.Text = txtSalary.Text = txtCl.Text = txtPl.Text = txtAl.Text = txtMl.Text = txtOtherL.Text = ""; pictureBox1.Image = null;
                        comboBoxreligion.SelectedIndex=comboBoxDesignation.SelectedIndex =-1;
                        radiobuttonmale.Checked = radiobuttonfemale.Checked = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Fill All Fields");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void rectangleShape1_Click(object sender, EventArgs e)
        {

        }

        private void rectangleShape1_Click_1(object sender, EventArgs e)
        {

        }
    }
}

