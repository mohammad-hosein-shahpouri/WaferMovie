namespace WaferMovie.Domain.Enums;

public enum SerieAgeRestriction : byte
{
    [Display(Name = "TV-Y")] TVY = 0,
    [Display(Name = "TV-Y7")] TVY7 = 1,
    [Display(Name = "TV-G")] TVG = 2,
    [Display(Name = "TV-PG")] TVPG = 3,
    [Display(Name = "TV-14")] TV14 = 4,
    [Display(Name = "TV-MA")] TVMA = 5,
}