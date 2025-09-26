using FluentValidation;

namespace Darak.Application.Features.Users.Commands.RequestRegistrationOtp;

public class RequestRegistrationOtpCommandValidator : AbstractValidator<RequestRegistrationOtpCommand>
{
    public RequestRegistrationOtpCommandValidator()
    {
        RuleFor(dto => dto.PhoneNumber)
          .Length(10, 15)
          .WithMessage("Please provide a valid phone number with country code (10-15 digits)");
    }
}