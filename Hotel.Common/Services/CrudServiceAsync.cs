using Hotel.Common.Repositories;

namespace Hotel.Common.Services;

public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : class
{
    private readonly IRepository<T> _repository;

    public CrudServiceAsync(IRepository<T> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<bool> CreateAsync(T element)
    {
        if (element is null) throw new ArgumentNullException(nameof(element));

        await _repository.AddAsync(element);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<T> ReadAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity ?? throw new KeyNotFoundException($"Елемент з Id = {id} не знайдено.");
    }

    public Task<IEnumerable<T>> ReadAllAsync()
    {
        return _repository.GetAllAsync();
    }

    // ПАГІНАЦІЯ: page нумерується з 1, amount — розмір сторінки.
    public Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
    {
        if (page < 1) throw new ArgumentOutOfRangeException(nameof(page), "Сторінка нумерується з 1.");
        if (amount < 1) throw new ArgumentOutOfRangeException(nameof(amount), "Розмір сторінки має бути > 0.");

        return _repository.GetPagedAsync(page, amount);
    }

    public async Task<bool> UpdateAsync(T element)
    {
        if (element is null) throw new ArgumentNullException(nameof(element));

        await _repository.Update(element);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveAsync(T element)
    {
        if (element is null) throw new ArgumentNullException(nameof(element));

        await _repository.Delete(element);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SaveAsync()
    {
        await _repository.SaveChangesAsync();
        return true;
    }
}