using WaferMovie.Application.Movies.Commands.CreateMovie;
using WaferMovie.Application.Series.Commands.CreateSerie;

namespace WaferMovie.Application.Test.Series.Commands;

public class CreateSerieCommandTest
{
    private readonly IApplicationDbContext dbContext = InMemoryDatabase.Create();
    private readonly IMediator mediator;

    public CreateSerieCommandTest()
    {
        mediator = Services.Configure(dbContext);
    }

    [Fact]
    public async Task ShouldCreateSerie()
    {
        var command = new CreateSerieCommand
        {
            AgeRestriction = SerieAgeRestriction.TVMA,
            Description = "An unusual group of robbers attempt to carry out the most perfect robbery in Spanish history - stealing 2.4 billion euros from the Royal Mint of Spain.",
            FirstSeasonYear = 2017,
            IMDB = "tt6468322",
            IsFree = true,
            Length = 60,
            Title = "Money Heist",
            Unavailable = false
        };

        var result = await mediator.Send(command);
        var newSerie = await dbContext.Series.FirstOrDefaultAsync(f => f.Id == result.Data.Id);

        result.Succeeded.ShouldBeTrue();
        result.Status.ShouldBe(CrudStatus.Succeeded);

        newSerie!.Description.ShouldBe(command.Description);
        newSerie.IsFree.ShouldBeTrue();
        newSerie.Length.ShouldBe(command.Length);
        newSerie.FirstSeasonYear.ShouldBe(command.FirstSeasonYear);
        newSerie.Title.ShouldBe(command.Title);
        newSerie.Unavailable.ShouldBeFalse();
    }

    [Fact]
    public async Task ShouldRestrictDuplicateIMDB()
    {
        var command = new CreateSerieCommand
        {
            Title = "How I Met Your Mother",
            AgeRestriction = SerieAgeRestriction.TV14,
            Description = "A father recounts to his children - through a series of flashbacks - the journey he and his four best friends took leading up to him meeting their mother.",
            IMDB = "tt0460649",
            IsFree = false,
            FirstSeasonYear = 2005,
            Length = 22,
            Unavailable = true
        };

        var result = await mediator.Send(command);

        result.Status.ShouldBe(CrudStatus.ValidationError);
        result.Messages.Count.ShouldBe(1);
    }

    [Fact]
    public async Task ShouldHaveValidationError()
    {
        var command = new CreateSerieCommand();

        var result = await mediator.Send(command);

        result.Status.ShouldBe(CrudStatus.ValidationError);
        result.Messages.Count.ShouldBe(2);
    }
}