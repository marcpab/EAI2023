using EAI.Logging.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.Logging
{
    public class LogProvider<T> 
        where T : ILogStage, new()
    {
        public string Service { get; private set; }
        public string Transaction { get; private set; }
        public string ChildTransaction { get; private set; }
        public string TransactionKey { get; private set; }
        public T Stage { get; private set; }
        public ILogWriterCollection Writers { get; private set; }

        public LogProvider(ILogWriterCollection writers, string service, string transaction, string childTransaction, string transactionKey)
        {
            Writers = writers;
            Stage = new T();
            Service = service;
            Transaction = transaction;
            ChildTransaction = childTransaction;
            TransactionKey = transactionKey;
        }

        public LogProvider(ILogger log, string service, string transaction, string childTransaction, string transactionKey)
        {
            Writers = new DefaultLogWriterCollection(log);
            Stage = new T();
            Service = service;
            Transaction = transaction;
            ChildTransaction = childTransaction;
            TransactionKey = transactionKey;
        }

        private static Func<string, object[], string> FormatString = (string text, object[] args) =>
        {
            if (args?.Length == 0)
                return text;

            return string.Format(text, args);
        };

        private async Task<LogItem> Create<U,V>(string description, LogMessage message, Exception ex, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            where V : ILogWriterId, new()
        {
            var record = new LogItem(new U().Level, Stage.Stage, Service, Transaction, ChildTransaction, TransactionKey, description, ex, message);
            
            LogQueue.Add(Writers, new V(), record);
            await LogQueue.ProcessAsync(cancellationToken: default);

            return record;
        }

        public async Task<LogItem> Variable<U, V>(string name, object value, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            where V : ILogWriterId, new()
        {
            if (value == null)
                return await String<U, V>($"{name} is NULL");
            else
                return await String<U, V>($"{name} = [{value.GetType().Name}]'{value}'");
        }

        public async Task<LogItem> Message<U,V>(string operation, string content, string description = null, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            where V : ILogWriterId, new()
        {
            return await Create<U, V>(description, new LogMessage(operation, content), null);
        }

        public async Task<LogItem> String<U,V>(string text, CancellationToken cancellationToken = default, params object[] args)
            where U : ILogLevel, new()
            where V : ILogWriterId, new()
        {
            return await Create<U, DefaultWriterId>(FormatString(text, args), null, null, cancellationToken);
        }

        public async Task<LogItem> String<U>(string text, CancellationToken cancellationToken = default, params object[] args)
            where U : ILogLevel, new()
            => await String<U, DefaultWriterId>(text, cancellationToken, args);

        public async Task<LogItem> Variable<U>(string name, object value)
            where U : ILogLevel, new()
            => await Variable<U, DefaultWriterId>(name, value);

        public async Task<LogItem> Message<U>(string operation, string content, string description = null)
            where U : ILogLevel, new() 
            => await Message<U, DefaultWriterId>(operation, content, description);
    }
}
