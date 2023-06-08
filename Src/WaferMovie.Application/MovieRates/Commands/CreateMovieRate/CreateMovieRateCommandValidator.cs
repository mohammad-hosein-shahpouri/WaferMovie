namespace WaferMovie.Application.MovieRates.Commands.CreateMovieRate;

public class CreateMovieRateCommandValidator : AbstractValidator<CreateMovieRateCommand>
{
    public CreateMovieRateCommandValidator(ILocalizationService localization)
    {
        RuleFor(x => x.MovieId).NotEmpty().WithMessage(m => localization.FromValidationResources("{0} is required", localization.FromPropertyResources(nameof(m.MovieId))));

        RuleFor(x => x.Score).NotEmpty()
            .WithMessage(m => localization.FromValidationResources("{0} is required", localization.FromPropertyResources(nameof(m.Score))))
            .InclusiveBetween((byte)1, (byte)10).WithMessage(m => localization.FromValidationResources("{0} must be between {{From}} and {{To}}", localization.FromPropertyResources(nameof(m.Score))));
    }
}