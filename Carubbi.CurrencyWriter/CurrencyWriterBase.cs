using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Carubbi.CurrencyWriter
{
    public abstract class CurrencyWriterBase : ICurrencyWriter
    {

        public CurrencyWriterBase(CultureInfo culture)
        {
            Culture = culture;
        }

        protected abstract string ApplyOrderIdentifiers(string valuePart, int order);
        protected abstract string[] UnionParts(string[] parts);
        protected abstract string ApplyCurrency(string[] parts, CurrencyType currencyType);
        protected abstract string WriteUnit(char digit);
        protected abstract string WriteDozen(char digit);
        protected abstract string WriteHundred(char digit);
        protected abstract string UnionDigits(string unit, string dozen, string hundred);

        protected virtual int[] SplitValue(decimal value)
        {
            int[] arrayReturn = new int[6];

            arrayReturn[0] = RetrieveCents(value);

            string strIntValue = Math.Floor(value).ToString();

            int i = 1;

            while (strIntValue.Length >= 3)
            {
                arrayReturn[i++] = int.Parse(strIntValue.Substring(strIntValue.Length - 3, 3));
                strIntValue = strIntValue.Substring(0, strIntValue.Length - 3);
            }
            if (strIntValue.Length > 0)
                arrayReturn[i] = int.Parse(strIntValue.Substring(0, strIntValue.Length));


            return arrayReturn;
        }

        protected virtual int RetrieveCents(decimal value)
        {
            int result;
            string[] strCents = (value % 1).ToString().Split(',');
            if (strCents.Length > 1)
            {
                if (!int.TryParse(strCents[1].PadRight(2, '0'), out result))
                    result = 0;

            }
            else
                result = 0;

            return result;
        }

        protected virtual string WritePart(int part)
        {
            string result = string.Empty;
            char[] digits = part.ToString().ToCharArray();
            string strUnit = string.Empty,
                   strDozen = string.Empty,
                   strHundred = string.Empty;

            strUnit = WriteUnit(digits[digits.Length - 1]);

            if (digits.Length >= 2)
            {
                strDozen = WriteDozen(digits[digits.Length - 2]);
            }

            if (digits.Length == 3)
            {
                strHundred = WriteHundred(digits[digits.Length - 3]);
            }

            result = UnionDigits(strUnit, strDozen, strHundred);

            return result;
        }

        #region ICurrencyWriter Members

        public string Write(decimal value, CurrencyType currencyType)
        {
            string result = string.Empty;
            if (value > 0)
            {
                Validate(value);

                
                string[] strValueParts = new string[6];
                int[] valueParts = SplitValue(value);
                for (int i = 0; i < valueParts.Length; i++)
                {
                    strValueParts[i] = WritePart(valueParts[i]);
                    strValueParts[i] = ApplyOrderIdentifiers(strValueParts[i], i);
                }



                string[] numberParts = UnionParts(strValueParts);
                result = ApplyCurrency(numberParts, currencyType);
                result = result.Substring(0, 1).ToUpper() + result.Substring(1, result.Length - 1);
            }
            return result;
        }



        private void Validate(decimal value)
        {

            /*if (value == 0)
                throw new InvalidNumberException("O Valor deve ser maior que zero.");
            */
            if (value > 999999999999999.99M)
                throw new InvalidNumberException("O Valor ultrapassa o limite permitido");
        }


        public System.Globalization.CultureInfo Culture
        {
            get;
            set;
            
        }

        #endregion
    }
}
