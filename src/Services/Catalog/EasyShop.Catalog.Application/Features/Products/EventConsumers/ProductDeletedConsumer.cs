using EasyShop.Catalog.Application.Interfaces;
using EasyShop.Catalog.Domain.Events;
using MassTransit;

namespace EasyShop.Catalog.Application.Features.Products.EventConsumers;

public class ProductDeletedConsumer : IConsumer<ProductDeletedEvent>
{
    private readonly IProductWriteToReadService _writeToReadService;

    public ProductDeletedConsumer(IProductWriteToReadService writeToReadService)
    {
        _writeToReadService = writeToReadService;
    }

    public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
    {
        var @event = context.Message;
        await _writeToReadService.DeleteProductFromReadModelAsync(@event.ProductId);
    }
}