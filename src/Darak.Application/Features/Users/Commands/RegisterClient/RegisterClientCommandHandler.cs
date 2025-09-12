using AutoMapper;
using Darak.Application.Common.Dtos.Users;
using Darak.Application.Common.Interfaces;
using Darak.Application.Common.Interfaces.Security;
using Darak.Domain.Constants;
using Darak.Domain.Entities;
using Darak.Domain.Enums;
using Darak.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Darak.Application.Features.Users.Commands.RegisterClient;

public class RegisterClientCommandHandler(
    IUserService userService,
    IMailService mailService,
    IMapper mapper,
    IConfiguration config
    ) : IRequestHandler<RegisterClientCommand, Guid>
{
    public async Task<Guid> Handle(RegisterClientCommand request, CancellationToken cancellationToken)
    {
        //var user = new AppUser
        //{
        //    FirstName = request.FirstName,
        //    LastName = request.LastName,
        //    UserName = request.Email,
        //    Email = request.Email,
        //    PhoneNumber = request.PhoneNumber,
        //    Location = request.Location,
        //    UserType = UserType.Client
        //};

        //var result = await userService.CreateAsync(user, request.Password, cancellationToken);

        var registerUserRequest = mapper.Map<RegisterClientUserRequest>(request);

        var user = await userService.CreateClientUserAsync(registerUserRequest, cancellationToken);

        await userService.AddToRoleAsync(user.Id, UserRoles.Client, cancellationToken);

        var token = await userService.GenerateEmailConfirmationTokenAsync(user.Id, cancellationToken);

        var confirmUrl = $"{config["App:ConfirmEmailApiUrl"]}?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(token!)}";

        var emailBody = $"<p>Please confirm your email by clicking <a href='{confirmUrl}'>here</a>.</p>";

        await mailService.SendEmailAsync(user.Email, "Confirm your email", emailBody, null);


        return user.Id;
    }
}
