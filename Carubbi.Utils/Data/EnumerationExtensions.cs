using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Carubbi.Utils.Data
{
    /// <summary>
    /// Métodos de extensão para Enums
    /// </summary>
    public static class EnumerationExtensions
    {
        /// <summary>
        /// Recupera o texto de um item de um Enum configurado no Atributo Description do namespace System.ComponentModel
        /// </summary>
        /// <typeparam name="T">Tipo do Enum</typeparam>
        /// <param name="item">Item do Enum</param>
        /// <returns>Texto da Description</returns>
        public static string Text<T>(this T item)
        {
            var memInfo = item.GetType().GetMember(item.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute),false);
            var description = ((DescriptionAttribute)attributes[0]).Description;
            return description;
        }


        /// <summary>
        /// Converte um Enum em List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this System.Enum type)
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();

        }

        /// <summary>
        /// Verifica se uma variável contém um determinado item do Enum nela
        /// </summary>
        /// <typeparam name="T">Tipo do Enum</typeparam>
        /// <param name="type">Tipo do Enum</param>
        /// <param name="value">Item do Enum</param>
        /// <returns>Indicador se possui ou não</returns>
        public static bool Has<T>(this System.Enum type, T value)
        {
            try
            {
                return (((int)(object)type & (int)(object)value) == (int)(object)value);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Verifica se uma variável é um determinado item do Enum nela
        /// </summary>
        /// <typeparam name="T">Tipo do Enum</typeparam>
        /// <param name="type">Tipo do Enum</param>
        /// <param name="value">Item do Enum</param>
        /// <returns>Indicador se possui ou não</returns>
        public static bool Is<T>(this System.Enum type, T value)
        {
            try
            {
                return (int)(object)type == (int)(object)value;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Retorna os valores numéricos dos itens de um Enum em uma string separados por virgula
        /// </summary>
        /// <param name="type">Tipo do Enum</param>
        /// <returns>String com os valores separados por vírgula</returns>
        public static string Ids(this System.Enum type)
        {
            return string.Join(",", type.ToList<int>().ConvertAll<string>(new Converter<int,string>(i => i.ToString())).ToArray());
        }

        /// <summary>
        /// Adiciona um item em uma variável do tipo Enum
        /// </summary>
        /// <typeparam name="T">Tipo do Enum</typeparam>
        /// <param name="type">Tipo do Enum</param>
        /// <param name="value">Item do Enum</param>
        /// <returns>O Proprio Valor do Enum</returns>
        public static T Add<T>(this System.Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type | (int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    string.Format(
                        "Could not append value from enumerated type '{0}'.",
                        typeof(T).Name
                        ), ex);
            }
        }

        /// <summary>
        /// Remove um item em uma variável do tipo Enum
        /// </summary>
        /// <typeparam name="T">Tipo do Enum</typeparam>
        /// <param name="type">Tipo do Enum</param>
        /// <param name="value">Item do Enum</param>
        /// <returns>O Proprio Valor do Enum</returns>
        public static T Remove<T>(this System.Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type & ~(int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    string.Format(
                        "Could not remove value from enumerated type '{0}'.",
                        typeof(T).Name
                        ), ex);
            }
        }


        /// <summary>
        /// Converte um Enum em uma lista de KeyValuePair<int, string>
        /// </summary>
        /// <typeparam name="T">Tipo do Enum</typeparam>
        /// <param name="instance">Valor do Enum a ser convertido</param>
        /// <returns>Lista de KeyValuePair com os valores dos enums e suas Descriptions<</returns>
        public static List<KeyValuePair<int, string>> ToDataSource<T>(this System.Enum instance)
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Select(item =>
                new KeyValuePair<int, string>(Convert.ToInt32(item), item.Text())).OrderBy(item => item.Key).ToList();
        }

    }
}
