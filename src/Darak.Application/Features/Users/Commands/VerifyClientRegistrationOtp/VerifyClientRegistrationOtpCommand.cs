using MediatR;

namespace Darak.Application.Features.Users.Commands.VerifyClientRegistrationOtp;

public class VerifyClientRegistrationOtpCommand : IRequest<bool>
{
    public string? FirstName { get; set; } = null!;
    public string? LastName { get; set; } = null!;
    public string? Password { get; set; } = null!;
    public string? Location { get; set; }
    public required string PhoneNumber { get; set; } 
    public required string Code { get; set; }
}
