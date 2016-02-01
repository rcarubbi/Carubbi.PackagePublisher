using System;

namespace Carubbi.ElasticSearch
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ElasticSearchKeyPropertyAttribute : Attribute
    {
        public ElasticSearchKeyPropertyAttribute()
        {
             
        }
    }
    
}
