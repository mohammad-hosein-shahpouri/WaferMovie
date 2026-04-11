namespace WaferMovie.Domain.ViewModels.Movies.DeleteMovie;

public record DeleteMovieRequest : IRequest<ApiResponse>
{
    public required Guid Id { get; set; }
}
