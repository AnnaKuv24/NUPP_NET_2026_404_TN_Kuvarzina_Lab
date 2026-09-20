using System.Text.Json;
using Hotel.Common.Entities;

namespace Hotel.Common.Services;

public class CrudService<T> : ICrudService<T> where T : class, IEntity
{
    private readonly Dictionary<Guid, T> _storage = new();

    public void Create(T element)
    {
        if (element is null) throw new ArgumentNullException(nameof(element));
        _storage[element.Id] = element;
    }

    public T Read(Guid id)
    {
        if (_storage.TryGetValue(id, out var element))
            return element;

        throw new KeyNotFoundException($"Елемент з Id = {id} не знайдено.");
    }

    public IEnumerable<T> ReadAll()
    {
        return _storage.Values;
    }

    public void Update(T element)
    {
        if (element is null) throw new ArgumentNullException(nameof(element));
        if (!_storage.ContainsKey(element.Id))
            throw new KeyNotFoundException($"Елемент з Id = {element.Id} не знайдено, оновлення неможливе.");

        _storage[element.Id] = element;
    }

    public void Remove(T element)
    {
        if (element is null) throw new ArgumentNullException(nameof(element));
        _storage.Remove(element.Id);
    }

    public void Save(string filePath)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(_storage.Values, options);
        File.WriteAllText(filePath, json);
    }

    public void Load(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Файл для завантаження не знайдено.", filePath);

        var json = File.ReadAllText(filePath);
        var items = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();

        _storage.Clear();
        foreach (var item in items)
        {
            _storage[item.Id] = item;
        }
    }
}
