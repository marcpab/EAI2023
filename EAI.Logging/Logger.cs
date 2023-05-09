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
        public int StageId { get; private set; }
        public ILogWriterCollection Writers { get; private set; }

        private static Func<string, object[], string> FormatString = (string text, object[] args) =>
        {
            if (args?.Length == 0)
                return text;

            return string.Format(text, args);
        };

        private static Func<string, int, (string, int)> VerifyCustomStage = (string stage, int stageId) =>
        {
            LogStage logStageValue;
            if (Enum.TryParse<LogStage>(stage, true, out logStageValue))
            {
                return (logStageValue.ToString(), (int)logStageValue);
            }

            var logStageString = Enum.GetName(typeof(LogStage), stageId);
            if (logStageString != null)
            {
                logStageValue = (LogStage)Enum.Parse(typeof(LogStage), logStageString);
                return (logStageValue.ToString(), (int)logStageValue);
            }

            return (stage, stageId);
        };

        public Logger(ILogWriterCollection writers, LogStage stage, string service, string transaction, string childTransaction, string transactionKey)
        {
            Writers = writers;
            Stage = stage.ToString();
            StageId = (int)stage;
            Service = service;
            Transaction = transaction;
            ChildTransaction = childTransaction;
            TransactionKey = transactionKey;
        }

        public Logger(ILogger log, LogStage stage, string service, string transaction, string childTransaction, string transactionKey)
        {
            Writers = new DefaultLogWriterCollection(log);
            Stage = stage.ToString();
            StageId = (int)stage;
            Service = service;
            Transaction = transaction;
            ChildTransaction = childTransaction;
            TransactionKey = transactionKey;
        }

        public Logger(ILogWriterCollection writers, string stage, int stageId, string service, string transaction, string childTransaction, string transactionKey)
        {
            var (stageName, stageNumber) = VerifyCustomStage(stage, stageId);

            Writers = writers;
            Stage = stageName;
            StageId = stageNumber;
            Service = service;
            Transaction = transaction;
            ChildTransaction = childTransaction;
            TransactionKey = transactionKey;
        }

        public Logger(ILogger log, string stage, int stageId, string service, string transaction, string childTransaction, string transactionKey)
        {
            var (stageName, stageNumber) = VerifyCustomStage(stage, stageId);

            Writers = new DefaultLogWriterCollection(log);
            Stage = stageName;
            StageId = stageNumber;
            Service = service;
            Transaction = transaction;
            ChildTransaction = childTransaction;
            TransactionKey = transactionKey;
        }

        private async Task<LogItem> Create<U, V>(string stage, int stageId, string description, LogMessage message, Exception ex, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            where V : ILogWriterId, new()
        {
            (stage, stageId) = VerifyCustomStage(stage, stageId);

            var record = new LogItem(new U().Level, stage, stageId, Service, Transaction, ChildTransaction, TransactionKey, description, ex, message);
            
            LogQueue.Add(Writers, new V(), record);
            await LogQueue.ProcessAsync(cancellationToken: default);

            return record;
        }

        public async Task<LogItem> Variable<U, V>(string overrideStage, int overrideStageId, string name, object value, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            where V : ILogWriterId, new()
        {
            if (value == null)
                return await String<U, V>(overrideStage, overrideStageId, $"{name} is NULL");
            else
                return await String<U, V>(overrideStage, overrideStageId, $"{name} = [{value.GetType().Name}]'{value}'");
        }

        public async Task<LogItem> Message<U, V>(string overrideStage, int overrideStageId, string operation, string content, string description = null, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            where V : ILogWriterId, new()
        {
            return await Create<U, V>(overrideStage, overrideStageId, description, new LogMessage(operation, content), null);
        }

        public async Task<LogItem> String<U, V>(string overridestage, int overrideStageId, string text, CancellationToken cancellationToken = default, params object[] args)
            where U : ILogLevel, new()
            where V : ILogWriterId, new()
        {
            return await Create<U, V>(overridestage, overrideStageId, FormatString(text, args), null, null, cancellationToken);
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
            return await Create<U, V>(Stage, StageId, description, new LogMessage(operation, content), null);
        }

        public async Task<LogItem> String<U, V>(string text, CancellationToken cancellationToken = default, params object[] args)
            where U : ILogLevel, new()
            where V : ILogWriterId, new()
        {
            return await Create<U, V>(Stage, StageId, FormatString(text, args), null, null, cancellationToken);
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

        public async Task<LogItem> String<U>(string overrideStage, int overrideStageId, string text, CancellationToken cancellationToken = default, params object[] args)
            where U : ILogLevel, new()
            => await String<U, DefaultWriterId>(overrideStage, overrideStageId, text, cancellationToken, args);

        public async Task<LogItem> Variable<U>(string overrideStage, int overrideStageId, string name, object value, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            => await Variable<U, DefaultWriterId>(overrideStage, overrideStageId, name, value);

        public async Task<LogItem> Message<U>(string overrideStage, int overrideStageId, string operation, string content, string description = null, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            => await Message<U, DefaultWriterId>(overrideStage, overrideStageId, operation, content, description);

        public async Task<LogItem> String<U>(LogStage stage, string text, CancellationToken cancellationToken = default, params object[] args)
            where U : ILogLevel, new()
            => await String<U, DefaultWriterId>(stage.ToString(), (int)stage, text, cancellationToken, args);

        public async Task<LogItem> Variable<U>(LogStage stage, string name, object value, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            => await Variable<U, DefaultWriterId>(stage.ToString(), (int)stage, name, value);

        public async Task<LogItem> Message<U>(LogStage stage, string operation, string content, string description = null, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            => await Message<U, DefaultWriterId>(stage.ToString(), (int)stage, operation, content, description);
    }
}
