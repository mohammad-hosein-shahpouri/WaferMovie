namespace WaferMovie.Domain.Entities;

public class Episode : IBaseEntity, IBaseAuditableEntity, IBaseSoftDeleteEntity, IEntityTypeConfiguration<Episode>
{
    public int Id { get; set; }

    public int SeasonId { get; set; }
    public int EpisodeNumber { get; set; } = 1;
    public bool IsLastEpisode { get; set; } = false;
    public virtual Season Season { get; set; } = default!;
    public virtual ICollection<SerieDownloadLink> DownloadLinks { get; set; } = new List<SerieDownloadLink>();

    #region Adit

    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public DateTime? DeletedOn { get; set; }
    public int? DeletedBy { get; set; }

    #endregion Adit

    public void Configure(EntityTypeBuilder<Episode> builder)
    {
        builder.HasKey(pk => pk.Id);

        builder.HasOne(o => o.Season)
            .WithMany(m => m.Episodes)
            .HasForeignKey(fk => fk.SeasonId);
    }
}