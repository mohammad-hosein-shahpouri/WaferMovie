using WaferMovie.Application.Series.Queries.FindSerieById;

namespace WaferMovie.Application.IntegrationTests.Series.Queries;

public class FindSerieByIdQueryTest : TestFixture
{
    [Test]
    public async Task ShouldFindSerie()
    {
        using var dbContext = Resolve<IApplicationDbContext>();
        var mediator = Resolve<IMediator>();

        var query = new FindSerieByIdQuery(1);
        var result = await mediator.Send(query);

        result.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNull();

        var serie = dbContext.Series.First(f => f.Id == 1);

        result.Data.IMDB.Should().Be(serie.IMDB);
        result.Data.Title.Should().Be(serie.Title);
    }

    [Test]
    public async Task ShouldReturnNotFound()
    {
        var mediator = Resolve<IMediator>();

        var query = new FindSerieByIdQuery(999);
        var result = await mediator.Send(query);

        result.Status.Should().Be(CrudStatus.NotFound);
        result.Data.Should().BeNull();
    }
}