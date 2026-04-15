namespace WaferMovie.Domain.Common.Extensions;

public static class StringExtensions
{
    public static string Repeat(this string text, int count = 1)
        => string.Join("", Enumerable.Repeat(text, count));

    public static string ToCamelCase(this string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        string[] words = input.Trim().Split(' ', '_', '-');
        words[0] = char.ToLowerInvariant(words[0][0]) + words[0].Substring(1);

        for (int i = 1; i < words.Length; i++)
            words[i] = char.ToUpperInvariant(words[i][0]) + words[i].Substring(1);

        return string.Join("", words);
    }
}