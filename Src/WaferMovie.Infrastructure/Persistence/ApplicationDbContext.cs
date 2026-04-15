using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WaferMovie.Infrastructure.Persistence.Generators;
using Role = WaferMovie.Domain.Entities.Role;

namespace WaferMovie.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, IdentityUserLogin<Guid>, RoleClaim, IdentityUserToken<Guid>>, IApplicationDbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        if (Database.IsNpgsql())
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
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
        //foreach (var entry in ChangeTracker.Entries<IBaseEntity>().Where(w => w.State == EntityState.Added))
        //{
        //    entry.Entity.CreatedOn = DateTime.UtcNow;
        //    entry.Entity.CreatedBy = currentUserService.Id;
        //}

        //foreach (var entry in ChangeTracker.Entries<IBaseAuditableEntity>().Where(w => w.State == EntityState.Modified))
        //{
        //    entry.Entity.ModifiedOn = DateTime.UtcNow;
        //    entry.Entity.ModifiedBy = currentUserService.Id;
        //}

        //foreach (var entry in ChangeTracker.Entries<IBaseSoftDeleteEntity>().Where(w => w.State == EntityState.Deleted))
        //{
        //    entry.Entity.DeletedOn = DateTime.UtcNow;
        //    entry.Entity.DeletedBy = currentUserService.Id;
        //    entry.State = EntityState.Modified;
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
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // Find all Guid properties (or specifically Primary Keys)
            var properties = entityType.GetProperties()
                .Where(p => p.ClrType == typeof(Guid));

            foreach (var property in properties)
            {
                // Set the generator for Guid v7
                property.SetValueGeneratorFactory((_, _) => new GuidV7Generator());
                property.ValueGenerated = ValueGenerated.OnAdd;
            }
        }
    }
}