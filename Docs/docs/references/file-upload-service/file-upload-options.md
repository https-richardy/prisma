# FileUploadOptions

## Overview
`FileUploadOptions` represents options for configuring file uploads. It provides settings for specifying upload directory, allowed file extensions, handling existing files, generating unique file names, and setting maximum file size.

By default, the upload directory is empty, allowed extensions include common image and media formats, existing files are not overwritten, unique file names are generated, and the maximum file size is set to 10 MB.

## Usage
### Properties
- `UploadsDirectory` (string): Gets or sets the directory where uploads will be stored. Default value is an empty string.
- `AllowedExtensions` (string[]): Gets or sets the allowed file extensions for uploads. Default includes .jpg, .jpeg, .png, .gif, .mp4, and .mp3.
- `OverwriteExistingFiles` (bool): Gets or sets a value indicating whether existing files should be overwritten. Default is false.
- `GenerateUniqueFileNames` (bool): Gets or sets a value indicating whether unique file names should be generated. Default is true.
- `MaxFileSize` (long): Gets or sets the maximum allowed file size (in bytes). Default is 10 MB.

## Remarks
- `AllowedExtensions` can be customized to include desired file formats.
- `OverwriteExistingFiles` can be set to true if existing files should be overwritten.
- `GenerateUniqueFileNames` can be set to false if unique file names are not desired.
- `MaxFileSize` can be customized according to specific requirements.


