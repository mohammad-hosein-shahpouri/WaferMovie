namespace WaferMovie.Application.Common.Models;

public class CrudResult
{
    public CrudResult(CrudStatus status)
    {
        Status = status;
        Messages.Add(status.ToString());
    }

    public CrudResult(CrudStatus status, string message)
    {
        Status = status;
        Messages.Add(message);
    }

    public CrudResult(CrudStatus status, List<string> messages)
    {
        Status = status;
        Messages = messages;
    }

    public bool Succeeded => Status == CrudStatus.Succeeded;
    public CrudStatus Status { get; set; }
    public List<string> Messages { get; set; } = new();

    public static CrudResult<TEntity> Create<TEntity>(CrudStatus status, List<string> messages)
      => new CrudResult<TEntity>(status, default!, messages);
}

public class CrudResult<TEntity> : CrudResult
{
    public CrudResult(CrudStatus status) : base(status)
    {
    }

    public CrudResult(CrudStatus status, string message) : base(status, message)
    {
    }

    public CrudResult(CrudStatus status, List<string> messages) : base(status, messages)
    {
    }

    public CrudResult(CrudStatus status, TEntity data) : base(status)
        => Data = data;

    public CrudResult(CrudStatus status, TEntity data, string message) : base(status, message)
        => Data = data;

    public CrudResult(CrudStatus status, TEntity data, List<string> messages) : base(status, messages)
        => Data = data;

    public TEntity Data { get; set; } = default!;
}