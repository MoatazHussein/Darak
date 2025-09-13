using FluentValidation;

namespace Darak.Application.Features.Users.Commands.UpdateClient;

public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
    public UpdateClientCommandValidator()
    {
        RuleFor(dto => dto.FirstName).
            NotEmpty().WithMessage("Please provide a First Name")
            .Length(3, 50);

        RuleFor(dto => dto.LastName).
            NotEmpty().WithMessage("Please provide a Last Name")
            .Length(3, 50);

        RuleFor(dto => dto.PhoneNumber)
          .Length(10, 15)
          .When(dto => !string.IsNullOrEmpty(dto.PhoneNumber))
          .WithMessage("Please provide a valid phone number with country code (10-15 digits)");

        RuleFor(dto => dto.Location)
            .MaximumLength(200)
            .When(dto => !string.IsNullOrEmpty(dto.Location))
            .WithMessage("Location must not exceed 200 characters");

    }
}