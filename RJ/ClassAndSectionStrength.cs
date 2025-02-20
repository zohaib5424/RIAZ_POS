using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace RJ
{
    public partial class ClassAndSectionStrength : Form
    {
        
        public ClassAndSectionStrength()
        {
            InitializeComponent();
        }

        DataTable sections = new DataTable();
        private void Classes_mgm_Load(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = true;
            groupBox1.Hide();
            try
            {
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }
                sections.Columns.Add("GradeName");
                sections.Columns.Add("Section");
                SqlCommand cmd = new SqlCommand("Select * from grades where sec_id='null'", con);
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sd.Fill(dt);
                listBox1.Items.Clear();
                foreach (DataRow d in dt.Rows)
                {
                    listBox1.Items.Add(d[1].ToString());
                    cmd = new SqlCommand("Select * from grades where gradename='" + d[1].ToString() + "' and sec_id!='null'", con);
                    sd = new SqlDataAdapter(cmd);
                    DataTable dt1 = new DataTable();
                    sd.Fill(dt1);
                    foreach (DataRow d1 in dt1.Rows)
                    {
                        cmd = new SqlCommand("Select * from sections where id='" + d1[2].ToString() + "'", con);
                        sd = new SqlDataAdapter(cmd);
                        DataTable dt2 = new DataTable();
                        sd.Fill(dt2);
                        foreach (DataRow d2 in dt2.Rows)
                        {
                            sections.Rows.Add(d1[1].ToString(), d2[1].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBox1.Show();
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.Clear();
                }
                string gradestrength="";
                SqlCommand cmd = new SqlCommand("Select count(*) from student where grade='"+listBox1.SelectedItem.ToString() +"'", con);
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sd.Fill(dt);
                foreach (DataRow d in dt.Rows)
                {
                    gradestrength = d[0].ToString();
                }
                label2.Text = gradestrength;
                foreach (DataRow d in sections.Rows)
                {
                    if (d[0].ToString() == listBox1.SelectedItem.ToString().Trim())
                    {
                        cmd = new SqlCommand("Select count(*) from student where grade='" + listBox1.SelectedItem.ToString() + "' and section='" + d[1].ToString() + "'", con);
                        sd = new SqlDataAdapter(cmd);
                        dt = new DataTable();
                        sd.Fill(dt);
                        string strength = "";
                        foreach (DataRow l in dt.Rows)
                        {
                            strength = l[0].ToString();
                        }
                        dataGridView1.Rows.Add(listBox1.SelectedItem.ToString(), d[1].ToString(), strength);
                    }
                }
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows[0].Cells[0].Selected = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
