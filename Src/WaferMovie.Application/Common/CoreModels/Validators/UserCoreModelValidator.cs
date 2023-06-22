using System.Text.RegularExpressions;

namespace WaferMovie.Application.Common.CoreModels.Validators;

public abstract class UserCoreModelValidator<T> : AbstractValidator<T> where T : UserCoreModel
{
    public UserCoreModelValidator(ILocalizationService localization)
    {
        RuleFor(r => r.Name).NotEmpty()
            .WithMessage(m => localization.FromValidationResources("{0} is required", localization.FromPropertyResources(nameof(m.Name))))
            .MaximumLength(30)
            .WithMessage(m => localization.FromValidationResources("{0} can not be longer than {1} characters", localization.FromPropertyResources(nameof(m.Name)), 30));

        RuleFor(r => r.Email).NotEmpty()
            .WithMessage(m => localization.FromValidationResources("{0} is required", localization.FromPropertyResources(nameof(m.Email))))
            .EmailAddress()
            .WithMessage(m => localization.FromValidationResources("{0} is invalid", localization.FromPropertyResources(nameof(m.Email))));

        RuleFor(r => r.PhoneNumber).NotEmpty()
            .WithMessage(m => localization.FromValidationResources("{0} is required", localization.FromPropertyResources(nameof(m.PhoneNumber))))
            .Matches(new Regex(""))
            .WithMessage(m => localization.FromValidationResources("{0} is invalid", localization.FromPropertyResources(nameof(m.PhoneNumber))));

        RuleFor(r => r.UserName).NotEmpty()
            .WithMessage(m => localization.FromValidationResources("{0} is required", localization.FromPropertyResources(nameof(m.UserName))))
            .Matches(new Regex(""))
            .WithMessage(m => localization.FromValidationResources("{0} is invalid", localization.FromPropertyResources(nameof(m.UserName))));
    }
}