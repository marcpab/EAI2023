using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using EAI.MessageQueue.Storage.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace EAI.MessageQueue.Storage.Manager
{
    public class DefaultQueueLease : IQueueLease
    {
        private SecureString ConnectionString { get; set; }
        private string Container { get; set; }
        private string Blob { get; set; }
        private bool Disposed { get; set; }

        private ILogger Log { get; set; }

        public string LeaseId { get; private set; }

        public string GetContainer() => Container.Replace("roedl-queue-", "").Replace("dequeue", "");

        public DefaultQueueLease(ILogger log, SecureString cs, string container, string blob, string leaseId)
        {
            ConnectionString = cs;
            Container = container;
            Blob = blob;
            Disposed = false;
            LeaseId = leaseId;
            Log = log;
        }

        public void Dispose()
        {
            if (!Disposed)
                while (ReleaseAsync().Result == false)
                    Task.Delay(1000).Wait();

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// renews the lease (and verify the lease)
        /// by reupload the blob to update the last modify date
        /// its actually a heartbeat mechanism to approve the dequeue is still alive
        /// 
        /// it will return true when renew was successful
        /// it will return false when the blob was either hijacked or the leaseid is invalid
        /// ATTENTION: it is possible that 2 leases are aquired at the same time but only one is valid
        /// so please check with this method first if your lease is really valid!!
        /// 
        /// -> in this case false cancellation of the requesting operation is highly recommended
        /// </summary>
        /// <returns></returns>
        public async Task<bool> RenewAsync(bool isValidCheck)
        {
            try
            {
                var client = new BlobContainerClient(new NetworkCredential(string.Empty, ConnectionString).Password, Container);
                var blob = client.GetBlobClient(Blob);

                return await blob.UploadStringAsync(string.Empty, LeaseId, true);
            }
            catch (Exception ex)
            {
                if (!isValidCheck) // when heartbeat and exception than log!
                    Log.LogError($"[MQ.{GetContainer()}] DefaultQueueLease.RenewAsync ({LeaseId}) ex:{ex.Message} {ex.InnerException?.Message} stack: {ex.StackTrace}");

                LeaseId = string.Empty;

                return false;
            }
        }

        /// <summary>
        /// releases the lock file
        /// by deleting blob from lease container
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ReleaseAsync()
        {
            if (Disposed || string.IsNullOrWhiteSpace(LeaseId))
            {
                Disposed = true;
                return true;
            }

            var client = new BlobContainerClient(new NetworkCredential(string.Empty, ConnectionString).Password, Container);
            var blob = client.GetBlobClient(Blob);

            if (await blob.ExistsAsync() == false)
                Disposed = true;
            else
            {
                try
                {
                    Log.LogInformation($"[MQ.{GetContainer()}] DefaultQueueLease.ReleaseAsync ({LeaseId})");

                    var leaseClient = blob.GetBlobLeaseClient(LeaseId);
                    await leaseClient.ReleaseAsync();

                    //Log.LogInformation($"[MQ.{GetContainer()}] DefaultQueueLease.ReleaseAsync ({LeaseId}) ok");
                    LeaseId = string.Empty;
                }
                catch (Exception ex)
                {
                    Log.LogError($"[MQ.{GetContainer()}] DefaultQueueLease.ReleaseAsync ({LeaseId}) ex: {ex.Message} {ex.InnerException?.Message} stack {ex.StackTrace}");
                }

                Disposed = true;
            }

            return Disposed;
        }
    }
}
