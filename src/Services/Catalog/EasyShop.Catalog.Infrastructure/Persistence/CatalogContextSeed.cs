using EasyShop.Catalog.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EasyShop.Catalog.Infrastructure.Persistence;

public class CatalogContextSeed
{
    public static async Task SeedAsync(CatalogDbContext context, ILogger<CatalogContextSeed> logger)
    {
        if (!context.Categories.Any())
        {
            var categories = new List<Category>
            {
                new Category { Id = Guid.NewGuid(), Name = "Electronics", Description = "Laptops, Smartphones, iPads and more" },
                new Category { Id = Guid.NewGuid(), Name = "Home & Kitchen", Description = "Appliances and decoration" }
            };

            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();
            logger.LogInformation("Se han insertado las categorías base.");
        }

        if (!context.Products.Any())
        {
            var electronicsId = context.Categories.First(c => c.Name == "Electronics").Id;

            context.Products.AddRange(new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "MacBook Air M2",
                    Description = "8GB RAM, 256GB SSD",
                    Price = 1199.99m,
                    CategoryId = electronicsId,
                    CreatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "iPhone 17 Pro",
                    Description = "Titanium Gray, 256GB",
                    Price = 999.00m,
                    CategoryId = electronicsId,
                    CreatedAt = DateTime.UtcNow
                }
            });

            await context.SaveChangesAsync();
            logger.LogInformation("Se han insertado los productos de prueba.");
        }
    }
}