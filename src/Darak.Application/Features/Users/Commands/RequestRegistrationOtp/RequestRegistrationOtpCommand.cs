using MediatR;

namespace Darak.Application.Features.Users.Commands.RequestRegistrationOtp;

public record RequestRegistrationOtpCommand(string PhoneNumber) : IRequest;

