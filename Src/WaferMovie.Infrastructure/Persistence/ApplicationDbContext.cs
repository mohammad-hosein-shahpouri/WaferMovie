using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WaferMovie.Application.Common.Interfaces;
using WaferMovie.Domain;
using WaferMovie.Domain.Entities;

namespace WaferMovie.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, IdentityUserLogin<int>, RoleClaim, IdentityUserToken<int>>, IApplicationDbContext
{
    private readonly ICurrentUserService currentUserService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        this.currentUserService = currentUserService;
    }

    #region Series

    public virtual DbSet<Serie> Series => Set<Serie>();
    public virtual DbSet<Season> Seasons => Set<Season>();
    public virtual DbSet<Episode> Episodes => Set<Episode>();
    public virtual DbSet<SerieDownloadLink> SerieDownloadLinks => Set<SerieDownloadLink>();

    #endregion Series

    #region Movies

    public virtual DbSet<Movie> Movies => Set<Movie>();
    public virtual DbSet<MovieDownloadLink> MovieDownloadLinks => Set<MovieDownloadLink>();

    #endregion Movies

    #region Groups

    public virtual DbSet<Group> Groups => Set<Group>();
    public virtual DbSet<MovieGroup> MovieGroups => Set<MovieGroup>();
    public virtual DbSet<SerieGroup> SerieGroups => Set<SerieGroup>();

    #endregion Groups

    #region Rates

    public virtual DbSet<MovieRate> MovieRates => Set<MovieRate>();
    public virtual DbSet<SerieRate> SerieRates => Set<SerieRate>();

    #endregion Rates

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        foreach (var entry in ChangeTracker.Entries<IBaseEntity>().Where(w => w.State == EntityState.Added))
        {
            entry.Entity.CreatedOn = DateTime.UtcNow;
            entry.Entity.CreatedBy = currentUserService.Id;
        }

        foreach (var entry in ChangeTracker.Entries<IBaseAuditableEntity>().Where(w => w.State == EntityState.Modified))
        {
            entry.Entity.ModifiedOn = DateTime.UtcNow;
            entry.Entity.ModifiedBy = currentUserService.Id;
        }

        foreach (var entry in ChangeTracker.Entries<IBaseSoftDeleteEntity>().Where(w => w.State == EntityState.Deleted))
        {
            entry.Entity.DeletedOn = DateTime.UtcNow;
            entry.Entity.DeletedBy = currentUserService.Id;
            entry.State = EntityState.Modified;
        }

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

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            if (typeof(IBaseSoftDeleteEntity).IsAssignableFrom(entityType.ClrType))
                entityType.AddSoftDeleteQueryFilter();
    }
}