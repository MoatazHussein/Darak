using System.Linq.Expressions;
using Darak.Application.Common.Models;

namespace Darak.Application.Common.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default,  params Expression<Func<T, object>>[] includes);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default,  params Expression<Func<T, object>>[] includes);
    Task<(IEnumerable<T>, int)> GetAllMatchingAsync(QueryParameters<T> parameters, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
