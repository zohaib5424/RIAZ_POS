using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace RJ
{
    public partial class MainCustomers : Form
    {
        public MainCustomers()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void MainCustomers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Dispose();
        }

        GMDB gm = new GMDB();
        public List<string> maincustomersnames = new List<string>();
        private void MainCustomers_Load(object sender, EventArgs e)
        {
            listBox1.SelectionMode = SelectionMode.MultiSimple;
            try
            {
                string billdate = DateTime.Now.Date.Year.ToString() + "-" + DateTime.Now.Date.Month.ToString() + "-" + DateTime.Now.Date.Day.ToString();
                DataTable dt = gm.GetTable(@"select id,customer_Or_Vendor_Name from Customer_Or_Vendor where parent='' and id in
(
select distinct Customer_Or_Vendor.parent from customer_or_vendor
inner join
bill on customer_Or_Vendor.id = Bill.Customer_Vendor_Id
 where customer_or_vendor.status ='1' and customer_or_vendor.customer_vendor_type='Customer' and
 bill.status ='1' and bill.Bill_Type='Sale Trading' and bill.Bill_Date='"+billdate+"')");
                foreach (DataRow d in dt.Rows)
                {
                    listBox1.Items.Add(d["customer_or_vendor_name"].ToString());
                }
            }
            catch { }
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    foreach (string names in listBox1.SelectedItems)
                    {
                        maincustomersnames.Add(names);
                    }
                    this.Dispose();
                }catch{}
            }
        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (string names in listBox1.SelectedItems)
                {
                    maincustomersnames.Add(names);
                }
                this.Dispose();
            }
            catch { }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private const int LB_SETITEMHEIGHT = 0x01A0;
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, int itemIndex, int itemHeight);

        private void listBox1_Resize(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                MeasureItemEventArgs eArgs = new MeasureItemEventArgs(listBox1.CreateGraphics(), i);
                listBox1_MeasureItem(listBox1, eArgs);
                SendMessage(listBox1.Handle, LB_SETITEMHEIGHT, i, eArgs.ItemHeight*2);
            }
            listBox1.Refresh();
        }

        private void listBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {

        }
    }
}
