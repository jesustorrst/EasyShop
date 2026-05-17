using EasyShop.Catalog.Application.DTOs;
using EasyShop.Catalog.Application.Interfaces;
using EasyShop.Catalog.Domain.Entities;
using FluentValidation;

namespace EasyShop.Catalog.Application.Features.Products.Commands.CreateProduct;

public class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    private readonly IGenericRepository<Category> _categoryRepository;

    public CreateProductValidator(IGenericRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;

        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("El nombre del producto es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(p => p.Description)
            .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres");

        RuleFor(p => p.Price)
            .NotEmpty().WithMessage("El precio es requerido")
            .GreaterThan(0).WithMessage("El precio debe ser mayor a 0");

        RuleFor(p => p.CategoryId)
            .NotEmpty().WithMessage("CategoryId es requerido")
            .MustAsync(CategoryExistsAsync).WithMessage("La categoría especificada no existe");
    }

    private async Task<bool> CategoryExistsAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        return category != null;
    }


}