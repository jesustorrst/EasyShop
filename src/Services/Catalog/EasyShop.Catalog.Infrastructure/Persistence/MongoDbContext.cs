using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using EasyShop.Catalog.Infrastructure.Settings;
using EasyShop.Catalog.Domain.Entities;

namespace EasyShop.Catalog.Infrastructure.Persistence;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    static MongoDbContext()
    {
        BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(GuidRepresentation.Standard));
    }

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {

        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
    public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");
}
