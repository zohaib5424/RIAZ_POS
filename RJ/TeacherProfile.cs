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
    public partial class TeacherProfile : Form
    {
        public TeacherProfile()
        {
            InitializeComponent();
        }

        private void TeacherProfile_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.ShowDialog();
            if (o.FileName.Length > 0)
            {
                pictureBox1.Image = Image.FromFile(o.FileName.ToString());                
            }

        }

        private void label1_MouseHover(object sender, EventArgs e)
        {
            label1.ForeColor = Color.SkyBlue;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.ForeColor = Color.Blue;
        }
    }
}
