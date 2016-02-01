using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Carubbi.MetroLayoutEngine
{
    public partial class ConfirmDialog : Form
    {

        public DialogResult Show(string title, string message)
        {
            lblQuestionText.Text = message;
            this.Text = title;
            return this.ShowDialog();
        }

        public ConfirmDialog()
        {
            InitializeComponent();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }
    }
}
