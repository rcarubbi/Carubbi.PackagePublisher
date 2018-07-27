using System;
using System.Reflection;

namespace Carubbi.Utils.Data
{
    /// <summary>
    /// Classe com métodos de extensão relacionados a conversão de dados
    /// </summary>
    public static class ConversionExtensions
    {
        /// <summary>
        /// Converte o tipo de dado do objeto chamador em outro especificado
        /// </summary>
        /// <typeparam name="TTarget">Tipo de destino</typeparam>
        /// <param name="instance">Instância do objeto chamador</param>
        /// <returns>Valor convertido em Tipo Nullable</returns>
        public static TTarget? To<TTarget>(this string instance)
            where TTarget : struct
        {
            instance = (instance ?? string.Empty).ToLower();
            var targeType = typeof(TTarget);
            var mi = targeType.GetMethod("TryParse", BindingFlags.Static | BindingFlags.Public, null,
                new Type[] {typeof(string), typeof(TTarget).MakeByRefType()}, null);

            if (mi == null)
            {
                return (TTarget)Enum.Parse(targeType, instance, true);
            }

            instance = ParseBoolean(instance, targeType);

            object[] args = { instance, null };
            var success = (bool)mi.Invoke(null, args);

            if (success)
            {
                return (TTarget)args[1];
            }

            return null;
        }

        /// <summary>
        /// Converte o tipo de dado do objeto chamador em outro especificado e em caso de falha retorna um valor padrão informado como parametro
        /// </summary>
        /// <typeparam name="TTarget">Tipo de destino</typeparam>
        /// <param name="instance">Instância do objeto chamador</param>
        /// <param name="defaultValue">Valor Padrão em caso de falha</param>
        /// <returns>Valor convertido</returns>
        public static TTarget To<TTarget>(this string instance, TTarget defaultValue)
           where TTarget : struct
        {
            instance = (instance ?? string.Empty).ToLower();
            var tipo = typeof(TTarget);

            var mi = tipo.GetMethod("TryParse", BindingFlags.Static | BindingFlags.Public, null,
                new Type[] {typeof(string), typeof(TTarget).MakeByRefType()}, null);

            if (mi == null)
            {
                return (TTarget)Enum.Parse(tipo, instance, true);
            }

            instance = ParseBoolean(instance, tipo);
            var args = new object[] { instance, null };
            var success = (bool)mi.Invoke(null, args);

            if (success)
            {
                return (TTarget)args[1];
            }

            return defaultValue;
        }

        /// <summary>
        /// Converte expressões conhecidas como '1', 'on', 'yes', 'sucesso' em true e qualquer coisa diferente disto como false
        /// </summary>
        /// <param name="instance">Objeto Chamador</param>
        /// <param name="tipo">Tipo do dado de origem</param>
        /// <returns>Valor do retorno</returns>
        private static string ParseBoolean(string instance, Type tipo)
        {
            if (tipo != typeof(bool)) return instance;

            if (instance == "1") instance = "true";
            if (instance == "on") instance = "true";
            if (instance == "yes") instance = "true";
            if (instance == "sim") instance = "true";
            if (instance == "sucesso") instance = "true";
            return instance;
        }

        /// <summary>
        /// Converte o tipo de dado do objeto chamador em outro especificado e em caso de falha retorna um valor padrão informado como parametro
        /// </summary>
        /// <typeparam name="TTarget">Tipo de destino</typeparam>
        /// <param name="instance">Instância do objeto chamador</param>
        /// <param name="defaultValue">Valor Padrão em caso de falha</param>
        /// <returns>Valor convertido</returns>
        public static TTarget To<TTarget>(this object instance, TTarget defaultValue)
            where TTarget : struct
        {
            var stringvalue = (instance ?? string.Empty)
                .ToString()
                .ToLower();

            return string.IsNullOrEmpty(stringvalue) 
                ? defaultValue 
                : To(stringvalue, defaultValue);
        }

        /// <summary>
        /// Converte o tipo de dado do objeto chamador em outro especificado
        /// </summary>
        /// <typeparam name="TTarget">Tipo de destino</typeparam>
        /// <param name="instance">Instância do objeto chamador</param>
        /// <returns>Valor convertido em Tipo Nullable</returns>
        public static TTarget? To<TTarget>(this object instance)
        where TTarget : struct
        {
            var stringvalue = (instance ?? string.Empty).ToString().ToLower();
            return !string.IsNullOrEmpty(stringvalue) 
                ? To<TTarget>(stringvalue) 
                : null;
        }

        /// <summary>
        /// Converte um array Bidimensional em um Jagged array
        /// </summary>
        /// <typeparam name="T">Tipo de entrada</typeparam>
        /// <param name="twoDimensionalArray">Array Bidimensional</param>
        /// <returns>Jagged Array</returns>
        public static T[][] ToJaggedArray<T>(this T[,] twoDimensionalArray)
        {
            var rowsFirstIndex = twoDimensionalArray.GetLowerBound(0);
            var rowsLastIndex = twoDimensionalArray.GetUpperBound(0);
            var numberOfRows = rowsLastIndex + 1;

            var columnsFirstIndex = twoDimensionalArray.GetLowerBound(1);
            var columnsLastIndex = twoDimensionalArray.GetUpperBound(1);
            var numberOfColumns = columnsLastIndex + 1;

            var jaggedArray = new T[numberOfRows][];

            for (var i = rowsFirstIndex; i <= rowsLastIndex; i++)
            {
                jaggedArray[i] = new T[numberOfColumns];

                for (var j = columnsFirstIndex; j <= columnsLastIndex; j++)
                {
                    jaggedArray[i][j] = twoDimensionalArray[i, j];
                }
            }
            return jaggedArray;
        }
     
    }
}
