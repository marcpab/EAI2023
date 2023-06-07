using Azure.Storage.Blobs;
using EAI.MessageQueue.Storage.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAI.MessageQueue.Storage.Ticket
{
    public class ContainerTicket : IQueueTicket
    {
        public Type TicketType { get => typeof(ContainerTicket); }
        public string Container { get; set; } = string.Empty;
        public string Blob { get; set; } = string.Empty;

        public async Task<MessageItem?> Get(string cs)
        {
            var dequeueClient = new BlobContainerClient(cs, Container);
            if (await dequeueClient.ExistsAsync() == false)
                throw new InvalidOperationException($"critical error: dequeue container {Container} missing, cannot get message!");

            var blobClient = dequeueClient.GetBlobClient(Blob);
            if (await blobClient.ExistsAsync() == false)
                throw new InvalidOperationException($"critical error: message {Blob} is missing in dequeue container {Container}");

            return await blobClient.DownloadAsync<MessageItem>();
        }
    }
}
