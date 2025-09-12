using AutoMapper;
using Darak.Application.Common.Interfaces;
using Darak.Domain.Entities;
using Darak.Domain.Exceptions;
using MediatR;

namespace Darak.Application.Features.ServiceCategories.Commands.CreateServiceCategory;

public class CreateServiceCategoryCommandHandler(
    IMapper mapper,
    IRepository<ServiceCategory> repository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<CreateServiceCategoryCommand, Guid>
{
    public async Task<Guid> Handle(CreateServiceCategoryCommand request, CancellationToken cancellationToken)
    {

        var existingServiceCategory = await repository.AnyAsync(e => e.NameEn == request.NameEn || e.NameAr == request.NameAr, cancellationToken);

        if (existingServiceCategory)
        {
            throw new AlreadyExistsException($"NameEn:{request.NameEn} Or NameAr:{request.NameAr}");
        }


        var serviceCategory = mapper.Map<ServiceCategory>(request);


        await repository.AddAsync(serviceCategory, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return serviceCategory.Id;
    }
}
