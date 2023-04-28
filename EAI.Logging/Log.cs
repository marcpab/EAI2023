using EAI.Logging.Model;
using System;

namespace EAI.Logging
{
    public class Log
    {
        private readonly static string _INDENT = new string(' ', 5);
        private const int _MAXSIZE_STAGE = -4;
        private const int _MAXSIZE_LEVEL = -11;

        public LogItem Record { get; set; }
        public LogMessage Message { get; set; }
        public LogException[] Exceptions { get; set; }
        private Exception _Exception { get; set; }
        


        public override string ToString() 
            => $"{_INDENT}{Record.Stage,_MAXSIZE_STAGE} {Record.Level,_MAXSIZE_LEVEL} {Record.Service}: {Record.TransactionHash:X8} {Record.TransactionKey} {Record.Description}{$"{Message?.Operation ?? ""} {Message?.Content ?? ""}".Trim()}";
    }
}
