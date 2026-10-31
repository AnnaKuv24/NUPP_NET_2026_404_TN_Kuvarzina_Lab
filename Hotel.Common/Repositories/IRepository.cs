namespace Hotel.Common.Repositories;
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetPagedAsync(int page, int amount);  
    Task AddAsync(T entity);
    Task Update(T entity);
    Task Delete(T entity);
    Task<int> SaveChangesAsync();                                
}