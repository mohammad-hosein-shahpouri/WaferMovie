using StackExchange.Redis;
using WaferMovie.Application.Series.Queries.GetAllSeries;

namespace WaferMovie.Application.Test.Series.Queries;

public class GetAllSeriesQueryTest
{
    private readonly IApplicationDbContext dbContext = InMemoryDatabase.Create();
    private readonly IMediator mediator;

    public GetAllSeriesQueryTest()
    {
        mediator = Services.Configure(dbContext);
    }

    [Fact]
    public async Task ShouldReturnAllSeries()
    {
        var result = await mediator.Send(new GetAllSeriesQuery());
        result.Succeeded.ShouldBeTrue();
        result.Data.Count.ShouldBe(dbContext.Series.Count());
        result.Data.All(a => dbContext.Series.Select(s => s.Id).Contains(a.Id)).ShouldBeTrue();
        dbContext.Series.All(a => result.Data.Select(s => s.Id).Contains(a.Id)).ShouldBeTrue();
    }
}