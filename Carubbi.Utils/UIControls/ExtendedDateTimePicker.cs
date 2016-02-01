using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Carubbi.Utils.UIControls
{
    public class ExtendedDateTimePicker : DateTimePicker
    {
        private Color _backDisabledColor;
        private Color _dropDownButtonColor;
        private Color _iconButtonColor;
        public ExtendedDateTimePicker() : base()
        {
            _dropDownButtonColor = Color.Gray;
            _iconButtonColor = Color.Black;
            this.SetStyle(ControlStyles.UserPaint, true);
            _backDisabledColor = Color.FromKnownColor(KnownColor.Control);
        }

        /// <summary>
        ///     Gets or sets the background color of the control
        /// </summary>
        [Browsable(true)]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [Browsable(true)]
        public Color IconButtonColor
        {
            get { return _iconButtonColor; }
            set { _iconButtonColor = value; }
        }

        [Browsable(true)]
        public Color DropDownButtonColor
        {
            get { return _dropDownButtonColor; }
            set { _dropDownButtonColor = value; }
        }

        /// <summary>
        ///     Gets or sets the background color of the control when disabled
        /// </summary>
        [Category("Appearance"), Description("The background color of the component when disabled")]
        [Browsable(true)]
        public Color BackDisabledColor
        {
            get { return _backDisabledColor; }
            set { _backDisabledColor = value; }
        }

        [Category("Appearance"), Description("The forecolor of the component")]
        [Browsable(true)]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }


        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            //Graphics g = e.Graphics;

            //The dropDownRectangle defines position and size of dropdownbutton block, 
            //the width is fixed to 17 and height to 16. The dropdownbutton is aligned to right
            Rectangle dropDownRectangle = new Rectangle(ClientRectangle.Width - ClientRectangle.Height, 0, ClientRectangle.Height, ClientRectangle.Height);
            
            Brush bkgBrush;
            Brush foreColorBrush = new SolidBrush(ForeColor);

            ComboBoxState visualState;

            //When the control is enabled the brush is set to Backcolor, 
            //otherwise to color stored in _backDisabledColor
            if (this.Enabled)
            {
                bkgBrush = new SolidBrush(this.BackColor);
                visualState = ComboBoxState.Normal;
            }
            else
            {
                bkgBrush = new SolidBrush(this._backDisabledColor);
                visualState = ComboBoxState.Disabled;
            }

            // Painting...in action

            //Filling the background
            g.FillRectangle(bkgBrush, 0, 0, ClientRectangle.Width, ClientRectangle.Height);

            //Drawing the datetime text
            g.DrawString(this.Text, this.Font, foreColorBrush, 0, 2);

            
            
            //Drawing the dropdownbutton using ComboBoxRenderer
            CustomComboBoxRenderer.DrawDropDownButton(g, ClientRectangle, dropDownRectangle, visualState, _dropDownButtonColor, _iconButtonColor);
                     
            g.Dispose();
            bkgBrush.Dispose();
        }
    }


}
