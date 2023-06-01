using EAI.AzureStorage;
using EAI.General;
using EAI.General.Cache;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.OnPrem.Storage
{
    public class OnPremClient
    {
        private static JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto,
        };

        private static CancellationTokenSource _cancelletionTokenSource = new CancellationTokenSource();
        private static RequestManager<OnPremMessage> _requestHandler = new RequestManager<OnPremMessage>();

        private static Task ResponseReceivedAsync(string jOnPremResponseMessage)
        {
            var onPremResponseMessage = (OnPremMessage)JsonConvert.DeserializeObject(jOnPremResponseMessage, _settings);

            _requestHandler.RequestCompleted(onPremResponseMessage);

            return Task.CompletedTask;
        }

        public string StorageQueueConnectionString { get => _storageQueue.StorageQueueConnectionString; set => _storageQueue.StorageQueueConnectionString = value; }
        public string StorageQueueName { get => _storageQueue.StorageQueueName; set => _storageQueue.StorageQueueName = value; }

        public string BlobStorageConnectionString { get => _storageQueue.BlobStorageConnectionString; set => _storageQueue.BlobStorageConnectionString = value; }
        public string ContainerName { get => _storageQueue.ContainerName; set => _storageQueue.ContainerName = value; }

        public bool EncodeMessage { get => _storageQueue.EncodeMessage; set => _storageQueue.EncodeMessage = value; }


        private LargeMessageQueue _storageQueue = new LargeMessageQueue();

        public async Task<responseT> SendRequest<responseT, requestT>(requestT requestMessage)
            where  requestT : OnPremMessage
            where responseT : OnPremMessage
        {
            var responseQueueName = $"{StorageQueueName}Response";

            requestMessage._requestId = Guid.NewGuid();
            requestMessage._responseQueueName = responseQueueName;

            var tcs = _requestHandler.RegisterRequest(requestMessage);

            var jOnPremRequestMessage = JsonConvert.SerializeObject(requestMessage, typeof(OnPremMessage), _settings);
            await _storageQueue.EnqueueAsync(jOnPremRequestMessage);

            await StartListenerAsync(responseQueueName);

            var onPremResponseMessage = await tcs.Task;

            var exceptionMessage = onPremResponseMessage as ExceptionMessage;
            if (exceptionMessage != null)
                throw exceptionMessage._exception;

            return (responseT)onPremResponseMessage;
        }

        private Task StartListenerAsync(string responseQueue)
        {
            return ResourceCache<StorageQueueListener>.GetResourceAsync(
                $"{StorageQueueConnectionString}-{responseQueue}", 
                () => Task.FromResult(
                    new ResourceCacheItem<StorageQueueListener>(CreateStorageQueueListener(responseQueue))
                    {
                        ExpiresOn = DateTime.MaxValue,
                    })
                );
        }

        private StorageQueueListener CreateStorageQueueListener(string responseQueue)
        {
            var storageQueue = new LargeMessageQueue()
            {
                StorageQueueConnectionString = _storageQueue.StorageQueueConnectionString,
                StorageQueueName = responseQueue,
                BlobStorageConnectionString = _storageQueue.BlobStorageConnectionString,
                ContainerName = _storageQueue.ContainerName,
                EncodeMessage = _storageQueue.EncodeMessage
            };

            var listener = new StorageQueueListener(storageQueue, ResponseReceivedAsync);

            Task.Run(() => listener.RunAsync(_cancelletionTokenSource.Token));

            return listener;
        }
    }
}
