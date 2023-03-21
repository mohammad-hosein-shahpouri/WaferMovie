using Microsoft.AspNetCore.Identity;

namespace WaferMovie.Domain.Entities;

public class RoleClaim : IdentityRoleClaim<Guid>, IEntityTypeConfiguration<RoleClaim>
{
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public virtual Role Role { get; set; } = default!;

    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.ToTable("RoleClaims");

        builder.HasOne(roleClaim => roleClaim.Role)
            .WithMany(claim => claim.Claims)
            .HasForeignKey(c => c.RoleId);
    }
}