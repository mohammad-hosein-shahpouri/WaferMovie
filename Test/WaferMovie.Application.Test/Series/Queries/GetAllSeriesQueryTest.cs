using StackExchange.Redis;
using WaferMovie.Application.Series.Queries.GetAllSeries;

namespace WaferMovie.Application.Test.Series.Queries;

public class GetAllSeriesQueryTest
{
    private readonly IApplicationDbContext dbContext = InMemoryDatabase.Create();
    private readonly IMediator mediator;

    public GetAllSeriesQueryTest()
    {
        var applicationAssembly = Assembly.Load("WaferMovie.Application");
        var mockCacheDb = new Mock<IDatabase>();
        var services = new ServiceCollection();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddValidatorsFromAssembly(applicationAssembly);
        services.AddSingleton(dbContext);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        services.AddSingleton(mockCacheDb.Object);
        mediator = services.BuildServiceProvider().GetService<IMediator>()!;
    }

    [Fact]
    public async Task ShouldReturnAllSeries()
    {
        var result = await mediator.Send(new GetAllSeriesQuery());
        result.Succeeded.ShouldBeTrue();
        result.Data.Count.ShouldBe(dbContext.Series.Count());
        result.Data.All(a => dbContext.Series.Contains(a)).ShouldBeTrue();
        dbContext.Series.All(a => result.Data.Contains(a)).ShouldBeTrue();
    }
}