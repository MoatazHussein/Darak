using Darak.Application.Common.Models;
using Darak.Domain.Constants;
using Darak.Domain.Entities;
using MediatR;

namespace Darak.Application.Features.ServiceCategories.Queries.GetAllServiceCategories;

public class GetAllServiceCategoriesQuery : IRequest<PagedResult<GetAllServiceCategoriesQueryResponse>>
{
    public string? SearchPhrase { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? OrderBy { get; set; }
    public bool Descending { get; set; } = true;
}