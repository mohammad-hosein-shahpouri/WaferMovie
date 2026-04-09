namespace WaferMovie.Domain.Entities;

public class Group : BaseEntity<Guid>, IEntityTypeConfiguration<Group>
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public string? ImageThumbnailUrl { get; set; }
    public bool IsPublic { get; set; }
    public bool IsDeleted { get; set; }
    public virtual ICollection<MovieGroup> Movies { get; set; } = [];
    public virtual ICollection<SerieGroup> Series { get; set; } = [];

    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).HasMaxLength(100);
        builder.Property(p => p.Description).HasMaxLength(500);
    }
}