using Darak.Application.Features.Users.Commands.LoginWithOtp;
using FluentValidation;

namespace Darak.Application.Features.Users.Commands.VerifyLoginOtp;

public class VerifyLoginOtpCommandValidator : AbstractValidator<VerifyLoginOtpCommand>
{
    public VerifyLoginOtpCommandValidator()
    {
        RuleFor(dto => dto.PhoneNumber)
          .Length(10, 15)
          .WithMessage("Please provide a valid phone number with country code (10-15 digits)");

        RuleFor(dto => dto.Code)
            .NotEmpty()
            .WithMessage("OTP code must not be empty");
    }
}