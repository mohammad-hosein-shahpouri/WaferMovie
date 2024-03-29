﻿namespace WaferMovie.Domain.Entities;

public class Serie : IBaseEntity, IBaseAuditableEntity, IBaseSoftDeleteEntity, IEntityTypeConfiguration<Serie>
{
    public int Id { get; set; }
    public string IMDB { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Length { get; set; }
    public bool IsFree { get; set; }
    public bool Unavailable { get; set; }
    public string? StreamNetwork { get; set; }

    //public Days ShowDay { get; set; }
    public int FirstSeasonYear { get; set; }

    public int? LastSeasonYear { get; set; }
    public SerieAgeRestriction AgeRestriction { get; set; }
    public DateTime? LastEpisodeDate { get; set; } = null;

    #region Adit

    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }

    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public DateTime? DeletedOn { get; set; }
    public int? DeletedBy { get; set; }

    #endregion Adit

    public virtual IEnumerable<Season> Seasons { get; set; } = new List<Season>();
    public virtual IEnumerable<SerieGroup> Groups { get; set; } = new List<SerieGroup>();
    public virtual IEnumerable<SerieRate> Rates { get; set; } = new List<SerieRate>();

    public void Configure(EntityTypeBuilder<Serie> builder)
    {
        builder.HasKey(pk => pk.Id);
        builder.Property(p => p.IMDB).HasMaxLength(20);
        builder.HasIndex(i => i.IMDB).IsUnique();

        builder.Property(p => p.Title).HasMaxLength(100);
        builder.Property(p => p.Description).HasMaxLength(500);
    }
}