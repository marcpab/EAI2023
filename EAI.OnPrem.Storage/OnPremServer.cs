using EAI.AzureStorage;
using EAI.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.OnPrem.Storage
{
    public class OnPremServer : IRequestListener, IService
    {
        private static JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto,
        };

        public string StorageQueueConnectionString { get => _storageQueue.StorageQueueConnectionString; set => _storageQueue.StorageQueueConnectionString = value; }
        public string StorageQueueName { get => _storageQueue.StorageQueueName; set => _storageQueue.StorageQueueName = value; }

        public string BlobStorageConnectionString { get => _storageQueue.BlobStorageConnectionString; set => _storageQueue.BlobStorageConnectionString = value; }
        public string ContainerName { get => _storageQueue.ContainerName; set => _storageQueue.ContainerName = value; }

        public bool EncodeMessage { get => _storageQueue.EncodeMessage; set => _storageQueue.EncodeMessage = value; }

        public IServiceRequestDispatcher[] Dispatchers { get => _dispatchers; set => _dispatchers = value; }

        private Dictionary<Type, Func<OnPremMessage, Task<OnPremMessage>>> _requestMap = new Dictionary<Type, Func<OnPremMessage, Task<OnPremMessage>>>();
        private LargeMessageQueue _storageQueue = new LargeMessageQueue();
        private IServiceRequestDispatcher[] _dispatchers;



        public Task RunAsync(CancellationToken cancellationToken)
        {
            if(_dispatchers != null)
                foreach (var dispatcher in _dispatchers)
                    dispatcher.Initialize(this);

            var storeageQueueListener = new StorageQueueListener(_storageQueue, RequestReceivedAsync);

            return Task.Run(() => storeageQueueListener.RunAsync(cancellationToken));
        }

        private async Task RequestReceivedAsync(string jOnPremRequestMessage)
        {
            var onPremRequestMessage = (OnPremMessage)JsonConvert.DeserializeObject(jOnPremRequestMessage, _settings);

            var processRequestAsync = _requestMap[onPremRequestMessage.GetType()];

            var onPremResponseMessage = await processRequestAsync(onPremRequestMessage);

            //            onPremResponseMessage._type = onPremResponseMessage.GetType().FullName;
            onPremResponseMessage._requestId = onPremRequestMessage._requestId;

            var sendStorageQueue = CreateResponseQueue(onPremRequestMessage);

            var jOnPremResponseMessage = JsonConvert.SerializeObject(onPremResponseMessage, _settings);

            await sendStorageQueue.EnqueueAsync(jOnPremResponseMessage);
        }

        private LargeMessageQueue CreateResponseQueue(OnPremMessage onPremRequestMessage)
        {
            return new LargeMessageQueue()
            {
                StorageQueueConnectionString = _storageQueue.StorageQueueConnectionString,
                StorageQueueName = onPremRequestMessage._responseQueueName,
                BlobStorageConnectionString = _storageQueue.BlobStorageConnectionString,
                ContainerName = _storageQueue.ContainerName,
                EncodeMessage = _storageQueue.EncodeMessage
            };
        }

        public void RegisterRequestHandler<requestT, responseT>(Func<requestT, Task<responseT>> requestHandler)
        {
            _requestMap.Add(typeof(requestT), async (r) => (OnPremMessage)(object)await requestHandler((requestT)(object)r));
        }
    }
}
