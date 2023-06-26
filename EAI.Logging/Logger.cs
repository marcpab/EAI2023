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

        private static readonly Func<string, object[], string> FormatString = (string text, object[] args) =>
        {
            if (args?.Length == 0)
                return text;

            return string.Format(text, args);
        };

        private static readonly Func<string, int, (string, int)> VerifyCustomStage = (string stage, int stageId) =>
        {
            if (Enum.TryParse<LogStage>(stage, true, out LogStage logStageValue))
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
            await LogQueue.ProcessAsync(cancellationToken: cancellationToken);

            return record;
        }

        public async Task<LogItem> Variable<U, V>(string overrideStage, int overrideStageId, string name, object value, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            where V : ILogWriterId, new()
        {
            if (value is null)
                return await String<U, V>(overrideStage, overrideStageId, $"{name} is NULL", cancellationToken);
            else
                return await String<U, V>(overrideStage, overrideStageId, $"{name} = [{value.GetType().Name}]'{value}'", cancellationToken);
        }

        public async Task<LogItem> Message<U, V>(string overrideStage, int overrideStageId, string operation, string content, string description = null, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            where V : ILogWriterId, new()
        {
            return await Create<U, V>(overrideStage, overrideStageId, description, new LogMessage(operation, content), null, cancellationToken);
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
            if (value is null)
                return await String<U, V>($"{name} is NULL", cancellationToken);
            else
                return await String<U, V>($"{name} = [{value.GetType().Name}]'{value}'", cancellationToken);
        }

        public async Task<LogItem> Message<U,V>(string operation, string content, string description = null, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            where V : ILogWriterId, new()
        {
            return await Create<U, V>(Stage, StageId, description, new LogMessage(operation, content), null, cancellationToken);
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
            => await Variable<U, DefaultWriterId>(name, value.GetHashCode(), cancellationToken);

        public async Task<LogItem> Message<U>(string operation, string content, string description = null, CancellationToken cancellationToken = default)
            where U : ILogLevel, new() 
            => await Message<U, DefaultWriterId>(operation: operation, content: content, description: description, cancellationToken: cancellationToken);

        public async Task<LogItem> String<U>(string overrideStage, int overrideStageId, string text, CancellationToken cancellationToken = default, params object[] args)
            where U : ILogLevel, new()
            => await String<U, DefaultWriterId>(overrideStage, overrideStageId, text, cancellationToken, args);

        public async Task<LogItem> Variable<U>(string overrideStage, int overrideStageId, string name, object value, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            => await Variable<U, DefaultWriterId>(overrideStage, overrideStageId, name, value, cancellationToken);

        public async Task<LogItem> Message<U>(string overrideStage, int overrideStageId, string operation, string content, string description = null, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            => await Message<U, DefaultWriterId>(overrideStage, overrideStageId, operation, content, description, cancellationToken);

        public async Task<LogItem> String<U>(LogStage stage, string text, CancellationToken cancellationToken = default, params object[] args)
            where U : ILogLevel, new()
            => await String<U, DefaultWriterId>(stage.ToString(), (int)stage, text, cancellationToken, args);

        public async Task<LogItem> Variable<U>(LogStage stage, string name, object value, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            => await Variable<U, DefaultWriterId>(stage.ToString(), (int)stage, name, value, cancellationToken);

        public async Task<LogItem> Message<U>(LogStage stage, string operation, string content, string description = null, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            => await Message<U, DefaultWriterId>(stage.ToString(), (int)stage, operation, content, description, cancellationToken);

        public async Task<LogItem> Exception<U>(LogStage stage, Exception ex, string description, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            => await Create<U, DefaultWriterId>(stage.ToString(), (int)stage, description, null, ex, cancellationToken);
        public async Task<LogItem> Exception<U>(string overrideStage, int overrideStageId, Exception ex, string description, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            => await Create<U, DefaultWriterId>(overrideStage, overrideStageId, description, null, ex, cancellationToken);
        public async Task<LogItem> Exception<U>(Exception ex, string description, CancellationToken cancellationToken = default)
            where U : ILogLevel, new()
            => await Create<U, DefaultWriterId>(Stage, StageId, description, null, ex, cancellationToken);

        public async Task<LogItem> DebugString(LogStage stage, string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelDebug, DefaultWriterId>(stage.ToString(), (int)stage, text, cancellationToken, args);
        public async Task<LogItem> DebugString(string overrideStage, int overrideStageId, string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelDebug, DefaultWriterId>(overrideStage, overrideStageId, text, cancellationToken, args);
        public async Task<LogItem> DebugString(string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelDebug, DefaultWriterId>(text, cancellationToken, args);
        public async Task<LogItem> WarnString(LogStage stage, string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelWarning, DefaultWriterId>(stage.ToString(), (int)stage, text, cancellationToken, args);
        public async Task<LogItem> WarnString(string overrideStage, int overrideStageId, string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelWarning, DefaultWriterId>(overrideStage, overrideStageId, text, cancellationToken, args);
        public async Task<LogItem> WarnString(string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelWarning, DefaultWriterId>(text, cancellationToken, args);
        public async Task<LogItem> ErrorString(LogStage stage, string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelError, DefaultWriterId>(stage.ToString(), (int)stage, text, cancellationToken, args);
        public async Task<LogItem> ErrorString(string overrideStage, int overrideStageId, string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelError, DefaultWriterId>(overrideStage, overrideStageId, text, cancellationToken, args);
        public async Task<LogItem> ErrorString(string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelError, DefaultWriterId>(text, cancellationToken, args);
        public async Task<LogItem> CriticalString(LogStage stage, string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelCritical, DefaultWriterId>(stage.ToString(), (int)stage, text, cancellationToken, args);
        public async Task<LogItem> CriticalString(string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelCritical, DefaultWriterId>(text, cancellationToken, args);
        public async Task<LogItem> CriticalString(string overrideStage, int overrideStageId, string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelCritical, DefaultWriterId>(overrideStage, overrideStageId, text, cancellationToken, args);
        public async Task<LogItem> TraceString(LogStage stage, string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelTrace, DefaultWriterId>(stage.ToString(), (int)stage, text, cancellationToken, args);
        public async Task<LogItem> TraceString(string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelTrace, DefaultWriterId>(text, cancellationToken, args);
        public async Task<LogItem> TraceString(string overrideStage, int overrideStageId, string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelTrace, DefaultWriterId>(overrideStage, overrideStageId, text, cancellationToken, args);
        public async Task<LogItem> InformationString(LogStage stage, string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelInformation, DefaultWriterId>(stage.ToString(), (int)stage, text, cancellationToken, args);
        public async Task<LogItem> InformationString(string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelInformation, DefaultWriterId>(text, cancellationToken, args);
        public async Task<LogItem> InformationString(string overrideStage, int overrideStageId, string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelInformation, DefaultWriterId>(overrideStage, overrideStageId, text, cancellationToken, args);
        public async Task<LogItem> ErrorException(LogStage stage, string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelError, DefaultWriterId>(stage.ToString(), (int)stage, text, cancellationToken, args);
        public async Task<LogItem> ErrorException(string overrideStage, int overrideStageId, string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelError, DefaultWriterId>(overrideStage, overrideStageId, text, cancellationToken, args);
        public async Task<LogItem> ErrorException(string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelError, DefaultWriterId>(text, cancellationToken, args);
        public async Task<LogItem> CriticalException(LogStage stage, string text, CancellationToken cancellationToken = default, params object[] args)
           => await String<LevelCritical, DefaultWriterId>(stage.ToString(), (int)stage, text, cancellationToken, args);
        public async Task<LogItem> CriticalException(string overrideStage, int overrideStageId, string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelCritical, DefaultWriterId>(overrideStage, overrideStageId, text, cancellationToken, args);
        public async Task<LogItem> CriticalException(string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelCritical, DefaultWriterId>(text, cancellationToken, args);
        public async Task<LogItem> WarningException(LogStage stage, string text, CancellationToken cancellationToken = default, params object[] args)
           => await String<LevelDebug, DefaultWriterId>(stage.ToString(), (int)stage, text, cancellationToken, args);
        public async Task<LogItem> WarningException(string overrideStage, int overrideStageId, string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelWarning, DefaultWriterId>(overrideStage, overrideStageId, text, cancellationToken, args);
        public async Task<LogItem> WarningException(string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelWarning, DefaultWriterId>(text, cancellationToken, args);
        public async Task<LogItem> DebugException(LogStage stage, string text, CancellationToken cancellationToken = default, params object[] args)
           => await String<LevelDebug, DefaultWriterId>(stage.ToString(), (int)stage, text, cancellationToken, args);
        public async Task<LogItem> DebugException(string overrideStage, int overrideStageId, string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelDebug, DefaultWriterId>(overrideStage, overrideStageId, text, cancellationToken, args);
        public async Task<LogItem> DebugException(string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelDebug, DefaultWriterId>(text, cancellationToken, args);
        public async Task<LogItem> InformationException(LogStage stage, string text, CancellationToken cancellationToken = default, params object[] args)
           => await String<LevelInformation, DefaultWriterId>(stage.ToString(), (int)stage, text, cancellationToken, args);
        public async Task<LogItem> InformationException(string overrideStage, int overrideStageId, string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelInformation, DefaultWriterId>(overrideStage, overrideStageId, text, cancellationToken, args);
        public async Task<LogItem> InformationException(string text, CancellationToken cancellationToken = default, params object[] args)
            => await String<LevelInformation, DefaultWriterId>(text, cancellationToken, args);
        public async Task<LogItem> DebugMessage(LogStage stage, string operation, string content, string description = null, CancellationToken cancellationToken = default)
            => await Message<LevelDebug, DefaultWriterId>(stage.ToString(), (int)stage, operation, content, description, cancellationToken);
        public async Task<LogItem> DebugMessage(string overrideStage, int overrideStageId, string operation, string content, string description = null, CancellationToken cancellationToken = default)
            => await Message<LevelDebug, DefaultWriterId>(overrideStage, overrideStageId, operation, content, description, cancellationToken);
        public async Task<LogItem> DebugMessage(string operation, string content, string description = null, CancellationToken cancellationToken = default)
            => await Message<LevelDebug, DefaultWriterId>(operation: operation, content: content, description: description, cancellationToken: cancellationToken);
        public async Task<LogItem> InformationMessage(LogStage stage, string operation, string content, string description = null, CancellationToken cancellationToken = default)
            => await Message<LevelInformation, DefaultWriterId>(stage.ToString(), (int)stage, operation, content, description, cancellationToken);
        public async Task<LogItem> InformationMessage(string overrideStage, int overrideStageId, string operation, string content, string description = null, CancellationToken cancellationToken = default)
            => await Message<LevelInformation, DefaultWriterId>(overrideStage, overrideStageId, operation, content, description, cancellationToken);
        public async Task<LogItem> InformationMessage(string operation, string content, string description = null, CancellationToken cancellationToken = default)
            => await Message<LevelInformation, DefaultWriterId>(operation: operation, content: content, description: description, cancellationToken: cancellationToken);
        public async Task<LogItem> WarningMessage(LogStage stage, string operation, string content, string description = null, CancellationToken cancellationToken = default)
            => await Message<LevelWarning, DefaultWriterId>(stage.ToString(), (int)stage, operation, content, description, cancellationToken);
        public async Task<LogItem> WarningMessage(string overrideStage, int overrideStageId, string operation, string content, string description = null, CancellationToken cancellationToken = default)
            => await Message<LevelWarning, DefaultWriterId>(overrideStage, overrideStageId, operation, content, description, cancellationToken);
        public async Task<LogItem> WarningMessage(string operation, string content, string description = null, CancellationToken cancellationToken = default)
            => await Message<LevelWarning, DefaultWriterId>(operation: operation, content: content, description: description, cancellationToken: cancellationToken);
        public async Task<LogItem> ErrorMessage(LogStage stage, string operation, string content, string description = null, CancellationToken cancellationToken = default)
            => await Message<LevelError, DefaultWriterId>(stage.ToString(), (int)stage, operation, content, description, cancellationToken);
        public async Task<LogItem> ErrorMessage(string overrideStage, int overrideStageId, string operation, string content, string description = null, CancellationToken cancellationToken = default)
            => await Message<LevelError, DefaultWriterId>(overrideStage, overrideStageId, operation, content, description, cancellationToken);
        public async Task<LogItem> ErrorMessage(string operation, string content, string description = null, CancellationToken cancellationToken = default)
            => await Message<LevelError, DefaultWriterId>(operation: operation, content: content, description: description, cancellationToken: cancellationToken);
        public async Task<LogItem> CriticalMessage(LogStage stage, string operation, string content, string description = null, CancellationToken cancellationToken = default)
            => await Message<LevelCritical, DefaultWriterId>(stage.ToString(), (int)stage, operation, content, description, cancellationToken);
        public async Task<LogItem> CriticalMessage(string overrideStage, int overrideStageId, string operation, string content, string description = null, CancellationToken cancellationToken = default)
            => await Message<LevelCritical, DefaultWriterId>(overrideStage, overrideStageId, operation, content, description, cancellationToken);
        public async Task<LogItem> CriticalMessage(string operation, string content, string description = null, CancellationToken cancellationToken = default)
            => await Message<LevelCritical, DefaultWriterId>(operation: operation, content: content, description: description, cancellationToken: cancellationToken);

    }
}
