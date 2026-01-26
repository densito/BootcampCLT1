namespace BootcampCLT.Infrastructure.Services
{
  public class LoggingService<T>
  {
    private readonly ILogger<T> _logger;

    public LoggingService(ILogger<T> logger)
    {
      _logger = logger;
    }

    public void LogInformation(string message, params object[] args)
    {
      _logger.LogInformation(message, args);
    }

    public void LogWarning(string message, params object[] args)
    {
      _logger.LogWarning(message, args);
    }

    public void LogError(Exception ex, string message, params object[] args)
    {
      _logger.LogError(ex, message, args);
    }
  }
}