using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Carubbi.Utils.UIControls
{
    /// <summary>
    /// Checkbox customizado para ser colocado na Toolstrip
    /// </summary>
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public class ToolstripCheckbox : ToolStripControlHost
    {
        private CheckBox _checkboxControl;

        public ToolstripCheckbox()
            : base(new CheckBox())
        {
            this._checkboxControl = this.Control as CheckBox;
        }
    }
}
