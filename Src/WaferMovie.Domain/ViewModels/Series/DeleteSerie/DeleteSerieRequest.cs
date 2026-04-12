namespace WaferMovie.Domain.ViewModels.Series.DeleteSerie;

public record DeleteSerieRequest : IRequest<ApiResponse>
{
    public required Guid Id { get; set; }
}
