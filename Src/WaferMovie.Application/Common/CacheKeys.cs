namespace WaferMovie.Application.Common;

public static class CacheKeys
{
    private const string Prefix = "WaferMovie";

    /// <summary>
    /// Key: WaferMovie:Users:<UserId>
    /// </summary>
    private const string UsersKey = $"{Prefix}:Users:{{0}}";

    /// <summary>
    /// Key: WaferMovie:Movies:<MovieId>
    /// </summary>
    private const string MoviesKey = $"{Prefix}:Movies:{{0}}";

    /// <summary>
    /// Key: WaferMovie:Series:<SerieId>
    /// </summary>
    private const string SeriesKey = $"{Prefix}:Series:{{0}}";

    /// <summary>
    /// Key: WaferMovie:Users:<UserId>
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public static string GetUsersKey(int userId) => string.Format(UsersKey, userId);

    /// <summary>
    /// Key: WaferMovie:Movies:<MovieId>
    /// </summary>
    /// <param name="movieId"></param>
    /// <returns></returns>
    public static string GetMoviesKey(int movieId) => string.Format(MoviesKey, movieId);

    /// <summary>
    /// Key: WaferMovie:Series:<SerieId>
    /// </summary>
    /// <param name="serieId"></param>
    /// <returns></returns>
    public static string GetSeriesKey(int serieId) => string.Format(SeriesKey, serieId);
}