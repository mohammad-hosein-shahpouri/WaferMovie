namespace WaferMovie.Domain.Entities;

public class Episode : BaseEntity<Guid>, IEntityTypeConfiguration<Episode>
{
    public Guid SeasonId { get; set; }
    public int EpisodeNumber { get; set; } = 1;
    public bool IsLastEpisode { get; set; } = false;
    public virtual Season Season { get; set; } = default!;
    public virtual ICollection<SerieDownloadLink> DownloadLinks { get; set; } = [];


    public void Configure(EntityTypeBuilder<Episode> builder)
    {
        builder.HasKey(pk => pk.Id);

        builder.HasOne(o => o.Season)
            .WithMany(m => m.Episodes)
            .HasForeignKey(fk => fk.SeasonId);
    }
}