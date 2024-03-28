/*
    author: Richard Garcia (https://github.com/https-richardy)
    license: Licensed under the MIT License
*/

namespace Prisma.Core.Services.Exceptions;

/// <summary>
/// Exception thrown when the size of the file exceeds the maximum allowed size.
/// </summary>
public class FileSizeLimitExceededException : Exception
{
    public FileSizeLimitExceededException(string message) : base(message) 
    {

    }
}