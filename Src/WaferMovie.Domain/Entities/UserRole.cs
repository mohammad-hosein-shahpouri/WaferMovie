using Microsoft.AspNetCore.Identity;

namespace WaferMovie.Domain.Entities;

public class UserRole : IdentityUserRole<int>, IBaseEntity, IEntityTypeConfiguration<UserRole>
{
    #region Adit

    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }

    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public DateTime? DeletedOn { get; set; }
    public int? DeletedBy { get; set; }

    #endregion Adit

    public virtual Role Role { get; set; } = default!;
    public virtual User User { get; set; } = default!;

    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");
        builder.HasKey(pk => new { pk.RoleId, pk.UserId });

        builder.HasOne(o => o.Role)
            .WithMany(m => m.Users)
            .HasForeignKey(fk => fk.RoleId);

        builder.HasOne(o => o.User)
            .WithMany(m => m.Roles)
            .HasForeignKey(fk => fk.UserId);
    }
}