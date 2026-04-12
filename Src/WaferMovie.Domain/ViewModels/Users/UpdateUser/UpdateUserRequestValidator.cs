using System.Text.RegularExpressions;

namespace WaferMovie.Domain.ViewModels.Users.UpdateUser;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(m => localization.FromValidationResources(ErrorMessages.IS_REQUIRED, localization.FromPropertyResources(nameof(m.Id))));

        RuleFor(r => r.Name)
            .NotEmpty()
            .WithMessage(m => localization.FromValidationResources(ErrorMessages.IS_REQUIRED, localization.FromPropertyResources(nameof(m.Name))))
            .MaximumLength(30)
            .WithMessage(m => localization.FromValidationResources(ErrorMessages.CAN_NOT_BE_LONGER_THAN, localization.FromPropertyResources(nameof(m.Name))));

        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage(m => localization.FromValidationResources(ErrorMessages.IS_REQUIRED, localization.FromPropertyResources(nameof(m.Email))))
            .EmailAddress()
            .WithMessage(m => localization.FromValidationResources(ErrorMessages.IS_INVALID, localization.FromPropertyResources(nameof(m.Email))));

        RuleFor(r => r.PhoneNumber)
            .NotEmpty()
            .WithMessage(m => localization.FromValidationResources(ErrorMessages.IS_REQUIRED, localization.FromPropertyResources(nameof(m.PhoneNumber))))
            .Matches(new Regex(""))
            .WithMessage(m => localization.FromValidationResources(ErrorMessages.IS_INVALID, localization.FromPropertyResources(nameof(m.PhoneNumber))));

        RuleFor(r => r.UserName)
            .NotEmpty()
            .WithMessage(m => localization.FromValidationResources(ErrorMessages.IS_REQUIRED, localization.FromPropertyResources(nameof(m.UserName))))
            .Matches(new Regex(""))
            .WithMessage(m => localization.FromValidationResources(ErrorMessages.IS_INVALID, localization.FromPropertyResources(nameof(m.UserName))));
    }
}
