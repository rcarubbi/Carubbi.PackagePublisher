using System.Collections.Generic;

namespace Carubbi.GenericRepository
{
    public interface IPagedListResult<out TEntity>
    {
        bool HasNext { get; set; }

        //-----------------------------------------------------------
        /// <summary>
        /// Does the returned result contains previous items ?
        /// </summary>
        bool HasPrevious { get; set; }

        //-----------------------------------------------------------
        /// <summary>
        /// Total number of rows that could be possibly be retrieved.
        /// </summary>
        int Count { get; set; }

        //-----------------------------------------------------------
        /// <summary>
        /// Result of the query.
        /// </summary>
        IEnumerable<TEntity> Entities { get; }
    }
}