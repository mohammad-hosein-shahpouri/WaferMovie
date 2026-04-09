namespace WaferMovie.Domain.Entities;

public class Season : BaseEntity<Guid>, IEntityTypeConfiguration<Season>
{
    public Guid SerieId { get; set; }
    public string? Quality { get; set; }
    public int SeasonNumber { get; set; } = 1;
    public string? AverageSize { get; set; }
    public bool IsLastSeason { get; set; } = false;

    public virtual IEnumerable<Episode> Episodes { get; set; } = [];
    public virtual Serie Serie { get; set; } = default!;

    public void Configure(EntityTypeBuilder<Season> builder)
    {
        builder.HasKey(pk => pk.Id);

        builder.HasOne(o => o.Serie)
            .WithMany(m => m.Seasons)
            .HasForeignKey(fk => fk.SerieId);
    }
}