namespace WaferMovie.Domain.Entities;

public class SerieDownloadLink : IEntityTypeConfiguration<SerieDownloadLink>
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int EpisodeId { get; set; }

    public string Quality { get; set; } = default!;
    public string? Encoder { get; set; }
    public string Link { get; set; } = default!;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public bool Dubbed { get; set; }
    public string? Size { get; set; }

    public virtual Episode Episode { get; set; } = default!;

    public void Configure(EntityTypeBuilder<SerieDownloadLink> builder)
    {
        builder.HasKey(pk => pk.Id);
        builder.HasOne(o => o.Episode)
            .WithMany(m => m.DownloadLinks)
            .HasForeignKey(fk => fk.EpisodeId);
    }
}