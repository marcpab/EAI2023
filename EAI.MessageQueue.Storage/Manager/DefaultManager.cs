using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure;
using EAI.MessageQueue.Storage.Ticket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using EAI.MessageQueue.Storage.Extensions;
using System.Security.Cryptography;
using Azure.Storage.Blobs.Specialized;

namespace EAI.MessageQueue.Storage.Manager
{
    public class DefaultManager : IMessageManager
    {
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);        

        private SecureString ConnectionString { get; set; }
        private MessageQueueConfiguration _mqc { get; set; }

        public string GetContainerEnqueue(string queueName) => EAI.Texts.DefaultStorage.BlobContainer(queueName);
        public string GetContainerDequeue(string queueName) => EAI.Texts.DefaultStorage.BlobDequeueContainer(queueName);
        public string GetContainerArchive(string queueName) => EAI.Texts.DefaultStorage.BlobArchiveContainer(queueName);

        private string _cs => new NetworkCredential(string.Empty, ConnectionString).Password;
        private ILogger _log;

        public string LeaseId { get; private set; } = string.Empty;

        public DequeueStatus Status { get; private set; }

        public DefaultManager(IConfiguration configuration, ILogger log)
        {
            var cs = configuration[EAI.Texts.DefaultStorage.StorageConfigurationKey] ?? configuration[$"Values:{EAI.Texts.DefaultStorage.StorageConfigurationKey}"];
            ConnectionString = new NetworkCredential(string.Empty, cs).SecurePassword;

            Status = DequeueStatus.None;

            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            _log = log;

            var mqc = GetConfiguration(configuration).Result;
            if(mqc == null)
            {
                throw new ArgumentNullException("configuration");
            }
            _mqc = mqc;
        }

