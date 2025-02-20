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
using System.IO;

namespace RJ
{
    public partial class ViewStudents : Form
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);

        public ViewStudents()
        {
            InitializeComponent();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = textBox2.Text = "";
            groupBox2.Hide();
            groupBox3.Show();
            groupBox4.Hide();
            groupBox5.Hide();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = textBox2.Text = "";
            if (radioButton1.Checked == true)
            {
                groupBox2.Hide();
                groupBox3.Hide();
                groupBox4.Hide();
                groupBox5.Hide();
            }
        }

        AutoCompleteStringCollection students,reg;
        private void ViewStudents_Load(object sender, EventArgs e)
        {
            textBox1.Text = textBox2.Text = "";
            students = new AutoCompleteStringCollection();
            reg = new AutoCompleteStringCollection();
            try
            {
                getclassandsections();
                radioButton1.Checked = true;
                groupBox2.Hide();
                groupBox3.Hide();
                groupBox4.Hide();
                groupBox5.Hide();
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("Select * from student where status='1'", con);
                SqlDataAdapter sd = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sd.Fill(dt);
                foreach (DataRow d in dt.Rows)
                    students.Add(d[1].ToString()+" ("+d[0].ToString()+")");
                textBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
                textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                textBox1.AutoCompleteCustomSource = students;
                cmd = new SqlCommand("Select * from student where status='1'", con);
                sd = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sd.Fill(dt);
                foreach (DataRow d in dt.Rows)
                    reg.Add(d[0].ToString());
                textBox2.AutoCompleteMode = AutoCompleteMode.Suggest;
                textBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;
                textBox2.AutoCompleteCustomSource = reg;
                cmd = new SqlCommand("Select * from class_section where status='1'", con);
                sd = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sd.Fill(dt);
                _sections cc = new _sections();
                cc.id = "-9";
                cc.section = "View All";
                comboBox2.Items.Add(cc);
                foreach (DataRow d in dt.Rows)
                {
                    cmd = new SqlCommand("Select * from class where id='" + d[1].ToString() + "' and status='1'", con);
                    sd = new SqlDataAdapter(cmd);
                    DataTable dt1 = new DataTable();
                    sd.Fill(dt1);
                    int ok = 1;
                    foreach (DataRow d1 in dt1.Rows)
                    {
                        _grades g = new _grades();
                        g.id = d1[0].ToString();
                        g.grade = d1[1].ToString();
                        ok = 1;
                        foreach (_grades g2 in comboBox1.Items)
                        {
                            if (g2.grade == d1[1].ToString())
                            {
                                ok = 0;
                            }
                        }
                        if (ok == 1)
                        {
                            comboBox1.Items.Add(g);
                        }
                    }
                    ok = 1;
                    cmd = new SqlCommand("Select * from sections where id='" + d[2].ToString() + "' and status='1'", con);
                    sd = new SqlDataAdapter(cmd);
                    DataTable dt2 = new DataTable();
                    sd.Fill(dt2);
                    foreach (DataRow d2 in dt2.Rows)
                    {
                        _sections c = new _sections();
                        c.id = d2[0].ToString();
                        c.section = d2[1].ToString();
                        foreach (_sections c2 in comboBox2.Items)
                        {
                            if (c2.section == d2[1].ToString())
                            {
                                ok = 0;
                            }
                        }
                        if (ok == 1)
                        {
                            comboBox2.Items.Add(c);
                        }
                    }
                }
                comboBox1.DisplayMember = "grade";
                comboBox1.ValueMember = "id";
                comboBox2.DisplayMember = "section";
                comboBox2.ValueMember = "id";
                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        DataTable class_sections_Datatable = new DataTable();//complete table
        DataTable class_Datatable = new DataTable();//id,name
        DataTable section_Datatable = new DataTable();//id,name
        public void getclassandsections()
        {
            try
            {
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }
                string query = @"select * from class_section where status='1'";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(class_sections_Datatable);
                query = @"select * from class where status='1'";
                cmd = new SqlCommand(query, con);
                class_Datatable = new DataTable();
                sda = new SqlDataAdapter(cmd);
                sda.Fill(class_Datatable);
                query = @"select * from sections where status='1'";
                cmd = new SqlCommand(query, con);
                sda = new SqlDataAdapter(cmd);
                section_Datatable = new DataTable();
                sda.Fill(section_Datatable);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public string getsession(string id)
        {
            try
            {
                string session = "";
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }
                string query = @"select session from session where id='"+id+"' and status = '1'";
                SqlCommand cmd = new SqlCommand(query , con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                session = dt.Rows[0][0].ToString();
                return session;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return "";
        }

        public string getclass(string id)
        {
            try
            {
                string _class = "";
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }
                string query = @"select class from class where id='" + id + "' and status = '1'";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                _class = dt.Rows[0][0].ToString();
                return _class;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return "";
        }

        public string getsection(string id)
        {
            try
            {
                string section = "";
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }
                string query = @"select section from sections where id='" + id + "' and status = '1'";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                section = dt.Rows[0][0].ToString();
                return section;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return "";
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            if (con.State.ToString() == "Closed")
            {
                con.Open();
            }
            if (radioButton1.Checked == true)//view all
            {
                try
                {
                    SqlCommand cm = new SqlCommand("select * from student", con);
                    SqlDataAdapter sda = new SqlDataAdapter(cm);
                    DataTable d = new DataTable();
                    sda.Fill(d);
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = d;
                    foreach (DataGridViewRow d1 in dataGridView1.Rows)
                    {
                        dataGridView1.Rows[d1.Index].Cells["session"].Value = getsession(d1.Cells["session"].Value.ToString());
                        dataGridView1.Rows[d1.Index].Cells["grade"].Value = getclass(d1.Cells["grade"].Value.ToString());
                        dataGridView1.Rows[d1.Index].Cells["section"].Value = getsection(d1.Cells["section"].Value.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (radioButton2.Checked == true)//view class
            {
                try
                {
                    if (comboBox1.SelectedIndex >= 0 && comboBox2.SelectedIndex >= 0)
                    {
                        if (comboBox2.SelectedIndex == 0)
                        {
                            SqlCommand cm = new SqlCommand("select * from student where grade='"+((_grades)comboBox1.SelectedItem).id+"' and status='1'", con);
                            SqlDataAdapter sda = new SqlDataAdapter(cm);
                            DataTable d = new DataTable();
                            sda.Fill(d);
                            dataGridView1.DataSource = null;
                            dataGridView1.DataSource = d;
                            foreach (DataGridViewRow d1 in dataGridView1.Rows)
                            {
                                dataGridView1.Rows[d1.Index].Cells["session"].Value = getsession(d1.Cells["session"].Value.ToString());
                                dataGridView1.Rows[d1.Index].Cells["grade"].Value = getclass(d1.Cells["grade"].Value.ToString());
                                dataGridView1.Rows[d1.Index].Cells["section"].Value = getsection(d1.Cells["section"].Value.ToString());
                            }
                        }
                        else
                        {
                            SqlCommand cm = new SqlCommand("select * from student where grade='" + ((_grades)comboBox1.SelectedItem).id + "' and section='"+((_sections)comboBox2.SelectedItem).id+"' and status='1'", con);
                            SqlDataAdapter sda = new SqlDataAdapter(cm);
                            DataTable d = new DataTable();
                            sda.Fill(d);
                            dataGridView1.DataSource = null;
                            dataGridView1.DataSource = d;
                            foreach (DataGridViewRow d1 in dataGridView1.Rows)
                            {
                                dataGridView1.Rows[d1.Index].Cells["session"].Value = getsession(d1.Cells["session"].Value.ToString());
                                dataGridView1.Rows[d1.Index].Cells["grade"].Value = getclass(d1.Cells["grade"].Value.ToString());
                                dataGridView1.Rows[d1.Index].Cells["section"].Value = getsection(d1.Cells["section"].Value.ToString());
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Select Class and section");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (radioButton3.Checked == true)//view addmissiondat
            {
                try
                {
                    if (dateTimePicker2.Value.Date < dateTimePicker1.Value.Date)
                    {
                        MessageBox.Show("\"From Date\" Must Be Less Than Or Equal To \" To Date\"");
                    }
                    else
                    {
                        SqlCommand cm = new SqlCommand("select * from student where status='1' and addmission_date>='" + dateTimePicker1.Value.ToShortDateString() + "' and addmission_date<='" + dateTimePicker2.Value.ToShortDateString() + "'", con);
                        SqlDataAdapter sda = new SqlDataAdapter(cm);
                        DataTable d = new DataTable();
                        sda.Fill(d);
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = d;
                        foreach (DataGridViewRow d1 in dataGridView1.Rows)
                        {
                            dataGridView1.Rows[d1.Index].Cells["session"].Value = getsession(d1.Cells["session"].Value.ToString());
                            dataGridView1.Rows[d1.Index].Cells["grade"].Value = getclass(d1.Cells["grade"].Value.ToString());
                            dataGridView1.Rows[d1.Index].Cells["section"].Value = getsection(d1.Cells["section"].Value.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (radioButton4.Checked == true)//view nammm
            {
                try
                {
                    SqlCommand cm = new SqlCommand("select * from student where status='1' and reg='"+textBox1.Text.Trim().Split('(')[1].ToString().Split(')')[0].ToString().Trim()+"'", con);
                    SqlDataAdapter sda = new SqlDataAdapter(cm);
                    DataTable d = new DataTable();
                    sda.Fill(d);
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = d;
                    foreach (DataGridViewRow d1 in dataGridView1.Rows)
                    {
                        dataGridView1.Rows[d1.Index].Cells["session"].Value = getsession(d1.Cells["session"].Value.ToString());
                        dataGridView1.Rows[d1.Index].Cells["grade"].Value = getclass(d1.Cells["grade"].Value.ToString());
                        dataGridView1.Rows[d1.Index].Cells["section"].Value = getsection(d1.Cells["section"].Value.ToString());
                    }
                    if (dataGridView1.Rows.Count <= 0)
                    {
                        MessageBox.Show("Record Not Found");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (radioButton5.Checked == true)//view reg
            {
                try
                {
                    SqlCommand cm = new SqlCommand("select * from student where status='1' and reg='" + textBox2.Text.Trim().ToString().ToString() + "'", con);
                    SqlDataAdapter sda = new SqlDataAdapter(cm);
                    DataTable d = new DataTable();
                    sda.Fill(d);
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = d;
                    foreach (DataGridViewRow d1 in dataGridView1.Rows)
                    {
                        dataGridView1.Rows[d1.Index].Cells["session"].Value = getsession(d1.Cells["session"].Value.ToString());
                        dataGridView1.Rows[d1.Index].Cells["grade"].Value = getclass(d1.Cells["grade"].Value.ToString());
                        dataGridView1.Rows[d1.Index].Cells["section"].Value = getsection(d1.Cells["section"].Value.ToString());
                    }
                    if (dataGridView1.Rows.Count <= 0)
                    {
                        MessageBox.Show("Record Not Found");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        int index =0;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            index = e.RowIndex;
            dataGridView1.Rows[e.RowIndex].Selected = true;
            try
            {
                if (e.ColumnIndex == 4)
                {
                    var data = (Byte[])(dataGridView1.Rows[e.RowIndex].Cells["pic"].Value);
                    var stream = new MemoryStream(data);
                    Image im = Image.FromStream(stream);
                    ViewImage vi = new ViewImage(im);
                    vi.ShowDialog();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Student_Reg st = new Student_Reg(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString());
            //st.ShowDialog();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Text = "";
            groupBox2.Hide();
            groupBox3.Hide();
            groupBox4.Show();
            groupBox5.Hide();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
            groupBox2.Hide();
            groupBox3.Hide();
            groupBox4.Hide();
            groupBox5.Show();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = textBox2.Text = "";
            groupBox2.Show();
            groupBox3.Hide();
            groupBox4.Hide();
            groupBox5.Hide();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _grades classes =(_grades)comboBox1.SelectedItem;
            try
            {
                comboBox2.Items.Clear();
                comboBox2.Text = "";
                comboBox2.SelectedIndex = -1;
                _sections cc = new _sections();
                cc.id = "-9";
                cc.section = "View All";
                comboBox2.Items.Add(cc);
                foreach (DataRow d in class_sections_Datatable.Rows)
                {
                    if (d[1].ToString() == classes.id.ToString())
                    {
                        _sections sections = new _sections();
                        sections.id = d[2].ToString();
                        string sectionnam = "";
                        foreach (DataRow d2 in section_Datatable.Rows)
                        {
                            if (d[2].ToString() == d2[0].ToString())
                            {
                                sectionnam = d2[1].ToString();
                            }
                        }
                        sections.id = d[2].ToString();
                        sections.section = sectionnam;
                        int exist = 1;
                        foreach (_sections g in comboBox2.Items)
                        {
                            if (g.id == sections.id)
                            {
                                exist = 0;
                            }
                        }
                        if (exist == 1 && sectionnam.Trim() !="")
                        {
                            comboBox2.Items.Add(sections);
                        }
                    }
                }
                comboBox2.DisplayMember = "section";
                comboBox2.ValueMember = "id";
                comboBox2.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //comboBoxsection.Items.Clear();
            //foreach (DataRow d in sections.Rows)
            //{
            //    if (d[0].ToString() == comboBoxgrade.SelectedItem.ToString().Trim())
            //    {
            //        comboBoxsection.Items.Add(d[1].ToString());
            //    }
            //}
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
    }
    public class _grades
    {
        public string id { set; get; }
        public string grade { set; get; }
    }
    public class _sections
    {
        public string id { set; get; }
        public string section { set; get; }
    }
    public class _sessions
    {
        public string id { set; get; }
        public string session { set; get; }
    }
}
