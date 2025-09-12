using Darak.Domain.Entities;
using FluentValidation;

namespace Darak.Application.Features.ServiceCategories.Queries.GetServiceCategoryById;

public class GetServiceCategoryByIdQueryHandlerValidator : AbstractValidator<GetServiceCategoryByIdQuery>
{
    private readonly string[] allowedSortByColumnNames = [nameof(ServiceCategory.NameAr), nameof(ServiceCategory.NameEn)];

    public GetServiceCategoryByIdQueryHandlerValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty()
            .WithMessage("Id is required");

    }
}