using MediatR;
using Darak.Application.Common.Interfaces;
using Darak.Application.Common.Interfaces.Security;
using Darak.Domain.Exceptions;
using Darak.Domain.Entities;
using Darak.Application.Common.Dtos.Users;

namespace Darak.Application.Features.Users.Commands.Login;

public class LoginCommandHandler(IUserService userService, IJwtService jwtService) : IRequestHandler<LoginCommand, LoginResponseDto>
{
    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userService.FindByEmailAsync(request.Email, cancellationToken);
        if (user is null)
            throw new NotFoundException(nameof(AppUser),$"{request.Email}");

        var ok = await userService.ValidateCredentialsAsync(request.Email, request.Password, lockoutOnFailure: true, cancellationToken);
        if (!ok)
            throw new UnAuthorizedAccessException("Invalid credentials.");

        var roles = await userService.GetRolesAsync(user.Id, cancellationToken);

        var token = await jwtService.GenerateTokenAsync(user, roles);

        var (userTypeValue, userTypeName) = await userService.GetUserTypeAsync(user.Id, cancellationToken);

        return new LoginResponseDto
        {
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber!,
            Token = token,
            UserTypeValue = userTypeValue,
            UserTypeName = userTypeName,
            Roles = roles
        };
    }
}
