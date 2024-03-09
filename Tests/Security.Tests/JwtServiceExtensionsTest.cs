// Kyon - Open Source Initiative
// Licensed under the MIT License

public class JwtServiceExtensionsTest
{
    private readonly IServiceCollection _services;
    private readonly Mock<IConfiguration> _configuration;
    private readonly ServiceProvider _serviceProvider;

    public JwtServiceExtensionsTest()
    {
        _services = new ServiceCollection();

        _configuration = new Mock<IConfiguration>();
        _configuration.Setup(x => x["Jwt:SecretKey"])
            .Returns(Guid.NewGuid().ToString());

        _services.AddScoped<IConfiguration>(_ => _configuration.Object);
        _services.AddJwtBearer(_configuration.Object);

        _serviceProvider = _services.BuildServiceProvider();
    }

    [Fact]
    public void AddJwtBearer_ShouldAddJwtServicesToCollection()
    {
        var jwtService = _serviceProvider.GetRequiredService<IJwtService>();
        Assert.NotNull(jwtService);
    }
}