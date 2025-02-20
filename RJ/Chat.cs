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
    public partial class Chat : Form
    {
        public Chat()
        {
            InitializeComponent();
        }

        int btnlx;
        int btnly;
        private void Chat_Load(object sender, EventArgs e)
        {
            richTextBox1.Hide();
            btnlx = button1.Location.X;
            btnly = button1.Location.Y;
            button1.Text = "GM";
            panel1.Hide();
            button1.Location = new Point(button1.Location.X, this.Height - button1.Height);

            int spacing = 15;
            int columns = 80;

            //Use your variable above to create the array
            Label[] map = new Label[columns];
            RichTextBox[] m2 = new RichTextBox[columns];

            int w = 0;
            int k = 0;
            //for (int j = 0; j < columns; j++)
            //{
            //    map[j] = new Label();
            //    map[j].AutoSize = true;

            //    map[j].BackColor = Color.SkyBlue;
            //    map[j].Location = new Point(0, j * spacing);
            //    map[j].Name = "map" + j.ToString() + "," + j.ToString();
            //    map[j].ForeColor = Color.Green;
            //    map[j].Width = spacing;
            //    map[j].Height = spacing;
            //    map[j].TabIndex = 0;
            //    map[j].Text = j.ToString();
            //    panel1.Controls.Add(map[j]);
            //}
            //for (int j = 0; j < columns; j++)
            //{
            //    map[j] = new Label();
            //    map[j].AutoSize = true;

            //    if (j % 2 != 0)
            //    {
            //        map[j].BackColor = Color.SkyBlue;
            //    }
            //    else
            //    {
            //        map[j].BackColor = Color.GreenYellow;
            //    }
            //    map[j].Name = "map" + j.ToString() + "," + j.ToString();
            //    map[j].ForeColor = Color.Green;
            //    map[j].Width = spacing+2;
            //    map[j].TabIndex = 0;
            //    map[j].Text ="gm: ("+DateTime.Now.ToString()+")"+ j.ToString() + "\ngm";
            //    if (j == 5)
            //    {
            //        map[j].Text = j.ToString() + "asdfasdf\ngm\ngmasdf";
            //    }
            //    if (j != 0)
            //    {
            //        map[j].Location = new Point(2, map[j - 1].Location.Y + map[j - 1].Height + 5);
            //    }
            //    else
            //    {
            //        map[j].Location = new Point(2, j * map[j].Height + 10);
            //    }
            //    k = map[j].Location.Y;
            //    panel1.Controls.Add(map[j]);
            //}
            for (int j = 0; j < columns; j++)
            {
                m2[j] = new RichTextBox();
                m2[j].AutoSize = true;
                int lx = 0;
                if (j % 2 != 0)
                {
                    m2[j].BackColor = Color.SkyBlue;
                    lx = 2;
                }
                else
                {
                    m2[j].BackColor = Color.GreenYellow;
                    lx = richTextBox1.Width-m2[j].Width-1;
                }
                m2[j].Name = "map" + j.ToString() + "," + j.ToString();
                m2[j].ForeColor = Color.Green;
                m2[j].ReadOnly = true;


                m2[j].BorderStyle = BorderStyle.None;
                Label l = new Label();
                m2[j].TabIndex = 0;
                m2[j].Text = "gm: (" + DateTime.Now.ToString() + ")" + j.ToString() + "\ngm";
                l.Text = m2[j].Text;
                if (j == 5)
                {
                    m2[j].Text = j.ToString() + "asdfasdf\ngm\ngmasdf";
                }
                if (j != 0)
                {
                    m2[j].Location = new Point(lx, m2[j - 1].Location.Y + m2[j - 1].Height + 5);
                }
                else
                {
                    m2[j].Location = new Point(lx, j * m2[j].Height + 10);
                }
                k = m2[j].Location.Y;
                panel1.Controls.Add(m2[j]);
            }

            panel1.AutoScroll = true;
            button1.Show();
        }

        
        private void button1_Click(object sender, EventArgs e)
        {

            if (panel1.Visible.ToString() == "False")
            {
                panel1.Show();
                richTextBox1.Show();
                button1.Location = new Point(button1.Location.X, panel1.Location.Y - button1.Height);
            }
            else
            {
                panel1.Hide();
                richTextBox1.Hide();
                button1.Location = new Point(button1.Location.X, this.Height-button1.Height);
            }
        }
    }
}
