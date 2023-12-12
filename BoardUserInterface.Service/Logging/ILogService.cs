namespace BoardUserInterface.Service.Logging;

public interface ILogService
{
    void LogMessage(string source, string context, string message, string type);
}
