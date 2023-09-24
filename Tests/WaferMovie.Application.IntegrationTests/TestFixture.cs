using Bogus;

namespace WaferMovie.Application.IntegrationTests;

[TestFixture]
public class TestFixture
{
    private static IServiceScopeFactory? _scopeFactory;

    #region SetUp

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        var factory = new WebApplicationFactory();
        _scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
        using var dbContext = Resolve<IApplicationDbContext>();

        await dbContext.Database.OpenConnectionAsync();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        //while (!(await dbContext.Database.EnsureCreatedAsync()))
        //    await dbContext.Database.EnsureDeletedAsync();
        // dispose the factory to release the db connection
        //dbContext.Database.ens

        // await SeedHandler.SeedAsync(dbContext);
        RunInTesting();
    }

    [SetUp]
    public void Setup()
    {
        RunInTesting();
    }

    [TearDown]
    public void TearDown()
    {
    }

    public Faker Faker { get => new("en"); }
    public static string Locale { get; set; } = "en-US";

    public TService Resolve<TService>() where TService : class
    {
        var scope = _scopeFactory!.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<TService>();
        return service;
    }

    #endregion SetUp

    #region Enviorment

    public static void SetEnvironment(string environment)
    => Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", environment);

    public static void RunInDevelopment() => SetEnvironment("Development");

    public static void RunInStaging() => SetEnvironment("Staging");

    public static void RunInProduction() => SetEnvironment("Production");

    public static void RunInTesting() => SetEnvironment("Testing");

    #endregion Enviorment
}