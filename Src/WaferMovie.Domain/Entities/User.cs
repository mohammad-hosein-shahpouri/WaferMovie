using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WaferMovie.Domain.Entities;

public class User : IdentityUser<int>, IEntityTypeConfiguration<User>
{
    public string? Name { get; set; }

    // TODO: Gender Enum
    //public GenderType? Gender { get; set; }

    public int AccountCharge { get; set; }
    public DateTime? BirthDate { get; set; }

    public virtual ICollection<UserRole> Roles { get; set; } = new List<UserRole>();
    public virtual ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
    }
}