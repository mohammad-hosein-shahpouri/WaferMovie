namespace WaferMovie.Domain.ViewModels.Series.CreateSerieRate;

public record CreateSerieRateRequest : IRequest<ApiResponse<Guid>>
{
    [JsonIgnore]
    public Guid SerieId { get; set; }
    public byte Score { get; set; }
}
