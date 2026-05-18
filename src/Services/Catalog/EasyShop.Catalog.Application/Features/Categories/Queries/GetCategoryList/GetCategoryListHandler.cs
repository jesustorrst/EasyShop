using EasyShop.Catalog.Application.DTOs;
using EasyShop.Catalog.Application.Interfaces;
using MediatR;
using EasyShop.Catalog.Application.Features.Categories.Queries.GetCategoryList;

namespace EasyShop.Catalog.Application.Features.Categories.Queries.GetCategoryList;

public class GetCategoryListHandler : IRequestHandler<GetCategoryListQuery, List<CategoryDto>>
{
    private readonly ICategoryReadRepository _readRepository;

    public GetCategoryListHandler(ICategoryReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<List<CategoryDto>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
    {
        var categories = await _readRepository.GetAllAsync();

        return categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        }).ToList();
    }
}

