# IRepository interface

## Overview

Extension of the [MinimalRepository<TEntity>](minimal-repository-interface.md) interface providing additional query operations.


## Type Parameters

* `TEntity`: The type of entity managed by the repository.

---

## Methods

### **FindSingleAsync**

```csharp
Task<TEntity> FindSingleAsync(Expression<Func<TEntity, bool>> predicate);
```
_Finds a single entity based on the specified predicate._

#### Parameters

* `predicate`: The predicate to filter entities.

**Returns**: A task representing the asynchronous operation, returning the found entity or null if not found.

### **FindAllAsync**

```csharp
Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);
```

_Finds all entities based on the specified predicate_

#### Parameters

* `predicate`: The predicate to filter entities.

**Returns**: A task representing the asynchronous operation, returning a collection of entities.


### PagedAsync

```csharp
Task<IEnumerable<TEntity>> PagedAsync(int pageNumber, int pageSize);
```

_Retrieves a paged collection of entities._

#### Parameters

* `pageNumber`: The page number to retrieve.
* `pageSize`: The number of entities per page.

**Returns**: A task representing the asynchronous operation, returning a paged collection of entities.

### PagedAsync (with predicate)

```csharp
Task<IEnumerable<TEntity>> PagedAsync(Expression<Func<TEntity, bool>> predicate, int pageNumber, int pageSize)
```

_Retrieves a paged collection of entities based on the specified predicate._

#### Parameters

* `predicate`: The predicate to filter entities.
* `pageNumber`: The page number to retrieve.
* `pageSize`: The number of entities per page.  

**Returns**: A task representing the asynchronous operation, returning a paged collection of entities.

### ExistsAsync

```csharp
Task<bool> ExistsAsync(object id);
```

_Checks if an entity with the specified indentifier exists in the repository._

#### Parameters
* `id`: The entity's identifier.

**Returns**: A task representing the asynchronous operation, returning true if the entity exists, false otherwise. 


### CountAsync

```csharp
Task<int> CountAsync();
```

_Counts the total number of entities in the repository._

**Returns**: A task representing the asynchronous operation, returning the total count of entities.

### CountAsync (with predicate)

```csharp
Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
```

_Counts the number of entities in the repository based on the specifies predicate._

#### Parameters

* `predicate`: The predicate to filter entities.

**Returns**: A task representing the asynchronous operation, returning the count of entities matching the predicate.

---


# Usage

The `IRepository<TEntity>` interface extends the functionality of the `IMinimalRepository<TEntity>` interface by introducing additional query operations, allowing for more sophisticated interactions with the underlying data store. Implement this interface when you require advanced querying capabilities in your repository classes.


## Example

```csharp
public class ProductRepository : IRepository<Product>
{
    // Implementation of the interface methods, including additional query operations
}
```

# Remarks

The `IRepository<TEntity>` interface is designed to offer a broader range of querying options beyond basic CRUD operations. It is particularly useful when your application demands more intricate data retrieval patterns, such as filtering, pagination, and conditional queries.

This interface can be employed in scenarios where you need a repository with extended query capabilities while maintaining the simplicity and consistency of the basic repository pattern. Feel free to customize or extend this interface to accommodate the specific querying requirements of your application.

Remember to consider the trade-offs between flexibility and simplicity when deciding whether to use `IMinimalRepository` or `IRepository`. Choose the one that aligns with the specific needs and complexity of your data access layer.

---

### See Also

* [IMinimalRepository<TEntity>](minimal-repository-interface.md)