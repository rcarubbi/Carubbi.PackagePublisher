using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carubbi.Utils.Data
{
    /// <summary>
    /// Biblioteca de Funções Matemáticas
    /// </summary>
    public class MathHelper
    {
        public static double CeilingWithPlaces(double input, int places)
        {
            double scale = Math.Pow(10, places);
            double multiplied = input * scale;
            double ceiling = Math.Ceiling(multiplied);
            return ceiling / scale;
        }
    }
}
