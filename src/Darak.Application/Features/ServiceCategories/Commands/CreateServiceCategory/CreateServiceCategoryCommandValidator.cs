using FluentValidation;

namespace Darak.Application.Features.ServiceCategories.Commands.CreateServiceCategory;

public class CreateServiceCategoryCommandValidator : AbstractValidator<CreateServiceCategoryCommand>
{
    public CreateServiceCategoryCommandValidator()
    {
        RuleFor(dto => dto.NameEn).
            NotEmpty().WithMessage("Please provide a valid English name")
            .Length(3, 50);

        RuleFor(dto => dto.NameAr).
            NotEmpty().WithMessage("Please provide a valid Arabic name")
            .Length(3, 50);
    }
}
