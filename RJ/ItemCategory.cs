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
    public partial class ItemCategory : UserControl
    {
        public event EventHandler treeView1AfterSelect;//form->additem work ref 1
        SqlConnection con = new SqlConnection(RJ.Properties.Settings.Default.Connectionstring);
        public ItemCategory()
        {
            InitializeComponent();
            treeView1.Nodes.Add("Header");
            LoadItems();
        }

        public void loadcategories()
        {
            try
            {
                treeView1.Nodes.Clear();
            }
            catch { }
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
                //MessageBox.Show(ex.Message);
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

        }

        private void ItemCategory_Load(object sender, EventArgs e)
        {

        }

        int parentchild = -1;
        string parnt = "";
        private void button3_Click(object sender, EventArgs e)
        {

        }

        protected void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (this.treeView1AfterSelect != null)//form->additem work ref 1
                    this.treeView1AfterSelect(this, e); if (e.Node.Text.ToString() == "Create Header")//form->additem work ref 1
                {
                    parentchild = 0;
                }
                if (e.Node.Text.ToString() == "Create Child")
                {
                    parnt = e.Node.Parent.Text;
                    parentchild = 1;
                }
            }
            catch { }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void treeView1_Click(object sender, EventArgs e)
        {

        }
    }
}
