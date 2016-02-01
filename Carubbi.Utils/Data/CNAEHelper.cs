using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carubbi.Utils.Data
{
    /// <summary>
    ///  Classe com métodos para tratamento de Códigos CNAE
    /// </summary>
    public class CNAEHelper
    {
        /// <summary>
        /// Separa a porção númerica da porção alfabética de um código CNAE
        /// <example>
        ///     <code>
        ///        var par = CNAEHelper.LerCnae("A1234")
        ///        par.Key // "1234"
        ///        par.Value // "A"
        ///     </code>
        /// </example>
        /// </summary>
        /// <param name="cnae">Código CNAE a ser analisado</param>
        /// <returns>Par Chave-Valor onde a chave é a porção numérica e o valor a porção alfabética</returns>
        public static KeyValuePair<long, string> LerCnae(string cnae)
        {
            cnae = (cnae ?? string.Empty).Replace("-", string.Empty).Replace(".", string.Empty).Replace(" ", string.Empty);
            string letra = string.Empty;
            long codigo;
            if (Char.IsLetter(cnae[0]))
            {
                letra = cnae[0].ToString();
                codigo = cnae.Substring(1, cnae.Length - 1).To<long>(0);
            }
            else
            {
                codigo = cnae.To<long>(0);
            }

            return new KeyValuePair<long, string>(codigo, letra);
        }
    }
}
