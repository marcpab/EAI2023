using EAI.Logging.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.Logging.Writer
{
    internal class LogQueue
    {
        internal class LogQueueItem
        {
            public ILogWriterCollection Writers { get; set; }
            public ILogWriterId Id { get; set; }
            public LogItem Record { get; set; }

            public async Task<string> Worker()
            {
                StringBuilder result = new StringBuilder();

                foreach (var w in Writers.Writers.Values)
                {
                    if (Id.Name != new DefaultWriterId().Name
                    && Id.Name != w.Id.Name)
                    {
                        continue;
                    }

                    try
                    {
                        await w.WriteAsync(Record);
                    }
                    catch (Exception ex)
                    {
                        // do not concat strings here use appends!
                        result.Append(w.Id.Name ?? "<Unknown>").Append(": ").AppendLine(ex.Message);
                    }
                }

                return result.ToString();
            }
        }

        // .net 6 singleton
        private static LogQueue _Instance { get; } = new LogQueue();

        private static readonly Queue<LogQueueItem> _Queue = new Queue<LogQueueItem>();

        private async Task<LogQueue> InitializeAsync(CancellationToken cancellationToken)
        {
            var localQueue = new Queue<LogQueueItem>();

            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return _Instance;
                }

                lock (_Queue)
                {
                    while (_Queue.Count > 0)
                    {
                        // we dont want to loose any log entries, so do not return yet
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        localQueue.Enqueue(_Queue.Dequeue());
                    }
                }

                while (localQueue.Count > 0)
                {
                    // we dont want to loose any log entries, so reenqueue them
                    if (cancellationToken.IsCancellationRequested)
                    {
                        while (localQueue.Count > 0)
                        {
                            _Queue.Enqueue(localQueue.Dequeue());
                        }

                        return _Instance;
                    }

                    await localQueue.Dequeue().Worker();
                }

                break;
            }

            return _Instance;
        }

        public static Task<LogQueue> ProcessAsync(CancellationToken cancellationToken)
            => _Instance.InitializeAsync(cancellationToken);

        private LogQueue() { }

        public static void Add(ILogWriter logger, LogItem logEntry)
        {
            var writerCollection = new DefaultLogWriterCollection(logger);

            Add(writerCollection, new DefaultWriterId(), logEntry);
        }

        public static void Add(ILogWriterCollection loggers, ILogWriterId id, LogItem logEntry)
        {
            lock (_Queue)
            {
                _Queue.Enqueue(new LogQueueItem { Writers = loggers, Record = logEntry, Id = id });
            }
        }

    }
}
