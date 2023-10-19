using EAI.MessageQueue.SQL.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAI.MessageQueue.SQL
{
    public interface IQueueMessageManager
    {
        IAsyncEnumerable<QueueMessage> DequeueMessages(string stage, string endpintName);
        Task<long> EnqueueMessage(QueueMessage queueMessage);
    }
}