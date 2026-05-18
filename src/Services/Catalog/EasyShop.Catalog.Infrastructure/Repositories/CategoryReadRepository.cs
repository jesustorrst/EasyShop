using EasyShop.Catalog.Application.DTOs;
using EasyShop.Catalog.Application.Interfaces;
using EasyShop.Catalog.Domain.Entities;
using EasyShop.Catalog.Infrastructure.Persistence;
using MongoDB.Driver;

namespace EasyShop.Catalog.Infrastructure.Repositories;

public class CategoryReadRepository : ICategoryReadRepository
{
    private readonly IMongoCollection<Category> _collection;

    public CategoryReadRepository(MongoDbContext context)
    {
        _collection = context.Categories;
    }

    public async Task<IReadOnlyList<Category>> GetAllAsync()
    {
        return (await _collection.Find(_ => true).ToListAsync()).AsReadOnly();
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        var filter = Builders<Category>.Filter.Eq("_id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

}