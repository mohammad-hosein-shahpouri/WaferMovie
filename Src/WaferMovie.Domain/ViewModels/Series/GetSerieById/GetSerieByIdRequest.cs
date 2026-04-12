namespace WaferMovie.Domain.ViewModels.Series.GetSerieById;

public record GetSerieByIdRequest : IRequest<ApiResponse<GetSerieByIdResponse>>
{
    public required Guid Id { get; set; }
}
