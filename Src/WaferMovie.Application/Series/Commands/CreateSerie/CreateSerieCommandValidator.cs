namespace WaferMovie.Application.Series.Commands.CreateSerie;

public class CreateSerieCommandValidator : SerieCoreModelValidator<CreateSerieCommand>
{
    private readonly IApplicationDbContext dbContext;

    public CreateSerieCommandValidator(IApplicationDbContext dbContext, ILocalizationService localization) : base(localization)
    {
        this.dbContext = dbContext;

        RuleFor(r => r.IMDB)
            .NotEmpty()
            .WithMessage(m => string.Format(localization.FromValidationResources("{0} is required"), localization.FromPropertyResources(nameof(m.IMDB))))
            .MustAsync(IMDBAllowed)
            .WithMessage(m => string.Format(localization.FromValidationResources("Another {0} exists with this {1}"), localization.FromPropertyResources("serie"), localization.FromPropertyResources(nameof(m.IMDB))));
    }

    private async Task<bool> IMDBAllowed(string imdb, CancellationToken cancellationToken)
        => !(await dbContext.Series.AnyAsync(a => a.IMDB == imdb, cancellationToken));
}