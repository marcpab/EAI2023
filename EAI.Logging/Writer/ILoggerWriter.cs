using EAI.Logging.Model;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using LogLevel = EAI.Logging.Model.LogLevel;

namespace EAI.Logging.Writer
{
    public class ILoggerWriterId : ILogWriterId
    {
        public string Name { get; set; } = "ILogger";
    }

    public class ILoggerWriter : ILogWriter
    {
        public ILogWriterId Id { get; } = new ILoggerWriterId();
        public ILogger Logger { get; set; }


        public ILoggerWriter(ILogger logger)
        {
            Logger = logger;
        }

        public Task WriteAsync(LogItem record)
        {
            switch (record.Level)
            {
                case LogLevel.Information:
                    Logger.LogInformation(record.ToString());
                    break;
                case LogLevel.Warning:
                    Logger.LogWarning(record.ToString());
                    break;
                case LogLevel.Error:
                    Logger.LogError(record.ToString());
                    break;
                case LogLevel.Critical:
                    Logger.LogCritical(record.ToString());
                    break;
                case LogLevel.Debug:
                    Logger.LogDebug(record.ToString());
                    break;
                case LogLevel.Trace:
                default:
                    Logger.LogTrace(record.ToString());
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
