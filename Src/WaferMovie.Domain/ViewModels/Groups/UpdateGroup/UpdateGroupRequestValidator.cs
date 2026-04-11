namespace WaferMovie.Domain.ViewModels.Groups.UpdateGroup;

public class UpdateGroupRequestValidator : AbstractValidator<UpdateGroupRequest>
{
    public UpdateGroupRequestValidator(ILocalizationService localizationService)
    {
        RuleFor(r => r.Name).NotEmpty()
         .WithMessage(m => localizationService.FromValidationResources(ErrorMessages.IS_REQUIRED, localizationService.FromPropertyResources(nameof(m.Name))))
         .MaximumLength(100)
         .WithMessage(m => localizationService.FromValidationResources(ErrorMessages.CAN_NOT_BE_LONGER_THAN, localizationService.FromPropertyResources(nameof(m.Name))));

        RuleFor(r => r.Description).MaximumLength(500)
            .WithMessage(m => localizationService.FromValidationResources(ErrorMessages.CAN_NOT_BE_LONGER_THAN, localizationService.FromPropertyResources(nameof(m.Description))));
    }
}
