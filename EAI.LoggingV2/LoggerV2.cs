
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using EAI.General;
using EAI.General.Extensions;
using EAI.General.SettingJson;
using EAI.LoggingV2.Model;
using Json = Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EAI.LoggingV2
{
    [Singleton]
    public class LoggerV2
    {
        //private string _stage;
        //private AsyncLocal<string> _service = new AsyncLocal<string>();
        //private AsyncLocal<string> _transactionId = new AsyncLocal<string>();
        //private AsyncLocal<string> _childTransactionId = new AsyncLocal<string>();
        private AsyncLocal<string> _transactionKey = new AsyncLocal<string>();
        private ILogWriter[] _writers;

        //public string Service { get => _service.Value; set => _service.Value = value; }
        //public string TransactionId { get => _transactionId.Value; set => _transactionId.Value = value; }
        //public string ChildTransactionId { get => _childTransactionId.Value; set => _childTransactionId.Value = value; }
        public string TransactionKey { get => _transactionKey.Value; set => _transactionKey.Value = value; }
        //public string Stage { get => _stage; set => _stage = value; }

        public ILogWriter[] Writers { get => _writers; set => _writers = value; }

        public void Variable<U>(string name, object value)
            where U : ILogLevel, new()
        {
            if (value is null)
                String<U>($"{name} is NULL");
            else
                String<U>($"{name} = [{value.GetType().Name}]'{value}'");
        }

        public void Start<U>(string messageName, object messageContent, string text = null, params object[] args)
            where U : ILogLevel, new()
        {
            if (text == null)
                text = "Start";

            Create<U>(LogActionEnum.start, FormatString(text, args), messageName, messageContent, null);
        }

        public void Update<U>(string text = null, params object[] args)
            where U : ILogLevel, new()
        {
            Create<U>(LogActionEnum.update, FormatString(text, args), null, null, null);
        }

        public void Message<U>(string messageName, object messageContent, string text, params object[] args)
            where U : ILogLevel, new()
        {
            Create<U>(LogActionEnum.logRecord, FormatString(text, args), messageName, messageContent, null);
        }

        public void String<U>(string text, params object[] args)
            where U : ILogLevel, new()
        {
            Create<U>(LogActionEnum.logRecord, FormatString(text, args), null, null, null);
        }

        public void Exception<U>(Exception ex, string text, params object[] args)
            where U : ILogLevel, new()
        {
            Create<U>(LogActionEnum.logRecord, ex.GetExceptionInformation(), null, null, ex);

            if(text != null)
                Create<U>(LogActionEnum.logRecord, FormatString(text, args), null, null, null);
        }

        public void Success<U>(string text = null, params object[] args)
            where U : ILogLevel, new()
        {
            if (text == null)
                text = "Leave (success)";

            Create<U>(LogActionEnum.leave, FormatString(text, args), null, null, null);
        }

        public void Failed<U>(Exception ex, string text = null, params object[] args)
            where U : ILogLevel, new()
        {
            if (text == null)
                text = "Leave (failed)";

            Create<U>(LogActionEnum.logRecord, ex.GetExceptionInformation(), null, null, ex);
            Create<U>(LogActionEnum.leave, FormatString(text, args), null, null, ex);
        }


        private static string FormatString(string text, object[] args)
        {
            if (args?.Length == 0)
                return text;

            return string.Format(text, args);
        }

        private void Create<U>(LogActionEnum action, string logText, string messageName, object messageContent, Exception ex)
            where U : ILogLevel, new()
        {
            var context = ProcessContext.GetCurrent();
            var transactionKey = TransactionKey;

            var processData = new ProcessData()
            {
                _parentProcessId = context?.ParentProcessId,
                _parentStage = context?.ParentStage,
                _serviceName = context?.ServiceName,
                _initialMessageKey = transactionKey,
            };

            if (action == LogActionEnum.leave)
            {
                processData._finishOnUTC = DateTime.UtcNow;
                processData._status = ex == null ? StatusEnum.success : StatusEnum.failed;
            }


            var logData = logText != null || messageContent != null || ex != null ? 
                new LogData()
                {
                    _logLevel = typeof(U).Name,
                    _logText = logText ?? ex.GetExceptionInformation(),
                    _messageKey = transactionKey,

                    _message = GetMessageData(messageName, messageContent),

                    _exceptions = GetExceptionData(ex),
                    _exception = ex
                } 
                : null;

            var logRecord = new LogRecord()
            {
                _logAction = action,
                _createdOnUTC = DateTime.UtcNow,
                _processId = context?.ProcessId,
                _stage = context?.Stage,

                _processData = processData,

                _logData = logData
            };

            LogQueueV2.Instance.Enqueue(_writers, logRecord);
        }

        private MessageData GetMessageData(string messageName, object messageContent)
        {
            if (messageContent == null && messageName == null)
                return null;

            if (messageContent == null)
                return new MessageData(
                    messageName,
                                null,
                                EnumContentType.Unknown.ToString(),
                                null);

            switch (messageContent)
            {
                case string stringContent:
                    return new MessageData(
                                    messageName,
                                    null,
                                    EnumContentType.RAW.ToString(),
                                    stringContent);
                case XElement xElement:
                    return GetMessageDataFromXElement(messageName, xElement);
                case XDocument xDocument:
                    return GetMessageDataFromXElement(messageName, xDocument.Root);
                case JObject jObject:
                    return GetMessageDataFromJToken(messageName, jObject);
                case JArray jArray:
                    return GetMessageDataFromJToken(messageName, jArray);
                default:
                    return new MessageData(
                                    messageName,
                                    messageContent.GetType().FullName,
                                    EnumContentType.Json.ToString(),
                                    Json.JsonConvert.SerializeObject(messageContent, Json.Formatting.Indented));
            }
        }

        private static MessageData GetMessageDataFromXElement(string messageName, XElement xElement)
        {
            return new MessageData(
                    messageName,
                    $"{xElement.Name.NamespaceName}#{xElement.Name.NamespaceName}",
                    EnumContentType.XML.ToString(),
                    xElement.ToString()
                );
        }

        private static MessageData GetMessageDataFromJToken(string messageName, JToken jToken)
        {
            return new MessageData(
                            messageName,
                            null,
                            EnumContentType.Json.ToString(),
                            jToken.ToString());
        }

        private static ExceptionData[] GetExceptionData(Exception ex)
        {
            var level = 0;

            return ex.GetExceptions()
                                    .Select(e => new ExceptionData()
                                    {
                                        _level = level++,
                                        _type = e.GetType().FullName ?? string.Empty,
                                        _message = e.Message ?? string.Empty,
                                        _source = e.Source ?? string.Empty,
                                        _stackTrace = e.StackTrace ?? string.Empty,
                                        _targetSite = e.TargetSite?.Name ?? string.Empty,
                                        _hResult = e.HResult
                                    })
                                    .ToArray();
        }

        public Task FlushAsync()
        {
            return LogQueueV2.Instance.FlushAsync();
        }
    }
}
