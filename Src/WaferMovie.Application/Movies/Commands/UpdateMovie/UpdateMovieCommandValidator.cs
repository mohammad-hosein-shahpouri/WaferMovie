namespace WaferMovie.Application.Movies.Commands.UpdateMovie;

public class UpdateMovieCommandValidator : MovieCoreModelValidator<UpdateMovieCommand>
{
    private readonly IApplicationDbContext dbContext;

    public UpdateMovieCommandValidator(IApplicationDbContext dbContext, ILocalizationService localization) : base(localization)
    {
        this.dbContext = dbContext;

        RuleFor(r => r.Id).NotEmpty()
            .WithMessage(m => localization.FromValidationResources("{0} is required", localization.FromPropertyResources(nameof(m.Id))));
    }
}