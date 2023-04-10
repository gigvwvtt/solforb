using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace project.Data;

public class DbRepository<T> : IDbRepository<T> where T : class
{
    private readonly AppDbContext _context;
    private DbSet<T> _dbSet;

    public DbRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetById(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public IEnumerable<T> GetWithFilter(params Func<T, bool>[] predicate)
    {
        var query = _dbSet.AsNoTracking().AsEnumerable();
        return predicate.Aggregate(query, (current, f) => current.Where(f));
    }

    public IEnumerable<T> GetWithFilterWithInclude(Func<T, bool> predicate,
        params Expression<Func<T, object>>[] includeProperties)
    {
        var query = Include(includeProperties);
        return query.Where(predicate).ToList();
    }

    public IEnumerable<T> GetAllWithInclude(params Expression<Func<T, object>>[] includeProperties)
    {
        return Include(includeProperties).ToList();
    }

    private IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties)
    {
        var query = _dbSet.AsNoTracking();
        return includeProperties
            .Aggregate(query, (current, includeProperty) =>
                current.Include(includeProperty));
    }

    public async Task<IEnumerable<TT>> GetDistinctTs<TT>(Expression<Func<T, TT>> select)
    {
        return await _dbSet.Select(select).Distinct().ToListAsync();
    }

    public bool Add(T obj)
    {
        _dbSet.Add(obj);
        return Save();
    }

    public bool Update(T obj)
    {
        _dbSet.Update(obj);
        return Save();
    }

    public bool Delete(T obj)
    {
        _dbSet.Remove(obj);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0;
    }
}