using WaferMovie.Domain.Common;
using WaferMovie.Domain.Interfaces;

namespace WaferMovie.Application.Series.CreateSerieRate;

public record CreateSerieRateCommand(Guid SerieId, byte Score) : IRequest<ApiResponse>;

public class CreateSerieRateCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService) : IRequestHandler<CreateSerieRateCommand, ApiResponse>
{
    public async Task<ApiResponse> Handle(CreateSerieRateCommand request, CancellationToken cancellationToken)
    {
        var entity = new SerieRate
        {
            SerieId = request.SerieId,
            Score = request.Score,
            UserId = currentUserService.Id
        };

        dbContext.SerieRates.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ApiResponse(EnumApiResponseStatus.Success);
    }
}