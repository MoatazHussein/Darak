using Microsoft.AspNetCore.Http;

namespace Darak.Application.Common.Interfaces;

public interface IStorageService
{
    Task EnsureImageDirectoryExistsAsync(CancellationToken cancellationToken = default);
    Task<string> SaveImageAsync(IFormFile file, CancellationToken cancellationToken);

}
