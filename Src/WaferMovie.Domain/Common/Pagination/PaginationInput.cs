namespace WaferMovie.Domain.Common.Pagination;

public record PaginationInput
{
    public List<SearchModelRequest> SearchObjects { get; set; } = [];
    public SortModel Order { get; set; } = new();
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public record SearchModelRequest
{
    public string Key { get; set; } = default!;
    public EnumComparisonOperator Operator { get; set; }
    public string Value { get; set; } = default!;
}