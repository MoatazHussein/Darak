using AutoMapper;
using Darak.Application.Common.Interfaces;
using Darak.Application.Common.Interfaces.Security;
using Darak.Domain.Entities;
using Darak.Domain.Entities.Projects;
using Darak.Domain.Exceptions;
using MediatR;

namespace Darak.Application.Features.Projects.ProjectRequests.Commands.CreateProjectRequest;

public class CreateProjectRequestCommandHandler(
    IRepository<ProjectRequest> projectRequestRepository,
    IRepository<ServiceCategory> serviceCategoryRepository,
    ICurrentUserService currentUserService,
    IMapper mapper,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<CreateProjectRequestCommand, Guid>
{
    public async Task<Guid> Handle(CreateProjectRequestCommand request, CancellationToken cancellationToken)
    {
        var existingServiceCategory = await serviceCategoryRepository.AnyAsync(sc => sc.Id == request.ServiceCategoryId, cancellationToken);

        if (!existingServiceCategory)
            throw new NotFoundException(nameof(ServiceCategory), request.ServiceCategoryId.ToString());

        var entity = mapper.Map<ProjectRequest>(request);

        entity.CreatorId = currentUserService.UserId;
        entity.CreatedAt = DateTime.UtcNow;

        await projectRequestRepository.AddAsync(entity, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
