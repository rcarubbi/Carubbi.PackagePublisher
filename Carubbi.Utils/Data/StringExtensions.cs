using System;

namespace Carubbi.Utils.Data
{
    /// <summary>
    /// Extension Methods para a classe String
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Retorna N caracteres a esquerda de um determinado texto
        /// </summary>
        /// <param name="value">Texto</param>
        /// <param name="maxLength">Quantidade de Caracteres a Esquerda</param>
        /// <returns>Trecho do texto selecionado</returns>
        public static string Left(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return (value.Length <= maxLength
                   ? value
                   : value.Substring(0, maxLength)
                   );
        }

        /// <summary>
        /// Converte uma string em um array de linhas quebrando pelo escape \n
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string[] ToLineArray(this string value)
        {
            return value.Split(new string[] { "\n" }, StringSplitOptions.None);
        }
    }
}
