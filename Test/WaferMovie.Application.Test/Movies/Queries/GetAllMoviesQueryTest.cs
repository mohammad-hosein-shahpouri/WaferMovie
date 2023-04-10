using Moq;
using StackExchange.Redis;
using WaferMovie.Application.Movies.Queries.GetAllMovies;

namespace WaferMovie.Application.Test.Movies.Queries;

public class GetAllMoviesQueryTest
{
    private readonly IApplicationDbContext dbContext = InMemoryDatabase.Create();
    private readonly IMediator mediator;

    public GetAllMoviesQueryTest()
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
    public async Task ShouldReturnAllMovies()
    {
        var result = await mediator.Send(new GetAllMoviesQuery());
        result.Succeeded.ShouldBeTrue();
        result.Data.Count.ShouldBe(dbContext.Movies.Count());
        result.Data.All(a => dbContext.Movies.Contains(a)).ShouldBeTrue();
        dbContext.Movies.All(a => result.Data.Contains(a)).ShouldBeTrue();
    }
}