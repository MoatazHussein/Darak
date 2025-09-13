using System.Linq.Expressions;
using AutoMapper;
using Darak.Application.Common.Interfaces;
using Darak.Application.Common.Models;
using Darak.Application.Features.Projects.ProjectRequests.Queries.Dtos;
using Darak.Application.Features.ServiceCategories.Queries.GetAllServiceCategories;
using Darak.Domain.Entities.Projects;
using LinqKit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Darak.Application.Features.Projects.ProjectRequests.Queries.GetAllProjectRequests;

public class GetAllProjectRequestsQueryHandler(ILogger<GetAllProjectRequestsQuery> logger,
    IMapper mapper,
    IRepository<ProjectRequest> repository,
    ITimeZoneConverter timeZoneConverter)
    : IRequestHandler<GetAllProjectRequestsQuery, PagedResult<GetAllProjectRequestsQueryResponse>>
{
    public async Task<PagedResult<GetAllProjectRequestsQueryResponse>> Handle(GetAllProjectRequestsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all Project Requests");

        var predicate = PredicateBuilder.New<ProjectRequest>(true);

        if (request.ServiceCategoryId != null)
        {
            predicate = predicate.And(x => x.ServiceCategoryId == request.ServiceCategoryId);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchPhrase))
        {
            var search = request.SearchPhrase.Trim().ToLower();
            predicate = predicate.And(x => x.Title.ToLower().Contains(search) ||
                                          x.Description.ToLower().Contains(search));
        }

        Expression<Func<ProjectRequest, bool>> filter = predicate;

        Expression<Func<ProjectRequest, object>>? orderBy = request.OrderBy?.ToLower() switch
        {
            "title" => p => p.Title,
            "description" => p => p.Description,

            _ => null // fallback: no ordering
        };

        var parameters = new QueryParameters<ProjectRequest>
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            Filter = filter,
            OrderBy = orderBy,
            Descending = request.Descending,
        };

        var (projectRequests, totalCount) = await repository.GetAllMatchingAsync(parameters, cancellationToken);

        var projectRequestDtos = projectRequests.Select(projectRequest => mapper.Map<GetAllProjectRequestsQueryResponse>(projectRequest)).ToList();

        var result = new PagedResult<GetAllProjectRequestsQueryResponse>(projectRequestDtos, totalCount, request.PageSize, request.PageNumber);

        return timeZoneConverter.ConvertUtcToLocal(result);
    }
}


