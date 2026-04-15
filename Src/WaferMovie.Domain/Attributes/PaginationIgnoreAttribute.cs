namespace WaferMovie.Domain.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class PaginationIgnoreAttribute : Attribute
{
}