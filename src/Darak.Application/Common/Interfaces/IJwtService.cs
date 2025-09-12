
namespace Darak.Application.Common.Interfaces;

public interface IJwtService
{
    Task<string> GenerateTokenAsync(Guid userId, string email, IEnumerable<string> roles);

}
