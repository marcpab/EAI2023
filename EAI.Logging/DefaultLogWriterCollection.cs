using EAI.Logging.Writer;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace EAI.Logging
{
    public class DefaultLogWriterCollection : ILogWriterCollection
    {
        public IDictionary<ILogWriterId, ILogWriter> Writers { get; }

        public DefaultLogWriterCollection(ILogger logger)
        {
            var defaultLogger = new ILoggerWriter(logger);
            Writers = new Dictionary<ILogWriterId, ILogWriter>() { { defaultLogger.Id, defaultLogger } };
        }

        public DefaultLogWriterCollection(ILogWriter logger)
        {
            Writers = new Dictionary<ILogWriterId, ILogWriter>() { { logger.Id, logger } };
        }

        public DefaultLogWriterCollection(IEnumerable<ILogWriter> loggers)
        {
            Writers = new Dictionary<ILogWriterId, ILogWriter>();

            foreach (var log in loggers)
            {
                Writers.Add(log.Id, log);
            }
        }
    }
}
