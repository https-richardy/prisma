// Kyon - Open Source Initiative
// Licensed under the MIT License

namespace Kyon.Dolphin.Tests;

/// <summary>
/// Represents a test-specific database context for testing purposes.
/// </summary>
public class TestDbContext : DbContext
{
    /// <summary>
    /// Gets or sets the DbSet for Foo entities.
    /// </summary>
    public DbSet<Foo> Foos { get; set; }

    public TestDbContext(DbContextOptions options)
        : base(options) {  }
}