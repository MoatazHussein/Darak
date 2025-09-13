using System.Linq.Expressions;
using AutoMapper;
using Darak.Application.Common.Interfaces;
using Darak.Application.Common.Models;
using Darak.Domain.Entities.Projects;
using LinqKit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Darak.Application.Features.Projects.ProjectProposals.Queries.GetAllProjectProposals;

public class GetAllProjectProposalsQueryHandler(
    IRepository<ProjectProposal> repository,
    ILogger<GetAllProjectProposalsQueryHandler> logger,
    IMapper mapper,
     ITimeZoneConverter timeZoneConverter
    ) : IRequestHandler<GetAllProjectProposalsQuery, PagedResult<GetAllProjectProposalsQueryResponse>>
{
    public async Task<PagedResult<GetAllProjectProposalsQueryResponse>> Handle(
        GetAllProjectProposalsQuery request,
        CancellationToken cancellationToken)
    {

        logger.LogInformation("Getting all Project Proposals");

        var predicate = PredicateBuilder.New<ProjectProposal>(true);

        if (request.ProjectRequestId != null)
        {
            predicate = predicate.And(x => x.ProjectRequestId == request.ProjectRequestId);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchPhrase))
        {
            var search = request.SearchPhrase.Trim().ToLower();
            predicate = predicate.And(x => x.Content.ToLower().Contains(search));
        }

        Expression<Func<ProjectProposal, bool>> filter = predicate;

        Expression<Func<ProjectProposal, object>>? orderBy = request.OrderBy?.ToLower() switch
        {
            "content" => p => p.Content,
            "proposedamount" => p => p.ProposedAmount,

            _ => null // fallback: no ordering
        };

        var parameters = new QueryParameters<ProjectProposal>
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            Filter = filter,
            OrderBy = orderBy,
            Descending = request.Descending,
            Includes = [ p => p.Creator!]
        };

        var (projectProposals, totalCount) = await repository.GetAllMatchingAsync(parameters, cancellationToken);

        var projectProposalDtos = projectProposals.Select(projectProposal => mapper.Map<GetAllProjectProposalsQueryResponse>(projectProposal)).ToList();

        var result = new PagedResult<GetAllProjectProposalsQueryResponse>(projectProposalDtos, totalCount, request.PageSize, request.PageNumber);

        return timeZoneConverter.ConvertUtcToLocal(result);
    }
}
