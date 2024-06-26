/*
    author: Richard Garcia (https://github.com/https-richardy)
    license: Licensed under the MIT License
*/

namespace Prisma.Tests.Objects;

/// <summary>
/// Represents a repository for testing purposes specifically design for the <see cref="Foo"/> entity.
/// </summary>
/// <remarks>
/// This repository is implemented based on the <see cref="MinimalRepository{TEntity, Tkey, TDbContext}"/>
/// and <see cref="IMinimalRepository{TEntity}"/> interfaces.
/// </remarks>
public class FooMinimalRepository : MinimalRepository<Foo, int, TestDbContext>, IMinimalRepository<Foo>
{
    public FooMinimalRepository(TestDbContext dbContext)
        : base(dbContext) {  }
}