using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WaferMovie.Domain.Entities;

public class Role : IdentityRole<int>, IEntityTypeConfiguration<Role>
{
    public Role()
    {
    }

    public Role(string name) : base(name)
    {
    }

    public Role(string name, string description) : base(name)
    {
        Description = description;
    }

    public string Description { get; set; } = default!;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public virtual IEnumerable<UserRole> Users { get; set; } = new List<UserRole>();
    public virtual IEnumerable<RoleClaim> Claims { get; set; } = new List<RoleClaim>();

    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
    }
}