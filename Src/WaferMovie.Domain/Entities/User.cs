using Microsoft.AspNetCore.Identity;

namespace WaferMovie.Domain.Entities;

public class User : IdentityUser<int>, IBaseEntity, IBaseAuditableEntity, IBaseSoftDeleteEntity, IEntityTypeConfiguration<User>
{
    public string Name { get; set; } = default!;
    public Gender Gender { get; set; } = Gender.PreferNotToSay;
    public int AccountBalance { get; set; }
    public DateTime? BirthDate { get; set; }

    #region Adit

    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }

    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public DateTime? DeletedOn { get; set; }
    public int? DeletedBy { get; set; }

    #endregion Adit

    public virtual ICollection<UserRole> Roles { get; set; } = new List<UserRole>();
    public virtual ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();
    public virtual ICollection<SerieRate> SerieRates { get; set; } = new List<SerieRate>();
    public virtual ICollection<MovieRate> MovieRates { get; set; } = new List<MovieRate>();

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.Property(p => p.Name).IsRequired()
            .HasMaxLength(63);

        builder.Property(p => p.Gender)
            .HasDefaultValue(Gender.PreferNotToSay);

        builder.Property(p => p.Email)
            .HasMaxLength(127);
        builder.Property(p => p.NormalizedEmail)
            .HasMaxLength(127);

        builder.Property(p => p.UserName)
            .HasMaxLength(63);
        builder.Property(p => p.NormalizedUserName)
            .HasMaxLength(63);

        builder.Property(p => p.PasswordHash)
            .HasMaxLength(255);

        builder.Property(p => p.PhoneNumber)
            .HasMaxLength(15);

        builder.Property(p => p.ConcurrencyStamp)
            .HasMaxLength(255);

        builder.Property(p => p.SecurityStamp)
            .HasMaxLength(255);

        builder.Property(p => p.BirthDate)
            .HasColumnType("date");
    }
}