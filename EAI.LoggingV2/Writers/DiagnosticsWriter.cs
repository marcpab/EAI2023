using EAI.LoggingV2.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace EAI.LoggingV2.Writers
{
    public class DiagnosticsWriter : ILogWriter
    {
        public Task WriteLogAsync(LogRecord record)
        {
            Trace.WriteLine(record.ToString());

            return Task.CompletedTask;
        }
    }
}
