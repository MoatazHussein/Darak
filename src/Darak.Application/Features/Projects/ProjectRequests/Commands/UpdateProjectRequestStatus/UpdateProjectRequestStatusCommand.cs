using MediatR;
using Darak.Domain.Enums;


namespace Darak.Application.Features.Projects.ProjectRequests.Commands.UpdateProjectRequestStatus;

public record UpdateProjectRequestStatusCommand(Guid Id, ProjectRequestStatus NewStatus)
    : IRequest;
