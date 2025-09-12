using Darak.Domain.Entities;
using FluentValidation;

namespace Darak.Application.Features.ServiceCategories.Queries.GetAllServiceCategories;

public class GetAllServiceCategoriesQueryHandlerValidator : AbstractValidator<GetAllServiceCategoriesQuery>
{
    private readonly string[] allowedSortByColumnNames = [nameof(ServiceCategory.NameAr), nameof(ServiceCategory.NameEn)];

    public GetAllServiceCategoriesQueryHandlerValidator()
    {
        RuleFor(r => r.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be at least 1");

        RuleFor(r => r.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be at least 1");

        RuleFor(r => r.OrderBy)
            .Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
            .WithMessage($"Sort by is optional, or must be one of: {string.Join(", ", allowedSortByColumnNames)}");

    }
}