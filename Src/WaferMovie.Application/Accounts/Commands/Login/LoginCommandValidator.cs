namespace WaferMovie.Application.Accounts.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.Email).NotEmpty()
            .WithMessage(m => localizationService.FromValidationResources("{0} is required", localizationService.FromPropertyResources(nameof(m.Email))))
            .EmailAddress()
            .WithMessage(m => localizationService.FromValidationResources("{0} is invalid", localizationService.FromPropertyResources(nameof(m.Email))));

        RuleFor(x => x.Password).NotEmpty()
            .WithMessage(m => localizationService.FromValidationResources("{0} is required", localizationService.FromPropertyResources(nameof(m.Password))));
    }
}