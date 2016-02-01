using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Carubbi.CurrencyWriter
{
    public class CurrencyWriterFactory
    {
        private volatile static CurrencyWriterFactory _instance;
        public static CurrencyWriterFactory GetInstance()
        {
            if (_instance == null)
            {
                lock (typeof(CurrencyWriterFactory))
                {
                    if (_instance == null)
                    {
                        _instance = new CurrencyWriterFactory();
                    }
                }
            }
            return _instance;
        }


       public static IList<CultureInfo> ListCultures()
        {
            IList<CultureInfo> cultures = new List<CultureInfo>();
            cultures.Add(new CultureInfo("pt"));
            cultures.Add(new CultureInfo("pt-BR"));
            cultures.Add(new CultureInfo("en"));
            cultures.Add(new CultureInfo("en-US"));
            cultures.Add(new CultureInfo("es"));
            cultures.Add(new CultureInfo("es-ES"));
            cultures.Add(new CultureInfo("es-CL"));
            return cultures;
        }

        public static IList<CurrencyType> ListCurrencies()
        {
            IList<CurrencyType> currencies = new List<CurrencyType>();
            currencies.Add(CurrencyType.real);
            currencies.Add(CurrencyType.dollar);
            currencies.Add(CurrencyType.euro);
            currencies.Add(CurrencyType.peso);
            return currencies;
        }

        public ICurrencyWriter GetCurrencyWriter(CultureInfo culture)
        {
            switch(culture.Name)
            {
                case "pt":
                case "pt-BR":
                    return new CurrencyWriterPtBr(culture);
                case "en":
                case "en-US":
                    return new CurrencyWriterEnUS(culture);
                case "es":
                case "es-ES":
                case "es-CL":
                    return new CurrencyWriterEsES(culture);
                default:
                    throw new NotImplementedException();
            }
                
            
                
        }

    }
}




