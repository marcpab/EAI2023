using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAI.General.Storage
{
    public interface IStorageQueue
    {
        Task EnqueueAsync(string messageContent);
        IAsyncEnumerable<StorageQueueMessage> DequeueAsync(int maxMessages, DequeueType dequeueType = DequeueType.ManualComplete);
        Task CompletedAsync(StorageQueueMessage message);
    }
}