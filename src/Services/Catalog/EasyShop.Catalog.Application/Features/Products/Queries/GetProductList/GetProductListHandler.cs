using EasyShop.Catalog.Application.DTOs;
using EasyShop.Catalog.Application.Interfaces;
using MediatR;


namespace EasyShop.Catalog.Application.Features.Products.Queries.GetProductList;

public class GetProductListHandler : IRequestHandler<GetProductListQuery, List<ProductDto>>
{
    private readonly IProductReadRepository _readRepository;
    private readonly IFileStorageService _fileStorageService;
    public GetProductListHandler(IProductReadRepository readRepository, IFileStorageService fileStorageService)
    {
        _readRepository = readRepository;
        _fileStorageService = fileStorageService;
    }

    public async Task<List<ProductDto>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
    {
        var products = await _readRepository.GetAllWithCategoryAsync();

        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            CategoryId = p.CategoryId,
            CategoryName = p.Category?.Name ?? string.Empty,
            ImageUrl = _fileStorageService.GetSecureUrl(p.ImageUrl, cancellationToken)
        }).ToList();
    }
}