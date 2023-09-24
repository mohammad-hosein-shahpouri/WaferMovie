namespace WaferMovie.Application.Common.CoreModels;

public record SerieCoreModel
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool Unavailable { get; set; }
    public int Length { get; set; }
    public bool IsFree { get; set; }
    public int FirstSeasonYear { get; set; }
    public int? LastSeasonYear { get; set; }
}