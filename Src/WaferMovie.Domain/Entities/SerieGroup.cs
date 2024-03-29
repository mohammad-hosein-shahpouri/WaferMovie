﻿namespace WaferMovie.Domain.Entities;

public class SerieGroup : IBaseEntity, IEntityTypeConfiguration<SerieGroup>
{
    public int GroupId { get; set; }
    public int SerieId { get; set; }

    #region Adit

    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }

    #endregion Adit

    public virtual Group Group { get; set; } = default!;
    public virtual Serie Serie { get; set; } = default!;

    public void Configure(EntityTypeBuilder<SerieGroup> builder)
    {
        builder.HasKey(e => new { e.GroupId, e.SerieId });

        builder.HasOne(e => e.Group)
            .WithMany(e => e.Series)
            .HasForeignKey(e => e.GroupId);

        builder.HasOne(e => e.Serie)
            .WithMany(e => e.Groups)
            .HasForeignKey(e => e.SerieId);
    }
}