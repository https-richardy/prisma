/*
    author: Richard Garcia (https://github.com/https-richardy)
    license: Licensed under the MIT License
*/

namespace Prisma.Tests.Objects;

/// <summary>
/// Represents a sample entity for testing purposes.
/// </summary>
public class Foo : Model<int>
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public DateTime BirthDate { get; set; }

    public Foo()
    {
        /* Default constructor */
    }

    public Foo(string name, int age, DateTime birthDate)
    {
        Name = name;
        Age = age;
        BirthDate = birthDate;
    }
}