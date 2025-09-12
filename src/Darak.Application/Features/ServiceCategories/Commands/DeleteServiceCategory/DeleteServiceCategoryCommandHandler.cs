using Darak.Application.Common.Interfaces;
using Darak.Domain.Entities;
using Darak.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Darak.Application.Features.ServiceCategories.Commands.DeleteServiceCategory;
internal class DeleteServiceCategoryCommandHandler(
    ILogger<DeleteServiceCategoryCommandHandler> logger,
    IRepository<ServiceCategory> repository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<DeleteServiceCategoryCommand>
{
    public async Task Handle(DeleteServiceCategoryCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting store Category with id: {serviceCategoryId}", request.Id);
        var serviceCategory = await repository.GetByIdAsync(request.Id);
        if (serviceCategory is null)

            throw new NotFoundException(nameof(ServiceCategory), request.Id.ToString());

        await repository.DeleteAsync(serviceCategory);

        await unitOfWork.SaveChangesAsync(cancellationToken);

    }
}
