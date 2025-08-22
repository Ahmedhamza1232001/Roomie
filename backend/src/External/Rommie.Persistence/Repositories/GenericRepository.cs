using Rommie.Application.Abstractions;
using Rommie.Persistence.Data;

namespace Rommie.Persistence.Repositories;

public class GenericRepository<T, TKey>(MerchantDbContext context) : IGenericRepository<T, TKey> where T : class
{
    private readonly MerchantDbContext _context = context;
    public async Task<T?> GetById(TKey id)
    {
        return await _context.Set<T>().FindAsync(id);
    }
    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }
    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }
}
