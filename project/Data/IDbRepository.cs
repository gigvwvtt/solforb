using project.Models;

namespace project.Data;

public interface IDbRepository
{
    bool Add(Order order);
    bool Update(Order order);
    bool Delete(Order order);
    bool Save();
}