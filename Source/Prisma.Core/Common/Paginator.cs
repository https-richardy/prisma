/*
    author: Richard Garcia (https://github.com/https-richardy)
    license: Licensed under the MIT License
*/

#pragma warning disable CS8603

using Microsoft.AspNetCore.Http;

namespace Prisma.Core.Common;

/// <summary>
/// Paginator is a utility class that facilitates pagination of a collection and computes next/previous URLs based on the current request path.
/// </summary>
/// <remarks>
/// <para>
/// This class is designed to be used in web applications where pagination is required. It takes a collection of items,
/// the current page number, the desired page size, and the HttpContext to calculate pagination information.
/// </para>
/// <para>
/// The class provides properties such as Count (total number of items), CurrentPage (current page number),
/// Next (URL for the next page), Previous (URL for the previous page), and Results (the paginated collection of items).
/// </para>
/// </remarks>
/// <typeparam name="T">The type of items in the collection.</typeparam>
public class Paginator<T>
{
    /// <summary>
    /// Gets the total count of items.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Gets the current page number.
    /// </summary>
    public int CurrentPage { get; private set; }

    /// <summary>
    /// Gets the URL for the next page, or null if no next page.
    /// </summary> 
    public string Next { get; private set; }

    /// <summary>
    /// Gets the URL for the previous page, or null if no previous page.
    /// </summary>
    public string Previous { get; private set; }

    /// <summary>
    /// Gets the collection of items on the current page.
    /// </summary>
    public IEnumerable<T> Results { get; private set; } = new List<T>();

    /// <summary>
    /// Initializes a new instance of the <see cref="Paginator{T}"/> class.
    /// </summary>
    /// <param name="data">The collection of items to paginate.</param>
    /// <param name="pageNumber">The current page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="httpContext">The current HttpContext. Required for calculating URLs based on the current request path.</param>
    public Paginator(IEnumerable<T> data, int pageNumber, int pageSize, HttpContext httpContext)
    {
        Count = data.Count();
        CurrentPage = pageNumber;

        int totalPages = (int)Math.Ceiling(Count / (double)pageSize);

        Next = CalculateNextUrl(httpContext.Request.Path, pageNumber, totalPages);
        Previous = CalculatePreviousUrl(httpContext.Request.Path, pageNumber);

        Results = data.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
    }

    private string CalculateNextUrl(PathString path, int pageNumber, int totalPages)
    {
        return pageNumber < totalPages
            ? $"{path}?={pageNumber + 1}"
            : null;
    }

    private string CalculatePreviousUrl(PathString path, int pageNumber)
    {
        return pageNumber > 1
            ? $"{path}?page={pageNumber - 1}"
            : null;
    }
}

#pragma warning restore CS8603