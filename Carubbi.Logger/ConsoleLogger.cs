using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carubbi.Logger
{
    /// <summary>
    /// Logger responsável por logar no output padrão (Tela)
    /// </summary>
    public class ConsoleLogger : Logger
    {

        public ConsoleLogger(LogLevel mask)
            : base(mask)
        {
        
        }

        public ConsoleLogger(LogLevel mask,  Func<String, String> formatMessageHandler)
            : base(mask, formatMessageHandler)
        { }

        protected override void WriteMessage(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
