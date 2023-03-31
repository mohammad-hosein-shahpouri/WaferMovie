using WaferMovie.Application.Movies.Commands.CreateMovie;

namespace WaferMovie.Application.Test.Movies.Commands;

public class CreateMovieCommandTest
{
    private readonly IApplicationDbContext dbContext = InMemoryDatabase.Create();
    private readonly IMediator mediator;

    public CreateMovieCommandTest()
    {
        var applicationAssembly = Assembly.Load("WaferMovie.Application");
        var services = new ServiceCollection();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddValidatorsFromAssembly(applicationAssembly);
        services.AddSingleton(dbContext);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        mediator = services.BuildServiceProvider().GetService<IMediator>()!;
    }

    [Fact]
    public async Task ShouldCreateMovie()
    {
        var command = new CreateMovieCommand("tt1825683")
        {
            Description = "T'Challa, heir to the hidden but advanced kingdom of Wakanda, must step forward to lead his people into a new future and must confront a challenger from his country's past.",
            IsFree = true,
            Length = 134,
            OutYear = 2018,
            Title = "Black Panther",
            Unavailable = false
        };

        var result = await mediator.Send(command);
        var newMovie = await dbContext.Movies.FirstOrDefaultAsync(f => f.Id == result.Data.Id);

        result.Succeeded.ShouldBeTrue();
        result.Status.ShouldBe(CrudStatus.Succeeded);

        newMovie!.Description.ShouldBe(command.Description);
        newMovie.IsFree.ShouldBeTrue();
        newMovie.Length.ShouldBe(command.Length);
        newMovie.OutYear.ShouldBe(command.OutYear);
        newMovie.Title.ShouldBe(command.Title);
        newMovie.Unavailable.ShouldBeFalse();
    }

    [Fact]
    public async Task ShouldHaveValidationError()
    {
        var command = new CreateMovieCommand("");

        var result = await mediator.Send(command);

        result.Status.ShouldBe(CrudStatus.ValidationError);
    }
}