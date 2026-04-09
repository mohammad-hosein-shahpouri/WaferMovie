namespace WaferMovie.Domain.Entities;

public class SerieRate : BaseEntity, IEntityTypeConfiguration<SerieRate>
{
    public Guid SerieId { get; set; }
    public Guid UserId { get; set; }

    public byte Score { get; set; }


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