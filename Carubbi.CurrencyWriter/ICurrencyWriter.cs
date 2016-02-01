using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Carubbi.CurrencyWriter
{
    public interface ICurrencyWriter
    {


        string Write(decimal value, CurrencyType currencyType);
        CultureInfo Culture { get; set; }
            
    }
}
