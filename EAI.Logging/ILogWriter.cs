using EAI.Logging.Model;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.Logging
{
    public interface ILogWriter
    {
        WriteAsyncResult Result { get; }
        string ResultMessage { get; }

        ILogWriterId Id { get; }
        Task WriteAsync(LogItem record, CancellationToken cancellationToken = default);
    }

    public enum WriteAsyncResult
    {
        Success,
        Failed,
        Cancelled,
        RetryRecommended
    }
}
