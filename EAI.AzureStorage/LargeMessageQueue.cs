using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using EAI.General.Storage;
using EAI.General.Cache;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Buffers.Text;
using Newtonsoft.Json;

namespace EAI.AzureStorage
{
    public class LargeMessageQueue : IStorageQueue
    {
        private const int _maxStorageQueueMessageSize = ushort.MaxValue;

        private BlobStorage _blobStorage = new BlobStorage();
        private StorageQueue _storageQueue = new StorageQueue();
        private bool _encodeMessage;
        
        public string StorageQueueConnectionString { get => _storageQueue.ConnectionString; set => _storageQueue.ConnectionString = value; }
        public string StorageQueueName { get => _storageQueue.StorageQueueName; set => _storageQueue.StorageQueueName = value; }
        public string BlobStorageConnectionString { get => _blobStorage.ConnectionString; set => _blobStorage.ConnectionString = value; }
        public string ContainerName { get => _blobStorage.RootPath; set => _blobStorage.RootPath = value; }
        public bool EncodeMessage { get => _encodeMessage; set => _encodeMessage = value; }

        public async Task EnqueueAsync(string messageContent)
        {
            var encodeMessage = _encodeMessage;

            _storageQueue.EncodeMessage = false;

            if (encodeMessage)
                messageContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(messageContent));

            var message = new QueueMessage()
            {
                _encoded = encodeMessage,
                _storageLocation = StorageLocationEnum.StorageQueue,
                _content = messageContent
            };

            var jMessage = JsonConvert.SerializeObject(message);

            if(jMessage.Length <= _maxStorageQueueMessageSize)
            {
                await _storageQueue.EnqueueAsync(jMessage);
                return;
            }

            message._storageLocation = StorageLocationEnum.BlobStorage;
            message._content = $"{Guid.NewGuid()}.msg";
            jMessage = JsonConvert.SerializeObject(message);

            await _blobStorage.SaveBlobAsync(message._content, messageContent);
            await _storageQueue.EnqueueAsync(jMessage);
        }

        public async IAsyncEnumerable<StorageQueueMessage> DequeueAsync(int maxMessages, DequeueType dequeueType = DequeueType.ManualComplete)
        {
            await foreach(var storageMessage in _storageQueue.DequeueAsync(maxMessages))
            {
                var message = JsonConvert.DeserializeObject<QueueMessage>(storageMessage.MessageContent);

                var content = await GetContentAsync(message);

                if (message._encoded)
                    content = Encoding.UTF8.GetString(Convert.FromBase64String(content));

                var storageQueueMessage = new LargeStorageQueueMessage(
                                                    storageMessage.MessageId, 
                                                    storageMessage.PopReceipt, 
                                                    content, 
                                                    message._storageLocation == StorageLocationEnum.BlobStorage ? message._content : null
                                                );

                if (dequeueType == DequeueType.AutoComplete)
                    await CompletedAsync(storageQueueMessage);

                yield return storageQueueMessage;
            }
        }

        public async Task CompletedAsync(StorageQueueMessage message)
        {
            var blobName = (message as LargeStorageQueueMessage)?.BlobName;

            if(blobName != null)
                await _blobStorage.DeleteAsync(blobName);

            await _storageQueue.CompletedAsync(message);
        }

        private async Task<string> GetContentAsync(QueueMessage message)
        {
            if (message._storageLocation == StorageLocationEnum.StorageQueue)
                return message._content;

            return await _blobStorage.GetBlobAsStringAsync(message._content);
        }
    }
}
