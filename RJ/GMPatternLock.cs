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
    public partial class GMPatternLock : MetroFramework.Forms.MetroForm
    {
        public GMPatternLock()
        {
            InitializeComponent();
        }

        private void GMPatternLock_Load(object sender, EventArgs e)
        {
            lockScreenControl1.SetPassCode(gmpasscode);
        }

        int[] gmpasscode = { 0, 1, 2 };
        private void lockScreenControl1_PassCodeSubmitted(object sender, GestureLockApp.GestureLockControl.PassCodeSubmittedEventArgs e)
        {
            if (e.Valid)
                MessageBox.Show("a");
            else
                MessageBox.Show("error");
        }

        private void GMPatternLock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Dispose(true);
        }
    }
}
