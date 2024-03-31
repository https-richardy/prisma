# FileUploadServiceExtension

## Overview
`FileUploadServiceExtension` provides extension methods to facilitate the registration of `FileUploadService` within the ASP.NET Core application's dependency injection container.

## Usage
### Methods
- `void AddFileUploadService(IServiceCollection services)`: Adds the `FileUploadService` to the specified `IServiceCollection` with default options. The default options include configuring the upload directory as a subfolder within the web root path.
- `void AddFileUploadService(IServiceCollection services, Action<FileUploadOptions> configureOptions)`: Adds the `FileUploadService` to the specified `IServiceCollection` with custom options specified through the `configureOptions` delegate.

### Remarks
- The `AddFileUploadService` methods allow the addition of the `FileUploadService` to the service collection, providing flexibility to use default options or customize options as needed.
- Custom options can be specified through the `configureOptions` delegate, allowing configuration of various aspects of file upload functionality.

