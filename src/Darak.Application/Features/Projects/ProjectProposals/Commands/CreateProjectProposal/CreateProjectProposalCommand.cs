using MediatR;

namespace Darak.Application.Features.Projects.ProjectProposals.Commands.CreateProjectProposal;

public record CreateProjectProposalCommand(
Guid ProjectRequestId,
string Content,
decimal ProposedAmount
) : IRequest<Guid>;
