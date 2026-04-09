namespace WaferMovie.Domain.Entities;

public class SerieDownloadLink : BaseEntity<Guid>, IEntityTypeConfiguration<SerieDownloadLink>
{
    public Guid EpisodeId { get; set; }
    public required string Quality { get; set; }
    public string? Encoder { get; set; }
    public required string Link { get; set; }
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