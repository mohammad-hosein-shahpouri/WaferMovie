using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WaferMovie.Application.Common.Interfaces;
using WaferMovie.Domain.Entities;

namespace WaferMovie.Infrastructure;

public class ApplicationDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, IdentityUserLogin<Guid>, RoleClaim, IdentityUserToken<Guid>>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    #region Series

    public virtual DbSet<Serie> Series { get; set; } = default!;
    public virtual DbSet<Season> Seasons { get; set; } = default!;
    public virtual DbSet<Episode> Episodes { get; set; } = default!;
    public virtual DbSet<SerieDownloadLink> SerieDownloadLinks { get; set; } = default!;

    #endregion Series

    #region Movies

    public virtual DbSet<Movie> Movies { get; set; } = default!;
    public virtual DbSet<MovieDownloadLink> MovieDownloadLinks { get; set; } = default!;

    #endregion Movies

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        //foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        //{
        //    switch (entry.State)
        //    {
        //        case EntityState.Added:
        //            entry.Entity.CreatedDate = DateTime.Now;
        //            break;
        //        case EntityState.Modified:
        //            entry.Entity.ModifiedDate = DateTime.Now;
        //            break;
        //    }
        //}

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.ApplyConfiguration(new User());
        modelBuilder.ApplyConfiguration(new Role());
        modelBuilder.ApplyConfiguration(new UserRole());
        modelBuilder.ApplyConfiguration(new RoleClaim());
        modelBuilder.ApplyConfiguration(new UserClaim());
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
    }
}