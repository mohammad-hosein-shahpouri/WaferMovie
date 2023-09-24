namespace WaferMovie.Domain.Entities;

public class Movie : IBaseEntity, IBaseAuditableEntity, IBaseSoftDeleteEntity, IEntityTypeConfiguration<Movie>
{
    public int Id { get; set; }

    public string IMDB { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool Unavailable { get; set; }
    public int Length { get; set; }
    public bool IsFree { get; set; }
    public int OutYear { get; set; } = DateTime.Now.Year;
    public MovieAgeRestriction AgeRestriction { get; set; }
    public virtual ICollection<MovieDownloadLink> DownloadLinks { get; set; } = new List<MovieDownloadLink>();
    public virtual ICollection<MovieGroup> Groups { get; set; } = new List<MovieGroup>();
    public virtual ICollection<MovieRate> Rates { get; set; } = new List<MovieRate>();

    #region Adit

    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public DateTime? DeletedOn { get; set; }
    public int? DeletedBy { get; set; }

    #endregion Adit

    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(pk => pk.Id);

        builder.Property(p => p.IMDB).HasMaxLength(20);
        builder.HasIndex(i => i.IMDB).IsUnique();

        builder.Property(p => p.Title).HasMaxLength(100);
        builder.Property(p => p.Description).HasMaxLength(500);
    }
}