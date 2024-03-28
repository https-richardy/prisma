/*
    author: Richard Garcia (https://github.com/https-richardy)
    license: Licensed under the MIT License
*/

namespace Prisma.Core.Services.Exceptions;

/// <summary>
/// Exception thrown when attempting to overwrite a file that already exists and overwriting is not allowed.
/// </summary>
public class FileOverwriteNotAllowedException : Exception
{
    public FileOverwriteNotAllowedException(string message) : base(message)
    {

    }
}