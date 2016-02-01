using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Carubbi.CaptchaBreaker.UI
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CaptchaDialog : Form
    {
        private string _imageFullPath = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageFullPath"></param>
        public CaptchaDialog(string imageFullPath)
        {
            InitializeComponent();
            _imageFullPath = imageFullPath;
            pbCaptcha.Load(_imageFullPath);
            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string textFile = Path.ChangeExtension(_imageFullPath, "txt");
            File.WriteAllText(textFile, txtSolution.Text);
            this.Close();
        }
    }
}
