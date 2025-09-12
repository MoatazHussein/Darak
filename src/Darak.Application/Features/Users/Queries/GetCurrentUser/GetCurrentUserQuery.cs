using System.Security.Claims;
using MediatR;
using Darak.Application.Common.Dtos.Users;

namespace Darak.Application.Features.Users.Queries.GetCurrentUser;

public class GetCurrentUserQuery(ClaimsPrincipal user) : IRequest<UserDto>
{
    public ClaimsPrincipal User { get; set; } = user;
}
