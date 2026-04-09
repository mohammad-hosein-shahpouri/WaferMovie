namespace WaferMovie.Domain.Entities.Abstractions;

public abstract class BaseEntity
{
}

public abstract class BaseEntity<TId> : BaseEntity
{
    public TId Id { get; set; } = default!;
}