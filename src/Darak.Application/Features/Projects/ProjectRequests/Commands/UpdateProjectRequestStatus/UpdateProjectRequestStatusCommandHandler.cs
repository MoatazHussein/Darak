using Darak.Application.Common.Interfaces;
using Darak.Application.Common.Interfaces.Security;
using Darak.Domain.Entities.Projects;
using Darak.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Darak.Application.Features.Projects.ProjectRequests.Commands.UpdateProjectRequestStatus;

public class UpdateProjectRequestStatusCommandHandler
    (ILogger<UpdateProjectRequestStatusCommandHandler> logger,
    ICurrentUserService currentUserService,
    IRepository<ProjectRequest> repository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<UpdateProjectRequestStatusCommand>
{
    public async Task Handle(UpdateProjectRequestStatusCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating ProjectRequest with id: {ProjectRequestId} with {@UpdatedProjectRequest}", request.Id, request);

        var projectRequest = await repository.GetByIdAsync(request.Id);

        if (projectRequest is null)
            throw new NotFoundException(nameof(ProjectRequest), request.Id.ToString());


        if (projectRequest.CreatorId != currentUserService.UserId)
            throw new BusinessRuleException("You are not allowed to Update this project.", 403);

        projectRequest.Status = request.NewStatus;

        await repository.UpdateAsync(projectRequest);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
