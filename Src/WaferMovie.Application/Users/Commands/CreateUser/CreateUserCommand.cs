using Microsoft.AspNetCore.Identity;

namespace WaferMovie.Application.Users.Commands.CreateUser;

[ValidateNever]
public record CreateUserCommand : UserCoreModel, IRequest<CrudResult<User>>
{
    public string Password { get; set; } = default!;
    public string PasswordConfirmation { get; set; } = default!;
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CrudResult<User>>
{
    private readonly IApplicationDbContext dbContext;
    private readonly IPasswordHasher<User> passwordHasher;

    public CreateUserCommandHandler(IApplicationDbContext dbContext, IPasswordHasher<User> passwordHasher)
    {
        this.dbContext = dbContext;
        this.passwordHasher = passwordHasher;
    }

    public async Task<CrudResult<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = request.Adapt<User>();
        user.NormalizedEmail = request.Email.ToUpper();
        user.NormalizedUserName = request.UserName.ToUpper();

        var passwordHash = passwordHasher.HashPassword(user, request.Password);

        user.PasswordHash = passwordHash;

        await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CrudResult<User>(CrudStatus.Succeeded, user);
    }
}