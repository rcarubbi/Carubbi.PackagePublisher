using System.Linq;
using System.Windows.Forms;

namespace Carubbi.Utils.Validation
{
    /// <summary>
    /// Biblioteca com algorítmos de máscaras de entrada
    /// </summary>
    public class MaskHelper
    {

        /// <summary>
        /// Permite apenas numeros
        /// </summary>
        /// <param name="e"></param>
        public static void OnlyDigits(KeyEventArgs e)
        {
            Keys[] digits = new Keys[] { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.Back, Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.NumPad0,
            Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9 };

            if (!digits.Contains(e.KeyData))
            {
                e.SuppressKeyPress = true;
            }
        }

     

    }
}