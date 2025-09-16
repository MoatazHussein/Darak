using AutoMapper;
using Darak.Application.Common.Interfaces;
using Darak.Application.Common.Interfaces.Security;
using Darak.Domain.Entities.Projects;
using Darak.Domain.Enums;
using Darak.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Darak.Application.Features.Projects.ProjectProposals.Commands.UpdateProjectProposal;

public class UpdateProjectProposalCommandHandler(
    ILogger<UpdateProjectProposalCommandHandler> logger,
    IRepository<ProjectProposal> repository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IMapper mapper
   ) : IRequestHandler<UpdateProjectProposalCommand>
{
    public async Task Handle(UpdateProjectProposalCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating ProjectProposal with id: {ProjectProposalId} with {@UpdatedProjectProposal}", request.Id, request);
        var projectProposal = await repository.GetByIdAsync(request.Id, cancellationToken,[p=>p.ProjectRequest]);
        if (projectProposal is null)
            throw new NotFoundException(nameof(ProjectProposal), request.Id.ToString());

        if (projectProposal.CreatorId != currentUserService.UserId)
            throw new BusinessRuleException("You are not allowed to update this Proposal.", 403);

        var projectRequest = projectProposal.ProjectRequest;

        if (projectRequest.Status != ProjectRequestStatus.Open)
            throw new BusinessRuleException("can't change status for proposal of closed project", 409);

        mapper.Map(request, projectProposal);

        await repository.UpdateAsync(projectProposal);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
