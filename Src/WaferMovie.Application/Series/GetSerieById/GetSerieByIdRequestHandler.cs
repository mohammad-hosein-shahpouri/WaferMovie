using WaferMovie.Domain.ViewModels.Series.GetSerieById;

namespace WaferMovie.Application.Series.GetSerieById;

public class GetSerieByIdRequestHandler(IApplicationDbContext dbContext, ILocalizationService localization, IDatabase cacheDb) : IRequestHandler<GetSerieByIdRequest, ApiResponse<GetSerieByIdResponse>>
{
    public async Task<ApiResponse<GetSerieByIdResponse>> Handle(GetSerieByIdRequest request, CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GetSeriesKey(request.Id);
        var cacheResult = await cacheDb.StringGetAsync(cacheKey);
        if (!cacheResult.IsNullOrEmpty)
            return new ApiResponse<GetSerieByIdResponse>(EnumApiResponseStatus.Success, JsonSerializer.Deserialize<GetSerieByIdResponse>(cacheResult.ToString())!);

        var serie = await dbContext.Series.ProjectToType<GetSerieByIdResponse>()
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        if (serie == null)
            return new ApiResponse<GetSerieByIdResponse>(EnumApiResponseStatus.NotFound, localization.FromValidationResources("No {0} found with this {1}", localization.FromPropertyResources("serie"), localization.FromPropertyResources(nameof(request.Id))));

        var cacheValue = JsonSerializer.Serialize(serie);
        await cacheDb.StringSetAsync(cacheKey, cacheValue, TimeSpan.FromDays(1));

        return new ApiResponse<GetSerieByIdResponse>(EnumApiResponseStatus.Success, serie);
    }
}