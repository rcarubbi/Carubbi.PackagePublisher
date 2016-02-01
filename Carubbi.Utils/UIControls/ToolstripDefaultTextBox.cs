using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Carubbi.Utils.UIControls
{
    /// <summary>
    /// Textbox padrão customizado para ser colocado na Toolstrip
    /// </summary>
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public class ToolstripDefaultTextBox: ToolStripControlHost
    {
        private TextBox _textBoxControl;

        public ToolstripDefaultTextBox()
            : base(new TextBox())
        {
            this._textBoxControl = this.Control as TextBox;
        }

        /// <summary>
        /// Caractere para senhas
        /// </summary>
        public char PasswordChar
        {
            get
            {
                return _textBoxControl.PasswordChar;
            }
            set
            {


                _textBoxControl.PasswordChar = value;
            }
        }
    }
}
