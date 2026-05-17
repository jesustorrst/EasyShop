using EasyShop.Catalog.Application.Interfaces;
using EasyShop.Catalog.Domain.Entities;
using EasyShop.Catalog.Infrastructure.Persistence;
using MongoDB.Driver;

namespace EasyShop.Catalog.Infrastructure.Services;

public class ProductWriteToReadService : IProductWriteToReadService
{
    private readonly MongoDbContext _mongoDbContext;

    public ProductWriteToReadService(MongoDbContext mongoDbContext)
    {
        _mongoDbContext = mongoDbContext;
    }

    public async Task SyncProductToReadModelAsync(Product product)
    {
        await _mongoDbContext.Products.InsertOneAsync(product);
    }

    public async Task UpdateProductInReadModelAsync(Product product)
    {
        var filter = Builders<Product>.Filter.Eq("_id", product.Id);
        await _mongoDbContext.Products.ReplaceOneAsync(filter, product);
    }

    public async Task DeleteProductFromReadModelAsync(Guid productId)
    {
        var filter = Builders<Product>.Filter.Eq("_id", productId);
        await _mongoDbContext.Products.DeleteOneAsync(filter);
    }

}
