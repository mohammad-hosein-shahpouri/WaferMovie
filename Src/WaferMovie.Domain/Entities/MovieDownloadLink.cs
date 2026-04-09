namespace WaferMovie.Domain.Entities;

public class MovieDownloadLink : BaseEntity<Guid>, IEntityTypeConfiguration<MovieDownloadLink>
{
    public Guid MovieId { get; set; }

    public required string Quality { get; set; }
    public string? Encoder { get; set; }
    public required string Link { get; set; }
    public string? QualityExampleLink { get; set; }
    public bool Dubbed { get; set; }
    public string? Size { get; set; }

    public virtual Movie Movie { get; set; } = default!;


    public void Configure(EntityTypeBuilder<MovieDownloadLink> builder)
    {
        builder.HasKey(pk => pk.Id);
        builder.HasOne(o => o.Movie)
            .WithMany(m => m.DownloadLinks)
            .HasForeignKey(fk => fk.MovieId);
    }
}