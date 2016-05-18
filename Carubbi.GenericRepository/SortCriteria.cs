using System;
using System.Linq;

namespace Carubbi.GenericRepository
{
    public interface ISortCriteria<T>
    {
        SortDirection Direction { get; set; }

        IQueryable<T> ApplyOrdering(IQueryable<T> query, Boolean useThenBy);
    }
}
