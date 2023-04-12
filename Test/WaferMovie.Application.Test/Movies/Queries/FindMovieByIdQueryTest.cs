using WaferMovie.Application.Movies.Queries.FindMovieById;

namespace WaferMovie.Application.Test.Movies.Queries;

public class FindMovieByIdQueryTest
{
    private readonly IApplicationDbContext dbContext = InMemoryDatabase.Create();
    private readonly IMediator mediator;

    public FindMovieByIdQueryTest()
    {
        mediator = Services.Configure(dbContext);
    }

    [Fact]
    public async Task ShouldFindMovie()
    {
        var query = new FindMovieByIdQuery(1);
        var result = await mediator.Send(query);

        result.Succeeded.ShouldBeTrue();
        result.Data.ShouldNotBeNull();

        var movie = dbContext.Movies.First(f => f.Id == 1);

        result.Data.IMDB.ShouldBe(movie.IMDB);
        result.Data.Title.ShouldBe(movie.Title);
    }

    [Fact]
    public async Task ShouldReturnNotFound()
    {
        var query = new FindMovieByIdQuery(999);
        var result = await mediator.Send(query);

        result.Status.ShouldBe(CrudStatus.NotFound);
        result.Data.ShouldBeNull();
    }
}