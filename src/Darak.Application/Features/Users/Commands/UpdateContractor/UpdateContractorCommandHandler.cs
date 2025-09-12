using AutoMapper;
using Darak.Application.Common.Dtos.Users;
using Darak.Application.Common.Interfaces;
using Darak.Application.Common.Interfaces.Security;
using Darak.Domain.Entities;
using Darak.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Darak.Application.Features.Users.Commands.UpdateContractor;

public class UpdateContractorCommandHandler(
    IUserService userService,
    IRepository<ServiceCategory> serviceCategoryRepository,
    IMapper mapper
    ) :  IRequestHandler<UpdateContractorCommand, IdentityResult>
{
    public async Task<IdentityResult> Handle(UpdateContractorCommand request, CancellationToken cancellationToken)
    {

        var existingServiceCategory = await serviceCategoryRepository.AnyAsync(sc => sc.Id == request.ServiceCategoryId, cancellationToken);

        if (!existingServiceCategory)
            throw new NotFoundException(nameof(ServiceCategory), request.ServiceCategoryId.ToString());


        var updateUserRequest = mapper.Map<UpdateContractorUserRequest>(request);

        return await userService.UpdateContractorUserAsync(updateUserRequest, cancellationToken);
    }
}
