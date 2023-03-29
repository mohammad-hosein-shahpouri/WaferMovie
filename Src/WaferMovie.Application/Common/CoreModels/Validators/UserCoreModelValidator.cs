using System.Text.RegularExpressions;

namespace WaferMovie.Application.Common.CoreModels.Validators;

public abstract class UserCoreModelValidator<T> : AbstractValidator<T> where T : UserCoreModel
{
    public UserCoreModelValidator()
    {
        RuleFor(r => r.Name).NotEmpty()
            .WithMessage(m => $"{nameof(m.Name)} is required")
            .MaximumLength(30)
            .WithMessage(m => $"{nameof(m.Name)} can not be longer than 30 characters");

        RuleFor(r => r.Email).NotEmpty()
         .WithMessage(m => $"{nameof(m.Email)} is required")
         .EmailAddress()
         .WithMessage(m => $"{nameof(m.Email)} is invalid");

        RuleFor(r => r.PhoneNumber).NotEmpty()
         .WithMessage(m => $"{nameof(m.PhoneNumber)} is required")
         .Matches(new Regex(""))
         .WithMessage(m => $"{nameof(m.PhoneNumber)} is invalid");

        RuleFor(r => r.UserName).NotEmpty()
         .WithMessage(m => $"{nameof(m.UserName)} is required")
         .Matches(new Regex(""))
         .WithMessage(m => $"{nameof(m.PhoneNumber)} is invalid");
    }
}