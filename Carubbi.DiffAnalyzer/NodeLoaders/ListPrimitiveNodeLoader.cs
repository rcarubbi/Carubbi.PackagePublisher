using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Carubbi.DiffAnalyzer.NodeLoaders
{
    public class ListPrimitiveNodeLoader : NodeLoaderBase
    {
        public ListPrimitiveNodeLoader(Action<Type, object, object, List<DiffComparison>> loadTypeNodeHandler) :
            base(loadTypeNodeHandler) { }

        private const string COUNT_PROPERTY = "Count";

        public override void LoadNode(string propertyName, object oldInstance, object newInstance, List<DiffComparison> comparisons, int depth)
        {
            int countOldList = Convert.ToInt32(oldInstance.GetType().InvokeMember(COUNT_PROPERTY, BindingFlags.GetProperty, null, oldInstance, null));
            int countNewList = Convert.ToInt32(newInstance.GetType().InvokeMember(COUNT_PROPERTY, BindingFlags.GetProperty, null, newInstance, null));
            int smallerList = (countOldList < countNewList ? countOldList : countNewList);
            int biggerList = (countOldList > countNewList ? countOldList : countNewList);

            for (int j = 0; j < biggerList; j++)
            {
                object oldValue = (oldInstance as IList).Count > j ? (oldInstance as IList)[j] : null;
                object newValue = (newInstance as IList).Count > j ? (newInstance as IList)[j] : null;

                comparisons.Add(new DiffComparison() { PropertyName = string.Format("{0}", propertyName), OldValue = oldValue, NewValue = newValue, Depth = depth });
            }
        }

        public override void LoadNode(Type type, object oldInstance, object newInstance, List<DiffComparison> comparisons)
        {
            throw new NotSupportedException();
        }
    }
}
