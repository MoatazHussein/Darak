using AutoMapper;
using Darak.Application.Common.Dtos.Users;
using Darak.Application.Common.Interfaces;
using Darak.Application.Common.Interfaces.Security;
using Darak.Domain.Constants;
using Darak.Domain.Entities;
using Darak.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Darak.Application.Features.Users.Commands.RegisterContractor;

public class RegisterContractorCommandHandler(
    IUserService userService,
    IRepository<ServiceCategory> serviceCategoryRepository,
    IMapper mapper,
    IMailService mailService,
    IConfiguration config
    ) : IRequestHandler<RegisterContractorCommand, Guid>
{
    public async Task<Guid> Handle(RegisterContractorCommand request, CancellationToken cancellationToken)
    {
        var existingServiceCategory = await serviceCategoryRepository.AnyAsync(sc => sc.Id == request.ServiceCategoryId, cancellationToken);

        if (!existingServiceCategory)
            throw new NotFoundException(nameof(ServiceCategory), request.ServiceCategoryId.ToString());

        var registerUserRequest = mapper.Map<RegisterContractorUserRequest>(request);

        var user = await userService.CreateContractorUserAsync(registerUserRequest, cancellationToken);

        await userService.AddToRoleAsync(user.Id, UserRoles.Contractor, cancellationToken);

        var token = await userService.GenerateEmailConfirmationTokenAsync(user.Id, cancellationToken);

        var confirmUrl = $"{config["App:ConfirmEmailApiUrl"]}?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(token!)}";

        var emailBody = $"<p>Please confirm your email by clicking <a href='{confirmUrl}'>here</a>.</p>";

        await mailService.SendEmailAsync(user.Email, "Confirm your email", emailBody, null);


        return user.Id;
    }
}
