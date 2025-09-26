using System.Linq.Expressions;
using System.Security.Claims;
using Darak.Application.Common.Dtos.Users;
using Darak.Application.Common.Models;
using Darak.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Darak.Application.Common.Interfaces.Security;

/// <summary>
/// Application-facing user gateway that hides ASP.NET Identity and EF Core from Application layer.
/// Implemented in Infrastructure (wrapping UserManager/RoleManager/DbContext).
/// </summary>
public interface IUserService
{

    Task<AppUser> CreateClientUserAsync(RegisterClientUserRequest request, CancellationToken ct = default);
    Task<AppUser> CreateContractorUserAsync(RegisterContractorUserRequest request, CancellationToken ct = default);
    Task<IdentityResult> UpdateClientUserAsync(UpdateClientUserRequest user, CancellationToken cancellationToken = default);
    Task<IdentityResult> UpdateContractorUserAsync(UpdateContractorUserRequest user, CancellationToken cancellationToken = default);
    Task<AppUser?> FindByEmailAsync(string email, CancellationToken ct = default);
    Task<AppUser?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<AppUser?> GetByIdAsync(Guid Id, Expression<Func<AppUser, object>>[]? includes = null, CancellationToken cancellationToken = default);
    Task<AppUser?> GetByClaimsPrincipalAsync(ClaimsPrincipal user, Expression<Func<AppUser, object>>[]? includes = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> GetRolesAsync(Guid userId, CancellationToken ct = default);
    Task<List<string>> GetUserRolesAsync(AppUser user, CancellationToken ct = default);
    Task<bool> AddToRoleAsync(Guid userId, string roleName, CancellationToken ct = default);
    Task<PagedResult<AppUser>> GetPagedAsync(int pageNumber, int pageSize, string? search = null, Expression<Func<AppUser, object>>[]? includes = null,
  CancellationToken ct = default);
    Task<bool> ConfirmEmailAsync(Guid userId, string token, CancellationToken ct = default);
    Task<bool> ConfirmEmailAsync(string email, string token, CancellationToken ct = default);
    Task<string?> GenerateEmailConfirmationTokenAsync(Guid userId, CancellationToken ct = default);
    Task<string?> GenerateEmailConfirmationTokenAsync(string email, CancellationToken ct = default);
    Task<bool> IsEmailConfirmedAsync(Guid userId, CancellationToken ct = default);
    Task<bool> IsEmailConfirmedAsync(string email, CancellationToken ct = default);
    Task<string?> GeneratePasswordResetTokenAsync(Guid userId, CancellationToken ct = default);
    Task<string?> GeneratePasswordResetTokenAsync(string email, CancellationToken ct = default);
    Task<bool> ValidateCredentialsAsync(string email, string password, bool lockoutOnFailure, CancellationToken ct = default);
    Task<(int UserTypeValue, string UserTypeName)> GetUserTypeAsync(Guid userId, CancellationToken ct = default);
    Task<bool> ResetPasswordAsync(string email, string token, string newPassword, CancellationToken ct = default);
    Task<bool> RemoveFromRoleAsync(string email, string roleName, CancellationToken ct = default);
    Task<string> NormalizePhoneAsync(string phone, CancellationToken ct);
    Task<AppUser?> FindByPhoneAsync(string phone, CancellationToken ct);
    Task<AppUser> CreateUserAsync(AppUser user, CancellationToken ct);
    Task MarkPhoneConfirmedAsync(Guid userId, CancellationToken ct);
}
