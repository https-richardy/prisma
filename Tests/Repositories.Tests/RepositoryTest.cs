// Kyon - Open Source Initiative
// Licensed under the MIT License

namespace Kyon.Dolphin.Tests;

public class RepositoryTest : IAsyncLifetime
{
    private readonly TestDbContext _dbContext;
    private readonly IRepository<Foo> _repository;
    private readonly IFixture _fixture;

    public RepositoryTest()
    {
        var options = new DbContextOptionsBuilder()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new TestDbContext(options);
        _repository = new FooRepository(_dbContext);

        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    public async Task InitializeAsync()
    {
        await _dbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContext.Database.EnsureDeletedAsync();
        _dbContext.Dispose();
    }

    [Fact]
    public async Task CountAsync_ShouldReturnZeroWhenNoEntitiesExist()
    {
        var repository = new FooRepository(_dbContext);
        var result = await repository.CountAsync();

        Assert.Equal(0, result);
    }

    [Fact]
    public async Task CountAsync_WithPredicate_ShouldReturnCorrectCount()
    {
        var repository = new FooRepository(_dbContext);
        var entitiesToAdd = _fixture.CreateMany<Foo>(3);

        _dbContext.Foos.AddRange(entitiesToAdd);
        await _dbContext.SaveChangesAsync();

        Expression<Func<Foo, bool>> predicate = x => true;

        var result = await repository.CountAsync(predicate);

        Assert.Equal(entitiesToAdd.Count(), result);
    }
}