using EAI.General;
using EAI.General.Storage;
using EAI.Messaging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAI.AzureStorage
{
    public class LargeStorageQueueMessageSender : IMessageSender
    {
        private LargeMessageQueue _largeMessageQueue = new LargeMessageQueue();

        public string StorageQueueConnectionString { get => _largeMessageQueue.StorageQueueConnectionString; set => _largeMessageQueue.StorageQueueConnectionString = value; }
        public string StorageQueueName { get => _largeMessageQueue.StorageQueueName; set => _largeMessageQueue.StorageQueueName = value; }
        public string BlobStorageConnectionString { get => _largeMessageQueue.BlobStorageConnectionString; set => _largeMessageQueue.BlobStorageConnectionString = value; }
        public string ContainerName { get => _largeMessageQueue.ContainerName; set => _largeMessageQueue.ContainerName = value; }
        public bool EncodeMessage { get => _largeMessageQueue.EncodeMessage; set => _largeMessageQueue.EncodeMessage = value; }

        public Task SendMessageAsync(object message)
        {
            switch(message)
            {
                case string messageString:
                    return _largeMessageQueue.EnqueueAsync(messageString);
                case JToken messageJson:
                    return _largeMessageQueue.EnqueueAsync(messageJson.ToString());
                default:
                    return _largeMessageQueue.EnqueueAsync(JsonConvert.SerializeObject(message));
            }
        }
    }
}
