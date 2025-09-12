using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Darak.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Darak.Infrastructure.Services.Identity;

public class JwtService(IConfiguration config) : IJwtService
{
    public Task<string> GenerateTokenAsync(Guid userId, string email, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email ?? string.Empty),
        };

        if (roles != null)
        {
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: creds
        );

        var encoded = new JwtSecurityTokenHandler().WriteToken(token);
        return Task.FromResult(encoded);

    }
}
