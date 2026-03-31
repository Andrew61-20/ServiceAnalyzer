namespace ServiceAnalyzer.Logging;
using Microsoft.Extensions.Logging;

public class FileLogger : ILogger
{
    private readonly string _path;
    private static readonly object _lock = new();

    public FileLogger(string path)
    {
        _path = path;
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception exception,
        Func<TState, Exception, string> formatter)
    {
        var message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {formatter(state, exception)}";

        var directory = Path.GetDirectoryName(_path);

        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory!);
        }

        lock (_lock)
        {
            File.AppendAllText(_path, message + Environment.NewLine);
        }
    }
}
