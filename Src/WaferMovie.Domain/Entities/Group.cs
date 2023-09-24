namespace WaferMovie.Domain.Entities;

public class Group : IBaseEntity, IBaseAuditableEntity, IBaseSoftDeleteEntity, IEntityTypeConfiguration<Group>
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public string? ImageThumbnailUrl { get; set; }
    public bool IsPublic { get; set; }
    public bool IsDeleted { get; set; }
    public virtual ICollection<MovieGroup> Movies { get; set; } = new List<MovieGroup>();
    public virtual ICollection<SerieGroup> Series { get; set; } = new List<SerieGroup>();

    #region Adit

    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public DateTime? DeletedOn { get; set; }
    public int? DeletedBy { get; set; }

    #endregion Adit

    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).HasMaxLength(100);
        builder.Property(p => p.Description).HasMaxLength(500);
    }
}