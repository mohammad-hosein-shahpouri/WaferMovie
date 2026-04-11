namespace WaferMovie.Domain.ViewModels.Groups.DeleteGroup;

public class DeleteGroupRequest : IRequest<ApiResponse>
{
    public required Guid Id { get; set; }
}
