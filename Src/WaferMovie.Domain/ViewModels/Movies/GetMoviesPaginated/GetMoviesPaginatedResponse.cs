using WaferMovie.Domain.Attributes;

namespace WaferMovie.Domain.ViewModels.Movies.GetMoviesPaginated;

public record GetMoviesPaginatedResponse : IRegister
{
    [PaginationColumn(DataType = EnumDataType.Guid, Hidden = true)]
    public Guid Id { get; set; }

    [Searchable]
    [PaginationColumn(DataType = EnumDataType.String)]
    public string IMDB { get; set; } = default!;

    [Searchable]
    [PaginationColumn(DataType = EnumDataType.String)]
    public string Title { get; set; } = default!;

    [PaginationColumn(DataType = EnumDataType.String)]
    public string Description { get; set; } = default!;


    [Searchable]
    [PaginationColumn(DataType = EnumDataType.Boolean)]
    public bool IsFree { get; set; }

    [Searchable]
    [PaginationColumn(DataType = EnumDataType.Number)]
    public int OutYear { get; set; }

    [Searchable]
    [PaginationColumn(DataType = EnumDataType.Enum)]
    public EnumMovieAgeRestriction AgeRestriction { get; set; }


    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Movie, GetMoviesPaginatedResponse>();
    }
}
