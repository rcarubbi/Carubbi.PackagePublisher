using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Carubbi.Utils.Data;

namespace Carubbi.DiffAnalyzer.NodeLoaders
{
    public class ListNodeLoader : NodeLoaderBase
    {
        public ListNodeLoader(Action<Type, object, object, List<DiffComparison>> loadTypeNodeHandler)
            : base(loadTypeNodeHandler) { }

        private const string COUNT_PROPERTY = "Count";

        public override void LoadNode(Type type, object oldInstance, object newInstance, List<DiffComparison> comparisons)
        {
            int countOldList = (oldInstance != null && oldInstance != string.Empty) ? Convert.ToInt32(oldInstance.GetType().InvokeMember(COUNT_PROPERTY, BindingFlags.GetProperty, null, oldInstance, null)) : 0;
            int countNewList = (newInstance != null && newInstance != string.Empty) ? Convert.ToInt32(newInstance.GetType().InvokeMember(COUNT_PROPERTY, BindingFlags.GetProperty, null, newInstance, null)) : 0;
            int smallerList = (countOldList < countNewList ? countOldList : countNewList);
            int biggerList = (countOldList > countNewList ? countOldList : countNewList);

            List<SortClass> sortClasses = new List<SortClass>();
            IEnumerable<string> propertyKeyNames = GetPropertyKeyNames(type);
            foreach (string propertyKeyName in propertyKeyNames)
            {
                sortClasses.Add(new SortClass(propertyKeyName, SortDirection.Ascending));
            }

            if (sortClasses.Count > 0)
            {
                object typedMultipleComparer = Extensions.CreateGenericTypeByReflection(typeof(GenericMultipleComparer<>), type, new Object[] { sortClasses });

                Type listType = oldInstance.GetType();

                listType.InvokeMember("Sort",
                    BindingFlags.InvokeMethod,
                    null, oldInstance,
                    new Object[] { typedMultipleComparer });

                listType.InvokeMember("Sort",
                    BindingFlags.InvokeMethod,
                   null, newInstance,
                   new Object[] { typedMultipleComparer });

                for (int i = 0; i < smallerList; i++)
                {
                    if (countOldList <= countNewList)
                    {
                        SyncronizeLists(type, oldInstance, newInstance, propertyKeyNames, listType, i);
                    }
                    else
                    {
                        SyncronizeLists(type, newInstance, oldInstance, propertyKeyNames, listType, i);
                    }
                }

                for (int i = 0; i < biggerList; i++)
                {
                    if (countOldList <= countNewList)
                    {
                        SyncronizeLists(type, newInstance, oldInstance, propertyKeyNames, listType, i);
                    }
                    else
                    {
                        SyncronizeLists(type, oldInstance, newInstance, propertyKeyNames, listType, i);
                    }
                }
                int countSyncronizedList = (oldInstance != null && oldInstance != string.Empty) ? Convert.ToInt32(oldInstance.GetType().InvokeMember(COUNT_PROPERTY, BindingFlags.GetProperty, null, oldInstance, null)) : 0;

                for (int i = 0; i < countSyncronizedList; i++)
                {
                    _loadTypeNodeHandler(type, (oldInstance as IList)[i], (newInstance as IList)[i], comparisons);
                    comparisons.Last().LastProperty = true;
                }
            }
            else
            {
                for (int i = 0; i < smallerList; i++)
                {
                    _loadTypeNodeHandler(type, (oldInstance as IList)[i], (newInstance as IList)[i], comparisons);
                    comparisons.Last().LastProperty = true;
                }

                for (int j = smallerList; j < biggerList; j++)
                {
                    object oldValue = "";
                    object newValue = "";

                    if (oldInstance != null && oldInstance != "")
                    {
                        oldValue = (oldInstance as IList).Count > j ? (oldInstance as IList)[j] : null;
                    }

                    if (newInstance != null && newInstance != "")
                    {
                        newValue = (newInstance as IList).Count > j ? (newInstance as IList)[j] : null;
                    }
                    _loadTypeNodeHandler(type, oldValue, newValue, comparisons);
                    comparisons.Last().LastProperty = true;
                }
            }
        }

        private static void SyncronizeLists(Type type, object oldInstance, object newInstance, IEnumerable<string> propertyKeyNames, Type listType, int i)
        {
            object[] constructorParams = propertyKeyNames.ToArray();
            object typedEqualityComparer = Extensions.CreateGenericTypeByReflection(typeof(GenericEqualityComparer<>), type, constructorParams);

            MethodInfo[] mInfo = typeof(System.Linq.Enumerable).GetMethods().Where(m => m.Name == "Contains").ToArray();
            MethodInfo containsMethod = mInfo[1].MakeGenericMethod(type);

            bool exists = (bool)containsMethod.Invoke(newInstance, new Object[] { newInstance, (oldInstance as IList)[i], typedEqualityComparer });
            if (!exists)
            {
                listType.InvokeMember("Insert", BindingFlags.InvokeMethod, null, newInstance, new object[] { i, null });
            }
        }

        private IEnumerable<String> GetPropertyKeyNames(Type type)
        {
            foreach (PropertyInfo p in type.GetProperties())
            {
                object[] attributes = p.GetCustomAttributes(typeof(DiffAnalyzableListItemKeyAttribute), true);
                if (attributes.Length > 0)
                {
                    yield return p.Name;
                }
            }
        }
    }
}
