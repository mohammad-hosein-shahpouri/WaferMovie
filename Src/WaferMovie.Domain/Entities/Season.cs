namespace WaferMovie.Domain.Entities;

public class Season : IBaseEntity, IBaseAuditableEntity, IBaseSoftDeleteEntity, IEntityTypeConfiguration<Season>
{
    public int Id { get; set; }

    public int SerieId { get; set; }
    public string? Quality { get; set; }
    public int SeasonNumber { get; set; } = 1;
    public string? AverageSize { get; set; }
    public bool IsLastSeason { get; set; } = false;

    #region Adit

    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }

    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public DateTime? DeletedOn { get; set; }
    public int? DeletedBy { get; set; }

    #endregion Adit

    public virtual IEnumerable<Episode> Episodes { get; set; } = new List<Episode>();
    public virtual Serie Serie { get; set; } = default!;

    public void Configure(EntityTypeBuilder<Season> builder)
    {
        builder.HasKey(pk => pk.Id);

        builder.HasOne(o => o.Serie)
            .WithMany(m => m.Seasons)
            .HasForeignKey(fk => fk.SerieId);
    }
}