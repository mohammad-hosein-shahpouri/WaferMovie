using WaferMovie.Domain.Common.Extensions;

namespace WaferMovie.Domain.Common.Pagination;

public class PaginationOutput<TOutput>
{
    public List<TOutput> Items { get; } = [];
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }
    public List<PaginationColumn> Columns { get; } = new();
    public SortModel Order { get; } = new();

    public PaginationOutput(List<TOutput> items, int count, int pageNumber, int pageSize, List<PaginationColumn> columns, SortModel order)
    {
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        Items = items;
        PageSize = pageSize;
        Columns = columns;
        Order = order;
    }

    public static async Task<PaginationOutput<TOutput>> CreateAsync(IQueryable<TOutput> source, int pageNumber, int pageSize, List<PaginationColumn> columns, SortModel order, CancellationToken cancellationToken = default)
    {
        var count = await source.AddSearchObjects(columns).CountAsync(cancellationToken);
        var items = await source.AddSearchObjects(columns).OrderBy(order).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new PaginationOutput<TOutput>(items, count, pageNumber, pageSize, columns, order);
    }
}

public record PaginationColumn
{
    public string DisplayName { get; set; } = default!;
    public string Key { get; set; } = default!;
    public string ClassName { get; set; } = string.Empty;
    public string PlaceHolder { get; set; } = string.Empty;
    public string Prefix { get; set; } = string.Empty;
    public bool SpaceAfterPrefix { get; set; }
    public string Postfix { get; set; } = string.Empty;
    public bool SpaceBeforePostfix { get; set; }
    public bool Copy { get; set; } = true;
    public bool Hidden { get; set; }
    public bool Sortable { get; set; }
    public EnumDataType DataType { get; set; } = EnumDataType.String;

    public SearchModelResponse? SearchModel { get; set; }
}

public record SearchModelResponse
{
    public EnumComparisonOperator? Operator { get; set; } = null;
    public List<SearchOperation> Operations { get; set; } = new();

    public string? Value { get; set; } = null;

    /// <summary>
    /// Use for dropdown
    /// </summary>
    public List<SearchItem>? SearchItems { get; set; }
}

public record SearchOperation
{
    public SearchOperation()
    {
    }

    public SearchOperation(string name, EnumComparisonOperator value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; set; } = default!;
    public EnumComparisonOperator Value { get; set; }
}

public record SearchItem
{
    public SearchItem()
    {
    }

    public SearchItem(string name, int value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; set; } = default!;
    public int Value { get; set; }
}