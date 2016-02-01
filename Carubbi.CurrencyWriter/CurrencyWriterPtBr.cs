using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Carubbi.CurrencyWriter
{
    public class CurrencyWriterPtBr : CurrencyWriterBase
    {

        public CurrencyWriterPtBr(CultureInfo culture) : 
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
                        strOrder = "mil";
                        break;
                    case 3:
                        strOrder = valuePart == "um" ? "milhão" : "milhões";
                        break;
                    case 4:
                        strOrder = valuePart == "um" ? "bilhão" : "bilhões";
                        break;
                    case 5:
                        strOrder = valuePart == "um" ? "trilhão" : "trilhões";
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
                    if (!parts[i].Trim().Contains("cento e") &&
                            !parts[i].Trim().Contains("duzentos e") &&
                            !parts[i].Trim().Contains("trezentos e") &&
                            !parts[i].Trim().Contains("quatrocentos e") &&
                            !parts[i].Trim().Contains("quinhentos e") &&
                            !parts[i].Trim().Contains("seiscentos e") &&
                            !parts[i].Trim().Contains("setecentos e") &&
                            !parts[i].Trim().Contains("oitocentos e") &&
                            !parts[i].Trim().Contains("novecentos e"))
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
                    else
                        break;
                }

            }

            for (int i = parts.Length - 1; i > 0; i--)
            {
                if (!string.IsNullOrEmpty(parts[i].Trim()))
                {
                    if (i == indiceConjuncao)
                        result[1] += string.Format("e {0} ", parts[i]);
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
                if (parts[0].Trim() != "um")
                    centName = "centavos";
                else
                    centName = "centavo";
            }

            if (!string.IsNullOrEmpty(parts[1]))
            {
                if (parts[1].Trim() != "um")
                {
                    if (parts[1].Trim().EndsWith("lhões") || parts[1].Trim().EndsWith("lhão"))
                        currencyName = "de ";
                    switch (currencyType)
                    {
                        case CurrencyType.real:
                            currencyName += "reais";
                            break;
                        case CurrencyType.dollar:
                            currencyName += "dólares";
                            break;
                        case CurrencyType.peso:
                            currencyName += "pesos";
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                else
                {
                    switch (currencyType)
                    {
                        case CurrencyType.real:
                            currencyName = "real";
                            break;
                        case CurrencyType.dollar:
                            currencyName = "dólar";
                            break;
                        case CurrencyType.peso:
                            currencyName = "peso";
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    
                }
            }

            parts[0] += centName;
            parts[1] += currencyName;


            if (!string.IsNullOrEmpty(parts[1].Trim()))
                result = parts[1];


            if (!string.IsNullOrEmpty(parts[0].Trim()))
            {
                if (!string.IsNullOrEmpty(result))
                    result += " e ";

                result += parts[0];
            }

            return result;
        }

        protected override string WriteUnit(char digit)
        {

            switch (int.Parse(digit.ToString()))
            {
                case 1:
                    return "um";
                case 2:
                    return "dois";
                case 3:
                    return "três";
                case 4:
                    return "quatro";
                case 5:
                    return "cinco";
                case 6:
                    return "seis";
                case 7:
                    return "sete";
                case 8:
                    return "oito";
                case 9:
                    return "nove";
                default:
                    return "";

            }
        }

        protected override string WriteDozen(char digit)
        {
            switch (int.Parse(digit.ToString()))
            {
                case 1:
                    return "dez";
                case 2:
                    return "vinte";
                case 3:
                    return "trinta";
                case 4:
                    return "quarenta";
                case 5:
                    return "cinquenta";
                case 6:
                    return "sessenta";
                case 7:
                    return "setenta";
                case 8:
                    return "oitenta";
                case 9:
                    return "noventa";
                default:
                    return "";
            }
        }

        protected override string WriteHundred(char digit)
        {
            switch (int.Parse(digit.ToString()))
            {
                case 1:
                    return "cem";
                case 2:
                    return "duzentos";
                case 3:
                    return "trezentos";
                case 4:
                    return "quatrocentos";
                case 5:
                    return "quinhentos";
                case 6:
                    return "seiscentos";
                case 7:
                    return "setecentos";
                case 8:
                    return "oitocentos";
                case 9:
                    return "novecentos";
                default:
                    return "";
            }

        }

        protected override string UnionDigits(string unit, string dozen, string hundred)
        {
            string part1, part2, part3;
            if (dozen == "dez")
            {
                switch (unit)
                {
                    case "um":
                        part1 = "onze";
                        break;
                    case "dois":
                        part1 = "doze";
                        break;
                    case "três":
                        part1 = "treze";
                        break;
                    case "quatro":
                        part1 = "quatorze";
                        break;
                    case "cinco":
                        part1 = "quinze";
                        break;
                    case "seis":
                        part1 = "dezesseis";
                        break;
                    case "sete":
                        part1 = "dezessete";
                        break;
                    case "oito":
                        part1 = "dezoito";
                        break;
                    case "nove":
                        part1 = "dezenove";
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

            if (hundred == "cem" && (!string.IsNullOrEmpty(part1) || !string.IsNullOrEmpty(part2)))
                part3 = "cento";
            else
                part3 = hundred;


            if (!string.IsNullOrEmpty(part3) && (!string.IsNullOrEmpty(part2) || !string.IsNullOrEmpty(part1)))
                part3 += " e ";

            if (!string.IsNullOrEmpty(part2) && !string.IsNullOrEmpty(part1))
                part2 += " e ";


            return string.Format("{0}{1}{2}", part3, part2, part1);
        }

       
    }
}
