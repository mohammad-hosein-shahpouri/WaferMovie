using WaferMovie.Domain.Common.Pagination;

namespace WaferMovie.Domain.ViewModels.Movies.GetMoviesPaginated;

public record GetMoviesPaginatedRequest : PaginationInput, IRequest<ApiResponse<PaginationOutput<GetMoviesPaginatedResponse>>>
{
}
