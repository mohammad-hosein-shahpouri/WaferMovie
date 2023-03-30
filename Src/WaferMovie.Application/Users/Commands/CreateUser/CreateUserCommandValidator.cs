using System.Text.RegularExpressions;

namespace WaferMovie.Application.Users.Commands.CreateUser;

public class CreateUserCommandValidator : UserCoreModelValidator<CreateUserCommand>
{
    private readonly IApplicationDbContext dbContext;

    public CreateUserCommandValidator(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;

        RuleFor(r => r.Email)
            .MustAsync(EmailAllowedAsync)
            .WithMessage(m => "Another user already exists with this email");

        RuleFor(r => r.UserName)
            .MustAsync(UserNameAllowedAsync)
            .WithMessage(m => "Another user already exists with this username");

        RuleFor(r => r.PhoneNumber)
            .MustAsync(PhoneNumberAllowedAsync)
            .WithMessage(m => "Another user already exists with this phone number");

        RuleFor(r => r.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .Matches(new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d\!\@\#\$\^\&\*\-]{8,}$"))
            .WithMessage("Password must contain at least one letter and one number");

        RuleFor(r => r.PasswordConfirmation)
            .NotEmpty()
            .WithMessage("Password confirmation is required")
            .Equal(r => r.Password)
            .WithMessage("Password confirmation must match password");
    }

    private async Task<bool> EmailAllowedAsync(string email, CancellationToken cancellationToken)
         => !(await dbContext.Users.AnyAsync(u => u.NormalizedEmail == email.ToUpper(), cancellationToken));

    private async Task<bool> PhoneNumberAllowedAsync(string phoneNumber, CancellationToken cancellationToken)
         => !(await dbContext.Users.AnyAsync(u => u.PhoneNumber == phoneNumber, cancellationToken));

    private async Task<bool> UserNameAllowedAsync(string userName, CancellationToken cancellationToken)
         => !(await dbContext.Users.AnyAsync(u => u.NormalizedUserName == userName.ToUpper(), cancellationToken));
}