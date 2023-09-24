namespace WaferMovie.Domain.Entities;

public class MovieDownloadLink : IBaseEntity, IBaseAuditableEntity, IBaseSoftDeleteEntity, IEntityTypeConfiguration<MovieDownloadLink>
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int MovieId { get; set; }

    public string Quality { get; set; } = default!;
    public string? Encoder { get; set; }
    public string Link { get; set; } = default!;
    public string? QualityExampleLink { get; set; }
    public bool Dubbed { get; set; }
    public string? Size { get; set; }

    public virtual Movie Movie { get; set; } = default!;

    #region Adit

    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public DateTime? DeletedOn { get; set; }
    public int? DeletedBy { get; set; }

    #endregion Adit

    public void Configure(EntityTypeBuilder<MovieDownloadLink> builder)
    {
        builder.HasKey(pk => pk.Id);
        builder.HasOne(o => o.Movie)
            .WithMany(m => m.DownloadLinks)
            .HasForeignKey(fk => fk.MovieId);
    }
}