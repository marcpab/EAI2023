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

namespace EAI.MessageQueue.SQL
{
    [Singleton]
    public class MQueue : IMQueue, IService
    {
        private LoggerV2 _log;
        private IQueueMessageManager _messageManager;
        private IQueueMessageStore _messageStore;

        private TimeSpan _initialWait = new TimeSpan(0, 0, 0, 5);
        private TimeSpan _maxWait = new TimeSpan(0, 0, 10, 0, 0);
        private TimeSpan _currentWait;

        public LoggerV2 Log { get => _log; set => _log = value; }
        public IQueueMessageManager MessageManager { get => _messageManager; set => _messageManager = value; }
        public IQueueMessageStore MessageStore { get => _messageStore; set => _messageStore = value; }
        public MessageQueueDestination[] MessageQueueDestinations { get; set; }


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

            queueMessage._messageId = await _messageManager.EnqueueMessage(queueMessage);

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
            _currentWait = _initialWait;

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await foreach (var message in DequeueAsync())
                    {



                        _currentWait = _initialWait;
                    }

                    await Task.Delay(_currentWait);

                    _currentWait += _currentWait * 0.25;
                    if (_currentWait > _maxWait)
                        _currentWait = _maxWait;
                }
                catch (Exception ignoreException)
                {

                }
            }
        }

        private async IAsyncEnumerable<QueueMessage> DequeueAsync()
        {
            await foreach (var queueMessage in _messageManager.DequeueMessages())
            {
                if (_messageStore != null)
                    await _messageStore.LoadAsync(queueMessage);

                yield return queueMessage;
            }
        }
    }
}
