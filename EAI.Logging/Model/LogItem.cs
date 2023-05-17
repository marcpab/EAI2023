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
        public string Stage { get; private set; }
        public int StageId { get; private set; }
        public string Transaction { get; private set; }
        public int TransactionHash { get; private set; }
        public string ChildTransaction { get; private set; }
        public string TransactionKey { get; private set; }
        public string Description { get; private set; }
        public LogException[] Exceptions { get; set; }
        public LogMessage LogMessage { get; set; }

        
        public LogItem(LogLevel level, string stage, int stageId, string service, string transaction, string childTransaction, string transactionKey, string description, Exception exception = null, LogMessage message = null)
        {
            CreatedOnUTC = DateTime.UtcNow;

            Stage = StringLimiter(stage, 4);
            StageId = stageId;
            Level = level;
            Service = service;
            Transaction = transaction;
            ChildTransaction = childTransaction;
            TransactionKey = transactionKey;
            Description = description;

            TransactionHash = transaction.GetHashCode();

            if (Exceptions != null)
            {
                Exceptions = LogException.GetExceptionArray(exception);
            }

            LogMessage = message;
        }

        private static string StringLimiter(string text, uint length)
        {
            if (string.IsNullOrWhiteSpace(text) || length == 0)
                return String.Empty;

            return (text.Substring(0, Math.Min(text.Length, (int)length)));
        }

        public override string ToString()
            => $"{_INDENT}{Stage,_MAXSIZE_STAGE} {Level,_MAXSIZE_LEVEL} {Service}: {TransactionHash:X8} {TransactionKey} {Description}{$"{LogMessage?.Operation ?? ""} {StringLimiter(LogMessage?.Content ?? "", 2000)}".Trim()}";
    }
}
