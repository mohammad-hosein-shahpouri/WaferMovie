namespace WaferMovie.Domain.ViewModels.Users.GetUserById;

public record GetUserByIdRequest : IRequest<ApiResponse<GetUserByIdResponse>>
{
    public required Guid Id { get; set; }
}
