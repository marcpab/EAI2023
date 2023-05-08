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

namespace EAI.AzureStorage
{
    public class StorageQueue : IStorageQueue
    {
        public string ConnectionString { get; set; }
        public string StorageQueueName { get; set; }
        public bool EncodeMessage { get; set; }

        public async Task EnqueuAsync(string messageContent)
        {
            var client = await GetQueueClientAsync();

            await client.SendMessageAsync(messageContent);
        }

        public async IAsyncEnumerable<StorageQueueMessage> DequeueAsync(int maxMessages, DequeueType dequeueType = DequeueType.ManualComplete)
        {
            var client = await GetQueueClientAsync();

            var messages = await client.ReceiveMessagesAsync(maxMessages);

            foreach (var message in messages.Value)
            {
                if(dequeueType == DequeueType.AutoComplete)
                    await client.DeleteMessageAsync(message.MessageId, message.PopReceipt);

                yield return new StorageQueueMessage(message.MessageId, message.PopReceipt, message.Body.ToString());
            };
        }

        public async Task CompletedAsync(StorageQueueMessage message)
        {
            var client = await GetQueueClientAsync();

            await client.DeleteMessageAsync(message.MessageId, message.PopReceipt);
        }

        private async Task<QueueClient> GetQueueClientAsync()
        {
            return await ResourceCache<QueueClient>.GetResourceAsync(
                        $"{ConnectionString}-{StorageQueueName}-{EncodeMessage}",
                        async () => new ResourceCacheItem<QueueClient>(await CreateQueueClientAsync(ConnectionString, StorageQueueName, EncodeMessage))
                        {
                            ExpiresOn = DateTime.UtcNow.AddHours(8)
                        }
                        );
        }

        private async Task<QueueClient> CreateQueueClientAsync(string connectionString, string storageQueueName, bool encodeMessage)
        {
            var options = new QueueClientOptions
            {
                MessageEncoding = encodeMessage ? QueueMessageEncoding.Base64 : QueueMessageEncoding.None
            };

            var client = new QueueClient(connectionString, storageQueueName.ToLower(), options);
            var createResponse = await client.CreateIfNotExistsAsync();
            if (createResponse != null)
            {

#warning                 Log
            }

            return client;
        }

    }
}
