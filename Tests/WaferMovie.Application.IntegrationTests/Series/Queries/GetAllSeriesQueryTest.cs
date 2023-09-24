using WaferMovie.Application.Series.Queries.GetAllSeries;

namespace WaferMovie.Application.IntegrationTests.Series.Queries;

public class GetAllSeriesQueryTest : TestFixture
{
    [Test]
    public async Task ShouldReturnAllSeries()
    {
        using var dbContext = Resolve<IApplicationDbContext>();
        var mediator = Resolve<IMediator>();
        var result = await mediator.Send(new GetAllSeriesQuery());
        result.Succeeded.Should().BeTrue();
        result.Data.Count.Should().Be(dbContext.Series.Count());
        result.Data.All(a => dbContext.Series.Select(s => s.Id).Contains(a.Id)).Should().BeTrue();
        dbContext.Series.All(a => result.Data.Select(s => s.Id).Contains(a.Id)).Should().BeTrue();
    }
}