using MediatR;
using Darak.Application.Common;
using Darak.Application.Common.Models;
using Darak.Application.Common.Dtos.Users;

namespace Darak.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<PagedResult<UserDto>>
{
    public string? SearchPhrase { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}


