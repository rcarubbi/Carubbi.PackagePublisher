using System;

namespace Carubbi.DiffAnalyzer
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class DiffAnalyzableListItemKeyAttribute : Attribute
    {
        public DiffAnalyzableListItemKeyAttribute() { }
    }
}
