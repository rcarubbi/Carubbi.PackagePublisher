using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Carubbi.MetroLayoutEngine
{
    [DefaultEvent("TileClick")]
    public partial class Tile : UserControl
    {
        private const int SMALL_WIDTH = 176;
        
        private const int SMALL_HEIGHT = 176;

        private const int MEDIUM_WIDTH = 176 * 2;
        
        private const int MEDIUM_HEIGHT = 176;

        private const int BIG_WIDTH = 176 * 2;
        
        private const int BIG_HEIGHT = 176 * 2;
                
        private bool _showBorder = false;
        
        void RaiseTileClick(object sender, System.EventArgs e)
        {
            if (TileClick != null)
                TileClick(sender, e);
        }
        
        public Tile()
        {
            InitializeComponent();
       
        }

        public event EventHandler TileClick;

        public string Titulo
        {
            get
            {
                return lblTitulo.Text;
            }
            set
            {
                lblTitulo.Text = value;
            }
        }

        public Image Icone
        {
            get
            {
                return picIcone.Image;
            }
            set
            {
                picIcone.Image = value;
            }
        }
        
        private ModoTile _modo;
        
        public ModoTile Modo
        {
            get
            {
                return _modo;
            }
            set
            {
                _modo = value;
                AlterarModo(_modo);
                if (DesignMode)
                {
                    Refresh();  
                }
            }
        }

        private void AlterarModo(ModoTile value)
        {
            switch (value)
            {
                case ModoTile.Pequeno:
                    this.Width = SMALL_WIDTH;
                    this.Height = SMALL_HEIGHT;
                    break;
                case ModoTile.Medio:
                    this.Width = MEDIUM_WIDTH;
                    this.Height = MEDIUM_HEIGHT;
                    break;
                case ModoTile.Grande:
                    this.Width = BIG_WIDTH;
                    this.Height = BIG_HEIGHT;
                    break;
                default:
                    break;
            }
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_showBorder)
            {
                var bounds =  e.ClipRectangle;
                bounds.Inflate(-1, -1);
                Pen whitePen = new Pen(Color.White, 1);
                e.Graphics.DrawRectangle(whitePen, bounds);
            }
          
        }

        private void Tile_MouseEnter(object sender, EventArgs e)
        {
            _showBorder = true;
            this.Refresh();
            _showBorder = false;
        }

        private void Tile_MouseLeave(object sender, EventArgs e)
        {
            if (this.ClientRectangle.Contains(this.PointToClient(Control.MousePosition)))
                return;
            else
            {
                this.Refresh();
            }
        }
    }
}

