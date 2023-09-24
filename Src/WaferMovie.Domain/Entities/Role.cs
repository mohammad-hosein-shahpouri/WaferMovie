using Microsoft.AspNetCore.Identity;

namespace WaferMovie.Domain.Entities;

public class Role : IdentityRole<int>, IBaseEntity, IBaseAuditableEntity, IBaseSoftDeleteEntity, IEntityTypeConfiguration<Role>
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

    #region Adit

    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }

    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public DateTime? DeletedOn { get; set; }
    public int? DeletedBy { get; set; }

    #endregion Adit

    public virtual IEnumerable<UserRole> Users { get; set; } = new List<UserRole>();
    public virtual IEnumerable<RoleClaim> Claims { get; set; } = new List<RoleClaim>();

    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
    }
}