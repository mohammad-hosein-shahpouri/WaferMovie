namespace WaferMovie.Application.Groups.Commands.CreateGroup;

public class CreateGroupCommandValidator : AbstractValidator<CreateGroupCommand>
{
    public CreateGroupCommandValidator(ILocalizationService localization)
    {
        RuleFor(r => r.Name).NotEmpty()
            .WithMessage(m => string.Format(localization.FromValidationResources("{0} is required"), localization.FromPropertyResources(nameof(m.Name))))
            .MaximumLength(100)
            .WithMessage(m => string.Format(localization.FromValidationResources("{0} can not be longer than {{MaxLength}} characters"), localization.FromPropertyResources(nameof(m.Name))));

        RuleFor(r => r.Description).MaximumLength(500)
            .WithMessage(m => string.Format(localization.FromValidationResources("{0} can not be longer than {{MaxLength}} characters"), localization.FromPropertyResources(nameof(m.Description))));
    }
}