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

        var formFile = new Mock<IFormFile>();
        formFile.Setup(file => file.FileName).Returns(fileName);

        var filePath = await _fileUploadService.UploadFileAsync(formFile.Object);

        Assert.NotNull(filePath);
        Assert.True(File.Exists(filePath));
    }

    [Fact(DisplayName = "UploadFileAsync should throw InvalidFileExtensionException for a file with disallowed extension")]
    public async Task UploadFileAsync_DisallowedExtension_ThrowsInvalidFileExtensionException()
    {
        const string fileName = "test.exe";

        var formFile = new Mock<IFormFile>();
        formFile.Setup(file => file.FileName).Returns(fileName);

        await Assert.ThrowsAsync<InvalidFileExtensionException>(() => _fileUploadService.UploadFileAsync(formFile.Object));
    }

    [Fact(DisplayName = "UploadFileAsync should throw FileSizeLimitExceededException for a file exceeding the maximum allowed size")]
    public async Task UploadFileAsync_ExceedsMaxFileSize_ThrowsFileSizeLimitExceededException()
    {
        const string fileName = "test.png";
        const long fileSize = 15 * 1024 * 1024; // 15 MB

        var formFile = new Mock<IFormFile>();

        formFile.Setup(file => file.Length).Returns(fileSize);
        formFile.Setup(file => file.FileName).Returns(fileName);

        await Assert.ThrowsAsync<FileSizeLimitExceededException>(() => _fileUploadService.UploadFileAsync(formFile.Object));
    }
}