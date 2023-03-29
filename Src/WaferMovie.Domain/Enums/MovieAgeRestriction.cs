namespace WaferMovie.Domain.Enums;

public enum MovieAgeRestriction : byte
{
    [Display(Name = "G")]
    G = 0,

    [Display(Name = "PG")]
    PG = 1,

    [Display(Name = "PG-13")]
    PG13 = 2,

    [Display(Name = "R")]
    R = 3,

    [Display(Name = "NC-17")]
    NC17 = 3,
}