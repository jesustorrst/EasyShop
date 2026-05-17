using EasyShop.Catalog.Application.Interfaces;
using EasyShop.Catalog.Infrastructure.Persistence;
using EasyShop.Catalog.Domain.Entities;
using MongoDB.Driver;

namespace EasyShop.Catalog.Infrastructure.Repositories;

public class GenericMongoRepository<T> where T : class
{
    private readonly IMongoCollection<T> _collection;

    public GenericMongoRepository(MongoDbContext context, string collectionName)
    {
        _collection = typeof(T).Name switch
        {
            nameof(Product) => (IMongoCollection<T>)(object)context.Products,
            nameof(Category) => (IMongoCollection<T>)(object)context.Categories,
            _ => throw new InvalidOperationException($"Collection {collectionName} no reconocida")
        };
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return (await _collection.Find(_ => true).ToListAsync()).AsReadOnly();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
        return entity;
    }


    public async Task UpdateAsync(T entity, Guid id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        await _collection.ReplaceOneAsync(filter, entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        await _collection.DeleteOneAsync(filter);
    }

}