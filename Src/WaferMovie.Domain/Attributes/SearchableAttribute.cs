using WaferMovie.Domain.Common.Extensions;

namespace WaferMovie.Domain.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class SearchableAttribute : Attribute
{
    public SearchableAttribute(params EnumComparisonOperator[] operators)
    {
        if (!operators.IsNullOrEmpty())
            Operators = operators.ToList();
    }

    public List<EnumComparisonOperator>? Operators { set; get; }

    public List<EnumComparisonOperator> GetDefaultOperators(EnumDataType type)
    {
        return type switch
        {
            EnumDataType.String or EnumDataType.Email or EnumDataType.PhoneNumber =>
            [
                EnumComparisonOperator.EqualTo,
                EnumComparisonOperator.Contains,
                EnumComparisonOperator.StartsWith,
                EnumComparisonOperator.EndsWith
            ],
            EnumDataType.Number or EnumDataType.CommaSeparatedNumber =>
            [
                EnumComparisonOperator.EqualTo,
                EnumComparisonOperator.GreaterThan,
                EnumComparisonOperator.GreaterThanOrEqual,
                EnumComparisonOperator.LessThan,
                EnumComparisonOperator.LessThanOrEqual
            ],
            EnumDataType.Rate =>
            [
                EnumComparisonOperator.EqualTo,
                EnumComparisonOperator.GreaterThanOrEqual,
                EnumComparisonOperator.LessThanOrEqual
            ],
            EnumDataType.Date =>
            [
                EnumComparisonOperator.EqualTo,
                EnumComparisonOperator.GreaterThan,
                EnumComparisonOperator.LessThan
            ],
            _ =>
            [
                EnumComparisonOperator.EqualTo
            ]
        };
    }
}