using EasyShop.Catalog.Domain.Entities;
namespace EasyShop.Catalog.Application.Interfaces;

public interface IProductWriteToReadService
{
    Task SyncProductToReadModelAsync(Product product);
    Task UpdateProductInReadModelAsync(Product product);
    Task DeleteProductFromReadModelAsync(Guid productId);

}
