using System.Text.RegularExpressions;

namespace WaferMovie.Application.Users.Commands.CreateUser;

public class CreateUserCommandValidator : UserCoreModelValidator<CreateUserCommand>
{
    private readonly IApplicationDbContext dbContext;

    public CreateUserCommandValidator(IApplicationDbContext dbContext, ILocalizationService localization) : base(localization)
    {
        this.dbContext = dbContext;

        RuleFor(r => r.Email).MustAsync(EmailAllowedAsync)
            .WithMessage(m => string.Format(localization.FromValidationResources("Another {0} exists with this {1}"), localization.FromPropertyResources("user"), localization.FromPropertyResources(nameof(m.Email))));

        RuleFor(r => r.UserName).MustAsync(UserNameAllowedAsync)
            .WithMessage(m => string.Format(localization.FromValidationResources("Another {0} exists with this {1}"), localization.FromPropertyResources("user"), localization.FromPropertyResources(nameof(m.UserName))));

        RuleFor(r => r.PhoneNumber).MustAsync(PhoneNumberAllowedAsync)
            .WithMessage(m => string.Format(localization.FromValidationResources("Another {0} exists with this {1}"), localization.FromPropertyResources("user"), localization.FromPropertyResources(nameof(m.PhoneNumber))));

        RuleFor(r => r.Password).NotEmpty()
            .WithMessage(m => string.Format(localization.FromValidationResources("{0} is required"), localization.FromPropertyResources(nameof(m.Password))))
            .Matches(new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d\!\@\#\$\^\&\*\-]{8,}$"))
            .WithMessage(m => string.Format(localization.FromValidationResources("{0} must contain at least one letter and one number"), nameof(m.Password)));

        RuleFor(r => r.PasswordConfirmation).NotEmpty()
            .WithMessage(m => string.Format(localization.FromValidationResources("{0} is required"), nameof(m.PasswordConfirmation)))
            .Equal(r => r.Password)
            .WithMessage(m => string.Format(localization.FromValidationResources("{0} must match {1}"), nameof(m.Password), nameof(m.PasswordConfirmation)));
    }

    private async Task<bool> EmailAllowedAsync(string email, CancellationToken cancellationToken)
         => !(await dbContext.Users.AnyAsync(u => u.NormalizedEmail == email.ToUpper(), cancellationToken));

    private async Task<bool> PhoneNumberAllowedAsync(string phoneNumber, CancellationToken cancellationToken)
         => !(await dbContext.Users.AnyAsync(u => u.PhoneNumber == phoneNumber, cancellationToken));

    private async Task<bool> UserNameAllowedAsync(string userName, CancellationToken cancellationToken)
         => !(await dbContext.Users.AnyAsync(u => u.NormalizedUserName == userName.ToUpper(), cancellationToken));
}