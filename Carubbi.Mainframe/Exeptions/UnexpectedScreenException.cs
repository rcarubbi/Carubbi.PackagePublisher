using System;

namespace Carubbi.Mainframe.Exeptions
{
    /// <summary>
    /// Exceção disparada quando uma tela não esperada é encontrada após uma navegação
    /// </summary>
    public class UnexpectedScreenException : Exception
    {
        private string _screen;
        
        public string Screen
        {
            get { return _screen; }
        }

  
        public UnexpectedScreenException(string message, string screen)
            : base(message, null)
        {
            _screen = screen;
        }

        public UnexpectedScreenException(string message, string screen, Exception innerException)
            : base(message, innerException)
        {
            _screen = screen;
        }
    }
}
