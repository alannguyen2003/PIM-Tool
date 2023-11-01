namespace PIMTool.Payload.Response;

public class BaseResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = null!;
    public object Data { get; set; } = null!;

    public BaseResponse()
    {
    }

    public BaseResponse(int statusCode, string message, object data)
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
    }
}