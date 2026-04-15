using System.Reflection;
using WaferMovie.Domain.Attributes;
using WaferMovie.Domain.Common.Pagination;

namespace WaferMovie.Domain.Common.Extensions;

public static class PaginationExtensions
{
    private static List<PaginationColumn> GetColumns<TDto>(this PaginationInput paginationInput) where TDto : class
    {
        var columns = new List<PaginationColumn>();
        var properties = typeof(TDto).GetProperties()
            .Where(w => !Attribute.IsDefined(w, typeof(PaginationIgnoreAttribute)));

        foreach (var property in properties)
        {
            var column = new PaginationColumn();
            var hasCustomInfo = Attribute.IsDefined(property, typeof(PaginationColumnAttribute));
            if (hasCustomInfo)
            {
                var attribute = property.GetCustomAttribute<PaginationColumnAttribute>()!;
                column.Hidden = attribute.Hidden;
                column.Copy = attribute.Copy ?? attribute.DataType is EnumDataType.String or EnumDataType.Number or EnumDataType.CommaSeparatedNumber or EnumDataType.PhoneNumber or EnumDataType.Email;
                column.ClassName = attribute.ClassName;
                column.PlaceHolder = attribute.PlaceHolder;
                column.Sortable = attribute.Sortable;
                column.DisplayName = attribute.DisplayName ?? property.Name;
                column.Key = (attribute.Key ?? property.Name).ToCamelCase();
                column.DataType = attribute.DataType;
                column.Prefix = attribute.Prefix;
                column.SpaceAfterPrefix = attribute.SpaceAfterPrefix;
                column.Postfix = attribute.Postfix;
                column.SpaceBeforePostfix = attribute.SpaceBeforePostfix;
            }
            else
            {
                column.Hidden = false;
                column.Copy = false;
                column.Sortable = false;
                column.DisplayName = property.Name;
                column.Key = property.Name.ToCamelCase();
                column.DataType = EnumDataType.String;
            }

            var isSearchable = Attribute.IsDefined(property, typeof(SearchableAttribute));
            if (isSearchable)
            {
                var attribute = property.GetCustomAttribute<SearchableAttribute>()!;
                var searchModel = new SearchModelResponse
                {
                    Operations = (attribute.Operators ?? attribute.GetDefaultOperators(column.DataType)).Select(s => new SearchOperation(s.GetDisplayName(), s)).ToList(),
                };

                if (column.DataType == EnumDataType.Enum)
                {
                    var isNullable = property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
                    var propertyType = isNullable ? Nullable.GetUnderlyingType(property.PropertyType)! : property.PropertyType;

                    if (propertyType.IsEnum)
                        searchModel.SearchItems = Enum.GetValues(property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(property.PropertyType)! : property.PropertyType)
                            .Cast<object>()
                            .OrderBy(Convert.ToInt32)
                            .Select(s => new SearchItem(((Enum)s).GetDisplayName(), Convert.ToInt32(s)))
                            .ToList();
                }

                if (paginationInput.SearchObjects != null)
                {
                    var requestModel = paginationInput.SearchObjects
                        .Where(w => w.Key.Equals(column.Key, StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault(f => searchModel.Operations.Any(a => a.Value == f.Operator));

                    if (requestModel != null)
                    {
                        searchModel.Value = requestModel.Value;
                        searchModel.Operator = requestModel.Operator;
                    }
                }

                column.SearchModel = searchModel;
            }

            columns.Add(column);
        }

        return columns;
    }

    public static async Task<PaginationOutput<TOutput>> ToPaginatedListAsync<TOutput>(this IQueryable<TOutput> queryable, PaginationInput searchModel, CancellationToken cancellationToken = default)
        where TOutput : class
     => await PaginationOutput<TOutput>.CreateAsync(queryable.AsNoTracking(), searchModel.PageNumber, searchModel.PageSize, searchModel.GetColumns<TOutput>(), searchModel.Order, cancellationToken);
}