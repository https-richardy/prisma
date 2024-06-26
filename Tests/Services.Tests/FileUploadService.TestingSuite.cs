/*
    author: Richard Garcia (https://github.com/https-richardy)
    license: Licensed under the MIT License
*/

namespace Prisma.Tests.Services;

public class FileUploadServiceTestSuite
{
    private readonly IFileUploadService _fileUploadService;
    private readonly Mock<IWebHostEnvironment> _webHostEnvironment;
    private readonly string _uploadsDirectory;

    public FileUploadServiceTestSuite()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var webRootPath = Path.Combine(currentDirectory, "wwwroot");

        _webHostEnvironment = new Mock<IWebHostEnvironment>();
        _webHostEnvironment.Setup(environment => environment.WebRootPath).Returns(webRootPath);

        _uploadsDirectory = Path.Combine(webRootPath, "uploads");

        _fileUploadService = new FileUploadService(_webHostEnvironment.Object);
    }

    [Fact(DisplayName = "UploadFileAsync should save the file with a unique name when GenerateUniqueFileNames is true")]
    public async Task UploadFileAsync_UniqueFileName_WhenGenerateUniqueFileNamesTrue()
    {
        var options = new FileUploadOptions
        {
            GenerateUniqueFileNames = true,
            UploadsDirectory = _uploadsDirectory,
        };

        var fileUploadService = new FileUploadService(options);

        const string fileName = "test.png";

        var formFile = new Mock<IFormFile>();
        formFile.Setup(file => file.FileName).Returns(fileName);

        var filePath = await fileUploadService.UploadFileAsync(formFile.Object);

        Assert.NotNull(filePath);
        Assert.True(File.Exists(filePath));
        Assert.NotEqual(fileName, Path.GetFileName(filePath));
    }

    [Fact(DisplayName = "UploadFileAsync should save the file with the same name when GenerateUniqueFileNames is false and OverwriteExistingFiles is true")]
    public async Task UploadFileAsync_SameFileName_WhenGenerateUniqueFileNamesFalseAndOverwriteExistingFilesTrue()
    {
        var options = new FileUploadOptions
        {
            GenerateUniqueFileNames = false,
            OverwriteExistingFiles = true,
            UploadsDirectory = _uploadsDirectory,
        };

        var fileUploadService = new FileUploadService(options);

        const string fileName = "test.png";
        var formFile = new Mock<IFormFile>();
        formFile.Setup(file => file.FileName).Returns(fileName);

        var filePath = await fileUploadService.UploadFileAsync(formFile.Object);

        Assert.NotNull(filePath);
        Assert.True(File.Exists(filePath));
        Assert.Equal(fileName, Path.GetFileName(filePath));
    }

    [Fact(DisplayName = "UploadFileAsync should create the uploads directory if it does not exist")]
    public async Task UploadFileAsync_CreateUploadsDirectoryIfNotExists()
    {
        var options = new FileUploadOptions
        {
            GenerateUniqueFileNames = false,
            OverwriteExistingFiles = true,
            UploadsDirectory = _uploadsDirectory,
        };

        // Delete the uploads directory if it exists
        if (Directory.Exists(_uploadsDirectory))
            Directory.Delete(_uploadsDirectory, true);

        var fileUploadService = new FileUploadService(options);

        const string fileName = "test.png";

        var formFile = new Mock<IFormFile>();
        formFile.Setup(file => file.FileName).Returns(fileName);

        var filePath = await fileUploadService.UploadFileAsync(formFile.Object);

        Assert.NotNull(filePath);
        Assert.True(File.Exists(filePath));

        Assert.Equal(fileName, Path.GetFileName(filePath));
        Assert.True(Directory.Exists(_uploadsDirectory));
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

    [Fact(DisplayName = "UploadFileAsync should throw FileOverwriteNotAllowedException for an existing file when overwrite is not allowed")]
    public async Task UploadFileAsync_ExistingFileNoOverwrite_ThrowsFileOverwriteNotAllowedException()
    {
        var options = new FileUploadOptions
        {
            GenerateUniqueFileNames = false,
            UploadsDirectory = _uploadsDirectory,
        };

        var fileUploadService = new FileUploadService(options);

        const string fileName = "existing_file.jpg";
        var existingFilePath = Path.Combine(_uploadsDirectory, fileName);

        File.WriteAllText(existingFilePath, "Some content");

        var formFile = new Mock<IFormFile>();
        formFile.Setup(file => file.FileName).Returns(fileName);

        await Assert.ThrowsAsync<FileOverwriteNotAllowedException>(() => fileUploadService.UploadFileAsync(formFile.Object));
    }
}