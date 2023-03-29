namespace WaferMovie.Domain.Enums;

public enum Gender : byte
{
    [Display(Name = "Male")]
    Male = 0,

    [Display(Name = "Female")]
    Female = 1,

    [Display(Name = "Prefer not to say")]
    PreferNotToSay = 2
}