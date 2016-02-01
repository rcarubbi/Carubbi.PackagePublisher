using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carubbi.Utils.Persistence
{
    /// <summary>
    /// Atributo utilizado para marcar Propriedades que não devem ser mapeadas pelo framework de persistência Carubbi.DAL
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class CampoNaoPersistivelAttribute : Attribute
    {

    }
 
}
