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
        private static TimeSpan S_criticalSpan = TimeSpan.FromSeconds(10);
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        private IMessageManager _manager { get; set; }
        private IMessageSender _sender { get; set; }
        private IConfiguration _configuration { get; set; }
        private MessageQueueConfiguration _mq { get; set; }
        private ILogger _log { get; set; }
        private string _name = string.Empty;

        public MessageQueue(IConfiguration configuration, ILogger log, string callerId)
        {
            _configuration = configuration;
            var mq = GetConfiguration(_configuration).Result;

            if (mq == null)
                throw new InvalidOperationException("Message Queue configuration 'MQ' is missing in your host.json!");
            _mq = mq;

            if (_configuration["AzureWebJobsStorage"] == null && _configuration["Values:AzureWebJobsStorage"] == null)
                throw new InvalidOperationException("AzureWebJobsStorage application setting env is missing!");

            var manager = (IMessageManager?)Activator.CreateInstance(_mq.ManagerType, new object[] { configuration, log });
            if (manager == null)
                throw new ArgumentNullException("ManagerType");
            _manager = manager;

            var sender = (IMessageSender?)Activator.CreateInstance(_mq.SenderType, new object[] { configuration, log });
            if (sender == null)
                throw new ArgumentNullException("SenderType");
            _sender = sender;

            _name = callerId;
            _log = log;
        }

        private async Task<MessageQueueConfiguration?> GetConfiguration(IConfiguration configuration)
        {
            try
            {
                var start = DateTimeOffset.UtcNow; // we cannot take stopwatch, because thread could sleep

                var cs = configuration["AzureWebJobsStorage"] ?? configuration[$"Values:{"AzureWebJobsStorage"}"];

                var client = new BlobContainerClient(cs, "roedl-configuration");
                var blob = client.GetBlobClient("mq.json");

                var span = start.Subtract(DateTimeOffset.UtcNow);
                if (span > S_criticalSpan)
                {
                    _log.LogWarning($"[MQ {_name}] slow processing in GetConfiguration {span.Seconds} sec");
                }


                if (await blob.ExistsAsync().ConfigureAwait(true) == false)
                {
                    _log.LogWarning($"[MQ {_name}] DefaultManager.GetConfiguration: no global mq.json found, trying to read from host.json...");
                    return configuration.Get<MessageQueueConfiguration>("MQ");
                }

                return await blob.DownloadAsync<MessageQueueConfiguration>().ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                _log.LogWarning($"[MQ {_name}] DefaultManager.GetConfiguration: {ex.Message} {ex.InnerException?.Message}");
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
                message = MessageItem.Create(queueName, payload, secondaryKey, messageType, _mq.ManagerType);

                _log.LogInformation($"[MQ.{queueName}] Enqueue for {message.Id} type {messageType} key {secondaryKey}");

                step = 1;
                _ = await _manager.EnqueueAsync(message);
                step = 2;

                if (triggerDequeue)
                    await Dequeue();
            }
            catch (Exception ex)
            {
                _log.LogError($"[MQ {_name}] MessageQueue (step {step}) ex: {ex.Message} {ex.InnerException?.Message}");
            }
            finally
            {
                _semaphoreSlim.Release();
            }

            return message;
        }

        public async Task Release(MessageItem message, bool fault)
        {
            await _manager.ReleaseAsync(message, fault);

            await Dequeue();
        }

        public async Task Dequeue(bool dupedetection = false)
        {
            await foreach (var queue in _manager.GetQueues())
            {
                var isMessageDequeued = true;
                while (isMessageDequeued)
                {
                    var start = DateTimeOffset.UtcNow; // we cannot take stopwatch, because thread could sleep

                    try
                    {
                        isMessageDequeued = false;
                        var timeout = _mq.GetTimeout(queue);
                        var maxTickets = _mq.GetMaxTickets(queue);

                        //_log.LogInformation($"[MQ.{queue}] Dequeue to AzureQueue start...");
                        await foreach (var message in _manager.DequeueAsync(queue, maxTickets, timeout))
                        {
                            //_log.LogInformation($"[MQ.{queue}] Dequeue {message.Endpoint}-{message.MessageType}");
                            await SendMessage(message);
                            isMessageDequeued = true;
                        }

                        var span = start.Subtract(DateTimeOffset.UtcNow);
                        if (span > S_criticalSpan)
                        {
                            _log.LogWarning($"[MQ {_name}] slow processing in Dequeue {span.Seconds} sec");
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.LogError($"[MQ {_name}] MessageQueue.Dequeue ex in {queue}: {ex.Message} {ex.InnerException?.Message} status: {_manager.Status} stack: {ex.StackTrace}");
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
                    var timeout = _mq.GetTimeout(queue);
                    var maxTickets = _mq.GetMaxTickets(queue);

                    await foreach (var message in _manager.DequeueAsync(queue, maxTickets, timeout))
                    {
                        await SendMessage(message);
                        isMessageDequeued = true;
                        cntMessages++;
                    }

                    var span = start.Subtract(DateTimeOffset.UtcNow);
                    if (span > S_criticalSpan)
                    {
                        _log.LogWarning($"[MQ {_name}] slow processing in Dequeue(q) {span.Seconds} sec");
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError($"[MQ {_name}] MessageQueue.Dequeue(q) ex in {queue}: {ex.Message} {ex.InnerException?.Message} status: {_manager.Status} stack: {ex.StackTrace}");
                }
            }

            return cntMessages;
        }

        private async Task SendMessage(MessageItem message)
        {
            await _sender.SendMessageAsync(message);
        }

        public async Task<string> GetPayload(MessageItem message)
        {
            return await Task.Run(() => {
                return message.Payload;
            });
        }

        public async Task<MessageItem?> GetPayload(string jsonTicket)
            => await MessageQueueTicket.GetMessageItem(_configuration, jsonTicket);
    }
}

