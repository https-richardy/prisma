// Kyon - Open Source Initiative
// Licensed under the MIT License

namespace Kyon.Dolphin.Tests.Security;

public class JwtServiceTest
{
    private readonly IJwtService _jwtService;
    private readonly Mock<IConfiguration> _configuration;
    private readonly IFixture _fixture;

    public JwtServiceTest()
    {
        _configuration = new Mock<IConfiguration>();
        _configuration.Setup(x => x["Jwt:SecretKey"])
            .Returns(Guid.NewGuid().ToString());

        _jwtService = new JwtService(_configuration.Object);

        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public void GenerateToken_ShouldGenerateToken()
    {
        var claimsIdentity = _fixture.Create<ClaimsIdentity>();
        var token = _jwtService.GenerateToken(claimsIdentity);

        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }
}