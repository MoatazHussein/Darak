using Darak.Application.Common.Interfaces.Messaging;
using Darak.Application.Common.Interfaces.Security;
using Darak.Domain.Constants;
using Darak.Domain.Exceptions;
using MediatR;

namespace Darak.Application.Features.Users.Commands.RequestLoginOtp;

public class RequestLoginOtpCommandHandler(
    IUserService userService,
    IOtpService otpService,
    ISmsSender sms
    ) : IRequestHandler<RequestLoginOtpCommand>
{
    public async Task Handle(RequestLoginOtpCommand req, CancellationToken ct)
    {
        var existing = await userService.FindByPhoneAsync(req.PhoneNumber, ct);

        if (existing is  null)
        {
            throw new BusinessRuleException("this phone does not exists");
        }

        var code = await otpService.GenerateAsync(req.PhoneNumber, OtpPurpose.Login , ct);

        await sms.SendAsync(req.PhoneNumber, $"Your verification code is: {code}", ct);

    }

   
}
