using Microsoft.AspNetCore.Identity;

namespace WaferMovie.Domain.Entities;

public class Role : IdentityRole<Guid>, IEntityTypeConfiguration<Role>
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

    public virtual IEnumerable<UserRole> Users { get; set; } = [];
    public virtual IEnumerable<RoleClaim> Claims { get; set; } = [];

    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
    }
}