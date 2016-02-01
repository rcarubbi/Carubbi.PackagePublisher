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
        /// <typeparam name="TargetType">Tipo de destino</typeparam>
        /// <param name="instance">Instância do objeto chamador</param>
        /// <returns>Valor convertido em Tipo Nullable</returns>
        public static TargetType? To<TargetType>(this string instance)
            where TargetType : struct
        {
            instance = (instance ?? string.Empty).ToLower();
            Type tipo = typeof(TargetType);
            MethodInfo mi = tipo.GetMethod("TryParse", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string), typeof(TargetType).MakeByRefType() }, null);
            bool success = false;
            object[] args = null;
           
            if (mi == null)
            {
                return (TargetType)Enum.Parse(tipo, instance, true);
            }
            else
            {
                instance = ParseBoolean(instance, tipo);
                args = new object[] { instance, null };
                success = (bool)mi.Invoke(null, args);
            }


            if (success)
            {
                return (TargetType)args[1];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Converte o tipo de dado do objeto chamador em outro especificado e em caso de falha retorna um valor padrão informado como parametro
        /// </summary>
        /// <typeparam name="TargetType">Tipo de destino</typeparam>
        /// <param name="instance">Instância do objeto chamador</param>
        /// <param name="defaultValue">Valor Padrão em caso de falha</param>
        /// <returns>Valor convertido</returns>
        public static TargetType To<TargetType>(this string instance, TargetType defaultValue)
           where TargetType : struct
        {
            instance = (instance ?? string.Empty).ToLower();
            Type tipo = typeof(TargetType);
            MethodInfo mi = tipo.GetMethod("TryParse", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string), typeof(TargetType).MakeByRefType() }, null);

            bool success = false;
            object[] args = null;
            if (mi == null)
            {
                return (TargetType)Enum.Parse(tipo, instance, true);
            }
            else
            {

                instance = ParseBoolean(instance, tipo);
                args = new object[] { instance, null };
                success = (bool)mi.Invoke(null, args);
            }
            if (success)
            {
                return (TargetType)args[1];
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Converte expressões conhecidas como '1', 'on', 'yes', 'sucesso' em true e qualquer coisa diferente disto como false
        /// </summary>
        /// <param name="instance">Objeto Chamador</param>
        /// <param name="tipo">Tipo do dado de origem</param>
        /// <returns>Valor do retorno</returns>
        private static string ParseBoolean(string instance, Type tipo)
        {
            if (tipo == typeof(bool))
            {
                if (instance == "1") instance = "true";
                if (instance == "on") instance = "true";
                if (instance == "yes") instance = "true";
                if (instance == "sim") instance = "true";
                if (instance == "sucesso") instance = "true";
            }
            return instance;
        }

        /// <summary>
        /// Converte o tipo de dado do objeto chamador em outro especificado e em caso de falha retorna um valor padrão informado como parametro
        /// </summary>
        /// <typeparam name="TargetType">Tipo de destino</typeparam>
        /// <param name="instance">Instância do objeto chamador</param>
        /// <param name="defaultValue">Valor Padrão em caso de falha</param>
        /// <returns>Valor convertido</returns>
        public static TargetType To<TargetType>(this object instance, TargetType defaultValue)
            where TargetType : struct
        {
            string stringvalue = (instance ?? string.Empty).ToString().ToLower();
            if (string.IsNullOrEmpty(stringvalue))
                return defaultValue;

            return To<TargetType>(stringvalue, defaultValue);
        }

        /// <summary>
        /// Converte o tipo de dado do objeto chamador em outro especificado
        /// </summary>
        /// <typeparam name="TargetType">Tipo de destino</typeparam>
        /// <param name="instance">Instância do objeto chamador</param>
        /// <returns>Valor convertido em Tipo Nullable</returns>
        public static TargetType? To<TargetType>(this object instance)
        where TargetType : struct
        {

            string stringvalue = (instance ?? string.Empty).ToString().ToLower();
            if (string.IsNullOrEmpty(stringvalue))
                return null;

            return To<TargetType>(stringvalue);
        }

        /// <summary>
        /// Converte um array Bidimensional em um Jagged array
        /// </summary>
        /// <typeparam name="T">Tipo de entrada</typeparam>
        /// <param name="twoDimensionalArray">Array Bidimensional</param>
        /// <returns>Jagged Array</returns>
        public static T[][] ToJaggedArray<T>(this T[,] twoDimensionalArray)
        {
            int rowsFirstIndex = twoDimensionalArray.GetLowerBound(0);
            int rowsLastIndex = twoDimensionalArray.GetUpperBound(0);
            int numberOfRows = rowsLastIndex + 1;

            int columnsFirstIndex = twoDimensionalArray.GetLowerBound(1);
            int columnsLastIndex = twoDimensionalArray.GetUpperBound(1);
            int numberOfColumns = columnsLastIndex + 1;

            T[][] jaggedArray = new T[numberOfRows][];
            for (int i = rowsFirstIndex; i <= rowsLastIndex; i++)
            {
                jaggedArray[i] = new T[numberOfColumns];

                for (int j = columnsFirstIndex; j <= columnsLastIndex; j++)
                {
                    jaggedArray[i][j] = twoDimensionalArray[i, j];
                }
            }
            return jaggedArray;
        }
     
    }
}
