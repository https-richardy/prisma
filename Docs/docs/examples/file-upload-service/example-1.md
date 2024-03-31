# Example of Using FileUploadService

In this example, we will show how to use `FileUploadService` to handle file uploads in an ASP.NET Core controller.

### 1. Adding the Service to `IServiceCollection`

Before using `FileUploadService`, you need to register it with the ASP.NET Core dependency injection container. This can be done in the `Startup` or `Program` class.

```csharp
// Add the FileUploadService to the IServiceCollection with the default options
services.AddFileUploadService();
```

When using the `AddFileUploadService()` method without specifying custom options, the service will be configured with the following default options:

* **Uploads Directory:** The uploads directory is defined as a subfolder within the web application's root directory (usually `wwwroot`). This approach is adopted to ensure that files uploaded by users are stored in a location that is accessible by the application and can be served via HTTP requests. Additionally, the [ASP.NET Core static files](https://learn.microsoft.com/pt-br/aspnet/core/fundamentals/static-files?view=aspnetcore-6.0) middleware, by default, serves files from the wwwroot directory, which facilitates the availability and management of files for the application.

* **Allowed File Extensions:** Allowed file extensions are jpg, jpeg, png, gif, mp4 and mp3. This means that only files with these extensions will be accepted for upload.


* **File overwriting:** The file overwriting option is not allowed by default, set to `false`. This means that if a file with the same name already exists in the uploads directory, the upload will be rejected.

* **Unique File Names:** By default, unique file names are generated for uploaded files. This is set to `true`. If you wish to disable this functionality, simply set this option to `false`.

* **Maximum File Size:** The maximum size allowed for files is `10 MB`, configured by default. This can be adjusted as needed to meet specific application requirements.

These default options were selected to provide a simple initial configuration for the file upload service, ensuring that files are stored properly and that the service functions correctly in a typical ASP.NET Core application.

#### Custom Settings

The default options are not mandatory and can be configured as desired. To customize the file upload service options, use the `AddFileUploadService() method with a configure action:

```csharp
services.AddFileUploadServices(options =>
{
     options.AllowedExtensions = new[] { ".docx", ".pdf", /* other allowed extensions */ };
     options.MaxFileSize = 20 * 1024 * 1024; // Set maximum file size to 20 MB
});
```

For more information about the available options, see the [FileUploadOptions API Reference](../../references/file-upload-service/file-upload-options.md)

### 2. Using `FileUploadService` in a Controller.

Now, we can use `FileUploadService` in a controller to handle file uploads. Below is an example of a controller action method that uses `FileUploadService` to process the upload of an uploaded file.

```csharp
public class UploadController : ControllerBase
{
    private readonly IFileUploadService _fileUploadService;

    public UploadController(IFileUploadService fileUploadService)
    {
        _fileUploadService = fileUploadService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFileAsync(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest("Nenhum arquivo foi enviado.");

            var filePath = await _fileUploadService.UploadFileAsync(file);

            return Ok($"Arquivo enviado com sucesso! Caminho: {filePath}");
        }
        catch (InvalidFileExtensionException ex)
        {
            return BadRequest($"Extensão de arquivo inválida: {ex.Message}");
        }
        catch (FileSizeLimitExceededException ex)
        {
            return BadRequest($"Tamanho máximo do arquivo excedido: {ex.Message}");
        }
        catch (FileOverwriteNotAllowedException ex)
        {
            return BadRequest($"Não é permitido sobrescrever o arquivo: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocorreu um erro ao processar o upload do arquivo: {ex.Message}");
        }
    }
}
```

In this example, the `UploadFileAsync` method is triggered by a POST request sent by an HTML form. The file parameter contains the file sent by the form. The method uses the FileUploadService to upload the file and returns an appropriate HTTP response based on the upload result.

This is just one example of how the FileUploadService can be used in an ASP.NET Core controller to handle file uploads. There are many other ways to integrate the service into your application, depending on your project's specific requirements.

### 3. Usage Considerations

* Properly handle exceptions thrown by `FileUploadService` to provide proper feedback to the user.