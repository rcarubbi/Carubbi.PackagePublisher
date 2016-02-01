
using System.Text.RegularExpressions;
namespace Carubbi.Utils.Validation
{
    /// <summary>
    /// Extension Methods de validação
    /// </summary>
    public static class ValidationExtension
    {
        /// <summary>
        /// Verifica se a string é um CNPJ válido
        /// </summary>
        /// <param name="cnpj">string chamadora</param>
        /// <returns>Resultado da validação</returns>
        public static bool IsCnpj(this string cnpj)
        {
            cnpj = cnpj.PadLeft(14, '0');

            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }

        /// <summary>
        /// Valida E-mail a partir do padrão RFC 2822
        /// Fonte: http://www.regular-expressions.info/email.html
        /// </summary>
        /// <param name="email">Endereço de e-mail para ser validado</param>
        /// <returns>Resultado da validação</returns>
        public static bool IsValidEmail(this string email)
        { 
            if (email == null)
                return false;

            if (email.Trim() == string.Empty)
                return false;

            email = email.Trim().ToLower();

            // E-mail válido perante a especificação
            string rfc2822 = @"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])";
            Regex regex1 = new Regex(rfc2822);

            // E-mail com domínio válido
            string custom = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+(?:[A-Z]{3}|com|org|net|edu|gov|mil|biz|info|mobi|name|aero|asia|jobs|museum|eco|emp|agr|am|art|b|coop|esp|far|fm|g12|imb|ind|inf|jus|leg|psi|mp|radio|rec|srv|tmp|tur|tv|etc|adm|adv|arq|ato|bio|bmd|cim|cng|cnt|ecn|eng|eti|fnd|fot|fst|ggf|jor|lel|mat|med|mus|not|ntr|odo|ppg|pro|psc|qsl|slg|taxi|teo|trd|vet|zlg|blog|flog|nom|vlog|wiki)\b";
            Regex regex2 = new Regex(custom);

            if (regex1.Match(email).Success && regex2.Match(email).Success)
                return true;
            else
                return false;
        }

     

        /// <summary>
        /// Valida CPF
        /// Fonte: http://www.macoratti.net/11/09/c_val1.htm
        /// </summary>
        /// <param name="cpf">CPF para ser validado</param>
        /// <returns>Retorna true caso seja um cpf válido e false caso não seja.</returns>
        public static bool IsCpf(this string cpf)
        {
            cpf = cpf.PadLeft(11, '0');

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
    }
}
