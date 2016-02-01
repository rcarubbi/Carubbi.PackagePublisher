using System;
using System.Collections.Generic;

namespace Carubbi.DiffAnalyzer.NodeLoaders
{
    public class DefaultNodeLoader : NodeLoaderBase
    {
        public DefaultNodeLoader(Action<Type, object, object, List<DiffComparison>> loadTypeNodeHandler)
            : base(loadTypeNodeHandler) { }

        public override void LoadNode(Type type, object oldInstance, object newInstance, List<DiffComparison> comparisons)
        {
            base._loadTypeNodeHandler(type, oldInstance, newInstance, comparisons);
        }
    }
}
