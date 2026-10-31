using System.Collections;
using System.Collections.Concurrent;
using System.Text.Json;
using Hotel.Common.Entities;

namespace Hotel.Common.Services;

public class InMemoryCrudServiceAsync<T> : ICrudServiceAsync<T>, IEnumerable<T> where T : class, IEntity
{
    private readonly ConcurrentDictionary<Guid, T> _storage = new();

    private readonly SemaphoreSlim _fileLock = new(1, 1);

    private readonly string _filePath;

    public InMemoryCrudServiceAsync(string filePath)
    {
        _filePath = filePath;
    }

    public Task<bool> CreateAsync(T element)
    {
        if (element is null) throw new ArgumentNullException(nameof(element));
        var added = _storage.TryAdd(element.Id, element);
        return Task.FromResult(added);
    }

    public Task<T> ReadAsync(Guid id)
    {
        if (_storage.TryGetValue(id, out var element))
            return Task.FromResult(element);

        throw new KeyNotFoundException($"Елемент з Id = {id} не знайдено.");
    }

    public Task<IEnumerable<T>> ReadAllAsync()
    {
        return Task.FromResult<IEnumerable<T>>(_storage.Values.ToList());
    }

    // ПАГІНАЦІЯ: page — номер сторінки (з 1), amount — розмір сторінки.
    public Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
    {
        if (page < 1) throw new ArgumentOutOfRangeException(nameof(page), "Сторінка нумерується з 1.");
        if (amount < 1) throw new ArgumentOutOfRangeException(nameof(amount), "Розмір сторінки має бути > 0.");

        var pageItems = _storage.Values
            .Skip((page - 1) * amount)
            .Take(amount)
            .ToList();

        return Task.FromResult<IEnumerable<T>>(pageItems);
    }

    public Task<bool> UpdateAsync(T element)
    {
        if (element is null) throw new ArgumentNullException(nameof(element));

        if (!_storage.ContainsKey(element.Id))
            return Task.FromResult(false);

        _storage[element.Id] = element;
        return Task.FromResult(true);
    }

    public Task<bool> RemoveAsync(T element)
    {
        if (element is null) throw new ArgumentNullException(nameof(element));
        var removed = _storage.TryRemove(element.Id, out _);
        return Task.FromResult(removed);
    }

    // Асинхронне збереження колекції у файл у форматі JSON.
    public async Task<bool> SaveAsync()
    {
        await _fileLock.WaitAsync();
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(_storage.Values, options);
            await File.WriteAllTextAsync(_filePath, json);
            return true;
        }
        finally
        {
            _fileLock.Release(); 
        }
    }

    public async Task<bool> LoadAsync()
    {
        if (!File.Exists(_filePath))
            return false;

        await _fileLock.WaitAsync();
        try
        {
            var json = await File.ReadAllTextAsync(_filePath);
            var items = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();

            _storage.Clear();
            foreach (var item in items)
                _storage.TryAdd(item.Id, item);

            return true;
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public IEnumerator<T> GetEnumerator() => _storage.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}