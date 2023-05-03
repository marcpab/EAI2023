using EAI.Logging.Model;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAI.Logging.Writer
{
    public class LogItemListWriterId : ILogWriterId
    {
        public string Name => "LogItemList";
    }

    public class LogItemListWriter : ILogWriter
    {
        public static LogItemListWriter Instance { get; } = new LogItemListWriter();

        private LogItemListWriter() { }

        public static ConcurrentBag<LogItem> LogItems { get; private set; } = new ConcurrentBag<LogItem>();

        public ILogWriterId Id => new LogItemListWriterId();

        public void Clear()
        {
            LogItems = new ConcurrentBag<LogItem>();
        }

        public Task WriteAsync(LogItem record)
        {
            LogItems.Add(record);

            return Task.CompletedTask;
        }
    }
}
