using AutoMapper;
using Darak.Application.Common.Interfaces;
using Darak.Application.Common.Interfaces.Security;
using Darak.Domain.Entities.Projects;
using MediatR;

namespace Darak.Application.Features.Projects.ProjectProposals.Commands.CreateProjectProposal;

public class CreateProjectProposalCommandHandler(
    IRepository<ProjectProposal> repository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IMapper mapper
    ) : IRequestHandler<CreateProjectProposalCommand, Guid>
{
    public async Task<Guid> Handle(CreateProjectProposalCommand request, CancellationToken cancellationToken)
    {

        var entity = mapper.Map<ProjectProposal>(request);
        entity.CreatorId = currentUserService.UserId;
        entity.CreatedAt = DateTime.UtcNow;

        await repository.AddAsync(entity, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
