using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Business.Dtos.CategoryDtos;

public record CategoryCreateDto
{
    public string Name { get; set; }
    public IFormFile Logo { get; set; }
}

public class CategoryCreateDtoValidator:AbstractValidator<CategoryCreateDto>
{
    public CategoryCreateDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
                .WithMessage("Kateqoriya adi bos ola bilmez")
            .NotNull()
                .WithMessage("Kateqoriya adi null ola bilmez")
            .MaximumLength(64)
                .WithMessage("Kateqoriya adi 64-den uzun ola bilmez");
        RuleFor(c => c.Logo)
            .NotNull()
                .WithMessage("Kateqoriya adi null ola bilmez");
    }
}

