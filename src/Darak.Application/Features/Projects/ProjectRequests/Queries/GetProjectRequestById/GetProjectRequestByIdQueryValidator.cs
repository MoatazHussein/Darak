using FluentValidation;

namespace Darak.Application.Features.Projects.ProjectRequests.Queries.GetProjectRequestById;

public class GetProjectRequestByIdQueryValidator : AbstractValidator<GetProjectRequestByIdQuery>
{
    public GetProjectRequestByIdQueryValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty()
            .WithMessage("Id is required");
    }
}