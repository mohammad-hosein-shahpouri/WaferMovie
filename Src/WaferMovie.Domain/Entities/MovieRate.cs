namespace WaferMovie.Domain.Entities;

public class MovieRate : BaseEntity, IEntityTypeConfiguration<MovieRate>
{
    public Guid MovieId { get; set; }
    public Guid UserId { get; set; }
    public byte Score { get; set; }

    public virtual Movie Movie { get; set; } = default!;
    public virtual User User { get; set; } = default!;

    public void Configure(EntityTypeBuilder<MovieRate> builder)
    {
        builder.HasKey(x => new { x.MovieId, x.UserId });
        builder.Property(x => x.Score).IsRequired();

        builder.HasOne(x => x.Movie)
            .WithMany(x => x.Rates)
            .HasForeignKey(x => x.MovieId);

        builder.HasOne(x => x.User)
            .WithMany(x => x.MovieRates)
            .HasForeignKey(x => x.UserId);
    }
}