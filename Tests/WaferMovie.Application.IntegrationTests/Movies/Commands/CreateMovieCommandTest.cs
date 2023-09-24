using WaferMovie.Application.Movies.Commands.CreateMovie;

namespace WaferMovie.Application.IntegrationTests.Movies.Commands;

public class CreateMovieCommandTest : TestFixture
{
    [Test]
    public async Task ShouldCreateMovie()
    {
        using var dbContext = Resolve<IApplicationDbContext>();
        var mediator = Resolve<IMediator>();

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
        var newMovie = await dbContext.Movies.FirstOrDefaultAsync(f => f.Id == result.Data);

        result.Succeeded.Should().BeTrue();
        result.Status.Should().Be(CrudStatus.Succeeded);

        newMovie!.Description.Should().Be(command.Description);
        newMovie.IsFree.Should().BeTrue();
        newMovie.Length.Should().Be(command.Length);
        newMovie.OutYear.Should().Be(command.OutYear);
        newMovie.Title.Should().Be(command.Title);
        newMovie.Unavailable.Should().BeFalse();
    }

    [Test]
    public async Task ShouldHaveValidationError()
    {
        var mediator = Resolve<IMediator>();
        var command = new CreateMovieCommand();

        var result = await mediator.Send(command);

        result.Status.Should().Be(CrudStatus.ValidationError);
        result.Messages.Count.Should().Be(2);
    }

    [Test]
    public async Task ShouldRestrictDuplicateIMDB()
    {
        var mediator = Resolve<IMediator>();
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

        result.Status.Should().Be(CrudStatus.ValidationError);
        result.Messages.Count.Should().Be(1);
    }
}