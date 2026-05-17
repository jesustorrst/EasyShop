using EasyShop.Catalog.Application.Interfaces;
using EasyShop.Catalog.Domain.Events;
using EasyShop.Catalog.Domain.Entities;
using MassTransit;

namespace EasyShop.Catalog.Application.Features.Products.EventConsumers;

public class ProductUpdatedConsumer : IConsumer<ProductUpdatedEvent>
{
    private readonly IProductWriteToReadService _writeToReadService;

    public ProductUpdatedConsumer(IProductWriteToReadService writeToReadService)
    {
        _writeToReadService = writeToReadService;
    }

    public async Task Consume(ConsumeContext<ProductUpdatedEvent> context)
    {
        var @event = context.Message;

        var product = new Product
        {
            Id = @event.ProductId,
            Name = @event.Name,
            Description = @event.Description,
            Price = @event.Price,
            CategoryId = @event.CategoryId,
            UpdatedAt = DateTime.UtcNow
        };

        await _writeToReadService.UpdateProductInReadModelAsync(product);
    }
}