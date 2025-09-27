using Darak.Application.Common.Dtos.Users;
using MediatR;

namespace Darak.Application.Features.Users.Commands.Login;

public class LoginCommand : IRequest<LoginResponseDto>
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}
