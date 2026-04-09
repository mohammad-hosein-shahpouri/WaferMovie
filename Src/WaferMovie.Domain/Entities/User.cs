using Microsoft.AspNetCore.Identity;

namespace WaferMovie.Domain.Entities;

public class User : IdentityUser<Guid>, IEntityTypeConfiguration<User>
{
    public string Name { get; set; } = default!;
    public EnumGender Gender { get; set; } = EnumGender.PreferNotToSay;
    public int AccountBalance { get; set; }
    public DateTime? BirthDate { get; set; }


    public virtual ICollection<UserRole> Roles { get; set; } = [];
    public virtual ICollection<UserClaim> Claims { get; set; } = [];
    public virtual ICollection<SerieRate> SerieRates { get; set; } = [];
    public virtual ICollection<MovieRate> MovieRates { get; set; } = [];

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.Property(p => p.Name).IsRequired()
            .HasMaxLength(63);

        builder.Property(p => p.Gender)
            .HasDefaultValue(EnumGender.PreferNotToSay);

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