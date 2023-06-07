using Azure.Storage.Blobs;
using EAI.MessageQueue.Storage.Extensions;
using EAI.MessageQueue.Storage.Manager;
using EAI.MessageQueue.Storage.Sender;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EAI.MessageQueue.Storage
{
     public class MessageQueue
    {
        private static readonly TimeSpan S_criticalSpan = TimeSpan.FromSeconds(10);
        private static readonly SemaphoreSlim _semaphoreSlim = new(1, 1);
        private IMessageManager Manager { get; set; }
        private IMessageSender Sender { get; set; }
        private IConfiguration Configuration { get; set; }
        private MessageQueueConfiguration MQ { get; set; }
        private ILogger Log { get; set; }
        private string Name { get; set; } = string.Empty;

        public MessageQueue(IConfiguration configuration, ILogger log, string callerId)
        {
            Configuration = configuration;
            var mq = GetConfiguration(Configuration).Result
                ?? throw new InvalidOperationException("Message Queue configuration 'MQ' is missing in your host.json!");
            MQ = mq;

            if (Configuration[EAI.Texts.DefaultStorage.StorageConfigurationKey] == null 
                && Configuration[$"Values:{EAI.Texts.DefaultStorage.StorageConfigurationKey}"] == null)
                throw new InvalidOperationException($"{EAI.Texts.DefaultStorage.StorageConfigurationKey} application setting env is missing!");

            var manager = (IMessageManager?)Activator.CreateInstance(MQ.ManagerType, new object[] { configuration, log }) 
                ?? throw new ArgumentException("ManagerType");
            Manager = manager;

            var sender = (IMessageSender?)Activator.CreateInstance(MQ.SenderType, new object[] { configuration, log })
                ?? throw new ArgumentException("SenderType");
            Sender = sender;

            Name = callerId;
            Log = log;
        }

        private async Task<MessageQueueConfiguration?> GetConfiguration(IConfiguration configuration)
        {
            try
            {
                var start = DateTimeOffset.UtcNow; // we cannot take stopwatch, because thread could sleep

                var cs = configuration[EAI.Texts.DefaultStorage.StorageConfigurationKey] ?? configuration[$"Values:{EAI.Texts.DefaultStorage.StorageConfigurationKey}"];

                var client = new BlobContainerClient(cs, EAI.Texts.DefaultStorage.ConfigurationContainer);
                var blob = client.GetBlobClient(EAI.Texts.DefaultStorage.ConfigurationFile);

                var span = start.Subtract(DateTimeOffset.UtcNow);
                if (span > S_criticalSpan)
                {
                    Log.LogWarning($"[MQ {Name}] slow processing in GetConfiguration {span.Seconds} sec");
                }


                if (await blob.ExistsAsync().ConfigureAwait(true) == false)
                {
                    Log.LogWarning($"[MQ {Name}] DefaultManager.GetConfiguration: no global {EAI.Texts.DefaultStorage.ConfigurationFile} found, trying to read from host.json...");
                    return configuration.Get<MessageQueueConfiguration>("MQ");
                }

                return await blob.DownloadAsync<MessageQueueConfiguration>().ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                Log.LogWarning($"[MQ {Name}] DefaultManager.GetConfiguration: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }

        public async Task<MessageItem> Enqueue(string queueName, string messageType, string secondaryKey, string payload, bool triggerDequeue = true)
        {
            int step = 0;
            var message = new MessageItem();

            await _semaphoreSlim.WaitAsync();

            try
            {
                message = MessageItem.Create(queueName, payload, secondaryKey, messageType, MQ.ManagerType);

                Log.LogInformation($"[MQ.{queueName}] Enqueue for {message.Id} type {messageType} key {secondaryKey}");

                step = 1;
                _ = await Manager.EnqueueAsync(message);
                step = 2;

                if (triggerDequeue)
                    await Dequeue();
            }
            catch (Exception ex)
            {
                Log.LogError($"[MQ {Name}] MessageQueue (step {step}) ex: {ex.Message} {ex.InnerException?.Message}");
            }
            finally
            {
                _semaphoreSlim.Release();
            }

            return message;
        }

        public async Task Release(MessageItem message, bool fault)
        {
            await Manager.ReleaseAsync(message, fault);

            await Dequeue();
        }

        public async Task Dequeue(bool dupedetection = false)
        {
            await foreach (var queue in Manager.GetQueues())
            {
                var isMessageDequeued = true;
                while (isMessageDequeued)
                {
                    var start = DateTimeOffset.UtcNow; // we cannot take stopwatch, because thread could sleep

                    try
                    {
                        isMessageDequeued = false;
                        var timeout = MQ.GetTimeout(queue);
                        var maxTickets = MQ.GetMaxTickets(queue);

                        //Log.LogInformation($"[MQ.{queue}] Dequeue to AzureQueue start...");
                        await foreach (var message in Manager.DequeueAsync(queue, maxTickets, timeout))
                        {
                            //Log.LogInformation($"[MQ.{queue}] Dequeue {message.Endpoint}-{message.MessageType}");
                            await SendMessage(message);
                            isMessageDequeued = true;
                        }

                        var span = start.Subtract(DateTimeOffset.UtcNow);
                        if (span > S_criticalSpan)
                        {
                            Log.LogWarning($"[MQ {Name}] slow processing in Dequeue {span.Seconds} sec");
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.LogError($"[MQ {Name}] MessageQueue.Dequeue ex in {queue}: {ex.Message} {ex.InnerException?.Message} status: {Manager.Status} stack: {ex.StackTrace}");
                    }
                }
            }
        }

        public async Task<int> Dequeue(string queue, bool dupedetection = false)
        {
            var cntMessages = 0;
            var isMessageDequeued = true;
            while (isMessageDequeued)
            {
                var start = DateTimeOffset.UtcNow; // we cannot take stopwatch, because thread could sleep

                try
                {
                    isMessageDequeued = false;
                    var timeout = MQ.GetTimeout(queue);
                    var maxTickets = MQ.GetMaxTickets(queue);

                    await foreach (var message in Manager.DequeueAsync(queue, maxTickets, timeout))
                    {
                        await SendMessage(message);
                        isMessageDequeued = true;
                        cntMessages++;
                    }

                    var span = start.Subtract(DateTimeOffset.UtcNow);
                    if (span > S_criticalSpan)
                    {
                        Log.LogWarning($"[MQ {Name}] slow processing in Dequeue(q) {span.Seconds} sec");
                    }
                }
                catch (Exception ex)
                {
                    Log.LogError($"[MQ {Name}] MessageQueue.Dequeue(q) ex in {queue}: {ex.Message} {ex.InnerException?.Message} status: {Manager.Status} stack: {ex.StackTrace}");
                }
            }

            return cntMessages;
        }

        private async Task SendMessage(MessageItem message)
            => await Sender.SendMessageAsync(message);

        public static async Task<string> GetPayload(MessageItem message)
            => await Task.FromResult(message.Payload);

        public async Task<MessageItem?> GetPayload(string jsonTicket)
            => await MessageQueueTicket.GetMessageItem(Configuration, jsonTicket);
    }
}

