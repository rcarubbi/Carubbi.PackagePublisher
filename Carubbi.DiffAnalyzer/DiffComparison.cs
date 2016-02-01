
namespace Carubbi.DiffAnalyzer
{
    public class DiffComparison
    {
        public string PropertyName { get; set; }

        private object _oldValue;
        public object OldValue
        {
            get
            {
                return _oldValue == null ? string.Empty : _oldValue;
            }
            set
            {
                _oldValue = value;
            }
        }

        private object _newValue;
        public object NewValue
        {
            get
            {
                return _newValue == null ? string.Empty : _newValue;
            }
            set
            {
                _newValue = value;
            }
        }

        public virtual DiffState State
        {
            get
            {
                if (OldValue == null && NewValue == null)
                {
                    return DiffState.Unknow;
                }
                else if (OldValue.GetHashCode() == NewValue.GetHashCode())
                {
                    return DiffState.NotChanged;
                }
                else if (OldValue == string.Empty && NewValue != string.Empty)
                {
                    return DiffState.Added;
                }
                else if (NewValue == string.Empty && OldValue != string.Empty)
                {
                    return DiffState.Deleted;
                }
                else if (NewValue.GetHashCode() != OldValue.GetHashCode())
                {
                    return DiffState.Modified;
                }
                else
                {
                    return DiffState.Unknow;
                }
            }
        }

        public int Depth { get; set; }

        public bool LastProperty { get; set; }
    }
}
