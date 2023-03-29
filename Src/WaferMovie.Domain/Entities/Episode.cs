namespace WaferMovie.Domain.Entities;

public class Episode : IEntityTypeConfiguration<Episode>
{
    public int Id { get; set; }

    public int SeasonId { get; set; }
    public int EpisodeNumber { get; set; } = 1;
    public bool IsLastEpisode { get; set; } = false;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public virtual Season Season { get; set; } = default!;
    public virtual ICollection<SerieDownloadLink> DownloadLinks { get; set; } = new List<SerieDownloadLink>();

    public void Configure(EntityTypeBuilder<Episode> builder)
    {
        builder.HasKey(pk => pk.Id);

        builder.HasOne(o => o.Season)
            .WithMany(m => m.Episodes)
            .HasForeignKey(fk => fk.SeasonId);
    }
}