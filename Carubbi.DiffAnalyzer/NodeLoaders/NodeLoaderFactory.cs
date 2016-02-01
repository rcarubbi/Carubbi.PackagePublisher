using System;
using System.Collections.Generic;

namespace Carubbi.DiffAnalyzer.NodeLoaders
{
    public class NodeLoaderFactory
    {
        private const string LIST_TYPE = "List`1";
        private const string NULLABLE_TYPE = "Nullable`1";
        private static object _locker = new object();
        private static volatile NodeLoaderFactory _instance;

        internal static NodeLoaderFactory GetInstance()
        {
            if (_instance == null)
            {
                lock (_locker)
                {
                    if (_instance == null)
                        _instance = new NodeLoaderFactory();
                }
            }
            return _instance;
        }

        public NodeLoaderBase GetLoader(string typeName, Action<Type, object, object, List<DiffComparison>> loadTypeNodeHandler)
        {
            switch (typeName)
            { 
                case LIST_TYPE:
                    return new ListNodeLoader(loadTypeNodeHandler);
                default:
                    return new DefaultNodeLoader(loadTypeNodeHandler);
            }
        }

        public NodeLoaderBase GetPrimitiveNodeLoader(string typeName, Action<Type, object, object, List<DiffComparison>> loadTypeNodeHandler)
        {
            switch (typeName)
            {
                case LIST_TYPE:
                    return new ListPrimitiveNodeLoader(loadTypeNodeHandler);
                case NULLABLE_TYPE:
                    return new NullablePrimitiveNodeLoader(loadTypeNodeHandler);

                default:
                    throw new NotSupportedException();                
            }
        }
    }
}
