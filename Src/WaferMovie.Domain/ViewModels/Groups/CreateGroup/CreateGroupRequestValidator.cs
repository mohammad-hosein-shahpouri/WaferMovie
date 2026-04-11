namespace WaferMovie.Domain.ViewModels.Groups.CreateGroup;

public class CreateGroupRequestValidator : AbstractValidator<CreateGroupRequest>
{
    public CreateGroupRequestValidator(ILocalizationService localizationService)
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .WithMessage(m => localizationService.FromValidationResources(ErrorMessages.IS_REQUIRED, localizationService.FromPropertyResources(nameof(m.Name))))
            .MaximumLength(1)
            .WithMessage(m => localizationService.FromValidationResources(ErrorMessages.CAN_NOT_BE_LONGER_THAN, localizationService.FromPropertyResources(nameof(m.Name))));

        RuleFor(r => r.Description)
            .MaximumLength(500)
            .WithMessage(m => localizationService.FromValidationResources(ErrorMessages.CAN_NOT_BE_LONGER_THAN, localizationService.FromPropertyResources(nameof(m.Description))));
    }
}
