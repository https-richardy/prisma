/*
    author: Richard Garcia (https://github.com/https-richardy)
    license: Licensed under the MIT License
*/

using Prisma.Core.Common;

namespace Prisma.Core.Data.Repositories;

/// <summary>
/// Minimal repository interface for basic CRUD operations on entities.
/// </summary>
/// <typeparam name="TEntity">The type of entity managed by the repository.</typeparam>
public interface IMinimalRepository<TEntity>
{
    /// <summary>
    /// Asynchronously saves a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to be saved.</param>
    /// <returns>A task representing the asynchronous save operation. The result is an <see cref="OperationResult"/> indicating success or failure.</returns>
    Task<OperationResult> SaveAsync(TEntity entity);

    /// <summary>
    /// Asynchronously deletes an existing entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to be deleted.</param>
    /// <returns>A task representing the asynchronous delete operation. The result is an <see cref="OperationResult"/> indicating success or failure.</returns>
    Task<OperationResult> DeleteAsync(TEntity entity);

    /// <summary>
    /// Asynchronously updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to be updated.</param>
    /// <returns>A task representing the asynchronous update operation. The result is an <see cref="OperationResult"/> indicating success or failure.</returns>
    Task<OperationResult> UpdateAsync(TEntity entity);

    /// <summary>
    /// Asynchronously retrieves an entity from the repository by its integer identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <returns>A task representing the asynchronous retrieval operation, returning the retrieved entity.</returns>
    Task<TEntity> RetrieveByIdAsync(object id);

    /// <summary>
    /// Asynchronously retrieves all entities from the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous retrieval operation, returning a collection of entities.</returns>
    Task<IEnumerable<TEntity>> RetrieveAllAsync();
}
