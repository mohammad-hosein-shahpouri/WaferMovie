namespace WaferMovie.Application.Movies.Commands.CreateMovie;

public class CreateMovieCommandValidator : MovieCoreModelValidator<CreateMovieCommand>
{
    private readonly IApplicationDbContext dbContext;

    public CreateMovieCommandValidator(IApplicationDbContext dbContext, ILocalizationService localization) : base(localization)
    {
        this.dbContext = dbContext;

        RuleFor(r => r.IMDB)
            .NotEmpty()
            .WithMessage(m => string.Format(localization.FromValidationResources("{0} is required"), localization.FromPropertyResources(nameof(m.IMDB))))
            .MustAsync(IMDBAllowed)
            .WithMessage(m => string.Format(localization.FromValidationResources("Another {0} exists with this {1}"), localization.FromPropertyResources("movie"), localization.FromPropertyResources(nameof(m.IMDB))));
    }

    private async Task<bool> IMDBAllowed(string imdbId, CancellationToken cancellationToken)
        => !(await dbContext.Movies.AnyAsync(a => a.IMDB == imdbId, cancellationToken));
}