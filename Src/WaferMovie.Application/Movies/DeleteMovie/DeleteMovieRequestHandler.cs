using WaferMovie.Domain.ViewModels.Movies.DeleteMovie;

namespace WaferMovie.Application.Movies.DeleteMovie;

public class DeleteMovieRequestHandler(IApplicationDbContext dbContext) : IRequestHandler<DeleteMovieRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(DeleteMovieRequest request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Movies.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        if (entity == null) return new ApiResponse(EnumApiResponseStatus.NotFound);

        dbContext.Movies.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse(EnumApiResponseStatus.Success);
    }
}