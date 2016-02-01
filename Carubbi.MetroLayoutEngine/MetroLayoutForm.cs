using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Carubbi.MetroLayoutEngine
{
    public partial class MetroLayoutForm : Form
    {
        public MetroLayoutForm()
        {
            InitializeComponent();
        }

        Stack<MetroLayoutUserControl> callStack = new Stack<MetroLayoutUserControl>();

        public void LoadPage(MetroLayoutUserControl userControl)
        {
            userControl.Voltar += userControl_Voltar;
            callStack.Push(userControl);
            AtualizarConteudo(userControl);
            userControl.Dock = DockStyle.Fill;
        }

        private void AtualizarConteudo(MetroLayoutUserControl userControl)
        {
            foreach (Control control in this.pnlConteudo.Controls)
            {
                control.Visible = false;
            }

            if (!pnlConteudo.Controls.Contains(userControl))
            {
                this.pnlConteudo.Controls.Add(userControl);
            }

            userControl.Visible = true;
        }

        void userControl_Voltar(object sender, EventArgs e)
        {
            Back();
        }

        protected void Back()
        {
            var control = callStack.Pop();
            Controls.Remove(control);
            control.Dispose();
            MetroLayoutUserControl userControl = callStack.Peek();
            AtualizarConteudo(userControl);
        }

        public string Titulo
        {
            set
            {
                lblTitulo.Text = value;
            }
            get
            {
                return lblTitulo.Text;
            }
        }

        public Color BodyBackColor
        {
            get
            {
                return pnlConteudo.BackColor;
            }

            set
            {
                pnlConteudo.BackColor = value;
            }
        }

        public Color TitleBackColor
        {
            get
            {
                return this.pnlTitulo.BackColor;
            }
            set
            {
                this.pnlTitulo.BackColor = value;
            }
        }

        public Image BrandLogo
        {
            get {
                return this.picLogo.Image;
            }
            set {
                this.picLogo.Image = value;
            }
        }

        public Color TitleForeColor
        {
            get
            {
                return lblTitulo.ForeColor;
            }
            set
            {
                lblTitulo.ForeColor = value;
            }
        }

        private MetroLayoutUserControl _paginaInicial;
        
        public MetroLayoutUserControl PaginaInicial
        {
            get
            {
                return _paginaInicial;
            }
            set
            {
                _paginaInicial = value;
                if (_paginaInicial != null)
                {
                
                    _paginaInicial.IsMainPage = true;
                    LoadPage(_paginaInicial);
                }
                else
                {
                    this.pnlConteudo.Controls.Clear();
                }

            
                this.Refresh();
               
            }
        }
    }
     
}
