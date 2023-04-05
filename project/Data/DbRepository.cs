using project.Models;

namespace project.Data;

public class DbRepository : IDbRepository
{
    private readonly AppDbContext _context;

    public DbRepository(AppDbContext context)
    {
        _context = context;
    }

    public bool Add(Order order)
    {
        _context.Add(order);
        return Save();
    }

    public bool Update(Order order)
    {
        _context.Update(order);
        return Save();
    }

    public bool Delete(Order order)
    {
        _context.Remove(order);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0;
    }
}