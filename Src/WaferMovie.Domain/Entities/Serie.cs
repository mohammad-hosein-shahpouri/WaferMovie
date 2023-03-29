namespace WaferMovie.Domain.Entities;

public class Serie : IEntityTypeConfiguration<Movie>
{
    public int Id { get; set; }
    public string IMDB { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public int Length { get; set; }
    public bool IsFree { get; set; }
    public bool Unavailable { get; set; }
    public string? StreamNetwork { get; set; }

    //public Days ShowDay { get; set; }
    public int FirstSeasonYear { get; set; } = DateTime.Now.Year;

    public int? LastSeasonYear { get; set; }

    //public TVAgeRestriction AgeRestriction { get; set; }
    public DateTime? LastEpisodeDate { get; set; } = null;

    public virtual IEnumerable<Season> Seasons { get; set; } = new List<Season>();

    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(pk => pk.Id);
        builder.Property(p => p.IMDB).HasMaxLength(20);
        builder.HasIndex(i => i.IMDB).IsUnique();
    }
}