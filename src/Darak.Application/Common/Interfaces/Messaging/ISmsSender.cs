namespace Darak.Application.Common.Interfaces.Messaging;

public interface ISmsSender
{
    Task SendAsync(string to, string message, CancellationToken ct = default);
}
