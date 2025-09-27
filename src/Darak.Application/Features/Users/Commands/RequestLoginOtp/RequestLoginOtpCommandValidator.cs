using FluentValidation;

namespace Darak.Application.Features.Users.Commands.RequestLoginOtp;

public class RequestLoginOtpCommandValidator : AbstractValidator<RequestLoginOtpCommand>
{
    public RequestLoginOtpCommandValidator()
    {
        RuleFor(dto => dto.PhoneNumber)
          .Length(10, 15)
          .WithMessage("Please provide a valid phone number with country code (10-15 digits)");
    }
}