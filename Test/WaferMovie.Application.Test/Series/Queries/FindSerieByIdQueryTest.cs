using WaferMovie.Application.Series.Queries.FindSerieById;

namespace WaferMovie.Application.Test.Series.Queries;

public class FindSerieByIdQueryTest
{
    private readonly IApplicationDbContext dbContext = InMemoryDatabase.Create();
    private readonly IMediator mediator;

    public FindSerieByIdQueryTest()
    {
        mediator = Services.Configure(dbContext);
    }

    [Fact]
    public async Task ShouldFindSerie()
    {
        var query = new FindSerieByIdQuery(1);
        var result = await mediator.Send(query);

        result.Succeeded.ShouldBeTrue();
        result.Data.ShouldNotBeNull();

        var serie = dbContext.Series.First(f => f.Id == 1);

        result.Data.IMDB.ShouldBe(serie.IMDB);
        result.Data.Title.ShouldBe(serie.Title);
    }

    [Fact]
    public async Task ShouldReturnNotFound()
    {
        var query = new FindSerieByIdQuery(999);
        var result = await mediator.Send(query);

        result.Status.ShouldBe(CrudStatus.NotFound);
        result.Data.ShouldBeNull();
    }
}