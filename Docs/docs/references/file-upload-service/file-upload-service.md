# FileUploadService

## Overview
`FileUploadService` provides file upload functionality within an ASP.NET Core application. It ensures that files are stored in a specified directory, manages file size limits, allowed file extensions, and handles file overwrite conditions.

## Usage
### Constructors
- `FileUploadService(IWebHostEnvironment webHostEnvironment)`: Initializes a new instance of the `FileUploadService` class with default options. The uploads directory will be set to the default specified in the web root path.
- `FileUploadService(FileUploadOptions options)`: Initializes a new instance of the `FileUploadService` class with custom options.

### Methods
- `Task<string> UploadFileAsync(IFormFile file)`: Uploads a file asynchronously. Throws `ArgumentNullException` if the file is null. Throws `InvalidFileExtensionException` if the file extension is not allowed. Throws `FileSizeLimitExceededException` if the file size exceeds the maximum allowed size. Throws `FileOverwriteNotAllowedException` if the file already exists and overwriting is not allowed.

## Remarks
- When using the `FileUploadService` with the default constructor, the uploads directory is typically set to the wwwroot/uploads directory to ensure accessibility and management of uploaded files.
- Custom options can be provided to the `FileUploadService` constructor to customize upload directory, allowed extensions, overwrite behavior, and maximum file size.

