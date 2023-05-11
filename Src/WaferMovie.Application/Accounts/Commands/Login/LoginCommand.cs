using Microsoft.AspNetCore.Identity;

namespace WaferMovie.Application.Accounts.Commands.Login;

[ValidateNever]
public record LoginCommand : IRequest<CrudResult<LoginCommandDto>>
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public bool IsPersistent { get; set; }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, CrudResult<LoginCommandDto>>
{
    private readonly IApplicationDbContext dbContext;
    private readonly IPasswordHasher<User> passwordHasher;
    private readonly ITokenServices tokenServices;

    public LoginCommandHandler(IApplicationDbContext dbContext, IPasswordHasher<User> passwordHasher, ITokenServices tokenServices)
    {
        this.dbContext = dbContext;
        this.passwordHasher = passwordHasher;
        this.tokenServices = tokenServices;
    }

    public async Task<CrudResult<LoginCommandDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.NormalizedEmail == request.Email.ToUpper(), cancellationToken);
        if (user == null) return new CrudResult<LoginCommandDto>(CrudStatus.Failed);
        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, request.Password);
        if (result != PasswordVerificationResult.Success) return new CrudResult<LoginCommandDto>(CrudStatus.Failed);

        var dto = user.Adapt<LoginCommandDto>();
        dto.Token = tokenServices.GenerateJwtAsync(user);

        return new CrudResult<LoginCommandDto>(CrudStatus.Succeeded, dto);
    }
}