using AutoMapper;
using Darak.Application.Common.Interfaces;
using Darak.Domain.Entities.Projects;
using Darak.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Darak.Application.Features.Projects.ProjectRequests.Queries.GetProjectRequestById;

public class GetProjectRequestByIdQueryHandler(
   IMapper mapper,
   ILogger<GetProjectRequestByIdQuery> logger,
    IRepository<ProjectRequest> repository,
    ITimeZoneConverter timeZoneConverter
    )
  : IRequestHandler<GetProjectRequestByIdQuery, GetProjectRequestByIdQueryResponse?>
{
    public async Task<GetProjectRequestByIdQueryResponse?> Handle(GetProjectRequestByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting ProjectRequest with Id {ProjectRequestId}", request.Id);

        var existingProjectRequest = await repository.GetByIdAsync(request.Id, cancellationToken, pr => pr.ServiceCategory);

        if (existingProjectRequest is null)
        {
            throw new NotFoundException(nameof(ProjectRequest), request.Id.ToString());
        }

        var projectRequestDto = mapper.Map<GetProjectRequestByIdQueryResponse>(existingProjectRequest);

        return timeZoneConverter.ConvertUtcToLocal(projectRequestDto);

    }
}
