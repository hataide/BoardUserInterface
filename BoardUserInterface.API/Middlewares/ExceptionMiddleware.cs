using BoardUserInterface.Service.DataAccess;
using BoardUserInterface.Service.Logging;
using BoardUserInterfaces.DataAccess.Models;
using DocumentFormat.OpenXml.Office2016.Excel;
using Microsoft.Extensions.DependencyInjection;

public class ExceptionMiddleware
{


    private readonly RequestDelegate _next;
    private readonly ILogService _logService;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ExceptionMiddleware(RequestDelegate next, ILogService logService, IServiceScopeFactory serviceScopeFactory)
    {
        _next = next;
        _logService = logService;
        _serviceScopeFactory = serviceScopeFactory;
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
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var errorResponse = new ErrorResponse(context.Response.StatusCode, "Internal Server Error from the custom middleware: " + exception.Message);

        // Log the exception here
        _logService.LogMessage("Backend", "Not Successful", exception.Message, "Error");

        // Create a scope to resolve scoped services
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var logsRepoService = scope.ServiceProvider.GetRequiredService<ILogsRepoService>();

            var newLog = new Logs
            {
                Source = "Backend",
                Context = "Exception",
                Message = exception.Message,
                Type = "Error"
            };

            logsRepoService.CreateLogAsync(newLog);
        }

        return context.Response.WriteAsync(errorResponse.ToString());
    }
}
