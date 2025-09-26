using System.Security.Claims;
using Darak.Application.Common.Interfaces.Security;
using Darak.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Darak.Infrastructure.Services.Security;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
            {
               throw new UnAuthorizedAccessException("User is not authenticated.");
            }

            return Guid.TryParse(userIdString, out var userId) ? 
                userId : throw new AppException("Invalid user identifier format in authentication"); ;
        }
    }

    public string UserName
    {
        get
        {
            var userName = _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(userName))
            {
                throw new UnAuthorizedAccessException("User is not authenticated or username is missing.");
            }

            return userName;
        }
    }

    public string Email
    {
        get
        {
            var userEmail = _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                throw new UnAuthorizedAccessException("User is not authenticated or username is missing.");
            }

            return userEmail;
        }
    }
    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public IEnumerable<string> Roles =>
        _httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role)?
            .Select(c => c.Value) ?? Enumerable.Empty<string>();

    public IEnumerable<Claim> Claims =>
        _httpContextAccessor.HttpContext?.User?.Claims ?? Enumerable.Empty<Claim>();

    public bool IsInRole(string role)
    {
        if (string.IsNullOrEmpty(role))
            return false;

        return _httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;
    }

    public bool HasClaim(string claimType, string claimValue)
    {
        if (string.IsNullOrEmpty(claimType))
            return false;

        return _httpContextAccessor.HttpContext?.User?.HasClaim(claimType, claimValue) ?? false;
    }
}
