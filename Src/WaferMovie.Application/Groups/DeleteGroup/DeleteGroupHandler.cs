using WaferMovie.Domain.ViewModels.Groups.DeleteGroup;

namespace WaferMovie.Application.Groups.DeleteGroup;

public class DeleteGroupHandler(IApplicationDbContext dbContext) : IRequestHandler<DeleteGroupRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(DeleteGroupRequest request, CancellationToken cancellationToken)
    {
        var group = await dbContext.Groups.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        if (group == null) return new ApiResponse(EnumApiResponseStatus.NotFound);

        group.IsDeleted = true;
        dbContext.Groups.Update(group);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse(EnumApiResponseStatus.Success);
    }
}