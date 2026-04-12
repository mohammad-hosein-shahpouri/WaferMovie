namespace WaferMovie.Domain.ViewModels.Series.CreateSerieRate;

public class CreateSerieRateRequestValidator : AbstractValidator<CreateSerieRateRequest>
{
    public CreateSerieRateRequestValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.SerieId).NotEmpty()
            .WithMessage(m => localizationService.FromValidationResources(ErrorMessages.IS_REQUIRED, localizationService.FromPropertyResources(nameof(m.SerieId))));

        RuleFor(x => x.Score).NotEmpty()
            .WithMessage(m => localizationService.FromValidationResources(ErrorMessages.IS_REQUIRED, localizationService.FromPropertyResources(nameof(m.Score))))
            .InclusiveBetween((byte)1, (byte)10)
            .WithMessage(m => localizationService.FromValidationResources(ErrorMessages.MUST_BE_BETWEEN, localizationService.FromPropertyResources(nameof(m.Score))));
    }
}
