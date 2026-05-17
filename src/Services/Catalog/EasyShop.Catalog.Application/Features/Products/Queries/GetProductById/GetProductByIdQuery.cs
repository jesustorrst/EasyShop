using EasyShop.Catalog.Application.DTOs;
using MediatR;

namespace EasyShop.Catalog.Application.Features.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto?>;