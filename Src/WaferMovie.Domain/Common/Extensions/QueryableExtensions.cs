using System.Linq.Expressions;
using WaferMovie.Domain.Common.Extensions;
using WaferMovie.Domain.Common.Pagination;

namespace WaferMovie.Domain.Common.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> AddSearchObjects<T>(this IQueryable<T> query, List<PaginationColumn> columns)
    {
        columns = columns.Where(w => w.SearchModel != null).ToList();

        if (columns.Count == 0) return query.Take(0);
        if (columns.Any(x => x.SearchModel!.Operator != null))
            foreach (var item in columns)
                if (item.SearchModel!.Operator != null)
                    query = query.Where(item.Key, item.SearchModel!.Operator.Value, item.SearchModel!.Value);

        return query;
    }

    public static IQueryable<T> Where<T>(this IQueryable<T> source, string propertyName, EnumComparisonOperator comparison, string? value)
    {
        return source.Where(ExpressionExtensions.BuildPredicate<T>(propertyName, comparison, value));
    }

    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, SortModel sortModel)
    {
        var expression = source.Expression;
        if (sortModel != null)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var selector = sortModel.ColumnName.Split('.').Aggregate((Expression)parameter, Expression.PropertyOrField);
            var method = sortModel.Ascending ? ("OrderBy") : ("OrderByDescending");
            expression = Expression.Call(typeof(Queryable), method,
                [source.ElementType, selector.Type],
                expression, Expression.Quote(Expression.Lambda(selector, parameter)));
            return source.Provider.CreateQuery<T>(expression);
        }
        else
            return source;
    }
}