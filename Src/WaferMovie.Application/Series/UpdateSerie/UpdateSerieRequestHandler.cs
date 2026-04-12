using WaferMovie.Domain.ViewModels.Series.UpdateSerie;

namespace WaferMovie.Application.Series.UpdateSerie;

public class UpdateSerieRequestHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateSerieRequest, ApiResponse<Guid>>
{
    public async Task<ApiResponse<Guid>> Handle(UpdateSerieRequest request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Series.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        if (entity == null) return new ApiResponse<Guid>(EnumApiResponseStatus.NotFound);

        entity = request.Adapt(entity);
        dbContext.Series.Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse<Guid>(EnumApiResponseStatus.Success, entity.Id);
    }
}