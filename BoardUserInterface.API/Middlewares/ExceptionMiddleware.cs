using BoardUserInterface.Service.Logging;
using DocumentFormat.OpenXml.Office2016.Excel;

public class ExceptionMiddleware
{

    private readonly RequestDelegate _next;
    private readonly ILogService _logService;

    public ExceptionMiddleware(RequestDelegate next,ILogService logService )
    {
        _next = next;
        _logService = logService;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        //context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var errorResponse = new ErrorResponse(context.Response.StatusCode, "Internal Server Error from the custom middleware: " + exception.Message);

        // You can log the exception here if needed (e.g., to a file or database)
        _logService.LogMessage("Backend", "Not Successful", exception.Message, "Error");
        // _logger.LogError(exception, exception.Message);

        return context.Response.WriteAsync(errorResponse.ToString());
    }
}
