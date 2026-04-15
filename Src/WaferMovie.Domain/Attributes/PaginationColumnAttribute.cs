namespace WaferMovie.Domain.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class PaginationColumnAttribute : Attribute
{
    public bool Hidden { get; set; }
    public bool? Copy { get; set; }

    // TODO: Fix Sortable
    public bool Sortable { get; set; }
    public string Prefix { get; set; } = string.Empty;
    public bool SpaceAfterPrefix { get; set; }
    public string Postfix { get; set; } = string.Empty;
    public bool SpaceBeforePostfix { get; set; }

    public string? Key { get; set; }
    public string? DisplayName { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public string PlaceHolder { get; set; } = string.Empty;
    public EnumDataType DataType { get; set; } = EnumDataType.String;
}