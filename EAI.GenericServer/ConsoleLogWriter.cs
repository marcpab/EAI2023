using EAI.LoggingV2;
using EAI.LoggingV2.Levels;
using EAI.LoggingV2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.GenericServer
{
    public class ConsoleLogWriter : ILogWriter
    {
        public Task WriteLogAsync(LogRecord record)
        {
            switch(record._logData._logLevel)
            {
                case nameof(Info):
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(record.ToString());
                    break;
                case nameof(Debug):
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(record.ToString());
                    break;
                case nameof(Warning):
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(record.ToString());
                    break;
                case nameof(Error):
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(record.ToString());
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
