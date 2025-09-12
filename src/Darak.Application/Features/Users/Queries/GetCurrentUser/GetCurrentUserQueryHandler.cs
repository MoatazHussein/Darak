using AutoMapper;
using MediatR;
using Darak.Application.Common.Interfaces;
using Darak.Domain.Exceptions;
using Darak.Application.Common.Interfaces.Security;
using Darak.Application.Common.Dtos.Users;

namespace Darak.Application.Features.Users.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler(
    IMapper mapper,
    ITimeZoneConverter timeZoneConverter,
    IUserService userService
) : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await userService.GetByClaimsPrincipalAsync(
            request.User,
            [u => u.ServiceCategory!],
            cancellationToken);

        if (user == null)
            throw new UnAuthorizedAccessException("User is not authenticated.");

        var dto = mapper.Map<UserDto>(user);

        dto.Roles = await userService.GetUserRolesAsync(user, cancellationToken);

        return timeZoneConverter.ConvertUtcToLocal(dto);
    }
}
