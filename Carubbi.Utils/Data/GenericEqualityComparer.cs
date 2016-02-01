using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Carubbi.Utils.Data
{
    /// <summary>
    /// Classe que compara igualdade entre objetos por reflexão a partir de propriedades configuradas no construtor ou no setter PropriedadesComparadas
    /// <para>Utilizado em métodos de coleção de objetos complexos que requerem uma implementação de IEqualityComparer<T> como por exemplo Contains()</para>
    /// <example>GenericComparer<Tabela> comparador = new GenericComparer<Tabela>("Propriedade1", "Propriedade2", ...); </example>
    /// </summary>
    /// <typeparam name="T">Tipo da classe a ser comparada</typeparam>
    public class GenericEqualityComparer<T> : IEqualityComparer<T>
    {


        /// <summary>
        /// Getter e Setter do array com os nomes das propriedades a serem comparadas
        /// </summary>
        public string[] PropriedadesComparadas { get; set; }

        /// <summary>
        /// Constroi um Objeto informando por quais propriedades devem ser feita a comparação
        /// </summary>
        /// <param name="propriedadesComparadas"></param>
        public GenericEqualityComparer(params string[] propriedadesComparadas)
        {
            this.PropriedadesComparadas = propriedadesComparadas;
        }

        /// <summary>
        /// Construtor Padrão
        /// </summary>
        public GenericEqualityComparer()
        {
        }

        /// <summary>
        /// Verifica a igualdade entre dois objetos por reflexão a partir das propriedades configuradas no construtor ou no setter PropriedadesComparadas
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(T x, T y)
         {
            IList<PropertyInfo> propriedades = this.ObterPropriedades();

            bool result = false;

            foreach (PropertyInfo propriedade in propriedades)
            {
                if (x == null || y == null)
                    return true;
                else if (x == null)
                    return false;
                else if (y == null)
                    return false;


                result = (propriedade.GetValue(x, null).GetHashCode() == propriedade.GetValue(y, null).GetHashCode());

                if (!result)
                    break;
            }

            return result;
        }

        /// <summary>
        /// Recupera o HashCode das propriedades configuradas no construtor ou no setter PropriedadesComparadas
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(T obj)
        {
            IList<PropertyInfo> propriedades = this.ObterPropriedades();

            StringBuilder stbHashCodes = new StringBuilder();

            for (int i = 0; i < PropriedadesComparadas.Length; i++)
                stbHashCodes.Append(propriedades[i].GetValue(obj, null).GetHashCode().ToString());

            return stbHashCodes.ToString().GetHashCode();
        }

        /// <summary>
        /// Recupera os metadados das propriedades configuradas no construtor ou no setter PropriedadesComparadas
        /// </summary>
        /// <returns></returns>
        private IList<PropertyInfo> ObterPropriedades()
        {
            Type type = typeof(T);

            IList<PropertyInfo> propriedades = new List<PropertyInfo>();
            foreach (string nome in PropriedadesComparadas)
            {
                PropertyInfo property = type.GetProperty(nome.Trim());

                if (property == null)
                    throw new Exception("Propriedade não existente");

                propriedades.Add(property);
            }

            return propriedades;
        }

        /// <summary>
        /// Compara dois objetos por reflexão a partir das propriedades configuradas no construtor ou no setter PropriedadesComparadas
        /// </summary>
        /// <param name="Object1">Primeiro Objeto</param>
        /// <param name="object2">Segundo Objeto</param>
        /// <returns>Verdadeiro se os objetos forem iguais e falso se forem diferentes</returns>
        public bool Compare(T Object1, T object2)
        {
            //Get the type of the object
            Type type = typeof(T);

            //return false if any of the object is false
            if (Object1 == null || object2 == null)
                return false;

            //Loop through each properties inside class and get values for the property from both the objects and compare
            foreach (System.Reflection.PropertyInfo property in type.GetProperties())
            {
                if (property.Name != "ExtensionData")
                {
                    string Object1Value = string.Empty;
                    string Object2Value = string.Empty;

                    if (type.GetProperty(property.Name).GetValue(Object1, null) != null)
                        Object1Value = type.GetProperty(property.Name).GetValue(Object1, null).ToString();

                    if (type.GetProperty(property.Name).GetValue(object2, null) != null)
                        Object2Value = type.GetProperty(property.Name).GetValue(object2, null).ToString();

                    if (property.PropertyType.Name.Equals("DateTime"))
                    {
                        if (!DateTime.Parse(Object1Value.Trim()).ToString("dd/MM/yyyy hh:mm:ss").Equals(DateTime.Parse(Object2Value.Trim()).ToString("dd/MM/yyyy hh:mm:ss")))
                        {
                            return false;
                        }
                    }
                    else if (Object1Value.Trim() != Object2Value.Trim())
                    {
                        return false;
                    }



                }
            }
            return true;
        }
    }

}



