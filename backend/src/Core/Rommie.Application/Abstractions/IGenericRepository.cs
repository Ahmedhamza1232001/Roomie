namespace Rommie.Application.Abstractions;

public interface IGenericRepository<T, TKey> where T : class
{
    Task<T?> GetById(TKey id);
    void Add(T entity);
    void Remove(T entity);
    void Update(T entity);
}
