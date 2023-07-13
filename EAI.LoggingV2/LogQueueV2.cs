using EAI.LoggingV2.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.LoggingV2
{
    class LogQueueV2
    {
        class LogQueueItem
        {
            public ILogWriter[] _writers;
            public LogRecord _logRecord;
        }


        private static LogQueueV2 _instance = CreateLogQueue();
        internal static LogQueueV2 Instance { get => _instance; }

        private static LogQueueV2 CreateLogQueue()
        {
            var instance = new LogQueueV2();
            var cts = new CancellationTokenSource();

            Task.Run(() => instance.RunAsync(cts.Token));

            return instance;
        }


        private BufferedQueue<LogQueueItem> _queue = new BufferedQueue<LogQueueItem>();

        public void Enqueue(ILogWriter[] writer, LogRecord logRecord)
        {
            _queue.EnqueueMessage(new LogQueueItem()
                { 
                    _writers = writer,
                    _logRecord = logRecord
            });    
        }

        private Task RunAsync(CancellationToken cancellationToken)
        {
            return _queue.RunAsync(ProcessMessageAsync, cancellationToken);
        }

        private async Task ProcessMessageAsync(LogQueueItem logQueueItem)
        {
            foreach(var writer in logQueueItem._writers)
                await writer.WriteLogAsync(logQueueItem._logRecord);
        }

        internal Task FlushAsync()
        {
            return _queue.FlushAsync();
        }
    }
}
