using EAI.Logging.Model;
using Microsoft.Extensions.Logging;
using System.Threading;
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
        public WriteAsyncResult Result { get; private set; }
        public string ResultMessage { get; private set; } = string.Empty;


        public ILoggerWriter(ILogger logger)
        {
            Logger = logger;
        }

        public Task WriteAsync(LogItem record, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                Result = WriteAsyncResult.Cancelled;
                return Task.CompletedTask;
            }

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

            Result = WriteAsyncResult.Success;
            return Task.CompletedTask;
        }
    }
}
