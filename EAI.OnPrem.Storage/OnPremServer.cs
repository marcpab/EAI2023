using EAI.AzureStorage;
using EAI.General;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.OnPrem.Storage
{
    public class OnPremServer : IRequestListener
    {
        public string StorageQueueConnectionString { get => _storageQueue.StorageQueueConnectionString; set => _storageQueue.StorageQueueConnectionString = value; }
        public string StorageQueueName { get => _storageQueue.StorageQueueName; set => _storageQueue.StorageQueueName = value; }

        public string BlobStorageConnectionString { get => _storageQueue.BlobStorageConnectionString; set => _storageQueue.BlobStorageConnectionString = value; }
        public string ContainerName { get => _storageQueue.ContainerName; set => _storageQueue.ContainerName = value; }

        public bool EncodeMessage { get => _storageQueue.EncodeMessage; set => _storageQueue.EncodeMessage = value; }

        private LargeMessageQueue _storageQueue = new LargeMessageQueue();
        private Func<string, Task<string>> _processRequestAsync;


        public Task RunAsync(Func<string, Task<string>> processRequestAsync, CancellationToken cancellationToken)
        {
            _processRequestAsync = processRequestAsync;
            var storeageQueueListener = new StorageQueueListener(_storageQueue, RequestReceivedAsync);

            return Task.Run(() => storeageQueueListener.RunAsync(cancellationToken));
        }

        private async Task RequestReceivedAsync(string jOnPremRequestMessage)
        {
            var onPremRequestMessage = JsonConvert.DeserializeObject<OnPremMessage>(jOnPremRequestMessage);

            onPremRequestMessage._payload = await _processRequestAsync(onPremRequestMessage._payload);

            var sendStorageQueue = new LargeMessageQueue()
            {
                StorageQueueConnectionString = _storageQueue.StorageQueueConnectionString,
                StorageQueueName = onPremRequestMessage._responseQueueName,
                BlobStorageConnectionString = _storageQueue.BlobStorageConnectionString,
                ContainerName = _storageQueue.ContainerName,
                EncodeMessage = _storageQueue.EncodeMessage
            };

            var jOnPremResponseMessage = JsonConvert.SerializeObject(onPremRequestMessage);

            await sendStorageQueue.EnqueueAsync(jOnPremResponseMessage);
        }
    }
}
