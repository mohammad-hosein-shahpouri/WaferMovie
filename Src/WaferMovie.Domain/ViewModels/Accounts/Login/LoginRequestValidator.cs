namespace WaferMovie.Domain.ViewModels.Accounts.Login;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator(ILocalizationService localizationService)
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage(m => localizationService.FromValidationResources(ErrorMessages.IS_REQUIRED,
                localizationService.FromPropertyResources(nameof(m.Email))))
            .EmailAddress()
            .WithMessage(m => localizationService.FromValidationResources(ErrorMessages.IS_INVALID,
                localizationService.FromPropertyResources(nameof(m.Email))));


        RuleFor(r => r.Password)
            .NotEmpty()
            .WithMessage(m => localizationService.FromValidationResources(ErrorMessages.IS_REQUIRED,
                localizationService.FromPropertyResources(nameof(m.Password))));
    }
}
