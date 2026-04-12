using WaferMovie.Domain.ViewModels.Series.CreateSerie;

namespace WaferMovie.Application.Series.CreateSerie;

public class CreateSerieRequestHandler(IApplicationDbContext dbContext, IDatabase cacheDb) : IRequestHandler<CreateSerieRequest, ApiResponse<Guid>>
{
    public async Task<ApiResponse<Guid>> Handle(CreateSerieRequest request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<Serie>();

        await dbContext.Series.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        await cacheDb.KeyDeleteAsync("WaferMovie:Series:All");

        return new ApiResponse<Guid>(EnumApiResponseStatus.Success, entity.Id);
    }
}