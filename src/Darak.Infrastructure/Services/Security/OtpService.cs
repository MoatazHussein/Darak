using Darak.Application.Common.Interfaces.Security;
using System.Security.Cryptography;
using System.Text;
using Darak.Domain.Entities;
using Darak.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Darak.Infrastructure.Services.Security;

internal sealed class OtpServiceDb(AppDbContext db, IConfiguration cfg) : IOtpService
{
    public async Task<string> GenerateAsync(string phoneNumber, string purpose, CancellationToken ct = default)
    {
        int length = cfg.GetValue<int?>("Otp:Length") ?? 6;
        int ttlSeconds = cfg.GetValue<int?>("Otp:TtlSeconds") ?? 300;
        int maxAttempts = cfg.GetValue<int?>("Otp:MaxAttempts") ?? 5;

        var code = GenerateNumeric(length);
        var salt = Convert.ToHexString(RandomNumberGenerator.GetBytes(16));
        var hash = Hash(code, salt);
        var now = DateTime.UtcNow;

        // Invalidate any previous active
        await db.OtpChallenges
            .Where(x => x.PhoneNumber == phoneNumber && x.Purpose == purpose && !x.Consumed && x.ExpiresAtUtc > now)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.ExpiresAtUtc, now), ct);

        db.OtpChallenges.Add(new OtpChallenge
        {
            PhoneNumber = phoneNumber,
            Purpose = purpose,
            CodeHash = hash,
            Salt = salt,
            ExpiresAtUtc = now.AddSeconds(ttlSeconds),
            MaxAttempts = maxAttempts,
            AttemptCount = 0,
            Consumed = false,
        });

        await db.SaveChangesAsync(ct);
        return code; 
    }

    public async Task<bool> VerifyAsync(string phoneNumber, string purpose, string code, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var challenge = await db.OtpChallenges
            .Where(x => x.PhoneNumber == phoneNumber && x.Purpose == purpose && !x.Consumed && x.ExpiresAtUtc > now)
            .OrderByDescending(x => x.CreatedAtUtc)
            .FirstOrDefaultAsync(ct);

        if (challenge is null) return false;
        if (challenge.AttemptCount >= challenge.MaxAttempts) return false;

        var expected = Hash(code, challenge.Salt);
        var ok = CryptographicOperations.FixedTimeEquals(
            Convert.FromHexString(expected), Convert.FromHexString(challenge.CodeHash));

        challenge.AttemptCount++;
        if (ok)
        {
            challenge.Consumed = true;
        }

        await db.SaveChangesAsync(ct);
        return ok;
    }

    private static string GenerateNumeric(int len)
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[len];
        rng.GetBytes(bytes);
        var chars = new char[len];
        for (int i = 0; i < len; i++) chars[i] = (char)('0' + (bytes[i] % 10));
        if (chars[0] == '0') chars[0] = '1';
        return new string(chars);
    }

    private static string Hash(string code, string salt)
    {
        using var h = new HMACSHA256(Convert.FromHexString(salt));
        return Convert.ToHexString(h.ComputeHash(Encoding.UTF8.GetBytes(code)));
    }
}
