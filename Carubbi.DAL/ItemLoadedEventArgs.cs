using System;

namespace Carubbi.DAL
{
    public class ItemLoadedEventArgs : EventArgs
    {
        /// <summary>
        /// Quando definido como True, Cancela o carregamento da lista
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Quando definido como True, descarta o item corrente mas continua o carregamento da lista
        /// </summary>
        public bool Skip { get; set; }
    }
}
