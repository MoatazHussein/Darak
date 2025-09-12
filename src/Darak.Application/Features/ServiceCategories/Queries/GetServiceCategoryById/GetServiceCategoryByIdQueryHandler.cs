using AutoMapper;
using Darak.Application.Common.Interfaces;
using Darak.Domain.Entities;
using Darak.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Darak.Application.Features.ServiceCategories.Queries.GetServiceCategoryById;

public class GetServiceCategoryByIdQueryHandler(
    IRepository<ServiceCategory> repository, 
    ILogger<GetServiceCategoryByIdQueryHandler> logger, 
    IMapper mapper, ITimeZoneConverter timeZoneConverter
    ) : IRequestHandler<GetServiceCategoryByIdQuery, GetServiceCategoryByIdQueryResponse?>
{
    public async Task<GetServiceCategoryByIdQueryResponse?> Handle(GetServiceCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting ServiceCategory with Id {ServiceCategoryId}", request.Id);

        var serviceCategory = await repository.GetByIdAsync(request.Id)
                        ?? throw new NotFoundException(nameof(ServiceCategory), request.Id.ToString());

        var serviceCategoryDto = mapper.Map<GetServiceCategoryByIdQueryResponse?>(serviceCategory);

        return timeZoneConverter.ConvertUtcToLocal(serviceCategoryDto);
    }
}
