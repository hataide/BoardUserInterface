using BoardUserInterface.Common.Exceptions;
using BoardUserInterface.FileService.Service;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BoardUserInterface.Service.Logging;

public class LogService : ILogService
{
    private readonly ILogger<IFileService> _logger;

    public LogService(ILogger<IFileService> logger)
    {
        _logger = logger;
    }

    public void LogMessage(string source, string context, string message, string type)
    {
        try
        {
            if(type == "Information")
            {
                _logger.LogInformation($"Source: {source}, Context: {context}, Message: {message}");
            }
            else if(type == "Error")
            {
                _logger.LogError($"Source: {source}, Context: {context}, Message: {message}");
            }
        }catch
        {
            throw new LoggingException("Failed to Log in Logging System");
        }
    }
}
