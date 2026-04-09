using Microsoft.AspNetCore.Identity;
using WaferMovie.Domain.Common;
using WaferMovie.Domain.Interfaces;
using WaferMovie.Domain.ViewModels.Accounts.Login;

namespace WaferMovie.Application.Accounts.Login;

public class LoginHandler(IApplicationDbContext dbContext, IPasswordHasher<User> passwordHasher, ITokenServices tokenServices) : IRequestHandler<LoginRequest, ApiResponse<LoginResponse>>
{
    public async Task<ApiResponse<LoginResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(x => x.Email!.Equals(request.Email, StringComparison.CurrentCultureIgnoreCase), cancellationToken);
        if (user == null) return new ApiResponse<LoginResponse>(EnumApiResponseStatus.Failure);
        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, request.Password);
        if (result != PasswordVerificationResult.Success) return new ApiResponse<LoginResponse>(EnumApiResponseStatus.Failure);

        var dto = user.Adapt<LoginResponse>();
        dto.Token = tokenServices.GenerateJwtAsync(user);

        return new ApiResponse<LoginResponse>(EnumApiResponseStatus.Success, dto);
    }
}