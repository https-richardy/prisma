// Kyon - Open Source Initiative
// Licensed under the MIT License

using AutoFixture;

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
}