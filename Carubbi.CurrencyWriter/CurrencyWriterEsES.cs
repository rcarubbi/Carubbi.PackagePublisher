using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Carubbi.CurrencyWriter
{
    public class CurrencyWriterEsES : CurrencyWriterBase
    {
        public CurrencyWriterEsES(CultureInfo culture) : 
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
                        strOrder = valuePart == "un" ? "millón" : "millones";
                        break;
                    case 4:
                        strOrder = valuePart == "un" ? "billón" : "billones";
                        break;
                    case 5:
                        strOrder = valuePart == "un" ? "trillón" : "trillones";
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
                    if (!parts[i].Trim().Contains("ciento ") &&
                            !parts[i].Trim().Contains("doscientos ") &&
                            !parts[i].Trim().Contains("trescientos ") &&
                            !parts[i].Trim().Contains("cuatrocientos ") &&
                            !parts[i].Trim().Contains("quinientos ") &&
                            !parts[i].Trim().Contains("seiscientos ") &&
                            !parts[i].Trim().Contains("setecientos ") &&
                            !parts[i].Trim().Contains("ochocientos ") &&
                            !parts[i].Trim().Contains("nuevecientos "))
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
                        result[1] += string.Format(" {0} ", parts[i]);
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
                if (parts[0].Trim() != "un")
                    centName = "centavos";
                else
                    centName = "centavo";
            }

            if (!string.IsNullOrEmpty(parts[1]))
            {
                if (parts[1].Trim() != "un")
                {
                    if (parts[1].Trim().EndsWith("llones") || parts[1].Trim().EndsWith("llón"))
                        currencyName = "de ";
                    switch (currencyType)
                    {
                        case CurrencyType.real:
                            currencyName += "reales";
                            break;
                        case CurrencyType.dollar:
                            currencyName += "dolares";
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
                            currencyName = "dolar";
                            break;
                        case CurrencyType.peso:
                            currencyName += "peso";
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
                    result += " con ";

                result += parts[0];
            }

            return result;
        }

        protected override string WriteUnit(char digit)
        {
            switch (int.Parse(digit.ToString()))
            {
                case 1:
                    return "un";
                case 2:
                    return "dos";
                case 3:
                    return "tres";
                case 4:
                    return "cuatro";
                case 5:
                    return "cinco";
                case 6:
                    return "seis";
                case 7:
                    return "siete";
                case 8:
                    return "ocho";
                case 9:
                    return "nueve";
                default:
                    return "";

            }
        }

       
        protected override string WriteDozen(char digit)
        {
            switch (int.Parse(digit.ToString()))
            {
                case 1:
                    return "diez";
                case 2:
                    return "veinte";
                case 3:
                    return "treinta";
                case 4:
                    return "cuarenta";
                case 5:
                    return "cincuenta";
                case 6:
                    return "sesenta";
                case 7:
                    return "setenta";
                case 8:
                    return "ochenta";
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
                    return "cien";
                case 2:
                case 3:
                case 4:
                case 6:
                case 8:
                case 9:
                    return WriteUnit(digit) + "cientos";
                case 5:
                    return "quinientos";
                case 7:
                    return "setecientos";
                default:
                    return "";
            }
        }

        protected override string UnionDigits(string unit, string dozen, string hundred)
        {
            string part1, part2, part3;
            if (dozen == "diez")
            {
                switch (unit)
                {
                    case "un":
                        part1 = "once";
                        break;
                    case "dos":
                        part1 = "doce";
                        break;
                    case "tres":
                        part1 = "trece";
                        break;
                    case "cuatro":
                        part1 = "catorce";
                        break;
                    case "cinco":
                        part1 = "quince";
                        break;
                    case "seis":
                        part1 = "dieciséis";
                        break;
                    case "siete":
                        part1 = "diecisiete";
                        break;
                    case "ocho":
                        part1 = "dieciocho";
                        break;
                    case "nueve":
                        part1 = "diecinueve";
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

            if (hundred == "cien" && (!string.IsNullOrEmpty(part1) || !string.IsNullOrEmpty(part2)))
                part3 = "ciento";
            else
                part3 = hundred;


            if (!string.IsNullOrEmpty(part3) && (!string.IsNullOrEmpty(part2) || !string.IsNullOrEmpty(part1)))
                part3 += " ";

            if (!string.IsNullOrEmpty(part2) && !string.IsNullOrEmpty(part1))
                part2 += " y ";


            return string.Format("{0}{1}{2}", part3, part2, part1);
        }
    }
}
