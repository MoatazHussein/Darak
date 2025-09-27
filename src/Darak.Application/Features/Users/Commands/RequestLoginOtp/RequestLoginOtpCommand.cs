using MediatR;

namespace Darak.Application.Features.Users.Commands.RequestLoginOtp;

public record RequestLoginOtpCommand(string PhoneNumber) : IRequest;

