namespace WaferMovie.Domain.ViewModels.Accounts.Login;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage(m => string.Format(ErrorMessages.IS_REQUIRED, nameof(m.Email)))
            .EmailAddress()
            .WithMessage(m => string.Format(ErrorMessages.IS_INVALID, nameof(m.Email)));


        RuleFor(r => r.Password)
            .NotEmpty()
            .WithMessage(m => string.Format(ErrorMessages.IS_REQUIRED, nameof(m.Password)));
    }
}
