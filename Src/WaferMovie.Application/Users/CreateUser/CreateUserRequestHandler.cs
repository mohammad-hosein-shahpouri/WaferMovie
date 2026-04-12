using Microsoft.AspNetCore.Identity;
using WaferMovie.Domain.ViewModels.Users.CreateUser;

namespace WaferMovie.Application.Users.CreateUser;

public class CreateUserRequestHandler(IApplicationDbContext dbContext, IPasswordHasher<User> passwordHasher) : IRequestHandler<CreateUserRequest, ApiResponse<Guid>>
{
    public async Task<ApiResponse<Guid>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = request.Adapt<User>();
        user.NormalizedEmail = request.Email.ToUpper();
        user.NormalizedUserName = request.UserName.ToUpper();
        user.ConcurrencyStamp = Guid.NewGuid().ToString();
        user.SecurityStamp = Guid.NewGuid().ToString();
        var passwordHash = passwordHasher.HashPassword(user, request.Password);

        user.PasswordHash = passwordHash;

        await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse<Guid>(EnumApiResponseStatus.Success, user.Id);
    }
}