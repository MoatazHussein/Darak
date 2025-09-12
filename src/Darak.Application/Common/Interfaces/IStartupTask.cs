namespace Darak.Application.Common.Interfaces;
public interface IStartupTask
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}
