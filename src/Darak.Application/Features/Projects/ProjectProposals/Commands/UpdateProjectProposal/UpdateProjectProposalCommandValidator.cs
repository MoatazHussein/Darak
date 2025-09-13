using FluentValidation;

namespace Darak.Application.Features.Projects.ProjectProposals.Commands.UpdateProjectProposal;

public class UpdateProjectProposalCommandValidator : AbstractValidator<UpdateProjectProposalCommand>
{
    public UpdateProjectProposalCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Proposal content is required")
            .MaximumLength(1000)
            .WithMessage("Content must not exceed 1000 characters");

        RuleFor(x => x.ProposedAmount)
            .NotEmpty()
            .WithMessage("Proposed amount is required")
            .GreaterThan(0)
            .WithMessage("Proposed amount must be greater than 0")
            .PrecisionScale(18, 2, false)
            .WithMessage("Proposed amount cannot have more than 2 decimal places");
    }
}
