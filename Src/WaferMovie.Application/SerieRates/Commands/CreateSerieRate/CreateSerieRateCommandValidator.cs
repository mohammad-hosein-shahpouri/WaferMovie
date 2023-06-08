namespace WaferSerie.Application.SerieRates.Commands.CreateSerieRate;

public class CreateSerieRateCommandValidator : AbstractValidator<CreateSerieRateCommand>
{
    public CreateSerieRateCommandValidator(ILocalizationService localization)
    {
        RuleFor(x => x.SerieId).NotEmpty().WithMessage(m => localization.FromValidationResources("{0} is required", localization.FromPropertyResources(nameof(m.SerieId))));

        RuleFor(x => x.Score).NotEmpty()
            .WithMessage(m => localization.FromValidationResources("{0} is required", localization.FromPropertyResources(nameof(m.Score))))
            .InclusiveBetween((byte)1, (byte)10).WithMessage(m => localization.FromValidationResources("{0} must be between {{From}} and {{To}}", localization.FromPropertyResources(nameof(m.Score))));
    }
}