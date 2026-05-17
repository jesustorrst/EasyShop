using EasyShop.Catalog.Application.DTOs;
using EasyShop.Catalog.Application.Interfaces;
using MediatR;

namespace EasyShop.Catalog.Application.Features.Products.Queries.GetProductList;

public record GetProductListQuery() : IRequest<List<ProductDto>>;
