using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carubbi.CurrencyWriter
{
    public class InvalidNumberException : ApplicationException
    {
        public InvalidNumberException(string message)
            : base(message)
        {
        
        }

        public InvalidNumberException()
            : base()
        {

        }

    }
}
