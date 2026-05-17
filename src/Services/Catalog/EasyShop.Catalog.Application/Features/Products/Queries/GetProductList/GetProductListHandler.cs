using EasyShop.Catalog.Application.DTOs;
using EasyShop.Catalog.Application.Interfaces;
using MediatR;

namespace EasyShop.Catalog.Application.Features.Products.Queries.GetProductList;

public class GetProductListHandler : IRequestHandler<GetProductListQuery, List<ProductDto>>
{
    private readonly IProductReadRepository _readRepository;

    public GetProductListHandler(IProductReadRepository readRepository)
    {
        _readRepository = readRepository;
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
            CategoryName = p.Category?.Name ?? string.Empty
        }).ToList();
    }
}