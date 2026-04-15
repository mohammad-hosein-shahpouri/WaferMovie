using WaferMovie.Domain.Common.Extensions;
using WaferMovie.Domain.Common.Pagination;
using WaferMovie.Domain.ViewModels.Movies.GetMoviesPaginated;

namespace WaferMovie.Application.Movies.GetMoviesPaginated;

public class GetMoviesPaginatedRequestHandler(IApplicationDbContext dbContext) : IRequestHandler<GetMoviesPaginatedRequest, ApiResponse<PaginationOutput<GetMoviesPaginatedResponse>>>
{
    public async Task<ApiResponse<PaginationOutput<GetMoviesPaginatedResponse>>> Handle(GetMoviesPaginatedRequest request, CancellationToken cancellationToken)
    {
        var result = await dbContext.Movies
            .ProjectToType<GetMoviesPaginatedResponse>()
            .ToPaginatedListAsync(request);

        return new ApiResponse<PaginationOutput<GetMoviesPaginatedResponse>>(EnumApiResponseStatus.Success, result);
    }
}
