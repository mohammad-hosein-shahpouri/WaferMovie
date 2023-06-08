using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WaferMovie.Application.Common.Interfaces;
using WaferMovie.Domain.Entities;

namespace WaferMovie.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, IdentityUserLogin<int>, RoleClaim, IdentityUserToken<int>>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
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

    #region Groups

    public virtual DbSet<Group> Groups { get; set; } = default!;
    public virtual DbSet<MovieGroup> MovieGroup { get; set; } = default!;
    public virtual DbSet<SerieGroup> SerieGroups { get; set; } = default!;

    #endregion Groups

    #region Rates

    public virtual DbSet<MovieRate> MovieRate { get; set; } = default!;
    public virtual DbSet<SerieRate> SerieRate { get; set; } = default!;

    #endregion Rates

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
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
        var domainAssembly = AppDomain.CurrentDomain.Load("WaferMovie.Domain");
        modelBuilder.ApplyConfigurationsFromAssembly(domainAssembly);
        modelBuilder.ApplyConfiguration(new User());
        modelBuilder.ApplyConfiguration(new Role());
        modelBuilder.ApplyConfiguration(new UserRole());
        modelBuilder.ApplyConfiguration(new RoleClaim());
        modelBuilder.ApplyConfiguration(new UserClaim());
        modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
    }
}