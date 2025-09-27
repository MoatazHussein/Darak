using Darak.Domain.Entities;

namespace Darak.Application.Common.Interfaces;

public interface IJwtService
{
    Task<string> GenerateTokenAsync(AppUser user, IEnumerable<string> roles);

}
