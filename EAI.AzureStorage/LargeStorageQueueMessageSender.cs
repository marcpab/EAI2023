using EAI.General;
using EAI.General.Storage;
using EAI.Messaging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EAI.AzureStorage
{
    public class LargeStorageQueueMessageSender : IMessageSender
    {
        private static readonly Regex _replaceInvalidQNameChars = new Regex("[^a-zA-Z-0-9-]+", RegexOptions.Compiled);

        private LargeMessageQueue _largeMessageQueue = new LargeMessageQueue();
        private string _storageQueueName;

        public string StorageQueueConnectionString { get => _largeMessageQueue.StorageQueueConnectionString; set => _largeMessageQueue.StorageQueueConnectionString = value; }
        public string StorageQueueName { get => _storageQueueName; set => _storageQueueName = value; }
        public string BlobStorageConnectionString { get => _largeMessageQueue.BlobStorageConnectionString; set => _largeMessageQueue.BlobStorageConnectionString = value; }
        public string ContainerName { get => _largeMessageQueue.ContainerName; set => _largeMessageQueue.ContainerName = value; }
        public bool EncodeMessage { get => _largeMessageQueue.EncodeMessage; set => _largeMessageQueue.EncodeMessage = value; }

        public Task SendMessageAsync(object message)
        {
            return SendMessageAsync(message, GetMessageType(message), null);
        }

        public Task SendMessageAsync(object message, string messageType, string transactionKey)
        {
            var storageQueueName = _storageQueueName;

            if (string.IsNullOrEmpty(storageQueueName))
                throw new EAIException("No storage queue name defined");
            
            storageQueueName = storageQueueName.Replace($"{{{nameof(messageType)}}}", messageType);

            storageQueueName = _replaceInvalidQNameChars.Replace(storageQueueName, "-");

            var largeMessageQueue = GetQueue(storageQueueName);

            switch(message)
            {
                case string messageString:
                    return largeMessageQueue.EnqueueAsync(messageString);
                case JToken messageJson:
                    return largeMessageQueue.EnqueueAsync(messageJson.ToString());
                default:
                    return largeMessageQueue.EnqueueAsync(JsonConvert.SerializeObject(message));
            }
        }

        private LargeMessageQueue GetQueue(string storageQueueName)
        {
            if(storageQueueName == _storageQueueName)
                return _largeMessageQueue;
            
            return new LargeMessageQueue()
            {
                StorageQueueConnectionString = StorageQueueConnectionString,
                StorageQueueName = storageQueueName,
                BlobStorageConnectionString = BlobStorageConnectionString,
                ContainerName = ContainerName,
                EncodeMessage = EncodeMessage
            };
        }

        private string GetMessageType(object message)
        {
            var messageType = message as IMessageType;

            if(messageType != null)
                return messageType.MessageType;

            return message?.GetType()?.Name;
        }
    }
}
