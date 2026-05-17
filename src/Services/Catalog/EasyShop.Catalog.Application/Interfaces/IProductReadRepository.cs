using EasyShop.Catalog.Domain.Entities;

namespace EasyShop.Catalog.Application.Interfaces;

public interface IProductReadRepository
{
    Task<IReadOnlyList<Product>> GetAllWithCategoryAsync();
    Task<Product?> GetByIdWithCategoryAsync(Guid id);
}