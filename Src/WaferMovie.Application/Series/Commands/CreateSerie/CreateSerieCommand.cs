﻿using StackExchange.Redis;

namespace WaferMovie.Application.Series.Commands.CreateSerie;

[ValidateNever]
public record CreateSerieCommand : SerieCoreModel, IRequest<CrudResult<int>>
{
    public string IMDB { get; set; } = default!;
    public SerieAgeRestriction AgeRestriction { get; set; }
}

public class CreateSerieCommandHandler : IRequestHandler<CreateSerieCommand, CrudResult<int>>
{
    private readonly IApplicationDbContext dbContext;
    private readonly IDatabase cacheDb;

    public CreateSerieCommandHandler(IApplicationDbContext dbContext, IDatabase cacheDb)
    {
        this.dbContext = dbContext;
        this.cacheDb = cacheDb;
    }

    public async Task<CrudResult<int>> Handle(CreateSerieCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<Serie>();

        await dbContext.Series.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        await cacheDb.KeyDeleteAsync("WaferMovie:Series:All");

        return new CrudResult<int>(CrudStatus.Succeeded, entity.Id);
    }
}