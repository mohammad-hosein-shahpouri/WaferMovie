using WaferMovie.Application.Series.Commands.CreateSerie;

namespace WaferMovie.Application.IntegrationTests.Series.Commands;

public class CreateSerieCommandTest : TestFixture
{
    [Test]
    public async Task ShouldCreateSerie()
    {
        using var dbContext = Resolve<IApplicationDbContext>();
        var mediator = Resolve<IMediator>();

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
        var newSerie = await dbContext.Series.FirstOrDefaultAsync(f => f.Id == result.Data);

        result.Succeeded.Should().BeTrue();
        result.Status.Should().Be(CrudStatus.Succeeded);

        newSerie!.Description.Should().Be(command.Description);
        newSerie.IsFree.Should().BeTrue();
        newSerie.Length.Should().Be(command.Length);
        newSerie.FirstSeasonYear.Should().Be(command.FirstSeasonYear);
        newSerie.Title.Should().Be(command.Title);
        newSerie.Unavailable.Should().BeFalse();
    }

    [Test]
    public async Task ShouldRestrictDuplicateIMDB()
    {
        var mediator = Resolve<IMediator>();

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

        result.Status.Should().Be(CrudStatus.ValidationError);
        result.Messages.Count.Should().Be(1);
    }

    [Test]
    public async Task ShouldHaveValidationError()
    {
        var mediator = Resolve<IMediator>();
        var command = new CreateSerieCommand();

        var result = await mediator.Send(command);

        result.Status.Should().Be(CrudStatus.ValidationError);
        result.Messages.Count.Should().Be(2);
    }
}