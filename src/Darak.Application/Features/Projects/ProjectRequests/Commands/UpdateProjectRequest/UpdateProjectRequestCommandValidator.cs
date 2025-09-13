using FluentValidation;

namespace Darak.Application.Features.Projects.ProjectRequests.Commands.UpdateProjectRequest;

public class UpdateProjectRequestCommandValidator : AbstractValidator<UpdateProjectRequestCommand>
{
    public UpdateProjectRequestCommandValidator()
    {
        RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.");

        RuleFor(dto => dto.ServiceCategoryId)
        .NotEmpty().WithMessage("Please provide a Service Category Id ");

        RuleFor(x => x.MinBudget)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum budget cannot be negative.")
                .LessThanOrEqualTo(x => x.MaxBudget).WithMessage("Minimum budget must be less than or equal to maximum budget.");

        RuleFor(x => x.MaxBudget)
                .GreaterThanOrEqualTo(0).WithMessage("Maximum budget cannot be negative.")
                .LessThanOrEqualTo(1000000).WithMessage("Maximum budget cannot exceed 1,000,000.");
    }
}
