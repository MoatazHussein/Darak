using MediatR;
using Darak.Application.Common.Models;

namespace Darak.Application.Features.Projects.ProjectRequests.Queries.GetAllProjectRequests;

public class GetAllProjectRequestsQuery : IRequest<PagedResult<GetAllProjectRequestsQueryResponse>>
{
    public Guid? ServiceCategoryId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchPhrase { get; set; }
    public string? OrderBy { get; set; }
    public bool Descending { get; set; } = true;
}