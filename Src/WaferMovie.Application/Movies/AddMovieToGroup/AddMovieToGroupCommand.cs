using WaferMovie.Domain.Common;
using WaferMovie.Domain.Interfaces;

namespace WaferMovie.Application.Movies.AddMovieToGroup;

public record AddMovieToGroupCommand(int MovieId, int GroupId) : IRequest<ApiResponse>;

public class AddMovieToGroupCommandHandler : IRequestHandler<AddMovieToGroupCommand, ApiResponse>
{
    private readonly IApplicationDbContext dbContext;

    public AddMovieToGroupCommandHandler(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<ApiResponse> Handle(AddMovieToGroupCommand request, CancellationToken cancellationToken)
    {
        var movieGroup = request.Adapt<MovieGroup>();
        await dbContext.MovieGroups.AddAsync(movieGroup!, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse(EnumApiResponseStatus.Success);
    }
}