using AutoMapper;
using Darak.Application.Common.Interfaces;
using Darak.Application.Common.Interfaces.Security;
using Darak.Domain.Entities;
using Darak.Domain.Entities.Projects;
using Darak.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Darak.Application.Features.Projects.ProjectRequests.Commands.UpdateProjectRequest;

public class UpdateProjectRequestCommandHandler(
    ILogger<UpdateProjectRequestCommandHandler> logger,
    IRepository<ProjectRequest> projectRequestRepository,
    IRepository<ServiceCategory> serviceCategoryRepository,
    ICurrentUserService currentUserService,
    IMapper mapper,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<UpdateProjectRequestCommand>
{
    public async Task Handle(UpdateProjectRequestCommand request, CancellationToken cancellationToken)
    {
        var existingServiceCategory = await serviceCategoryRepository.AnyAsync(sc => sc.Id == request.ServiceCategoryId, cancellationToken);

        if (!existingServiceCategory)
            throw new NotFoundException(nameof(ServiceCategory), request.ServiceCategoryId.ToString());

        logger.LogInformation("Updating ProjectRequest with id: {ProjectRequestId} with {@UpdatedProjectRequest}", request.Id, request);

        var projectRequest = await projectRequestRepository.GetByIdAsync(request.Id);

        if (projectRequest is null)
            throw new NotFoundException(nameof(ProjectRequest), request.Id.ToString());


        if (projectRequest.CreatorId != currentUserService.UserId)
            throw new BusinessRuleException("You are not allowed to Update this project.", 403);

        mapper.Map(request, projectRequest);

        await projectRequestRepository.UpdateAsync(projectRequest);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
