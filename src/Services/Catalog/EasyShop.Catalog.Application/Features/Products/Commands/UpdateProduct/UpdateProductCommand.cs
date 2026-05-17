using EasyShop.Catalog.Application.DTOs;
using MediatR;

namespace EasyShop.Catalog.Application.Features.Products.Commands.UpdateProduct;

public record UpdateProductCommand(Guid Id, CreateProductDto ProductDto) : IRequest<ProductDto?>;