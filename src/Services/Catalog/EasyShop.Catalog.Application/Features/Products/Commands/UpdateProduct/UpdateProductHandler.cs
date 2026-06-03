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

    private readonly IFileStorageService _fileStorageService;

    public UpdateProductHandler(
        IProductWriteRepository writeRepository,
        IFileStorageService fileStorageService,
        IProductReadRepository readRepository,
        IPublishEndpoint publishEndpoint)
    {
        _writeRepository = writeRepository;
        _readRepository = readRepository;
        _publishEndpoint = publishEndpoint;
        _fileStorageService = fileStorageService;
    }

    public async Task<ProductDto?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _writeRepository.GetByIdAsync(request.Id);

        if (product == null)
            return null;

        string oldImageUrl = product.ImageUrl;
        string finalImageUrl = product.ImageUrl;

        if (request.ProductDto.ImageStream != null && request.ProductDto.ImageFileName != null)
        {
            string uniqueFileName = $"{Guid.NewGuid()}_{request.ProductDto.ImageFileName}";

            finalImageUrl = await _fileStorageService.UploadFileAsync(request.ProductDto.ImageStream, uniqueFileName, cancellationToken);

            if (!string.IsNullOrEmpty(oldImageUrl))
            {
                try
                {
                    string oldFileName = Path.GetFileName(new Uri(oldImageUrl).LocalPath);
                    await _fileStorageService.DeleteFileAsync(oldFileName, cancellationToken);
                }
                catch
                {
                }
            }
        }

        product.Name = request.ProductDto.Name;
        product.Description = request.ProductDto.Description;
        product.Price = request.ProductDto.Price;
        product.CategoryId = request.ProductDto.CategoryId;
        product.UpdatedAt = DateTime.UtcNow;
        product.ImageUrl = finalImageUrl;

        //mongo
        await _writeRepository.UpdateAsync(product, request.Id);

        var @event = new ProductUpdatedEvent
        {
            ProductId = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            ImageUrl = product.ImageUrl

        };

        await _publishEndpoint.Publish(@event, cancellationToken);

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            ImageUrl = product.ImageUrl
        };
    }
}