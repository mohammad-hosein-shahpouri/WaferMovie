using WaferMovie.Domain.ViewModels.Series.DeleteSerie;

namespace WaferMovie.Application.Series.DeleteSerie;


public class DeleteSerieRequestHandler(IApplicationDbContext dbContext) : IRequestHandler<DeleteSerieRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(DeleteSerieRequest request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Series.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        if (entity == null) return new ApiResponse<Guid>(EnumApiResponseStatus.NotFound);

        dbContext.Series.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse<Guid>(EnumApiResponseStatus.Success, entity.Id);
    }
}