using EasyShop.Catalog.Application.Interfaces;
using EasyShop.Catalog.Domain.Entities;
using EasyShop.Catalog.Infrastructure.Persistence;

namespace EasyShop.Catalog.Infrastructure.Repositories;

public class ProductWriteRepository : GenericRepository<Product>, IProductWriteRepository
{
    public ProductWriteRepository(CatalogDbContext context) : base(context)
    {
    }
}