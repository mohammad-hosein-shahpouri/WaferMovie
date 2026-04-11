using WaferMovie.Domain.ViewModels.Movies.CreateMovie;

namespace WaferMovie.Application.Movies.CreateMovie;

public class CreateMovieRequestHandler(IApplicationDbContext dbContext, IDatabase cacheDb) : IRequestHandler<CreateMovieRequest, ApiResponse<Guid>>
{
    public async Task<ApiResponse<Guid>> Handle(CreateMovieRequest request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<Movie>();

        await dbContext.Movies.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        await cacheDb.KeyDeleteAsync("WaferMovie:Movies:All");

        return new ApiResponse<Guid>(EnumApiResponseStatus.Success, entity.Id);
    }
}