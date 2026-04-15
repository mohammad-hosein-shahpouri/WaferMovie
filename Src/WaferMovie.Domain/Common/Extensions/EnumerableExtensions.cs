namespace WaferMovie.Domain.Common.Extensions;

public static class EnumerableExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? enumerable) => enumerable is null || !enumerable.Any();
}