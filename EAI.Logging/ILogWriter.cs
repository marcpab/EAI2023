using EAI.Logging.Model;
using System.Threading.Tasks;

namespace EAI.Logging
{
    public interface ILogWriter
    {
        ILogWriterId Id { get; }
        Task WriteAsync(LogItem record);
    }
}
