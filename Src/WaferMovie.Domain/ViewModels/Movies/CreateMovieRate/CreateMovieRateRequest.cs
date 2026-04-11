namespace WaferMovie.Domain.ViewModels.Movies.CreateMovieRate;

public record CreateMovieRateRequest : IRequest<ApiResponse<Guid>>
{
    [JsonIgnore]
    public Guid MovieId { get; set; }
    public byte Score { get; set; }
}
