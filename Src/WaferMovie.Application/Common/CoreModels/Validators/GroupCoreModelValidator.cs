namespace WaferMovie.Application.Common.CoreModels.Validators;

public class GroupCoreModelValidator<T> : AbstractValidator<T> where T : GroupCoreModel
{
    public GroupCoreModelValidator(ILocalizationService localizationService)
    {
        RuleFor(r => r.Name).NotEmpty()
            .WithMessage(m => localizationService.FromValidationResources("{0} is required", localizationService.FromPropertyResources(nameof(m.Name))))
            .MaximumLength(100)
            .WithMessage(m => localizationService.FromValidationResources("{0} can not be longer than {{MaxLength}} characters", localizationService.FromPropertyResources(nameof(m.Name))));

        RuleFor(r => r.Description).MaximumLength(500)
            .WithMessage(m => localizationService.FromValidationResources("{0} can not be longer than {{MaxLength}} characters", localizationService.FromPropertyResources(nameof(m.Description))));
    }
}