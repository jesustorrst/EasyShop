using EasyShop.Catalog.Application.Interfaces;
using EasyShop.Catalog.Domain.Events;
using MassTransit;
using MediatR;

namespace EasyShop.Catalog.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductWriteRepository _writeRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    private readonly IFileStorageService _fileStorageService;

    public DeleteProductHandler(IProductWriteRepository writeRepository, IPublishEndpoint publishEndpoint, IFileStorageService fileStorageService)
    {
        _writeRepository = writeRepository;
        _publishEndpoint = publishEndpoint;
        _fileStorageService = fileStorageService;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _writeRepository.GetByIdAsync(request.Id);

        if (product == null)
            return false;


        try
        {
            string fileNameForAzure = Path.GetFileName(new Uri(product.ImageUrl).LocalPath);

            await _fileStorageService.DeleteFileAsync(fileNameForAzure, cancellationToken);
        }
        catch
        {


        }


        await _writeRepository.DeleteAsync(request.Id);

        // mongo
        var @event = new ProductDeletedEvent
        {
            ProductId = request.Id
        };

        await _publishEndpoint.Publish(@event, cancellationToken);

        return true;
    }
}