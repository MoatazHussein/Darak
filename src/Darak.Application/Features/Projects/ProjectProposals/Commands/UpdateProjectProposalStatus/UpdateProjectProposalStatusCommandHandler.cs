using Darak.Application.Common.Interfaces;
using Darak.Application.Common.Interfaces.Security;
using Darak.Domain.Entities.Projects;
using Darak.Domain.Enums;
using Darak.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Darak.Application.Features.Projects.ProjectProposals.Commands.UpdateProjectProposalStatus;

public class UpdateProjectProposalStatusCommandHandler(
    ILogger<UpdateProjectProposalStatusCommandHandler> logger,
    IRepository<ProjectProposal> projectProposalRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService
    )
    : IRequestHandler<UpdateProjectProposalStatusCommand>
{
    public async Task Handle(UpdateProjectProposalStatusCommand request, CancellationToken cancellationToken)
    {

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        logger.LogInformation("Updating ProjectProposal with id: {ProjectProposalId} with {@UpdatedProjectProposal}", request.Id, request);
        var projectProposal = await projectProposalRepository.GetByIdAsync(request.Id, cancellationToken, [p =>p.ProjectRequest]);
        if (projectProposal is null)
            throw new NotFoundException(nameof(ProjectProposal), request.Id.ToString());

        var projectRequest = projectProposal.ProjectRequest;
        if (projectRequest is null)
            throw new NotFoundException(nameof(ProjectRequest), projectProposal.ProjectRequestId.ToString());

        if (projectRequest.Status != ProjectRequestStatus.Open)
            throw new BusinessRuleException("can't change status for proposal of closed project", 409);

        var allowedContractorActions = new[] { ProjectProposalStatus.Cancelled };
        var allowedClientActions = new[] { ProjectProposalStatus.Accepted, ProjectProposalStatus.Rejected };


        if (projectProposal.CreatorId == currentUserService.UserId && !allowedContractorActions.Contains(request.NewStatus))
            throw new BusinessRuleException($"You are only allowed to update with {string.Join(", ", allowedContractorActions)}", 403);


        if (projectRequest.CreatorId == currentUserService.UserId && !allowedClientActions.Contains(request.NewStatus))
            throw new BusinessRuleException($"You are only allowed to update with {string.Join(", ", allowedClientActions)}", 403);

        if (projectProposal.CreatorId != currentUserService.UserId && projectRequest.CreatorId != currentUserService.UserId)
            throw new BusinessRuleException("You are not allowed to update this Proposal.", 403);

        if (request.NewStatus == ProjectProposalStatus.Accepted && projectProposal.Status == ProjectProposalStatus.Accepted)
            throw new BusinessRuleException("This proposal is already accepted.", 400);

        var acceptedProposalExists = await projectProposalRepository.AnyAsync(
                 pp => pp.ProjectRequestId == projectRequest.Id 
              && pp.Status == ProjectProposalStatus.Accepted 
              && request.NewStatus == ProjectProposalStatus.Accepted
              && pp.Id != projectProposal.Id
            , cancellationToken);

        if (acceptedProposalExists)
            throw new BusinessRuleException("Another proposal has already been accepted for this project request.", 409);

        projectProposal.Status = request.NewStatus;

        // If the proposal is accepted, close the project request 
        if (request.NewStatus == ProjectProposalStatus.Accepted)
        {
            projectRequest.Status = ProjectRequestStatus.Closed;

            // Reject all other pending proposals for the same project request
            await projectProposalRepository.BulkUpdateAsync(
                pp => pp.ProjectRequestId == projectRequest.Id
                      && pp.Id != request.Id
                      && pp.Status == ProjectProposalStatus.Pending,
                set => set.SetProperty(pp => pp.Status, ProjectProposalStatus.Rejected)
            );
        }

        var projectInitiationStatuses = new[] { ProjectProposalStatus.Rejected, ProjectProposalStatus.Cancelled };

        if (projectInitiationStatuses.Contains(request.NewStatus))
        {
            projectRequest.Status = ProjectRequestStatus.Open;
        }

        await projectProposalRepository.UpdateAsync(projectProposal);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await unitOfWork.CommitAsync(cancellationToken);

    }
}
