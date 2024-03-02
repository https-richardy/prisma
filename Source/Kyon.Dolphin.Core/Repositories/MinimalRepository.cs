// Kyon - Open Source Initiative
// Licensed under the MIT License

using Kyon.Dolphin.Core.Common;
using Kyon.Dolphin.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Kyon.Dolphin.Core.Data.Repositories;

/// <summary>
/// A generic repository providing CRUD operations for entities using Entity Framework.
/// </summary>
/// <typeparam name="TEntity">The type of entity managed by the repository.</typeparam>
/// <typeparam name="TKey">The type of entity's primary key.</typeparam>
/// <typeparam name="TDbContext">The type of the Entity Framework DbContext.</typeparam>
public abstract class MinimalRepository<TEntity, TKey, TDbContext> : IMinimalRepository<TEntity>
    where TEntity : class, IEntity<TKey>
    where TDbContext : DbContext
{
    /// <summary>
    /// The Entity Framework DbContext for database interactions.
    /// </summary>
    protected readonly TDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="MinimalRepository{TEntity, TKey, TDbContext}"/> class.
    /// </summary>
    /// <param name="dbContext">The Entity Framework DbContext instance.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public MinimalRepository(TDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <summary>
    /// Asynchronously saves a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to be saved.</param>
    /// <returns>
    /// A task representing the asynchronous save operation. The result is an <see cref="OperationResult"/> indicating success or failure.
    /// </returns>
    public virtual async Task<OperationResult> SaveAsync(TEntity entity)
    {
        try
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            int result = await _dbContext.SaveChangesAsync();

            return result > 0 ? OperationResult.Success : OperationResult.Failed;
        }
        catch (Exception)
        {
            return OperationResult.Failed;
        }
    }

    /// <summary>
    /// Asynchronously updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to be updated.</param>
    /// <returns>
    /// A task representing the asynchronous update operation.
    /// The result is an <see cref="OperationResult"/> indicating success or failure.
    /// </returns>
    public virtual async Task<OperationResult> UpdateAsync(TEntity entity)
    {
        try
        {
            var existingEntity = await _dbContext.Set<TEntity>().FindAsync(entity.Id);

            if (existingEntity != null)
            {
                _dbContext.Entry(existingEntity).State = EntityState.Detached;
                _dbContext.Entry(entity).State = EntityState.Modified;

                int result = await _dbContext.SaveChangesAsync();

                return result > 0 ? OperationResult.Success : OperationResult.Failed;
            }
            else
                return OperationResult.Failed;
        }
        catch (Exception)
        {
            return OperationResult.Failed;
        }
    }

    /// <summary>
    /// Asynchronously deletes an existing entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to be deleted.</param>
    /// <returns>
    /// A task representing the asynchronous delete operation.
    /// The result is an <see cref="OperationResult"/> indicating sucess or failure.
    /// </returns>
    public virtual async Task<OperationResult> DeleteAsync(TEntity entity)
    {
        try
        {
            _dbContext.Set<TEntity>().Remove(entity);
            int result = await _dbContext.SaveChangesAsync();

            return result > 0 ? OperationResult.Success : OperationResult.Failed;
        }
        catch (Exception)
        {
            return OperationResult.Failed;
        }
    }

    /// <summary>
    /// Asynchronously retrieves all entities from the repository.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous retrieval operation.
    /// The result is a collection of entities represented by <see cref="IEnumerable{TEntity}"/>.
    /// </returns>
    public virtual async Task<IEnumerable<TEntity>> RetrieveAllAsync()
    {
        return await _dbContext.Set<TEntity>().ToListAsync();
    }

    /// <summary>
    /// Asynchronously retrieves an entity from the repository by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <returns>
    /// A task representing the asynchronous retrieval operation.
    /// The result is the retrieved entity or null if not found.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the provided indentifier has an unsupported key type.
    /// </exception>
    public virtual async Task<TEntity> RetrieveByIdAsync(object id)
    {
        # pragma warning disable CS8603

        switch (id)
        {

            case int intId:
                return await _dbContext.Set<TEntity>().FindAsync(intId);
            case string stringId:
                return await _dbContext.Set<TEntity>().FindAsync(stringId);
            default:
                throw new ArgumentException("Unsupported key type");
        }

        # pragma warning restore
    }
}