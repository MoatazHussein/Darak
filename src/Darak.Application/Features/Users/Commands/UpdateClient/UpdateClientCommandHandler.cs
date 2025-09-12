using AutoMapper;
using Darak.Application.Common.Dtos.Users;
using Darak.Application.Common.Interfaces.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Darak.Application.Features.Users.Commands.UpdateClient;

public class UpdateClientCommandHandler(
    IUserService userService,
    IMapper mapper
    ) :  IRequestHandler<UpdateClientCommand, IdentityResult>
{
    public async Task<IdentityResult> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var updateUserRequest = mapper.Map<UpdateClientUserRequest>(request);

        return await userService.UpdateClientUserAsync(updateUserRequest, cancellationToken);
    }
}
