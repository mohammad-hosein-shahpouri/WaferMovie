namespace WaferMovie.Application.Common.Extensions;

public static class StringExtensions
{
    public static string Repeat(this string text, int count = 1)
        => string.Join("", Enumerable.Repeat(text, count));
}