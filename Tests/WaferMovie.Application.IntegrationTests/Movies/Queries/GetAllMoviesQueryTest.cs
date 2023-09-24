using WaferMovie.Application.Movies.Queries.GetAllMovies;

namespace WaferMovie.Application.IntegrationTests.Movies.Queries;

public class GetAllMoviesQueryTest : TestFixture
{
    [Test]
    public async Task ShouldReturnAllMovies()
    {
        using var dbContext = Resolve<IApplicationDbContext>();
        var mediator = Resolve<IMediator>();

        var result = await mediator.Send(new GetAllMoviesQuery());
        result.Succeeded.Should().BeTrue();
        result.Data.Count.Should().Be(dbContext.Movies.Count());
        result.Data.All(a => dbContext.Movies.Select(s => s.Id).Contains(a.Id)).Should().BeTrue();
        dbContext.Movies.All(a => result.Data.Select(s => s.Id).Contains(a.Id)).Should().BeTrue();
    }
}