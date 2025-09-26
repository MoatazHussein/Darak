using Darak.Application.Features.Users.Commands.VerifyClientRegistrationOtp;
using FluentValidation;

namespace Darak.Application.Features.Users.Commands.VerifyClientRegistrationOtp;

public class VerifyClientRegistrationOtpCommandValidator : AbstractValidator<VerifyClientRegistrationOtpCommand>
{
    public VerifyClientRegistrationOtpCommandValidator()
    {
        RuleFor(dto => dto.FirstName)
           .MaximumLength(50);

        RuleFor(dto => dto.LastName)
            .MaximumLength(50);

        RuleFor(dto => dto.Password)
            .Cascade(CascadeMode.Stop)
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .MaximumLength(100).WithMessage("Password must not exceed 100 characters")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one number")
            .Matches(@"[!@#$%^&*(),.?"":{}|<>]").WithMessage("Password must contain at least one special character")
            .Must(p => p == null || !p.Any(char.IsWhiteSpace)).WithMessage("Password cannot contain whitespace")
            .When(x => !string.IsNullOrWhiteSpace(x.Password));


        RuleFor(dto => dto.PhoneNumber)
          .Length(10, 15)
          .WithMessage("Please provide a valid phone number with country code (10-15 digits)");

        RuleFor(dto => dto.Location)
            .MaximumLength(200)
            .When(dto => !string.IsNullOrEmpty(dto.Location))
            .WithMessage("Location must not exceed 200 characters");


    }
}