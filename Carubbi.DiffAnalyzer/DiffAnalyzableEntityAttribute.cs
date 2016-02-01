using System;

namespace Carubbi.DiffAnalyzer
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DiffAnalyzableEntityAttribute : Attribute
    {
        private string _description;

        public DiffAnalyzableEntityAttribute()
            : this(string.Empty) { }

        public DiffAnalyzableEntityAttribute(string description)
        {
            _description = description;
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
    }
}
