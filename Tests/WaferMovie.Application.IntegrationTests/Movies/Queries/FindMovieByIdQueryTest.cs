using WaferMovie.Application.Movies.Queries.FindMovieById;

namespace WaferMovie.Application.IntegrationTests.Movies.Queries;

public class FindMovieByIdQueryTest : TestFixture
{
    [Test]
    public async Task ShouldFindMovie()
    {
        using var dbContext = Resolve<IApplicationDbContext>();
        var mediator = Resolve<IMediator>();

        var query = new FindMovieByIdQuery(1);
        var result = await mediator.Send(query);

        result.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNull();

        var movie = dbContext.Movies.First(f => f.Id == 1);

        result.Data.IMDB.Should().Be(movie.IMDB);
        result.Data.Title.Should().Be(movie.Title);
    }

    [Test]
    public async Task ShouldReturnNotFound()
    {
        var mediator = Resolve<IMediator>();

        var query = new FindMovieByIdQuery(999);
        var result = await mediator.Send(query);

        result.Status.Should().Be(CrudStatus.NotFound);
        result.Data.Should().BeNull();
    }
}