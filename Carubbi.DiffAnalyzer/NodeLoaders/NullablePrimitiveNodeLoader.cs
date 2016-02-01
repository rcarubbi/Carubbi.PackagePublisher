using System;
using System.Collections.Generic;
using System.Reflection;

namespace Carubbi.DiffAnalyzer.NodeLoaders
{
    public class NullablePrimitiveNodeLoader : NodeLoaderBase
    {
        public NullablePrimitiveNodeLoader(Action<Type, object, object, List<DiffComparison>> loadTypeNodeHandler)
            : base(loadTypeNodeHandler) { }

        public override void LoadNode(Type type, object oldInstance, object newInstance, List<DiffComparison> comparisons)
        {
            throw new NotSupportedException();
        }

        private const string VALUE_PROPERTY = "Value";
        private const string HAS_VALUE_PROPERTY = "HasValue";

        public override void LoadNode(string propertyName, object oldInstance, object newInstance, List<DiffComparison> comparisons, int depth)
        {
            object oldValue = string.Empty, newValue = string.Empty;

            if (oldInstance != null && oldInstance != string.Empty && !oldInstance.GetType().IsPrimitive)
            {
                bool oldHasValue = Convert.ToBoolean(newInstance.GetType().InvokeMember(HAS_VALUE_PROPERTY, BindingFlags.GetProperty, null, oldInstance, null));

                if (oldHasValue)
                    oldValue = oldInstance.GetType().InvokeMember(VALUE_PROPERTY, BindingFlags.GetProperty, null, oldInstance, null);
            }
            else if(oldInstance != null)
            {
                oldValue = oldInstance.ToString();
            }

            if (newInstance != null && newInstance != string.Empty && !newInstance.GetType().IsPrimitive)
            {
                bool newHasValue = Convert.ToBoolean(newInstance.GetType().InvokeMember(HAS_VALUE_PROPERTY, BindingFlags.GetProperty, null, newInstance, null));

                if (newHasValue)
                    newValue = newInstance.GetType().InvokeMember(VALUE_PROPERTY, BindingFlags.GetProperty, null, newInstance, null);
            }
            else if (newInstance != null)
            {
                newValue = newInstance.ToString();
            }

            comparisons.Add(new DiffComparison() { PropertyName = string.Format("{0}", propertyName), OldValue = oldValue, NewValue = newValue, Depth = depth });
        }
    }
}
