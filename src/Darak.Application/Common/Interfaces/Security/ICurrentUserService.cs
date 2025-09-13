using System.Security.Claims;

namespace Darak.Application.Common.Interfaces.Security;

/// <summary>
/// Framework-agnostic current user abstraction.
/// Implement it in the API/Infrastructure by reading from HttpContext or the hosting environment.
/// </summary>
public interface ICurrentUserService
{
    Guid UserId { get; }
    string UserName { get; }
    string Email { get; }
    bool IsAuthenticated { get; }
    IEnumerable<string> Roles { get; }
    IEnumerable<Claim> Claims { get; }
    bool IsInRole(string role);
    bool HasClaim(string claimType, string claimValue);
}
