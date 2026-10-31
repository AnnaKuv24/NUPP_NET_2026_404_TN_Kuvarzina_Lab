using Hotel.Common.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Infrastructure.Repositories;
public class Repository<T> : IRepository<T> where T : class
{
    private readonly HotelContext _context;
    private readonly DbSet<T> _set;

    public Repository(HotelContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _set = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _set.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _set.ToListAsync();
    }

    public async Task<IEnumerable<T>> GetPagedAsync(int page, int amount)
    {
        return await _set
            .OrderBy(e => EF.Property<Guid>(e, "Id"))
            .Skip((page - 1) * amount)
            .Take(amount)
            .ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _set.AddAsync(entity);
    }

    public Task Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public Task Delete(T entity)
    {
        _set.Remove(entity);
        return Task.CompletedTask;
    }

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}