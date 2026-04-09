namespace WaferMovie.Domain.Entities;

public class Serie : BaseEntity<Guid>, IEntityTypeConfiguration<Serie>
{
    public required string IMDB { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int Length { get; set; }
    public bool IsFree { get; set; }
    public bool Unavailable { get; set; }
    public string? StreamNetwork { get; set; }

    //public Days ShowDay { get; set; }
    public int FirstSeasonYear { get; set; }

    public int? LastSeasonYear { get; set; }
    public EnumSerieAgeRestriction AgeRestriction { get; set; }
    public DateTime? LastEpisodeDate { get; set; } = null;

    public virtual IEnumerable<Season> Seasons { get; set; } = [];
    public virtual IEnumerable<SerieGroup> Groups { get; set; } = [];
    public virtual IEnumerable<SerieRate> Rates { get; set; } = [];

    public void Configure(EntityTypeBuilder<Serie> builder)
    {
        builder.HasKey(pk => pk.Id);
        builder.Property(p => p.IMDB).HasMaxLength(20);
        builder.HasIndex(i => i.IMDB).IsUnique();

        builder.Property(p => p.Title).HasMaxLength(100);
        builder.Property(p => p.Description).HasMaxLength(500);
    }
}