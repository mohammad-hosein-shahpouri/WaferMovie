using WaferMovie.Domain.ViewModels.Movies.UpdateMovie;

namespace WaferMovie.Application.Movies.UpdateMovie;

public class UpdateMovieRequestHandler(IApplicationDbContext dbContext, IDatabase cacheDb) : IRequestHandler<UpdateMovieRequest, ApiResponse<Guid>>
{
    public async Task<ApiResponse<Guid>> Handle(UpdateMovieRequest request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Movies.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        if (entity == null) return new ApiResponse<Guid>(EnumApiResponseStatus.NotFound);

        entity = request.Adapt(entity);
        dbContext.Movies.Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        await cacheDb.KeyDeleteAsync("WaferMovie:Movies:All");
        return new ApiResponse<Guid>(EnumApiResponseStatus.Success, entity.Id);
    }
}