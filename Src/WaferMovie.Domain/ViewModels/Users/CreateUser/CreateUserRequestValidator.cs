using System.Text.RegularExpressions;

namespace WaferMovie.Domain.ViewModels.Users.CreateUser;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator(ILocalizationService localization)
    {
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

        RuleFor(r => r.Password).NotEmpty()
            .WithMessage(m => localization.FromValidationResources(ErrorMessages.IS_REQUIRED, localization.FromPropertyResources(nameof(m.Password))))
            .Matches(new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d\!\@\#\$\^\&\*\-]{8,}$"))
            .WithMessage(m => localization.FromValidationResources("{0} must contain at least one letter and one number", localization.FromPropertyResources(nameof(m.Password))));

        RuleFor(r => r.PasswordConfirmation).NotEmpty()
            .WithMessage(m => localization.FromValidationResources(ErrorMessages.IS_REQUIRED, localization.FromPropertyResources(nameof(m.PasswordConfirmation))))
            .Equal(r => r.Password)
            .WithMessage(m => localization.FromValidationResources("{0} must match {1}", localization.FromPropertyResources(nameof(m.Password)), localization.FromPropertyResources(nameof(m.PasswordConfirmation))));
    }
}
