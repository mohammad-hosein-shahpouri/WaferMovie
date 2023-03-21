using Microsoft.AspNetCore.Identity;

namespace WaferMovie.Domain.Entities;

public class UserClaim : IdentityUserClaim<Guid>, IEntityTypeConfiguration<UserClaim>
{
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public virtual User User { get; set; } = default!;

    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.ToTable("UserClaims");

        builder.HasOne(o => o.User)
            .WithMany(m => m.Claims)
            .HasForeignKey(fk => fk.UserId);
    }
}