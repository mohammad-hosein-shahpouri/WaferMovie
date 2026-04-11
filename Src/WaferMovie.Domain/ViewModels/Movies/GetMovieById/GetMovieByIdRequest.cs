namespace WaferMovie.Domain.ViewModels.Movies.GetMovieById;

public record GetMovieByIdRequest : IRequest<ApiResponse<GetMovieByIdResponse>>
{
    public required Guid Id { get; set; }
}
