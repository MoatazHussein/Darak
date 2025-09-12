using Darak.Application.Common.Interfaces;
using Darak.Infrastructure.Persistence;

namespace Darak.Infrastructure.Services.UnitOfWork;

public class EfUnitOfWork(AppDbContext _context) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}

