using EasyShop.Catalog.Application.DTOs;
using EasyShop.Catalog.Application.Interfaces;
using EasyShop.Catalog.Domain.Entities;
using FluentValidation;

namespace EasyShop.Catalog.Application.Features.Products.Commands.CreateProduct;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    private readonly IGenericRepository<Category> _categoryRepository;

    private static readonly Dictionary<string, byte[]> _imageSignatures = new()
    {
        { "jpeg", new byte[] { 0xFF, 0xD8, 0xFF } },
        { "png",  new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } },
        { "webp", new byte[] { 0x52, 0x49, 0x46, 0x46 } }
    };

    public CreateProductValidator(IGenericRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;

        RuleFor(p => p.ProductDto.Name)
            .NotEmpty().WithMessage("El nombre del producto es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(p => p.ProductDto.Description)
            .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres");

        RuleFor(p => p.ProductDto.Price)
            .NotEmpty().WithMessage("El precio es requerido")
            .GreaterThan(0).WithMessage("El precio debe ser mayor a 0");

        RuleFor(p => p.ProductDto.CategoryId)
            .NotEmpty().WithMessage("CategoryId es requerido")
            .MustAsync(CategoryExistsAsync).WithMessage("La categoría especificada no existe");

        RuleFor(p => p.ProductDto.ImageStream)
            .Must(HaveValidSize).WithMessage("La imagen no puede exceder 2MB")
            .Must(BeValidExtension).WithMessage("El archivo debe ser una imagen válida (JPEG, PNG, WEBP)")
            .When(p => p.ProductDto.ImageStream != null);

    }

    private bool HaveValidSize(Stream imageStream)
    {
        if (imageStream == null)
        {
            return true;
        }

        const long maxSizeBytes = 2 * 1024 * 1024;
        return imageStream.Length <= maxSizeBytes;
    }

    private bool BeValidExtension(Stream imageStream)
    {
        if (imageStream == null)
        {
            return true;
        }

        try
        {
            using var reader = new BinaryReader(imageStream);

            var headerBytes = reader.ReadBytes(8);

            imageStream.Position = 0;

            foreach (var signature in _imageSignatures.Values)
            {
                if (headerBytes.Take(signature.Length).SequenceEqual(signature))
                {
                    return true;
                }
            }

            return false;
        }
        catch
        {
            return false;
        }

    }

    private async Task<bool> CategoryExistsAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        return category != null;
    }


}