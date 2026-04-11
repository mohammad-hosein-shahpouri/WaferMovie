using WaferMovie.Domain.ViewModels.Movies.GetMovieById;

namespace WaferMovie.Application.Movies.GetMovieById;

public class GetMovieByIdRequestHandler(IApplicationDbContext dbContext, ILocalizationService localization, IDatabase cacheDb) : IRequestHandler<GetMovieByIdRequest, ApiResponse<GetMovieByIdResponse>>
{
    public async Task<ApiResponse<GetMovieByIdResponse>> Handle(GetMovieByIdRequest request, CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.GetMoviesKey(request.Id);
        var cacheResult = await cacheDb.StringGetAsync(cacheKey);
        if (!cacheResult.IsNullOrEmpty)
            return new ApiResponse<GetMovieByIdResponse>(EnumApiResponseStatus.Success, JsonSerializer.Deserialize<GetMovieByIdResponse>(cacheResult.ToString())!);

        var movie = await dbContext.Movies.Include(i => i.Rates).ProjectToType<GetMovieByIdResponse>()
            .FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        if (movie == null)
            return new ApiResponse<GetMovieByIdResponse>(EnumApiResponseStatus.NotFound,
                localization.FromValidationResources(ErrorMessages.IS_NOT_FOUND, localization.FromPropertyResources("movie"), localization.FromPropertyResources(nameof(request.Id))));

        var cacheValue = JsonSerializer.Serialize(movie);
        await cacheDb.StringSetAsync(cacheKey, cacheValue, TimeSpan.FromDays(1));

        return new ApiResponse<GetMovieByIdResponse>(EnumApiResponseStatus.Success, movie);
    }
}