using Darak.Domain.Entities;
using FluentValidation;

namespace Darak.Application.Features.ServiceCategories.Queries.GetServiceCategoryById;

public class GetServiceCategoryByIdQueryValidator : AbstractValidator<GetServiceCategoryByIdQuery>
{
    private readonly string[] allowedSortByColumnNames = [nameof(ServiceCategory.NameAr), nameof(ServiceCategory.NameEn)];

    public GetServiceCategoryByIdQueryValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty()
            .WithMessage("Id is required");

    }
}