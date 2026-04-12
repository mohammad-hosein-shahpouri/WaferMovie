namespace WaferMovie.Domain.ViewModels.Series.UpdateSerie;

public class UpdateSerieRequestValidator : AbstractValidator<UpdateSerieRequest>
{
    public UpdateSerieRequestValidator(ILocalizationService localizationService)
    {
        RuleFor(r => r.Id)
            .NotEmpty()
            .WithMessage(m => localizationService.FromValidationResources(ErrorMessages.IS_REQUIRED, localizationService.FromPropertyResources(nameof(m.Id))));

        RuleFor(r => r.Title).NotEmpty()
            .WithMessage(m => localizationService.FromValidationResources(ErrorMessages.IS_REQUIRED, localizationService.FromPropertyResources(nameof(m.Title))))
            .MaximumLength(100)
            .WithMessage(m => localizationService.FromValidationResources(ErrorMessages.CAN_NOT_BE_LONGER_THAN, localizationService.FromPropertyResources(nameof(m.Title))));

        RuleFor(r => r.Description).MaximumLength(500)
            .WithMessage(m => localizationService.FromValidationResources(ErrorMessages.CAN_NOT_BE_LONGER_THAN, localizationService.FromPropertyResources(nameof(m.Description))));

    }
}
