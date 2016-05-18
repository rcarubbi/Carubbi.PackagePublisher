using System.Data.Entity;

namespace Carubbi.GenericRepository
{
    public interface IDbContext
    {
        IDbSet<T> Set<T>() where T : class;
    }
}
