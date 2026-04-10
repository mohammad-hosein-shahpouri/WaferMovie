using WaferMovie.Domain.ViewModels.Groups.CreateGroup;

namespace WaferMovie.Application.Groups.CreateGroup;

public class CreateGroupHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateGroupRequest, ApiResponse<Guid>>
{
    public async Task<ApiResponse<Guid>> Handle(CreateGroupRequest request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<Group>();

        await dbContext.Groups.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse<Guid>(EnumApiResponseStatus.Success, entity.Id);
    }
}