using System.Linq.Expressions;
using System.Security.Claims;
using Darak.Application.Common.Dtos.Users;
using Darak.Application.Common.Interfaces.Security;
using Darak.Application.Common.Models;
using Darak.Domain.Entities;
using Darak.Domain.Enums;
using Darak.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Darak.Infrastructure.Services.Security;

public sealed class UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signIn) : IUserService
{
    // -------------------- User Commands --------------------

    public async Task<AppUser> CreateClientUserAsync(RegisterClientUserRequest request, CancellationToken ct = default)
    {

        var existing = await userManager.FindByEmailAsync(request.Email);
        if (existing is not null)
            throw new AlreadyExistsException(request.Email!);

        var user = new AppUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.Email,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Location = request.Location,
            UserType = UserType.Client,
            CreatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new ApplicationException(string.Join(", ", result.Errors.Select(e => e.Description)));

        return new AppUser { Id = user.Id, Email = user.Email };
    }


    public async Task<AppUser> CreatePhoneOnlyUserAsync2(AppUser user, CancellationToken ct)
    {
        // Create without password (passwordless / OTP-based)
        var result = await userManager.CreateAsync(user);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

        return user;
    }


    public async Task<AppUser> CreateContractorUserAsync(RegisterContractorUserRequest request, CancellationToken ct = default)
    {

        var existing = await userManager.FindByEmailAsync(request.Email);
        if (existing is not null)
            throw new AlreadyExistsException(request.Email!);

        var user = new AppUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.Email,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Location = request.Location,
            ServiceCategoryId = request.ServiceCategoryId,
            UserType = UserType.Contractor,
            CreatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new ApplicationException(string.Join(", ", result.Errors.Select(e => e.Description)));

        return new AppUser { Id = user.Id, Email = user.Email };
    }

    public async Task<IdentityResult> UpdateClientUserAsync(UpdateClientUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            throw new NotFoundException("User Don't Exist", $"{request.UserId}");
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        user.Location = request.Location;
        user.UpdatedAt = DateTime.UtcNow;

        var result = await userManager.UpdateAsync(user);

        return result;
    }

    public async Task<IdentityResult> UpdateContractorUserAsync(UpdateContractorUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            throw new NotFoundException("User Don't Exist", $"{request.UserId}");
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        user.Location = request.Location;
        user.ServiceCategoryId = request.ServiceCategoryId;
        user.UpdatedAt = DateTime.UtcNow;

        var result = await userManager.UpdateAsync(user);

        return result;
    }

    // -------------------- Queries --------------------
    public async Task<AppUser?> FindByEmailAsync(string email, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(email)) return null;

        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return null;

