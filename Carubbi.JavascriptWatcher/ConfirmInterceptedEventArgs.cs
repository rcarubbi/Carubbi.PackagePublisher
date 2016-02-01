using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carubbi.JavascriptWatcher
{
    /// <summary>
    /// Argumentos enviados no evento de interceptação de confirm javascript
    /// </summary>
    public class ConfirmInterceptedEventArgs : EventArgs
    {
        /// <summary>
        /// Texto do confirm interceptado
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Propriedade para informar a resposta que se deseja enviar ao confirm interceptado
        /// </summary>
        public bool Result { get; set; }
    }
}
