using Hotel.Common.Entities;
using Hotel.Common.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;

namespace Hotel.Nosql.Repositories;
public class MongoRepository<T> : IRepository<T> where T : class, IEntity
{
    private readonly IMongoCollection<T> _collection;

    public MongoRepository(IMongoDatabase database, string collectionName)
    {
        _collection = database.GetCollection<T>(collectionName);
    }

    private static FilterDefinition<T> ById(Guid id) => new BsonDocument("_id", id.ToString());

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _collection.Find(ById(id)).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _collection.Find(FilterDefinition<T>.Empty).ToListAsync();
    }

    public async Task<IEnumerable<T>> GetPagedAsync(int page, int amount)
    {
        return await _collection
            .Find(FilterDefinition<T>.Empty)
            .Sort(Builders<T>.Sort.Ascending("_id"))
            .Skip((page - 1) * amount)
            .Limit(amount)
            .ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public async Task Update(T entity)
    {
        await _collection.ReplaceOneAsync(ById(entity.Id), entity);
    }

    public async Task Delete(T entity)
    {
        await _collection.DeleteOneAsync(ById(entity.Id));
    }

    public Task<int> SaveChangesAsync()
    {
        return Task.FromResult(0);
    }
}