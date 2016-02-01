using System;

namespace Carubbi.Utils.Persistence
{
    /// <summary>
    /// Atributo para marcar propriedades como Campos Chave, utilizada junto com o framework de persistencia Carubbi.DAL
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class CampoChaveAttribute : Attribute
    {
        public CampoChaveAttribute()
        {

        }
    }
}
