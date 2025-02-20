using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RJ
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            userControl11.Controls["lbl"].Text = "GM Rehman";
        }

        private void Home_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Dispose(true);
        }

        private void metroTile1_Enter(object sender, EventArgs e)
        {
            metroPanel1.Location = new Point(metroTile1.Location.X, metroTile1.Location.Y + metroTile1.Height);
        }

        private void metroTile2_Enter(object sender, EventArgs e)
        {
            metroPanel1.Location = new Point(metroTile2.Location.X, metroTile2.Location.Y + metroTile2.Height);
        }

        private void metroTile3_Enter(object sender, EventArgs e)
        {
            metroPanel1.Location = new Point(metroTile3.Location.X, metroTile3.Location.Y + metroTile3.Height);
        }

        private void metroTile3_MouseHover(object sender, EventArgs e)
        {
//            metroPanel1.Location = new Point(metroTile3.Location.X, metroTile3.Location.Y + metroTile3.Height);
        }

        private void metroTile2_MouseHover(object sender, EventArgs e)
        {
//            metroPanel1.Location = new Point(metroTile2.Location.X, metroTile2.Location.Y + metroTile2.Height);
        }

        private void metroTile1_MouseHover(object sender, EventArgs e)
        {
//            metroPanel1.Location = new Point(metroTile1.Location.X, metroTile1.Location.Y + metroTile1.Height);
        }

        private void metroTile1_MouseEnter(object sender, EventArgs e)
        {
            metroPanel1.Location = new Point(metroTile1.Location.X, metroTile1.Location.Y + metroTile1.Height);
        }

        private void metroTile2_MouseEnter(object sender, EventArgs e)
        {
            metroPanel1.Location = new Point(metroTile2.Location.X, metroTile2.Location.Y + metroTile2.Height);
        }

        private void metroTile3_MouseEnter(object sender, EventArgs e)
        {
            metroPanel1.Location = new Point(metroTile3.Location.X, metroTile3.Location.Y + metroTile3.Height);
        }

        private void metroTile4_Click(object sender, EventArgs e)
        {

        }

        private void metroTile4_Enter(object sender, EventArgs e)
        {
            metroPanel2.Location = new Point(metroTile4.Location.X, metroTile4.Location.Y + metroTile4.Height);
        }

        private void metroTile5_Enter(object sender, EventArgs e)
        {
            metroPanel2.Location = new Point(metroTile5.Location.X, metroTile5.Location.Y + metroTile5.Height);
        }

        private void metroTile6_Enter(object sender, EventArgs e)
        {
            metroPanel2.Location = new Point(metroTile6.Location.X, metroTile6.Location.Y + metroTile6.Height);
        }

        private void metroTile7_Enter(object sender, EventArgs e)
        {
            metroPanel2.Location = new Point(metroTile7.Location.X, metroTile7.Location.Y + metroTile7.Height);
        }

        private void metroTile4_MouseEnter(object sender, EventArgs e)
        {
            metroPanel2.Location = new Point(metroTile4.Location.X, metroTile4.Location.Y + metroTile4.Height);
        }

        private void metroTile5_MouseEnter(object sender, EventArgs e)
        {
            metroPanel2.Location = new Point(metroTile5.Location.X, metroTile5.Location.Y + metroTile5.Height);
        }

        private void metroTile6_MouseEnter(object sender, EventArgs e)
        {
            metroPanel2.Location = new Point(metroTile6.Location.X, metroTile6.Location.Y + metroTile6.Height);
        }

        private void metroTile7_MouseEnter(object sender, EventArgs e)
        {
            metroPanel2.Location = new Point(metroTile7.Location.X, metroTile7.Location.Y + metroTile7.Height);
        }   
    }
}
