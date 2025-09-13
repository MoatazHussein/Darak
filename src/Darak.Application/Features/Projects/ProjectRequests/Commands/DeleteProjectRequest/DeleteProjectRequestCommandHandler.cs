using Darak.Application.Common.Interfaces;
using Darak.Application.Common.Interfaces.Security;
using Darak.Domain.Entities.Projects;
using Darak.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Darak.Application.Features.Projects.ProjectRequests.Commands.DeleteProjectRequest
{
    internal class DeleteProjectRequestCommandHandler(
        ILogger<DeleteProjectRequestCommandHandler> logger,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork,
        IRepository<ProjectRequest> repository
        ) : IRequestHandler<DeleteProjectRequestCommand>
    {
        public async Task Handle(DeleteProjectRequestCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting ProjectRequest with id: {ProjectRequestId}", request.Id);
            var projectRequest = await repository.GetByIdAsync(request.Id);
            if (projectRequest is null)
                throw new NotFoundException(nameof(projectRequest), request.Id.ToString());

            if (projectRequest.CreatorId != currentUserService.UserId)
                throw new BusinessRuleException("You are not allowed to delete this project.", 403);

            await repository.DeleteAsync(projectRequest);

            await unitOfWork.SaveChangesAsync(cancellationToken);

        }
    }
}
