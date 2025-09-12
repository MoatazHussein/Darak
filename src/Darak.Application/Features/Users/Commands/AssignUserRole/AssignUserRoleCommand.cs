using MediatR;

namespace Darak.Application.Features.Users.Commands.AssignUserRole;

public class AssignUserRoleCommand : IRequest<bool>
{
    public string Email { get; set; } = default!;
    public string RoleName { get; set; } = default!;
}