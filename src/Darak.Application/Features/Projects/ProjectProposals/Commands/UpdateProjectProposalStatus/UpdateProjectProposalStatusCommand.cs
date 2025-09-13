using MediatR;
using Darak.Domain.Enums;


namespace Darak.Application.Features.Projects.ProjectProposals.Commands.UpdateProjectProposalStatus;

public record UpdateProjectProposalStatusCommand(Guid Id, ProjectProposalStatus NewStatus)
    : IRequest;

