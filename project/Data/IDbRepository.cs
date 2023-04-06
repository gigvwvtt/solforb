using System.Linq.Expressions;

namespace project.Data;

public interface IDbRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<T?> GetById(object id);
    IEnumerable<T> GetByXWithInclude(Func<T, bool> predicate, 
        params Expression<Func<T, object>>[] includeProperties);
    bool Add(T obj);
    bool Update(T obj);
    bool Delete(T obj);
    bool Save();
}