using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carubbi.Utils.Data
{
    /// <summary>
    /// Métodos de estensão para a interface IEnumerable<T>
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Intercala duas listas na seguinte sequencia A1, B1, A2, B2, A3, B3, ....
        /// </summary>
        /// <typeparam name="T">Tipo do item das listas</typeparam>
        /// <param name="first">Lista A</param>
        /// <param name="second">Lista B</param>
        /// <returns>Lista Intercalada</returns>
        public static IEnumerable<T> InterleaveLists<T>(this IEnumerable<T> first,  IEnumerable<T> second)
        {
            using (IEnumerator<T>
                enumerator1 = first.GetEnumerator(),
                enumerator2 = second.GetEnumerator())
            {
                while (enumerator1.MoveNext())
                {
                    yield return enumerator1.Current;
                    if (enumerator2.MoveNext())
                        yield return enumerator2.Current;
                }
            }
        }
    }
}
