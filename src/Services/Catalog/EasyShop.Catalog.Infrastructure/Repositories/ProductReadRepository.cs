using EasyShop.Catalog.Application.DTOs;
using EasyShop.Catalog.Application.Interfaces;
using EasyShop.Catalog.Domain.Entities;
using EasyShop.Catalog.Infrastructure.Persistence;
using MongoDB.Driver;

namespace EasyShop.Catalog.Infrastructure.Repositories;

public class ProductReadRepository : IProductReadRepository
{
    private readonly IMongoCollection<Product> _collection;

    public ProductReadRepository(MongoDbContext context)
    {
        _collection = context.Products;
    }

    public async Task<IReadOnlyList<Product>> GetAllWithCategoryAsync()
    {
        return (await _collection.Find(_ => true).ToListAsync()).AsReadOnly();
    }

    public async Task<Product?> GetByIdWithCategoryAsync(Guid id)
    {
        var filter = Builders<Product>.Filter.Eq("_id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

}