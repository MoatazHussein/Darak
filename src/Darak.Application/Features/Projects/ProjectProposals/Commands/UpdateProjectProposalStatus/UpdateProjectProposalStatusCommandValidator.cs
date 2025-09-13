using FluentValidation;

namespace Darak.Application.Features.Projects.ProjectProposals.Commands.UpdateProjectProposalStatus;

public class UpdateProjectProposalStatusCommandValidator : AbstractValidator<UpdateProjectProposalStatusCommand>
{
    public UpdateProjectProposalStatusCommandValidator()
    {
        RuleFor(x => x.NewStatus)
      .IsInEnum()
      .WithMessage("Invalid status value. Must be one of: Pending, Accepted, Rejected");

    }
}
