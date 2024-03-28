/*
    author: Richard Garcia (https://github.com/https-richardy)
    license: Licensed under the MIT License
*/

namespace Prisma.Core.Services.Exceptions;

/// <summary>
/// Exception thrown when the file extension is not allowed.
/// </summary>
public class InvalidFileExtensionException : Exception
{
    public InvalidFileExtensionException(string message) : base(message)
    {

    }
}