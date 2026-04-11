namespace WaferMovie.Domain.ViewModels.Movies.CreateMovieRate;

public class CreateMovieRateRequestValidator : AbstractValidator<CreateMovieRateRequest>
{
    public CreateMovieRateRequestValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.MovieId).NotEmpty()
            .WithMessage(m => localizationService.FromValidationResources(ErrorMessages.IS_REQUIRED, localizationService.FromPropertyResources(nameof(m.MovieId))));

        RuleFor(x => x.Score).NotEmpty()
            .WithMessage(m => localizationService.FromValidationResources(ErrorMessages.IS_REQUIRED, localizationService.FromPropertyResources(nameof(m.Score))))
            .InclusiveBetween((byte)1, (byte)10)
            .WithMessage(m => localizationService.FromValidationResources(ErrorMessages.MUST_BE_BETWEEN, localizationService.FromPropertyResources(nameof(m.Score))));
    }
}
