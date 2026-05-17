using EasyShop.Catalog.Application.DTOs;
using EasyShop.Catalog.Application.Interfaces;
using EasyShop.Catalog.Domain.Entities;
using MediatR;
using MassTransit;
using EasyShop.Catalog.Domain.Events;

namespace EasyShop.Catalog.Application.Features.Products.Commands.CreateProduct;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductWriteRepository _writeRepository;
    private readonly IPublishEndpoint _publishEndpoint;


    public CreateProductHandler(IProductWriteRepository writeRepository, IPublishEndpoint publishEndpoint)
    {
        _writeRepository = writeRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.ProductDto.Name,
            Description = request.ProductDto.Description,
            Price = request.ProductDto.Price,
            CategoryId = request.ProductDto.CategoryId,
            CreatedAt = DateTime.UtcNow
        };

        await _writeRepository.AddAsync(product);

        var @event = new ProductCreatedEvent
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