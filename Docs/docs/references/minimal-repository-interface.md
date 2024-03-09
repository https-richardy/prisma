# IMinimalRepository Interface

## Overview

The `IMinimalRepository` interface defines a minimal set of methods for basic CRUD (Create, Read, Update, Delete) operations on entities. It is designed to provide a common foundation for repository interfaces, allowing the management of entities in a simple and standardized way.

---

## Members

### SaveAsync

```csharp
Task<OperationResult> SaveAsync(TEntity entity);
```

Asynchronously saves a new entity to the repository.

* Parameters:
    * `entity`: The entity to be saved.

* Returns: A task representing the asynchronous save operation. The result is an `OperationResult` indicating success or failure.


### DeleteAsync

```csharp
Task<OperationResult> DeleteAsync(TEntity entity);
```

Asynchronously deletes an existing entity from the repository.

* Parameters:
    * `entity`: The entity to be deleted.
* Returns: A task representing the asynchronous delete operation. The result is an `OperationResult`` indicating success or failure.


### UpdateAsync

```csharp
Task<OperationResult> UpdateAsync(TEntity entity);
```
* Parameters:
    * `entity`: The entity to be updated.

* Returns: A task representing the asynchronous update operation. The result is an OperationResult indicating success or failure.

### RetrieveByIdAsync

```csharp
Task<TEntity> RetrieveByIdAsync(object id);
```
Asynchronously retrieves an entity from the repository by its identifier.

* Parameters:
    * `id`: The identifier of the entity.

* Returns: A task representing the asynchronous retrieval operation, returning the retrieved entity.

### RetrieveAllAsync

```csharp
Task<IEnumerable<TEntity>> RetrieveAllAsync();
```
Asynchronously retrieves all entities from the repository.

* Returns: A task representing the asynchronous retrieval operation, returning a collection of entities.

---

## Usage

The `IMinimalRepository`` interface serves as a foundation for defining repositories in your application. Implement this interface to create repository classes that handle basic CRUD operations for specific entities.

### Example

```csharp
public class UserRepository : IMinimalRepository<User>
{
    // Implementation of the interface methods
}
```

In this example, `UserRepository` implements the `IMinimalRepository` interface to manage User entities using the defined CRUD operations.

## Remarks

The `IMinimalRepository` interface is particularly useful in scenarios where you need a standardized set of methods for managing entities without the complexity of a full repository pattern. It provides a clean and minimalistic approach to handling basic data access operations.

Feel free to extend this interface or use it as is, depending on the needs of your application.