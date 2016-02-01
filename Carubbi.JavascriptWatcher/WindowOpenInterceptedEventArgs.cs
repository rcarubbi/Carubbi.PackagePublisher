using System;

namespace Carubbi.JavascriptWatcher
{
    /// <summary>
    /// Argumentos enviados no evento que intercepta comandos javascript window.open();
    /// </summary>
    public class WindowOpenInterceptedEventArgs : EventArgs
    {
        /// <summary>
        /// Url da página que deve ser carregada na nova janela
        /// </summary>
        public Uri Url { get; set; }
    }
}
