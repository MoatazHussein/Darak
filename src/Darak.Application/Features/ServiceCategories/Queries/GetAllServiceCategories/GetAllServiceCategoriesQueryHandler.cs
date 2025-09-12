using System.Linq.Expressions;
using AutoMapper;
using Darak.Application.Common.Interfaces;
using Darak.Application.Common.Models;
using Darak.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Darak.Application.Features.ServiceCategories.Queries.GetAllServiceCategories;

public class GetAllServiceCategoriesQueryHandler(ILogger<GetAllServiceCategoriesQuery> logger,
    IMapper mapper,
    IRepository<ServiceCategory> repository, ITimeZoneConverter timeZoneConverter) 
    : IRequestHandler<GetAllServiceCategoriesQuery, PagedResult<GetAllServiceCategoriesQueryResponse>>
{
    public async Task<PagedResult<GetAllServiceCategoriesQueryResponse>> Handle(GetAllServiceCategoriesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all Categories");

        Expression<Func<ServiceCategory, bool>>? filter = null;

        if (!string.IsNullOrWhiteSpace(request.SearchPhrase))
        {
            var search = request.SearchPhrase.Trim().ToLower();
            filter = x => x.NameEn.ToLower().Contains(search) || x.NameAr.ToLower().Contains(search);
        }

        Expression<Func<ServiceCategory, object>>? orderBy = request.OrderBy?.ToLower() switch
        {
            "nameen" => p => p.NameEn,
            "namear" => p => p.NameAr,

            _ => null // fallback: no ordering
        };

        var parameters = new QueryParameters<ServiceCategory>
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            Filter = filter,
            OrderBy = orderBy,
            Descending = request.Descending,
        };

        var (categories, totalCount) = await repository.GetAllMatchingAsync(parameters, cancellationToken);

        var newsCategoriesDto = categories.Select(category => mapper.Map<GetAllServiceCategoriesQueryResponse>(category)).ToList();

        var result = new PagedResult<GetAllServiceCategoriesQueryResponse>(newsCategoriesDto, totalCount, request.PageSize, request.PageNumber);

        return timeZoneConverter.ConvertUtcToLocal(result);
    }


}