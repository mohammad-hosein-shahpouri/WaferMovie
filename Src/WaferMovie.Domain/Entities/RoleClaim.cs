using Microsoft.AspNetCore.Identity;

namespace WaferMovie.Domain.Entities;

public class RoleClaim : IdentityRoleClaim<int>, IBaseEntity, IBaseAuditableEntity, IEntityTypeConfiguration<RoleClaim>
{
    #region Adit

    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }

    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }

    #endregion Adit

    public virtual Role Role { get; set; } = default!;

    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.ToTable("RoleClaims");

        builder.HasOne(roleClaim => roleClaim.Role)
            .WithMany(claim => claim.Claims)
            .HasForeignKey(c => c.RoleId);
    }
}