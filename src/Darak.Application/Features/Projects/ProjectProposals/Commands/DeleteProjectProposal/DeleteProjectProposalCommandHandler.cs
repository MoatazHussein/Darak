using Darak.Application.Common.Interfaces;
using Darak.Application.Common.Interfaces.Security;
using Darak.Domain.Entities.Projects;
using Darak.Domain.Enums;
using Darak.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Darak.Application.Features.Projects.ProjectProposals.Commands.DeleteProjectProposal
{
    internal class DeleteProjectProposalCommandHandler
        (ILogger<DeleteProjectProposalCommandHandler> logger,
        IRepository<ProjectProposal> repository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService
      )  : IRequestHandler<DeleteProjectProposalCommand>
    {
        public async Task Handle(DeleteProjectProposalCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting ProjectProposal with id: {ProjectProposalId}", request.Id);
            var projectProposal = await repository.GetByIdAsync(request.Id);
            if (projectProposal is null)
                throw new NotFoundException(nameof(ProjectProposal), request.Id.ToString());


            if (projectProposal.CreatorId != currentUserService.UserId)
                throw new BusinessRuleException("You are not allowed to delete this Proposal.", 403);


            if (projectProposal.Status != ProjectProposalStatus.Pending)
                throw new BusinessRuleException($"Can't delete Proposal with {projectProposal.Status.ToString()} status", 409);

            await repository.DeleteAsync(projectProposal);

            await  unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
