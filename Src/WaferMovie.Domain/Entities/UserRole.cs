using Microsoft.AspNetCore.Identity;

namespace WaferMovie.Domain.Entities;

public class UserRole : IdentityUserRole<int>, IEntityTypeConfiguration<UserRole>
{
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
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