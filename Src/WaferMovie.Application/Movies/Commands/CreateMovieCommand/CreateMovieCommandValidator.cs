namespace WaferMovie.Application.Movies.Commands.CreateMovieCommand;

public class CreateMovieCommandValidator : MovieCoreModelValidator<CreateMovieCommand>
{
    private readonly IApplicationDbContext dbContext;

    public CreateMovieCommandValidator(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;

        // TODO: CreateMovieCommandValidator Messages
        RuleFor(r => r.IMDB)
            .NotEmpty()
            .WithMessage("IMDB Required")
            .MustAsync(IMDBAllowed)
            .WithMessage("Duplicate IMDB");
    }

    private async Task<bool> IMDBAllowed(string imdbId, CancellationToken cancellationToken)
        => !(await dbContext.Movies.AnyAsync(a => a.IMDB == imdbId, cancellationToken));
}