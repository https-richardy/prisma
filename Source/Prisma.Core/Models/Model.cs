/*
    author: Richard Garcia (https://github.com/https-richardy)
    license: Licensed under the MIT License
*/

namespace Prisma.Core.Models;

# pragma warning disable CS8618

/// <summary>
/// Represents a base model for entities with a primary key of type <typeparamref name="TKey"/>.
/// </summary>
/// <typeparam name="TKey">The type of the entity's identifier.</typeparam>
/// <remarks>
/// This base model provides a common structure for entities by introducing a unique identifier property.
/// The identifier, represented by the property <see cref="Id"/>, is of type <typeparamref name="TKey"/>.
/// Entities deriving from this base model can leverage the standardized identifier property for
/// consistent handling of primary keys across the application.
/// </remarks>
public abstract class Model<TKey> : IEntity<TKey>
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public TKey Id { get; set; }
}

/// <summary>
/// Represents a base model for entities with a primary key of type <see cref="int"/>.
/// </summary>
/// <remarks>
/// This class is a convenience class that inherits from <see cref="Model{TKey}"/> with
/// the type parameter Tkey set to <see cref="int"/>.
/// </remarks>
public class Model : Model<int>, IEntity
{

}

# pragma warning restore CS8618