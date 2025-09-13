using FluentValidation;

namespace Darak.Application.Features.Projects.ProjectRequests.Commands.UpdateProjectRequestStatus;

public class UpdateProjectRequestStatusCommandValidator : AbstractValidator<UpdateProjectRequestStatusCommand>
{
    public UpdateProjectRequestStatusCommandValidator()
    {
        RuleFor(x => x.NewStatus)
           .IsInEnum()
           .WithMessage("Invalid status value. Must be one of: Open, Closed, Cancelled.");
    }
}
