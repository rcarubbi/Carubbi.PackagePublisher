using System;

namespace Carubbi.DiffAnalyzer
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class DiffAnalyzableAttribute : Attribute
    {
        private DiffAnalyzableUsage _usage;
        private string _description;

        public DiffAnalyzableAttribute()
            : this(DiffAnalyzableUsage.Mark, string.Empty) { }

        public DiffAnalyzableAttribute(string description)
            : this(DiffAnalyzableUsage.Mark, description) { }

        public DiffAnalyzableAttribute(DiffAnalyzableUsage usage)
            : this(usage, string.Empty) { }

        public DiffAnalyzableAttribute(DiffAnalyzableUsage usage, string description)
        {
            _usage = usage;
            _description = description;
        }

        public DiffAnalyzableUsage Usage
        {
            get { return _usage; }
            set { _usage = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
    }

    public enum DiffAnalyzableUsage : int
    {
        Mark,
        Ignore
    }
}
