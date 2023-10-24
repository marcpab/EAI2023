using EAI.MessageQueue.SQL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAI.MessageQueue.SQL
{
    public interface IQueueMessageManager
    {
        IAsyncEnumerable<QueueMessage> DequeueMessagesAsync(string stage, string endpintName);
        Task<long> EnqueueMessageAsync(QueueMessage queueMessage);
        Task FailedAsync(long messageId);
        Task SuccessAsync(long messageId);
    }
}