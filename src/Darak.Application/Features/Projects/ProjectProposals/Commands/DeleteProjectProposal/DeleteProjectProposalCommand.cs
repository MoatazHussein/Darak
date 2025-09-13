using MediatR;

namespace Darak.Application.Features.Projects.ProjectProposals.Commands.DeleteProjectProposal;

public class DeleteProjectProposalCommand(Guid id) : IRequest
{
    public Guid Id { get; } = id;
}
