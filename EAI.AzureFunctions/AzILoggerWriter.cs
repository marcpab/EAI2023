using EAI.LoggingV2;
using EAI.LoggingV2.Levels;
using EAI.LoggingV2.Model;
using Grpc.Core.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAI.AzureFunctions
{
    public class AzILoggerWriter : ILogWriter
    {
        public ILogger Logger { get; set; }

        public Task WriteLogAsync(LogRecord record)
        {
            if(Logger != null)
                switch(record?._logData?._logLevel)
                {
                    case nameof(Info):
                        Logger.Info(record._logData._logText);
                        break;
                    case nameof(Warning):
                        Logger.Warning(record._logData._logText);
                        break;
                    case nameof(Debug):
                        Logger.Debug(record._logData._logText);
                        break;
                    case nameof(Error):
                        Logger.Error(record._logData._exception, record._logData._logText);
                        break;
                    default:
                        break;
                }

            return Task.CompletedTask;
        }
    }
}
