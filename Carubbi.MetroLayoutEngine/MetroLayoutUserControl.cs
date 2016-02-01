using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Carubbi.MetroLayoutEngine
{
    [ToolboxItem(true)]
    public partial class MetroLayoutUserControl : UserControl
    {
        public MetroLayoutUserControl()
        {
            InitializeComponent();

           
        }

        public void LoadPage(MetroLayoutUserControl control)
        {
            
            (ParentForm as MetroLayoutForm).LoadPage(control);
        }

        public event EventHandler Voltar;

        protected void PerformVoltar(object sender, EventArgs e)
        {
            if (Voltar != null)
                Voltar(sender, e);
        }

        private bool _isMainPage;
        
        public bool IsMainPage
        {
            get
            {
                return _isMainPage;
            }
            set
            {
                _isMainPage = value;
                picVoltar.Visible = !_isMainPage;
            }
        }
    }
}
