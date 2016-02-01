using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carubbi.Utils.Data
{
    /// <summary>
    /// Extension methods para a classe Object
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Definir o valor de uma propriedade por reflexão
        /// </summary>
        /// <param name="instance">Objeto a ser afetado</param>
        /// <param name="propertyName">Nome da propriedade</param>
        /// <param name="value">Novo Valor</param>
        public static void SetProperty(this object instance, string propertyName, object value)
        {
            var property = instance.GetType().GetProperty(propertyName);

            if (property != null)
            {
                property.SetValue(instance, value, null);
            }
        }

        /// <summary>
        /// Recuperar o valor de uma propriedade por reflexão
        /// </summary>
        /// <typeparam name="T">Tipo do valor da propriedade</typeparam>
        /// <param name="instance">Objeto a ser afetado</param>
        /// <param name="propertyName">Nome da Propriedade</param>
        /// <returns>Valor da propriedade</returns>
        public static T GetProperty<T>(this object instance, string propertyName)
        {
            var property = instance.GetType().GetProperty(propertyName);
            if (property != null)
            {
                return (T)property.GetValue(instance, null);
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// Chama um método que retorna valor por reflexão
        /// </summary>
        /// <typeparam name="T">Tipo de saida do método</typeparam>
        /// <param name="instance">Objeto a ser afetado</param>
        /// <param name="methodName">Nome do método</param>
        /// <param name="parameters">Lista de Parâmetros</param>
        /// <returns>Valor de retorno</returns>
        public static T Call<T>(this object instance, string methodName, params object[] parameters)
        {
            var typeArray = new List<Type>();

            foreach (object parameter in parameters)
            {
                typeArray.Add(parameter.GetType());
            }

            var method = instance.GetType().GetMethod(methodName, typeArray.ToArray());
            if (method != null)
            {
                return (T)method.Invoke(instance, parameters);
            }
            else
                return default(T);

        }

        /// <summary>
        /// Chama um método sem retorno por reflexão
        /// </summary>
        /// <param name="instance">Objeto a ser afetado</param>
        /// <param name="methodName">Nome do método</param>
        /// <param name="parameters">Lista de parâmetros</param>
        public static void Call(this object instance, string methodName, params object[] parameters)
        {
            var typeArray =new List<Type>();

            foreach(object parameter in parameters)
            {
                typeArray.Add(parameter.GetType());
            }

            var method = instance.GetType().GetMethod(methodName, typeArray.ToArray());
            if (method != null)
            {
                method.Invoke(instance, parameters);
            }
        }
    }
}
