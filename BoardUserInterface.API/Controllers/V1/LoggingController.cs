using BoardUserInterface.Service.Logging;
using Microsoft.AspNetCore.Mvc;

namespace BoardUserInterface.API.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/logs")]
[ApiVersion("1.0")]
public class LoggingController : ControllerBase
{
    private readonly ILogService _logService;

    public LoggingController(ILogService logService)
    {
        _logService = logService;
    }

    [HttpPost]
    public IActionResult Log([FromBody] LogRequest request)
    {
        _logService.LogMessage(request.Source, request.Context, request.Message, request.Type);
        return Ok("Log entry created successfully.");
    }
}

public class LogRequest
{
    public string Source { get; set; } = string.Empty;
    public string Context { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}
