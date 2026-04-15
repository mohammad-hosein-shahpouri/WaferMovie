using System.Linq.Expressions;

namespace WaferMovie.Domain.Common.Extensions;

public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> BuildPredicate<T>(string propertyName, EnumComparisonOperator comparison, string value)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var left = propertyName.Split('.').Aggregate((Expression)parameter, Expression.Property);
        var body = MakeComparison(left, comparison, value);
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    private static Expression MakeComparison(Expression left, EnumComparisonOperator comparison, string value)
    {
        switch (comparison)
        {
            case EnumComparisonOperator.EqualTo:
                return MakeBinary(ExpressionType.Equal, left, value);

            case EnumComparisonOperator.NotEqualTo:
                return MakeBinary(ExpressionType.NotEqual, left, value);

            case EnumComparisonOperator.GreaterThan:
                return MakeBinary(ExpressionType.GreaterThan, left, value);

            case EnumComparisonOperator.GreaterThanOrEqual:
                return MakeBinary(ExpressionType.GreaterThanOrEqual, left, value);

            case EnumComparisonOperator.LessThan:
                return MakeBinary(ExpressionType.LessThan, left, value);

            case EnumComparisonOperator.LessThanOrEqual:
                return MakeBinary(ExpressionType.LessThanOrEqual, left, value);

            case EnumComparisonOperator.Contains:
                return Expression.Call(MakeString(left), "Contains", Type.EmptyTypes, Expression.Constant(value, typeof(string)));

            case EnumComparisonOperator.NotContains:
                return Expression.Call(MakeString(left), "NotContains", Type.EmptyTypes, Expression.Constant(value, typeof(string)));

            case EnumComparisonOperator.StartsWith:
                return Expression.Call(MakeString(left), "StartsWith", Type.EmptyTypes, Expression.Constant(value, typeof(string)));

            case EnumComparisonOperator.EndsWith:
                return Expression.Call(MakeString(left), "EndsWith", Type.EmptyTypes, Expression.Constant(value, typeof(string)));

            default:
                throw new NotSupportedException($"Invalid comparison operator '{comparison}'.");
        }
    }

    private static Expression MakeString(Expression source)
    {
        return source.Type == typeof(string) ? source : Expression.Call(source, "ToString", Type.EmptyTypes);
    }

    private static Expression MakeBinary(ExpressionType type, Expression left, string value)
    {
        object typedValue = value;
        if (left.Type != typeof(string))
        {
            if (string.IsNullOrEmpty(value))
            {
                typedValue = null!;
                if (Nullable.GetUnderlyingType(left.Type) == null)
                    left = Expression.Convert(left, typeof(Nullable<>).MakeGenericType(left.Type));
            }
            else
            {
                var valueType = Nullable.GetUnderlyingType(left.Type) ?? left.Type;
                typedValue = valueType.IsEnum ? Enum.Parse(valueType, value) :
                    valueType == typeof(Guid) ? Guid.Parse(value) :
                    valueType == typeof(DateTime) && type == ExpressionType.LessThanOrEqual ? Convert.ToDateTime(value).AddDays(1) :
                    Convert.ChangeType(value, valueType);
            }
        }
        var right = Expression.Constant(typedValue, left.Type);
        return Expression.MakeBinary(type, left, right);
    }
}
