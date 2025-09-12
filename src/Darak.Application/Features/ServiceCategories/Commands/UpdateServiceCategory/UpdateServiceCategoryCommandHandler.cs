using AutoMapper;
using Darak.Application.Common.Interfaces;
using Darak.Domain.Entities;
using Darak.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Darak.Application.Features.ServiceCategories.Commands.UpdateServiceCategory;

public class UpdateServiceCategoryCommandHandler(
    ILogger<UpdateServiceCategoryCommandHandler> logger,
    IRepository<ServiceCategory> repository,
    IUnitOfWork unitOfWork,
    IMapper mapper
    ) : IRequestHandler<UpdateServiceCategoryCommand>
{
    public async Task Handle(UpdateServiceCategoryCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating ServiceCategory with id: {ServiceCategoryId} with {@UpdatedServiceCategory}", request.Id, request);

        var serviceCategory = await repository.GetByIdAsync(request.Id);

        if (serviceCategory is null)
            throw new NotFoundException(nameof(ServiceCategory), request.Id.ToString());

        var existingServiceCategory = await repository.AnyAsync(
            e => (e.NameEn == request.NameEn || e.NameAr == request.NameAr) && e.Id != request.Id  , cancellationToken);

        if (existingServiceCategory)
        {
            throw new AlreadyExistsException($"NameEn:{request.NameEn} Or NameAr:{request.NameAr}");
        }

        var updatedCategory = mapper.Map(request, serviceCategory);

        await repository.UpdateAsync(updatedCategory);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
