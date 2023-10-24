using EAI.General;
using EAI.MessageQueue.SQL.Model;
using Json = Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using EAI.General.Extensions;
using EAI.Messaging.Abstractions;
using EAI.General.SettingJson;
using System.Threading;
using EAI.LoggingV2;
using EAI.LoggingV2.Levels;
using EAI.General.Storage;
using System.Collections.Generic;
using System.Net;

namespace EAI.MessageQueue.SQL
{
    [Singleton]
    public class MQueue : IMQueue, IService
    {
        private LoggerV2 _log;
        private IQueueMessageManager _messageManager;
        private IQueueMessageStore _messageStore;

        private TimeSpan _initialWait = new TimeSpan(0, 0, 0, 1);
        private TimeSpan _maxWait = new TimeSpan(0, 0, 0, 20);
        private TimeSpan _currentWait;
        private MessageQueueDestination[] _messageQueueDestinations;
        private string _stage;
        private string[] _endpoints;
        private ProcessContext _processContext;

        public LoggerV2 Log { get => _log; set => _log = value; }
        public IQueueMessageManager MessageManager { get => _messageManager; set => _messageManager = value; }
        public IQueueMessageStore MessageStore { get => _messageStore; set => _messageStore = value; }
        public MessageQueueDestination[] MessageQueueDestinations { get => _messageQueueDestinations; set => _messageQueueDestinations = value; }
        public string Stage { get => _stage; set => _stage = value; }
        public string[] Endpoints { get => _endpoints; set => _endpoints = value; }


        public async Task<long> EnqueueAsync(string endpointName, object message, string messageType = null, string messageKey = null)
        {
            var context = ProcessContext.GetCurrent();

            var queueMessage = new QueueMessage()
            {
                _endpointName = endpointName,
                _stage = context.Stage,
                _processId = context.ProcessId,
                _createdOnUTC = DateTime.UtcNow,
            };

            SetMessageData(queueMessage, message);

            if (!string.IsNullOrEmpty(messageType))
                queueMessage._messageType = messageType;

            if (!string.IsNullOrEmpty(messageKey))
                queueMessage._messageKey = messageKey;

            queueMessage._messageId = await _messageManager.EnqueueMessageAsync(queueMessage);

            if (_messageStore != null)
                await _messageStore.SaveAsync(queueMessage);

            return queueMessage._messageId;
        }

        private void SetMessageData(QueueMessage queueMessage, object messageContent)
        {
            switch (messageContent)
            {
                case string stringContent:
                    SetMessageDataFromString(queueMessage, stringContent);
                    break;
                case XElement xElement:
                    SetMessageDataFromXElement(queueMessage, xElement);
                    break;
                case XDocument xDocument:
                    SetMessageDataFromXElement(queueMessage, xDocument.Root);
                    break;
                case JObject jObject:
                    SetMessageDataFromJToken(queueMessage, jObject);
                    break;
                case JArray jArray:
                    SetMessageDataFromJToken(queueMessage, jArray);
                    break;
                default:
                    SetMessageDataFromObject(queueMessage, messageContent);
                    break;
            }

            queueMessage._messageHash = queueMessage._messageContent.CalculateSipHash();
        }

        private static void SetMessageDataFromString(QueueMessage queueMessage, string stringContent)
        {
            queueMessage._messageType = null;
            queueMessage._messageContent = stringContent;
            queueMessage._messageContentType = EnumMessageContentType.RAW.ToString();
        }

        private static void SetMessageDataFromXElement(QueueMessage queueMessage, XElement xElement)
        {
            queueMessage._messageType = $"{xElement.Name.NamespaceName}#{xElement.Name.NamespaceName}";
            queueMessage._messageContent = xElement.ToString();
            queueMessage._messageContentType = EnumMessageContentType.XML.ToString();
        }

        private static void SetMessageDataFromJToken(QueueMessage queueMessage, JToken jToken)
        {
            queueMessage._messageType = null;
            queueMessage._messageContent = jToken.ToString();
            queueMessage._messageContentType = EnumMessageContentType.Json.ToString();
        }
        private static void SetMessageDataFromObject(QueueMessage queueMessage, object messageContent)
        {
            queueMessage._messageType = messageContent.GetType().FullName;
            queueMessage._messageContent = Json.JsonConvert.SerializeObject(messageContent, Json.Formatting.Indented);
            queueMessage._messageContentType = EnumMessageContentType.Json.ToString();
            queueMessage._messageKey = (messageContent as IMessageTransactionKey)?.TransactionKey;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            using (var _ = new ProcessScope(null, null, GetType().FullName))
                try
                {
                    _processContext = ProcessContext.GetCurrent();

                    Log?.Start<Info>(null, null, $"Starting service {GetType().FullName}");

                    await Task.Run(() => RunLoopAsync(cancellationToken));

                    Log?.Success<Info>();
                }
                catch (Exception ex)
                {
                    Log?.Failed<Error>(ex);

                    throw;
                }
        }

        public async Task RunLoopAsync(CancellationToken cancellationToken)
        {
            ProcessContext.Restore(_processContext);

            _currentWait = _initialWait;

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await foreach (var message in DequeueAsync())
                    {
                        foreach(var destination in _messageQueueDestinations)
                        {
                            if(destination.IsMatch(message._endpointName, message._messageType))
                                await destination.SendMessageAsync(this, message);
                        }

                        _currentWait = _initialWait;
                    }
                }
                catch (Exception ex)
                {
                    Log?.Exception<Error>(ex, "dequeue failed");
                }

                await Task.Delay(_currentWait);

                _currentWait += _currentWait * 0.25;
                if (_currentWait > _maxWait)
                    _currentWait = _maxWait;
            }
        }

        private async IAsyncEnumerable<QueueMessage> DequeueAsync()
        {
            if (_stage == null || _endpoints == null)
                yield break;

            foreach(var endpoint in _endpoints)
                await foreach (var queueMessage in _messageManager.DequeueMessagesAsync(_stage, endpoint))
                {
                    if (_messageStore != null) 
                        await _messageStore.LoadAsync(queueMessage);

                    yield return queueMessage;
                }
        }

        internal Task SuccessAsync(QueueMessage queueMessage)
        {
            return _messageManager.SuccessAsync(queueMessage._messageId);
        }

        internal Task FailedAsync(QueueMessage queueMessage)
        {
            return _messageManager.FailedAsync(queueMessage._messageId);
        }
    }
}
