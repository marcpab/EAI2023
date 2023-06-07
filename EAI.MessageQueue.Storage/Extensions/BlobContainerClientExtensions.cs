using Azure.Storage.Blobs;
using System.Collections.Concurrent;

namespace EAI.MessageQueue.Storage.Extensions
{
    public static class BlobContainerClientExtensions
    {
        public async static Task<ConcurrentBag<string>> ListBlobNamesAsync(this BlobContainerClient client)
        {
            ConcurrentBag<string> items = new ConcurrentBag<string>();

            try
            {
                await foreach (var item in client.GetBlobsAsync())
                {
                    if (!item.Deleted)
                        items.Add(item.Name);
                }
            }
            catch (Exception)
            {
                // when we fail to obtain the list of blobs, we return an empty list
            }

            return items;
        }
    }
}
