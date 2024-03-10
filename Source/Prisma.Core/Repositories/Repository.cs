/*
    author: Richard Garcia (https://github.com/https-richardy)
    license: Licensed under the MIT License
*/

using System.Linq.Expressions;
using Prisma.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Prisma.Core.Data.Repositories;

/// <summary>
/// Base repository class providing extended query operations for entities using Entity Framework.
/// </summary>
/// <typeparam name="TEntity">The type of entity managed by the repository.</typeparam>
/// <typeparam name="TKey">The type of entity's primary key</typeparam>
/// <typeparam name="TDbContext">The type of the Entity Framework DbContext.</typeparam>
public abstract class Repository<TEntity, TKey, TDbContext> : MinimalRepository<TEntity, TKey, TDbContext>, IRepository<TEntity>
    where TEntity : class, IEntity<TKey>
    where TDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Repository{TEntity, TKey, TDbContext}"/> class.
    /// </summary>
    /// <param name="dbContext">The Entity Framework DbContext instance.</param>
    protected Repository(TDbContext dbContext)
        : base(dbContext) {  }

    /// <summary>
    /// Counts the total number of entities in the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, returning the total count of entities.</returns>
    public virtual async Task<int> CountAsync()
    {
        var result = await _dbContext.Set<TEntity>().CountAsync();
        return result;
    }

    /// <summary>
    /// Counts the number of entities in the repository based on the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to filter entities.</param>
    /// <returns>A task representing the asynchronous operation, returning the count of entities matching the predicate.</returns>
    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var result = await _dbContext.Set<TEntity>().CountAsync(predicate);
        return result;
    }

    /// <summary>
    /// Checks if an entity with the specified ID exists in the repository.
    /// </summary>
    /// <param name="id">The entity's indentifier.</param>
    /// <returns>A task representing the asynchronous operation, returning true if the entity exists, false otherwise.</returns>
    public virtual async Task<bool> ExistsAsync(object id)
    {
        var existingEntity = await _dbContext.Set<TEntity>().FindAsync(id);
        return existingEntity != null;
    }

    /// <summary>
    /// Finds all entities based on the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to filter entities.</param>
    /// <returns>A task representing the asynchronous operation, returning a collection of entities.</returns>
    public virtual async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbContext.Set<TEntity>().Where(predicate).ToListAsync();
    }

    /// <summary>
    /// Finds a single entity based on the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to filter entities.</param>
    /// <returns>A task representing the asynchronous operation, returning the found entity or null if not found.</returns>
    # pragma warning disable CS8603
    public virtual async Task<TEntity> FindSingleAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
    }
    # pragma warning restore CS8603

    /// <summary>
    /// Retrieves a paged collection of entities.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of entities per page.</param>
    /// <returns>A task representing the asynchronous operation, returning a paged collection of entities.</returns>
    public virtual async Task<IEnumerable<TEntity>> PagedAsync(int pageNumber, int pageSize)
    {
        /* Represents the adjustment applied to page numbers to align with zero-based indices in LINQ queries. */
        const int pageIndexAdjustment = 1;

        return await _dbContext.Set<TEntity>()
            .Skip((pageNumber - pageIndexAdjustment) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves a paged collection of entities based on the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to filter entities.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of entities per page.</param>
    /// <returns>A task representing the asynchronous operation, returning a paged collection of entities.</returns>
    public virtual async Task<IEnumerable<TEntity>> PagedAsync(Expression<Func<TEntity, bool>> predicate, int pageNumber, int pageSize)
    {
        /* Represents the adjustment applied to page numbers to align with zero-based indices in LINQ queries. */
        const int pageIndexAdjustment = 1;

        return await _dbContext.Set<TEntity>()
            .Where(predicate)
            .Skip((pageNumber - pageIndexAdjustment) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}