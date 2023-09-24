using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace WaferMovie.Application.Common.Interfaces;

public interface IApplicationDbContext : IDisposable
{
    #region Identity

    DbSet<User> Users { get; set; }
    DbSet<UserClaim> UserClaims { get; set; }
    DbSet<IdentityUserLogin<int>> UserLogins { get; set; }
    DbSet<IdentityUserToken<int>> UserTokens { get; set; }
    DbSet<Domain.Entities.Role> Roles { get; set; }
    DbSet<RoleClaim> RoleClaims { get; set; }
    DbSet<UserRole> UserRoles { get; set; }

    #endregion Identity

    #region Series

    DbSet<Serie> Series { get; }
    DbSet<Season> Seasons { get; }
    DbSet<Episode> Episodes { get; }
    DbSet<SerieDownloadLink> SerieDownloadLinks { get; }

    #endregion Series

    #region Movies

    DbSet<Movie> Movies { get; }
    DbSet<MovieDownloadLink> MovieDownloadLinks { get; }

    #endregion Movies

    #region Groups

    DbSet<Group> Groups { get; }
    DbSet<MovieGroup> MovieGroups { get; }
    DbSet<SerieGroup> SerieGroups { get; }

    #endregion Groups

    #region Rates

    DbSet<MovieRate> MovieRates { get; }
    DbSet<SerieRate> SerieRates { get; }

    #endregion Rates

    DatabaseFacade Database { get; }

    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}