using Microsoft.AspNetCore.Identity;

namespace WaferMovie.Application.Common.Interfaces;

public interface IApplicationDbContext : IDisposable
{
    #region Identity

    DbSet<User> Users { get; set; }
    DbSet<UserClaim> UserClaims { get; set; }
    DbSet<IdentityUserLogin<int>> UserLogins { get; set; }
    DbSet<IdentityUserToken<int>> UserTokens { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<RoleClaim> RoleClaims { get; set; }
    DbSet<UserRole> UserRoles { get; set; }

    #endregion Identity

    #region Series

    DbSet<Serie> Series { get; set; }
    DbSet<Season> Seasons { get; set; }
    DbSet<Episode> Episodes { get; set; }
    DbSet<SerieDownloadLink> SerieDownloadLinks { get; set; }

    #endregion Series

    #region Movies

    DbSet<Movie> Movies { get; set; }
    DbSet<MovieDownloadLink> MovieDownloadLinks { get; set; }

    #endregion Movies

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}