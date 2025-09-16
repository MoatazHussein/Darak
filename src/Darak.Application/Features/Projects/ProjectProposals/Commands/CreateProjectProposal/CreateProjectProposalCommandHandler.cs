using AutoMapper;
using Darak.Application.Common.Interfaces;
using Darak.Application.Common.Interfaces.Security;
using Darak.Domain.Entities.Projects;
using Darak.Domain.Enums;
using Darak.Domain.Exceptions;
using MediatR;

namespace Darak.Application.Features.Projects.ProjectProposals.Commands.CreateProjectProposal;

public class CreateProjectProposalCommandHandler(
    IRepository<ProjectProposal> projectProposalRepository,
    IRepository<ProjectRequest> projectRequestRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IMapper mapper
    ) : IRequestHandler<CreateProjectProposalCommand, Guid>
{
    public async Task<Guid> Handle(CreateProjectProposalCommand request, CancellationToken cancellationToken)
    {

        var projectRequest = await projectRequestRepository.GetByIdAsync(request.ProjectRequestId);
        if (projectRequest is null)
            throw new NotFoundException(nameof(ProjectRequest), request.ProjectRequestId.ToString());

        if (projectRequest.Status != ProjectRequestStatus.Open)
            throw new BusinessRuleException("This Project is not open now", 409);

        var userHasAnotherProposal = await projectProposalRepository.AnyAsync(
            p => p.CreatorId == currentUserService.UserId && p.ProjectRequestId == request.ProjectRequestId
        , cancellationToken);

        if (userHasAnotherProposal)
            throw new BusinessRuleException("You are not allowed to add more than one Proposal.", 403);


        var entity = mapper.Map<ProjectProposal>(request);
        entity.CreatorId = currentUserService.UserId;
        entity.CreatedAt = DateTime.UtcNow;

        await projectProposalRepository.AddAsync(entity, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
