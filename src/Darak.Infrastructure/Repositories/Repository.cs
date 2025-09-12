using System.Linq.Expressions;
using Darak.Application.Common.Interfaces;
using Darak.Application.Common.Models;
using Darak.Infrastructure.Extensions;
using Darak.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Darak.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
    public Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();

        // Apply eager loading for includes
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        // Assume entities have an Id property of type object
        return query.FirstOrDefaultAsync(e => EF.Property<object>(e, "Id") == id, cancellationToken);
    }
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().AnyAsync(predicate, cancellationToken);
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default,
       params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;

        foreach (var include in includes)
            query = query.Include(include);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<T>, int)> GetAllMatchingAsync(QueryParameters<T> parameters, CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet;

        // Apply filter
        if (parameters.Filter != null)
            query = query.Where(parameters.Filter);

        // Apply includes
        foreach (var include in parameters.Includes)
            query = query.Include(include);

        // Total count before paging
        var totalItems = await query.CountAsync(cancellationToken);

        // Apply sorting
        if (parameters.OrderBy is not null)
        {
            query = query.ApplyOrdering(parameters.OrderBy, parameters.Descending);
        }

        // Apply paging
        var skip = (parameters.PageNumber - 1) * parameters.PageSize;
        var items = await query.Skip(skip).Take(parameters.PageSize).ToListAsync(cancellationToken);


        return (items, totalItems);

    }


    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        => await _dbSet.AddAsync(entity, cancellationToken);

    public Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity); 
        return Task.CompletedTask; 
    }


    public Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);


}
