using System.Linq.Expressions;
using AutoMapper;
using Darak.Application.Common.Dtos.Users;
using Darak.Application.Common.Interfaces;
using Darak.Application.Common.Interfaces.Security;
using Darak.Application.Common.Models;
using Darak.Domain.Entities;
using MediatR;

namespace Darak.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler(
    IUserService userService,
    IMapper mapper,
    ITimeZoneConverter timeZoneConverter
) : IRequestHandler<GetAllUsersQuery, PagedResult<UserDto>>
{
    public async Task<PagedResult<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var includes = new Expression<Func<AppUser, object>>[]
            {
                u => u.ServiceCategory!,
            };

        var pagedUsers = await userService.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            request.SearchPhrase,
            includes,
            cancellationToken);

        var userDtos = new List<UserDto>();
        foreach (var user in pagedUsers.Items)
        {
            var dto = mapper.Map<UserDto>(user);
            dto.Roles = await userService.GetUserRolesAsync(user, cancellationToken);
            userDtos.Add(dto);
        }

        var result = new PagedResult<UserDto>(userDtos, pagedUsers.TotalItemsCount, request.PageSize, request.PageNumber);

        return timeZoneConverter.ConvertUtcToLocal(result);

    }
}


