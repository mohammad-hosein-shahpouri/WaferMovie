using WaferMovie.Domain.ViewModels.Users.UpdateUser;

namespace WaferMovie.Application.Users.UpdateUser;

public class UpdateUserRequestHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateUserRequest, ApiResponse<Guid>>
{
    public async Task<ApiResponse<Guid>> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        if (user == null) return new ApiResponse<Guid>(EnumApiResponseStatus.NotFound);

        request.Adapt(user);
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse<Guid>(EnumApiResponseStatus.Success, user.Id);
    }
}