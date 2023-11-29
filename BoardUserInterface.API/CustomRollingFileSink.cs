// CustomRollingFileSink.cs
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace BoardUserInterface.API;

public class CustomRollingFileSink : ILogEventSink
{
    private readonly ITextFormatter _textFormatter;
    private readonly string _logDirectory;

    public CustomRollingFileSink(ITextFormatter textFormatter, string logDirectory)
    {
        _textFormatter = textFormatter ?? throw new ArgumentNullException(nameof(textFormatter));
        _logDirectory = logDirectory ?? throw new ArgumentNullException(nameof(logDirectory));
    }

    // CustomRollingFileSink.cs
    public void Emit(LogEvent logEvent)
    {
        if (logEvent.Level == LogEventLevel.Error) // Check if it's an error-level event
        {
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var uniqueId = Guid.NewGuid().ToString(); // Generate a unique identifier

            // Create a unique file name for each exception
            var fileName = $"log-{timestamp}-{uniqueId}.json"; // Change extension to .json
            var filePath = Path.Combine(_logDirectory, fileName);

            // Ensure the directory exists
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }

            // Write the log event to the file in JSON format
            using (var fileStream = File.Create(filePath))
            using (var streamWriter = new StreamWriter(fileStream))
            {
                _textFormatter.Format(logEvent, streamWriter);
                streamWriter.Flush(); // Ensure all data is written to the file
            }
        }
    }

}



