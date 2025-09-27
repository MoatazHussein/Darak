using Darak.Application.Common.Dtos.Users;
using MediatR;

namespace Darak.Application.Features.Users.Commands.VerifyLoginOtp;
public record VerifyLoginOtpCommand(string PhoneNumber,string Code) :IRequest<LoginResponseDto>;

