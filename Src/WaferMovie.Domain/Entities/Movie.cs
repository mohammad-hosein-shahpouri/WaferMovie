namespace WaferMovie.Domain.Entities;

public class Movie : IEntityTypeConfiguration<Movie>
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string IMDB { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime CreatedOn { get; set; } = DateTime.Now;

    public bool Unavailable { get; set; }
    public int Length { get; set; }
    public bool IsFree { get; set; }
    public int OutYear { get; set; } = DateTime.Now.Year;

    // TODO: MovieAgeRestriction Enum
    // public MovieAgeRestriction AgeRestriction { get; set; }

    public virtual ICollection<MovieDownloadLink> DownloadLinks { get; set; } = new List<MovieDownloadLink>();

    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(pk => pk.Id);

        builder.Property(p => p.IMDB).HasMaxLength(20);
        builder.HasIndex(i => i.IMDB).IsUnique();
    }
}