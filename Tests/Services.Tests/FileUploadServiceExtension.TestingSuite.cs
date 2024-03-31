/*
    author: Richard Garcia (https://github.com/https-richardy)
    license: Licensed under the MIT License
*/

public class FileUploadServiceExtensionTestingSuite
{
    private readonly IServiceCollection _services;
    private readonly Mock<IWebHostEnvironment> _webHostEnvironment;
    private readonly string _uploadsDirectory;

    public FileUploadServiceExtensionTestingSuite()
    {
        _services = new ServiceCollection();

        var currentDirectory = Directory.GetCurrentDirectory();
        var webRootPath = Path.Combine(currentDirectory, "wwwroot");

        _uploadsDirectory = Path.Combine(webRootPath, "uploads");

        _webHostEnvironment = new Mock<IWebHostEnvironment>();
        _webHostEnvironment
            .Setup(environment => environment.WebRootPath)
            .Returns(webRootPath);

        _services.AddSingleton<IWebHostEnvironment>(_webHostEnvironment.Object);
    }

    [Fact(DisplayName = "AddFileUploadService should add FileUploadService to the service collection")]
    public void AddFileUploadService_ShouldAddFileUploadServiceToCollection()
    {
        _services.AddFileUploadService();

        var serviceProvider = _services.BuildServiceProvider();
        var fileUploadService = serviceProvider.GetRequiredService<IFileUploadService>();

        Assert.NotNull(fileUploadService);
    }
}