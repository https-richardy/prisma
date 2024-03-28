/*
    author: Richard Garcia (https://github.com/https-richardy)
    license: Licensed under the MIT License
*/

namespace Prisma.Tests.Services;

public class FileUploadServiceTestSuite
{
    private readonly IFileUploadService _fileUploadService;
    private readonly Mock<IWebHostEnvironment> _webHostEnvironment;

    public FileUploadServiceTestSuite()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var webRootPath = Path.Combine(currentDirectory, "wwwroot");

        _webHostEnvironment = new Mock<IWebHostEnvironment>();
        _webHostEnvironment.Setup(environment => environment.WebRootPath).Returns(webRootPath);
    
        _fileUploadService = new FileUploadService(_webHostEnvironment.Object);
    }

    [Fact(DisplayName = "UploadFileAsync should return file path for a valid file")]
    public async Task UploadFileAsync_ValidFileReturnsFilePath()
    {
        const string fileName = "test.png";
        const long fileSizeBytes = 1024; // Sample file size bytes - 1KB

        var formFile = new Mock<IFormFile>();

        formFile.Setup(file => file.FileName).Returns(fileName);
        formFile.Setup(file => file.Length).Returns(fileSizeBytes);

        var filePath = await _fileUploadService.UploadFileAsync(formFile.Object);

        Console.WriteLine(filePath);

        Assert.NotNull(filePath);
        Assert.True(File.Exists(filePath));
    }
}