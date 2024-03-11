/*
    author: Richard Garcia (https://github.com/https-richardy)
    license: Licensed under the MIT License
*/

namespace Prisma.Core.Models;

# pragma warning disable CS8618

/// <summary>
/// Represents a base model for entities 
/// </summary>
/// <typeparam name="TKey">The type of the entity's indentifier.</typeparam>
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
public class Model : Model<int>
{

}

# pragma warning restore CS8618