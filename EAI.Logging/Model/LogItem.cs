using System;

namespace EAI.Logging.Model
{
    public class LogItem
    {
        private readonly static string _INDENT = new string(' ', 5);
        private const int _MAXSIZE_STAGE = -4;
        private const int _MAXSIZE_LEVEL = -11;

        public DateTime CreatedOnUTC { get; private set; }
        public LogLevel Level { get; private set; }
        public string Service { get; private set; }
        public LogStage Stage { get; private set; }
        public string Transaction { get; private set; }
        public int TransactionHash { get; private set; }
        public string ChildTransaction { get; private set; }
        public string TransactionKey { get; private set; }
        public string Description { get; private set; }
        public LogException[] Exceptions { get; set; }
        public LogMessage LogMessage { get; set; }

        public LogItem(LogLevel level, LogStage stage, string service, string transaction, string childTransaction, string transactionKey, string description)
        {
            CreatedOnUTC = DateTime.UtcNow;

            Stage = stage;
            Level = level;
            Service = service;
            Transaction = transaction;
            ChildTransaction = childTransaction;
            TransactionKey = transactionKey;
            Description = description;

            TransactionHash = transaction.GetHashCode();
        }

        public LogItem(LogLevel level, LogStage stage, string service, string transaction, string childTransaction, string transactionKey, string description, Exception exception, LogMessage message)
        {
            CreatedOnUTC = DateTime.UtcNow;

            Stage = stage;
            Level = level;
            Service = service;
            Transaction = transaction;
            ChildTransaction = childTransaction;
            TransactionKey = transactionKey;
            Description = description;

            TransactionHash = transaction.GetHashCode();

            Exceptions = LogException.GetExceptionArray(exception);
            LogMessage = message;
        }

        private string StringLimiter(string text, uint length)
        {
            if (string.IsNullOrWhiteSpace(text) || length == 0)
                return String.Empty;

            return (text.Substring(Math.Min(text.Length, (int)length)));
        }

        public override string ToString()
            => $"{_INDENT}{Stage,_MAXSIZE_STAGE} {Level,_MAXSIZE_LEVEL} {Service}: {TransactionHash:X8} {TransactionKey} {Description}{$"{LogMessage?.Operation ?? ""} {StringLimiter(LogMessage?.Content ?? "", 2000)}".Trim()}";
    }
}
