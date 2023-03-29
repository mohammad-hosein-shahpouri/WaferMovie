namespace WaferMovie.Application.Movies.Commands.CreateMovieCommand;

public class CreateMovieCommandValidator : MovieCoreModelValidator<CreateMovieCommand>
{
    private readonly IApplicationDbContext dbContext;

    public CreateMovieCommandValidator(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;

        RuleFor(r => r.IMDB)
            .NotEmpty()
            .WithMessage(m => $"{nameof(m.IMDB)} is Required")
            .MustAsync(IMDBAllowed)
            .WithMessage((m => $"Another movie exists with this {nameof(m.IMDB)}"));
    }

    private async Task<bool> IMDBAllowed(string imdbId, CancellationToken cancellationToken)
        => !(await dbContext.Movies.AnyAsync(a => a.IMDB == imdbId, cancellationToken));
}