using MediatR;
using Darak.Application.Features.Projects.ProjectRequests.Queries.Dtos;

namespace Darak.Application.Features.Projects.ProjectRequests.Queries.GetProjectRequestById;

public class GetProjectRequestByIdQuery(Guid id) : IRequest<GetProjectRequestByIdQueryResponse?>
{
    public Guid Id { get;} = id;
}
