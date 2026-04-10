namespace WaferMovie.Domain.Common;

public class ApiResponse
{
    public ApiResponse()
    {

    }

    public ApiResponse(EnumApiResponseStatus status)
    {
        Status = status;
        Messages.Add(status.ToString());
    }

    public ApiResponse(EnumApiResponseStatus status, string message)
    {
        Status = status;
        Messages.Add(message);
    }

    public ApiResponse(EnumApiResponseStatus status, List<string> messages)
    {
        Status = status;
        Messages = messages;
    }

    public bool Succeeded => Status == EnumApiResponseStatus.Success;
    public EnumApiResponseStatus Status { get; set; }
    public List<string> Messages { get; set; } = [];

    public static ApiResponse<TEntity> Create<TEntity>(EnumApiResponseStatus status, List<string> messages)
      => new(status, default!, messages);
}

public class ApiResponse<TEntity> : ApiResponse
{
    public ApiResponse()
    {

    }

    public ApiResponse(EnumApiResponseStatus status) : base(status)
    {
    }

    public ApiResponse(EnumApiResponseStatus status, string message) : base(status, message)
    {
    }

    public ApiResponse(EnumApiResponseStatus status, List<string> messages) : base(status, messages)
    {
    }

    public ApiResponse(EnumApiResponseStatus status, TEntity data) : base(status)
        => Data = data;

    public ApiResponse(EnumApiResponseStatus status, TEntity data, string message) : base(status, message)
        => Data = data;

    public ApiResponse(EnumApiResponseStatus status, TEntity data, List<string> messages) : base(status, messages)
        => Data = data;

    public TEntity Data { get; set; } = default!;
}