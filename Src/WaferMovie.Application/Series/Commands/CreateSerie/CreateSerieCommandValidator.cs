namespace WaferMovie.Application.Series.Commands.CreateSerie;

public class CreateSerieCommandValidator : SerieCoreModelValidator<CreateSerieCommand>
{
    private readonly IApplicationDbContext dbContext;

    public CreateSerieCommandValidator(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;

        RuleFor(r => r.IMDB)
            .NotEmpty()
            .WithMessage(m => $"{nameof(m.IMDB)} is Required")
            .MustAsync(IMDBAllowed)
            .WithMessage((m => $"Another serie exists with this {nameof(m.IMDB)}"));
    }

    private async Task<bool> IMDBAllowed(string imdb, CancellationToken cancellationToken)
        => !(await dbContext.Series.AnyAsync(a => a.IMDB == imdb, cancellationToken));
}