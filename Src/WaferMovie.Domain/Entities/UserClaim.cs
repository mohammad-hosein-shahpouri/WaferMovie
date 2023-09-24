using Microsoft.AspNetCore.Identity;

namespace WaferMovie.Domain.Entities;

public class UserClaim : IdentityUserClaim<int>, IBaseEntity, IBaseAuditableEntity, IEntityTypeConfiguration<UserClaim>
{
    #region Adit

    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }

    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }

    #endregion Adit

    public virtual User User { get; set; } = default!;

    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.ToTable("UserClaims");

        builder.HasOne(o => o.User)
            .WithMany(m => m.Claims)
            .HasForeignKey(fk => fk.UserId);
    }
}