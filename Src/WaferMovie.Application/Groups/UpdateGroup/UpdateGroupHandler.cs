using WaferMovie.Domain.ViewModels.Groups.UpdateGroup;

namespace WaferMovie.Application.Groups.UpdateGroup;

public class UpdateGroupHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateGroupRequest, ApiResponse<Guid>>
{
    public async Task<ApiResponse<Guid>> Handle(UpdateGroupRequest request, CancellationToken cancellationToken)
    {
        var group = await dbContext.Groups.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        if (group == null) return new ApiResponse<Guid>(EnumApiResponseStatus.NotFound);

        request.Adapt(group);
        dbContext.Groups.Update(group);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse<Guid>(EnumApiResponseStatus.Success, group.Id);
    }
}