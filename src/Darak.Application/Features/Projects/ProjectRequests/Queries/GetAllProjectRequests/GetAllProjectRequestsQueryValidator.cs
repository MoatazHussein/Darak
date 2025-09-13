using Darak.Domain.Entities.Projects;
using FluentValidation;

namespace Darak.Application.Features.Projects.ProjectRequests.Queries.GetAllProjectRequests;

public class GetAllProjectRequestsQueryValidator : AbstractValidator<GetAllProjectRequestsQuery>
{
    private string[] allowedSortByColumnNames = [nameof(ProjectRequest.Title), nameof(ProjectRequest.Description)];


    public GetAllProjectRequestsQueryValidator()
    {
        RuleFor(r => r.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(r => r.OrderBy)
            .Must(value => allowedSortByColumnNames.Contains(value))
            .When(q => q.OrderBy != null)
            .WithMessage($"Sort by is optional, or must be in [{string.Join(",", allowedSortByColumnNames)}]");

    }
}