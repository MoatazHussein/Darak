using Darak.Application.Common.Models;
using MediatR;

namespace Darak.Application.Features.Projects.ProjectProposals.Queries.GetAllProjectProposals;
public class GetAllProjectProposalsQuery : IRequest<PagedResult<GetAllProjectProposalsQueryResponse>>
{
    public Guid? ProjectRequestId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchPhrase { get; set; }
    public string? OrderBy { get; set; }
    public bool Descending { get; set; } = true;
}