using EAI.Logging.Model;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAI.Logging.Writer
{
    public class StringBuilderWriterId : ILogWriterId
    {
        public string Name => "StringBuilder";
    }

    public class StringBuilderWriter : ILogWriter
    {
        public StringBuilderWriter Instance { get; } = new StringBuilderWriter();

        public static StringBuilder Logs { get; private set; } = new StringBuilder();

        public ILogWriterId Id => new StringBuilderWriterId();

        public void Clear()
        {
            lock (Logs)
            {
                Logs.Clear();
            }
        }

        public Task WriteAsync(LogItem record)
        {
            lock (Logs)
            {
                Logs.AppendLine(record.ToString());
            }

            return Task.CompletedTask;
        }
    }
}
