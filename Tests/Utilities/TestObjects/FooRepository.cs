/*
    author: Richard Garcia (https://github.com/https-richardy)
    license: Licensed under the MIT License
*/

namespace Prisma.Tests.Objects;

/// <summary>
/// Represents a repository for testing purposes specifically design for the <see cref="Foo"/> entity.
/// </summary>
/// <remarks>
/// This repository is implemented based on the <see cref="Repository{TEntity, Tkey, TDbContext}"/>
/// and <see cref="IRepository{TEntity}"/> interfaces.
/// </remarks>
public class FooRepository : Repository<Foo, int, TestDbContext>, IRepository<Foo>
{
    public FooRepository(TestDbContext dbContext)
        : base(dbContext) {  }
}