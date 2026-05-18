using EasyShop.Catalog.Domain.Entities;

namespace EasyShop.Catalog.Application.Interfaces;

public interface ICategoryReadRepository
{
    Task<IReadOnlyList<Category>> GetAllAsync();
}