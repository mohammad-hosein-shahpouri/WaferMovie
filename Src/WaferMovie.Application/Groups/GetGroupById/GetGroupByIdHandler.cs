using WaferMovie.Domain.ViewModels.Groups.GetGroupById;

namespace WaferMovie.Application.Groups.GetGroupById;

public class GetGroupByIdHandler(IApplicationDbContext dbContext) : IRequestHandler<GetGroupByIdRequest, ApiResponse<GetGroupByIdResponse>>
{
    public async Task<ApiResponse<GetGroupByIdResponse>> Handle(GetGroupByIdRequest request, CancellationToken cancellationToken)
    {
        var group = await dbContext.Groups.ProjectToType<GetGroupByIdResponse>()
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);

        if (group is null)
            return new ApiResponse<GetGroupByIdResponse>(EnumApiResponseStatus.NotFound);

        return new ApiResponse<GetGroupByIdResponse>(EnumApiResponseStatus.Success, group);
    }
}
