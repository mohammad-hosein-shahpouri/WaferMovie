namespace WaferMovie.Domain;

public interface IBaseEntity
{
    public DateTime CreatedOn { get; set; }
    public int CreatedBy { get; set; }
}

public interface IBaseAuditableEntity
{
    public DateTime? ModifiedOn { get; set; }
    public int? ModifiedBy { get; set; }
}

public interface IBaseSoftDeleteEntity
{
    public DateTime? DeletedOn { get; set; }
    public int? DeletedBy { get; set; }
}