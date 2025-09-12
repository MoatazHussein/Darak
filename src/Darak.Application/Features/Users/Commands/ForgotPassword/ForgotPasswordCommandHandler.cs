using Darak.Application.Common.Interfaces;
using Darak.Application.Common.Interfaces.Security;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Darak.Application.Features.Users.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler(
    IUserService userService,
    IMailService mailService,
    IConfiguration config) : IRequestHandler<ForgotPasswordCommand, bool>
{
    public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userService.FindByEmailAsync(request.Email, cancellationToken);
        if (user is null || !await userService.IsEmailConfirmedAsync(request.Email, cancellationToken))
        {
            return true;
        }

        var token = await userService.GeneratePasswordResetTokenAsync(user.Id, cancellationToken);

        var resetUrl = $"{config["App:ResetPasswordUrl"]}?email={Uri.EscapeDataString(user.Email!)}&token={Uri.EscapeDataString(token)}";

        var emailBody = $@"
            <p>Hi,</p>
            <p>You requested to reset your password. Click the link below:</p>
            <p><a href='{resetUrl}'>Reset your password</a></p>
            <p>If you didn’t request this, just ignore this email.</p>";

        await mailService.SendEmailAsync(user.Email!, "Password Reset Request", emailBody, null);

        return true;
    }
}
