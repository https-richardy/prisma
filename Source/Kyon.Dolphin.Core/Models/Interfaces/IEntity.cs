namespace Kyon.Dolphin.Core.Models;

/// <summary>
/// Interface representing an entity with a primary key.
/// </summary>
/// <typeparam name="TKey">The type of the primary key</typeparam>
public interface IEntity<TKey>
{
    TKey Id { get; }
}