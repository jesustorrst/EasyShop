using EasyShop.Catalog.Application.Interfaces;
using EasyShop.Catalog.Domain.Events;
using EasyShop.Catalog.Domain.Entities;
using MassTransit;

namespace EasyShop.Catalog.Application.Features.Products.EventConsumers;

public class ProductCreatedConsumer : IConsumer<ProductCreatedEvent>
{
    private readonly IProductWriteToReadService _writeToReadService;

    public ProductCreatedConsumer(IProductWriteToReadService writeToReadService)
    {
        _writeToReadService = writeToReadService;
    }

    public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
    {
        var @event = context.Message;

        var product = new Product
        {
            Id = @event.ProductId,
            Name = @event.Name,
            Description = @event.Description,
            Price = @event.Price,
            CategoryId = @event.CategoryId,
            CreatedAt = DateTime.UtcNow
        };

        await _writeToReadService.SyncProductToReadModelAsync(product);
    }
}
