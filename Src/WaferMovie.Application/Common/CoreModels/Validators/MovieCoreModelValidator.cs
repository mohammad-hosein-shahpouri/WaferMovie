namespace WaferMovie.Application.Common.CoreModels.Validators;

public abstract class MovieCoreModelValidator<T> : AbstractValidator<T> where T : MovieCoreModel
{
    public MovieCoreModelValidator(ILocalizationService localization)
    {
        RuleFor(r => r.Title).NotEmpty()
            .WithMessage(m => string.Format(localization.FromValidationResources("{0} is required"), localization.FromPropertyResources(nameof(m.Title))))
            .MaximumLength(100)
            .WithMessage(m => string.Format(localization.FromValidationResources("{0} can not be longer than {1} characters"), localization.FromPropertyResources(nameof(m.Title)), 100));

        RuleFor(r => r.Description).MaximumLength(500)
            .WithMessage(m => string.Format(localization.FromValidationResources("{0} can not be longer than {1} characters"), localization.FromPropertyResources(nameof(m.Description)), 500));
    }
}