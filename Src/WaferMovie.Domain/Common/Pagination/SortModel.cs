namespace WaferMovie.Domain.Common.Pagination;

public class SortModel
{
    public string ColumnName { get; set; } = "Id";
    public bool Ascending { get; set; } = false;
}