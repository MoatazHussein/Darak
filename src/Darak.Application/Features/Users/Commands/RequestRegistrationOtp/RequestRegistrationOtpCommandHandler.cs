using Darak.Application.Common.Interfaces.Messaging;
using Darak.Application.Common.Interfaces.Security;
using Darak.Domain.Constants;
using Darak.Domain.Exceptions;
using MediatR;

namespace Darak.Application.Features.Users.Commands.RequestRegistrationOtp;

public class RequestRegistrationOtpCommandHandler(
    IUserService userService,
    IOtpService otpService,
    ISmsSender sms
    ) : IRequestHandler<RequestRegistrationOtpCommand>
{
    public async Task Handle(RequestRegistrationOtpCommand req, CancellationToken ct)
    {
        var existing = await userService.FindByPhoneAsync(req.PhoneNumber, ct);

        if (existing is not null)
        {
            throw new BusinessRuleException("This User with associated phone already exists");
        }

        var code = await otpService.GenerateAsync(req.PhoneNumber,OtpPurpose.Register, ct);

        await sms.SendAsync(req.PhoneNumber, $"Your verification code is: {code}", ct);

    }
}
