using WaferMovie.Domain.ViewModels.Movies.CreateMovieRate;

namespace WaferMovie.Application.Movies.CreateMovieRate;

public class CreateMovieRateRequestHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService) : IRequestHandler<CreateMovieRateRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(CreateMovieRateRequest request, CancellationToken cancellationToken)
    {
        var entity = new MovieRate
        {
            MovieId = request.MovieId,
            Score = request.Score,
            UserId = currentUserService.Id
        };

        dbContext.MovieRates.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse(EnumApiResponseStatus.Success);
    }
}