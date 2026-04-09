using WaferMovie.Domain.ViewModels.Accounts.GetCurrentUser;

namespace WaferMovie.Application.Accounts.GetCurrentUser;

public class GetCurrentUserHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService,
    IDatabase cacheDb) : IRequestHandler<GetCurrentUserRequest, ApiResponse<GetCurrentUserResponse>>
{
    public async Task<ApiResponse<GetCurrentUserResponse>> Handle(GetCurrentUserRequest request, CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GetUsersKey(currentUserService.Id);
        var cacheResult = await cacheDb.StringGetAsync(cacheKey);
        if (!cacheResult.IsNullOrEmpty)
            return new ApiResponse<GetCurrentUserResponse>(EnumApiResponseStatus.Success,
                JsonSerializer.Deserialize<GetCurrentUserResponse>(cacheResult.ToString())!);

        var user = await dbContext.Users.Where(w => w.Id == currentUserService.Id)
            .ProjectToType<GetCurrentUserResponse>().FirstOrDefaultAsync(cancellationToken);
        if (user == null)
            return new ApiResponse<GetCurrentUserResponse>(EnumApiResponseStatus.NotFound);

        var cacheValue = JsonSerializer.Serialize(user);
        await cacheDb.StringSetAsync(cacheKey, cacheValue, TimeSpan.FromDays(1));

        return new ApiResponse<GetCurrentUserResponse>(EnumApiResponseStatus.Success, user);
    }
}