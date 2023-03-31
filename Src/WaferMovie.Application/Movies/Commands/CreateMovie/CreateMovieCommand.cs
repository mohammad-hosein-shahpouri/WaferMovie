﻿namespace WaferMovie.Application.Movies.Commands.CreateMovie;

[ValidateNever]
public record CreateMovieCommand(string IMDB) : MovieCoreModel, IRequest<CrudResult<Movie>>;

public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, CrudResult<Movie>>
{
    private readonly IApplicationDbContext dbContext;

    public CreateMovieCommandHandler(IApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<CrudResult<Movie>> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<Movie>();

        await dbContext.Movies.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CrudResult<Movie>(CrudStatus.Succeeded, entity);
    }
}