        return new AppUser
        {
            Id = user.Id,
            Email = user.Email,
        };
    }

    public async Task<AppUser?> FindByIdAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        if (Id == Guid.Empty) return null;

        var user = await userManager.FindByIdAsync(Id.ToString());
        if (user is null) return null;

        return new AppUser
        {
            Id = user.Id,
            Email = user.Email,
        };
    }

    public async Task<AppUser?> GetByIdAsync(Guid Id, Expression<Func<AppUser, object>>[]? includes = null, CancellationToken cancellationToken = default)
    {
        IQueryable<AppUser> query = userManager.Users.AsNoTracking();

        // Apply includes if provided
        if (includes?.Length > 0)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        return await query.FirstOrDefaultAsync(u => u.Id == Id, cancellationToken);
    }

    public async Task<AppUser?> GetByClaimsPrincipalAsync(ClaimsPrincipal user, Expression<Func<AppUser, object>>[]? includes = null, CancellationToken cancellationToken = default)
    {
        // First get the user to extract the ID
        var baseUser = await userManager.GetUserAsync(user);
        if (baseUser == null)
            return null;

        // If no includes needed, return the base user
        if (includes?.Length == 0)
            return baseUser;

        // Otherwise, query with includes
        return await GetByIdAsync(baseUser.Id, includes, cancellationToken);
    }

    public async Task<List<string>> GetUserRolesAsync(AppUser user, CancellationToken ct = default)
    {
        var roles = await userManager.GetRolesAsync(user);
        return roles.ToList();
    }

    public async Task<PagedResult<AppUser>> GetPagedAsync(
       int pageNumber,
       int pageSize,
       string? search = null,
       Expression<Func<AppUser, object>>[]? includes = null,
       CancellationToken ct = default)
    {
        IQueryable<AppUser> query = userManager.Users.AsNoTracking();

        // Apply includes
        if (includes?.Length > 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(u => (u.Email ?? string.Empty).Contains(search));
        }

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderBy(u => u.Email)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(ct);

        return new PagedResult<AppUser>(items, total, pageSize, pageNumber);
    }


    // -------------------- Roles --------------------
    public async Task<IReadOnlyList<string>> GetRolesAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null) return Array.Empty<string>();
        var roles = await userManager.GetRolesAsync(user);
        return roles.ToArray();
    }
    public async Task<bool> AddToRoleAsync(Guid userId, string roleName, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null) return false;
        var res = await userManager.AddToRoleAsync(user, roleName);
        return res.Succeeded;
    }
    public async Task<bool> RemoveFromRoleAsync(string email, string roleName, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return false;
        var res = await userManager.RemoveFromRoleAsync(user, roleName);
        return res.Succeeded;
    }

    // -------- Auth / credentials --------
    public async Task<bool> ValidateCredentialsAsync(string email, string password, bool lockoutOnFailure, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return false;
        var result = await signIn.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
        return result.Succeeded;
    }

    // -------- User type --------
    public async Task<(int UserTypeValue, string UserTypeName)> GetUserTypeAsync(Guid userId, CancellationToken ct = default)
    {

        var user = await userManager.FindByIdAsync(userId.ToString());

        var roles = await GetRolesAsync(userId, ct);

        return ((int)user!.UserType, user.UserType.ToString());
    }

    // -------------------- Email Confirmation (by Id) --------------------
    public async Task<bool> ConfirmEmailAsync(Guid userId, string token, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null) return false;
        var result = await userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded;
    }

    public async Task<string?> GenerateEmailConfirmationTokenAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null) return null;
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        return token;
    }

    public async Task<bool> IsEmailConfirmedAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null) return false;
        return await userManager.IsEmailConfirmedAsync(user);
    }

    // -------------------- Email Confirmation (by Email) --------------------
    public async Task<bool> ConfirmEmailAsync(string email, string token, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return false;
        var result = await userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded;
    }

    public async Task<string?> GenerateEmailConfirmationTokenAsync(string email, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return null;
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        return token;
    }

    public async Task<bool> IsEmailConfirmedAsync(string email, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return false;
        return await userManager.IsEmailConfirmedAsync(user);
    }
    public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return false;
        var result = await userManager.ResetPasswordAsync(user, token, newPassword);
        return result.Succeeded;
    }


    // -------------------- Password Reset --------------------
    public async Task<string?> GeneratePasswordResetTokenAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null) return null;
        return await userManager.GeneratePasswordResetTokenAsync(user);
    }

    public async Task<string?> GeneratePasswordResetTokenAsync(string email, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) return null;
        return await userManager.GeneratePasswordResetTokenAsync(user);
    }

    public Task<string> NormalizePhoneAsync(string phone, CancellationToken ct)
    {
        // TODO: use libphonenumber for true E.164; here’s a simple fallback
        var p = phone.Trim().Replace(" ", "");
        return Task.FromResult(p);
    }

    public Task<AppUser?> FindByPhoneAsync(string phone, CancellationToken ct)
        => userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phone, ct);

    public async Task<AppUser> CreateUserAsync(AppUser user, CancellationToken ct)
    {
        // Create without password (passwordless / OTP-based)
        var result = await userManager.CreateAsync(user);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

        return user;
    }

    public async Task MarkPhoneConfirmedAsync(Guid userId, CancellationToken ct)
    {
        var user = await userManager.Users.FirstAsync(u => u.Id == userId, ct);
        if (!user.PhoneNumberConfirmed)
        {
            user.PhoneNumberConfirmed = true;
            user.UpdatedAt = DateTime.UtcNow;
            await userManager.UpdateAsync(user);
        }
    }
}
