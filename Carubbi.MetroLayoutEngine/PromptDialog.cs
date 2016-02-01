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
    public partial class PromptDialog<T> : Form
    {
        private string _validationMessage;

        public DialogResult Show(string title, string message, string validationMessage)
        {
            lblMessage.Text = message;
            _validationMessage = validationMessage;
            this.Text = title;
            return this.ShowDialog();
        }

        public PromptDialog()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (AnswerIsValid)
            {
                Answer = (T)Converter.ConvertFromString(txtAnswer.Text);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            { 
                MessageBox.Show(this, _validationMessage, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool AnswerIsValid
        {
            get
            {

                return Converter.IsValid(txtAnswer.Text);
            }
          
        }

        private TypeConverter Converter
        {
            get
            {
                return TypeDescriptor.GetConverter(typeof(T));
            }
        }

        public T Answer { 
            get; 
            private set;
        }
    }
}
