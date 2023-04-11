using WaferMovie.Application.Movies.Commands.CreateMovie;

namespace WaferMovie.Application.Test.Movies.Commands;

public class CreateMovieCommandTest
{
    private readonly IApplicationDbContext dbContext = InMemoryDatabase.Create();
    private readonly IMediator mediator;

    public CreateMovieCommandTest()
    {
        mediator = Services.Configure(dbContext);
    }

    [Fact]
    public async Task ShouldCreateMovie()
    {
        var command = new CreateMovieCommand
        {
            Title = "Black Panther",
            AgeRestriction = MovieAgeRestriction.PG13,
            Description = "T'Challa, heir to the hidden but advanced kingdom of Wakanda, must step forward to lead his people into a new future and must confront a challenger from his country's past.",
            IsFree = true,
            IMDB = "tt1825683",
            Length = 134,
            OutYear = 2018,
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
        var command = new CreateMovieCommand();

        var result = await mediator.Send(command);

        result.Status.ShouldBe(CrudStatus.ValidationError);
        result.Messages.Count.ShouldBe(2);
    }

    [Fact]
    public async Task ShouldRestrictDuplicateIMDB()
    {
        var command = new CreateMovieCommand
        {
            Title = "No Time to Die",
            AgeRestriction = MovieAgeRestriction.PG13,
            Description = "James Bond has left active service. His peace is short-lived when Felix Leiter, an old friend from the CIA, turns up asking for help, leading Bond onto the trail of a mysterious villain armed with dangerous new technology.",
            IMDB = "tt2382320",
            IsFree = false,
            Length = 163,
            OutYear = 2021,
            Unavailable = false,
        };
        var result = await mediator.Send(command);

        result.Status.ShouldBe(CrudStatus.ValidationError);
        result.Messages.Count.ShouldBe(1);
    }
}