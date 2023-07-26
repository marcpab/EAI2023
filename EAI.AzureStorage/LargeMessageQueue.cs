using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using EAI.General.Storage;
using EAI.General.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Buffers.Text;
using Newtonsoft.Json;
using EAI.General;

namespace EAI.AzureStorage
{
    public class LargeMessageQueue : IStorageQueue
    {
        private const int _maxStorageQueueMessageSize = ushort.MaxValue;
        private const int _maxEncodedStorageQueueMessageSize = ushort.MaxValue / 4 * 3;

        private BlobStorage _blobStorage = new BlobStorage();
        private StorageQueue _storageQueue = new StorageQueue();
        
        public string StorageQueueConnectionString { get => _storageQueue.ConnectionString; set => _storageQueue.ConnectionString = value; }
        public string StorageQueueName { get => _storageQueue.StorageQueueName; set => _storageQueue.StorageQueueName = value; }
        public string BlobStorageConnectionString { get => _blobStorage.ConnectionString; set => _blobStorage.ConnectionString = value; }
        public string ContainerName { get => _blobStorage.RootPath; set => _blobStorage.RootPath = value; }
        public bool EncodeMessage { get => _storageQueue.EncodeMessage; set => _storageQueue.EncodeMessage = value; }

        public async Task EnqueueAsync(string messageContent)
        {
            //if (encodeMessage)
            //    messageContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(messageContent));

            var message = new QueueMessage()
            {
                _processContext = ProcessContext.GetCurrent(),
                _storageLocation = StorageLocationEnum.StorageQueue,
                _content = messageContent
            };

            var jMessage = JsonConvert.SerializeObject(message);

            var maxMessageSize = _storageQueue.EncodeMessage ? _maxEncodedStorageQueueMessageSize : _maxStorageQueueMessageSize;
            if(jMessage.Length <= maxMessageSize)
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
                yield return await GetMessage(storageMessage, dequeueType); 
        }


        public async Task<StorageQueueMessage> FromStorageQueueTrigger(string myQueueItem, DequeueType dequeueType = DequeueType.ManualComplete)
        {
            var storageQueueMessage = new StorageQueueMessage(null, null, myQueueItem);

            return await GetMessage(storageQueueMessage, dequeueType);
        }

        public async Task CompletedAsync(StorageQueueMessage message)
        {
            var blobName = (message as LargeStorageQueueMessage)?.BlobName;

            if(blobName != null)
                await _blobStorage.DeleteAsync(blobName);

            if(message.MessageId != null)
                await _storageQueue.CompletedAsync(message);
        }

        private async Task<LargeStorageQueueMessage> GetMessage(StorageQueueMessage storageMessage, DequeueType dequeueType)
        {
            var message = JsonConvert.DeserializeObject<QueueMessage>(storageMessage.MessageContent);

            var content = await GetContentAsync(message);

            var storageQueueMessage = new LargeStorageQueueMessage(
                                                storageMessage.MessageId,
                                                storageMessage.PopReceipt,
                                                message._processContext,
                                                content,
                                                message._storageLocation == StorageLocationEnum.BlobStorage ? message._content : null
                                            );

            if (dequeueType == DequeueType.AutoComplete)
                await CompletedAsync(storageQueueMessage);
            return storageQueueMessage;
        }

        private async Task<string> GetContentAsync(QueueMessage message)
        {
            if (message._storageLocation == StorageLocationEnum.StorageQueue)
                return message._content;

            return await _blobStorage.GetBlobAsStringAsync(message._content);
        }

        public Task DeleteStorageQueueAsync()
        {
            return _storageQueue.DeleteAsync();
        }
    }
}
