using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Carubbi.CurrencyWriter
{
    public class CurrencyWriterEnUS : CurrencyWriterBase
    {

        public CurrencyWriterEnUS(CultureInfo culture) :
            base(culture)
        {

        }

        protected override string ApplyOrderIdentifiers(string valuePart, int order)
        {
            string strOrder = string.Empty;
            if (!string.IsNullOrEmpty(valuePart))
            {
                switch (order)
                {
                    case 2:
                        strOrder = "thousand";
                        break;
                    case 3:
                        strOrder = valuePart == "one" ? "million" : "millions";
                        break;
                    case 4:
                        strOrder = valuePart == "one" ? "billion" : "billions";
                        break;
                    case 5:
                        strOrder = valuePart == "one" ? "trillion" : "trillions";
                        break;
                }
            }
            if (!(string.IsNullOrEmpty(strOrder)))
                valuePart += string.Format(" {0}", strOrder);
            return valuePart;

        }


        protected override string[] UnionParts(string[] parts)
        {
            string[] result = new string[2];

            result[0] = parts[0] + " ";


            int indiceConjuncao = -1;

            for (int i = 1; i < parts.Length; i++)
            {
                if (!string.IsNullOrEmpty(parts[i]))
                {

                    bool hasMoreParts = false;
                    for (int j = i + 1; j < parts.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(parts[j]))
                        {
                            hasMoreParts = true;
                            break;
                        }

                    }
                    if (hasMoreParts && string.IsNullOrEmpty(parts[0]))
                    {
                        indiceConjuncao = i;
                        break;
                    }

                }

            }

            for (int i = parts.Length - 1; i > 0; i--)
            {
                if (!string.IsNullOrEmpty(parts[i].Trim()))
                {
                    if (i == indiceConjuncao)
                        result[1] += string.Format("and {0} ", parts[i]);
                    else
                        result[1] += parts[i] + " ";
                }
            }


            return result;
        }

        protected override string ApplyCurrency(string[] parts, CurrencyType currencyType)
        {
            string centName = string.Empty;
            string currencyName = string.Empty;
            string result = string.Empty;

            if (!string.IsNullOrEmpty(parts[0].Trim()))
            {
                if (currencyType == CurrencyType.dollar)
                {
                    if (parts[0].Trim() == "one")
                    {
                        parts[0] = "";
                        centName = "a penny";
                    }
                    else if (parts[0].Trim() == "ten")
                    {
                        parts[0] = "";
                        centName = "a dimme";
                    }
                    else if (parts[0].Trim() == "fifty")
                    {
                        parts[0] = "";
                        centName = "a half";
                    }
                    else if (parts[0].Trim() == "twenty-five")
                    {
                        parts[0] = "";
                        centName = "a quarter";
                    }
                    else
                    {
                        centName = "cents";
                    }
                }
                else if (currencyType == CurrencyType.real)
                {
                    if (parts[0].Trim() == "one")
                    {
                        centName = "cent";
                    }
                    else
                    {
                        centName = "cents";
                    }
                }
            }

            if (!string.IsNullOrEmpty(parts[1]))
            {
                switch (currencyType)
                {
                    case CurrencyType.dollar:
                        if (parts[1].Trim() != "one")
                            currencyName += "dollars";
                        else
                            currencyName = "dollar";
                        break;
                    case CurrencyType.real:
                        if (parts[1].Trim() != "one")
                            currencyName += "reals";
                        else
                            currencyName = "real";
                        break;
                    case CurrencyType.peso:
                        if (parts[1].Trim() != "one")
                            currencyName += "pesos";
                        else
                            currencyName += "peso";
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            parts[0] += centName;
            parts[1] += currencyName;


            if (!string.IsNullOrEmpty(parts[1].Trim()))
                result = parts[1];


            if (!string.IsNullOrEmpty(parts[0].Trim()))
            {
                if (!string.IsNullOrEmpty(result))
                    result += " and ";

                result += parts[0];
            }

            return result;
        }

        protected override string WriteUnit(char digit)
        {
            switch (int.Parse(digit.ToString()))
            {
                case 1:
                    return "one";
                case 2:
                    return "two";
                case 3:
                    return "three";
                case 4:
                    return "four";
                case 5:
                    return "five";
                case 6:
                    return "six";
                case 7:
                    return "seven";
                case 8:
                    return "eight";
                case 9:
                    return "nine";
                default:
                    return "";
            }
        }

        protected override string WriteDozen(char digit)
        {
            switch (int.Parse(digit.ToString()))
            {
                case 1:
                    return "ten";
                case 2:
                    return "twenty";
                case 3:
                    return "thirty";
                case 4:
                    return "forty";
                case 5:
                    return "fifty";
                case 6:
                    return "sixty";
                case 7:
                    return "seventy";
                case 8:
                    return "eighty";
                case 9:
                    return "ninety";
                default:
                    return "";
            }

        }

        protected override string WriteHundred(char digit)
        {
            return string.Format("{0} {1}", WriteUnit(digit), "hundred");
        }

        protected override string UnionDigits(string unit, string dozen, string hundred)
        {
            string part1, part2, part3;
            if (dozen == "ten")
            {
                switch (unit)
                {
                    case "one":
                        part1 = "eleven";
                        break;
                    case "two":
                        part1 = "twelve";
                        break;
                    case "three":
                        part1 = "thirteen";
                        break;
                    case "four":
                        part1 = "forteen";
                        break;
                    case "five":
                        part1 = "fifteen";
                        break;
                    case "six":
                        part1 = "sixteen";
                        break;
                    case "seven":
                        part1 = "sevemteen";
                        break;
                    case "eight":
                        part1 = "eighteen";
                        break;
                    case "nine":
                        part1 = "nineteen";
                        break;
                    default:
                        part1 = string.Empty;
                        break;
                }

                if (string.IsNullOrEmpty(part1))
                {
                    part2 = dozen;
                }
                else
                {
                    part2 = string.Empty;
                }
            }
            else
            {
                part1 = unit;
                part2 = dozen;
            }


            part3 = hundred;


            if (!string.IsNullOrEmpty(part3) && (!string.IsNullOrEmpty(part2) || !string.IsNullOrEmpty(part1)))
                part3 += " and ";

            if (!string.IsNullOrEmpty(part2) && !string.IsNullOrEmpty(part1))
                part2 += "-";


            return string.Format("{0}{1}{2}", part3, part2, part1);
        }


    }
}
