using System.Reflection;

namespace WaferMovie.Domain.Common.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        var name = enumValue.GetType().GetMember(enumValue.ToString())
            .First().GetCustomAttribute<DisplayAttribute>()?.Name;
        return name ?? enumValue.ToString();
    }

    public static TEnum ToEnum<TEnum>(this string value) where TEnum : Enum
        => (TEnum)Enum.Parse(typeof(TEnum), value);
}