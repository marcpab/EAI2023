using EAI.Logging.Model;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.Logging.Writer
{
    public class StringBuilderWriterId : ILogWriterId
    {
        public string Name => "StringBuilder";
    }

    public class StringBuilderWriter : ILogWriter
    {
        public static StringBuilderWriter Instance { get; } = new StringBuilderWriter();
        public WriteAsyncResult Result { get; private set; }
        public string ResultMessage { get; private set; } = string.Empty;
        public static StringBuilder Logs { get; private set; } = new StringBuilder();

        private StringBuilderWriter() { }

        public ILogWriterId Id => new StringBuilderWriterId();

        public void Clear()
        {
            lock (Logs)
            {
                Logs.Clear();
            }
        }

        public Task WriteAsync(LogItem record, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                Result = WriteAsyncResult.Cancelled;                
                return Task.CompletedTask;
            }

            lock (Logs)
            {
                Logs.AppendLine(record.ToString());
            }

            Result = WriteAsyncResult.Success;
            return Task.CompletedTask;
        }
    }
}
