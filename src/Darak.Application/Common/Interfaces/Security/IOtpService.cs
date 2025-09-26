namespace Darak.Application.Common.Interfaces.Security;
public interface IOtpService
{
    Task<string> GenerateAsync(string phone, string purpose, CancellationToken ct = default);
    Task<bool> VerifyAsync(string phone, string purpose, string code, CancellationToken ct = default);
}
