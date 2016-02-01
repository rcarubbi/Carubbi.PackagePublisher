using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using Carubbi.Utils.Data;
using System.Collections.Generic;
namespace Carubbi.DAL
{
    /// <summary>
    /// Conversor de linha da tabela para entidade por reflexão
    /// <para>
    /// Busca o nome do parametro das procedures a partir do nome das prorpriedades da entidade 
    /// </para>
    /// </summary>
    public static class FieldReflector
    {
        /// <summary>
        /// Transforma uma linha da tabela que se encontra no data reader em uma entidade T
        /// </summary>
        /// <typeparam name="T">Entidade a ser criada</typeparam>
        /// <param name="dr">DataReader com o resultado de uma procedure</param>
        /// <returns></returns>
        public static T Reflect<T>(IDataReader dr)
        {

            T obj = Activator.CreateInstance<T>();
            var properties = typeof(T).GetProperties();

            foreach (var item in properties)
            {
                string parameterName = string.Concat(DataBaseConventions.OutputFieldnamePrefix, item.Name);
                try
                {
                    object value = null;
                    TypeConverter converter = TypeDescriptor.GetConverter(item.PropertyType);
                    if (converter.CanConvertFrom(typeof(string)))
                    {
                        value = converter.ConvertFromString(null, new CultureInfo("pt-BR"), dr[parameterName].ToString());
                    }
                    else
                    {
                        value = dr[parameterName].ToString();
                    }
                    
                    item.SetValue(obj, value, null);
                }
                catch (Exception)
                {
                    
                }
            }

            return obj;

        }


        public static TOutput Reflect<TInput, TOutput>(TInput input)
        {
            throw new NotImplementedException();
        }
         
        // Em Construção...

            /*TOutput obj = Activator.CreateInstance<TOutput>();
            Type inputType = typeof(TInput);
            Type outputType = typeof(TOutput);

            var outputProperties = outputType.GetProperties();
             

            foreach (var outputProperty in outputProperties)
            {
                
                try
                {
                    var inputProperty = inputType.GetProperty(outputProperty.Name);

                    if (inputProperty != null)
                    {
                        if (inputProperty.ReflectedType.IsGenericType)
                        {
                            ReflectList(inputProperty);
                        }
                        else
                        {
                            var value = inputProperty.GetValue(input, null);
                            outputProperty.SetValue(obj, value, null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Logger.LogException(ex);
                }
            }

            return obj;
        }

        private static void ReflectList(System.Reflection.PropertyInfo inputProperty)
        {
            foreach (var propertyTypeinterface in inputProperty.ReflectedType.GetInterfaces())
            {
                if (propertyTypeinterface.IsGenericType && propertyTypeinterface.GetGenericTypeDefinition() == typeof(IList<>))
                {
                    Type inputItemType = propertyTypeinterface.GetGenericArguments()[0];

                    break;
                }
            }
        }*/

    }
}


