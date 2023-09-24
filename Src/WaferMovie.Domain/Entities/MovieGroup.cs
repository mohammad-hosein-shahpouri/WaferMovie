namespace WaferMovie.Domain.Entities;

public class MovieGroup : IBaseEntity, IEntityTypeConfiguration<MovieGroup>
{
    public int GroupId { get; set; }
    public int MovieId { get; set; }

    #region Adit

    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }

    #endregion Adit

    public virtual Group Group { get; set; } = default!;
    public virtual Movie Movie { get; set; } = default!;

    public void Configure(EntityTypeBuilder<MovieGroup> builder)
    {
        builder.HasKey(mg => new { mg.GroupId, mg.MovieId });

        builder.HasOne(mg => mg.Group)
            .WithMany(g => g.Movies)
            .HasForeignKey(mg => mg.GroupId);

        builder.HasOne(mg => mg.Movie)
            .WithMany(m => m.Groups)
            .HasForeignKey(mg => mg.MovieId);
    }
}