        private async Task<MessageQueueConfiguration?> GetConfiguration(IConfiguration configuration)
        {
            try
            {
                var client = new BlobContainerClient(_cs, EAI.Texts.DefaultStorage.ConfigurationContainer);
                var blob = client.GetBlobClient("mq.json");

                if (await blob.ExistsAsync().ConfigureAwait(true) == false)
                {
                    _log.LogWarning($"[MQ] DefaultManager.GetConfiguration: no global mq.json found, trying to read from host.json...");
                    return configuration.Get<MessageQueueConfiguration>("MQ");
                }

                return await blob.DownloadAsync<MessageQueueConfiguration>().ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                _log.LogWarning($"[MQ] DefaultManager.GetConfiguration: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }

        public async Task<int> ContainerCount(string containerName)
        {
            var client = new BlobContainerClient(_cs, containerName);
            return await ContainerCount(client).ConfigureAwait(true);
        }

        private async Task<int> ContainerCount(BlobContainerClient client)
        {
            if (await client.ExistsAsync() == false)
                return 0;

            return (await client.ListBlobNamesAsync()).Count;
        }

        public async IAsyncEnumerable<MessageItem> DequeueAsync(IQueueLease lease, string queueName, int maxTickets)
        {
            if (lease == null) // we didnt got any lock
            {
                Status = DequeueStatus.NoLock;
                yield break;
            }

            if (await lease.RenewAsync(true) == false) // check if the lease is valid at all
            {
                Status = DequeueStatus.RenewLeaseFailed;
                yield break;
            }

            var enqueueClient = new BlobContainerClient(_cs, GetContainerEnqueue(queueName));
            if (await enqueueClient.ExistsAsync() == false)
            {
                Status = DequeueStatus.ContainerMissing;
                yield break;
            }

            var dequeueClient = new BlobContainerClient(_cs, GetContainerDequeue(queueName));
            _ = await dequeueClient.CreateIfNotExistsAsync();

            var count = maxTickets - await ContainerCount(dequeueClient);
            if (count < 1)
            {
                Status = DequeueStatus.MaxTicketsAtStart;
                yield break;
            }

            while (true)
            {
                Status = DequeueStatus.None;
                var blobs = await enqueueClient.ListBlobNamesAsync();

                if (blobs.Count < 1)
                {
                    Status = DequeueStatus.Finished;
                    yield break;
                }

                foreach (var b in blobs)
                {
                    if (count > 0)
                    {
                        // read blob
                        var blob = enqueueClient.GetBlobClient(b);
                        var item = await blob.DownloadAsync<MessageItem>();

                        // when empty continue
                        if (item == null)
                            continue;

                        // we decrement after item check!
                        count--;

                        // delete blob
                        _ = await blob.DeleteIfExistsAsync();

                        item.Id = DateTime.Now.Ticks.ToString();
                        var newBlob = dequeueClient.GetBlobClient($"{item.Id}.json");
                        _ = await newBlob.UploadStringAsync(JsonConvert.SerializeObject(item));

                        string asciiKey = item.MessageKey;
                        if (!string.IsNullOrWhiteSpace(asciiKey))
                        {
                            asciiKey = Encoding.ASCII.GetString(
                                Encoding.Convert(
                                    Encoding.UTF8,
                                    Encoding.GetEncoding(
                                        Encoding.ASCII.EncodingName,
                                        new EncoderReplacementFallback(string.Empty),
                                        new DecoderExceptionFallback()
                                        ),
                                    Encoding.UTF8.GetBytes(asciiKey)
                                )
                            );
                        }
                        else
                            asciiKey = string.Empty;

                        var hexHash = string.Empty;
                        if (!string.IsNullOrWhiteSpace(item.Payload))
                        {
                            byte[]? hash;
                            using (MD5 md5 = MD5.Create())
                            {
                                md5.Initialize();
                                md5.ComputeHash(Encoding.UTF8.GetBytes(item.Payload));
                                hash = md5.Hash;
                            }

                            hexHash = hash?.ToHexString() ?? string.Empty;
                        }

                        _ = await newBlob.SetMetadataAsync(new Dictionary<string, string>() { { "hash", hexHash }, { "key", asciiKey } });

                        Status = DequeueStatus.OK;

                        yield return item;
                    }
                    else
                    {
                        // ContainerCount has bad performance, since we locked the access we can assume that there is np
                        // other process that is dequeuing -> so we  counting our tickets to 0 before we refresh the real count
                        count = maxTickets - await ContainerCount(dequeueClient);
                        if (count == 0)
                        {
                            Status = DequeueStatus.MaxTicketsAtEnd;
                            yield break;
                        }
                    }

                    if (await lease.RenewAsync(false) == false) // heartbeat
                    {
                        Status = DequeueStatus.RenewLeaseFailed;
                        yield break;
                    }
                }
            }
        }

        public async IAsyncEnumerable<MessageItem> DequeueAsync(string queueName, int maxTickets, int timeoutInSeconds)
        {
            var freedup = 0;

            using (var lease = RequestLeaseAsync(queueName).Result) // we have to sync wait
            {
                if (lease == null)
                    yield break;

                freedup = await FreeTimeoutMessagesAsync(lease, queueName, timeoutInSeconds);

                await foreach (var r in DequeueAsync(lease, queueName, maxTickets))
                {
                    yield return r;
                }
            }
        }

        public bool ValidateMetadata(MessageItem message, bool fault = true)
        {
            bool errors = false;

            if (message == null)
            {
                if (fault)
                    throw new ArgumentNullException("message");
                else
                    errors = true;
            }

            if (string.IsNullOrWhiteSpace(message?.Id))
            {
                if (fault)
                    throw new ArgumentNullException("message property Id is empty");
                else
                    errors = true;
            }

            if (message?.MessageManager != null && message?.MessageManager != typeof(DefaultManager))
            {
                if (fault)
                    throw new InvalidOperationException($"{message?.Id ?? EAI.Texts.Properties.NULL} is not targeting default manager ({message?.MessageManager.ToString() ?? EAI.Texts.Properties.NULL})");
                else
                    errors = true;
            }

            return errors;
        }

        private async Task<bool> FreeTimeoutLeaseAsync(string queueName)
        {
            try
            {
                var client = new BlobContainerClient(_cs, EAI.Texts.DefaultStorage.BlobLockContainer);
                var blob = client.GetBlobClient(EAI.Texts.DefaultStorage.FileSemaphore(queueName));

                if (await blob.ExistsAsync()== false)
                    return true;

                var props = await blob.GetPropertiesAsync();
                if (props.Value.LeaseState != LeaseState.Leased)
                    return true;

                var lastRenew = props.Value.LastModified;
                var adjusted = DateTimeOffset.UtcNow.Add(lastRenew.Offset);

                if (adjusted < lastRenew.AddSeconds(_mqc.LeaseTimeout))
                    return false;

                var leaseClient = blob.GetBlobLeaseClient();
                _ = await leaseClient.BreakAsync();
                _log.LogWarning($"[MQ.{queueName}] DefaultManager.FreeTimeoutLeaseAsync broke old lease {adjusted.ToString("dd. HH:mm:ss")} {lastRenew.ToString("dd. HH:mm:ss")}");

                return true;
            }
            catch (Exception ex)
            {
                _log.LogWarning($"[MQ.{queueName}] DefaultManager.FreeTimeoutLeaseAsync ex: {ex.Message} {ex.InnerException?.Message}");
            }

            return false;
        }

        /// <summary>
        /// this task is intended to be fire and forget
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        private async Task updateQueueList(string queueName)
        {
            try
            {
                var container = new BlobContainerClient(_cs, EAI.Texts.DefaultStorage.QueueListContainer);
                _ = await container.CreateIfNotExistsAsync();
                var blob = container.GetBlobClient(queueName);

                if (await blob.ExistsAsync() == false)
                    await blob.UploadStringAsync(string.Empty);
            }
            catch (Exception ex)
            {
                _log.LogWarning($"[MQ.{queueName}] DefaultManager.updateQueueList ex: {ex.Message} {ex.InnerException?.Message}");
            }
        }

        public async IAsyncEnumerable<string> GetQueues()
        {
            var container = new BlobContainerClient(_cs, EAI.Texts.DefaultStorage.QueueListContainer);
            if (await container.ExistsAsync() == false)
                yield break;

            foreach (var queue in await container.ListBlobNamesAsync())
                yield return queue;
        }

        public async Task<MessageItem> EnqueueAsync(MessageItem message)
        {
            ValidateMetadata(message);

            var container = new BlobContainerClient(_cs, GetContainerEnqueue(message.GetQueue));
            await container.CreateIfNotExistsAsync();
            _ = await container.GetBlobClient($"{message.Id}.json").UploadStringAsync(JsonConvert.SerializeObject(message));

            _ = updateQueueList(message.GetQueue);

            return message;
        }

        public async Task<int> FreeTimeoutMessagesAsync(IQueueLease lease, string queueName, int timeoutInSeconds)
        {
            var count = 0;

            if (lease == null)
                return count;

            var dequeueClient = new BlobContainerClient(_cs, GetContainerDequeue(queueName));
            if (await dequeueClient.ExistsAsync() == false)
                return count;

            var blobs = await dequeueClient.ListBlobNamesAsync();
            var nowTicks = DateTime.Now.Ticks;

            foreach (var b in blobs)
            {
                try
                {
                    var ticks = Int64.Parse(b.Replace(".json", ""));
                    if (ticks + TimeSpan.FromSeconds(timeoutInSeconds).Ticks <= nowTicks)
                    {
                        // we just delete it and do not archive it because...
                        var item = await releaseAsync(dequeueClient.GetBlobClient(b), true, false);
                        if (item != null)
                        {
                            item.Status = ProcessingStatus.ProcessingTimeout;
                            // ...we enqueue it again
                            await EnqueueAsync(item);
                            count++;
                            _log.LogWarning($"[MQ.{queueName}] DefaultManager.FreeTimeoutMessagesAsync removed {b} key: {item.MessageKey} from dequeue");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError($"[MQ.{queueName}] DefaultManager.FreeTimeoutMessagesAsync ex: {ex.Message} {ex.InnerException?.Message}");
                }
            }

            return count;
        }

        private async Task<bool> storeInArchive(MessageItem message)
        {
            var container = new BlobContainerClient(_cs, GetContainerArchive(message.GetQueue));
            _ = await container.CreateIfNotExistsAsync();

            var file = $"{message.ReceivedOn.DateTime.ToString("yyyyMMdd_HHmmss")}_{message.MessageType}_{message.MessageKey}{{0}}.json";
            var blob = container.GetBlobClient(string.Format(file, string.Empty));
            if (await blob.ExistsAsync())
                blob = container.GetBlobClient(string.Format(file, $".{Path.GetRandomFileName().Replace(".", "")}"));

            return await blob.UploadStringAsync(JsonConvert.SerializeObject(message));
        }

        private async Task<MessageItem?> releaseAsync(BlobClient blobClient, bool fault, bool archive)
        {
            MessageItem? result = await blobClient.DownloadAsync<MessageItem>();
            await blobClient.DeleteIfExistsAsync();

            if (result != null)
            {
                result.Status = fault ? ProcessingStatus.FinishedWithErrors : ProcessingStatus.Finished;

                var toArchive = _mqc.Archive.Default;
                if (_mqc.Archive.Custom?.Count > 0 
                    && !string.IsNullOrWhiteSpace(result.Endpoint)
                    && _mqc.Archive.Custom.ContainsKey(result.Endpoint))
                    toArchive = _mqc.Archive.Custom[result.Endpoint];

                if (toArchive)
                    _ = await storeInArchive(result);
            }

            return result;
        }

        public async Task<MessageItem?> ReleaseAsync(MessageItem message, bool fault)
        {
            try
            {
                ValidateMetadata(message);

                var container = new BlobContainerClient(_cs, GetContainerDequeue(message.GetQueue));
                var blobClient = container.GetBlobClient($"{message.Id}.json");

                var toArchive = _mqc.Archive.Default;
                if (_mqc.Archive.Custom?.Count > 0 
                    && !string.IsNullOrWhiteSpace(message.Endpoint)
                    && _mqc.Archive.Custom.ContainsKey(message.Endpoint))
                    toArchive = _mqc.Archive.Custom[message.Endpoint];

                var item = await releaseAsync(blobClient, fault, toArchive);

                if (item == null)
                {
                    message.Status = ProcessingStatus.FinishedButNotInQueue;

                    if (toArchive)
                        _ = await storeInArchive(message);

                    return message;
                }

                return item;
            }
            catch (Exception ex)
            {
                _log.LogError($"[MQ.{message?.GetQueue}] DefaultManager.ReleaseAsync ex: {ex.Message} {ex.InnerException?.Message}");
            }

            return message;
        }

        /// <summary>
        /// creates an lock by placing a blob acting as a semaphore into the management container
        /// aquires a lease on the container
        /// when already a blob is existing, checks if the blob has timed out
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public async Task<IQueueLease?> RequestLeaseAsync(string queueName)
        {
            var mo = new MessageObject();
            mo.SetMessage(MessageObjectId.ReleaseAsyncStop, false);

            queueName = queueName.ToLower();
            var name = EAI.Texts.DefaultStorage.FileSemaphore(queueName);

            _log.LogInformation($"[MQ.{queueName}] DefaultManager.RequestLeaseAsync I request semaphore");

            var ok = await _semaphoreSlim.WaitAsync(2000).ConfigureAwait(false);
            if (!ok)
            {
                _log.LogInformation($"[MQ.{queueName}] DefaultManager.RequestLeaseAsync semaphore timeout");
                mo.SetMessage(MessageObjectId.ReleaseAsyncStop, true);

                return null;
                //_log.LogInformation($"[MQ.{queueName}] DefaultManager.RequestLeaseAsync I response semaphore");
            }

            try
            {
                var retry = true;
                bool result = false;
                var client = new BlobContainerClient(_cs, EAI.Texts.DefaultStorage.BlobLockContainer);
                var blob = client.GetBlobClient(name);

                if (!blob.Exists())
                    await blob.UploadStringAsync(string.Empty);

                while (retry)
                {
                    try
                    {
                        var leaseClient = blob.GetBlobLeaseClient();
                        var lease = await leaseClient.AcquireAsync(TimeSpan.FromSeconds(-1));
                        LeaseId = lease.Value.LeaseId;
                        result = true;
                        retry = false;
                    }
                    catch (RequestFailedException)
                    {
                        result = false;

                        _log.LogInformation($"[MQ.{queueName}] DefaultManager.RequestLeaseAsync I FreeTimeoutLeaseAsync");

                        if (await FreeTimeoutLeaseAsync(queueName) == false)
                            retry = false;
                    }
                    catch (Exception ex)
                    {
                        _log.LogWarning($"[MQ.{queueName}] DefaultManager.RequestLeaseAsync I ex: {ex.Message} {ex.InnerException?.Message}");
                        result = false;
                        retry = false;
                    }
                }

                if (!result)
                    return null;
            }
            catch (Exception ex)
            {
                _log.LogWarning($"[MQ.{queueName}]] DefaultManager.RequestLeaseAsync II ex: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
            finally
            {
                _log.LogInformation($"[MQ.{queueName}] DefaultManager.RequestLeaseAsync I release semaphore");
                _semaphoreSlim.Release();
            }

            return new DefaultQueueLease(_log, ConnectionString, EAI.Texts.DefaultStorage.BlobLockContainer, name, LeaseId);
        }

        public IQueueTicket GetTicket(MessageItem message)
        {
            return new ContainerTicket()
            {
                Container = GetContainerDequeue(message.GetQueue),
                Blob = $"{message.Id}.json"
            };
        }
    }
}
