using Darak.Application.Common.Dtos.Users;
using Darak.Application.Common.Interfaces;
using Darak.Application.Common.Interfaces.Security;
using Darak.Domain.Constants;
using Darak.Domain.Entities;
using Darak.Domain.Exceptions;
using MediatR;

namespace Darak.Application.Features.Users.Commands.VerifyLoginOtp;

public class VerifyLoginOtpCommandHandler(
    IUserService userService,
    IJwtService jwtService,
    IOtpService otpService
    ) : IRequestHandler<VerifyLoginOtpCommand, LoginResponseDto>
{
    public async Task<LoginResponseDto> Handle(VerifyLoginOtpCommand request, CancellationToken cancellationToken)
    {
        var user = await userService.FindByPhoneAsync(request.PhoneNumber, cancellationToken);
        if (user is null)
            throw new NotFoundException(nameof(AppUser),$"{request.PhoneNumber}");

        //  Verify OTP
        var ok = await otpService.VerifyAsync(request.PhoneNumber, OtpPurpose.Login, request.Code, cancellationToken);

        if (!ok) throw new BusinessRuleException("Invalid or expired OTP.");

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
