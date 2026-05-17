using EasyShop.Catalog.Application.DTOs;
using EasyShop.Catalog.Application.Interfaces;
using EasyShop.Catalog.Domain.Entities;
using EasyShop.Catalog.Domain.Events;
using MassTransit;
using MediatR;

namespace EasyShop.Catalog.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductDto?>
{
    private readonly IProductWriteRepository _writeRepository;
    private readonly IProductReadRepository _readRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateProductHandler(
        IProductWriteRepository writeRepository,
        IProductReadRepository readRepository,
        IPublishEndpoint publishEndpoint)
    {
        _writeRepository = writeRepository;
        _readRepository = readRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<ProductDto?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _writeRepository.GetByIdAsync(request.Id);

        if (product == null)
            return null;

        product.Name = request.ProductDto.Name;
        product.Description = request.ProductDto.Description;
        product.Price = request.ProductDto.Price;
        product.CategoryId = request.ProductDto.CategoryId;
        product.UpdatedAt = DateTime.UtcNow;

        await _writeRepository.UpdateAsync(product, request.Id);

        var @event = new ProductUpdatedEvent
        {
            ProductId = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId
        };

        await _publishEndpoint.Publish(@event, cancellationToken);

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId
        };
    }
}