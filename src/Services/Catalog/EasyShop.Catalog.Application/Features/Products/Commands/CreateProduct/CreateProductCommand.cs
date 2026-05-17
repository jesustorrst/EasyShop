using EasyShop.Catalog.Application.DTOs;
using MediatR;

namespace EasyShop.Catalog.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(CreateProductDto ProductDto) : IRequest<ProductDto>;