namespace WaferMovie.Domain.ViewModels.Movies.CreateMovie;

public class CreateMovieRequestValidator : AbstractValidator<CreateMovieRequest>
{
    public CreateMovieRequestValidator(ILocalizationService localizationService)
    {
        RuleFor(r => r.Title).NotEmpty()
            .WithMessage(m => localizationService.FromValidationResources(ErrorMessages.IS_REQUIRED, localizationService.FromPropertyResources(nameof(m.Title))))
            .MaximumLength(100)
            .WithMessage(m => localizationService.FromValidationResources(ErrorMessages.CAN_NOT_BE_LONGER_THAN, localizationService.FromPropertyResources(nameof(m.Title))));

        RuleFor(r => r.Description).MaximumLength(500)
            .WithMessage(m => localizationService.FromValidationResources(ErrorMessages.CAN_NOT_BE_LONGER_THAN, localizationService.FromPropertyResources(nameof(m.Description))));

        RuleFor(r => r.IMDB).NotEmpty()
            .WithMessage(m => localizationService.FromValidationResources(ErrorMessages.IS_REQUIRED, localizationService.FromPropertyResources(nameof(m.IMDB))));
    }
}
