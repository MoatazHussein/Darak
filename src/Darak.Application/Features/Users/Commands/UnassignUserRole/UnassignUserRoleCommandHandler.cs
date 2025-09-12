using MediatR;
using Microsoft.Extensions.Logging;
using Darak.Application.Common.Interfaces.Security;
using Darak.Domain.Exceptions;

namespace Darak.Application.Features.Users.Commands.UnassignUserRole;

public class UnassignUserRoleCommandHandler(ILogger<UnassignUserRoleCommandHandler> logger, IUserService userService) : IRequestHandler<UnassignUserRoleCommand>
{
    public async Task Handle(UnassignUserRoleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Unassigning user role: {@Request}", request);

        var ok = await userService.RemoveFromRoleAsync(request.UserEmail, request.RoleName, cancellationToken);
        if (!ok)
            throw new NotFoundException("User or Role", $"{request.UserEmail} / {request.RoleName}");
    }
}
