using StackExchange.Redis;
using WaferMovie.Application.Movies.Queries.GetAllMovies;

namespace WaferMovie.Application.Test.Movies.Queries;

public class GetAllMoviesQueryTest
{
    private readonly IApplicationDbContext dbContext = InMemoryDatabase.Create();
    private readonly IMediator mediator;

    public GetAllMoviesQueryTest()
    {
        mediator = Services.Configure(dbContext); ;
    }

    [Fact]
    public async Task ShouldReturnAllMovies()
    {
        var result = await mediator.Send(new GetAllMoviesQuery());
        result.Succeeded.ShouldBeTrue();
        result.Data.Count.ShouldBe(dbContext.Movies.Count());
        result.Data.All(a => dbContext.Movies.Contains(a)).ShouldBeTrue();
        dbContext.Movies.All(a => result.Data.Contains(a)).ShouldBeTrue();
    }
}