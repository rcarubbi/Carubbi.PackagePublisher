using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Carubbi.ComponentClient
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            InitializeComponent();
        }

        private void btnGsaCaptchaBreakerTest_Click(object sender, EventArgs e)
        {
            GSACaptchaBreakerServiceProxy.CaptchaBreakerClient client = new GSACaptchaBreakerServiceProxy.CaptchaBreakerClient();
            byte[] bytes = File.ReadAllBytes("captcha.gif");
            var resultado = client.Break(bytes, new GSACaptchaBreakerServiceProxy.CaptchaConfig());
            MessageBox.Show(resultado);
        }
    }
}
