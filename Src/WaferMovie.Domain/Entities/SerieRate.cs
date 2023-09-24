namespace WaferMovie.Domain.Entities;

public class SerieRate : IBaseEntity, IBaseAuditableEntity, IEntityTypeConfiguration<SerieRate>
{
    public int SerieId { get; set; }
    public int UserId { get; set; }

    public byte Score { get; set; }

    #region Adit

    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }

    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }

    #endregion Adit

    public virtual Serie Serie { get; set; } = default!;
    public virtual User User { get; set; } = default!;

    public void Configure(EntityTypeBuilder<SerieRate> builder)
    {
        builder.HasKey(x => new { x.SerieId, x.UserId });
        builder.Property(x => x.Score).IsRequired();

        builder.HasOne(x => x.Serie)
            .WithMany(x => x.Rates)
            .HasForeignKey(x => x.SerieId);

        builder.HasOne(x => x.User)
            .WithMany(x => x.SerieRates)
            .HasForeignKey(x => x.UserId);
    }
}