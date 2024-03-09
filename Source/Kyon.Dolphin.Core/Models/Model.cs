// Kyon - Open Source Initiative
// Licensed under the MIT License

namespace Kyon.Dolphin.Core.Models;

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

# pragma warning restore CS8618