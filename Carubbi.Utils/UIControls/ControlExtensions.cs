using System;
using System.Windows.Forms;

namespace Carubbi.Utils.UIControls
{

    /// <summary>
    /// Extension Methods da Classe Control
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// Envolve um trecho de código no padrão InvokeRequired
        /// <para>Verifica se é necessária a passagem do controle para a thread principal caso o controle tenha sido construido a partir dela para poder chamar o método no delegate action</para>
        /// <example>
        ///     var objeto = new Foo();
        ///     objeto.InvokeIfRequired(obj => {
        ///         obj.Prop = 123; 
        ///         obj.Metodo();    
        ///     });
        /// </example>
        /// </summary>
        /// <param name="c">Controle</param>
        /// <param name="action">Método a ser executado pela thread</param>
        public static void InvokeIfRequired(this Control c, Action<Control> action)
        {
            // indica que o controle foi criado em outra thread e a thread corrente não pode chamar o método solicitado
            if (c.InvokeRequired)
            {
                // Troca para a thread "dona" do controle
                c.Invoke(action, c);
            }
            else
            {
                // Neste caso a Thread corrente é capaz de executar o método
                action(c);
            }
        }
    }
}
