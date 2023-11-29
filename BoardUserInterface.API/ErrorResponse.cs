// ErrorResponse.cs
public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public ErrorResponse(int statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }
    // ErrorResponse.cs (updated)
    public override string ToString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }

}