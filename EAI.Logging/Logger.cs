using EAI.Logging.Model;
using EAI.Logging.Writer;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.Logging
{
    public class Logger
    {
        public string Service { get; private set; }
        public string Transaction { get; private set; }
        public string ChildTransaction { get; private set; }
        public string TransactionKey { get; private set; }
        public string Stage { get; private set; }
        public ILogWriterCollection Writers { get; private set; }

        public Logger(ILogWriterCollection writers, LogStage stage, string service, string transaction, string childTransaction, string transactionKey)
        {
            Writers = writers;
            Stage = stage.ToString();
            Service = service;
            Transaction = transaction;
            ChildTransaction = childTransaction;
            TransactionKey = transactionKey;
        }

        public Logger(ILogger log, LogStage stage, string service, string transaction, string childTransaction, string transactionKey)
        {
            Writers = new DefaultLogWriterCollection(log);
            Stage = stage.ToString();
            Service = service;
            Transaction = transaction;
            ChildTransaction = childTransaction;
            TransactionKey = transactionKey;
        }

        public Logger(ILogWriterCollection writers, string stage, string service, string transaction, string childTransaction, string transactionKey)
        {
            Writers = writers;
            Stage = stage;
            Service = service;
            Transaction = transaction;
            ChildTransaction = childTransaction;
            TransactionKey = transactionKey;
        }

        public Logger(ILogger log, string stage, string service, string transaction, string childTransaction, string transactionKey)
        {
            Writers = new DefaultLogWriterCollection(log);
            Stage = stage;
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

        private async Task<LogItem> Create<U, V>(string stage, string description, LogMessage message, Exception ex, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            where V : ILogWriterId, new()
        {
            var record = new LogItem(new U().Level, stage, Service, Transaction, ChildTransaction, TransactionKey, description, ex, message);
            
            LogQueue.Add(Writers, new V(), record);
            await LogQueue.ProcessAsync(cancellationToken: default);

            return record;
        }

        public async Task<LogItem> Variable<U, V>(string overridestage, string name, object value, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            where V : ILogWriterId, new()
        {
            if (value == null)
                return await String<U, V>(overridestage, $"{name} is NULL");
            else
                return await String<U, V>(overridestage, $"{name} = [{value.GetType().Name}]'{value}'");
        }

        public async Task<LogItem> Message<U, V>(string overridestage, string operation, string content, string description = null, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            where V : ILogWriterId, new()
        {
            return await Create<U, V>(overridestage, description, new LogMessage(operation, content), null);
        }

        public async Task<LogItem> String<U, V>(string overridestage, string text, CancellationToken cancellationToken = default, params object[] args)
            where U : ILogLevel, new()
            where V : ILogWriterId, new()
        {
            return await Create<U, V>(overridestage, FormatString(text, args), null, null, cancellationToken);
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
            return await Create<U, V>(Stage, description, new LogMessage(operation, content), null);
        }

        public async Task<LogItem> String<U, V>(string text, CancellationToken cancellationToken = default, params object[] args)
            where U : ILogLevel, new()
            where V : ILogWriterId, new()
        {
            return await Create<U, V>(Stage, FormatString(text, args), null, null, cancellationToken);
        }

        public async Task<LogItem> String<U>(string text, CancellationToken cancellationToken = default, params object[] args)
            where U : ILogLevel, new()
            => await String<U, DefaultWriterId>(text, cancellationToken, args);

        public async Task<LogItem> Variable<U>(string name, object value, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            => await Variable<U, DefaultWriterId>(name, value);

        public async Task<LogItem> Message<U>(string operation, string content, string description = null, CancellationToken cancellationToken = default)
            where U : ILogLevel, new() 
            => await Message<U, DefaultWriterId>(operation: operation, content: content, description: description);

        public async Task<LogItem> String<U>(string overridestage, string text, CancellationToken cancellationToken = default, params object[] args)
            where U : ILogLevel, new()
            => await String<U, DefaultWriterId>(overridestage, text, cancellationToken, args);

        public async Task<LogItem> Variable<U>(string overridestage, string name, object value, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            => await Variable<U, DefaultWriterId>(overridestage, name, value);

        public async Task<LogItem> Message<U>(string overridestage, string operation, string content, string description = null, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            => await Message<U, DefaultWriterId>(overridestage, operation, content, description);
    }
}
