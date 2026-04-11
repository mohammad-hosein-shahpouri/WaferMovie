namespace WaferMovie.Domain.ViewModels.Groups.GetGroupById;

public class GetGroupByIdRequest : IRequest<ApiResponse<GetGroupByIdResponse>>
{
    public required Guid Id { get; set; }
}
