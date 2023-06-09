namespace WaferMovie.Application.Movies.Commands.UpdateMovie;

public class UpdateMovieCommandValidator : MovieCoreModelValidator<UpdateMovieCommand>
{
    public UpdateMovieCommandValidator(ILocalizationService localization) : base(localization)
    {
        RuleFor(r => r.Id).NotEmpty()
            .WithMessage(m => localization.FromValidationResources("{0} is required", localization.FromPropertyResources(nameof(m.Id))));
    }
}