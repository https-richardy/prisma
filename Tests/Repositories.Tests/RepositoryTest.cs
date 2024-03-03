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
        var result = await _repository.CountAsync();

        Assert.Equal(0, result);
    }

    [Fact]
    public async Task CountAsync_WithPredicate_ShouldReturnCorrectCount()
    {
        var entitiesToAdd = _fixture.CreateMany<Foo>(3);

        _dbContext.Foos.AddRange(entitiesToAdd);
        await _dbContext.SaveChangesAsync();

        Expression<Func<Foo, bool>> predicate = x => true;

        var result = await _repository.CountAsync(predicate);

        Assert.Equal(entitiesToAdd.Count(), result);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrueForExistingId()
    {
        var entityToAdd = _fixture.Create<Foo>();
        _dbContext.Foos.Add(entityToAdd);

        await _dbContext.SaveChangesAsync();

        var result = await _repository.ExistsAsync(entityToAdd.Id);
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnFalseForNonExistingId()
    {
        const int nonExistingId = 999999;
        var result = await _repository.ExistsAsync(nonExistingId);

        Assert.False(result);
    }

    [Fact]
    public async Task FindAllAsync_ShouldReturnCorrectCollectionForMatchingPredicate()
    {
        var entity1 = _fixture.Build<Foo>()
            .With(e => e.Age, 20)
            .Create();

        var entity2 = _fixture.Build<Foo>()
            .With(e => e.Age, 30)
            .Create();

        var entity3 = _fixture.Build<Foo>()
            .With(e => e.Age, 35)
            .Create();

        _dbContext.Foos.AddRange(entity1, entity2, entity3);
        await _dbContext.SaveChangesAsync();

        Expression<Func<Foo, bool>> predicate = e => e.Age > 25;

        var result = await _repository.FindAllAsync(predicate);

        Assert.Equal(2, result.Count());
        Assert.Contains(entity2, result);
        Assert.Contains(entity3, result);
    }

    [Fact]
    public async Task FindSingleAsync_ShouldReturnCorrectEntityForMatchingPredicate()
    {
        var entity1 = _fixture.Build<Foo>()
            .With(e => e.Age, 20)
            .Create();

        var entity2 = _fixture.Build<Foo>()
            .With(e => e.Age, 30)
            .Create();

        _dbContext.Foos.AddRange(entity1, entity2);
        await _dbContext.SaveChangesAsync();

        Expression<Func<Foo, bool>> predicate = e => e.Age == 30;
        var result = await _repository.FindSingleAsync(predicate);

        Assert.NotNull(result);
        Assert.Equal(entity2, result);
    }

    [Fact]
    public async Task FindSingleAsync_ShouldReturnNullForNonMatchingPredicate()
    {
        var entity1 = _fixture.Build<Foo>().With(e => e.Age, 20).Create();
        var entity2 = _fixture.Build<Foo>().With(e => e.Age, 30).Create();

        _dbContext.Foos.AddRange(entity1, entity2);
        await _dbContext.SaveChangesAsync();

        Expression<Func<Foo, bool>> nonMatchingPredicate = e => e.Age == 25;
        var result = await _repository.FindSingleAsync(nonMatchingPredicate);

        Assert.Null(result);
    }

    [Fact]
    public async Task PagedAsync_ShouldReturnCorrectPagedCollection()
    {
        var entities = _fixture.Build<Foo>()
            .With(e => e.Age, 25)
            .CreateMany(10);

        _dbContext.Foos.AddRange(entities);
        await _dbContext.SaveChangesAsync();

        int pageNumber = 2;
        int pageSize = 3;

        var result = await _repository.PagedAsync(pageNumber, pageSize);

        Assert.Equal(pageSize, result.Count());

        int expectedStartIndex = (pageNumber - 1) * pageSize;

        var expectedEntities = entities.Skip(expectedStartIndex).Take(pageSize);
        Assert.Equal(expectedEntities, result);
    }
}