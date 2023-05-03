using EAI.Logging.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAI.Logging
{
    public interface ILogWriterCollection
    {
        IDictionary<ILogWriterId, ILogWriter> Writers { get; }
    }
}
