using RJ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Cal : Form
    {
        Double resultValue = 0;
        String operationPerformed = "";
        bool isOperationPerformed = false;
        public Cal()
        {
            InitializeComponent();
        }

        int num_ok = 0;
        private void button_click(object sender, EventArgs e)
        {
            num_ok = 1; 
            try
            {
            if ((textBox_Result.Text == "0") || (isOperationPerformed))
                textBox_Result.Clear();

            isOperationPerformed = false;
            Button button = (Button)sender;
            if (button.Text == ".")
            { 
               if(!textBox_Result.Text.Contains("."))
                   textBox_Result.Text = textBox_Result.Text + button.Text;

            }else
            textBox_Result.Text = textBox_Result.Text + button.Text;
            }
            catch { textBox_Result.Text = "0"; }
        }

        private void operator_click(object sender, EventArgs e)
        {
            if (num_ok == 1)
            {
                try
                {
                    Button button = (Button)sender;

                    if (resultValue != 0)
                    {
                        button15.PerformClick();
                        operationPerformed = button.Text;
                        labelCurrentOperation.Text = resultValue + " " + operationPerformed;
                        isOperationPerformed = true;
                    }
                    else
                    {

                        operationPerformed = button.Text;
                        resultValue = Double.Parse(textBox_Result.Text);
                        labelCurrentOperation.Text = resultValue + " " + operationPerformed;
                        isOperationPerformed = true;
                    }
                }
                catch { textBox_Result.Text = "0"; }
                //textBox_Result.Text = "0";
                num_ok = 0;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                labelCurrentOperation.Text = "";
                textBox_Result.Text = "0";
            }
            catch { textBox_Result.Text = "0"; }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                textBox_Result.Text = "0";
                resultValue = 0;
            }
            catch { textBox_Result.Text = "0"; }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                switch (operationPerformed)
                {
                    case "+":
                        textBox_Result.Text = (resultValue + Double.Parse(textBox_Result.Text)).ToString();
                        break;
                    case "-":
                        textBox_Result.Text = (resultValue - Double.Parse(textBox_Result.Text)).ToString();
                        break;
                    case "*":
                        textBox_Result.Text = (resultValue * Double.Parse(textBox_Result.Text)).ToString();
                        break;
                    case "/":
                        textBox_Result.Text = (resultValue / Double.Parse(textBox_Result.Text)).ToString();
                        break;
                    default:
                        break;
                }
                resultValue = Double.Parse(textBox_Result.Text);
                labelCurrentOperation.Text = "";
                operationPerformed = "";
            }
            catch { textBox_Result.Text = "0"; }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            try
            {
                double opr1;
                if (double.TryParse(textBox_Result.Text, out opr1))
                {
                    textBox_Result.Text = (Math.Sqrt(opr1)).ToString();
                    labelCurrentOperation.Text = textBox_Result.Text;
                }
            }
            catch { textBox_Result.Text = "0"; }
            //textBox_Result.Text = "0";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
            double opr1;
            if (double.TryParse(textBox_Result.Text, out opr1))
            {
                textBox_Result.Text = (opr1 / 2).ToString();
                labelCurrentOperation.Text = textBox_Result.Text;
            }
            }
            catch { textBox_Result.Text = "0"; }
            textBox_Result.Text = "0";
        }

        private void button20_Click(object sender, EventArgs e)
        {
            try
            {
                double opr1;
                if (double.TryParse(textBox_Result.Text, out opr1))
                {
                    textBox_Result.Text = (opr1 / 4).ToString();
                    labelCurrentOperation.Text = textBox_Result.Text;
                }
            }
            catch { textBox_Result.Text = "0"; }
            textBox_Result.Text = "0";
        }

        private void Cal_Load(object sender, EventArgs e)
        {
            //textBox_Result.ReadOnly = true;
            label1.Text = RJ.Properties.Settings.Default.CopyRight;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        GMDB gm = new GMDB();
        private void textBox_Result_TextChanged(object sender, EventArgs e)
        {
            try
            {
                gm.AcceptDouble(sender, e);
            }
            catch { }
        }

        private void textBox_Result_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D0)
            {
                button19.PerformClick();
            }
            else if(e.KeyCode == Keys.D1)
            {
                button14.PerformClick();
            }
            else if(e.KeyCode == Keys.D2)
            {
                button13.PerformClick();
            }
            else if(e.KeyCode == Keys.D3)
            {
                button12.PerformClick();
            }
            else if(e.KeyCode == Keys.D4)
            {
                button9.PerformClick();
            }
            else if(e.KeyCode == Keys.D5)
            {
                button8.PerformClick();
            }
            else if(e.KeyCode == Keys.D6)
            {
                button7.PerformClick();
            }
            else if (e.KeyCode == Keys.D7)
            {
                buttonOne.PerformClick();
            }
            else if(e.KeyCode == Keys.D8)
            {
                button1.PerformClick();
            }
            else if(e.KeyCode == Keys.D9)
            {
                button2.PerformClick();
            }
        }
    }
}
//How to Create a Calculator in Visual Studio C#
//