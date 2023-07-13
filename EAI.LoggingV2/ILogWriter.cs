using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EAI.LoggingV2.Model;

namespace EAI.LoggingV2
{
    public interface ILogWriter
    {
        Task WriteLogAsync(LogRecord record);
    }
}
