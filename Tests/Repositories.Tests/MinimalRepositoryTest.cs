/*
    author: Richard Garcia (https://github.com/https-richardy)
    license: Licensed under the MIT License
*/

namespace Prisma.Tests.Repositories;

public class MinimalRepositoryTest : IAsyncLifetime
{
    private readonly TestDbContext _dbContext;
    private readonly IMinimalRepository<Foo> _repository;
    private readonly IFixture _fixture;

    public MinimalRepositoryTest()
    {
        var options = new DbContextOptionsBuilder()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new TestDbContext(options);
        _repository = new FooMinimalRepository(_dbContext);

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

    [Fact(DisplayName = "SaveAsync() should add entity and return success")]
    public async Task SaveAsync_ShouldAddEntityAndReturnSuccess()
    {
        var newEntity = _fixture.Create<Foo>();
        var result = await _repository.SaveAsync(newEntity);

        Assert.Equal(OperationResult.Success, result);
        Assert.Contains(newEntity, _dbContext.Foos);
    }

    [Fact(DisplayName = "SaveAsync() should return failed when an exception is thrown")]
    public async Task SaveAsync_ShouldReturnFailedOnException()
    {
        var newEntity = _fixture.Create<Foo>();

        _dbContext.Foos.AddRange(newEntity);
        await _dbContext.SaveChangesAsync();

        var mockSet = new Mock<DbSet<Foo>>();
        _dbContext.Foos = mockSet.Object;
        var repositoryWithMock = new FooMinimalRepository(_dbContext);

        mockSet.Setup(m => m.AddAsync(It.IsAny<Foo>(), It.IsAny<CancellationToken>()))
               .Throws(new Exception("Simulated exception"));


        var result = await repositoryWithMock.SaveAsync(newEntity);

        Assert.Equal(OperationResult.Failed, result);
    }

    [Fact(DisplayName = "UpdateAsync() should update existing entity and return sucess")]
    public async Task UpdateAsync_ShouldUpdateExistingEntityAndReturnSuccess()
    {
        var existingEntity = _fixture.Create<Foo>();

        _dbContext.Foos.Add(existingEntity);
        await _dbContext.SaveChangesAsync();

        existingEntity.Name = "John Doe";
        var result = await _repository.UpdateAsync(existingEntity);

        Assert.Equal(OperationResult.Success, result);

        var updatedEntity = _dbContext.Foos.Single();

        Assert.Equal(existingEntity.Id, updatedEntity.Id);
        Assert.Equal("John Doe", updatedEntity.Name);
    }

    [Fact(DisplayName = "UpdateAsync() should return failed if entity does not exist")]
    public async Task UpdateAsync_ShouldReturnFailedIfEntityDoesNotExist()
    {
        var nonExistingEntity = _fixture.Create<Foo>();
        var result = await _repository.UpdateAsync(nonExistingEntity);

        Assert.Equal(OperationResult.Failed, result);
    }

    [Fact(DisplayName = "DeleteAsync() should return delete existing entity and return success")]
    public async Task DeleteAsync_ShouldDeleteExistingEntityAndReturnSuccess()
    {
        var existingEntity = _fixture.Create<Foo>();

        _dbContext.Foos.Add(existingEntity);
        await _dbContext.SaveChangesAsync();

        var result = await _repository.DeleteAsync(existingEntity);

        Assert.Equal(OperationResult.Success, result);
        Assert.DoesNotContain(existingEntity, _dbContext.Foos);
    }

    [Fact(DisplayName = "DeleteAsync() should return failed if entity does not exist")]
    public async Task DeleteAsync_ShouldReturnFailedIfEntityDoesNotExist()
    {
        var nonExistingEntity = _fixture.Create<Foo>();
        var result = await _repository.DeleteAsync(nonExistingEntity);

        Assert.Equal(OperationResult.Failed, result);
    }

    [Fact(DisplayName = "RetrieveAllAsync() should return all entities in context")]
    public async Task RetrieveAllAsync_ShouldReturnAllEntitiesInContext()
    {
        var entities = _fixture.CreateMany<Foo>(3).ToList();

        _dbContext.Foos.AddRange(entities);
        await _dbContext.SaveChangesAsync();

        var resultEntities = await _repository.RetrieveAllAsync();

        Assert.Equal(entities.Count(), resultEntities.Count());
        foreach (var entity in entities)
        {
            Assert.Contains(entity, resultEntities);
        }
    }

    [Fact(DisplayName = "RetrieveByIdAsync() should return entity when ID exists")]
    public async Task RetrieveByIdAsync_ShouldReturnEntityWhenIdExists()
    {
        var existingEntity = _fixture.Create<Foo>();

        _dbContext.Foos.Add(existingEntity);
        await _dbContext.SaveChangesAsync();

        var resultEntity = await _repository.RetrieveByIdAsync(existingEntity.Id);

        Assert.NotNull(resultEntity);
        Assert.Equal(existingEntity.Id, resultEntity.Id);
    }

    [Fact(DisplayName = "RetrieveByIdAsync() should return null when ID does not exist")]
    public async Task RetrieveByIdAsync_ShouldReturnNullWhenIdDoesNotExist()
    {
        var nonExistingId = _fixture.Create<int>();
        var resultEntity = await _repository.RetrieveByIdAsync(nonExistingId);

        Assert.Null(resultEntity);
    }

    [Theory(DisplayName = "RetrieveByIdAsync() should throw ArgumentException for unsupported key types")]
    [InlineData(typeof(Guid))]
    [InlineData(typeof(decimal))]
    [InlineData(typeof(object))]
    [InlineData(typeof(DateTime))]
    [InlineData(typeof(TimeSpan))]
    # pragma warning disable CS8604
    public async Task RetrieveByIdAsync_ShouldThrowArgumentExceptionForUnsupportedKeyTypes(Type invalidKeyType)
    {
        var invalidId = Activator.CreateInstance(invalidKeyType);

        await Assert.ThrowsAsync<ArgumentException>(() => _repository.RetrieveByIdAsync(invalidId));
    }
    # pragma warning restore CS8604
}