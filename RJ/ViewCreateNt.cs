using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using GETPAK_AMS_SQL_CLASS;
using GETPAK_AMS.Properties;
using GETPAK_AMS;
using System.IO;

namespace gmCalenderNote
{
    public partial class ViewCreateNt : Form
    {
        public ViewCreateNt()
        {
            InitializeComponent();
        }

        public void Hidesharefields()
        {
            label2.Hide();
            textBox1.Hide();
            listBox1.Hide();
            label3.Hide();
        }


        SqlConnection con = new SqlConnection(GETPAK_AMS.Properties.Settings.Default.Connectionstring);
        public void Getusers()
        {
            if (con.State.ToString() == "Closed")
            {
                con.Open();
            }
            listBox1.Items.Clear();
            SqlCommand cmd = new SqlCommand("select * from Guard_Reg",con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow g in dt.Rows)
            {
                if (LoginUser.userName != g[2].ToString())
                {
                    listBox1.Items.Add(g[2].ToString());
                }
            }
        }
        
        public void Showsharefields()
        {
            label2.Show();
            textBox1.Show();
            listBox1.Show();
            label3.Show();
        }
        public DateTime dt;
        public string uid = "";
        private void ViewCreateNt_Load(object sender, EventArgs e)
        {
            button4.Hide();
            richTextBox1.ReadOnly = false;
            textBox1.ReadOnly = true;
            textBox1.Text = "";
            richTextBox1.Text = "";
            button3.Hide();
            button1.Show();
            button2.Show();
            Hidesharefields();
            Getusers();
            if (con.State.ToString() == "Closed")
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("select * from note where notedate='" + dt.ToShortDateString() + "' and userid='" + LoginUser.userLogin + "'", con);
            SqlDataAdapter d1 = new SqlDataAdapter(cmd);
            DataTable t=new DataTable();
            d1.Fill(t);
            foreach (DataRow w in t.Rows)
            {
                richTextBox1.Text = w[3].ToString();
                cmd = new SqlCommand("select * from notedetail where id='" + w[0].ToString() + "'", con);
                SqlDataReader d = cmd.ExecuteReader();
                while (d.Read())
                {
                    if (!textBox1.Text.Contains(d[2].ToString()))
                    {
                        textBox1.Text += d[2].ToString() + " , ";
                    }
                }
                d.Close();
                Showsharefields();
                richTextBox1.ReadOnly = true;
                textBox1.ReadOnly = true;
                listBox1.Hide();
                button1.Hide();


            }
            }

        private void button2_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text != "")
            {
                Getusers();
                button3.Show();
                button1.Hide();
                button2.Hide();
                textBox1.Text = "";
                Showsharefields();
            }
            else
            {
                MessageBox.Show("Enter Note First");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!textBox1.Text.Contains(listBox1.SelectedItem.ToString()))
            {
                if (textBox1.Text.Trim().Length == 0)
                {
                    textBox1.Text += listBox1.SelectedItem.ToString();
                }
                else if (textBox1.Text.Trim().Length > 0)
                {
                    textBox1.Text += "," + listBox1.SelectedItem.ToString();
                }
            }
            else
            {
                MessageBox.Show("Already Entered");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text != "")
            {
                try
                {
                    if (con.State.ToString() == "Closed")
                    {
                        con.Open();
                    }
                    listBox1.Items.Clear();
                    string id = "";
                    SqlCommand cmd = new SqlCommand("select Cast(max(id) as int) from note", con);
                    SqlDataReader sd = cmd.ExecuteReader();
                    while (sd.Read())
                    {
                        id = sd[0].ToString();
                    }
                    sd.Close();
                    if (id == "")
                    {
                        id = "0";
                    }
                    id = (int.Parse(id) + 1).ToString();

                    string userid = LoginUser.userLogin;
                    string tou = textBox1.Text;
                    string msg = LoginUser.userLogin + "   (" + DateTime.Now.ToString() + ")" + "\n" + richTextBox1.Text.Trim();

                    //FileStream fs;
                    //fs = new FileStream(n, FileMode.Open, FileAccess.Read);
                    //byte[] picbyte = new byte[fs.Length];
                    //fs.Read(picbyte, 0, System.Convert.ToInt32(fs.Length));
                    //fs.Close();
                    //string query;

                    //query = "insert into note2 values('" + id + "','" + userid + "','" + dt.ToShortDateString() + "','" + msg + "'," + " @pic)";
                    //SqlParameter picparameter = new SqlParameter();

                    //picparameter.SqlDbType = SqlDbType.VarBinary;

                    //picparameter.ParameterName = "pic";

                    //picparameter.Value = picbyte;

                    //cmd = new SqlCommand(query, con);

                    //cmd.Parameters.Add(picparameter);

                    //cmd.ExecuteNonQuery();
                    
                    cmd = new SqlCommand("insert into note values('" + id + "','" + userid + "','" + dt.ToShortDateString() + "','" + msg + "')", con);
                    int ra = cmd.ExecuteNonQuery();
                    if (ra > 0)
                    {
                        MessageBox.Show("Note Saved");
                        richTextBox1.ReadOnly = true;
                        textBox1.ReadOnly = true;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Enter Note");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                string[] u = textBox1.Text.Split(',');
                for (int i = 0; i < u.Count(); i++)
                {
                    if (con.State.ToString() == "Closed")
                    {
                        con.Open();
                    }
                    string id = "";
                    SqlCommand cmd = new SqlCommand("select Cast(max(id) as int) from note", con);
                    SqlDataReader sd = cmd.ExecuteReader();
                    while (sd.Read())
                    {
                        id = sd[0].ToString();
                    }
                    sd.Close();

                    string userid = LoginUser.userLogin;
                    string tou = u[i].Trim().ToString();
                    cmd = new SqlCommand("insert into notedetail values('" + id + "','" + userid + "','" + tou + "','0')", con);
                    int ra = cmd.ExecuteNonQuery();
                    if (ra > 0)
                    {
                        MessageBox.Show("Share Successfully");
                        richTextBox1.ReadOnly = true;
                        textBox1.Text = "";
                        textBox1.ReadOnly = true;

                    }
                }
            }
            else
            {
                MessageBox.Show("Enter User To Share");
            }
        }

        string n = "";
        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                n = openFileDialog1.FileName.ToString();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(GETPAK_AMS.Properties.Settings.Default.Connectionstring);
            if (con.State.ToString() == "Closed")
            {
                con.Open();
            }
            using (var sqlQuery = new SqlCommand(@"SELECT attachment FROM [dbo].[note2] WHERE [id] = @varID", con))
            {
                sqlQuery.Parameters.AddWithValue("@varID", "5");
                using (var sqlQueryResult = sqlQuery.ExecuteReader())
                    if (sqlQueryResult != null)
                    {
                        sqlQueryResult.Read();
                        var blob = new Byte[(sqlQueryResult.GetBytes(0, 0, null, 0, int.MaxValue))];
                        sqlQueryResult.GetBytes(0, 0, blob, 0, blob.Length);
                        string s = " ";
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            s = saveFileDialog1.FileName.ToString();
                        }
                        using (var fs = new FileStream(s, FileMode.Create, FileAccess.Write))
                            fs.Write(blob, 0, blob.Length);
                    }
            }
        }
    }
}
