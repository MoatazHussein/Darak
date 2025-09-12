using Darak.Application.Common.Interfaces;

namespace Darak.Infrastructure.Startup;

public class EnsureStorageFoldersTask(IStorageService storageService) : IStartupTask
{
    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        await storageService.EnsureImageDirectoryExistsAsync(cancellationToken);
    }
}
