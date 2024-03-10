/*
    author: Richard Garcia (https://github.com/https-richardy)
    license: Licensed under the MIT License
*/

namespace Prisma.Core.Models;

/// <summary>
/// Interface representing an entity with a primary key.
/// </summary>
/// <typeparam name="TKey">The type of the primary key</typeparam>
public interface IEntity<TKey>
{
    /// <summary>
    /// Gets the unique identifier for the entity.
    /// </summary>
    TKey Id { get; }
}