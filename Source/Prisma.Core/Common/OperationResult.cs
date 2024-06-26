/*
    author: Richard Garcia (https://github.com/https-richardy)
    license: Licensed under the MIT License
*/

namespace Prisma.Core.Common;

/// <summary>
/// Represents the result of an operation, indicating whether the operation was successful or failed.
/// </summary>
public enum OperationResult
{
    /// <summary>
    /// Indicates that the operation was successful.
    /// </summary>
    Success,

    /// <summary>
    /// Indicates that the operation failed.
    /// </summary>
    Failed
}