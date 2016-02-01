using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace Carubbi.Utils.Data
{
    public static class CNPJHelper
    {
        /// <summary>
        /// Retorna um CNPJ completo, com os digitos verificadores
        /// </summary>
        /// <param name="strCNPJSemDigitosVerificadores">CNPJ sem digitos Verificadores</param>
        /// <returns>CNPJ Completo</returns>
        public static string CompletarDigitos(string cnpj)
        {
            if (cnpj.Length > 12)  cnpj = cnpj.Substring(0, 12);

            //VALIDAR CNPJ
            int dig1, dig2, dig3, dig4, dig5, dig6, dig7, dig8, dig9, dig10, dig11, dig12, dig13, dig14, dv1, dv2, qDig;

            qDig = cnpj.Length;

            //Gravar posição dos caracteres
            dig1 = cnpj.Substring(qDig - 12, 1).To<short>(0);
            dig2 = cnpj.Substring(qDig - 11, 1).To<short>(0);
            dig3 = cnpj.Substring(qDig - 10, 1).To<short>(0);
            dig4 = cnpj.Substring(qDig - 9, 1).To<short>(0);
            dig5 = cnpj.Substring(qDig - 8, 1).To<short>(0);
            dig6 = cnpj.Substring(qDig - 7, 1).To<short>(0);
            dig7 = cnpj.Substring(qDig - 6, 1).To<short>(0);
            dig8 = cnpj.Substring(qDig - 5, 1).To<short>(0);
            dig9 = cnpj.Substring(qDig - 4, 1).To<short>(0);
            dig10 = cnpj.Substring(qDig - 3, 1).To<short>(0);
            dig11 = cnpj.Substring(qDig - 2, 1).To<short>(0);
            dig12 = cnpj.Substring(qDig - 1, 1).To<short>(0);
            //dig13 = Convert.ToInt16(strCNPJ.Substring(qDig - 2, 1));
            //dig14 = Convert.ToInt16(strCNPJ.Substring(qDig - 1, 1));

            //Cálculo para o primeiro dígito validador
            dv1 = (dig1 * 6) + (dig2 * 7) + (dig3 * 8) + (dig4 * 9) + (dig5 * 2) + (dig6 * 3) + (dig7 * 4) + (dig8 * 5) + (dig9 * 6) + (dig10 * 7) + (dig11 * 8) + (dig12 * 9);
            dv1 = dv1 % 11;

            if (dv1 == 10)
            {
                dv1 = 0; //Se o resto for igual a 10, dv1 igual a zero
            }
            dig13 = dv1;

            //Cálculo para o segundo dígito validador
            dv2 = (dig1 * 5) + (dig2 * 6) + (dig3 * 7) + (dig4 * 8) + (dig5 * 9) + (dig6 * 2) + (dig7 * 3) + (dig8 * 4) + (dig9 * 5) + (dig10 * 6) + (dig11 * 7) + (dig12 * 8) + (dv1 * 9);
            dv2 = dv2 % 11;

            if (dv2 == 10)
            {
                dv2 = 0; //Se o resto for igual a 10, dv1 igual a zero

            }
            dig14 = dv2;

            //Validação dos dígitos validadores, após o cálculo realizado
            return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}",
                                dig1, dig2, dig3, dig4, dig5, dig6, dig7, dig8, dig9, dig10, dig11, dig12, dig13, dig14);
        }

        /// <summary>
        /// Pega a Raiz do CNPJ informado e resolve os digitos verificadores para o sufixo 0001
        /// </summary>
        /// <param name="cnpj">CNPJ a ser resolvido</param>
        /// <returns>Raiz do original + Sufixo 0001 e seu DV</returns>
        public static string GetMilContra(string cnpj)
        {
            cnpj = ToString(cnpj);
            string cnpjMatriz = string.Format("{0}0001", cnpj.Substring(0, cnpj.Length - 6));
            cnpjMatriz = CompletarDigitos(cnpjMatriz);
            return cnpjMatriz;
        }

        /// <summary>
        /// Verifica se o sufixo de um CNPJ é 0001
        /// </summary>
        /// <param name="cnpj">CNPJ a ser analisado</param>
        /// <returns>indicador se possui sufixo igual à 0001</returns>
        public static bool IsMilContra(string cnpj)
        {
            return GetSufixo(cnpj) == "0001";
        }

        /// <summary>
        /// Verifica se o sufixo de um CNPJ é 0001
        /// </summary>
        /// <param name="cnpj">CNPJ a ser analisado</param>
        /// <returns>indicador se possui sufixo igual à 0001</returns>
        public static bool IsMilContra(long cnpj)
        {
            return IsMilContra(CNPJHelper.ToString(cnpj));
        }

        /// <summary>
        /// Retorna o sufixo de um CNPJ
        /// </summary>
        /// <param name="cnpj">CNPJ a ser analisado</param>
        /// <returns>Sufixo do CNPJ</returns>
        public static string GetSufixo(string cnpj)
        {
            return ToString(cnpj).Substring(8, 4);
        }

        /// <summary>
        /// Converte o cnpj numérico para string completando com 0s à esquerda
        /// </summary>
        /// <param name="cnpj">CNPJ</param>
        /// <returns>CNPJ Convertido</returns>
        public static string ToString(long cnpj)
        {
            return cnpj.ToString().PadLeft(14, '0');
        }


        /// <summary>
        /// Converte um texto que contem um cnpj em apenas o CNPJ
        /// </summary>
        /// <param name="cnpj">CNPJ</param>
        /// <returns>CNPJ Convertido</returns>
        public static string ToString(string cnpj)
        {
            cnpj = Regex.Replace(cnpj ?? string.Empty, @"[^\d]", string.Empty);
            return cnpj.Trim().PadLeft(14, '0');
        }

        /// <summary>
        ///  Converte um cnpj em texto para número
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public static long ToLong(string cnpj)
        {
            cnpj = Regex.Replace(cnpj ?? string.Empty, @"[^\d]", string.Empty);
            return cnpj.PadLeft(14, '0').To<long>(0);
        }

        /// <summary>
        /// Recupera a Raiz de um CNPJ
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        public static string GetRaiz(string cnpj)
        {
            return ToString(cnpj).Substring(0, 8);
        }

        /// <summary>
        /// Recupera os digitos verificadores
        /// </summary>
        /// <param name="cnpj">CNPJ</param>
        /// <returns>Digitos Verificadores</returns>
        public static string GetDigitos(string cnpj)
        {
            return ToString(cnpj).Substring(12, 2);
        }
    }
}
