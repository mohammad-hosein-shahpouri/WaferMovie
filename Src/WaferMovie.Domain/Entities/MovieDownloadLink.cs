namespace WaferMovie.Domain.Entities;

public class MovieDownloadLink : IEntityTypeConfiguration<MovieDownloadLink>
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MovieId { get; set; }

    public string Quality { get; set; } = default!;
    public string? Encoder { get; set; }
    public string Link { get; set; } = default!;
    public string? QualityExampleLink { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public bool Dubbed { get; set; }
    public string? Size { get; set; }

    public virtual Movie Movie { get; set; } = default!;

    public void Configure(EntityTypeBuilder<MovieDownloadLink> builder)
    {
        builder.HasKey(pk => new { pk.Id, pk.MovieId });
        builder.HasOne(o => o.Movie)
            .WithMany(m => m.DownloadLinks)
            .HasForeignKey(fk => fk.MovieId);
    }
}