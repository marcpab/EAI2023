using System;

namespace EAI.Logging.Model
{
    public class LogItem
    {
        public DateTime CreatedOnUTC { get; private set; }
        public LogLevel Level { get; private set; }
        public string Service { get; private set; }
        public LogStage Stage { get; private set; }
        public string Transaction { get; private set; }
        public int TransactionHash { get; private set; }
        public string ChildTransaction { get; private set; }
        public string TransactionKey { get; private set; }
        public string Description { get; private set; }

        public LogItem(LogStage stage, LogLevel level, string service, string transaction, string childTransaction, string transactionKey, string description)
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
    }
}
