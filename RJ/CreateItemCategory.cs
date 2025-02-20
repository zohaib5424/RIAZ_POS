using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace RJ
{
    public partial class CreateItemCategory : Form// MetroFramework.Forms.MetroForm
    {
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public CreateItemCategory()
        {
            InitializeComponent();
            treeView1.Nodes.Add("Header");
            LoadItems();
        }

        public void loadcategories()
        {
            textBox3.Enabled = false;
            button3.Enabled = false;
            treeView1.Nodes.Clear();
            try
            {
                if (con.State.ToString() == "Closed")
                {
                    con.Open();
                }
                string query = @"select * from categories where parent_location = '' and status='1'";
                SqlCommand cmd = new SqlCommand(query, con);
                DataTable dt1 = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt1);
                foreach (DataRow d in dt1.Rows)
                {
                    if (d[2].ToString().Trim() != "" && (!treeView1.Nodes.ContainsKey(d[2].ToString().Trim())))
                    {
                        treeView1.Nodes.Add(d[2].ToString());
                        try
                        {
                            //
                            query = @"select * from categories where parent_location='" + d[2].ToString() + "' and status='1'";
                            cmd = new SqlCommand(query, con);
                            DataTable dt5 = new DataTable();
                            sda = new SqlDataAdapter(cmd);
                            sda.Fill(dt5);
                            foreach (DataRow d1 in dt5.Rows)
                            {
                                if (d1[2].ToString().Trim() != "" && (!treeView1.Nodes.ContainsKey(d1[2].ToString().Trim())))
                                {
                                    treeView1.Nodes[treeView1.Nodes.Count - 1].Nodes.Add(d1[2].ToString());
                                }
                            }

                        }
                        catch { }
                        //
                    }
                }
            }
            catch { }
            treeView1.Nodes.Add("Create Parent");
        }

        DataTable dtHeaders2;
        public void LoadItems()
        {
            try
            {
                metroTile1.Enabled = false;
                //gmnew
                string query = "SELECT * FROM categories where status='1'";
                if (con.State.ToString() == "Closed")
                    con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                dtHeaders2 = new DataTable();
                sda.Fill(dtHeaders2);
                try
                {
                    treeView1.Nodes[0].Nodes.Clear();
                }
                catch { }
                if (dtHeaders2.Rows.Count == 0)
                {
                    createDefaultTreeNode2(treeView1.Nodes[0]);
                }

                if (dtHeaders2.Select("[parent_location] IS NULL").Length < 1) return;
                foreach (DataRow dr in dtHeaders2.Select("[parent_location] IS NULL"))
                    createTreeNode2(dr, null);
                createDefaultTreeNode2(treeView1.Nodes[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private DataRow createTreeNode2(DataRow dr, TreeNode parentItem)
        {
            if (dr == null)
                return null;
            // 
            // createHeaderToolStripMenuItem
            // 

            System.Windows.Forms.TreeNode item = new TreeNode();
            item.Name = "treeNode" + dr["child_location"].ToString();
            item.Text = dr["child_location"].ToString();
            item.Tag = dr["id"];

            if (parentItem == null)
                treeView1.Nodes[0].Nodes.Add(item);
            else
                parentItem.Nodes.Add(item);
            DataTable dt;
            try
            {
                dt = dtHeaders2.Select("[parent_location] = " + "\'" + item.Text.Trim().ToString() + "\'", "child_location ASC").CopyToDataTable();
            }
            catch { createDefaultTreeNode2(item); return null; }

            for (int i = 0; i < dt.Rows.Count; i++)
                createTreeNode2(dt.Rows[i], item);

            createDefaultTreeNode2(item);
            return null;
        }

        private void createDefaultTreeNode2(TreeNode parentItem)
        {
            System.Windows.Forms.TreeNode item = new TreeNode();
            item.Name = "createHeaderNode";
            item.Text = "Create Header";
            parentItem.Nodes.Add(item);
        }

        private void ItemCategory_Load(object sender, EventArgs e)
        {
            textBox3.Enabled = false;
            //metroTile1.Hide();
        }

        int parentchild = -1;
        string parnt = "";
        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node.Text.ToString() == "Create Header")
                {
                    textBox3.Enabled = true;
                    button3.Enabled = true;
                    parentchild = 0;
                }
                if (e.Node.Text.ToString() == "Create Child")
                {
                    textBox3.Enabled = true;
                    parnt = e.Node.Parent.Text;
                    button3.Enabled = true;
                    parentchild = 1;
                }
                if (treeView1.SelectedNode != null)
                {
                    if (treeView1.SelectedNode.Level > 0)
                    {
                        if (treeView1.SelectedNode.Nodes.Count == 1)
                        {
                            if (treeView1.SelectedNode.Nodes[0].Text == "Create Header")
                            {
                                metroTile1.Enabled = true;
                            }
                        }
                        else
                        {
                            metroTile1.Enabled = false;
                        }
                    }
                }
            }
            catch { }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button3.PerformClick();
        }

        private void treeView1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (textBox3.Text.Trim() != "")
                {
                    if (treeView1.SelectedNode.Level == 1)
                    {
                        if (con.State.ToString() == "Closed")
                        {
                            con.Open();
                        }
                        string query = @"select max(cast(id as int)) from categories";
                        SqlCommand cmd = new SqlCommand(query, con);
                        DataTable dt1 = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        sda.Fill(dt1);
                        string id = "";
                        foreach (DataRow d in dt1.Rows)
                        {
                            id = d[0].ToString();
                        }
                        if (id.Trim() == "")
                        {
                            id = "0";
                        }
                        id = (int.Parse(id.Trim()) + 1).ToString();
                        query = @"select * from categories where (parent_location='" + textBox3.Text.Trim() + "' or child_location='" + textBox3.Text.Trim() + "') and status!='-1'";
                        dt1 = gm.GetTable(query);
                        if(dt1.Rows.Count>0)
                        {
                            MessageBox.Show("Category Already Exist");
                            textBox3.Focus();
                            return;
                        }
                        textBox3.Enabled = false;
                        query = @"insert into categories values('" + id + "'," + "NULL" + ",N'" + textBox3.Text.Trim() + "','1')";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Category Inserted");
                        //loadcategories();
                        textBox3.Text = "";
                        button3.Enabled = false;
                        parentchild = -1;
                    }
                    else if (treeView1.SelectedNode.Level > 1)
                    {
                        if (con.State.ToString() == "Closed")
                        {
                            con.Open();
                        }
                        string query = @"select max(cast(id as int)) from categories";
                        SqlCommand cmd = new SqlCommand(query, con);
                        DataTable dt1 = new DataTable();
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        sda.Fill(dt1);
                        string id = "";
                        foreach (DataRow d in dt1.Rows)
                        {
                            id = d[0].ToString();
                        }
                        if (id.Trim() == "")
                        {
                            id = "0";
                        }
                        id = (int.Parse(id.Trim()) + 1).ToString();
                        query = @"select * from categories where (parent_location='" + textBox3.Text.Trim() + "' or child_location='" + textBox3.Text.Trim() + "') and status!='-1'";
                        dt1 = gm.GetTable(query);
                        if (dt1.Rows.Count > 0)
                        {
                            MessageBox.Show("Category Already Exist");
                            textBox3.Focus();
                            return;
                        }
                        textBox3.Enabled = false;
                        query = @"insert into categories values('" + id + "',N'" + treeView1.SelectedNode.Parent.Text.Trim().ToString() + "',N'" + textBox3.Text.Trim() + "','1')";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Category Inserted Successfully");
                        //loadcategories();
                        textBox3.Text = "";
                        button3.Enabled = false;
                        parentchild = -1;
                        parnt = "";

                    }
                    LoadItems();
                }
                else
                {
                    MessageBox.Show("Enter Category");
                }
            }
            catch { }
        }

        private void textBox3_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button3.PerformClick();
        }

        private void CreateItemCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Dispose(true);
        }

        GMDB gm = new GMDB();
        private void metroTile1_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeView1.SelectedNode != null)
                {
                    if (treeView1.SelectedNode.Level > 0)
                    {
                        if (treeView1.SelectedNode.Nodes.Count == 1)
                        {
                            if (treeView1.SelectedNode.Nodes[0].Text == "Create Header")
                            {
                                string query = "update categories set status = '-1' where id='" + treeView1.SelectedNode.Tag.ToString() + "'";
                                int b = gm.ExecuteNonQuery(query);
                                if (b > 0)
                                {
                                    MessageBox.Show("Category Successfully Deleted");
                                    LoadItems();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("First Delete End Node/Child of selected node");
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void metroPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
