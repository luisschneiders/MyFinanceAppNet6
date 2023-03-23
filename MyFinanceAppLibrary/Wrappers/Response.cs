namespace MyFinanceAppLibrary.Wrappers;

public class Response<T>
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
    public string ErrorCode { get; set; }
    public T Data { get; set; }

#nullable disable
    public Response()
    {
    }
#nullable enable

    public Response(T data)
    {
        Success = true;
        ErrorMessage = string.Empty;
        ErrorCode = string.Empty;
        Data = data;
    }
}
