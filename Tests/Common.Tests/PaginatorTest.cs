/*
    author: Richard Garcia (https://github.com/https-richardy)
    license: Licensed under the MIT License
*/

namespace Prisma.Tests.Common;

public class PaginatorTest
{
    private readonly IFixture _fixture;

    public PaginatorTest()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact(DisplayName = "Paginator should be constructed with Foo entity")]
    public void Paginator_ConstructedWithFooEntity()
    {
        var data = _fixture.CreateMany<Foo>(100);
        var httpContext = new DefaultHttpContext();

        const int pageNumber = 1;
        const int pageSize = 10;

        var paginator = new Paginator<Foo>(data, pageNumber, pageSize, httpContext);

        Assert.Equal(data.Count(), paginator.Count);
        Assert.Equal(pageNumber, paginator.CurrentPage);
        Assert.NotNull(paginator.Next);
        Assert.Null(paginator.Previous);
        Assert.Equal(pageSize, paginator.Results.Count());
    }
}