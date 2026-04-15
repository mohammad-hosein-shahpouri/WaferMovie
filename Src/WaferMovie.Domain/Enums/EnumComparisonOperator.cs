namespace WaferMovie.Domain.Enums;

public enum EnumComparisonOperator : byte
{
    /// <summary>
    /// برابر باشد با
    /// </summary>
    [Display(Name = nameof(EqualTo))]
    EqualTo = 0,

    /// <summary>
    /// مخالف باشد با
    /// </summary>
    [Display(Name = nameof(NotEqualTo))]
    NotEqualTo = 1,

    /// <summary>
    /// بزرگتر از
    /// </summary>
    [Display(Name = nameof(GreaterThan))]
    GreaterThan = 2,

    /// <summary>
    /// کوچکتر از
    /// </summary>
    [Display(Name = nameof(LessThan))]
    LessThan,

    /// <summary>
    /// بزرگتر یا مساوی با
    /// </summary>
    [Display(Name = nameof(GreaterThanOrEqual))]
    GreaterThanOrEqual,

    /// <summary>
    /// کوچکتر یا مساوی با
    /// </summary>
    [Display(Name = nameof(LessThanOrEqual))]
    LessThanOrEqual,

    /// <summary>
    /// شامل باشد
    /// </summary>
    [Display(Name = nameof(Contains))]
    Contains,

    /// <summary>
    /// شامل نباشد
    /// </summary>
    [Display(Name = nameof(NotContains))]
    NotContains,

    /// <summary>
    /// شروع شود با
    /// </summary>
    [Display(Name = nameof(StartsWith))]
    StartsWith,

    /// <summary>
    /// خاتمه یابد با
    /// </summary>
    [Display(Name = nameof(EndsWith))]
    EndsWith
}
