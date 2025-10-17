using Microsoft.Extensions.Logging;
using MinibusTracker.Application.Common.Interfaces;

namespace MinibusTracker.Infrastructure.Common.Logging;
public class LoggerManager : ILoggerManager
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger _logger;

    public LoggerManager(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
        _logger = _loggerFactory.CreateLogger("MinibusTracker");
    }

    public void LogInfo(string message)
    {
        _logger.LogInformation(message);
    }

    public void LogWarn(string message)
    {
        _logger.LogWarning(message);
    }

    public void LogDebug(string message)
    {
        _logger.LogDebug(message);
    }

    public void LogError(string message, Exception? ex = null)
    {
        _logger.LogError(ex, message);
    }
}
