using EAI.Logging.Writer;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace EAI.Logging
{
    public class DefaultLogWriterCollection : ILogWriterCollection
    {
        public IDictionary<string, ILogWriter> Writers { get; }

        public DefaultLogWriterCollection(ILogger logger)
        {
            var defaultLogger = new ILoggerWriter(logger);
            Writers = new Dictionary<string, ILogWriter>() { { defaultLogger.Id.Name, defaultLogger } };
        }

        public DefaultLogWriterCollection(ILogWriter logger)
        {
            Writers = new Dictionary<string, ILogWriter>() { { logger.Id.Name, logger } };
        }

        public DefaultLogWriterCollection(IEnumerable<ILogWriter> loggers)
        {
            Writers = new Dictionary<string, ILogWriter>();

            foreach (var log in loggers)
            {
                Writers.Add(log.Id.Name, log);
            }
        }
    }
}
