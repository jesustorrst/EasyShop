using EasyShop.Catalog.Application.DTOs;
using EasyShop.Catalog.Application.Interfaces;
using MediatR;

namespace EasyShop.Catalog.Application.Features.Categories.Queries.GetCategoryList;

public record GetCategoryListQuery() : IRequest<List<CategoryDto>>;
